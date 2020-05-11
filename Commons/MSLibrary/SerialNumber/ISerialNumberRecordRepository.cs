using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SerialNumber
{
    /// <summary>
    /// 序列号记录仓储
    /// </summary>
    public interface ISerialNumberRecordRepository
    {
        /// <summary>
        /// 根据前缀查询
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        Task<SerialNumberRecord> QueryByPrefix(string prefix);
    }
}
