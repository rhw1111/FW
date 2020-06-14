using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.Configuration.DAL;
using MSLibrary.Logger.DAL;

namespace FW.TestPlatform.Main.DAL
{
    /// <summary>
    /// 主连接字符串工厂
    /// </summary>
    public interface IMainDBConnectionFactory:ISystemConfigurationConnectionFactory,ICommonLogConnectionFactory
    {
        /// <summary>
        /// 创建主读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForMain();
        /// <summary>
        /// 创建主只读连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateReadForMain();
    }
}
