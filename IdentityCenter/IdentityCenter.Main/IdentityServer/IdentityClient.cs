using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.Identity.Client;
using MSLibrary;
using MSLibrary.DI;

namespace IdentityCenter.Main.IdentityServer
{
    public class IdentityClient : EntityBase<IIdentityClientIMP>
    {
        private static IFactory<IIdentityClientIMP>? _identityClientIMPFacory;

        public IdentityClient(IFactory<IIdentityClientIMP> identityClientIMPFacory)
        {
            _identityClientIMPFacory = identityClientIMPFacory;
        }
        public override IFactory<IIdentityClientIMP>? GetIMPFactory()
        {
            return _identityClientIMPFacory;
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
        /// 客户端ID
        /// </summary>
        public string ClientID
        {
            get
            {

                return GetAttribute<string>(nameof(ClientID));
            }
            set
            {
                SetAttribute<string>(nameof(ClientID), value);
            }
        }

        /// <summary>
        /// 客户端机密列表
        /// </summary>
        public string[] ClientSecrets
        {
            get
            {

                return GetAttribute<string[]>(nameof(ClientSecrets));
            }
            set
            {
                SetAttribute<string[]>(nameof(ClientSecrets), value);
            }
        }

        /// <summary>
        /// 允许的协议列表
        /// </summary>
        public string[] AllowedGrantTypes
        {
            get
            {

                return GetAttribute<string[]>(nameof(AllowedGrantTypes));
            }
            set
            {
                SetAttribute<string[]>(nameof(AllowedGrantTypes), value);
            }
        }
        

        /// <summary>
        /// 允许的范围列表
        /// </summary>
        public string[] AllowedScopes
        {
            get
            {

                return GetAttribute<string[]>(nameof(AllowedScopes));
            }
            set
            {
                SetAttribute<string[]>(nameof(AllowedScopes), value);
            }
        }

        /// <summary>
        /// 允许的客户端重定向地址
        /// </summary>
        public string[] RedirectUris
        {
            get
            {

                return GetAttribute<string[]>(nameof(RedirectUris));
            }
            set
            {
                SetAttribute<string[]>(nameof(RedirectUris), value);
            }
        }

        /// <summary>
        /// 登出时需要Post到的地址列表
        /// </summary>
        public string[] PostLogoutRedirectUris
        {
            get
            {

                return GetAttribute<string[]>(nameof(PostLogoutRedirectUris));
            }
            set
            {
                SetAttribute<string[]>(nameof(PostLogoutRedirectUris), value);
            }
        }

        /// <summary>
        /// 是否允许通过浏览器传递AccessTokens
        /// </summary>
        public bool AllowAccessTokensViaBrowser
        {
            get
            {

                return GetAttribute<bool>(nameof(AllowAccessTokensViaBrowser));
            }
            set
            {
                SetAttribute<bool>(nameof(AllowAccessTokensViaBrowser), value);
            }
        }

        /// <summary>
        /// 是否需要准许
        /// </summary>
        public bool RequireConsent
        {
            get
            {

                return GetAttribute<bool>(nameof(RequireConsent));
            }
            set
            {
                SetAttribute<bool>(nameof(RequireConsent), value);
            }
        }

        /// <summary>
        /// AccessToken的有效时间（秒）
        /// </summary>
        public int AccessTokenLifetime
        {
            get
            {

                return GetAttribute<int>(nameof(AccessTokenLifetime));
            }
            set
            {
                SetAttribute<int>(nameof(AccessTokenLifetime), value);
            }
        }
        /// <summary>
        /// RefreshToken的绝对过期时间（秒）
        /// </summary>
        public int AbsoluteRefreshTokenLifetime
        {
            get
            {

                return GetAttribute<int>(nameof(AbsoluteRefreshTokenLifetime));
            }
            set
            {
                SetAttribute<int>(nameof(AbsoluteRefreshTokenLifetime), value);
            }
        }

        /// <summary>
        /// RefreshToken的滑动过期时间（秒）
        /// </summary>
        public int SlidingRefreshTokenLifetime
        {
            get
            {

                return GetAttribute<int>(nameof(SlidingRefreshTokenLifetime));
            }
            set
            {
                SetAttribute<int>(nameof(SlidingRefreshTokenLifetime), value);
            }
        }
        /// <summary>
        /// IDToken的过期时间（秒）
        /// </summary>
        public int IdentityTokenLifetime
        {
            get
            {

                return GetAttribute<int>(nameof(IdentityTokenLifetime));
            }
            set
            {
                SetAttribute<int>(nameof(IdentityTokenLifetime), value);
            }
        }

        /// <summary>
        /// 允许的跨域源列表
        /// </summary>
        public string[] AllowedCorsOrigins
        {
            get
            {

                return GetAttribute<string[]>(nameof(AllowedCorsOrigins));
            }
            set
            {
                SetAttribute<string[]>(nameof(AllowedCorsOrigins), value);
            }
        }

        /// <summary>
        /// 是否要求Pkce
        /// </summary>
        public bool RequirePkce
        {
            get
            {

                return GetAttribute<bool>(nameof(RequirePkce));
            }
            set
            {
                SetAttribute<bool>(nameof(RequirePkce), value);
            }
        }
        /// <summary>
        /// 是否需要RefreshToken
        /// </summary>
        public bool AllowOfflineAccess
        {
            get
            {

                return GetAttribute<bool>(nameof(AllowOfflineAccess));
            }
            set
            {
                SetAttribute<bool>(nameof(AllowOfflineAccess), value);
            }
        }

        /// <summary>
        /// 本地登录是否可用
        /// </summary>
        public bool EnableLocalLogin
        {
            get
            {

                return GetAttribute<bool>(nameof(EnableLocalLogin));
            }
            set
            {
                SetAttribute<bool>(nameof(EnableLocalLogin), value);
            }
        }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enabled
        {
            get
            {

                return GetAttribute<bool>(nameof(Enabled));
            }
            set
            {
                SetAttribute<bool>(nameof(Enabled), value);
            }
        }
        /// <summary>
        /// 扩展配置
        /// </summary>
        public string ExtensionConfiguration
        {
            get
            {

                return GetAttribute<string>(nameof(ExtensionConfiguration));
            }
            set
            {
                SetAttribute<string>(nameof(ExtensionConfiguration), value);
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

        /// <summary>
        /// 生成对应的Client
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public async Task<Client> GenerateIdentityClient(CancellationToken cancellationToken = default)
        {
            return await _imp.GenerateIdentityClient(this, cancellationToken);
        }

    }

    public interface IIdentityClientIMP
    {
        Task<Client> GenerateIdentityClient(IdentityClient client, CancellationToken cancellationToken = default);
    }

    [Injection(InterfaceType = typeof(IIdentityClientIMP), Scope = InjectionScope.Transient)]
    public class IdentityClientIMP : IIdentityClientIMP
    {
        public async Task<Client> GenerateIdentityClient(IdentityClient client, CancellationToken cancellationToken = default)
        {
            List<Secret> identityClientSecrets = new List<Secret>();
            foreach(var item in client.ClientSecrets)
            {
                identityClientSecrets.Add(new Secret(item));
            }
             Client identityClient = new Client();
            identityClient.ClientId = client.ClientID;
            identityClient.ClientSecrets = identityClientSecrets;
            identityClient.ClientName = client.Name;
            identityClient.AllowedGrantTypes = client.AllowedGrantTypes;
            identityClient.AllowAccessTokensViaBrowser = client.AllowAccessTokensViaBrowser;
            identityClient.AccessTokenLifetime = client.AccessTokenLifetime;
            identityClient.AbsoluteRefreshTokenLifetime = client.AbsoluteRefreshTokenLifetime;
            identityClient.SlidingRefreshTokenLifetime = client.SlidingRefreshTokenLifetime;
            identityClient.IdentityTokenLifetime = client.IdentityTokenLifetime;
            identityClient.AllowedCorsOrigins = client.AllowedCorsOrigins;
            identityClient.RedirectUris = client.RedirectUris;
            identityClient.PostLogoutRedirectUris = client.PostLogoutRedirectUris;
            identityClient.RequirePkce = client.RequirePkce;
            identityClient.AllowOfflineAccess = client.AllowOfflineAccess;
            identityClient.EnableLocalLogin = client.EnableLocalLogin;
            identityClient.Enabled = client.Enabled;

            return await Task.FromResult(identityClient);
        }
    }
}
