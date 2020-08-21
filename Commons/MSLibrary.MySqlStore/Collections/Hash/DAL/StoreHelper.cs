using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using MSLibrary.Collections.Hash;

namespace MSLibrary.MySqlStore.Collections.Hash.DAL
{
    /// <summary>
    /// 数据操作帮助类
    /// 帮助每个实体统一查询
    /// </summary>
    public static class StoreHelper
    {
        /// <summary>
        /// 获取一致性哈希组策略的数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetHashGroupStrategySelectFields(string prefix)
        {
            var strSelect = @"{0}.id as {0}id,{0}.name as {0}name,{0}.strategyservicefactorytype as {0}strategyservicefactorytype,{0}.strategyservicefactorytypeusedi as {0}strategyservicefactorytypeusedi,{0}.createtime as {0}createtime,{0}.modifytime as {0}modifytime,{0}.sequence as {0}sequence";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"id,name,strategyservicefactorytype,strategyservicefactorytypeusedi,createtime,modifytime,sequence";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 为一致性哈希组策略从DbDataReader中赋值
        /// </summary>
        /// <param name="record"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetHashGroupStrategySelectFields(HashGroupStrategy record, DbDataReader reader, string prefix)
        {
            record.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                record.Name = reader[string.Format("{0}name", prefix)].ToString();
            }

            if (reader[string.Format("{0}strategyservicefactorytype", prefix)] != DBNull.Value)
            {
                record.StrategyServiceFactoryType = reader[string.Format("{0}strategyservicefactorytype", prefix)].ToString();
            }

            if (reader[string.Format("{0}strategyservicefactorytypeusedi", prefix)] != DBNull.Value)
            {
                record.StrategyServiceFactoryTypeUseDI = Convert.ToBoolean(reader[string.Format("{0}strategyservicefactorytypeusedi", prefix)]);
            }

            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                record.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }

            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                record.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }
        }


        /// <summary>
        /// 获取一致性哈希组的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetHashGroupSelectFields(string prefix)
        {
            var strSelect = @"{0}.id as {0}id,{0}.name as {0}name,{0}.type as {0}type,{0}.count as {0}count,{0}.strategyid as {0}strategyid,{0}.createtime as {0}createtime,{0}.modifytime as {0}modifytime,{0}.sequence as {0}sequence";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"id,name,type,count,strategyid,createtime,modifytime,sequence";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 为一致性哈希组从DbDataReader中赋值
        /// </summary>
        /// <param name="record"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetHashGroupSelectFields(HashGroup record, DbDataReader reader, string prefix)
        {
            record.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                record.Name = (string)reader[string.Format("{0}name", prefix)];
            }

            if (reader[string.Format("{0}type", prefix)] != DBNull.Value)
            {
                record.Type = (string)reader[string.Format("{0}type", prefix)];
            }

            if (reader[string.Format("{0}count", prefix)] != DBNull.Value)
            {
                record.Count = (long)reader[string.Format("{0}count", prefix)];
            }


            if (reader[string.Format("{0}strategyid", prefix)] != DBNull.Value)
            {
                record.StrategyID = (Guid)reader[string.Format("{0}strategyid", prefix)];
            }

            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                record.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }

            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                record.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }
        }



        /// <summary>
        /// 获取一致性哈希节点的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetHashNodeSelectFields(string prefix)
        {
            var strSelect = @"{0}.id as {0}id,{0}.realnodeid as {0}realnodeid,{0}.groupid as {0}groupid,{0}.code as {0}code,{0}.status as {0}status,{0}.createtime as {0}createtime,{0}.modifytime as {0}modifytime,{0}.sequence as {0}sequence";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"id,realnodeid,groupid,code,status,createtime,modifytime,sequence";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 为一致性哈希节点从DbDataReader中赋值
        /// </summary>
        /// <param name="record"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetHashNodeSelectFields(HashNode record, DbDataReader reader, string prefix)
        {
            record.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}realnodeid", prefix)] != DBNull.Value)
            {
                record.RealNodeId = (Guid)reader[string.Format("{0}realnodeid", prefix)];
            }

            if (reader[string.Format("{0}groupid", prefix)] != DBNull.Value)
            {
                record.GroupId = (Guid)reader[string.Format("{0}groupid", prefix)];
            }

            if (reader[string.Format("{0}code", prefix)] != DBNull.Value)
            {
                record.Code = (long)reader[string.Format("{0}code", prefix)];
            }

            if (reader[string.Format("{0}status", prefix)] != DBNull.Value)
            {
                record.Status = (int)reader[string.Format("{0}status", prefix)];
            }

            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                record.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }

            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                record.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }
        }

        /// <summary>
        /// 获取真实一致性哈希节点的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetHashRealNodeSelectFields(string prefix)
        {
            var strSelect = @"{0}.id as {0}id,{0}.name as {0}name,{0}.groupid as {0}groupid,{0}.nodekey as {0}nodekey,{0}.createtime as {0}createtime,{0}.modifytime as {0}modifytime,{0}.sequence as {0}sequence";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"id,name,groupid,nodekey,createtime,modifytime,sequence";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 为真实一致性哈希节点从DbDataReader中赋值
        /// </summary>
        /// <param name="record"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetHashRealNodeSelectFields(HashRealNode record, DbDataReader reader, string prefix)
        {
            record.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}groupid", prefix)] != DBNull.Value)
            {
                record.GroupId = (Guid)reader[string.Format("{0}groupid", prefix)];
            }

            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                record.Name = reader[string.Format("{0}name", prefix)].ToString();
            }

            if (reader[string.Format("{0}nodekey", prefix)] != DBNull.Value)
            {
                record.NodeKey = reader[string.Format("{0}nodekey", prefix)].ToString();
            }

            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                record.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }

            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                record.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }
        }

    }
}
