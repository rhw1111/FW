using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Grpc
{
    /// <summary>
    /// 服务端凭证生成器
    /// </summary>
    public class ServerCredentialsGenerator : EntityBase<IServerCredentialsGeneratorIMP>
    {
        private static IFactory<IServerCredentialsGeneratorIMP> _serverCredentialsGeneratorIMPFactory;

        public static IFactory<IServerCredentialsGeneratorIMP> ServerCredentialsGeneratorIMPFactory
        {
            set
            {
                _serverCredentialsGeneratorIMPFactory = value;
            }
        }


        public override IFactory<IServerCredentialsGeneratorIMP> GetIMPFactory()
        {
            return _serverCredentialsGeneratorIMPFactory;
        }

        /// <summary>
        /// Id
        /// </summary>
        public Guid ID
        {
            get
            {
                return GetAttribute<Guid>("ID");
            }
            set
            {
                SetAttribute<Guid>("ID", value);
            }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type
        {
            get
            {
                return GetAttribute<string>("Type");
            }
            set
            {
                SetAttribute<string>("Type", value);
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>("CreateTime");
            }
            set
            {
                SetAttribute<DateTime>("CreateTime", value);
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>("ModifyTime");
            }
            set
            {
                SetAttribute<DateTime>("ModifyTime", value);
            }
        }

        public async Task<ServerCredentials> Generate(string configuration)
        {
            return await _imp.Generate(this, configuration);
        }
    }

    public interface IServerCredentialsGeneratorIMP
    {
        /// <summary>
        /// 为指定配置生成服务端凭证
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        Task<ServerCredentials> Generate(ServerCredentialsGenerator generator, string configuration);
    }

    [Injection(InterfaceType = typeof(IServerCredentialsGeneratorIMP), Scope = InjectionScope.Transient)]
    public class ServerCredentialsGeneratorIMP : IServerCredentialsGeneratorIMP
    {
        private IServerCredentialsGeneratorService _serverCredentialsGeneratorService;

        public ServerCredentialsGeneratorIMP(IServerCredentialsGeneratorService serverCredentialsGeneratorService)
        {
            _serverCredentialsGeneratorService = serverCredentialsGeneratorService;
        }

        public async Task<ServerCredentials> Generate(ServerCredentialsGenerator generator, string configuration)
        {
           return await _serverCredentialsGeneratorService.Generate(generator.Type, configuration);
        }
    }


    public interface IServerCredentialsGeneratorService
    {
        Task<ServerCredentials> Generate(string type, string configuration);
    }

    [Injection(InterfaceType = typeof(IServerCredentialsGeneratorService), Scope = InjectionScope.Singleton)]
    public class ServerCredentialsGeneratorMainService:IServerCredentialsGeneratorService
    {
        private static IDictionary<string, IFactory<IServerCredentialsGeneratorService>> _serviceFactories = new Dictionary<string, IFactory<IServerCredentialsGeneratorService>>();

        public static IDictionary<string, IFactory<IServerCredentialsGeneratorService>> ServiceFactories
        {
            get
            {
                return _serviceFactories;
            }
        }
        public async Task<ServerCredentials> Generate(string type, string configuration)
        {
            if (!_serviceFactories.TryGetValue(type,out IFactory<IServerCredentialsGeneratorService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundGrpcServerCredentialsGeneratorServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Grpc服务端凭证生成服务，发生位置：{1}",
                    ReplaceParameters = new List<object>() { type, $"{this.GetType().FullName}.Generate"  }
                };
                throw new UtilityException((int)Errors.NotFoundGrpcServerCredentialsGeneratorServiceByType, fragment);
            }

            var service = serviceFactory.Create();
            return await service.Generate(type, configuration);
        }
    }
}
