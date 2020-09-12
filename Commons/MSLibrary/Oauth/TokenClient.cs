using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Oauth
{
    public class TokenClient : EntityBase<ITokenClientIMP>
    {
        public override IFactory<ITokenClientIMP> GetIMPFactory()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Id
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
        /// 类型
        /// </summary>
        public string Type
        {
            get
            {

                return GetAttribute<string>(nameof(Type));
            }
            set
            {
                SetAttribute<string>(nameof(Type), value);
            }
        }

        /// <summary>
        /// 配置信息
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

    }

    public interface ITokenClientIMP
    {
        Task<string> GetToken(TokenClient client, CancellationToken cancellationToken = default);
    }


    [Injection(InterfaceType = typeof(ITokenClientIMP), Scope = InjectionScope.Transient)]
    public class TokenClientIMP : ITokenClientIMP
    {
        public static IDictionary<string, IFactory<ITokenClientService>> TokenClientServiceFactories { get; } = new Dictionary<string, IFactory<ITokenClientService>>();
        
        public Task<string> GetToken(TokenClient client, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        private ITokenClientService getService(string type)
        {
            if (!TokenClientServiceFactories.TryGetValue(type,out IFactory<ITokenClientService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundTokenClientServiceByType,
                    DefaultFormatting = "找不到类型为{0}的令牌客户端服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type,$"{this.GetType().FullName}.TokenClientServiceFactories" }
                };

                throw new UtilityException((int)Errors.NotFoundTokenClientServiceByType, fragment);
            }

            return serviceFactory.Create();
        }

    }


    public interface ITokenClientService
    {
        Task<string> GetToken();
    }
}
