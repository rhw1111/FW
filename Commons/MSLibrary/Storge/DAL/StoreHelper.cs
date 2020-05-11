using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace MSLibrary.Storge.DAL
{
    public static class StoreHelper
    {
        /// <summary>
        /// 获取分片存储信息数据操作
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetMultipartStorgeInfoSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],
                                {0}.[name] as [{0}name],
                                {0}.[displayname] as [{0}displayname],
                                {0}.[suffix] as [{0}suffix],
                                {0}.[size] as [{0}mode],
                                {0}.[sourceinfo] as [{0}sourceinfo],
                                {0}.[credentialinfo] as [{0}credentialinfo]
                                {0}.[extensioninfo] as [{0}extensioninfo],
                                {0}.[completeextensioninfo] as [{0}completeextensioninfo],
                                {0}.[status] as [{0}status],
                                {0}.[createtime] as [{0}createtime],
                                {0}.[modifytime] as [{0}modifytime],
                                {0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],
                               [name],
                               [displayname],
                               [suffix],
                               [size],
                               [sourceinfo],
                               [credentialinfo],
                               [extensioninfo],
                               [completeextensioninfo],
                               [status],
                               [createtime],
                               [modifytime],
                               [sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 赋值分片存储信息数据操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetMultipartStorgeInfoSelectFields(MultipartStorgeInfo info, DbDataReader reader, string prefix)
        {
            info.ID = (Guid)reader[string.Format("{0}id", prefix)];
            info.Name = reader[string.Format("{0}name", prefix)].ToString();
            info.DisplayName = reader[string.Format("{0}displayname", prefix)].ToString();
            info.Suffix = reader[string.Format("{0}suffix", prefix)].ToString();
            info.Size = (long)reader[string.Format("{0}size", prefix)];
            info.SourceInfo= reader[string.Format("{0}sourceinfo", prefix)].ToString();
            info.CredentialInfo = reader[string.Format("{0}credentialinfo", prefix)].ToString();
           
            if (reader[string.Format("{0}extensioninfo", prefix)]!=DBNull.Value)
            {
                info.ExtensionInfo = reader[string.Format("{0}extensioninfo", prefix)].ToString();
            }

            if (reader[string.Format("{0}completeextensioninfo", prefix)] != DBNull.Value)
            {
                info.CompleteExtensionInfo = reader[string.Format("{0}completeextensioninfo", prefix)].ToString();
            }

            info.Status= (int)reader[string.Format("{0}status", prefix)];

            info.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            info.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
        }


        /// <summary>
        /// 获取分片存储信息明细数据操作
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetMultipartStorgeInfoDetailSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],
                                {0}.[infoid] as [{0}infoid],
                                {0}.[number] as [{0}number],
                                {0}.[start] as [{0}start],
                                {0}.[end] as [{0}end],
                                {0}.[completeextensioninfo] as [{0}completeextensioninfo],
                                {0}.[status] as [{0}status],
                                {0}.[createtime] as [{0}createtime],
                                {0}.[modifytime] as [{0}modifytime],
                                {0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],
                               [infoid],
                               [number],
                               [start],
                               [end],
                               [completeextensioninfo],
                               [status],
                               [createtime],
                               [modifytime],
                               [sequence]";
            }
            return string.Format(strSelect, prefix);
        }


        /// <summary>
        /// 赋值分片存储信息明细数据操作
        /// </summary>
        /// <param name="info"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetMultipartStorgeInfoDetailSelectFields(MultipartStorgeInfoDetail detail, DbDataReader reader, string prefix)
        {
            detail.ID = (Guid)reader[string.Format("{0}id", prefix)];
            detail.Number = (int)reader[string.Format("{0}number", prefix)];
            detail.Start = (long)reader[string.Format("{0}start", prefix)];
            detail.End = (long)reader[string.Format("{0}end", prefix)];

            if (reader[string.Format("{0}completeextensioninfo", prefix)]!=DBNull.Value)
            {
                detail.CompleteExtensionInfo = reader[string.Format("{0}completeextensioninfo", prefix)].ToString();
            }

            detail.Status = (int)reader[string.Format("{0}status", prefix)];

            detail.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            detail.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
        }




        /// <summary>
        /// 获取存储组的数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetStoreGroupSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],
                                {0}.[name] as [{0}name],
                                {0}.[createtime] as [{0}createtime],
                                {0}.[modifytime] as [{0}modifytime],
                                {0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],
                               [name],
                               [createtime],
                               [modifytime],
                               [sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 存储组实体的数据赋值
        /// </summary>
        /// <param name="info"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetStoreGroupSelectFields(StoreGroup group, DbDataReader reader, string prefix)
        {
            group.ID = (Guid)reader[string.Format("{0}id", prefix)];
            group.Name = reader[string.Format("{0}name", prefix)].ToString();

            group.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            group.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
        }

        /// <summary>
        /// 获取存储组的数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetStoreGroupMemberSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],
                                {0}.[name] as [{0}name],
                                {0}.[groupid] as [{0}groupid],
                                {0}.[storeinfo] as [{0}storeinfo],
                                {0}.[createtime] as [{0}createtime],
                                {0}.[modifytime] as [{0}modifytime],
                                {0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],
                               [name],
                               [groupid],
                               [storeinfo],
                               [createtime],
                               [modifytime],
                               [sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 存储组实体的数据赋值
        /// </summary>
        /// <param name="info"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetStoreGroupMemberSelectFields(StoreGroupMember member, DbDataReader reader, string prefix)
        {
            member.ID = (Guid)reader[string.Format("{0}id", prefix)];
            member.Name = reader[string.Format("{0}name", prefix)].ToString();
            member.GroupID = (Guid)reader[string.Format("{0}groupid", prefix)];
            member.StoreInfo = reader[string.Format("{0}storeinfo", prefix)].ToString();
            member.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            member.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
        }
    }
}
