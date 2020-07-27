using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using MSLibrary.Configuration;
using MSLibrary.Schedule;

namespace MSLibrary.MySqlStore.Schedule.DAL
{
    /// <summary>
    /// 数据存储帮助类
    /// 统一管理查询赋值
    /// </summary>
    public static class StoreHelper
    {
        /// <summary>
        /// 获取调度动作数据操作
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetScheduleActionStoreSelectFields(string prefix)
        {
            var strSelect = @"{0}.id as {0}id,
                                {0}.name as {0}name,
                                {0}.groupid as {0}groupid,
                                {0}.triggercondition as {0}triggercondition,
                                {0}.configuration as {0}configuration,
                                {0}.mode as {0}mode,
                                {0}.scheduleactionservicefactorytype as {0}scheduleactionservicefactorytype,
                                {0}.scheduleactionservicefactorytypeusedi as {0}scheduleactionservicefactorytypeusedi
                                {0}.scheduleactionserviceweburl as {0}scheduleactionserviceweburl,
                                {0}.websignature as {0}websignature,
                                {0}.status as {0}status,
                                {0}.createtime as {0}createtime,
                                {0}.modifytime as {0}modifytime,
                                {0}.sequence as {0}sequence";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"id,
                               name,
                               groupid,
                               triggercondition,
                               configuration,
                               mode,
                               scheduleactionservicefactorytype,
                               scheduleactionservicefactorytypeusedi,
                               scheduleactionserviceweburl,
                               websignature,
                               status,
                               createtime,
                               modifytime,
                               sequence";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 赋值调度动作数据操作
        /// </summary>
        /// <param name="action"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetScheduleActionStoreSelectFields(ScheduleAction action, DbDataReader reader, string prefix)
        {
            action.ID = (Guid)reader[string.Format("{0}id", prefix)];
            action.Name = reader[string.Format("{0}name", prefix)].ToString();
            action.TriggerCondition = reader[string.Format("{0}triggercondition", prefix)].ToString();
            action.Configuration = reader[string.Format("{0}configuration", prefix)].ToString();
            action.Mode = (int)reader[string.Format("{0}Mode", prefix)];
            if (reader[string.Format("{0}ScheduleActionServiceFactoryType", prefix)] != DBNull.Value)
            {
                action.ScheduleActionServiceFactoryType = reader[string.Format("{0}ScheduleActionServiceFactoryType", prefix)].ToString();
            }
            if (reader[string.Format("{0}ScheduleActionServiceFactoryTypeUseDI", prefix)] != DBNull.Value)
            {
                action.ScheduleActionServiceFactoryTypeUseDI = (bool)reader[string.Format("{0}ScheduleActionServiceFactoryTypeUseDI", prefix)];
            }
            if (reader[string.Format("{0}ScheduleActionServiceWebUrl", prefix)] != DBNull.Value)
            {
                action.ScheduleActionServiceWebUrl = reader[string.Format("{0}ScheduleActionServiceWebUrl", prefix)].ToString();
            }
            if (reader[string.Format("{0}WebSignature", prefix)] != DBNull.Value)
            {
                action.WebSignature = reader[string.Format("{0}WebSignature", prefix)].ToString();
            }
            action.Status = (int)reader[string.Format("{0}Status", prefix)];
            action.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            action.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
        }

        /// <summary>
        /// 获取调度动作组数据操作
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetScheduleActionGroupSelectFields(string prefix)
        {
            var strSelect = @"{0}.id as {0}id,
                              {0}.name as {0}name,
                              {0}.createtime as {0}createtime,
                              {0}.modifytime as {0}modifytime,
                              {0}.uselog as {0}uselog,
                              {0}.executeactioninittype as {0}executeactioninittype,
                              {0}.executeactioninitconfiguration as {0}executeactioninitconfiguration
                              ";

            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"id
                             ,name
                             ,createtime
                             ,modifytime
                             ,uselog
                             ,executeactioninittype
                             ,executeactioninitconfiguration";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 赋值调度动作组数据操作
        /// </summary>
        /// <param name="actionGroup"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetScheduleActionGroupSelectFields(ScheduleActionGroup actionGroup, DbDataReader reader, string prefix)
        {
            actionGroup.ID = (Guid)reader[string.Format("{0}id", prefix)];
            actionGroup.Name = reader[string.Format("{0}name", prefix)].ToString();
            actionGroup.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            actionGroup.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            actionGroup.UseLog = (bool)reader[string.Format("{0}uselog", prefix)];
            actionGroup.ExecuteActionInitType = reader[string.Format("{0}executeactioninittype", prefix)].ToString();
            actionGroup.ExecuteActionInitConfiguration = reader[string.Format("{0}executeactioninitconfiguration", prefix)].ToString();
        }
    }
}
