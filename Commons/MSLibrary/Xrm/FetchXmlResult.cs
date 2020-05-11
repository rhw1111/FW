using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm
{
    [DataContract]
    public class FetchXmlResult
    {
        /// <summary>
        /// 下一页查询信息
        /// </summary>
        [DataMember]
        public string PagingCookie { get; set; }
        /// <summary>
        /// 结果集
        /// </summary>
        [DataMember]
        public IEnumerable<CrmEntity> Results { get; set; }
    }
}
