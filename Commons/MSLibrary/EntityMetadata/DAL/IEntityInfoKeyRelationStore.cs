using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.EntityMetadata.DAL
{
    /// <summary>
    /// 实体元数据主关键字的关联关系数据操作
    /// </summary>
    public interface IEntityInfoKeyRelationStore
    {
        /// <summary>
        /// 根据实体元数据ID查询该实体元数据关联的全部记录
        /// </summary>
        /// <param name="infoId"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task QueryAllByEntityInfoId(Guid infoId,Func<EntityInfoKeyRelation,Task> callback);


    }
}
