using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SystemToken.Application
{
    /// <summary>
    /// 应用层刷新通用令牌
    /// 更新传入的通用令牌字符串，返回新的通用令牌字符串
    /// </summary>
    public interface IAppRefreashCommonToken
    {
        Task<string> Do(string strToken);
    }
}
