using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Grpc.Core;
using Grpc.Core.Interceptors;
using MSLibrary.Collections;
using MSLibrary.Grpc.Filters;
using MSLibrary.Grpc.Interceptors;

namespace MSLibrary.Grpc
{
    public class GrpcChannelPoolForDefault : IGrpcChannelPool
    {
        private GrpcChannelGlobalConfiguration _configuration = new GrpcChannelGlobalConfiguration();
        private Dictionary<string, GrpcChannelItem> _items = new Dictionary<string, GrpcChannelItem>();

        public async Task Add(string channelName, GrpcChannelConfiguration configuration)
        {
            if (!_items.TryGetValue(channelName,out GrpcChannelItem item))
            {
                lock (_items)
                {
                    if (!_items.TryGetValue(channelName, out item))
                    {
                        item = new GrpcChannelItem()
                        {
                             Name=channelName
                        };

                        item.Versions.Insert(0,new GrpcChannelVersionItem()
                        {
                             Configuration=configuration,
                             Pool=new SharePool<Channel>("GrpcChannelVersionItem",
                             null,
                             null,
                             null,
                             async ()=>
                             {
                                 var newChannel = new Channel(configuration.Target, configuration.ChannelCredentials);
                                 newChannel.Intercept(await getChainInterceptor(_configuration.GlobalFilters, configuration.Filters));
                                 await newChannel.ConnectAsync();
                                 return newChannel;
                             },
                             async (channel)=>
                             {
                                 if (channel.State!= ChannelState.Shutdown && channel.State!=ChannelState.TransientFailure)
                                 {
                                     return await Task.FromResult(false);
                                 }

                                 return await Task.FromResult(true);
                             },
                             async (channel)=>
                             {
                                 if (channel.State != ChannelState.Shutdown)
                                 {
                                     try
                                     {
                                         await channel.ShutdownAsync();
                                     }
                                     catch
                                     {

                                     }
                                 }
                             },
                             configuration.Length                   
                             )
                        });

                        _items.Add(channelName, item);
                    }
                }
            }

            await Task.FromResult(0);
        }

        public async Task AddNewVersion(string channelName, GrpcChannelConfiguration configuration)
        {
            if (_items.TryGetValue(channelName, out GrpcChannelItem item))
            {              
                lock(item)
                {
                    item.Versions.Insert(0, new GrpcChannelVersionItem()
                    {
                        Configuration = configuration,
                        Pool = new SharePool<Channel>("GrpcChannelVersionItem",
                         null,
                         null,
                         null,
                         async () =>
                         {
                             var newChannel = new Channel(configuration.Target, configuration.ChannelCredentials);
                             newChannel.Intercept(await getChainInterceptor(_configuration.GlobalFilters, configuration.Filters));
                             await newChannel.ConnectAsync();
                             return newChannel;
                         },
                         async (channel) =>
                         {
                             if (channel.State != ChannelState.Shutdown && channel.State != ChannelState.TransientFailure)
                             {
                                 return await Task.FromResult(false);
                             }

                             return await Task.FromResult(true);
                         },
                         async (channel) =>
                         {
                             if (channel.State != ChannelState.Shutdown)
                             {
                                 try
                                 {
                                     await channel.ShutdownAsync();
                                 }
                                 catch
                                 {

                                 }
                             }
                         },
                         configuration.Length
                         )
                    });
                }
            }

            await Task.FromResult(0);
        }

        public async Task<Channel> Get(string channelName)
        {
            if (_items.TryGetValue(channelName, out GrpcChannelItem item))
            {
                return await item.Versions[0].Pool.GetAsync();
            }
            return null;
        }

        public async Task GlobalConfig(Func<GrpcChannelGlobalConfiguration, Task> action)
        {
            await action(_configuration);
        }

