using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Configuration.DAL
{
    /// <summary>
    /// 系统配置实体数据操作
    /// </summary>
    public interface ISystemConfigurationStore
    {
        /// <summary>
        /// 根据名称查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        SystemConfiguration QueryByName(string name);

        /// <summary>
        /// 修改配置内容
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        Task UpdateContent(SystemConfiguration configuration);
    }
}
