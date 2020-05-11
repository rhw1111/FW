using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace MSLibrary.Context.EnvironmentClaimGeneratorServices
{
    /// <summary>
    /// 环境声明生成服务
    /// </summary>
    public interface IEnvironmentClaimGeneratorService
    {
        /// 生成声明
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        Task<ClaimsIdentity> Do();
    }
}