        public async Task Remove(string channelName)
        {
            if (_items.TryGetValue(channelName, out GrpcChannelItem item))
            {
                List<SharePool<Channel>> poolList = new List<SharePool<Channel>>();
                lock(_items)
                {
                    if (!_items.ContainsKey(channelName))
                    {
                        foreach (var versionItem in item.Versions)
                        {
                            poolList.Add(versionItem.Pool);
                        }
                        _items.Remove(channelName);
                    }
                }
                foreach(var poolItem in poolList)
                {
                    await poolItem.ClearAsync();
                }
            }
        }
        public async Task RemoveOldVersion(string channelName)
        {
            if (_items.TryGetValue(channelName, out GrpcChannelItem item))
            {
                List<SharePool<Channel>> poolList = new List<SharePool<Channel>>();
                lock (item)
                {
                    if (item.Versions.Count>1)
                    {
                        var removeVersionList=item.Versions.GetRange(1, item.Versions.Count - 1);

                        foreach(var removeVersionItem in removeVersionList)
                        {
                            poolList.Add(removeVersionItem.Pool);
                        }

                        item.Versions.RemoveRange(1, item.Versions.Count - 1);
                    }
                }

                foreach (var poolItem in poolList)
                {
                    await poolItem.ClearAsync();
                }
            }
        }

        public async Task Clear()
        {
            foreach(var item in _items)
            {
                foreach(var versionItem in item.Value.Versions)
                {
                    try
                    {
                        await versionItem.Pool.ClearAsync();
                    }
                    catch
                    {

                    }
                }
            }
            _items.Clear();
        }

        private async Task<ChainInterceptor> getChainInterceptor(IList<FilterBase> globalFilters, IList<FilterBase> clientFilters)
        {
            var newFilters = new List<FilterBase>();
            newFilters.AddRange(globalFilters);
            newFilters.AddRange(clientFilters);
            while (true)
            {
                var firstOverrideFilter = (from item in newFilters
                                           where item is OverrideFilter
                                           select item).FirstOrDefault();
                if (firstOverrideFilter == null)
                {
                    break;
                }

                var index = newFilters.IndexOf(firstOverrideFilter);
                var overrideFilter = firstOverrideFilter as OverrideFilter;
                if (overrideFilter.OverrideType == null && string.IsNullOrEmpty(overrideFilter.OverrideTypeName))
                {
                    newFilters.RemoveRange(0, index);
                }
                else if (overrideFilter.OverrideType != null && !string.IsNullOrEmpty(overrideFilter.OverrideTypeName))
                {
                    var filtersRange = newFilters.GetRange(0, index);
                    var removeItems = (from item in filtersRange
                                       where item.Type == overrideFilter.OverrideTypeName && item.GetType() == overrideFilter.OverrideType
                                       select item).ToList();

                    foreach (var removeItem in removeItems)
                    {
                        newFilters.Remove(removeItem);
                    }
                }
                else if (overrideFilter.OverrideType != null && string.IsNullOrEmpty(overrideFilter.OverrideTypeName))
                {
                    var filtersRange = newFilters.GetRange(0, index);
                    var removeItems = (from item in filtersRange
                                       where item.GetType() == overrideFilter.OverrideType
                                       select item).ToList();

                    foreach (var removeItem in removeItems)
                    {
                        newFilters.Remove(removeItem);
                    }
                }
                else if (overrideFilter.OverrideType == null && !string.IsNullOrEmpty(overrideFilter.OverrideTypeName))
                {
                    var filtersRange = newFilters.GetRange(0, index);
                    var removeItems = (from item in filtersRange
                                       where item.Type == overrideFilter.OverrideTypeName
                                       select item).ToList();

                    foreach (var removeItem in removeItems)
                    {
                        newFilters.Remove(removeItem);
                    }
                }

                newFilters.Remove(overrideFilter);
            }


            var newInterceptorList = new List<Interceptor>();
            foreach (var item in newFilters)
            {
                newInterceptorList.Add(item.Interceptor);
            }

            ChainInterceptor interceptor = new ChainInterceptor(newInterceptorList);

            return await Task.FromResult(interceptor);
        }

        private class GrpcChannelItem
        {
            public string Name { get; set; }

            public List<GrpcChannelVersionItem> Versions { get; } = new List<GrpcChannelVersionItem>();
        }

        private class GrpcChannelVersionItem
        {
            public GrpcChannelConfiguration Configuration { get; set; }
           
            public SharePool<Channel> Pool { get; set; }
        }
    }


    public class GrpcChannelPoolForDefaultFactory : SingletonFactory<IGrpcChannelPool>
    {
        protected override IGrpcChannelPool RealCreate()
        {
            return new GrpcChannelPoolForDefault();
        }
    }
}
