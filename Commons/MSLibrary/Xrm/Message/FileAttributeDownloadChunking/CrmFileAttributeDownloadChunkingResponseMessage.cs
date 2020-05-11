using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MSLibrary.Xrm.Message.FileAttributeDownloadChunking
{
    [DataContract]
    public class CrmFileAttributeDownloadChunkingResponseMessage: CrmResponseMessage
    {
        /// <summary>
        /// 文件的总大小
        /// </summary>
        [DataMember]
        public int Total { get; set; }
        /// <summary>
        /// 下载的文件块字节数组
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
