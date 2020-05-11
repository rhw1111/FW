using System;
using System.Collections.Generic;
using System.Text;
using Grpc.Core.Interceptors;

namespace MSLibrary.Grpc.Filters
{
    /// <summary>
    /// 覆盖过滤器
    /// 覆盖之前存在的所有过滤器
    /// 可以指定要覆盖的过滤器的类型名称和类型
    /// 如果两者都指定，则取交集
    /// </summary>
    public class OverrideFilter : FilterBase
    {
        public OverrideFilter(string overrideTypeName, Type overrideType) : base(GrpcFilterTypes.Override)
        {
            OverrideTypeName = overrideTypeName;
            OverrideType = overrideType;
        }
        public override Interceptor Interceptor => null;

        /// <summary>
        /// 要覆盖的过滤器的类型名称
        /// </summary>
        public string OverrideTypeName { get; private set; }
        /// <summary>
        /// 要覆盖的过滤器的类型
        /// </summary>
        public Type OverrideType { get; private set; }
    }
}
