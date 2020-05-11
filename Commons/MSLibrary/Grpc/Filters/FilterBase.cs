using System;
using System.Collections.Generic;
using System.Text;
using Grpc.Core.Interceptors;

namespace MSLibrary.Grpc.Filters
{
    /// <summary>
    /// 过滤器基类
    /// </summary>
    public abstract class FilterBase : Attribute
    {
        public FilterBase(string type)
        {
            Type = type;
        }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// 包含的拦截器
        /// 需要子类实现该属性
        /// </summary>
        public abstract Interceptor Interceptor { get; }


    }
}
