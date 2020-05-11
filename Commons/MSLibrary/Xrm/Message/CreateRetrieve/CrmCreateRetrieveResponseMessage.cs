using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.CreateRetrieve
{
    /// <summary>
    /// 创建实体记录并返回查询的响应
    /// </summary>
    [DataContract]
    public class CrmCreateRetrieveResponseMessage: CrmResponseMessage
    {
        /// <summary>
        /// 查询的实体记录
        /// </summary>
        [DataMember]
        public CrmEntity Entity { get; set; }
    }
}
