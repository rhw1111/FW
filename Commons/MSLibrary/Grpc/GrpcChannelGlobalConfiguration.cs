using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.Grpc.Filters;

namespace MSLibrary.Grpc
{
    /// <summary>
    /// Grpc通道全局配置
    /// </summary>
    public class GrpcChannelGlobalConfiguration
    {
        /// <summary>
        /// 全局过滤器
        /// </summary>
        public IList<FilterBase> GlobalFilters { get; } = new List<FilterBase>();

    }
}
