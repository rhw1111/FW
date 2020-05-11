using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.UpsertRetrieve
{
    [DataContract]
    public class CrmUpsertRetrieveRequestMessage:CrmRequestMessage
    {
        public CrmUpsertRetrieveRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.UpsertRetrieve;
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
        /// 唯一键集合
        /// </summary>
        [DataMember]
        public Dictionary<string, object> AlternateKeys { get; set; }

    }
}
