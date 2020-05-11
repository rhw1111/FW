using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace MSLibrary.Security.BusinessSecurityRule.DAL
{
    /// <summary>
    /// 数据操作帮助类
    /// 帮助每个实体统一查询
    /// </summary>
    public static class StoreHelper
    {
        /// <summary>
        /// 获取业务动作数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetBusinessActionSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[name] as [{0}name],{0}.[rule] as [{0}rule],{0}.[originalparametersfiltertype] as [{0}originalparametersfiltertype],{0}.[errorreplacetext] as [{0}errorreplacetext],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[name],[rule],[originalparametersfiltertype],[errorreplacetext],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }
        /// <summary>
        /// 为业务动作从DbDataReader中赋值
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetBusinessActionSelectFields(BusinessAction action, DbDataReader reader, string prefix)
        {
            action.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                action.Name = reader[string.Format("{0}name", prefix)].ToString();
            }

            if (reader[string.Format("{0}rule", prefix)] != DBNull.Value)
            {
                action.Rule = reader[string.Format("{0}rule", prefix)].ToString();
            }

            if (reader[string.Format("{0}originalparametersfiltertype", prefix)] != DBNull.Value)
            {
                action.OriginalParametersFilterType = reader[string.Format("{0}originalparametersfiltertype", prefix)].ToString();
            }

            if (reader[string.Format("{0}errorreplacetext", prefix)] != DBNull.Value)
            {
                action.ErrorReplaceText = reader[string.Format("{0}errorreplacetext", prefix)].ToString();
            }


            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                action.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }

            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                action.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }
        }

        /// <summary>
        /// 获取业务动作组数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetBusinessActionGroupSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[name] as [{0}name],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[name],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 为业务动作组从DbDataReader中赋值
        /// </summary>
        /// <param name="group"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetBusinessActionGroupSelectFields(BusinessActionGroup group, DbDataReader reader, string prefix)
        {
            group.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                group.Name = reader[string.Format("{0}name", prefix)].ToString();
            }

            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                group.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }

            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                group.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }

        }


    }
}
