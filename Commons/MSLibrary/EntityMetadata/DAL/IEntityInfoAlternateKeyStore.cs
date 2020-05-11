using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.EntityMetadata.DAL
{
    /// <summary>
    /// 实体元数据备用关键字数据操作
    /// </summary>
    public interface IEntityInfoAlternateKeyStore
    {
        /// <summary>
        /// 根据实体元数据Id和备用关键字名称查询备用关键字
        /// </summary>
        /// <param name="infoId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<EntityInfoAlternateKey> QueryByEntityInfoIdAndName(Guid infoId,string name);
    }
}
