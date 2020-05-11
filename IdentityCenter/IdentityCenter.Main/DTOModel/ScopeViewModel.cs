using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary;

namespace IdentityCenter.Main.DTOModel
{
    [DataContract]
    public class ScopeViewModel:ModelBase
    {
        [DataMember]
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

        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
    }
}
