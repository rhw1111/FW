using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace MSLibrary.SRR
{
    /// <summary>
    /// 消息请求响应管道上下文
    /// </summary>
    public class SRRPipeContext:ISRRPipeContext
    {
        /// <summary>
        /// 请求源类型
        /// </summary>
        public string RequestSourceType { get; set; }
        /// <summary>
        /// 请求源参数键值对
        /// </summary>
        public IDictionary<string, object> RequestSourceParameters { get;} = new Dictionary<string, object>();
        public ClaimsPrincipal Identity { get; set; }

        public IDictionary<string, object> Items { get; }=new Dictionary<string,object>();
    }
}
