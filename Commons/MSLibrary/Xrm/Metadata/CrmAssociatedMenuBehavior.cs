using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Metadata
{
    [DataContract]
    public enum CrmAssociatedMenuBehavior
    {
        UseCollectionName = 0,
        UseLabel = 1,
        DoNotDisplay = 2
    }
}
