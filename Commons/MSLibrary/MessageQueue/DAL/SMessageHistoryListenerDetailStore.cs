using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.Collections.Hash;
using MSLibrary.DAL;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Transaction;

namespace MSLibrary.MessageQueue.DAL
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

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _messageQueueConnectionFactory.CreateAllForSMessageHistoryListenerDetail(storeInfo.DBConnectionNames), async (conn, transaction) =>
            {
                //新增
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }
                await using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                })
                {
                    SqlParameter parameter;
                    if (detail.ID == Guid.Empty)
                    {
                        command.CommandText = string.Format(@"
                                                INSERT INTO {0}
                                                     (
		                                               [id]
                                                      ,[SMessageHistoryID]
	                                                  ,[ListenerName]
	                                                  ,[ListenerMode]
	                                                  ,[ListenerFactoryType]
	                                                  ,[ListenerWebUrl]
	                                                  ,[ListenerRealWebUrl]
                                                      ,[createtime]
                                                      ,[modifytime]
                                                     )
                                                VALUES
                                                    (default
                                                    , @SMessageHistoryID
                                                    , @ListenerName
                                                    , @ListenerMode
                                                    , @ListenerFactoryType
                                                    , @ListenerWebUrl
                                                    , @ListenerRealWebUrl
                                                    , GETUTCDATE()
                                                    , GETUTCDATE());
                                                select @newid =[id] from {0} where [sequence] = SCOPE_IDENTITY()", tableNameListenerDetail);
                        parameter = new SqlParameter("@newid", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        command.CommandText = string.Format(@"
                                                INSERT INTO {0}
                                                     (
		                                               [id]
                                                      ,[SMessageHistoryID]
	                                                  ,[ListenerName]
	                                                  ,[ListenerMode]
	                                                  ,[ListenerFactoryType]
	                                                  ,[ListenerWebUrl]
	                                                  ,[ListenerRealWebUrl]
                                                      ,[createtime]
                                                      ,[modifytime]
                                                     )
                                                VALUES
                                                    (
                                                      @id
                                                    , @SMessageHistoryID
                                                    , @ListenerName
                                                    , @ListenerMode
                                                    , @ListenerFactoryType
                                                    , @ListenerWebUrl
                                                    , @ListenerRealWebUrl
                                                    , GETUTCDATE()
                                                    , GETUTCDATE());", tableNameListenerDetail);

                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = detail.ID
                        };
                        command.Parameters.Add(parameter);
                    }

                    parameter = new SqlParameter("@SMessageHistoryID", SqlDbType.UniqueIdentifier)
                    {
                        Value = detail.SMessageHistoryID
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@ListenerName", SqlDbType.VarChar, 150)
                    {
                        Value = detail.ListenerName
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@ListenerMode", SqlDbType.Int)
                    {
                        Value = detail.ListenerMode
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@ListenerFactoryType", SqlDbType.VarChar, 150)
                    {
                        Value = detail.ListenerFactoryType
                    };
                    command.Parameters.Add(parameter);

                    if (detail.ListenerWebUrl == null)
                    {
                        parameter = new SqlParameter("@ListenerWebUrl", SqlDbType.VarChar, 200)
                        {
                            Value = DBNull.Value
                        };
                    }
                    else
                    {
                        parameter = new SqlParameter("@ListenerWebUrl", SqlDbType.VarChar, 200)
                        {
                            Value = detail.ListenerWebUrl
                        };
                    }
                    command.Parameters.Add(parameter);

                    if (detail.ListenerRealWebUrl == null)
                    {
                        parameter = new SqlParameter("@ListenerRealWebUrl", SqlDbType.VarChar, 200)
                        {
                            Value = DBNull.Value
                        };
                    }
                    else
                    {
                        parameter = new SqlParameter("@ListenerRealWebUrl", SqlDbType.VarChar, 200)
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
                    catch (SqlException ex)
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

                    //如果用户未赋值ID则创建成功后返回ID
                    if (detail.ID == Guid.Empty)
                    {
                        detail.ID = (Guid)command.Parameters["@newid"].Value;
                    };
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

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false,_messageQueueConnectionFactory.CreateReadForSMessageHistoryListenerDetail(storeInfo.DBConnectionNames), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }
                await using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"SELECT {0},{1} 
                                                  FROM {2} as d join  {3} as h on (d.smessagehistoryid = h.id )
                                                  WHERE [ListenerName]=@listenername and SMessageHistoryID = @historyId",
                                                  StoreHelper.GetSMessageHistoryListenerDetailSelectFields("d"), StoreHelper.GetSMessageHistorySelectFields("h"),
                                                  tableNameListenerDetail, tableNameHistory),
                    Transaction = sqlTran,
                })
                {
                    var parameter = new SqlParameter("@listenername", SqlDbType.NVarChar, 100)
                    {
                        Value = name
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@historyId", SqlDbType.UniqueIdentifier)
                    {
                        Value = historyId
                    };
                    command.Parameters.Add(parameter);


                    await command.PrepareAsync();
                    SqlDataReader reader = null;

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
