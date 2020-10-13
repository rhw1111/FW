using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.CommandLine
{
    public static class CommandLineTextCodes
    {
        /// <summary>
        /// 找不到指定类型的SSH终结点服务
        /// 格式为“找不到类型为{0}的SSH终结点服务，发生位置为{1}”
        /// {0}：终结点类型
        /// {1}：发生位置
        /// </summary>
        public static string NotFoundISSHEndpointServiceByType = "NotFoundISSHEndpointServiceByType";
        /// <summary>
        /// SSH执行操作超时
        /// 格式为“SSH执行操作超时”
        /// </summary>
        public static string SSHOperationTimeout = "SSHOperationTimeout";
        /// <summary>
        /// 上传文件错误
        /// 格式为“上传文件错误，服务地址为：{0}，文件路径：{1}，错误内容：{2}”
        /// {0}：上传文件服务地址
        /// {1}：文件保存路径
        /// {2}：错误内容
        /// </summary>
        public static string UploadFileError = "UploadFileError";
        /// <summary>
        /// 命令执行错误
        /// 格式为“命令执行错误，服务地址为：{0}，命令：{1}，错误内容：{2}”
        /// {0}：命令服务地址
        /// {1}：命令
        /// {2}：错误内容
        /// </summary>
        public static string CommandExecuteError = "CommandExecuteError";
    }
}
