using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.Grpc.Filters;

namespace MSLibrary.Grpc
{
    /// <summary>
    /// Grpc宿主配置
    /// </summary>
    public class GrpcHostConfiguration
    {
        /// <summary>
        /// 服务列表
        /// </summary>
        public IDictionary<Type, GrpcServiceDescription> Services { get; set; } = new Dictionary<Type, GrpcServiceDescription>();
        /// <summary>
        /// 服务端口列表
        /// </summary>
        public IList<GrpcPortDescription> Ports { get; set; } = new List<GrpcPortDescription>();
        /// <summary>
        /// 全局过滤器列表
        /// </summary>
        public IList<FilterBase> GlobalFilters { get; set; } = new List<FilterBase>();
    }
}
