using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Grpc.Interceptors
{
    /// <summary>
    /// 拦截器描述
    /// 用于创建拦截器实例
    /// </summary>
    public class InterceptorDescription
    {
        public InterceptorDescription(Type interceptorType, object[] arguments, Type[] argumentType)
        {
            InterceptorType = interceptorType;
            Arguments = arguments;
            ArgumentTypes = argumentType;
        }
        public Type InterceptorType { get; private set; } = null!;
        public object[] Arguments { get; private set; } = null!;
        public Type[] ArgumentTypes { get; private set; } = null!;
    }
}
