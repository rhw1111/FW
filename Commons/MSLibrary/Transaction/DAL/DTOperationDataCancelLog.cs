using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Transaction.DAL
{
    /// <summary>
    /// 分布式操作数据执行Cancel后的日志
    /// 用来控制数据的Cancel动作不会重复执行
    /// </summary>
    public class DTOperationDataCancelLog
    {
        public Guid DataID { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
