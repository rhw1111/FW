using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Linq.Query
{
    /// <summary>
    /// 逻辑操作符
    /// </summary>
    [DataContract]
    public enum LogicalOperator
    {
        And=0,
        Or=1
    }
}
