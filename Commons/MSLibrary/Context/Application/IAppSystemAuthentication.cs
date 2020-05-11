using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MSLibrary.Context.Application
{
    /// <summary>
    /// 应用层执行系统身份验证
    /// </summary>
    public interface IAppSystemAuthentication
    {
        /// <summary>
        /// 根据生成器名称执行系统身份验证
        /// </summary>
        /// <param name="httpContext">http上下文</param>
        /// <param name="generatorName">生成器名称</param>
        /// <returns></returns>
        Task<ClaimsIdentity> Do(HttpContext httpContext, string generatorName);
    }
}
