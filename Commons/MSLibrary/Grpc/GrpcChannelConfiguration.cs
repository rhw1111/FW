using System;
using System.Collections.Generic;
using System.Text;
using Grpc.Core;
using MSLibrary.Grpc.Filters;

namespace MSLibrary.Grpc
{
    /// <summary>
    /// Grpc通道配置
    /// </summary>
    public class GrpcChannelConfiguration
    {
        /// <summary>
        /// 地址与端口
        /// 格式为：xxx:xxx
        /// </summary>
        public string Target { get; set; }
        /// <summary>
        /// 安全设置
        /// </summary>
        public ChannelCredentials ChannelCredentials { get;set;}
        /// <summary>
        /// 通道数量
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// 过滤器
        /// </summary>
        public IList<FilterBase> Filters { get; } = new List<FilterBase>();

    }
}
