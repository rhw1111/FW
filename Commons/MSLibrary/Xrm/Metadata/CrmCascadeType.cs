using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Metadata
{
    [DataContract]
    public enum CrmCascadeType
    {
        NoCascade = 0,
        Cascade = 1,
        Active = 2,
        UserOwned = 3,
        RemoveLink = 4,
        Restrict = 5
    }
}
