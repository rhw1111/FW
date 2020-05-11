using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MSLibrary.Logger
{
    /// <summary>
    /// 通用日志信息http处理服务
    /// 供CommonlogInfoHandler中间件使用
    /// 处理诸如父日志上下文信息的解析等任务
    /// </summary>
    public interface ICommonLogInfoHttpHandleService
    {
        /// <summary>
        /// 执行处理
        /// </summary>
        /// <param name="context">Http上下文</param>
        /// <returns></returns>
        Task<bool> Execute(HttpContext context);
    }
}
