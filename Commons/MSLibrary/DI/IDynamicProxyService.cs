using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.DI
{
    /// <summary>
    /// 动态代理服务的接口
    /// </summary>
    public interface IDynamicProxyService
    {
        /// <summary>
        /// 生成代理类
        /// </summary>
        /// <typeparam name="T">代理类类型</typeparam>
        /// <param name="target">实际对象</param>
        /// <param name="interceptorTypes">拦截器类型列表（类型必须继承IInterceptorItem）</param>
        /// <returns></returns>
        T CreateAopProxy<T>(T target,Type[] interceptorTypes);
    }
}
