using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SerialNumber.DAL
{
    /// <summary>
    /// 序列号生成配置数据操作
    /// </summary>
    public interface ISerialNumberGeneratorConfigurationStore
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        Task Add(SerialNumberGeneratorConfiguration configuration);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        Task Update(SerialNumberGeneratorConfiguration configuration);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(Guid id);
        /// <summary>
        /// 根据名称查询
        /// </summary>
        /// <param name="naem"></param>
        /// <returns></returns>
        Task<SerialNumberGeneratorConfiguration> QueryByName(string name);
        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SerialNumberGeneratorConfiguration> QueryById(Guid id);
    }
}
