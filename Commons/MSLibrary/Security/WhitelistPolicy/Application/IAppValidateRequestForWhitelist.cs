using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Security.WhitelistPolicy.Application
{
    /// <summary>
    /// 验证传入的请求是否符合白名单策略
    /// </summary>
    public interface IAppValidateRequestForWhitelist
    {
        Task<ValidateResult> Do(string systemOperationName,string systemName,string strToken,string ip);
    }
}
