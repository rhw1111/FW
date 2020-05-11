using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace MSLibrary.Entity.FillEntityServices
{
    /// <summary>
    /// 填充实体服务
    /// </summary>
    public interface IFillEntityService
    {
        Task FillEntity(EntityAttributeMapConfigurationDetail configurationDetail, DbDataReader reader, string prefix, ModelBase entity);

        void FillEntitySync(EntityAttributeMapConfigurationDetail configurationDetail, DbDataReader reader, string prefix, ModelBase entity);

    }
}
