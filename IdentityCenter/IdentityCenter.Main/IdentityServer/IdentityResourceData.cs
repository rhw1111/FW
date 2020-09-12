using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Models;
using MSLibrary;
using MSLibrary.DI;

namespace IdentityCenter.Main.IdentityServer
{
    public class IdentityResourceData:ResourceData
    {
        private static IFactory<IIdentityResourceDataIMP>? _identityResourceDataIMPFactory;

        public static IFactory<IIdentityResourceDataIMP>  IdentityResourceDataIMPFactory
        {
            set
            {
                _identityResourceDataIMPFactory = value;
            }
        }

        private class innerIdentityResourceDataIMPFactory : IFactory<IIdentityResourceDataIMP>
        {
            public IIdentityResourceDataIMP Create()
            {
                IIdentityResourceDataIMP imp;
                var di = ContextContainer.GetValue<IDIContainer>(ContextTypes.DI);
                if (di == null)
                {
                    imp = DIContainerContainer.Get<IIdentityResourceDataIMP>();
                }
                else
                {
                    imp = di.Get<IIdentityResourceDataIMP>();
                }
                return imp;
            }
        }

        public override IFactory<IResourceDataIMP>? GetIMPFactory()
        {
            if (_identityResourceDataIMPFactory==null)
            {
                return new innerIdentityResourceDataIMPFactory();
            }
            else
            {
                return _identityResourceDataIMPFactory;
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


        public async Task<IdentityResource> GenerateIdentityResource(CancellationToken cancellationToken = default)
        {
            return await ((IIdentityResourceDataIMP)_imp).GenerateIdentityResource(this,cancellationToken);
        }
    }

    public interface IIdentityResourceDataIMP:IResourceDataIMP
    {
        Task<IdentityResource> GenerateIdentityResource(IdentityResourceData data, CancellationToken cancellationToken = default);
    }

    [Injection(InterfaceType = typeof(IIdentityResourceDataIMP), Scope = InjectionScope.Transient)]
    public class IdentityResourceDataIMP : ResourceDataIMP, IIdentityResourceDataIMP
    {
        public async Task<IdentityResource> GenerateIdentityResource(IdentityResourceData data, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(new IdentityResource(data.Name, data.DisplayName, data.UserClaims)
            {
                Emphasize = data.Emphasize,
                Required = data.Required,
                ShowInDiscoveryDocument = data.ShowInDiscoveryDocument,
                Description = data.Description,
                Enabled = data.Enabled,
                Properties = data.Properties,
                UserClaims = data.UserClaims
            });
        }
    }
}
