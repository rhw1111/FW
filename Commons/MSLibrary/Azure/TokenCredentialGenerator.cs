using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Azure
{
    /// <summary>
    /// 令牌凭据生成器
    /// </summary>
    public class TokenCredentialGenerator:EntityBase<ITokenCredentialGeneratorIMP>
    {
        private static IFactory<ITokenCredentialGeneratorIMP> _tokenCredentialGeneratorIMPFactory;

        public static IFactory<ITokenCredentialGeneratorIMP> TokenCredentialGeneratorIMPFactory
        {
            set
            {
                _tokenCredentialGeneratorIMPFactory = value;
            }
        }
        public override IFactory<ITokenCredentialGeneratorIMP> GetIMPFactory()
        {
            return _tokenCredentialGeneratorIMPFactory;
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
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {
                return GetAttribute<string>("Name");
            }
            set
            {
                SetAttribute<string>("Name", value);
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
        /// 配置
        /// </summary>
        public string Configuration
        {
            get
            {
                return GetAttribute<string>("Configuration");
            }
            set
            {
                SetAttribute<string>("Configuration", value);
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

        public async Task<TokenCredential> Generate()
        {
            return await _imp.Generate(this);
        }
    }

    public interface ITokenCredentialGeneratorIMP
    {
        Task<TokenCredential> Generate(TokenCredentialGenerator generator);
    }

    public interface ITokenCredentialGeneratorService
    {
        Task<TokenCredential> Generate(string configuration);
    }


    [Injection(InterfaceType = typeof(ITokenCredentialGeneratorIMP), Scope = InjectionScope.Transient)]
    public class TokenCredentialGeneratorIMP : ITokenCredentialGeneratorIMP
    {
       public static IDictionary<string, IFactory<ITokenCredentialGeneratorService>> TokenCredentialGeneratorServiceFactories { get; } = new Dictionary<string, IFactory<ITokenCredentialGeneratorService>>();

        public async Task<TokenCredential> Generate(TokenCredentialGenerator generator)
        {
            var service = getService(generator.Type);
            return await service.Generate(generator.Configuration);
        }

        private ITokenCredentialGeneratorService getService(string type)
        {
            if (!TokenCredentialGeneratorServiceFactories.TryGetValue(type,out IFactory<ITokenCredentialGeneratorService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundTokenCredentialGeneratorServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Azure令牌凭据生成服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type,$"{this.GetType().FullName}.TokenCredentialGeneratorServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundTokenCredentialGeneratorServiceByType, fragment);
            }

            return serviceFactory.Create();
        }

    }
}
