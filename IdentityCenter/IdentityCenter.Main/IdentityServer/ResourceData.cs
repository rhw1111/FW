using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace IdentityCenter.Main.IdentityServer
{
    public class ResourceData : EntityBase<IResourceDataIMP>
    {
        private static IFactory<IResourceDataIMP>? _resourceDataIMPFactory;

        public static IFactory<IResourceDataIMP> ResourceDataIMPFactory
        {
            set
            {
                _resourceDataIMPFactory = value;
            }
        }
        public override IFactory<IResourceDataIMP>? GetIMPFactory()
        {
            return _resourceDataIMPFactory;
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

        public string DisplayName
        {
            get
            {

                return GetAttribute<string>(nameof(DisplayName));
            }
            set
            {
                SetAttribute<string>(nameof(DisplayName), value);
            }
        }

        public bool ShowInDiscoveryDocument
        {
            get
            {

                return GetAttribute<bool>(nameof(ShowInDiscoveryDocument));
            }
            set
            {
                SetAttribute<bool>(nameof(ShowInDiscoveryDocument), value);
            }
        }

        public string Description
        {
            get
            {

                return GetAttribute<string>(nameof(Description));
            }
            set
            {
                SetAttribute<string>(nameof(Description), value);
            }
        }

        public List<string> UserClaims
        {
            get
            {

                return GetAttribute<List<string>>(nameof(UserClaims));
            }
            set
            {
                SetAttribute<List<string>>(nameof(UserClaims), value);
            }
        }


        public Dictionary<string,string> Properties
        {
            get
            {

                return GetAttribute<Dictionary<string, string>>(nameof(Properties));
            }
            set
            {
                SetAttribute<Dictionary<string, string>>(nameof(Properties), value);
            }
        }

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
        
    }

    public interface IResourceDataIMP
    {

    }

    [Injection(InterfaceType = typeof(IResourceDataIMP), Scope = InjectionScope.Transient)]
    public class ResourceDataIMP: IResourceDataIMP
    {

    }
}
