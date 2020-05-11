using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.EntityMetadata.DAL
{
    /// <summary>
    /// 实体元数据数据操作
    /// </summary>
    public interface IEntityInfoStore
    {
        /// <summary>
        /// 根据实体类型查询
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        Task<EntityInfo> QueryByEntityType(string entityType);
    }
}
