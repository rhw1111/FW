using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Configuration
{
    /// <summary>
    /// 系统配置仓储
    /// </summary>
    public interface ISystemConfigurationRepository
    {
        /// <summary>
        /// 根据名称查询系统配置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        SystemConfiguration QueryByName(string name);
    }
}
