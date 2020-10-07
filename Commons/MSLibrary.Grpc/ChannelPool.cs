using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Core.Interceptors;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Collections;
using MSLibrary.LanguageTranslate;
using System.IO.Pipelines;
using MSLibrary.Grpc.Interceptors;
using System.Linq;

namespace MSLibrary.Grpc
{
    public class ChannelPool : EntityBase<IChannelPoolIMP>
    {
        private static IFactory<IChannelPoolIMP>? _channelPoolIMPFactory;

        public static IFactory<IChannelPoolIMP> ChannelPoolIMPFactory
        {
            set
            {
                _channelPoolIMPFactory = value;
            }
        }

        public override IFactory<IChannelPoolIMP>? GetIMPFactory()
        {
            return _channelPoolIMPFactory;
        }

        /// <summary>
        /// id
        /// </summary>
        public Guid ID
        {
            get
            {
                return GetAttribute<Guid>(nameof(ID));
            }
            set
            {
                SetAttribute<Guid>(nameof(ID), value);
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {
                return GetAttribute<string>(nameof(Name));
            }
            set
            {
                SetAttribute<string>(nameof(Name), value);
            }
        }

        /// <summary>
        /// 服务基地址
        /// </summary>
        public string Address
        {
            get
            {
                return GetAttribute<string>(nameof(Address));
            }
            set
            {
                SetAttribute<string>(nameof(Address), value);
            }
        }

        /// <summary>
        /// 需要使用的拦截器名称集合
        /// </summary>
        public string[] InterceptNames
        {
            get
            {
                return GetAttribute<string[]>(nameof(InterceptNames));
            }
            set
            {
                SetAttribute<string[]>(nameof(InterceptNames), value);
            }
        }

        /// <summary>
        /// 客户端使用的证书生成服务名称
        /// </summary>
        public string? CertificateGenerateName
        {
            get
            {
                return GetAttribute<string?>(nameof(CertificateGenerateName));
            }
            set
            {
                SetAttribute<string?>(nameof(CertificateGenerateName), value);
            }
        }


        /// <summary>
        /// 配置
        /// </summary>
        public string Configuration
        {
            get
            {
                return GetAttribute<string>(nameof(Configuration));
            }
            set
            {
                SetAttribute<string>(nameof(Configuration), value);
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>(nameof(CreateTime));
            }
            set
            {
                SetAttribute<DateTime>(nameof(CreateTime), value);
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>(nameof(ModifyTime));
            }
            set
            {
                SetAttribute<DateTime>(nameof(ModifyTime), value);
            }
        }

        public async Task<CallInvoker> GetInvoker()
        {
            return await _imp.GetInvoker(this);
        }
       

    }

    public interface IChannelPoolIMP
    {
        Task<CallInvoker> GetInvoker(ChannelPool pool);
    }

    /// <summary>
    /// 证书生成服务
    /// </summary>
    public interface ICertificateGenerateService
    {
        Task<X509Certificate2> Generate();
    }

    [Injection(InterfaceType = typeof(IChannelPoolIMP), Scope = InjectionScope.Transient)]
    public class ChannelPoolIMP : IChannelPoolIMP
    {
        public static IDictionary<string, IFactory<ICertificateGenerateService>> CertificateGenerateServiceFactories { get; } = new Dictionary<string, IFactory<ICertificateGenerateService>>();
        public static IDictionary<string, InterceptorDescription> InterceptorDescriptions { get; } = new Dictionary<string, InterceptorDescription>();

        private static ConcurrentDictionary<Guid, channelContainer> _pools = new ConcurrentDictionary<Guid, channelContainer>();

