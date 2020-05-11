using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using IdentityServer4;
using IdentityServer4.Configuration;
using Microsoft.IdentityModel.Tokens;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Serializer;
using System.Security.Cryptography.X509Certificates;

namespace IdentityCenter.Main.IdentityServer
{
    /// <summary>
    /// 认证主机
    /// </summary>
    public class IdentityHost : EntityBase<IIdentityHostIMP>
    {
        private static IFactory<IIdentityHostIMP>? _identityHostIMPFactory;

        public static IFactory<IIdentityHostIMP> IdentityHostIMPFactory
        {
            set
            {
                _identityHostIMPFactory = value;
            }
        }
        public override IFactory<IIdentityHostIMP>? GetIMPFactory()
        {
            return _identityHostIMPFactory;
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
        /// 环境声明生成器名称
        /// </summary>
        public string EnvironmentClaimGeneratorName
        {
            get
            {
                return GetAttribute<string>(nameof(EnvironmentClaimGeneratorName));
            }
            set
            {
                SetAttribute<string>(nameof(EnvironmentClaimGeneratorName), value);
            }
        }

        /// <summary>
        /// 声明上下文生成器名称
        /// </summary>
        public string ClaimContextGeneratorName
        {
            get
            {
                return GetAttribute<string>(nameof(ClaimContextGeneratorName));
            }
            set
            {
                SetAttribute<string>(nameof(ClaimContextGeneratorName), value);
            }
        }

        /// <summary>
        /// 允许的跨域源列表
        /// </summary>
        public List<string> AllowedCorsOrigins
        {
            get
            {
                return GetAttribute<List<string>>(nameof(AllowedCorsOrigins));
            }
            set
            {
                SetAttribute<List<string>>(nameof(AllowedCorsOrigins), value);
            }
        }

        /// <summary>
        /// 外部认证回调地址
        /// </summary>
        public string ExternalCallbackUri
        {
            get
            {
                return GetAttribute<string>(nameof(ExternalCallbackUri));
            }
            set
            {
                SetAttribute<string>(nameof(ExternalCallbackUri), value);
            }
        }

        /// <summary>
        /// 外部认证绑定页面地址
        /// </summary>
        public string ExternalIdentityBindPage
        {
            get
            {
                return GetAttribute<string>(nameof(ExternalIdentityBindPage));
            }
            set
            {
                SetAttribute<string>(nameof(ExternalIdentityBindPage), value);
            }
        }

        /// <summary>
        /// 外部登出回调地址
        /// </summary>
        public string ExternalLogoutCallbackUri
        {
            get
            {
                return GetAttribute<string>(nameof(ExternalLogoutCallbackUri));
            }
            set
            {
                SetAttribute<string>(nameof(ExternalLogoutCallbackUri), value);
            }
        }

        /// <summary>
        /// 登出后页面地址
        /// </summary>
        public string LoggedPage
        {
            get
            {
                return GetAttribute<string>(nameof(LoggedPage));
            }
            set
            {
                SetAttribute<string>(nameof(LoggedPage), value);
            }
        }

        /// <summary>
        /// 本地登录设置
        /// </summary>
        public LocalLoginSetting LocalLoginSetting
        {
            get
            {
                return GetAttribute<LocalLoginSetting>(nameof(LocalLoginSetting));
            }
            set
            {
                SetAttribute<LocalLoginSetting>(nameof(LocalLoginSetting), value);
            }
        }


        /// <summary>
        /// 签名密钥配置信息
        /// </summary>
        public string SigningCredentialConfiguration
        {
            get
            {

                return GetAttribute<string>(nameof(SigningCredentialConfiguration));
            }
            set
            {
                SetAttribute<string>(nameof(SigningCredentialConfiguration), value);
            }
        }


        /// <summary>
        /// 服务选项配置信息
        /// </summary>
        public string ServerOptionsConfiguration
        {
            get
            {

                return GetAttribute<string>(nameof(ServerOptionsConfiguration));
            }
            set
            {
                SetAttribute<string>(nameof(ServerOptionsConfiguration), value);
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
        /// 初始化option
        /// </summary>
        /// <param name="host"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public async Task<IIdentityServerOptionsInit> InitIdentityServerOption(CancellationToken cancellationToken = default)
        {
            return await _imp.InitIdentityServerOption(this,cancellationToken);
        }
        /// <summary>
        /// 生成签名密钥
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public async Task<Object> GenerateSigningCredentials(CancellationToken cancellationToken = default)
        {
            return await _imp.GenerateSigningCredentials(this,cancellationToken);
        }

    }


    /// <summary>
    /// 本地登录设置
    /// </summary>
    public class LocalLoginSetting
    {
        /// <summary>
        /// 是否允许本地登录
        /// </summary>
        public bool AllowLocalLogin
        {
            get; set;
        }
        /// <summary>
        /// 是否保持登录
        /// </summary>
        public bool AllowRememberLogin
        {
            get;
            set;
        }
        /// <summary>
        /// 保持登录的时间（天）
        /// </summary>
        public int RememberLoginDuration
        {
            get;
            set;
        }
        /// <summary>
        /// 是否显示登出窗口
        /// </summary>
        public bool ShowLogoutPrompt
        {
            get;
            set;
        }
    }

    public interface IIdentityServerOptionsInit
    {
        void Init(IdentityServerOptions options);
    }

    public interface IIdentityHostIMP
    {
        /// <summary>
        /// 初始化option
        /// </summary>
        /// <param name="host"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<IIdentityServerOptionsInit> InitIdentityServerOption(IdentityHost host, CancellationToken cancellationToken = default);
        /// <summary>
        /// 生成签名密钥
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        Task<Object> GenerateSigningCredentials(IdentityHost host, CancellationToken cancellationToken = default);
    }




    [Injection(InterfaceType = typeof(IIdentityHostIMP), Scope = InjectionScope.Transient)]
    public class IdentityHostIMP : IIdentityHostIMP
    {
        public async Task<Object> GenerateSigningCredentials(IdentityHost host, CancellationToken cancellationToken = default)
        {
            object signingCredentials;
           
            var configurationObj= getSigningCredentialConfiguration(host.SigningCredentialConfiguration);
            switch (configurationObj.Type)
            {
                case 0:
                    RsaSecurityKey securityKey = new RsaSecurityKey((JsonSerializerHelper.Deserialize<RSAParameters>(configurationObj.RSAParameterData)));
                    signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256Signature);
                    break;
                default:
                    signingCredentials = new X509Certificate2(configurationObj.CertificateFile, configurationObj.CertificatePasword);
                    break;
            }


            return await Task.FromResult(signingCredentials);
        }

        public async Task<IIdentityServerOptionsInit> InitIdentityServerOption(IdentityHost host, CancellationToken cancellationToken = default)
        {
            var configurationObj = getIdentityServerOptionConfiguration(host.ServerOptionsConfiguration);
            IdentityServerOptionsInitDefault init = new IdentityServerOptionsInitDefault(configurationObj);
            return await Task.FromResult(init);
        }

        private SigningCredentialConfiguration getSigningCredentialConfiguration(string configuration)
        {
            var configurationObj= JsonSerializerHelper.Deserialize<SigningCredentialConfiguration>(configuration);
            return configurationObj;
        }

        private IdentityServerOptionConfiguration getIdentityServerOptionConfiguration(string configuration)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<IdentityServerOptionConfiguration>(configuration);
            return configurationObj;
        }

        /// <summary>
        /// 签名密钥配置类
        /// </summary>
        [DataContract]
        private class SigningCredentialConfiguration
        {
            /// <summary>
            /// 类型
            /// 0：使用RSAParameterData
            /// 1：使用路径+密码的证书
            /// </summary>
            [DataMember]
            public int Type { get; set; }
            /// <summary>
            /// 非对称加密密钥数据
            /// </summary>
            [DataMember]
            public string? RSAParameterData { get; set; }
            /// <summary>
            /// 证书路径
            /// </summary>
            [DataMember]
            public string? CertificateFile { get; set; }
            /// <summary>
            /// 证书密码
            /// </summary>
            [DataMember]
            public string? CertificatePasword { get; set; }
        }

        /// <summary>
        /// 认证服务选项配置类
        /// </summary>
        [DataContract]
        private class IdentityServerOptionConfiguration
        {
            /// <summary>
            /// 登录地址
            /// </summary>
            [DataMember]
            public string? LoginUrl { get; set; }
            /// <summary>
            /// 登出地址
            /// </summary>
            [DataMember]
            public string? LogoutUrl { get; set; }
            /// <summary>
            /// 确认权限页面地址
            /// </summary>
            [DataMember]
            public string? ConsentUrl { get; set; }
        }

        private class IdentityServerOptionsInitDefault : IIdentityServerOptionsInit
        {
            private IdentityServerOptionConfiguration _configuration;

            public IdentityServerOptionsInitDefault(IdentityServerOptionConfiguration configuration)
            {
                _configuration = configuration;
            }
            public void Init(IdentityServerOptions options)
            {
                options.UserInteraction.LogoutUrl = _configuration.LogoutUrl;
                options.UserInteraction.LoginUrl = _configuration.LoginUrl;
                options.UserInteraction.ConsentUrl = _configuration.ConsentUrl;
            }
        }
    }
}
