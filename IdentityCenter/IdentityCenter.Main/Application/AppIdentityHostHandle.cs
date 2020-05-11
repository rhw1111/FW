using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Tokens;
using IdentityServer4.Configuration;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using IdentityCenter.Main.Configuration;
using IdentityCenter.Main.IdentityServer;
using IdentityCenter.Main.DTOModel;


namespace IdentityCenter.Main.Application
{
    [Injection(InterfaceType = typeof(IAppIdentityHostHandle), Scope = InjectionScope.Singleton)]
    public class AppIdentityHostHandle : IAppIdentityHostHandle
    {
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly IIdentityHostRepositoryCacheProxy _identityHostRepositoryCacheProxy;

        public AppIdentityHostHandle(ISystemConfigurationService systemConfigurationService, IIdentityHostRepositoryCacheProxy identityHostRepositoryCacheProxy)
        {
            _systemConfigurationService = systemConfigurationService;
            _identityHostRepositoryCacheProxy = identityHostRepositoryCacheProxy;
        }
        public async Task<(IdentityHostHandleResult, IIdentityServiceOptionsInitController)> Do()
        {
            IdentityHostHandleResult result = new IdentityHostHandleResult();
            var appName = await _systemConfigurationService.GetIdentityHostApplicationName();

            var host = await _identityHostRepositoryCacheProxy.QueryByName(appName);
            if (host == null)
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.NotFoundIdentityHostByName,
                    DefaultFormatting = "找不到名称为{0}的认证主机",
                    ReplaceParameters = new List<object>() { appName }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.NotFoundIdentityHostByName, fragment, 1, 0);
            }

            IdentityServiceOptionsInitControllerDefault serviceOptionsInitController = new IdentityServiceOptionsInitControllerDefault(await host.InitIdentityServerOption());

            var signingCredentialObj=await host.GenerateSigningCredentials();
            if (signingCredentialObj == null)
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.IdentityHostSigningCredentialTypeError,
                    DefaultFormatting = "认证主机{0}的签名密钥类型不正确，当前类型为{1}",
                    ReplaceParameters = new List<object>() { appName,"null" }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.IdentityHostSigningCredentialTypeError, fragment, 1, 0);
            }
            switch(signingCredentialObj)
            {
                case X509Certificate2 cert:
                    result.SignCredentialCertificate = cert;
                    break;
                case SigningCredentials key:
                    result.SigningCredentials = key;
                    break;
                default:
                    var fragment = new TextFragment()
                    {
                        Code = IdentityCenterTextCodes.IdentityHostSigningCredentialTypeError,
                        DefaultFormatting = "认证主机{0}的签名密钥类型不正确，当前类型为{1}",
                        ReplaceParameters = new List<object>() { appName, signingCredentialObj.GetType().FullName??string.Empty }
                    };

                    throw new UtilityException((int)IdentityCenterErrorCodes.IdentityHostSigningCredentialTypeError, fragment, 1, 0);
            }

            result.AllowedCorsOrigins = host.AllowedCorsOrigins;

            return (result, serviceOptionsInitController);

        }
    }

    public class IdentityServiceOptionsInitControllerDefault : IIdentityServiceOptionsInitController
    {
        private readonly IIdentityServerOptionsInit _identityServerOptionsInit;

        public IdentityServiceOptionsInitControllerDefault(IIdentityServerOptionsInit identityServerOptionsInit)
        {
            _identityServerOptionsInit = identityServerOptionsInit;
        }
        public void Init(IdentityServerOptions options)
        {
            _identityServerOptionsInit.Init(options);
        }
    }
}
