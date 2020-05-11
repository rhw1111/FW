using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm.Message.GetFileAttributeUploadInfo
{
    [DataContract]
    public class CrmGetFileAttributeUploadInfoResponseMessage: CrmResponseMessage
    {
        /// <summary>
        /// /上传地址
        /// </summary>
        [DataMember]
        public string UploadUrl { get; set; }
        /// <summary>
        /// 每次上传接受的最大字节数
        /// </summary>
        [DataMember]
        public int PerSize { get; set; }
    }
}
