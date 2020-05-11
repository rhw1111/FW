using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace MSLibrary.SocketManagement.DAL
{
    /// <summary>
    /// 数据操作帮助类
    /// 帮助每个实体统一查询
    /// </summary>
    public static class StoreHelper
    {
        /// <summary>
        /// 获取Tcp监听器数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetTcpListenerSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[name] as [{0}name],{0}.[port] as [{0}port],{0}.[keepalive] as [{0}keepalive],{0}.[maxconcurrencycount] as [{0}maxconcurrencycount],{0}.[maxbuffercount] as [{0}maxbuffercount],{0}.[executedatafactorytype] as [{0}executedatafactorytype],{0}.[executedatafactorytypeusedi] as [executedatafactorytypeusedi],{0}.[heartbeatsenddata] as [{0}heartbeatsenddata],{0}.[description] as [{0}description],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[name],[port],[keepalive],[maxconcurrencycount],[maxbuffercount],[executedatafactorytype],[executedatafactorytypeusedi],[heartbeatsenddata],[description],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }
        /// <summary>
        /// 为Tcp监听器从DbDataReader中赋值
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetTcpListenerSelectFields(TcpListener listener, DbDataReader reader, string prefix)
        {
            listener.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                listener.Name = reader[string.Format("{0}name", prefix)].ToString();
            }

            if (reader[string.Format("{0}port", prefix)] != DBNull.Value)
            {
                listener.Port = (int)reader[string.Format("{0}port", prefix)];
            }

            if (reader[string.Format("{0}keepalive", prefix)] != DBNull.Value)
            {
                listener.KeepAlive = (bool)reader[string.Format("{0}keepalive", prefix)];
            }

            if (reader[string.Format("{0}maxconcurrencycount", prefix)] != DBNull.Value)
            {
                listener.MaxConcurrencyCount = (int)reader[string.Format("{0}maxconcurrencycount", prefix)];
            }

            if (reader[string.Format("{0}maxbuffercount", prefix)] != DBNull.Value)
            {
                listener.MaxBufferCount = (int)reader[string.Format("{0}maxbuffercount", prefix)];
            }



            if (reader[string.Format("{0}executedatafactorytype", prefix)] != DBNull.Value)
            {
                listener.ExecuteDataFactoryType = reader[string.Format("{0}executedatafactorytype", prefix)].ToString();
            }

            if (reader[string.Format("{0}executedatafactorytypeusedi", prefix)] != DBNull.Value)
            {
                listener.ExecuteDataFactoryTypeUseDI =(bool)reader[string.Format("{0}executedatafactorytypeusedi", prefix)];
            }

            if (reader[string.Format("{0}heartbeatsenddata", prefix)] != DBNull.Value)
            {
                listener.HeartBeatSendData = reader[string.Format("{0}heartbeatsenddata", prefix)].ToString();
            }

            if (reader[string.Format("{0}description", prefix)] != DBNull.Value)
            {
                listener.Description = reader[string.Format("{0}description", prefix)].ToString();
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
        /// 获取Tcp监听器日志数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetTcpListenerLogSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[listenername] as [{0}listenername],{0}.[requesttime] as [{0}requesttime],{0}.[requestcontent] as [{0}requestcontent],{0}.[executeduration] as [{0}executeduration],{0}.[responsecontent] as [{0}responsecontent],{0}.[responsetime] as [{0}responsetime],{0}.[iserror] as [{0}iserror],{0}.[errormessage] as [{0}errormessage],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[listenername],[requesttime],[requestcontent],[executeduration],[responsecontent],[responsetime],[iserror],[errormessage],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 为Tcp监听器日志从DbDataReader中赋值
        /// </summary>
        /// <param name="log"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetTcpListenerLogSelectFields(TcpListenerLog log, DbDataReader reader, string prefix)
        {
            log.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}listenername", prefix)] != DBNull.Value)
            {
                log.ListenerName = reader[string.Format("{0}listenername", prefix)].ToString();
            }

            if (reader[string.Format("{0}requesttime", prefix)] != DBNull.Value)
            {
                log.RequestTime = (DateTime)reader[string.Format("{0}requesttime", prefix)];
            }

            if (reader[string.Format("{0}requestcontent", prefix)] != DBNull.Value)
            {
                log.RequestContent = reader[string.Format("{0}requestcontent", prefix)].ToString();
            }

            if (reader[string.Format("{0}executeduration", prefix)] != DBNull.Value)
            {
                log.ExecuteDuration = (int)reader[string.Format("{0}executeduration", prefix)];
            }

            if (reader[string.Format("{0}responsecontent", prefix)] != DBNull.Value)
            {
                log.ResponseContent = reader[string.Format("{0}responsecontent", prefix)].ToString();
            }

            if (reader[string.Format("{0}responsetime", prefix)] != DBNull.Value)
            {
                log.ResponseTime = (DateTime)reader[string.Format("{0}responsetime", prefix)];
            }

            if (reader[string.Format("{0}iserror", prefix)] != DBNull.Value)
            {
                log.IsError = (bool)reader[string.Format("{0}iserror", prefix)];
            }

            if (reader[string.Format("{0}errormessage", prefix)] != DBNull.Value)
            {
                log.ErrorMessage = reader[string.Format("{0}errormessage", prefix)].ToString();
            }

        }


        /// <summary>
        /// 获取Tcp客户端终结点数据查询的字段字符串
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetTcpClientEndpointSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id],{0}.[name] as [{0}name],{0}.[serveraddress] as [{0}serveraddress],{0}.[serverport] as [{0}serverport],{0}.[keepalive] as [{0}keepalive],{0}.[poolmaxsize] as [{0}poolmaxsize],{0}.[executedatafactorytype] as [{0}executedatafactorytype],{0}.[executedatafactorytypeusedi] as [executedatafactorytypeusedi],{0}.[heartbeatsenddata] as [{0}heartbeatsenddata],{0}.[createtime] as [{0}createtime],{0}.[modifytime] as [{0}modifytime],{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id],[name],[serveraddress],[serverport],[keepalive],[poolmaxsize],[executedatafactorytype],[executedatafactorytypeusedi],[heartbeatsenddata],[createtime],[modifytime],[sequence]";
            }
            return string.Format(strSelect, prefix);
        }
        /// <summary>
        /// 为Tcp客户端终结点从DbDataReader中赋值
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetTcpClientEndpointSelectFields(TcpClientEndpoint endpoint, DbDataReader reader, string prefix)
        {
            endpoint.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                endpoint.Name = reader[string.Format("{0}name", prefix)].ToString();
            }

            if (reader[string.Format("{0}serveraddress", prefix)] != DBNull.Value)
            {
                endpoint.ServerAddress = reader[string.Format("{0}serveraddress", prefix)].ToString();
            }

            if (reader[string.Format("{0}serverport", prefix)] != DBNull.Value)
            {
                endpoint.ServerPort = (int)reader[string.Format("{0}serverport", prefix)];
            }

            if (reader[string.Format("{0}keepalive", prefix)] != DBNull.Value)
            {
                endpoint.KeepAlive = (bool)reader[string.Format("{0}keepalive", prefix)];
            }

            if (reader[string.Format("{0}poolmaxsize", prefix)] != DBNull.Value)
            {
                endpoint.PoolMaxSize = (int)reader[string.Format("{0}poolmaxsize", prefix)];
            }


            if (reader[string.Format("{0}executedatafactorytype", prefix)] != DBNull.Value)
            {
                endpoint.ExecuteDataFactoryType = reader[string.Format("{0}executedatafactorytype", prefix)].ToString();
            }

            if (reader[string.Format("{0}executedatafactorytypeusedi", prefix)] != DBNull.Value)
            {
                endpoint.ExecuteDataFactoryTypeUseDI = (bool)reader[string.Format("{0}executedatafactorytypeusedi", prefix)];
            }

            if (reader[string.Format("{0}heartbeatsenddata", prefix)] != DBNull.Value)
            {
                endpoint.HeartBeatSendData = reader[string.Format("{0}heartbeatsenddata", prefix)].ToString();
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
