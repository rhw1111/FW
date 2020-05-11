using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.DI
{
    /// <summary>
    /// 依赖注入容器初始化处理接口
    /// </summary>
    public interface IDIContainerInit
    {
        /// <summary>
        /// 加载指定程序集中的类
        /// </summary>
        /// <param name="assemblyNames">程序集名称集合</param>
        void Execute(params string[] assemblyNames);
    }
}
