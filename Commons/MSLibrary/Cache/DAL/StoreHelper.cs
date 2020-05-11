using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace MSLibrary.Cache.DAL
{
    /// <summary>
    /// 数据操作帮助类
    /// 帮助每个实体统一查询
    /// </summary>
    public static class StoreHelper
    {
        /// <summary>
        /// 获取缓存远程配置数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetCacheRemoteConfigurationSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[name] as [{0}name],{0}.[remoteaddresses] as [{0}remoteaddresses],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[name],[remoteaddresses],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }
        /// <summary>
        /// 为队列从DbDataReader中赋值
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetCacheRemoteConfigurationSelectFields(CacheRemoteConfiguration configuration, DbDataReader reader, string prefix)
        {
            configuration.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                configuration.Name = reader[string.Format("{0}name", prefix)].ToString();
            }

            if (reader[string.Format("{0}remoteaddresses", prefix)] != DBNull.Value)
            {
                configuration.RemoteAddresses = reader[string.Format("{0}remoteaddresses", prefix)].ToString();
            }


            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                configuration.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }

            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                configuration.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }
        }

    }
}
