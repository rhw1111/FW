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
        SSHOperationTimeout= 314800310,
        /// <summary>
        /// 上传文件错误
        /// </summary>
        UploadFileError= 314800320,
        /// <summary>
        /// 命令执行错误
        /// </summary>
        CommandExecuteError= 314800321,

    }
}
