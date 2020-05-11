using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SerialNumber
{
    /// <summary>
    /// 序列号生成配置仓储
    /// </summary>
    public interface ISerialNumberGeneratorConfigurationRepository
    {
        /// <summary>
        /// 根据名称查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<SerialNumberGeneratorConfiguration> QueryByName(string name);
    }
}
