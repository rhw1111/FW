using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Metadata
{
    [DataContract]
    public enum CrmAssociatedMenuGroup
    {
        Details = 0,
        Sales = 1,
        Service = 2,
        Marketing = 3
    }
}
