using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.CreateRetrieve
{
    /// <summary>
    /// 创建实体记录并返回查询的请求
    /// </summary>
    [DataContract]
    public class CrmCreateRetrieveRequestMessage:CrmRequestMessage
    {
        public CrmCreateRetrieveRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.CreateRetrieve;
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
            get;set;
        }
    }
}
