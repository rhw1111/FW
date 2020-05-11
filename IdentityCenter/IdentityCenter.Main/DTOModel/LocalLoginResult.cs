using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary;

namespace IdentityCenter.Main.DTOModel
{
    [DataContract]
    public class LocalLoginResult:ModelBase
    {
        [DataMember]
        public string SubjectID
        {
            get
            {

                return GetAttribute<string>(nameof(SubjectID));
            }
            set
            {
                SetAttribute<string>(nameof(SubjectID), value);
            }
        }

        [DataMember]
        public string UserName
        {
            get
            {

                return GetAttribute<string>(nameof(UserName));
            }
            set
            {
                SetAttribute<string>(nameof(UserName), value);
            }
        }

        [DataMember]
        public bool RememberLogin
        {
            get
            {

                return GetAttribute<bool>(nameof(RememberLogin));
            }
            set
            {
                SetAttribute<bool>(nameof(RememberLogin), value);
            }
        }

        [DataMember]
        public int RememberMeLoginDuration
        {
            get
            {

                return GetAttribute<int>(nameof(RememberMeLoginDuration));
            }
            set
            {
                SetAttribute<int>(nameof(RememberMeLoginDuration), value);
            }
        }

        

    }
}
