using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace MSLibrary.Azure.DAL
{
    /// <summary>
    /// 数据存储帮助类
    /// 统一管理查询赋值
    /// </summary>
    public static class StoreHelper
    {

        /// <summary>
        /// 获取令牌凭据生成器实体的数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetTokenCredentialGeneratorSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[name] as [{0}name],{0}.[type] as [{0}type],{0}.[configuration] as [{0}configuration],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[name],[type],[configuration],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }


        /// <summary>
        /// 系统配置实体的赋值
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetTokenCredentialGeneratorSelectFields(TokenCredentialGenerator generator, DbDataReader reader, string prefix)
        {
            generator.ID = (Guid)reader[string.Format("{0}id", prefix)];
            generator.Name = reader[string.Format("{0}name", prefix)].ToString();
            generator.Type = reader[string.Format("{0}type", prefix)].ToString();
            generator.Configuration = reader[string.Format("{0}configuration", prefix)].ToString();
            generator.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            generator.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];

        }
    }
}
