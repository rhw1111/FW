using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary;

namespace IdentityCenter.Main.DTOModel
{
    [DataContract]
    public class ConsentInputModel:ModelBase
    {
        [DataMember]
        public bool RememberConsent
        {
            get
            {

                return GetAttribute<bool>(nameof(RememberConsent));
            }
            set
            {
                SetAttribute<bool>(nameof(RememberConsent), value);
            }
        }

        [DataMember]
        public bool Accept
        {
            get
            {

                return GetAttribute<bool>(nameof(Accept));
            }
            set
            {
                SetAttribute<bool>(nameof(Accept), value);
            }
        }

        [DataMember]
        public string ReturnUrl
        {
            get
            {

                return GetAttribute<string>(nameof(ReturnUrl));
            }
            set
            {
                SetAttribute<string>(nameof(ReturnUrl), value);
            }
        }

    }
}
