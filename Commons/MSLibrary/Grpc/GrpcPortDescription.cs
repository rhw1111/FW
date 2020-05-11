using System;
using System.Collections.Generic;
using System.Text;
using Grpc.Core;

namespace MSLibrary.Grpc
{
    /// <summary>
    /// Grpc端口描述
    /// </summary>
    public class GrpcPortDescription
    {
        /// <summary>
        /// 主机地址
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 安全策略
        /// </summary>
        public ServerCredentials Credential { get; set; }
    }
}
