using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Entity.FillEntityServices
{
    /// <summary>
    /// 针对值类型的填充实体服务
    /// </summary>
    public class FillEntityServiceForValue<T> : IFillEntityService
    {
        public async Task FillEntity(EntityAttributeMapConfigurationDetail configurationDetail, DbDataReader reader, string prefix, ModelBase entity)
        {

            FillEntitySync(configurationDetail, reader, prefix, entity);

            await Task.FromResult(0);
        }

        public void FillEntitySync(EntityAttributeMapConfigurationDetail configurationDetail, DbDataReader reader, string prefix, ModelBase entity)
        {
            if (reader[string.Format("{0}{1}", prefix, configurationDetail.DBColumnName)] != DBNull.Value)
            {
                entity.Attributes[configurationDetail.EntityAttributeName] = (T)reader[string.Format("{0}{1}", prefix, configurationDetail.DBColumnName)];
            }
        }
    }
}
