using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary;

namespace IdentityCenter.Main.DTOModel
{
    [DataContract]
    public class LoggedOutViewModel:ModelBase
    {
        /// <summary>
        /// 登出后重定向的地址
        /// </summary>
        [DataMember]
        public string PostLogoutRedirectUri
        {
            get
            {

                return GetAttribute<string>(nameof(PostLogoutRedirectUri));
            }
            set
            {
                SetAttribute<string>(nameof(PostLogoutRedirectUri), value);
            }
        }

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

        /// <summary>
        /// 用于单点退出的IFrame地址
        /// </summary>
        [DataMember]
        public string SignOutIframeUrl
        {
            get
            {

                return GetAttribute<string>(nameof(SignOutIframeUrl));
            }
            set
            {
                SetAttribute<string>(nameof(SignOutIframeUrl), value);
            }
        }

        [DataMember]
        public string LogoutId
        {
            get
            {

                return GetAttribute<string>(nameof(LogoutId));
            }
            set
            {
                SetAttribute<string>(nameof(LogoutId), value);
            }
        }

    }
}
