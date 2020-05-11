using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.Delete
{
    [DataContract]
    public class CrmDeleteResponseMessage : CrmResponseMessage
    {
        public CrmDeleteResponseMessage()
        {
        }
    }
}
