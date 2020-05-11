using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.Upsert
{
    [DataContract]
    public class CrmUpsertRequestMessage:CrmRequestMessage
    {
        public CrmUpsertRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.Upsert;
        }

        /// <summary>
        /// 实体记录
        /// </summary>
        [DataMember]
        public CrmExecuteEntity Entity { get; set; }

        /// <summary>
        /// 唯一键集合
        /// </summary>
        [DataMember]
        public Dictionary<string, object> AlternateKeys { get; set; }
    }
}
