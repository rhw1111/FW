using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.FileAttributeDownloadChunking
{
    [DataContract]
    public class CrmFileAttributeDownloadChunkingRequestMessage: CrmRequestMessage
    {
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
        /// 本次下载数据所处的起始位置
        /// </summary>
        [DataMember]
        public long Start { get; set; }
        /// <summary>
        /// 本次下载数据所处的结束位置
        /// </summary>
        [DataMember]
        public long End { get; set; }


    }
}
