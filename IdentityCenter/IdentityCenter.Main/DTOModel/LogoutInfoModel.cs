using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary;

namespace IdentityCenter.Main.DTOModel
{
    [DataContract]
    public class LogoutInfoModel:ModelBase
    {
        [DataMember]
        public string LoggedPage
        {
            get
            {

                return GetAttribute<string>(nameof(LoggedPage));
            }
            set
            {
                SetAttribute<string>(nameof(LoggedPage), value);
            }
        }

        [DataMember]
        public string ExternalLogoutCallbackUri
        {
            get
            {

                return GetAttribute<string>(nameof(ExternalLogoutCallbackUri));
            }
            set
            {
                SetAttribute<string>(nameof(ExternalLogoutCallbackUri), value);
            }
        }
    }
}
