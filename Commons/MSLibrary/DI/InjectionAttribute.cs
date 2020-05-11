using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.DI
{
    /// <summary>
    /// 注入类型特性
    /// 增加该特性的类将被DI容器自动初始化工具捕获
    /// 以类型方式注入容器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited =false)]
    public class InjectionAttribute : Attribute
    {
        /// <summary>
        /// 要注入的接口类型
        /// </summary>
        public Type InterfaceType { get; set; }
        /// <summary>
        /// 注入范围
        /// </summary>
        public InjectionScope Scope { get; set; }
    }
}
