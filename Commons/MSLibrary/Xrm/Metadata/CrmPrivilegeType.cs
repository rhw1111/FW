using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Metadata
{
    [DataContract]
    public enum CrmPrivilegeType
    {
        None = 0,
        Create = 1,
        Read = 2,
        Write = 3,
        Delete = 4,
        Assign = 5,
        Share = 6,
        Append = 7,
        AppendTo = 8
    }
}
