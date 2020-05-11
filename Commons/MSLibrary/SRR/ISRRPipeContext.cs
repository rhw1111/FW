using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Claims;

namespace MSLibrary.SRR
{
    /// <summary>
    /// 消息请求响应管道上下文接口
    /// </summary>
    public interface ISRRPipeContext
    {
        /// <summary>
        /// 请求源类型
        /// </summary>
        string RequestSourceType { get; set; }
        /// <summary>
        /// 请求源参数键值对
        /// </summary>
        IDictionary<string, object> RequestSourceParameters { get;}
        /// <summary>
        /// 身份声明
        /// </summary>
        ClaimsPrincipal Identity { get; set; }
        /// <summary>
        /// 附加键值对
        /// </summary>
        IDictionary<string, object> Items { get; }
    }
}
