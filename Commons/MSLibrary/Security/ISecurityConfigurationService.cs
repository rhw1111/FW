using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Security
{
    /// <summary>
    /// 安全服务所使用的配置服务
    /// </summary>
    public interface ISecurityConfigurationService
    {
        /// <summary>
        /// 获取对称密钥的Key
        /// </summary>
        /// <returns></returns>
        string GetEncryptKey();
        /// <summary>
        /// 获取对称密钥的IV
        /// </summary>
        /// <returns></returns>
        string GetEncryptIV();
    }
}
