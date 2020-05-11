using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.EntityMetadata.DAL
{
    /// <summary>
    /// 实体元数据备用关键字的关联关系数据操作
    /// </summary>
    public interface IEntityInfoAlternateKeyRelationStore
    {
        /// <summary>
        /// 根据实体元树备用关键字Id查询全部记录
        /// </summary>
        /// <param name="alternateKeyId"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task QueryAllByAlternateKeyId(Guid alternateKeyId, Func<EntityInfoAlternateKeyRelation, Task> callback);

    }
}
