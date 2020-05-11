using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.Grpc.Filters;

namespace MSLibrary.Grpc
{
    /// <summary>
    /// Grpc服务描述
    /// </summary>
    public class GrpcServiceDescription
    {
        /// <summary>
        /// 命名空间类型
        /// </summary>
        public Type NamespaceType { get; set; }
        /// <summary>
        /// 服务类型
        /// </summary>
        public Type ServiceType { get; set; }
        /// <summary>
        /// 过滤器集合
        /// </summary>
        public IList<FilterBase> Filters { get; set; } = new List<FilterBase>();
    }
}
