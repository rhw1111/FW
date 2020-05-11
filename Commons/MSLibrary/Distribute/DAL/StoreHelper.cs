using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace MSLibrary.Distribute.DAL
{
    public static class StoreHelper
    {
        /// <summary>
        /// 获取应用程序限流数据操作
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetApplicationLimitStoreSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],
                                {0}.[name] as [{0}name],
                                {0}.[type] as [{0}type],
                                {0}.[configuration] as [{0}configuration],                                           
                                {0}.[createtime] as [{0}createtime],
                                {0}.[modifytime] as [{0}modifytime],
                                {0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],
                               [name],
                               [type],
                               [configuration],
                               [createtime],
                               [modifytime],
                               [sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 赋值应用程序限流数据操作
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetApplicationLimitStoreSelectFields(ApplicationLimit limit, DbDataReader reader, string prefix)
        {
            limit.ID = (Guid)reader[string.Format("{0}id", prefix)];
            limit.Name = reader[string.Format("{0}name", prefix)].ToString();
            limit.Type = reader[string.Format("{0}type", prefix)].ToString();
            limit.Configuration = reader[string.Format("{0}configuration", prefix)].ToString();
            limit.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            limit.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
        }


        /// <summary>
        /// 获取应用程序锁数据操作
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetApplicationLockStoreSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],
                                {0}.[name] as [{0}name],
                                {0}.[type] as [{0}type],
                                {0}.[configuration] as [{0}configuration],                                           
                                {0}.[createtime] as [{0}createtime],
                                {0}.[modifytime] as [{0}modifytime],
                                {0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],
                               [name],
                               [type],
                               [configuration],
                               [createtime],
                               [modifytime],
                               [sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 赋值应用程序锁数据操作
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetApplicationLockStoreSelectFields(ApplicationLock alock, DbDataReader reader, string prefix)
        {
            alock.ID = (Guid)reader[string.Format("{0}id", prefix)];
            alock.Name = reader[string.Format("{0}name", prefix)].ToString();
            alock.Type = reader[string.Format("{0}type", prefix)].ToString();
            alock.Configuration = reader[string.Format("{0}configuration", prefix)].ToString();
            alock.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            alock.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
        }




    }
}
