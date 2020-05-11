using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace MSLibrary.SerialNumber.DAL
{
    /// <summary>
    /// 数据操作帮助类
    /// 帮助每个实体统一查询
    /// </summary>
    public static class StoreHelper
    {
        /// <summary>
        /// 获取序列号记录的数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetSerialNumberRecordSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[prefix] as [{0}prefix],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[prefix],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 为序列号记录从DbDataReader中赋值
        /// </summary>
        /// <param name="record"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetSerialNumberRecordSelectFields(SerialNumberRecord record, DbDataReader reader, string prefix)
        {
            record.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}prefix", prefix)] != DBNull.Value)
            {
                record.Prefix = reader[string.Format("{0}prefix", prefix)].ToString();
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
        /// 获取序列号生成配置的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetSerialNumberGeneratorConfigurationSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[name] as [{0}name],{0}.[prefixtemplate] as [{0}prefixtemplate],{0}.[seriallength] as [{0}seriallength],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[name],[prefixtemplate],[seriallength],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 为序列号生成配置从DbDataReader中赋值
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetSerialNumberGeneratorConfigurationSelectFields(SerialNumberGeneratorConfiguration configuration, DbDataReader reader, string prefix)
        {
            configuration.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                configuration.Name = (string)reader[string.Format("{0}name", prefix)];
            }

            if (reader[string.Format("{0}prefixtemplate", prefix)] != DBNull.Value)
            {
                configuration.PrefixTemplate = (string)reader[string.Format("{0}prefixtemplate", prefix)];
            }


            if (reader[string.Format("{0}seriallength", prefix)] != DBNull.Value)
            {
                configuration.SerialLength = (int)reader[string.Format("{0}seriallength", prefix)];
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
