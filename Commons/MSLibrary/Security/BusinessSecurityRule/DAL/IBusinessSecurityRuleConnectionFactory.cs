using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Security.BusinessSecurityRule.DAL
{
    /// <summary>
    /// 业务安全规则的数据连接字符串工厂
    /// </summary>
    public interface IBusinessSecurityRuleConnectionFactory
    {
        /// <summary>
        /// 创建业务安全规则的读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForBusinessSecurityRule();
        /// <summary>
        /// 创建业务安全规则的只读连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateReadForBusinessSecurityRule();
    }
}
