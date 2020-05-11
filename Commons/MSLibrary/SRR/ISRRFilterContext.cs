using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Claims;

namespace MSLibrary.SRR
{
    /// <summary>
    /// 请求响应过滤器上下文
    /// </summary>
    public interface ISRRFilterContext
    {
        SRRRequest Request { get; }

        ClaimsPrincipal Identity { get; set; }

        IDictionary<string, object> Items { get; }

        SRRResponse Response { get; }
    }
}
