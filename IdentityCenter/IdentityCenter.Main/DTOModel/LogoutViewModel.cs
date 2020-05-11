using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary;

namespace IdentityCenter.Main.DTOModel
{
    [DataContract]
    public class LogoutViewModel:ModelBase
    {
        [DataMember]
        public string IDToken
        {
            get
            {

                return GetAttribute<string>(nameof(IDToken));
            }
            set
            {
                SetAttribute<string>(nameof(IDToken), value);
            }
        }

        [DataMember]
        public string Binding
        {
            get
            {

                return GetAttribute<string>(nameof(Binding));
            }
            set
            {
                SetAttribute<string>(nameof(Binding), value);
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
