using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.UpdateRetrieve
{
    [DataContract]
    public class CrmUpdateRetrieveResponseMessage:CrmResponseMessage
    {
        /// <summary>
        /// 查询的实体记录
        /// </summary>
        [DataMember]
        public CrmEntity Entity { get; set; }
    }
}
