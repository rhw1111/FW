using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.EntityMetadata
{
    /// <summary>
    /// 实体元数据仓储
    /// </summary>
    public interface IEntityInfoRepository
    {
        /// <summary>
        /// 根据实体类型查询
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        Task<EntityInfo> QueryByEntityType(string entityType);
    }
}
