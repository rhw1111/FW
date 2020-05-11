using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Metadata
{
    [DataContract]
    public enum CrmSecurityTypes
    {
        None = 0,
        Append = 1,
        ParentChild = 2,
        Pointer = 4,
        Inheritance = 8
    }
}
