using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace MSLibrary.Redis.DAL
{
    public static class StoreHelper
    {
        /// <summary>
        /// 获取Redis客户端工厂数据操作
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetRedisClientFactoryStoreSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],
                                {0}.[name] as [{0}name],
                                {0}.[type] as [{0}type],
                                {0}.[configuration] as [{0}configuration],                                           
                                {0}.[createtime] as [{0}createtime],
                                {0}.[modifytime] as [{0}modifytime],
                                {0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],
                               [name],
                               [type],
                               [configuration],
                               [createtime],
                               [modifytime],
                               [sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 赋值Redis客户端工厂数据操作
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetRedisClientFactoryStoreSelectFields(RedisClientFactory factory, DbDataReader reader, string prefix)
        {
            factory.ID = (Guid)reader[string.Format("{0}id", prefix)];
            factory.Name = reader[string.Format("{0}name", prefix)].ToString();
            factory.Type = reader[string.Format("{0}type", prefix)].ToString();
            factory.Configuration = reader[string.Format("{0}configuration", prefix)].ToString();
            factory.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            factory.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
        }

    }
}
