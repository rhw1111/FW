using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using IdentityCenter.Main.Entities;
using IdentityServer4.Stores;
using MongoDB.Libmongocrypt;

namespace IdentityCenter.Main.IdentityServer
{
    public class MainResourceStore : IResourceStore
    {
        private readonly IIdentityResourceDataRepositoryCacheProxy _identityResourceDataRepositoryCacheProxy;
        private readonly IApiResourceDataRepositoryCacheProxy _apiResourceDataRepositoryCacheProxy;

        public MainResourceStore(IIdentityResourceDataRepositoryCacheProxy identityResourceDataRepositoryCacheProxy, IApiResourceDataRepositoryCacheProxy apiResourceDataRepositoryCacheProxy)
        {
            _identityResourceDataRepositoryCacheProxy = identityResourceDataRepositoryCacheProxy;
            _apiResourceDataRepositoryCacheProxy = apiResourceDataRepositoryCacheProxy;
        }

        public async Task<ApiResource> FindApiResourceAsync(string name)
        {
            var resource = await _apiResourceDataRepositoryCacheProxy.QueryEnabled(name);
            if (resource==null)
            {
                var fragment = new TextFragment()
                {
                    Code = IdentityCenterTextCodes.NotFoundApiResourceByName,
                    DefaultFormatting = "找不到名称为{0}的Api资源",
                    ReplaceParameters = new List<object>() { name }
                };

                throw new UtilityException((int)IdentityCenterErrorCodes.NotFoundApiResourceByName, fragment, 1, 0);
            }

            return await resource.GenerateApiResource();

        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var resources = await _apiResourceDataRepositoryCacheProxy.QueryEnabled(scopeNames.ToList());

            List<ApiResource> apis = new List<ApiResource>();

            foreach(var item in resources)
            {
                apis.Add(await item.GenerateApiResource());
            }
            return apis;
        }

        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var resources = await _identityResourceDataRepositoryCacheProxy.QueryEnabled(scopeNames.ToList());
            List<IdentityResource> identitys = new List<IdentityResource>();
            foreach (var item in resources)
            {
                identitys.Add(await item.GenerateIdentityResource());
            }


            return identitys;
        }

        public async Task<Resources> GetAllResourcesAsync()
        {
            var apiResources = await _apiResourceDataRepositoryCacheProxy.QueryAllEnabled();
            List<ApiResource> apis = new List<ApiResource>();

            foreach (var item in apiResources)
            {
                apis.Add(await item.GenerateApiResource());
            }

            var identityResources = await _identityResourceDataRepositoryCacheProxy.QueryAllEnabled();
            List<IdentityResource> identitys = new List<IdentityResource>();
            foreach (var item in identityResources)
            {
                identitys.Add(await item.GenerateIdentityResource());
            }



            Resources resources = new Resources(identitys, apis);

            return resources;

        }
    }
}
