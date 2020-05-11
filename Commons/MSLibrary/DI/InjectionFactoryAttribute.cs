using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.DI
{
    /// <summary>
    /// 注入工厂特性
    /// 增加该特性的类将被DI容器自动初始化工具捕获
    /// 以工厂方式注入容器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class InjectionFactoryAttribute : Attribute
    {
        /// <summary>
        /// 要注入的接口类型
        /// </summary>
        public Type InterfaceType { get; set; }
        /// <summary>
        /// 工厂名称
        /// 初始化工具将从FactoryStorage中根据名称获取工厂
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 注入范围
        /// </summary>
        public InjectionScope Scope { get; set; }
    }
}
