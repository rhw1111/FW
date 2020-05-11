using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Entity.DAL
{
    /// <summary>
    /// 实体属性映射配置明细数据操作
    /// </summary>
    public interface IEntityAttributeMapConfigurationDetailStore
    {
        /// <summary>
        /// 根据配置Id查询所有关联的明细
        /// </summary>
        /// <param name="configurationId"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task QueryAllByConfigurationId(Guid configurationId,Func<EntityAttributeMapConfigurationDetail,Task> callback);

        /// <summary>
        /// 根据配置Id查询所有关联的明细（同步）
        /// </summary>
        /// <param name="configurationId"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        void QueryAllByConfigurationIdSync(Guid configurationId, Action<EntityAttributeMapConfigurationDetail> callback);
        /// <summary>
        /// 根据配置Id分页查询关联的明细
        /// </summary>
        /// <param name="configurationId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<EntityAttributeMapConfigurationDetail>> QueryByPage(Guid configurationId,int page,int pageSize);
        /// <summary>
        /// 根据配置Id分页查询关联的明细（同步）
        /// </summary>
        /// <param name="configurationId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        QueryResult<EntityAttributeMapConfigurationDetail> QueryByPageSync(Guid configurationId, int page, int pageSize);
    }
}
