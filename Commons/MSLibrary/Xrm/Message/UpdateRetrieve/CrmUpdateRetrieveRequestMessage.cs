using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.UpdateRetrieve
{
    [DataContract]
    public class CrmUpdateRetrieveRequestMessage:CrmRequestMessage
    {
        public CrmUpdateRetrieveRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.UpdateRetrieve;
        }

        /// <summary>
        /// 实体记录
        /// </summary>
        [DataMember]
        public CrmExecuteEntity Entity { get; set; }

        /// <summary>
        /// 要查询的属性集合
        /// </summary>
        [DataMember]
        public string[] Attributes
        {
            get; set;
        }

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
