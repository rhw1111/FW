using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Security.PrivilegeManagement.DAL
{
    /// <summary>
    /// 权限管理数据连接创建工厂
    /// </summary>
    public interface IPrivilegeManagementConnectionFactory
    {
        /// <summary>
        /// 创建权限管理的读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForPrivilegeManagement();
        /// <summary>
        /// 创建权限管理的只读连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateReadForPrivilegeManagement();
    }
}
