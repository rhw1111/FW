using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary;

namespace IdentityCenter.Main.DTOModel
{
    /// <summary>
    /// 授权确认视图
    /// </summary>
    [DataContract]
    public class ConsentViewModel:ModelBase
    {
        [DataMember]
        public string ClientName
        {
            get
            {

                return GetAttribute<string>(nameof(ClientName));
            }
            set
            {
                SetAttribute<string>(nameof(ClientName), value);
            }
        }

        [DataMember]
        public string ClientUrl
        {
            get
            {

                return GetAttribute<string>(nameof(ClientUrl));
            }
            set
            {
                SetAttribute<string>(nameof(ClientUrl), value);
            }
        }

        [DataMember]
        public string ClientLogoUrl
        {
            get
            {

                return GetAttribute<string>(nameof(ClientLogoUrl));
            }
            set
            {
                SetAttribute<string>(nameof(ClientLogoUrl), value);
            }
        }

        [DataMember]
        public bool AllowRememberConsent
        {
            get
            {

                return GetAttribute<bool>(nameof(AllowRememberConsent));
            }
            set
            {
                SetAttribute<bool>(nameof(AllowRememberConsent), value);
            }
        }
        [DataMember]
        public List<ScopeViewModel> IdentityScopes
        {
            get
            {

                return GetAttribute<List<ScopeViewModel>>(nameof(IdentityScopes));
            }
            set
            {
                SetAttribute<List<ScopeViewModel>>(nameof(IdentityScopes), value);
            }
        }
        [DataMember]
        public List<string> ResourceScopes
        {
            get
            {

                return GetAttribute<List<string>>(nameof(ResourceScopes));
            }
            set
            {
                SetAttribute<List<string>>(nameof(ResourceScopes), value);
            }

        }
    }
}
