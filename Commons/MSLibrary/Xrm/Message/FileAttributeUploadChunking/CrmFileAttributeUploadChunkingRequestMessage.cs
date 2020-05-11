using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.FileAttributeUploadChunking
{
    [DataContract]
    public class CrmFileAttributeUploadChunkingRequestMessage: CrmRequestMessage
    {
        public CrmFileAttributeUploadChunkingRequestMessage() : base()
        {
            MessageName = CrmRequestMessageExistsNames.FileAttributeUploadChunking;
        }

        /// <summary>
        /// 上传文件的Url
        /// </summary>
        [DataMember]
        public string UploadUrl { get; set; }
        /// <summary>
        /// 本次上传数据所处的起始位置
        /// </summary>
        [DataMember]
        public long Start { get; set; }
        /// <summary>
        /// 本次上传数据所处的结束位置
        /// </summary>
        [DataMember]
        public long End { get; set; }
        /// <summary>
        /// 文件的总大小
        /// </summary>
        [DataMember]
        public long Total { get; set; }
        /// <summary>
        /// 本次上传的数据
        /// </summary>
        [DataMember]
        public byte[] Data { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        [DataMember]
        public string FileName { get; set; }
    }
}
