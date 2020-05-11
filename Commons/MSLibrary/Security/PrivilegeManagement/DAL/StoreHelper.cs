using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace MSLibrary.Security.PrivilegeManagement.DAL
{
    /// <summary>
    /// 数据操作帮助类
    /// </summary>
    public static class StoreHelper
    {
        /// <summary>
        /// 获取权限数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetPrivilegeSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[name] as [{0}name],{0}.[systemid] as [{0}systemid],{0}.[description] as [{0}description],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[name],[systemid],[description],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }
        /// <summary>
        /// 为权限从DbDataReader中赋值
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetPrivilegeSelectFields(Privilege privilege, DbDataReader reader, string prefix)
        {
            privilege.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                privilege.Name = reader[string.Format("{0}name", prefix)].ToString();
            }

            if (reader[string.Format("{0}systemid", prefix)] != DBNull.Value)
            {
                privilege.SystemId = (Guid)reader[string.Format("{0}systemid", prefix)];
            }

            if (reader[string.Format("{0}description", prefix)] != DBNull.Value)
            {
                privilege.Description = reader[string.Format("{0}description", prefix)].ToString();
            }

            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                privilege.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }

            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                privilege.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }
        }


        /// <summary>
        /// 获取角色数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetRoleSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[name] as [{0}name],{0}.[systemid] as [{0}systemid],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[name],[systemid],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }
        /// <summary>
        /// 为角色从DbDataReader中赋值
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetRoleSelectFields(Role role, DbDataReader reader, string prefix)
        {
            role.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                role.Name = reader[string.Format("{0}name", prefix)].ToString();
            }

            if (reader[string.Format("{0}systemid", prefix)] != DBNull.Value)
            {
                role.SystemId = (Guid)reader[string.Format("{0}systemid", prefix)];
            }

            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                role.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }

            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                role.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }
        }



        /// <summary>
        /// 获取权限系统数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetPrivilegeSystemSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[name] as [{0}name],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[name],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }
        /// <summary>
        /// 为权限系统从DbDataReader中赋值
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetPrivilegeSystemSelectFields(PrivilegeSystem system, DbDataReader reader, string prefix)
        {
            system.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                system.Name = reader[string.Format("{0}name", prefix)].ToString();
            }

            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                system.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }

            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                system.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }
        }


        /// <summary>
        /// 获取用户角色关联关系数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetUserRoleRelationSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[userkey] as [{0}userkey],{0},[{0}systemid],{0},[systemid] as [{0}systemid],{0}，[{0}systemid],{0},[roleid] as [{0}roleid],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[userkey],[systemid],[roleid],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }
        /// <summary>
        /// 为权限系统从DbDataReader中赋值
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetUserRoleRelationSelectFields(UserRoleRelation relation, DbDataReader reader, string prefix)
        {
            relation.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}userkey", prefix)] != DBNull.Value)
            {
                relation.UserKey = reader[string.Format("{0}userkey", prefix)].ToString();
            }

            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                relation.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }

            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                relation.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }
        }





    }
}
