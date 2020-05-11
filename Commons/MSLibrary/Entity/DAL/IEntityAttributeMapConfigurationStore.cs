using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Entity.DAL
{
    /// <summary>
    /// 实体属性映射配置数据操作
    /// </summary>
    public interface IEntityAttributeMapConfigurationStore
    {
        /// <summary>
        /// 根据实体名称查询
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        Task<EntityAttributeMapConfiguration> QueryByEntityName(string entityName);

        /// <summary>
        /// 根据实体名称查询（同步）
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        EntityAttributeMapConfiguration QueryByEntityNameSync(string entityName);

    }
}
