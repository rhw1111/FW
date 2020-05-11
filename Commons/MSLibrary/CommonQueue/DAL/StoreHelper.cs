using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace MSLibrary.CommonQueue.DAL
{
    public static class StoreHelper
    {
        /// <summary>
        /// 获取通用队列消费终结点的数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetCommonQueueConsumeEndpointSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[name] as [{0}name],{0}.[queuetype] as [{0}queuetype],{0}.[queueconfiguration] as [{0}queueconfiguration],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[name],[queuetype],[queueconfiguration],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 为通用队列消费终结点从DbDataReader中赋值
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetCommonQueueConsumeEndpointSelectFields(CommonQueueConsumeEndpoint endpoint, DbDataReader reader, string prefix)
        {
            endpoint.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                endpoint.Name = reader[string.Format("{0}name", prefix)].ToString();
            }

            if (reader[string.Format("{0}queuetype", prefix)] != DBNull.Value)
            {
                endpoint.QueueType = reader[string.Format("{0}queuetype", prefix)].ToString();
            }

            if (reader[string.Format("{0}queueconfiguration", prefix)] != DBNull.Value)
            {
                endpoint.QueueConfiguration = reader[string.Format("{0}queueconfiguration", prefix)].ToString();
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
        /// 获取通用队列生产终结点的数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetCommonQueueProductEndpointSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[name] as [{0}name],{0}.[queuetype] as [{0}queuetype],{0}.[queueconfiguration] as [{0}queueconfiguration],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[name],[queuetype],[queueconfiguration],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 为通用队列生产终结点从DbDataReader中赋值
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetCommonQueueProductEndpointSelectFields(CommonQueueProductEndpoint endpoint, DbDataReader reader, string prefix)
        {
            endpoint.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                endpoint.Name = reader[string.Format("{0}name", prefix)].ToString();
            }

            if (reader[string.Format("{0}queuetype", prefix)] != DBNull.Value)
            {
                endpoint.QueueType = reader[string.Format("{0}queuetype", prefix)].ToString();
            }

            if (reader[string.Format("{0}queueconfiguration", prefix)] != DBNull.Value)
            {
                endpoint.QueueConfiguration = reader[string.Format("{0}queueconfiguration", prefix)].ToString();
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
        /// 获取通用消息客户端类型的数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetCommonMessageClientTypeFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[name] as [{0}name],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[name],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 为通用消息客户端类型从DbDataReader中赋值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetCommonMessageClientTypeSelectFields(CommonMessageClientType type, DbDataReader reader, string prefix)
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


    }
}
