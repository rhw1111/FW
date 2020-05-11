using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SerialNumber.DAL
{
    /// <summary>
    /// 序列号记录数据操作
    /// </summary>
    public interface ISerialNumberRecordStore
    {
        /// <summary>
        /// 新增序列号记录
        /// 完成后会填充Value值
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        Task Add(SerialNumberRecord record);
        /// <summary>
        /// 修改序列号记录的Value值
        /// 完成后会填充Value值
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        Task UpdateValue(SerialNumberRecord record);
        /// <summary>
        /// 根据前缀查询序列号记录
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        Task<SerialNumberRecord> QueryByPrefix(string prefix);
    }
}
