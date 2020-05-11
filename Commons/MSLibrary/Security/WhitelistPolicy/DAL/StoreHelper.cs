using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace MSLibrary.Security.WhitelistPolicy.DAL
{
    /// <summary>
    /// 数据存储帮助类
    /// 统一管理查询赋值
    /// </summary>
    public static class StoreHelper
    {

        /// <summary>
        /// 获取白名单实体的数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetWhitelistSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[systemname] as [{0}systemname],{0}.[systemsecret] as [{0}systemsecret],{0}.[trustips] as [{0}trustips],{0}.[enableipvalidation] as [{0}enableipvalidation],{0}.[status] as [{0}status],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[systemname],[systemsecret],[trustips],[enableipvalidation],[status],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }


        /// <summary>
        /// 白名单实体的赋值
        /// </summary>
        /// <param name="whitelist"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetWhitelistSelectFields(Whitelist whitelist, DbDataReader reader, string prefix)
        {
            whitelist.ID = (Guid)reader[string.Format("{0}id", prefix)];
            whitelist.SystemName = reader[string.Format("{0}systemname", prefix)].ToString();
            whitelist.SystemSecret = reader[string.Format("{0}systemsecret", prefix)].ToString();
            whitelist.TrustIPs = reader[string.Format("{0}trustips", prefix)].ToString();
            whitelist.EnableIPValidation = (bool)reader[string.Format("{0}enableipvalidation", prefix)];
            whitelist.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            whitelist.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            whitelist.Status = (int)reader[string.Format("{0}status", prefix)];

        }

        /// <summary>
        /// 获取系统操作的数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetSystemOperationSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[name] as [{0}name],{0}.[status] as [{0}status],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[name],[status],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        public static void SetSystemOperationSelectFields(SystemOperation operation, DbDataReader reader, string prefix)
        {
            operation.ID = (Guid)reader[string.Format("{0}id", prefix)];
            operation.Name = reader[string.Format("{0}name", prefix)].ToString();
            operation.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            operation.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            operation.Status = (int)reader[string.Format("{0}status", prefix)];
        }


        /// <summary>
        /// 获取客户端白名单数据操作字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetClientWhitelistStoreSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],
                                {0}.[systemname] as [{0}systemname],
                                {0}.[systemsecret] as [{0}systemsecret],
                                {0}.[signatureexpire] as [{0}signatureexpire],
                                {0}.[createtime] as [{0}createtime],
                                {0}.[modifytime] as [{0}modifytime],
                                {0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[systemname],[systemsecret],[signatureexpire],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }
        /// <summary>
        /// 客户端白名单数据操作实体赋值
        /// </summary>
        /// <param name="whitelist"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetClientWhitelistStoreSelectFields(ClientWhitelist whitelist, DbDataReader reader, string prefix)
        {
            whitelist.ID = (Guid)reader[string.Format("{0}id", prefix)];
            whitelist.SystemName = reader[string.Format("{0}systemname", prefix)].ToString();
            whitelist.SystemSecret = reader[string.Format("{0}systemsecret", prefix)].ToString();
            whitelist.SignatureExpire = (int)reader[string.Format("{0}signatureexpire", prefix)];
            whitelist.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            whitelist.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
        }
    }
}
