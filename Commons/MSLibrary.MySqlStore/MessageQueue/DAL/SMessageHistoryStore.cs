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
    [Injection(InterfaceType = typeof(ISMessageHistoryStore), Scope = InjectionScope.Singleton)]
    public class SMessageHistoryStore : ISMessageHistoryStore
    {
        private static string _messageHistoryHashGroupName;
        /// <summary>
        /// 消息历史记录需要用到的一致性哈希组的名称
        /// 需要在系统初始化时赋值
        /// </summary>
        public static string MessageHistoryHashGroupName
        {
            set
            {
                _messageHistoryHashGroupName = value;
            }
        }


        private IHashGroupRepositoryCacheProxy _hashGroupRepository;
        private IMessageQueueConnectionFactory _messageQueueConnectionFactory;

        private IStoreInfoResolveService _storeInfoResolveService;


        public SMessageHistoryStore(IHashGroupRepositoryCacheProxy hashGroupRepository, IMessageQueueConnectionFactory messageQueueConnectionFactory, IStoreInfoResolveService storeInfoResolveService)
        {
            _hashGroupRepository = hashGroupRepository;
            _messageQueueConnectionFactory = messageQueueConnectionFactory;
            _storeInfoResolveService = storeInfoResolveService;
        }

        public async Task Add(SMessageHistory history)
        {
            var storeInfo = await StoreInfoHelper.GetHashStoreInfo(_storeInfoResolveService, _hashGroupRepository, _messageHistoryHashGroupName, history.ID.ToString());

            if (!storeInfo.TableNames.TryGetValue(HashEntityNames.SMessageHistory, out string tableName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _messageHistoryHashGroupName, HashEntityNames.SMessageHistory }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _messageQueueConnectionFactory.CreateAllForSMessageHistory(storeInfo.DBConnectionNames), async (conn, transaction) =>
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
                    if (history.ID == Guid.Empty)
                    {
                        history.ID = Guid.NewGuid();
                    }

                        command.CommandText = string.Format(@"
                                                INSERT INTO {0}
                                                     (
                                                      id
                                                      ,key
	                                                  ,type
	                                                  ,data
                                                      ,originalmessageid
                                                      ,delaymessageid
	                                                  ,status
                                                      ,createtime
                                                      ,modifytime
                                                     )
                                                VALUES
                                                    ( 
                                                      @id
                                                    , @key
                                                    , @type
                                                    , @data
                                                    , @originalmessageid
                                                    , @delaymessageid
                                                    , @status
                                                    , utc_timestamp()
                                                    , utc_timestamp());", tableName);

                        parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                        {
                            Value = history.ID
                        };
                        command.Parameters.Add(parameter);
                    

                    parameter = new MySqlParameter("@key", MySqlDbType.VarChar, 150)
                    {
                        Value = history.Key
                    };
                    command.Parameters.Add(parameter);
                    parameter = new MySqlParameter("@type", MySqlDbType.VarChar, 150)
                    {
                        Value = history.Type
                    };
                    command.Parameters.Add(parameter);
                    parameter = new MySqlParameter("@data", MySqlDbType.VarChar, history.Data.Length)
                    {
                        Value = history.Data
                    };
                    command.Parameters.Add(parameter);

                    if (history.OriginalMessageID.HasValue)
                    {
                        parameter = new MySqlParameter("@originalmessageid", MySqlDbType.Guid)
                        {
                            Value = history.OriginalMessageID
                        };
                    }
                    else
                    {
                        parameter = new MySqlParameter("@originalmessageid", MySqlDbType.Guid)
                        {
                            Value = DBNull.Value
                        };
                    }
                    command.Parameters.Add(parameter);

                    if (history.DelayMessageID.HasValue)
                    {
                        parameter = new MySqlParameter("@delaymessageid", MySqlDbType.Guid)
                        {
                            Value = history.DelayMessageID
                        };
                    }
                    else
                    {
                        parameter = new MySqlParameter("@delaymessageid", MySqlDbType.Guid)
                        {
                            Value = DBNull.Value
                        };
                    }
                    command.Parameters.Add(parameter);



                    parameter = new MySqlParameter("@status", MySqlDbType.Int32)
                    {
                        Value = history.Status
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();


                    await command.ExecuteNonQueryAsync();

                }

            });

        }

        public async Task<SMessageHistory> QueryById(Guid id)
        {
            var storeInfo = await StoreInfoHelper.GetHashStoreInfo(_storeInfoResolveService, _hashGroupRepository, _messageHistoryHashGroupName, id.ToString());

            if (!storeInfo.TableNames.TryGetValue(HashEntityNames.SMessageHistory, out string tableName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _messageHistoryHashGroupName, HashEntityNames.SMessageHistory }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }
            SMessageHistory smssageHistory = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _messageQueueConnectionFactory.CreateReadForSMessageHistory(storeInfo.DBConnectionNames), async (conn, transaction) =>
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
                    CommandText = string.Format(@"SELECT {0} FROM {1} WHERE id=@id", StoreHelper.GetSMessageHistorySelectFields(string.Empty), tableName),
                    Transaction = sqlTran,
                })
                {
                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);
                    command.Prepare();
                    DbDataReader reader = null;

                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            smssageHistory = new SMessageHistory();
                            StoreHelper.SetSMessageHistorySelectFields(smssageHistory, reader, string.Empty);
                        }
                        await reader.CloseAsync();
                    }
                }

            });


            return smssageHistory;
        }

        public async Task UpdateStatus(Guid id, int status)
        {
            var storeInfo = await StoreInfoHelper.GetHashStoreInfo(_storeInfoResolveService, _hashGroupRepository, _messageHistoryHashGroupName, id.ToString());

            if (!storeInfo.TableNames.TryGetValue(HashEntityNames.SMessageHistory, out string tableName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _messageHistoryHashGroupName, HashEntityNames.SMessageHistory }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _messageQueueConnectionFactory.CreateAllForSMessageHistory(storeInfo.DBConnectionNames), async (conn, transaction) =>
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
                    CommandText = string.Format(@"update {0} set status = @status WHERE id=@id", tableName),
                    Transaction = sqlTran,
                })
                {
                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);


                    parameter = new MySqlParameter("@status", MySqlDbType.Int32)
                    {
                        Value = status
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    await command.ExecuteNonQueryAsync();

                }
            });
        }
    }
}