        public async Task<CallInvoker> GetInvoker(ChannelPool pool)
        {
            bool found = true;
            if (!_pools.TryGetValue(pool.ID, out channelContainer? channelPoolContainer))
            {
                lock (_pools)
                {
                    if (!_pools.TryGetValue(pool.ID, out channelPoolContainer))
                    {
                        channelPoolContainer = new channelContainer()
                        {
                            Address = pool.Address,
                            CertificateGenerateName = pool.CertificateGenerateName,
                            InterceptNames = pool.InterceptNames,
                            Configuration = pool.Configuration,
                            ChannelPool = new SharePool<channelPoolItem>(pool.Name,
                            () =>
                            {
                                throw new NotImplementedException();
                            },
                            (item) =>
                            {
                                return true;
                            }
                            ,
                            (item) =>
                            {

                            }
                            ,
                            async () =>
                            {


                                GrpcChannel channel=null!;
                                if (pool.CertificateGenerateName != null)
                                {
                                    var certificateGenerateService = getCertificateGenerateService(pool.CertificateGenerateName);
                                    var certificate = await certificateGenerateService.Generate();
                                    var handler = new HttpClientHandler();
                                    handler.ClientCertificates.Add(certificate);
                                    
                                     channel = GrpcChannel.ForAddress(pool.Address, new GrpcChannelOptions
                                    {
                                        HttpHandler = handler,                            
                                    });
                                }
                                else
                                {
                                    channel = GrpcChannel.ForAddress(pool.Address);
                                }

                                CallInvoker callInvoker=null!;
                                if (pool.InterceptNames.Count()==0)
                                {
                                    callInvoker=channel.Intercept(new ClientEmptyInterceptor());
                                }
                                else
                                {
                                    List<Interceptor> interceptorList = new List<Interceptor>();
                                    foreach (var item in pool.InterceptNames)
                                    {
                                        var interceptorDescription = getInterceptorDescription(item);
                                        var interceptor = DIContainerContainer.Get(interceptorDescription.InterceptorType, interceptorDescription.Arguments, interceptorDescription.ArgumentTypes);
                                        if (interceptor is IExtensionInfoinject)
                                        {
                                            await ((IExtensionInfoinject)interceptor).SetData(pool.Configuration);
                                        }
                                        interceptorList.Add((Interceptor)interceptor);
                                    }
                                    callInvoker = channel.Intercept(interceptorList.ToArray());
                                }

                             


                                return new channelPoolItem() { Channel=channel, Invoker= callInvoker };
                            }
                            ,
                            async (item) =>
                            {
                                return await Task.FromResult(true);
                            },
                            async (item) =>
                            {
                              
                                await item.Channel.ShutdownAsync();
                            },
                            100
                            )
                        };
                        _pools[pool.ID] = channelPoolContainer;
                        found = false;
                    }
                }
            }
            if (found)
            {
                if (pool.Address != channelPoolContainer.Address || pool.CertificateGenerateName != channelPoolContainer.CertificateGenerateName || pool.Configuration != channelPoolContainer.CertificateGenerateName
                    || pool.InterceptNames.ToDisplayString((item) => item, () => "") != channelPoolContainer.InterceptNames.ToDisplayString((item) => item, () => "")
                    )
                {
                    await channelPoolContainer.ChannelPool.ClearAsync();
                }
            }

            return (await channelPoolContainer.ChannelPool.GetAsync()).Invoker;
        }

        private InterceptorDescription getInterceptorDescription(string name)
        {
            if (!InterceptorDescriptions.TryGetValue(name,out InterceptorDescription? description))
            {
                var fragment = new TextFragment()
                {
                    Code = GrpcTextCodes.NotFoundGrpcClientInterceptorDescriptionByName,
                    DefaultFormatting = "找不到名称为{0}的Grpc客户端拦截器描述，发生位置为{1}",
                    ReplaceParameters = new List<object>() { name, $"{this.GetType().FullName}.InterceptorDescriptions" }
                };

                throw new UtilityException((int)GrpcErrorCodes.NotFoundGrpcClientInterceptorDescriptionByName, fragment);
            }

            return description;
        }

        private ICertificateGenerateService getCertificateGenerateService(string name)
        {
            if (!CertificateGenerateServiceFactories.TryGetValue(name, out IFactory<ICertificateGenerateService>? serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = GrpcTextCodes.NotFoundCertificateGenerateServiceByName,
                    DefaultFormatting = "找不到名称为{0}的Grpc客户端拦截器，发生位置为{1}",
                    ReplaceParameters = new List<object>() { name, $"{this.GetType().FullName}.CertificateGenerateServiceFactories" }
                };

                throw new UtilityException((int)GrpcErrorCodes.NotFoundCertificateGenerateServiceByName, fragment);
            }
           
            return serviceFactory.Create(); ;
        }

        private class channelContainer
        {
            public string Address { get; set; } = null!;
            public string[] InterceptNames { get; set; } = null!;
            public string? CertificateGenerateName { get; set; } = null!;
            public string Configuration { get; set; } = null!;
            public SharePool<channelPoolItem> ChannelPool { get; set; } = null!;

        }

        private class channelPoolItem
        {
            public GrpcChannel Channel { get; set; } = null!;
            public CallInvoker Invoker { get; set; } = null!;
                 
        }

    }
}
