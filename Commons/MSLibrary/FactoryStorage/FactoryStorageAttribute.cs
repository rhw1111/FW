using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.FactoryStorage
{
    /// <summary>
    /// 工厂存储特性
    /// 增加特性的类将被工厂存储自动初始化工具捕获
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class FactoryStorageAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
