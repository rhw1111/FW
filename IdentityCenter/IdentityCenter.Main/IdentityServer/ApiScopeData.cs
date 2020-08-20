using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using IdentityServer4.Models;
using MSLibrary;
using MSLibrary.DI;
using MongoDB.Driver;

namespace IdentityCenter.Main.IdentityServer
{
    public class ApiScopeData: ResourceData
    {
        private static IFactory<IApiScopeDataIMP>? _apiScopeDataIMPFactory;

        public static IFactory<IApiScopeDataIMP> ApiScopeDataIMPFactory
        {
            set
            {
                _apiScopeDataIMPFactory = value;
            }
        }

        private class innerApiScopeDataIMPFactory : IFactory<IApiScopeDataIMP>
        {
            public IApiScopeDataIMP Create()
            {
                IApiScopeDataIMP imp;
                var di = ContextContainer.GetValue<IDIContainer>("DI");
                if (di == null)
                {
                    imp = DIContainerContainer.Get<IApiScopeDataIMP>();
                }
                else
                {
                    imp = di.Get<IApiScopeDataIMP>();
                }
                return imp;
            }
        }

        public override IFactory<IResourceDataIMP>? GetIMPFactory()
        {
            if (_apiScopeDataIMPFactory == null)
            {
                return new innerApiScopeDataIMPFactory();
            }
            else
            {
                return _apiScopeDataIMPFactory;
            }

        }

        public bool Required
        {
            get
            {

                return GetAttribute<bool>(nameof(Required));
            }
            set
            {
                SetAttribute<bool>(nameof(Required), value);
            }
        }

        public bool Emphasize
        {
            get
            {

                return GetAttribute<bool>(nameof(Emphasize));
            }
            set
            {
                SetAttribute<bool>(nameof(Emphasize), value);
            }
        }

        public async Task<ApiScope> GenerateApiScope(CancellationToken cancellationToken = default)
        {
            return await ((IApiScopeDataIMP)_imp).GenerateApiScope(this, cancellationToken);
        }
    }

    public interface IApiScopeDataIMP : IResourceDataIMP
    {
        Task<ApiScope> GenerateApiScope(ApiScopeData data, CancellationToken cancellationToken = default);
    }

    [Injection(InterfaceType = typeof(IApiScopeDataIMP), Scope = InjectionScope.Transient)]
    public class ApiScopeDataIMP : ResourceDataIMP, IApiScopeDataIMP
    {
        public async Task<ApiScope> GenerateApiScope(ApiScopeData data, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(new ApiScope(data.Name, data.DisplayName, data.UserClaims)
            {
                Description = data.Description,
                Enabled = data.Enabled,
                Properties = data.Properties,
                ShowInDiscoveryDocument = data.ShowInDiscoveryDocument,
                Required = data.Required,
                Emphasize = data.Emphasize,
            });
        }
    }
}
