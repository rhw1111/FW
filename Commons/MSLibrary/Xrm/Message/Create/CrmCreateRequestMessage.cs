using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.Create
{
    /// <summary>
    /// 创建实体记录请求
    /// </summary>
    [DataContract]
    public class CrmCreateRequestMessage:CrmRequestMessage
    {
        public CrmCreateRequestMessage():base()
        {
            MessageName = CrmRequestMessageExistsNames.Create;
        }
        /// <summary>
        /// 实体记录
        /// </summary>
        [DataMember]
        public CrmExecuteEntity Entity { get; set; }

    }
}
