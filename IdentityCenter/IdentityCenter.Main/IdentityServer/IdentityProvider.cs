using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using IdentityCenter.Main.Entities;

namespace IdentityCenter.Main.IdentityServer
{
    /// <summary>
    /// 认证提供方信息
    /// </summary>
    public class IdentityProvider : EntityBase<IIdentityProviderIMP>
    {
        private static IFactory<IIdentityProviderIMP>? _identityProviderIMPFactory;

        public static IFactory<IIdentityProviderIMP> IdentityProviderIMPFactory
        {
            set
            {
                _identityProviderIMPFactory = value;
            }
        }

        public override IFactory<IIdentityProviderIMP>? GetIMPFactory()
        {
            return _identityProviderIMPFactory;
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
        /// 认证Scheme的名称
        /// </summary>
        public string SchemeName
        {
            get
            {

                return GetAttribute<string>(nameof(SchemeName));
            }
            set
            {
                SetAttribute<string>(nameof(SchemeName), value);
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
        /// 不同类型有不同的配置信息
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
        /// 显示名称
        /// </summary>
        public string DisplayName
        {
            get
            {

                return GetAttribute<string>(nameof(DisplayName));
            }
            set
            {
                SetAttribute<string>(nameof(DisplayName), value);
            }
        }

        /// <summary>
        /// 图标地址
        /// </summary>
        public string Icon
        {
            get
            {

                return GetAttribute<string>(nameof(Icon));
            }
            set
            {
                SetAttribute<string>(nameof(Icon), value);
            }
        }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Active
        {
            get
            {

                return GetAttribute<bool>(nameof(Active));
            }
            set
            {
                SetAttribute<bool>(nameof(Active), value);
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

        public async Task<IdentityClaims> GetClaims(ClaimsPrincipal userPrincipal,CancellationToken cancellationToken = default)
        {
            return await _imp.GetClaims(this, userPrincipal, cancellationToken);
        }

        public async Task InitUserAccount(AuthenticateResult authenticateResult, UserAccount userAccount, UserThirdPartyAccount partyAccount, CancellationToken cancellationToken = default)
        {
            await _imp.InitUserAccount(this, authenticateResult, userAccount, partyAccount,cancellationToken);
        }

        public async Task InitOption<T>( T option, CancellationToken cancellationToken = default)
        {
            await _imp.InitOption(this, option, cancellationToken);
        }
    }

    public interface IIdentityProviderIMP
    {
        Task<IdentityClaims> GetClaims(IdentityProvider provider, ClaimsPrincipal userPrincipal, CancellationToken cancellationToken = default);
        Task InitUserAccount(IdentityProvider provider, AuthenticateResult authenticateResult, UserAccount userAccount, UserThirdPartyAccount partyAccount, CancellationToken cancellationToken = default);
        Task InitOption<T>(IdentityProvider provider, T option, CancellationToken cancellationToken = default);
    }

    public class IdentityClaims
    {
        public virtual string ProviderUserId
        {
            get;
        } =  null!;

        public virtual IReadOnlyList<Claim> Claims
        {
            get;
        } = null!;
    }

    public interface IIdentityProviderService
    {
        Task<IdentityClaims> GetClaims(ClaimsPrincipal userPrincipal, CancellationToken cancellationToken = default);
        Task InitUserAccount(AuthenticateResult authenticateResult, UserAccount userAccount, UserThirdPartyAccount partyAccount, CancellationToken cancellationToken = default);
        Task InitOption<T>(string configuration,T option, CancellationToken cancellationToken = default);
    }

    [Injection(InterfaceType = typeof(IIdentityProviderIMP), Scope = InjectionScope.Transient)]
    public class IdentityProviderIMP : IIdentityProviderIMP
    {
        public static IDictionary<string, IFactory<IIdentityProviderService>> IdentityProviderServices { get; } = new Dictionary<string, IFactory<IIdentityProviderService>>();


        public async Task<IdentityClaims> GetClaims(IdentityProvider provider, ClaimsPrincipal userPrincipal, CancellationToken cancellationToken = default)
        {
            var service = getService(provider.Type);
            return await service.GetClaims(userPrincipal, cancellationToken);
        }

        public async Task InitOption<T>(IdentityProvider provider, T option, CancellationToken cancellationToken = default)
        {
            var service = getService(provider.Type);
             await service.InitOption(provider.Configuration,option, cancellationToken);
        }

        public async Task InitUserAccount(IdentityProvider provider, AuthenticateResult authenticateResult, UserAccount userAccount, UserThirdPartyAccount partyAccount, CancellationToken cancellationToken = default)
        {
            var service = getService(provider.Type);
             await service.InitUserAccount(authenticateResult, userAccount, partyAccount, cancellationToken);
        }

        private IIdentityProviderService getService(string type)
        {
            if (!IdentityProviderServices.TryGetValue(type,out IFactory<IIdentityProviderService>? serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.NotFoundIdentityProviderServiceByType,
                    DefaultFormatting = "找不到类型为{0}的认证方服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type,$"{this.GetType().FullName}.IdentityProviderServices" }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.NotFoundIdentityProviderServiceByType, fragment, 1, 0);
            }

            return serviceFactory.Create();
        }
    }
}
