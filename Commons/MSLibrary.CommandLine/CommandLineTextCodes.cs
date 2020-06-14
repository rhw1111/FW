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
    }
}
