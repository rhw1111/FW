using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using MSLibrary.LanguageTranslate;
using MSLibrary.DI;

namespace MSLibrary.SystemToken
{
    /// <summary>
    /// 令牌控制器
    /// 负责令牌的生成与验证
    /// </summary>
    public class TokenController : EntityBase<ITokenControllerIMP>
    {
        private static IFactory<ITokenControllerIMP> _tokenControllerIMPFactory;

        public static IFactory<ITokenControllerIMP> TokenControllerIMPFactory
        {
            set
            {
                _tokenControllerIMPFactory = value;
            }
        }
        public override IFactory<ITokenControllerIMP> GetIMPFactory()
        {
            return _tokenControllerIMPFactory;
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

        /// <summary>
        /// 生成令牌
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public async Task<string> Generate(IEnumerable<Claim> claims)
        {
            return await _imp.Generate(this, claims);
        }
        /// <summary>
        /// 验证令牌
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ClaimsPrincipal> Validate(string token)
        {
            return await _imp.Validate(this, token);
        }

    }

    public interface ITokenControllerIMP
    {
        Task<string> Generate(TokenController controller,IEnumerable<Claim> claims);
        Task<ClaimsPrincipal> Validate(TokenController controller,string token);
    }



    [Injection(InterfaceType = typeof(ITokenControllerIMP), Scope = InjectionScope.Transient)]
    public class TokenControllerIMP : ITokenControllerIMP
    {
        public static IDictionary<string, IFactory<ITokenControllerService>> TokenControllerServiceFactories { get; } = new Dictionary<string, IFactory<ITokenControllerService>>();

        public async Task<string> Generate(TokenController controller, IEnumerable<Claim> claims)
        {
            var service=getService(controller.Type);
            return await service.Generate(controller.Configuration, claims);
        }

        public async Task<ClaimsPrincipal> Validate(TokenController controller, string token)
        {
            var service = getService(controller.Type);
            return await service.Validate(controller.Configuration, token);
        }

        private ITokenControllerService getService(string type)
        {

            if (!TokenControllerServiceFactories.TryGetValue(type,out IFactory<ITokenControllerService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundTokenControllerServiceByType,
                    DefaultFormatting = "找不到类型为{0}的令牌控制器服务",
                    ReplaceParameters = new List<object>() { type }
                };

                throw new UtilityException((int)Errors.NotFoundTokenControllerServiceByType, fragment);
            }

            return serviceFactory.Create();
        }
    }

    public interface ITokenControllerService
    {
        Task<string> Generate(string configuration, IEnumerable<Claim> claims);
        Task<ClaimsPrincipal> Validate(string configuration, string token);
    }

}
