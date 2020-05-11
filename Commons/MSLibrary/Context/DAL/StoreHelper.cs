using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;

namespace MSLibrary.Context.DAL
{
    /// <summary>
    /// 数据操作帮助类
    /// 帮助每个实体统一查询
    /// </summary>
    public static class StoreHelper
    {
        /// <summary>
        /// 获取Http声明生成器的数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetHttpClaimGeneratorSelectFields(string prefix)
        {
           
            var strSelect = @"{0}.[id] as [{0}id],{0}.[name] as [{0}name],{0}.[type] as [{0}type],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[name],[type],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 为Http声明生成器从DbDataReader中赋值
        /// </summary>
        /// <param name="record"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetHttpClaimGeneratorSelectFields(HttpClaimGenerator generator, DbDataReader reader, string prefix)
        {
            generator.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                generator.Name = reader[string.Format("{0}name", prefix)].ToString();
            }

            if (reader[string.Format("{0}type", prefix)] != DBNull.Value)
            {
                generator.Type = reader[string.Format("{0}type", prefix)].ToString();
            }

            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                generator.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }

            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                generator.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }
        }


        /// <summary>
        /// 获取声明上下文生成的数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetClaimContextGeneratorSelectFields(string prefix)
        {

            var strSelect = @"{0}.[id] as [{0}id],{0}.[name] as [{0}name],{0}.[type] as [{0}type],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[name],[type],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 为声明上下文从DbDataReader中赋值
        /// </summary>
        /// <param name="record"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetClaimContextGeneratorSelectFields(ClaimContextGenerator generator, DbDataReader reader, string prefix)
        {
            generator.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                generator.Name = reader[string.Format("{0}name", prefix)].ToString();
            }

            if (reader[string.Format("{0}type", prefix)] != DBNull.Value)
            {
                generator.Type = reader[string.Format("{0}type", prefix)].ToString();
            }

            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                generator.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }

            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                generator.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }
        }


        /// <summary>
        /// 获取环境声明生成的数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetEnvironmentClaimGeneratorSelectFields(string prefix)
        {

            var strSelect = @"{0}.[id] as [{0}id],{0}.[name] as [{0}name],{0}.[type] as [{0}type],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[name],[type],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 为环境声明生成器从DbDataReader中赋值
        /// </summary>
        /// <param name="record"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetEnvironmentClaimGeneratorSelectFields(EnvironmentClaimGenerator generator, DbDataReader reader, string prefix)
        {
            generator.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                generator.Name = reader[string.Format("{0}name", prefix)].ToString();
            }

            if (reader[string.Format("{0}type", prefix)] != DBNull.Value)
            {
                generator.Type = reader[string.Format("{0}type", prefix)].ToString();
            }

            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                generator.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }

            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                generator.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }
        }

    }
}
