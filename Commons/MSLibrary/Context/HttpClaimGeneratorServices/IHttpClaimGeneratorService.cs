using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;


namespace MSLibrary.Context.HttpClaimGeneratorServices
{
    /// <summary>
    /// Http声明生成服务接口
    /// </summary>
    public interface IHttpClaimGeneratorService
    {
        /// <summary>
        /// 根据Http上下文生成声明
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        Task<ClaimsIdentity> Do(HttpContext httpContext);
    }
}
