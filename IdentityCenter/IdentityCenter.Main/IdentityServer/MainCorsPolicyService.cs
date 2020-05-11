using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using IdentityCenter.Main.Entities;
using IdentityCenter.Main.Configuration;

namespace IdentityCenter.Main.IdentityServer
{
    public class MainCorsPolicyService : ICorsPolicyService
    {
        private readonly ISystemConfigurationService _systemConfigurationService;
        private readonly IIdentityHostRepositoryCacheProxy _identityHostRepositoryCacheProxy;

        public MainCorsPolicyService(ISystemConfigurationService systemConfigurationService, IIdentityHostRepositoryCacheProxy identityHostRepositoryCacheProxy)
        {
            _systemConfigurationService = systemConfigurationService;
            _identityHostRepositoryCacheProxy = identityHostRepositoryCacheProxy;
        }
        public async Task<bool> IsOriginAllowedAsync(string origin)
        {
            var appName=await _systemConfigurationService.GetIdentityHostApplicationName();
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

            if (host.AllowedCorsOrigins.Contains(origin))
            {
                return true;
            }
            return false;
        }
    }
}
