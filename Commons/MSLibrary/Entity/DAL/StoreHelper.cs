using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace MSLibrary.Entity.DAL
{
    /// <summary>
    /// 数据存储帮助类
    /// 统一管理查询赋值
    /// </summary>
    public static class StoreHelper
    {
        /// <summary>
        /// 实体属性映射配置明细实体的赋值
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetEntityAttributeMapConfigurationDetailSelectFields(EntityAttributeMapConfigurationDetail detail, DbDataReader reader, string prefix)
        {
            detail.ID = (Guid)reader[string.Format("{0}id", prefix)];
            detail.EntityAttributeName = reader[string.Format("{0}entityattributename", prefix)].ToString();
            detail.DBColumnName = reader[string.Format("{0}dbcolumnname", prefix)].ToString();
            detail.AttributeType = (int)reader[string.Format("{0}attributetype", prefix)];
            detail.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            detail.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
        }

    }
}
