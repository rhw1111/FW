using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace MSLibrary.MessageQueue.DAL
{
    /// <summary>
    /// 数据操作帮助类
    /// 帮助每个实体统一查询
    /// </summary>
    public static class StoreHelper
    {
        /// <summary>
        /// 获取队列数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetSQueueSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[groupname] as [{0}groupname],{0}.[interval] as [{0}interval],{0}.[storetype] as [{0}stroetype],{0}.[servername] as [{0}servername],{0}.[name] as [{0}name],{0}.[code] as [{0}code],{0}.[isdead] as [{0}isdead],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[groupname],[interval],[storetype],[servername],[name],[code],[isdead],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }
        /// <summary>
        /// 为队列从DbDataReader中赋值
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetSQueueSelectFields(SQueue queue, DbDataReader reader, string prefix)
        {
            queue.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}groupname", prefix)] != DBNull.Value)
            {
                queue.GroupName = reader[string.Format("{0}groupname", prefix)].ToString();
            }

            if (reader[string.Format("{0}interval", prefix)] != DBNull.Value)
            {
                queue.Interval = (int)reader[string.Format("{0}interval", prefix)];
            }

            if (reader[string.Format("{0}storetype", prefix)] != DBNull.Value)
            {
                queue.StoreType = (int)reader[string.Format("{0}storetype", prefix)];
            }

            if (reader[string.Format("{0}servername", prefix)] != DBNull.Value)
            {
                queue.ServerName = reader[string.Format("{0}servername", prefix)].ToString();
            }

            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                queue.Name = reader[string.Format("{0}name", prefix)].ToString();
            }

            if (reader[string.Format("{0}code", prefix)] != DBNull.Value)
            {
                queue.Code = (int)reader[string.Format("{0}code", prefix)];
            }

            if (reader[string.Format("{0}isdead", prefix)] != DBNull.Value)
            {
                queue.IsDead = (bool)reader[string.Format("{0}isdead", prefix)];
            }

            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                queue.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }

            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                queue.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }
        }

        /// <summary>
        /// 获取队列执行组数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetSQueueProcessGroupSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[name] as [{0}name],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[name],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 为队列执行组从DbDataReader中赋值
        /// </summary>
        /// <param name="group"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetSQueueProcessGroupSelectFields(SQueueProcessGroup group, DbDataReader reader, string prefix)
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


        /// <summary>
        /// 获取消息执行类型数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetSMessageExecuteTypeSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[name] as [{0}name],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[name],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 为消息执行类型从DbDataReader中赋值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetSMessageExecuteTypeSelectFields(SMessageExecuteType type, DbDataReader reader, string prefix)
        {
            type.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                type.Name = reader[string.Format("{0}name", prefix)].ToString();
            }

            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                type.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }

            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                type.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }

        }



        /// <summary>
        /// 获取消息执行类型监听数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetSMessageTypeListenerSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[name] as [{0}name],{0}.QueueGroupName as [{0}QueueGroupName],{0}.[mode] as [{0}mode],{0}.[listenerfactorytype] as [{0}listenerfactorytype],{0}.[listenerfactorytypeusedi] as [{0}listenerfactorytypeusedi],{0}.[listenerweburl] as [{0}listenerweburl],{0}.[listenerwebsignature] as [{0}listenerwebsignature],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[name],[QueueGroupName],[mode],[listenerfactorytype],[listenerfactorytypeusedi],[listenerweburl],[listenerwebsignature],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 为消息执行类型监听从DbDataReader中赋值
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetSMessageTypeListenerSelectFields(SMessageTypeListener listener, DbDataReader reader, string prefix)
        {
            listener.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                listener.Name = reader[string.Format("{0}name", prefix)].ToString();
            }

            if (reader[string.Format("{0}queuegroupname", prefix)] != DBNull.Value)
            {
                listener.QueueGroupName = reader[string.Format("{0}queuegroupname", prefix)].ToString();
            }

            if (reader[string.Format("{0}mode", prefix)] != DBNull.Value)
            {
                listener.Mode = (int)reader[string.Format("{0}mode", prefix)];
            }

            if (reader[string.Format("{0}listenerfactorytype", prefix)] != DBNull.Value)
            {
                listener.ListenerFactoryType = reader[string.Format("{0}listenerfactorytype", prefix)].ToString();
            }

            if (reader[string.Format("{0}listenerfactorytypeusedi", prefix)] != DBNull.Value)
            {
                listener.ListenerFactoryTypeUseDI = (bool)reader[string.Format("{0}listenerfactorytypeusedi", prefix)];
            }

            if (reader[string.Format("{0}listenerweburl", prefix)] != DBNull.Value)
            {
                listener.ListenerWebUrl = reader[string.Format("{0}listenerweburl", prefix)].ToString();
            }

            if (reader[string.Format("{0}listenerwebsignature", prefix)] != DBNull.Value)
            {
                listener.ListenerWebSignature = reader[string.Format("{0}listenerwebsignature", prefix)].ToString();
            }

            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                listener.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }

            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                listener.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }

        }

        /// <summary>
        /// 获取消息数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetSMessageSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[key] as [{0}key],{0}.[type] as [{0}type],{0}.[data] as [{0}data],{0}.[typelistenerid] as [{0}typelistenerid],{0}.[originalmessageid] as [{0}originalmessageid],{0}.[delaymessageid] as [{0}delaymessageid],{0}.[extensionmessage] as [{0}extensionmessage],{0}.[createtime] as [{0}createtime],{0}.[expectationexecutetime] as [{0}expectationexecutetime],{0}.[lastexecutetime] as [{0}lastexecutetime],{0}.[retrynumber] as [{0}retrynumber],{0}.[exceptionmessage] as [{0}exceptionmessage],{0}.[isdead] as [{0}isdead],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[key],[type],[data],[typelistenerid],[originalmessageid],[delaymessageid],[extensionmessage],[createtime],[expectationexecutetime],[lastexecutetime],[retrynumber],[exceptionmessage],[isdead],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }


        public static void SetSMessageSelectFields(SMessage message, DbDataReader reader, string prefix)
        {
            message.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}key", prefix)] != DBNull.Value)
            {
                message.Key = reader[string.Format("{0}key", prefix)].ToString();
            }

            if (reader[string.Format("{0}type", prefix)] != DBNull.Value)
            {
                message.Type = reader[string.Format("{0}type", prefix)].ToString();
            }

            if (reader[string.Format("{0}data", prefix)] != DBNull.Value)
            {
                message.Data = (string)reader[string.Format("{0}data", prefix)];
            }

            if (reader[string.Format("{0}typelistenerid", prefix)] != DBNull.Value)
            {
                message.TypeListenerID = (Guid)reader[string.Format("{0}typelistenerid", prefix)];
            }

            if (reader[string.Format("{0}originalmessageid", prefix)] != DBNull.Value)
            {
                message.OriginalMessageID = (Guid)reader[string.Format("{0}originalmessageid", prefix)];
            }

            if (reader[string.Format("{0}delaymessageid", prefix)] != DBNull.Value)
            {
                message.DelayMessageID = (Guid)reader[string.Format("{0}delaymessageid", prefix)];
            }
          

            if (reader[string.Format("{0}extensionmessage", prefix)] != DBNull.Value)
            {
                message.ExtensionMessage = (string)reader[string.Format("{0}extensionmessage", prefix)];
            }


            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                message.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }

            if (reader[string.Format("{0}expectationexecutetime", prefix)] != DBNull.Value)
            {
                message.ExpectationExecuteTime = (DateTime)reader[string.Format("{0}expectationexecutetime", prefix)];
            }

            if (reader[string.Format("{0}lastexecutetime", prefix)] != DBNull.Value)
            {
                message.LastExecuteTime = (DateTime)reader[string.Format("{0}lastexecutetime", prefix)];
            }

            if (reader[string.Format("{0}retrynumber", prefix)] != DBNull.Value)
            {
                message.RetryNumber = (int)reader[string.Format("{0}retrynumber", prefix)];
            }

            if (reader[string.Format("{0}exceptionmessage", prefix)] != DBNull.Value)
            {
                message.ExceptionMessage = (string)reader[string.Format("{0}exceptionmessage", prefix)];
            }

            if (reader[string.Format("{0}isdead", prefix)] != DBNull.Value)
            {
                message.IsDead = (bool)reader[string.Format("{0}isdead", prefix)];
            }

        }

        /// <summary>
        /// 获取客户端消息类型监听终结点查询
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetClientSMessageTypeListenerEndpointSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id]
                                ,{0}.[name] as [{0}name]
                                ,{0}.[signaturekey] as [{0}signaturekey]                                
                                ,{0}.[createtime] as [{0}createtime]
                                ,{0}.[modifytime] as [{0}modifytime]
                                ,{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[name],[signaturekey],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 设置客户端消息类型监听终结点赋值
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetClientSMessageTypeListenerEndpointSelectFields(ClientSMessageTypeListenerEndpoint endpoint, DbDataReader reader, string prefix)
        {
            endpoint.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                endpoint.Name = reader[string.Format("{0}name", prefix)].ToString();
            }
            if (reader[string.Format("{0}signaturekey", prefix)] != DBNull.Value)
            {
                endpoint.SignatureKey = reader[string.Format("{0}signaturekey", prefix)].ToString();
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

        /// <summary>
        /// 获取消息历史记录数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetSMessageHistorySelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[key] as [{0}key],{0}.[type] as [{0}type],{0}.[data] as [{0}data],{0}.[originalmessageid] as [{0}originalmessageid],{0}.[delaymessageid] as [{0}delaymessageid] ,{0}.[status] as [{0}status],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[key],[type],[data],[originalmessageid],[delaymessageid],[status],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }
        /// <summary>
        /// 为消息历史记录从DbDataReader中赋值
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetSMessageHistorySelectFields(SMessageHistory history, DbDataReader reader, string prefix)
        {
            history.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}key", prefix)] != DBNull.Value)
            {
                history.Key = reader[string.Format("{0}key", prefix)].ToString();
            }

            if (reader[string.Format("{0}type", prefix)] != DBNull.Value)
            {
                history.Type = reader[string.Format("{0}type", prefix)].ToString();
            }

            if (reader[string.Format("{0}data", prefix)] != DBNull.Value)
            {
                history.Data = reader[string.Format("{0}data", prefix)].ToString();
            }

            if (reader[string.Format("{0}originalmessageid", prefix)] != DBNull.Value)
            {
                history.OriginalMessageID = (Guid)reader[string.Format("{0}originalmessageid", prefix)];
            }

            if (reader[string.Format("{0}delaymessageid", prefix)] != DBNull.Value)
            {
                history.DelayMessageID = (Guid)reader[string.Format("{0}delaymessageid", prefix)];
            }

            

            if (reader[string.Format("{0}status", prefix)] != DBNull.Value)
            {
                history.Status = Convert.ToInt16(reader[string.Format("{0}status", prefix)].ToString());
            }

            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                history.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }
            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                history.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }

        }
        /// <summary>
        /// 获取消息历史监听记录数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetSMessageHistoryListenerDetailSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id]
                            ,{0}.[SMessageHistoryID] as [{0}SMessageHistoryID]
                            ,{0}.[ListenerName] as [{0}ListenerName]
                            ,{0}.[ListenerMode] as [{0}ListenerMode]
                            ,{0}.[ListenerFactoryType] as [{0}ListenerFactoryType]
                            ,{0}.[ListenerWebUrl] as [{0}ListenerWebUrl]
                            ,{0}.[ListenerRealWebUrl] as [{0}ListenerRealWebUrl]
                            ,{0}.[createtime] as [{0}createtime]
                            ,{0}.[modifytime] as [{0}modifytime]
                            ,{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id]
                            ,[SMessageHistoryID]
                            ,[ListenerName]
                            ,[ListenerMode]
                            ,[ListenerFactoryType]
                            ,[ListenerWebUrl]
                            ,[ListenerRealWebUrl]
                            ,[createtime]
                            ,[modifytime]
                            ,[sequence]";
            }
            return string.Format(strSelect, prefix);
        }
        /// <summary>
        /// 为消息历史监听记录从DbDataReader中赋值
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetSMessageHistoryListenerDetailSelectFields(SMessageHistoryListenerDetail detail, DbDataReader reader, string prefix)
        {
            detail.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}SMessageHistoryID", prefix)] != DBNull.Value)
            {
                detail.SMessageHistoryID = new Guid(reader[string.Format("{0}SMessageHistoryID", prefix)].ToString());
            }
            if (reader[string.Format("{0}ListenerName", prefix)] != DBNull.Value)
            {
                detail.ListenerName = reader[string.Format("{0}ListenerName", prefix)].ToString();
            }
            if (reader[string.Format("{0}ListenerMode", prefix)] != DBNull.Value)
            {
                detail.ListenerMode = int.Parse(reader[string.Format("{0}ListenerMode", prefix)].ToString());
            }
            if (reader[string.Format("{0}ListenerFactoryType", prefix)] != DBNull.Value)
            {
                detail.ListenerFactoryType = reader[string.Format("{0}ListenerFactoryType", prefix)].ToString();
            }
            if (reader[string.Format("{0}ListenerWebUrl", prefix)] != DBNull.Value)
            {
                detail.ListenerWebUrl = reader[string.Format("{0}ListenerWebUrl", prefix)].ToString();
            }
            if (reader[string.Format("{0}ListenerRealWebUrl", prefix)] != DBNull.Value)
            {
                detail.ListenerRealWebUrl = reader[string.Format("{0}ListenerRealWebUrl", prefix)].ToString();
            }
            if (reader[string.Format("{0}createtime", prefix)] != DBNull.Value)
            {
                detail.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            }
            if (reader[string.Format("{0}modifytime", prefix)] != DBNull.Value)
            {
                detail.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
            }

        }
    }
}
