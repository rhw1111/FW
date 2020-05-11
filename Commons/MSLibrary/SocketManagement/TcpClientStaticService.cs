using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SocketManagement
{
    /// <summary>
    /// Tcp客户端静态服务
    /// 整个应用程序都通过该服务来进行Tcp通信
    /// </summary>
    public static class TcpClientStaticService
    {
        private static ITcpClientService _tcpClientService;

        public static ITcpClientService TcpClientService
        {
            set
            {
                _tcpClientService = value;
            }
        }

        /// <summary>
        /// 使用指定名称的终结点发送信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<byte[]> Send(string endpointName, byte[] data)
        {
            return await _tcpClientService.Send(endpointName,data);
        }
        /// <summary>
        /// 清理资源
        /// </summary>
        /// <returns></returns>
        public static async Task Dispose()
        {
            await _tcpClientService.Dispose();
        }
    }
}
