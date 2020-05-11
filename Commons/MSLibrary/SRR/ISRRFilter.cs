using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SRR
{
    /// <summary>
    /// 消息请求响应过滤器
    /// </summary>
    public interface ISRRFilter
    {
        Task ExecutePre(ISRRFilterContext context);

        void ExecutePreSync(ISRRFilterContext context);

        Task ExecutePost(ISRRFilterContext context);

        void ExecutePostSync(ISRRFilterContext context);

        Task Finally(ISRRFilterContext context);
        void FinallySync(ISRRFilterContext context);

    }

    /// <summary>
    /// 标记接口，表示实现这个接口之前的所有过滤器都不执行
    /// </summary>
    public interface ISRROverrideFilter
    {

    }
}
