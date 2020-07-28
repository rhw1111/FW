using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.CommandLine
{
    public enum CommandLineErrorCodes
    {
        /// <summary>
        /// 找不到指定类型的SSH终结点服务
        /// </summary>
        NotFoundISSHEndpointServiceByType = 314800301,
        /// <summary>
        /// SSH执行操作超时
        /// </summary>
        SSHOperationTimeout= 314800310
    }
}
