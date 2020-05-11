 using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.Validation;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using IdentityCenter.Main.Entities;
using IdentityServer4.Stores;
using IdentityServer4.Models;

namespace IdentityCenter.Main.IdentityServer
{
    /// <summary>
    /// IdentityServer的客户端信息
    /// </summary>
    public class MainClientStore : IClientStore
    {
        private IIdentityClientRepositoryCacheProxy _identityClientRepositoryCacheProxy;

        public MainClientStore(IIdentityClientRepositoryCacheProxy identityClientRepositoryCacheProxy)
        {
            _identityClientRepositoryCacheProxy = identityClientRepositoryCacheProxy;
        }
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
   
            var identitycClient = await _identityClientRepositoryCacheProxy.QueryByClientID(clientId);
            if (identitycClient == null)
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.NotFoundIdentityServerClientByID,
                    DefaultFormatting = "找不到id为{0}的认证服务客户端",
                    ReplaceParameters = new List<object>() { clientId }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.NotFoundIdentityServerClientByID, fragment, 1, 0);
            }

            return await identitycClient.GenerateIdentityClient();


        }
    }
}
