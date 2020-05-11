using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Entity.DAL
{
    /// <summary>
    /// 实体属性映射配置连接字符串工厂
    /// </summary>
    public interface IEntityAttributeMapConfigurationConnectionFactory
    {
        /// <summary>
        /// 创建实体属性映射配置的读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForEntityAttributeMapConfiguration();
        /// <summary>
        /// 创建实体属性映射配置的只读连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateReadForEntityAttributeMapConfiguration();
    }
}
