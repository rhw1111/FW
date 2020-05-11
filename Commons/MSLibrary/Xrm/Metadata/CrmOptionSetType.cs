using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Metadata
{
    [DataContract]
    public enum CrmOptionSetType
    {

        Picklist = 0,

        State = 1,

        Status = 2,

        Boolean = 3
    }
}
