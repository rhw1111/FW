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
        private readonly IApiScopeDataRepositoryCacheProxy _apiScopeDataRepositoryCacheProxy;

        public MainResourceStore(IIdentityResourceDataRepositoryCacheProxy identityResourceDataRepositoryCacheProxy, IApiResourceDataRepositoryCacheProxy apiResourceDataRepositoryCacheProxy, IApiScopeDataRepositoryCacheProxy apiScopeDataRepositoryCacheProxy)
        {
            _identityResourceDataRepositoryCacheProxy = identityResourceDataRepositoryCacheProxy;
            _apiResourceDataRepositoryCacheProxy = apiResourceDataRepositoryCacheProxy;
            _apiScopeDataRepositoryCacheProxy = apiScopeDataRepositoryCacheProxy;
        }


        public async Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            List<ApiResource> result = new List<ApiResource>();

            var resources = await _apiResourceDataRepositoryCacheProxy.QueryEnabled(apiResourceNames.ToList());

            foreach(var item in resources)
            {
                result.Add(await item.GenerateApiResource());
            }

            return result;
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var resources = await _apiResourceDataRepositoryCacheProxy.QueryByScopeEnabled(scopeNames.ToList());

            List<ApiResource> apis = new List<ApiResource>();

            foreach (var item in resources)
            {
                apis.Add(await item.GenerateApiResource());
            }
            return apis;
        }

        public async Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            var scopes = await _apiScopeDataRepositoryCacheProxy.QueryEnabled(scopeNames.ToList());

            List<ApiScope> apis = new List<ApiScope>();

            foreach (var item in scopes)
            {
                apis.Add(await item.GenerateApiScope());
            }
            return apis;
        }


        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
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
            var scopes = await _apiScopeDataRepositoryCacheProxy.QueryAllEnabled();
            List<ApiResource> apis = new List<ApiResource>();
            List<ApiScope> apiScopes = new List<ApiScope>();


            foreach (var item in apiResources)
            {
                apis.Add(await item.GenerateApiResource());
            
            }

            foreach(var item in scopes)
            {
                apiScopes.Add(await item.GenerateApiScope());
            }

            var identityResources = await _identityResourceDataRepositoryCacheProxy.QueryAllEnabled();
            List<IdentityResource> identitys = new List<IdentityResource>();
            foreach (var item in identityResources)
            {
                identitys.Add(await item.GenerateIdentityResource());
            }



            Resources resources = new Resources(identitys, apis, apiScopes);

            return resources;

        }
    }
}
