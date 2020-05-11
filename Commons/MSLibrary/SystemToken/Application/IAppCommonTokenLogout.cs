using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SystemToken.Application
{
    /// <summary>
    /// 应用层通用令牌登出
    /// 用于登出时处理后台的登出
    /// </summary>
    public interface IAppCommonTokenLogout
    {
        Task Do(List<string> strTokenList);
    }
}
