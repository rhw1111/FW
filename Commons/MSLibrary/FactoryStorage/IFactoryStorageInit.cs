using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.FactoryStorage
{
    /// <summary>
    /// 工厂存储初始化接口
    /// </summary>
    public interface IFactoryStorageInit
    {
        /// <summary>
        /// 处理指定程序集名称集合的工厂
        /// </summary>
        /// <param name="assemblyNames">程序集名称集合</param>
        void Execute(params string[] assemblyNames);
    }
}
