using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace MSLibrary.EntityMetadata.DAL
{
    /// <summary>
    /// 数据操作帮助类
    /// 帮助每个实体统一查询
    /// </summary>
    public static class StoreHelper
    {
        /// <summary>
        /// 获取选项集元数据的数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetOptionSetValueMetadataSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[name] as [{0}name],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[name],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 为选项集元数据从DbDataReader中赋值
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetOptionSetValueMetadataSelectFields(OptionSetValueMetadata metadata, DbDataReader reader, string prefix)
        {
            metadata.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                metadata.Name = reader[string.Format("{0}name", prefix)].ToString();
            }

            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                metadata.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }

            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                metadata.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }
        }


        /// <summary>
        /// 获取选项集单项的数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetOptionSetValueItemSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[value] as [{0}value],{0}.[stringvalue] as [{0}stringvalue],{0}.[defaultlabel] as [{0}defaultlabel],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[value],[stringvalue],[defaultlabel],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 为选项集单项从DbDataReader中赋值
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetOptionSetValueItemSelectFields(OptionSetValueItem item, DbDataReader reader, string prefix)
        {
            item.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}value", prefix)] != DBNull.Value)
            {
                item.Value = (int)reader[string.Format("{0}value", prefix)];
            }

            if (reader[string.Format("{0}stringvalue", prefix)] != DBNull.Value)
            {
                item.StringValue = (string)reader[string.Format("{0}stringvalue", prefix)];
            }


            if (reader[string.Format("{0}defaultlabel", prefix)] != DBNull.Value)
            {
                item.DefaultLabel = reader[string.Format("{0}defaultlabel", prefix)].ToString();
            }

            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                item.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }

            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                item.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }
        }


        /// <summary>
        /// 获取实体元数据字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetEntityInfoSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],
                                {0}.[entitytype] as [{0}entitytype],
                                {0}.[createtime] as [{0}createtime],
                                {0}.[modifytime] as [{0}modifytime]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],
                               [entitytype],
                               [createtime],
                               [modifytime]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 为实体元数据从DbDataReader中赋值
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetEntityInfoSelectFields(EntityInfo metadata, DbDataReader reader, string prefix)
        {
            metadata.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}entitytype", prefix)] != DBNull.Value)
            {
                metadata.EntityType = reader[string.Format("{0}entitytype", prefix)].ToString();
            }
            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                metadata.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }
            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                metadata.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }
        }

        /// <summary>
        /// 获取实体元数据的数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetEntityAttributeInfoSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],
                                {0}.[entityinfoid] as [{0}entityinfoid],
                                {0}.[name] as [{0}name],
                                {0}.[type] as [{0}type],
                                {0}.[createtime] as [{0}createtime],
                                {0}.[modifytime] as [{0}modifytime]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id]
                              ,[entityinfoid]
                              ,[name]
                              ,[type]
                              ,[createtime]
                              ,[modifytime]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 为实体元数据数据从DbDataReader中赋值
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetEntityAttributeInfoSelectFields(EntityAttributeInfo metadata, DbDataReader reader, string prefix)
        {
            metadata.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}entityinfoid", prefix)] != DBNull.Value)
            {
                metadata.EntityInfoId = (Guid)reader[string.Format("{0}entityinfoid", prefix)];
            }
            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                metadata.Name = reader[string.Format("{0}name", prefix)].ToString();
            }
            if (reader[string.Format("{0}type", prefix)] != DBNull.Value)
            {
                metadata.Type = reader[string.Format("{0}type", prefix)].ToString();
            }
            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                metadata.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }
            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                metadata.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }
        }

        /// <summary>
        /// 获取实体元数据主关键字的关联关系查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetEntityInfoKeyRelationSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],
                                {0}.[entityinfoid] as [{0}entityinfoid],
                                {0}.[entityattributeinfoid] as [{0}entityattributeinfoid],
                                {0}.[order] as [{0}order],
                                {0}.[createtime] as [{0}createtime],
                                {0}.[modifytime] as [{0}modifytime]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id]
                              ,[entityinfoid]
                              ,[entityattributeinfoid]
                              ,[order]
                              ,[createtime]
                              ,[modifytime]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 为实体元数据主关键字的关联关系赋值
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetEntityInfoKeyRelationSelectFields(EntityInfoKeyRelation metadata, DbDataReader reader, string prefix)
        {
            metadata.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}entityinfoid", prefix)] != DBNull.Value)
            {
                metadata.EntityInfoId = (Guid)reader[string.Format("{0}entityinfoid", prefix)];
            }
            if (reader[string.Format("{0}entityattributeinfoid", prefix)] != DBNull.Value)
            {
                metadata.EntityAttributeInfoId = (Guid)reader[string.Format("{0}entityattributeinfoid", prefix)];
            }
            if (reader[string.Format("{0}order", prefix)] != DBNull.Value)
            {
                metadata.Order = (int)reader[string.Format("{0}order", prefix)];
            }
            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                metadata.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }
            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                metadata.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }
        }

        /// <summary>
        /// 获取实体元数据备用关键字字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetEntityInfoAlternateKeySelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],
                                {0}.[name] as [{0}name],
                                {0}.[entityinfoid] as [{0}entityinfoid],
                                {0}.[createtime] as [{0}createtime],
                                {0}.[modifytime] as [{0}modifytime]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id]
                              ,[name]
                              ,[entityinfoid]
                              ,[createtime]
                              ,[modifytime]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 为实体元数据备用关键字赋值
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetEntityInfoAlternateKeySelectFields(EntityInfoAlternateKey metadata, DbDataReader reader, string prefix)
        {
            metadata.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                metadata.Name = reader[string.Format("{0}name", prefix)].ToString();
            }
            if (reader[string.Format("{0}entityinfoid", prefix)] != DBNull.Value)
            {
                metadata.EntityInfoId = (Guid)reader[string.Format("{0}entityinfoid", prefix)];
            }
            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                metadata.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }
            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                metadata.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }
        }

        /// <summary>
        /// 获取实体元数据备用关键字的关联关系字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetEntityInfoAlternateKeyRelationSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],
                                {0}.[entityinfoAlternatekeyid] as [{0}entityinfoAlternatekeyid],
                                {0}.[entityattributeinfoid] as [{0}entityattributeinfoid],
                                {0}.[order] as [{0}order],
                                {0}.[createtime] as [{0}createtime],
                                {0}.[modifytime] as [{0}modifytime]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id]
                              ,[entityinfoAlternatekeyid]
                              ,[entityattributeinfoid]
                              ,[order]
                              ,[createtime]
                              ,[modifytime]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 为实体元数据备用关键字的关联关系赋值
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetEntityInfoAlternateKeyRelationSelectFields(EntityInfoAlternateKeyRelation metadata, DbDataReader reader, string prefix)
        {
            metadata.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}entityinfoAlternatekeyid", prefix)] != DBNull.Value)
            {
                metadata.EntityInfoAlternateKeyId = (Guid)reader[string.Format("{0}entityinfoAlternatekeyid", prefix)];
            }
            if (reader[string.Format("{0}entityattributeinfoid", prefix)] != DBNull.Value)
            {
                metadata.EntityAttributeInfoId = (Guid)reader[string.Format("{0}entityattributeinfoid", prefix)];
            }
            if (reader[string.Format("{0}order", prefix)] != DBNull.Value)
            {
                metadata.Order = (int)reader[string.Format("{0}order", prefix)];
            }
            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                metadata.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }
            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                metadata.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }
        }
    }
}
