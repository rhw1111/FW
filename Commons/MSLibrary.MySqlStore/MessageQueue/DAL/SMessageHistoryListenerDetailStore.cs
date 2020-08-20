using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using MSLibrary.LanguageTranslate;
using MSLibrary.DAL;
using MSLibrary.DI;
using MSLibrary.Transaction;
using MSLibrary.Collections.Hash;
using MSLibrary.MessageQueue;
using MSLibrary.MessageQueue.DAL;

namespace MSLibrary.MySqlStore.MessageQueue.DAL
{
    [Injection(InterfaceType = typeof(ISMessageHistoryListenerDetailStore), Scope = InjectionScope.Singleton)]
    public class SMessageHistoryListenerDetailStore : ISMessageHistoryListenerDetailStore
    {
        private static string _messageHistoryListenerDetailHashGroupName;
        /// <summary>
        /// 消息历史监听明细记录需要用到的一致性哈希组的名称
        /// 需要在系统初始化时赋值
        /// </summary>
        public static string MessageHistoryListenerDetailHashGroupName
        {
            set
            {
                _messageHistoryListenerDetailHashGroupName = value;
            }
        }


        private IHashGroupRepositoryCacheProxy _hashGroupRepository;
        private IMessageQueueConnectionFactory _messageQueueConnectionFactory;

        private IStoreInfoResolveService _storeInfoResolveService;


        public SMessageHistoryListenerDetailStore(IHashGroupRepositoryCacheProxy hashGroupRepository, IMessageQueueConnectionFactory messageQueueConnectionFactory, IStoreInfoResolveService storeInfoResolveService)
        {
            _hashGroupRepository = hashGroupRepository;
            _messageQueueConnectionFactory = messageQueueConnectionFactory;
            _storeInfoResolveService = storeInfoResolveService;
        }


