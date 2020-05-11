using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace MSLibrary.DI
{
    /// <summary>
    /// 拦截器处理上下文
    /// </summary>
    public interface IInterceptorItemContext
    {
        /// <summary>
        /// 拦截的方法传入的参数列表
        /// </summary>
        object[] Arguments { get; }
        /// <summary>
        /// 拦截的方法元数据
        /// </summary>
        MethodInfo Method { get; }
        /// <summary>
        /// 方法执行后的返回结果
        /// </summary>
        object ReturnValue { get; set; }
        /// <summary>
        /// 执行下一步
        /// </summary>
        void Next();
    }
}
