using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace MSLibrary.SystemToken.DAL
{
    public class StoreHelper
    {

        /// <summary>
        /// 获取终结点数据操作
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetAuthorizationEndpointStoreSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id]  as [{0}id]
                              ,{0}.[name]  as [{0}name]
                              ,{0}.[thirdpartytype]  as [{0}thirdpartytype]
                              ,{0}.[thirdpartypostexecutetype]  as [{0}thirdpartypostexecutetype]
                              ,{0}.[thirdpartyconfiguration]  as [{0}thirdpartyconfiguration]
                              ,{0}.[thirdpartypostconfiguration]  as [{0}thirdpartypostconfiguration]
                              ,{0}.[keepthirdpartytoken]  as [{0}keepthirdpartytoken]
                              ,{0}.[authmodes] as [{0}authmodes]
                              ,{0}.[createtime]  as [{0}createtime]
                              ,{0}.[modifytime]  as [{0}modifytime]
                              ,{0}.[sequence]  as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id]
                              ,[name]
                              ,[thirdpartytype]
                              ,[thirdpartypostexecutetype]
                              ,[thirdpartyconfiguration] 
                              ,[thirdpartypostconfiguration]
                              ,[keepthirdpartytoken]
                              ,[authmodes]
                              ,[createtime]
                              ,[modifytime]
                              ,[sequence]";
            }
            return string.Format(strSelect, prefix);
        }



        /// <summary>
        /// 为终结点从DbDataReader中赋值
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetAuthorizationEndpointSelectFields(AuthorizationEndpoint endpoint, DbDataReader reader, string prefix)
        {
            endpoint.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                endpoint.Name = reader[string.Format("{0}name", prefix)].ToString();
            }
            if (reader[string.Format("{0}thirdpartytype", prefix)] != DBNull.Value)
            {
                endpoint.ThirdPartyType = reader[string.Format("{0}thirdpartytype", prefix)].ToString();
            }

            if (reader[string.Format("{0}thirdpartypostexecutetype", prefix)] != DBNull.Value)
            {
                endpoint.ThirdPartyPostExecuteType = reader[string.Format("{0}thirdpartypostexecutetype", prefix)].ToString();
            }

            if (reader[string.Format("{0}thirdpartyconfiguration", prefix)] != DBNull.Value)
            {
                endpoint.ThirdPartyConfiguration = reader[string.Format("{0}thirdpartyconfiguration", prefix)].ToString();
            }

            if (reader[string.Format("{0}thirdpartypostconfiguration", prefix)] != DBNull.Value)
            {
                endpoint.ThirdPartyPostConfiguration = reader[string.Format("{0}thirdpartypostconfiguration", prefix)].ToString();
            }

            if (reader[string.Format("{0}keepthirdpartytoken", prefix)] != DBNull.Value)
            {
                endpoint.KeepThirdPartyToken = (bool)reader[string.Format("{0}keepthirdpartytoken", prefix)];
            }

            if (reader[string.Format("{0}authmodes", prefix)] != DBNull.Value)
            {
                var strModes = reader[string.Format("{0}authmodes", prefix)].ToString();
                var arrayModes = strModes.Split(',');
                var intModes = new List<int>();
                foreach(var arrayItem in arrayModes)
                {
                    intModes.Add(Convert.ToInt32(arrayItem));
                }

                endpoint.AuthModes = intModes.ToArray();
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
        /// 获取系统登录终结点数据操作
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetSystemLoginEndpointStoreSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id] as [{0}id]
                              ,{0}.[name] as [{0}name]
                              ,{0}.[secretkey] as [{0}secretkey]
                              ,{0}.[expiresecond] as [{0}expiresecond]
                              ,{0}.[clientredirectbaseurls] as [{0}clientredirectbaseurls]
                              ,{0}.[baseurl] as [{0}baseurl]
                              ,{0}.[userinfokey] as [{0}userinfokey]
                              ,{0}.[createtime] as [{0}createtime]
                              ,{0}.[modifytime] as [{0}modifytime]
                              ,{0}.[sequence] as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id]
                              ,[name]
                              ,[secretkey]
                              ,[expiresecond]
                              ,[clientredirectbaseurls]
                              ,[baseurl]
                              ,[userinfokey]
                              ,[createtime]
                              ,[modifytime]
                              ,[sequence]";
            }
            return string.Format(strSelect, prefix);
        }

        /// <summary>
        /// 赋值系统登录终结点数据操作
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetSystemLoginEndpointStoreSelectFields(SystemLoginEndpoint endpoint, DbDataReader reader, string prefix)
        {
            endpoint.ID = (Guid)reader[string.Format("{0}id", prefix)];
            if (reader[string.Format("{0}name", prefix)] != DBNull.Value)
            {
                endpoint.Name = reader[string.Format("{0}name", prefix)].ToString();
            }
            if (reader[string.Format("{0}secretkey", prefix)] != DBNull.Value)
            {
                endpoint.SecretKey = reader[string.Format("{0}SecretKey", prefix)].ToString();
            }
            if (reader[string.Format("{0}expiresecond", prefix)] != DBNull.Value)
            {
                endpoint.ExpireSecond = (int)reader[string.Format("{0}expiresecond", prefix)];
            }
            if (reader[string.Format("{0}clientredirectbaseurls", prefix)] != DBNull.Value)
            {
                string[] str = (reader[string.Format("{0}clientredirectbaseurls", prefix)].ToString()).Split(',');
                Uri[] uris = new Uri[str.Length];
                for (int i = 0; i < str.Length; i++)
                {
                    uris[i] = new Uri(str[i]);
                }
                endpoint.ClientRedirectBaseUrls = uris;
            }
            if (reader[string.Format("{0}baseurl", prefix)] != DBNull.Value)
            {
                endpoint.BaseUrl = reader[string.Format("{0}baseurl", prefix)].ToString();
            }
            if (reader[string.Format("{0}userinfokey", prefix)] != DBNull.Value)
            {
                endpoint.UserInfoKey = reader[string.Format("{0}userinfokey", prefix)].ToString();
            }
            endpoint.CreateTime = (DateTime)reader[string.Format("{0}createtime", prefix)];
            endpoint.ModifyTime = (DateTime)reader[string.Format("{0}modifytime", prefix)];
        }


        /// <summary>
        /// 获取客户端系统登陆终结点查询
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetClientSystemLoginEndpointSelectFields(string prefix)
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
        /// 设置客户端系统登陆终结点赋值
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetClientSystemLoginEndpointSelectFields(ClientSystemLoginEndpoint endpoint, DbDataReader reader, string prefix)
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
        /// 获取第三方系统令牌记录数据操作
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string GetThirdPartySystemTokenRecordStoreSelectFields(string prefix)
        {
            var strSelect = @"{0}.[id]  as [{0}id]
                              ,{0}.[userkey]  as [{0}userkey]
                              ,{0}.[systemloginendpointid]  as [{0}systemloginendpointid]
                              ,{0}.[authorizationendpointid]  as [{0}authorizationendpointid]
                              ,{0}.[token]  as [{0}token]
                              ,{0}.[timeout]  as [{0}timeout]
                              ,{0}.[lastrefeshtime]  as [{0}lastrefeshtime]                         
                              ,{0}.[createtime]  as [{0}createtime]
                              ,{0}.[modifytime]  as [{0}modifytime]
                              ,{0}.[sequence]  as [{0}sequence]";
            if (string.IsNullOrEmpty(prefix))
            {
                strSelect = @"[id]
                              ,[userkey]
                              ,[systemloginendpointid]
                              ,[authorizationendpointid]
                              ,[token]
                              ,[timeout]
                              ,[lastrefeshtime]
                              ,[createtime]
                              ,[modifytime]
                              ,[sequence]";
            }
            return string.Format(strSelect, prefix);
        }



        /// <summary>
        /// 为第三方系统令牌记录从DbDataReader中赋值
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="reader"></param>
        /// <param name="prefix"></param>
        public static void SetThirdPartySystemTokenRecordSelectFields(ThirdPartySystemTokenRecord record, DbDataReader reader, string prefix)
        {
            record.ID = (Guid)reader[string.Format("{0}id", prefix)];

            if (reader[string.Format("{0}userkey", prefix)] != DBNull.Value)
            {
                record.UserKey = reader[string.Format("{0}userkey", prefix)].ToString();
            }
            if (reader[string.Format("{0}systemloginendpointid", prefix)] != DBNull.Value)
            {
                record.SystemLoginEndpointID =(Guid) reader[string.Format("{0}systemloginendpointid", prefix)];
            }

            if (reader[string.Format("{0}authorizationendpointid", prefix)] != DBNull.Value)
            {
                record.AuthorizationEndpointID = (Guid)reader[string.Format("{0}authorizationendpointid", prefix)];
            }

            if (reader[string.Format("{0}token", prefix)] != DBNull.Value)
            {
                record.Token = reader[string.Format("{0}token", prefix)].ToString();
            }

            if (reader[string.Format("{0}timeout", prefix)] != DBNull.Value)
            {
                record.Timeout = (int)reader[string.Format("{0}timeout", prefix)];
            }

            if (reader[string.Format("{0}lastrefeshtime", prefix)] != DBNull.Value)
            {
                record.LastRefeshTime = (DateTime)reader[string.Format("{0}lastrefeshtime", prefix)];
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


    }
}
