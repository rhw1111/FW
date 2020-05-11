using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.Update
{
    /// <summary>
    /// 修改实体记录请求
    /// </summary>
    [DataContract]
    public class CrmUpdateRequestMessage:CrmRequestMessage
    {
        public CrmUpdateRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.Update;
        }

        /// <summary>
        /// 实体记录
        /// </summary>
        [DataMember]
        public CrmExecuteEntity Entity { get; set; }
        /// <summary>
        /// 记录版本号
        /// </summary>
        [DataMember]
        public string Version { get; set; }
        /// <summary>
        /// 唯一键集合
        /// </summary>
        [DataMember]
        public Dictionary<string, object> AlternateKeys { get; set; }
    }
}
