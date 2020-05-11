using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.GetFileAttributeUploadInfo
{
    [DataContract]
    public class CrmGetFileAttributeUploadInfoRequestMessage : CrmRequestMessage
    {
        public CrmGetFileAttributeUploadInfoRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.GetFileAttributeUploadInfo;
        }

        /// <summary>
        /// 实体名称
        /// </summary>
        [DataMember]
        public string EntityName { get; set; }
        /// <summary>
        /// 实体Id
        /// </summary>
        [DataMember]
        public Guid EntityId { get; set; }

        /// <summary>
        /// 文件类型的属性名称
        /// </summary>
        [DataMember]
        public string AttributeName { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        [DataMember]
        public string FileName { get; set; }
    }
}
