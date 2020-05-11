using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace MSLibrary.FileManagement.DAL
{
    /// <summary>
    /// 数据存储帮助类
    /// 统一管理查询赋值
    /// </summary>
    public static class StoreHelper
    {
        /// <summary>
        /// 获取UploadFile的数据查询字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetUploadFileSelectFields(string prefix)
        {

            var strSelect = @"{0}.[id] as [{0}id]  
                                ,{0}.[uniquename] as [{0}uniquename]
                                ,{0}.[displayname] as [{0}displayname]
                                ,{0}.[filetype] as [{0}filetype]
                                ,{0}.[suffix] as [{0}suffix]
                                ,{0}.[size] as [{0}size]
                                ,{0}.[regardingtype] as [{0}regardingtype]
                                ,{0}.[regardingkey] as [{0}regardingkey]
                                ,{0}.[sourcetype] as [{0}sourcetype]
                                ,{0}.[sourcekey] as [{0}sourcekey]
                                ,{0}.[status] as [{0}status]
                                ,{0}.[createtime] as [{0}createtime]
                                ,{0}.[modifytime] as [{0}modifytime]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id] as [id]
                                ,[uniquename] as [uniquename]
                                ,[displayname] as [displayname]
                                ,[filetype] as [filetype]
                                ,[suffix] as [suffix]
                                ,[size] as [size]
                                ,[regardingtype] as [regardingtype]
                                ,[regardingkey] as [regardingkey]
                                ,[sourcetype] as [sourcetype]
                                ,[sourcekey] as [sourcekey]
                                ,[status] as [status]
                                ,[createtime] as [createtime]
                                ,[modifytime] as [modifytime]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// UploadFile数据赋值
        /// </summary>
        public static void SetUploadFileFields(UploadFile uploadFile, DbDataReader reader, string prefix)
        {
            if (reader[string.Format("{0}id", prefix)] != DBNull.Value)
            {
                uploadFile.ID = (Guid)reader[string.Format("{0}id", prefix)];
            }
            if (reader[string.Format("{0}uniquename", prefix)] != DBNull.Value)
            {
                uploadFile.UniqueName = reader[string.Format("{0}uniquename", prefix)].ToString();
            }
            if (reader[string.Format("{0}displayname", prefix)] != DBNull.Value)
            {
                uploadFile.DisplayName = reader[string.Format("{0}displayname", prefix)].ToString();
            }
            if (reader[string.Format("{0}filetype", prefix)] != DBNull.Value)
            {
                uploadFile.FileType = (FileType)Enum.Parse(typeof(FileType), reader[string.Format("{0}filetype", prefix)].ToString());
            }
            if (reader[string.Format("{0}suffix", prefix)] != DBNull.Value)
            {
                uploadFile.Suffix = reader[string.Format("{0}suffix", prefix)].ToString();
            }
            if (reader[string.Format("{0}size", prefix)] != DBNull.Value)
            {
                uploadFile.Size = long.Parse(reader[string.Format("{0}size", prefix)].ToString());
            }
            if (reader[string.Format("{0}regardingtype", prefix)] != DBNull.Value)
            {
                uploadFile.RegardingType = reader[string.Format("{0}regardingtype", prefix)].ToString();
            }
            if (reader[string.Format("{0}regardingkey", prefix)] != DBNull.Value)
            {
                uploadFile.RegardingKey = reader[string.Format("{0}regardingkey", prefix)].ToString();
            }
            if (reader[string.Format("{0}sourcetype", prefix)] != DBNull.Value)
            {
                uploadFile.SourceType = reader[string.Format("{0}sourcetype", prefix)].ToString();
            }
            if (reader[string.Format("{0}sourcekey", prefix)] != DBNull.Value)
            {
                uploadFile.SourceKey = reader[string.Format("{0}sourcekey", prefix)].ToString();
            }
            if (reader[string.Format("{0}status", prefix)] != DBNull.Value)
            {
                uploadFile.Status = int.Parse(reader[string.Format("{0}status", prefix)].ToString());
            }
            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                uploadFile.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }
            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                uploadFile.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }
        }

        /// <summary>
        /// 获取UploadFileHandleConfiguration的数据查询字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetUploadFileHandleConfigurationFields(string prefix)
        {

            var strSelect = @" {0}.[id] as [{0}id]
                              ,{0}.[name] as [{0}name]
                              ,{0}.[AllowSuffixs]  as [{0}AllowSuffixs]
                              ,{0}.[Configuration] as [{0}Configuration]
                              ,{0}.[Description]   as [{0}Description]
                              ,{0}.[Status]        as [{0}Status]
                              ,{0}.[createtime]    as [{0}createtime]
                              ,{0}.[modifytime]    as [{0}modifytime]";

            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @" [id] as [id]
                              ,[name] as [name]
                              ,[AllowSuffixs]  as [AllowSuffixs]
                              ,[Configuration] as [Configuration]
                              ,[Description]   as [Description]
                              ,[Status]        as [Status]
                              ,[createtime]    as [createtime]
                              ,[modifytime]    as [modifytime]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// uploadFileHandleConfiguration数据赋值
        /// </summary>
        /// <param name="uploadFileHandleConfiguration"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetUploadFileHandleConfigurationFields(UploadFileHandleConfiguration uploadFileHandleConfiguration, DbDataReader reader, string prefix)
        {
            if (reader[string.Format("{0}id", prefix)] != DBNull.Value)
            {
                uploadFileHandleConfiguration.ID = (Guid)reader[string.Format("{0}id", prefix)];
            }
            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                uploadFileHandleConfiguration.Name = reader[string.Format("{0}name", prefix)].ToString();
            }
            if (reader[string.Format("{0}AllowSuffixs", prefix)] != DBNull.Value)
            {
                uploadFileHandleConfiguration.AllowSuffixs = reader[string.Format("{0}AllowSuffixs", prefix)].ToString().Split(',');
            }

            if (reader[string.Format("{0}Configuration", prefix)] != DBNull.Value)
            {
                uploadFileHandleConfiguration.Configuration = reader[string.Format("{0}Configuration", prefix)].ToString();
            }
            if (reader[string.Format("{0}Description", prefix)] != DBNull.Value)
            {
                uploadFileHandleConfiguration.Description = reader[string.Format("{0}Description", prefix)].ToString();
            }

            if (reader[string.Format("{0}status", prefix)] != DBNull.Value)
            {
                uploadFileHandleConfiguration.Status = int.Parse(reader[string.Format("{0}status", prefix)].ToString());
            }
            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                uploadFileHandleConfiguration.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }
            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                uploadFileHandleConfiguration.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }

        }

        /// <summary>
        /// 获取UploadFileHandleRecord的数据查询字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetUploadFileHandleRecordSelectFields(string prefix)
        {
            var strSelect = @"	   [id] as [{0}id]
                                  ,[UploadFileId] as [{0}UploadFileId]
                                  ,[ConfigurationName] as [{0}ConfigurationName]
                                  ,[Error] as [{0}Error]
                                  ,[Status] as [{0}Status]
                                  ,[UploadFileRegardingType] as [{0}UploadFileRegardingType]
                                  ,[UploadFileRegardingKey] as [{0}UploadFileRegardingKey]
                                  ,[ExtensionInfo] as [{0}ExtensionInfo]
                                  ,[ResultInfo] as [{0}ResultInfo]
                                  ,[createtime] as [{0}createtime]
                                  ,[modifytime] as [{0}modifytime]";

            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"     [id] as [id]
                                  ,[UploadFileId] as [UploadFileId]
                                  ,[ConfigurationName] as [ConfigurationName]
                                  ,[Error] as [Error]
                                  ,[Status] as [Status]
                                  ,[UploadFileRegardingType] as [UploadFileRegardingType]
                                  ,[UploadFileRegardingKey] as [UploadFileRegardingKey]
                                  ,[ExtensionInfo] as [ExtensionInfo]
                                  ,[ResultInfo] as [ResultInfo]
                                  ,[createtime] as [createtime]
                                  ,[modifytime] as [modifytime]";
            }

            return string.Format(strSelect, prefix);
        }
        
        /// <summary>
        /// UploadFileHandleRecord 数据赋值
        /// </summary>
        /// <param name="record"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetUploadFileHandleRecordFields(UploadFileHandleRecord record, DbDataReader reader, string prefix)
        {
            if (reader[string.Format("{0}id", prefix)] != DBNull.Value)
            {
                record.ID = (Guid)reader[string.Format("{0}id", prefix)];
            }

            if (reader[string.Format("{0}UploadFileId", prefix)] != DBNull.Value)
            {
                record.UploadFileId = (Guid)reader[string.Format("{0}UploadFileId", prefix)];
            }

            if (reader[string.Format("{0}ConfigurationName", prefix)] != DBNull.Value)
            {
                record.ConfigurationName = reader[string.Format("{0}ConfigurationName", prefix)].ToString();
            }

            if (reader[string.Format("{0}Error", prefix)] != DBNull.Value)
            {
                record.Error = reader[string.Format("{0}Error", prefix)].ToString();
            }

            if (reader[string.Format("{0}status", prefix)] != DBNull.Value)
            {
                record.Status = int.Parse(reader[string.Format("{0}status", prefix)].ToString());
            }

            if (reader[string.Format("{0}UploadFileRegardingType", prefix)] != DBNull.Value)
            {
                record.UploadFileRegardingType = reader[string.Format("{0}UploadFileRegardingType", prefix)].ToString();
            }

            if (reader[string.Format("{0}UploadFileRegardingKey", prefix)] != DBNull.Value)
            {
                record.UploadFileRegardingKey = reader[string.Format("{0}UploadFileRegardingKey", prefix)].ToString();
            }

            if (reader[string.Format("{0}ExtensionInfo", prefix)] != DBNull.Value)
            {
                record.ExtensionInfo = reader[string.Format("{0}ExtensionInfo", prefix)].ToString();
            }

            if (reader[string.Format("{0}ResultInfo", prefix)] != DBNull.Value)
            {
                record.ResultInfo = reader[string.Format("{0}ResultInfo", prefix)].ToString();
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
