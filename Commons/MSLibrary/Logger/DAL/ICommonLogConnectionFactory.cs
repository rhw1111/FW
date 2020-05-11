using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DAL;

namespace MSLibrary.Logger.DAL
{
    /// <summary>
    /// 通用日志连接字符串工厂
    /// </summary>
    public interface ICommonLogConnectionFactory
    {
        string CreateAllForLocalCommonLog();
        string CreateReadForLocalCommonLog();
    }
    
}
