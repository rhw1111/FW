using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using MSLibrary.DI;

namespace MSLibrary.Context.Application
{
    /// <summary>
    /// 应用层用户授权
    /// 指定上下文生成器名称，
    /// 调用对应的生成器完成初始化
    /// </summary>
    public interface IAppUserAuthorize
    {
        Task<IAppUserAuthorizeResult> Do(IEnumerable<Claim> claims,string generatorName);
    }

    public interface IAppUserAuthorizeResult
    {
        void Execute();
    }



    
}
