using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using MSLibrary.Configuration;

namespace MSLibrary.MySqlStore.Configuration.DAL
{
    /// <summary>
    /// 数据存储帮助类
    /// 统一管理查询赋值
    /// </summary>
    public static class NStoreHelper
    {

        /// <summary>
        /// 获取系统配置实体的数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetSystemConfigurationSelectFields(string prefix)
        {
            var strSelect = @"{0}.id as {0}id,{0}.name as {0}name,{0}.content as {0}content,{0}.createtime as {0}createtime,{0}.modifytime as {0}modifytime,{0}.sequence as {0}sequence";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"id,name,content,createtime,modifytime,sequence";
            }
            return string.Format(strSelect, prefix);
        }


        /// <summary>
        /// 系统配置实体的赋值
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetSystemConfigurationSelectFields(SystemConfiguration configuration, DbDataReader reader, string prefix)
        {
            configuration.ID = (Guid)reader[string.Format("{0}id", prefix)];
            configuration.Name = reader[string.Format("{0}name", prefix)].ToString();
            configuration.Content = reader[string.Format("{0}content", prefix)].ToString();
            configuration.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            configuration.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];

        }
    }
}
