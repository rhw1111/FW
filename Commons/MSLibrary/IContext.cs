using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary
{
    /// <summary>
    /// 系统上下文接口，
    /// 所有系统中的上下文类都必须实现该接口
    /// </summary>
    public interface IContext<T>
    {
        /// <summary>
        /// 上下文值属性
        /// </summary>
        T Value { get; set; }
        /// <summary>
        /// 检查上下文是否是自动生成的
        /// </summary>
        /// <returns></returns>
        bool IsAuto();
    }
}
