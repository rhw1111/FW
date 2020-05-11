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
    /// 通道凭证生成器
    /// </summary>
    public class ChannelCredentialsGenerator : EntityBase<IChannelCredentialsGeneratorIMP>
    {
        private static IFactory<IChannelCredentialsGeneratorIMP> _channelCredentialsGeneratorIMPFactory;

        public static IFactory<IChannelCredentialsGeneratorIMP> ChannelCredentialsGeneratorIMPFactory
        {
            set
            {
                _channelCredentialsGeneratorIMPFactory = value;
            }
        }


        public override IFactory<IChannelCredentialsGeneratorIMP> GetIMPFactory()
        {
            return _channelCredentialsGeneratorIMPFactory;
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

        public async Task<ChannelCredentials> Generate(string configuration)
        {
            return await _imp.Generate(this, configuration);
        }

    }


    public interface IChannelCredentialsGeneratorIMP
    {
        /// <summary>
        /// 为指定配置生成通道凭证
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        Task<ChannelCredentials> Generate(ChannelCredentialsGenerator generator, string configuration);
    }

    [Injection(InterfaceType = typeof(IChannelCredentialsGeneratorIMP), Scope = InjectionScope.Transient)]
    public class ChannelCredentialsGeneratorIMP:IChannelCredentialsGeneratorIMP
    {
        private IChannelCredentialsGeneratorService _channelCredentialsGeneratorService;

        public ChannelCredentialsGeneratorIMP(IChannelCredentialsGeneratorService channelCredentialsGeneratorService)
        {
            _channelCredentialsGeneratorService = channelCredentialsGeneratorService;
        }

        public async Task<ChannelCredentials> Generate(ChannelCredentialsGenerator generator, string configuration)
        {
            return await _channelCredentialsGeneratorService.Generate(generator.Type, configuration);
        }
    }




    public interface IChannelCredentialsGeneratorService
    {
        Task<ChannelCredentials> Generate(string type, string configuration);
    }

    [Injection(InterfaceType = typeof(IChannelCredentialsGeneratorService), Scope = InjectionScope.Singleton)]
    public class ChannelCredentialsGeneratorMainService : IChannelCredentialsGeneratorService
    {
        private static IDictionary<string, IFactory<IChannelCredentialsGeneratorService>> _channelFactories = new Dictionary<string, IFactory<IChannelCredentialsGeneratorService>>();

        public static IDictionary<string, IFactory<IChannelCredentialsGeneratorService>> ChannelFactories
        {
            get
            {
                return _channelFactories;
            }
        }
        public async Task<ChannelCredentials> Generate(string type, string configuration)
        {
            if (!_channelFactories.TryGetValue(type, out IFactory<IChannelCredentialsGeneratorService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundGrpcChannelCredentialsGeneratorServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Grpc通道凭证生成服务，发生位置：{1}",
                    ReplaceParameters = new List<object>() { type, $"{this.GetType().FullName}.Generate" }
                };
                throw new UtilityException((int)Errors.NotFoundGrpcChannelCredentialsGeneratorServiceByType, fragment);
            }

            var service = serviceFactory.Create();
            return await service.Generate(type, configuration);
        }
    }



}
