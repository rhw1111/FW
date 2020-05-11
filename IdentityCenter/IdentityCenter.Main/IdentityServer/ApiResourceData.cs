﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using IdentityServer4.Models;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace IdentityCenter.Main.IdentityServer
{
    public class ApiResourceData:ResourceData
    {
        private static IFactory<IApiResourceDataIMP>? _apiResourceDataIMPFactory;

        public static IFactory<IApiResourceDataIMP> ApiResourceDataIMPFactory
        {
            set
            {
                _apiResourceDataIMPFactory = value;
            }
        }

        private class innerApiResourceDataIMPFactory : IFactory<IApiResourceDataIMP>
        {
            public IApiResourceDataIMP Create()
            {
                IApiResourceDataIMP imp;
                var di = ContextContainer.GetValue<IDIContainer>("DI");
                if (di == null)
                {
                    imp = DIContainerContainer.Get<IApiResourceDataIMP>();
                }
                else
                {
                    imp = di.Get<IApiResourceDataIMP>();
                }
                return imp;
            }
        }

        public override IFactory<IResourceDataIMP>? GetIMPFactory()
        {
            if (_apiResourceDataIMPFactory == null)
            {
                return new innerApiResourceDataIMPFactory();
            }
            else
            {
                return _apiResourceDataIMPFactory;
            }

        }

        public List<string> ApiSecrets
        {
            get
            {

                return GetAttribute<List<string>>(nameof(ApiSecrets));
            }
            set
            {
                SetAttribute<List<string>>(nameof(ApiSecrets), value);
            }
        }

        public async Task<ApiResource> GenerateApiResource(CancellationToken cancellationToken = default)
        {
            return await ((IApiResourceDataIMP)_imp).GenerateApiResource(this, cancellationToken);
        }
    }


    public interface IApiResourceDataIMP:IResourceDataIMP
    {
        Task<ApiResource> GenerateApiResource(ApiResourceData data, CancellationToken cancellationToken = default);
    }

    [Injection(InterfaceType = typeof(IApiResourceDataIMP), Scope = InjectionScope.Transient)]
    public class ApiResourceDataIMP : ResourceDataIMP, IApiResourceDataIMP
    {
        public async Task<ApiResource> GenerateApiResource(ApiResourceData data, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(new ApiResource(data.Name, data.DisplayName, data.UserClaims)
            {
                ApiSecrets = (from t in data.ApiSecrets
                              select new Secret(t)).ToList(),
                Description = data.Description,
                Enabled = data.Enabled,
                Properties = data.Properties,
                UserClaims = data.UserClaims
            });
        }
    }
}
