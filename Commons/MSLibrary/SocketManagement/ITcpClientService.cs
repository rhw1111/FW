using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SocketManagement
{
    /// <summary>
    /// Tcp客户端服务
    /// 应用程序根据该服务与服务端通信
    /// 该服务管理应用程序所有要使用的Tcp客户端终结点
    /// </summary>
    public interface ITcpClientService
    {
        /// <summary>
        /// 使用指定名称的终结点发送信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<byte[]> Send(string endpointName,byte[] data);
        /// <summary>
        /// 清理资源
        /// </summary>
        /// <returns></returns>
        Task Dispose();
    }
}