        public async Task Add(SMessageHistoryListenerDetail detail)
        {
            var storeInfo = await StoreInfoHelper.GetHashStoreInfo(_storeInfoResolveService, _hashGroupRepository, _messageHistoryListenerDetailHashGroupName, detail.SMessageHistoryID.ToString());

            if (!storeInfo.TableNames.TryGetValue(HashEntityNames.SMessageHistoryListenerDetail, out string tableNameListenerDetail))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _messageHistoryListenerDetailHashGroupName, HashEntityNames.SMessageHistoryListenerDetail }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }
            if (!storeInfo.TableNames.TryGetValue(HashEntityNames.SMessageHistory, out string tableNameHistory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _messageHistoryListenerDetailHashGroupName, HashEntityNames.SMessageHistory }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _messageQueueConnectionFactory.CreateAllForSMessageHistoryListenerDetail(storeInfo.DBConnectionNames), async (conn, transaction) =>
            {
                //新增
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }
                await using (MySqlCommand command = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                })
                {
                    MySqlParameter parameter;
                    if (detail.ID == Guid.Empty)
                    {
                        detail.ID = Guid.NewGuid();
                    }

                        command.CommandText = string.Format(@"
                                                INSERT INTO {0}
                                                     (
		                                               id
                                                      ,smessagehistoryid
	                                                  ,listenername
	                                                  ,listenermode
	                                                  ,listenerfactorytype
	                                                  ,listenerweburl
	                                                  ,listenerrealweburl
                                                      ,createtime
                                                      ,modifytime
                                                     )
                                                VALUES
                                                    (
                                                      @id
                                                    , @smessagehistoryid
                                                    , @listenername
                                                    , @listenermode
                                                    , @listenerfactorytype
                                                    , @listenerweburl
                                                    , @listenerrealweburl
                                                    , utc_timestamp()
                                                    , utc_timestamp());", tableNameListenerDetail);

                        parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                        {
                            Value = detail.ID
                        };
                        command.Parameters.Add(parameter);
                    

                    parameter = new MySqlParameter("@smessagehistoryid", MySqlDbType.Guid)
                    {
                        Value = detail.SMessageHistoryID
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@listenername", MySqlDbType.VarChar, 150)
                    {
                        Value = detail.ListenerName
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@listenermode", MySqlDbType.Int32)
                    {
                        Value = detail.ListenerMode
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@listenerfactorytype", MySqlDbType.VarChar, 150)
                    {
                        Value = detail.ListenerFactoryType
                    };
                    command.Parameters.Add(parameter);

                    if (detail.ListenerWebUrl == null)
                    {
                        parameter = new MySqlParameter("@listenerweburl", MySqlDbType.VarChar, 200)
                        {
                            Value = DBNull.Value
                        };
                    }
                    else
                    {
                        parameter = new MySqlParameter("@listenerweburl", MySqlDbType.VarChar, 200)
                        {
                            Value = detail.ListenerWebUrl
                        };
                    }
                    command.Parameters.Add(parameter);

                    if (detail.ListenerRealWebUrl == null)
                    {
                        parameter = new MySqlParameter("@listenerrealweburl", MySqlDbType.VarChar, 200)
                        {
                            Value = DBNull.Value
                        };
                    }
                    else
                    {
                        parameter = new MySqlParameter("@listenerrealweburl", MySqlDbType.VarChar, 200)
                        {
                            Value = detail.ListenerRealWebUrl
                        };
                    }
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (MySqlException ex)
                    {
                        if (ex.Number == 2601)
                        {
                            var fragment = new TextFragment()
                            {
                                Code = TextCodes.ExistSMessageHistoryDetailByName,
                                DefaultFormatting = "消息历史监听明细中存在相同的名称\"{0}\"数据",
                                ReplaceParameters = new List<object>() { detail.ListenerName }
                            };

                            throw new UtilityException((int)Errors.ExistSMessageHistoryDetailByName, fragment);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

            });
        }

        public async Task<SMessageHistoryListenerDetail> QueryByName(Guid historyId, string name)
        {
            var storeInfo = await StoreInfoHelper.GetHashStoreInfo(_storeInfoResolveService, _hashGroupRepository, _messageHistoryListenerDetailHashGroupName, historyId.ToString());

            if (!storeInfo.TableNames.TryGetValue(HashEntityNames.SMessageHistoryListenerDetail, out string tableNameListenerDetail))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _messageHistoryListenerDetailHashGroupName, HashEntityNames.SMessageHistoryListenerDetail }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }
            if (!storeInfo.TableNames.TryGetValue(HashEntityNames.SMessageHistory, out string tableNameHistory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _messageHistoryListenerDetailHashGroupName, HashEntityNames.SMessageHistory }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }

            SMessageHistoryListenerDetail smessageHistoryListenerDetail = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _messageQueueConnectionFactory.CreateReadForSMessageHistoryListenerDetail(storeInfo.DBConnectionNames), async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }
                await using (MySqlCommand command = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"SELECT {0},{1} 
                                                  FROM {2} as d join  {3} as h on (d.smessagehistoryid = h.id )
                                                  WHERE listenername=@listenername and smessagehistoryid = @historyid",
                                                  StoreHelper.GetSMessageHistoryListenerDetailSelectFields("d"), StoreHelper.GetSMessageHistorySelectFields("h"),
                                                  tableNameListenerDetail, tableNameHistory),
                    Transaction = sqlTran,
                })
                {
                    var parameter = new MySqlParameter("@listenername", MySqlDbType.VarChar, 100)
                    {
                        Value = name
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@historyid", MySqlDbType.Guid)
                    {
                        Value = historyId
                    };
                    command.Parameters.Add(parameter);


                    await command.PrepareAsync();
                    DbDataReader reader = null;

                    using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            smessageHistoryListenerDetail = new SMessageHistoryListenerDetail();
                            StoreHelper.SetSMessageHistoryListenerDetailSelectFields(smessageHistoryListenerDetail, reader, "d");
                            smessageHistoryListenerDetail.SMessageHistory = new SMessageHistory();
                            StoreHelper.SetSMessageHistorySelectFields(smessageHistoryListenerDetail.SMessageHistory, reader, "h");
                        }
                        await reader.CloseAsync();
                    }
                }

            });


            return smessageHistoryListenerDetail;
        }
    }
}
