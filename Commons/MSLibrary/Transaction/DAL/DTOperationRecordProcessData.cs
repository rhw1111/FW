using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Transaction.DAL
{
    /// <summary>
    /// 分布式操作记录过程数据
    /// </summary>
    public class DTOperationRecordProcessData
    {
        /// <summary>
        /// 所属的分布式操作记录的唯一名称
        /// </summary>
        public string RecordUniqueName { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
