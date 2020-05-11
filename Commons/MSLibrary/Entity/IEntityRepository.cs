using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Entity
{
    /// <summary>
    /// 基于实体的查询
    /// </summary>
    public interface IEntityRepository
    {
        /// <summary>
        /// 根据实体类型和实体关键字查询
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="entityKey"></param>
        /// <returns></returns>
        Task<ModelBase> QueryByTypeAndKey(string entityType,string entityKey);
    }
}
