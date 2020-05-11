using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.Create
{
    /// <summary>
    /// 创建实体记录响应
    /// </summary>
    [DataContract]
    public class CrmCreateResponseMessage:CrmResponseMessage
    {
        /// <summary>
        /// 实体记录Id
        /// </summary>
        [DataMember]
        public Guid Id { get; set; }
    }
}
