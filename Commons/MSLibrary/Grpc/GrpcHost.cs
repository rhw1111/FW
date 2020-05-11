using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Grpc.Core;
using Grpc.Core.Interceptors;
using MSLibrary.Grpc.Filters;
using MSLibrary.Grpc.Interceptors;
using MSLibrary.DI;

namespace MSLibrary.Grpc
{
    public class GrpcHost
    {
        private GrpcHostConfiguration _configuration;
        private Server _grpcServer;

        private IGrpcServiceFindService _grpcServiceFindService;
        private IGrpcServiceFindFilterService _grpcServiceFindFilterService;


        private static IFactory<IGrpcServiceFindService> _grpcServiceFindServiceFactory;
        private static IFactory<IGrpcServiceFindFilterService> _grpcServiceFindFilterServiceFactory;

        public static IFactory<IGrpcServiceFindService> GrpcServiceFindServiceFactory
        {
            set
            {
                _grpcServiceFindServiceFactory = value;
            }
        }

        public static IFactory<IGrpcServiceFindFilterService> GrpcServiceFindFilterServiceFactory
        {
            set
            {
                _grpcServiceFindFilterServiceFactory = value;
            }
        }




        public GrpcHost()
        {
            _configuration = new GrpcHostConfiguration();
            _grpcServer = new Server();
        }
        public GrpcHost GlobalFiltersConfig(Action<IList<FilterBase>> action)
        {
            action(_configuration.GlobalFilters);
            return this;
        }

        public GrpcHost PortsConfig(Action<IList<GrpcPortDescription>> action)
        {
            action(_configuration.Ports);
            return this;
        }

        public GrpcHost GetGrpcServiceFindService(Func<IGrpcServiceFindService> serviceGenerator)
        {
            _grpcServiceFindService = serviceGenerator();
            return this;
        }

        public GrpcHost GetGrpcServiceFindFilterService(Func<IGrpcServiceFindFilterService> serviceGenerator)
        {
            _grpcServiceFindFilterService = serviceGenerator();
            return this;
        }

        public async Task Start()
        {
            //检查grpc服务过滤器查找服务是否已经有值，如果没有，使用工厂创建
            if (_grpcServiceFindFilterService==null)
            {
                _grpcServiceFindFilterService = _grpcServiceFindFilterServiceFactory.Create();
            }
            //检查grpc服务查找服务是否已经有值，如果没有，使用工厂创建
            if (_grpcServiceFindService==null)
            {
                _grpcServiceFindService = _grpcServiceFindServiceFactory.Create();
            }
            //获取Grpc服务信息
            var grpcServiceInfos = await _grpcServiceFindService.Execute();

            List<GrpcServiceDescription> grpcServiceDescriptionList = new List<GrpcServiceDescription>();
            GrpcServiceDescription newGrpcServiceDescription;

            //组装Grpc服务描述，为GrpcHostConfiguartion赋值
            //以及为Gprc服务的服务定义赋值
            foreach (var infoItem in grpcServiceInfos)
            {
                newGrpcServiceDescription = new GrpcServiceDescription() { NamespaceType = infoItem.NameSpaceType, ServiceType = infoItem.ServiceType };
                grpcServiceDescriptionList.Add(newGrpcServiceDescription);


                //获取每个Grpc服务的过滤器
                var serviceFilters=await _grpcServiceFindFilterService.Execute(newGrpcServiceDescription.ServiceType);
                newGrpcServiceDescription.Filters = serviceFilters;
                //获取合并后的链式拦截器
                var chainInterceptor=await getChainInterceptor(_configuration.GlobalFilters, serviceFilters);


                //组装Grpc服务器中的服务定义
                var gprcService=DIContainerContainer.Get(newGrpcServiceDescription.ServiceType);
                ServerServiceDefinition newServerServiceDefinition = ((ServerServiceDefinition)newGrpcServiceDescription.NamespaceType.GetMethod("BindService").Invoke(null, new object[] { gprcService })).Intercept(chainInterceptor);
                _grpcServer.Services.Add(newServerServiceDefinition);
            }

            //为Gprc服务的端口定义赋值
            foreach(var portItem in _configuration.Ports)
            {
                _grpcServer.Ports.Add(new ServerPort(portItem.Host, portItem.Port, portItem.Credential));
            }

            _grpcServer.Start();
        }

        public async Task Shutdown()
        {
            await _grpcServer.ShutdownAsync();
        }

        private async Task<ChainInterceptor> getChainInterceptor(IList<FilterBase> globalFilters,IList<FilterBase> serviceFilters)
        {
            var newFilters = new List<FilterBase>();
            newFilters.AddRange(globalFilters);
            newFilters.AddRange(serviceFilters);
            while(true)
            {
                var firstOverrideFilter = (from item in newFilters
                                           where item is OverrideFilter
                                           select item).FirstOrDefault();
                if (firstOverrideFilter==null)
                {
                    break;
                }

                var index = newFilters.IndexOf(firstOverrideFilter);
                var overrideFilter = firstOverrideFilter as OverrideFilter;
                if (overrideFilter.OverrideType==null && string.IsNullOrEmpty(overrideFilter.OverrideTypeName))
                {
                    newFilters.RemoveRange(0, index);
                }
                else if (overrideFilter.OverrideType != null && !string.IsNullOrEmpty(overrideFilter.OverrideTypeName))
                {
                    var filtersRange=newFilters.GetRange(0, index);
                    var removeItems = (from item in filtersRange
                                       where item.Type == overrideFilter.OverrideTypeName && item.GetType() == overrideFilter.OverrideType
                                       select item).ToList();

                    foreach(var removeItem in removeItems)
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
            foreach(var item in newFilters)
            {
                newInterceptorList.Add(item.Interceptor);
            }

            ChainInterceptor interceptor = new ChainInterceptor(newInterceptorList);

            return await Task.FromResult(interceptor);
        }
        

    }
}
