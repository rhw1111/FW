using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Xrm
{
    /// <summary>
    /// Crm实体集合
    /// </summary>
    [DataContract]
    public class CrmEntityCollection
    {
        /// <summary>
        /// 总数量（有限制）
        /// </summary>
        [DataMember]
        public int Count { get; set; }
        /// <summary>
        /// 结果集
        /// </summary>
        [DataMember]
        public List<CrmEntity> Results { get; set; }
        /// <summary>
        /// 下一页查询表达式
        /// </summary>
        [DataMember]
        public string NextLinkExpression { get; set; }
        /// <summary>
        /// 是否还有剩余的记录
        /// </summary>
        [DataMember]
        public bool MoreRecords { get; set; }
        /// <summary>
        /// 分页cookie
        /// </summary>

        [DataMember]
        public string PagingCookie { get; set; }
    }
}
