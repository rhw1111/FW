using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm
{
    [DataContract]
    public enum CrmEntityKeyIndexStatus
    {
        Pending = 0,
        InProgress = 1,
        Active = 2,
        Failed = 3
    }
}
