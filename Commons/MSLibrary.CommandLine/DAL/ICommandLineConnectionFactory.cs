using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.CommandLine.SSH.DAL
{
    /// <summary>
    /// 与命令行相关的连接字符串工厂
    /// </summary>
    public interface ICommandLineConnectionFactory
    {
        /// <summary>
        /// 创建有关命令行的读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForCommandLine();
        /// <summary>
        /// 创建有关命令行的只读连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateReadForCommandLine();
    }
}
