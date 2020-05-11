using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace MSLibrary.Security.Jwt.DAL
{
    /// <summary>
    /// 数据操作帮助类
    /// 帮助每个实体统一查询
    /// </summary>
    public static class StoreHelper
    {
        /// <summary>
        /// 获取Jwt终结点数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetJwtEnpointSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[name] as [{0}name],{0}.[createsignkeytype] as [{0}createsignkeytype],{0}.[createsignkeyconfiguration] as [{0}createsignkeyconfiguration],{0}.[validatesignkeytype] as [{0}validatesignkeytype],{0}.[validatesignkeyconfiguration] as [{0}validatesignkeyconfiguration],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[name],[createsignkeytype],[createsignkeyconfiguration],[validatesignkeytype],[validatesignkeyconfiguration],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }
        /// <summary>
        /// 为Jwt终结点从DbDataReader中赋值
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetJwtEnpointSelectFields(JwtEnpoint endpoint, DbDataReader reader, string prefix)
        {
            endpoint.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                endpoint.Name = reader[string.Format("{0}name", prefix)].ToString();
            }

            if (reader[string.Format("{0}createsignkeytype", prefix)] != DBNull.Value)
            {
                endpoint.CreateSignKeyType = reader[string.Format("{0}createsignkeytype", prefix)].ToString();
            }

            if (reader[string.Format("{0}createsignkeyconfiguration", prefix)] != DBNull.Value)
            {
                endpoint.CreateSignKeyConfiguration = reader[string.Format("{0}createsignkeyconfiguration", prefix)].ToString();
            }

            if (reader[string.Format("{0}validatesignkeytype", prefix)] != DBNull.Value)
            {
                endpoint.ValidateSignKeyType = reader[string.Format("{0}validatesignkeytype", prefix)].ToString();
            }

            if (reader[string.Format("{0}validatesignkeyconfiguration", prefix)] != DBNull.Value)
            {
                endpoint.ValidateSignKeyConfiguration = reader[string.Format("{0}validatesignkeyconfiguration", prefix)].ToString();
            }


            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                endpoint.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }

            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                endpoint.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }
        }

    }
}
