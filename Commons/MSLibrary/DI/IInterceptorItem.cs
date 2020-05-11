using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.DI
{
    /// <summary>
    /// Aop拦截器接口
    /// </summary>
    public interface IInterceptorItem
    {
        /// <summary>
        /// 执行拦截
        /// </summary>
        /// <param name="context">拦截上下文</param>
        /// <returns></returns>
        Task Execute(IInterceptorItemContext context);
    }
}
