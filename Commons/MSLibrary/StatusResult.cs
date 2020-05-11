using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary
{
    /// <summary>
    /// 状态结果
    /// 适用于返回结果有多种状态的动作
    /// </summary>
    [DataContract]
    public class StatusResult
    {
        /// <summary>
        /// 状态值
        /// 0:成功，1:未到执行时间，2：失败，3:移除到死队列
        /// </summary>
        [DataMember]
        public int Status { get; set; }
        /// <summary>
        /// 结果描述
        /// </summary>
        [DataMember]
        public string Description { get; set; }
       

    }
}
