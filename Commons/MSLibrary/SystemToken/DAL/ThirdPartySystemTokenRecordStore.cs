using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.Cache.DAL;
using MSLibrary.DAL;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Transaction;
using MSLibrary.Collections.Hash;
using MSLibrary.Thread;

namespace MSLibrary.SystemToken.DAL
{
    [Injection(InterfaceType = typeof(IThirdPartySystemTokenRecordStore), Scope = InjectionScope.Singleton)]
    public class ThirdPartySystemTokenRecordStore : IThirdPartySystemTokenRecordStore
    {
        private static string _thirdPartySystemTokenRecordHashGroupName;
        /// <summary>
        /// 第三方系统令牌记录需要用到的一致性哈希组的名称
        /// 需要在系统初始化时赋值
        /// </summary>
        public static string ThirdPartySystemTokenRecordHashGroupName
        {
            set
            {
                _thirdPartySystemTokenRecordHashGroupName = value;
            }
        }


        private IHashGroupRepositoryCacheProxy _hashGroupRepository;
        private ISystemTokenConnectionFactory _systemTokenConnectionFactory;

        private IStoreInfoResolveService _storeInfoResolveService;

        private IAuthorizationEndpointStore _authorizationEndpointStore;
        private ISystemLoginEndpointStore _systemLoginEndpointStore;

        public ThirdPartySystemTokenRecordStore(IHashGroupRepositoryCacheProxy hashGroupRepository, ISystemTokenConnectionFactory systemTokenConnectionFactory, IStoreInfoResolveService storeInfoResolveService
            , ISystemLoginEndpointStore systemLoginEndpointStore, IAuthorizationEndpointStore authorizationEndpointStore
            )
        {
            _hashGroupRepository = hashGroupRepository;
            _systemTokenConnectionFactory = systemTokenConnectionFactory;
            _storeInfoResolveService = storeInfoResolveService;
            _systemLoginEndpointStore = systemLoginEndpointStore;
            _authorizationEndpointStore = authorizationEndpointStore;
        }


        public async Task Add(ThirdPartySystemTokenRecord record)
        {
            var storeInfo = await GetDBAllStoreInfo(record.UserKey,_thirdPartySystemTokenRecordHashGroupName);
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, storeInfo[0], async (conn, transaction) =>
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
                    if (record.ID == Guid.Empty)
                    {
                        command.CommandText = string.Format(@"
                                                INSERT INTO {0}
                                                     (
		                                               [id]
                                                      ,[userkey]
	                                                  ,[systemloginendpointid]
	                                                  ,[authorizationendpointid]
	                                                  ,[token]
	                                                  ,[timeout]
	                                                  ,[lastrefeshtime]
                                                      ,[createtime]
                                                      ,[modifytime]
                                                     )
                                                VALUES
                                                    (default
                                                    , @userkey
                                                    , @systemloginendpointid
                                                    , @authorizationendpointid
                                                    , @token
                                                    , @timeout
                                                    , @lastrefeshtime
                                                    , GETUTCDATE()
                                                    , GETUTCDATE());
                                                select @newid =[id] from {0} where [sequence] = SCOPE_IDENTITY()", storeInfo[1]);
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
                                                      ,[userkey]
	                                                  ,[systemloginendpointid]
	                                                  ,[authorizationendpointid]
	                                                  ,[token]
	                                                  ,[timeout]
	                                                  ,[lastrefeshtime]
                                                      ,[createtime]
                                                      ,[modifytime]
                                                     )
                                                VALUES
                                                    (
                                                      @id
                                                    , @userkey
                                                    , @systemloginendpointid
                                                    , @authorizationendpointid
                                                    , @token
                                                    , @timeout
                                                    , @lastrefeshtime
                                                    , GETUTCDATE()
                                                    , GETUTCDATE());", storeInfo[1]);

                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = record.ID
                        };
                        command.Parameters.Add(parameter);
                    }

                    parameter = new SqlParameter("@userkey", SqlDbType.NVarChar, 150)
                    {
                        Value = record.UserKey
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@systemloginendpointid", SqlDbType.UniqueIdentifier)
                    {
                        Value = record.SystemLoginEndpointID
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@authorizationendpointid", SqlDbType.UniqueIdentifier)
                    {
                        Value = record.AuthorizationEndpointID
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@token", SqlDbType.NVarChar, record.Token.Length)
                    {
                        Value = record.Token
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@timeout", SqlDbType.Int)
                    {
                        Value = record.Timeout
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@lastrefeshtime", SqlDbType.DateTime)
                    {
                        Value = record.LastRefeshTime
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (SqlException ex)
                    {
                        if (ex == null)
                        {
                            throw;
                        }
                        if (ex.Number == 2601)
                        {
                            var fragment = new TextFragment()
                            {
                                Code = TextCodes.ExistSameThirdPartySystemTokenRecord,
                                DefaultFormatting = "已经存在登录终结点{0}、验证终结点{1}、用户关键字{2}的第三方系统令牌记录",
                                ReplaceParameters = new List<object>() { record.SystemLoginEndpointID.ToString(), record.AuthorizationEndpointID.ToString(), record.UserKey }
                            };

                            throw new UtilityException((int)Errors.ExistSameThirdPartySystemTokenRecord, fragment);
                        }
                        else
                        {
                            throw;
                        }
                    }

                    //如果用户未赋值ID则创建成功后返回ID
                    if (record.ID == Guid.Empty)
                    {
                        record.ID = (Guid)command.Parameters["@newid"].Value;
                    };
                }

            });

        }

        public async Task Delete(string userKey, Guid id)
        {
            var storeInfo = await GetDBAllStoreInfo(userKey, _thirdPartySystemTokenRecordHashGroupName);
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, storeInfo[0], async (conn, transaction) =>
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

                    command.CommandText = string.Format(@"delete from {0} where [id]=@id", storeInfo[1]);

                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);


                    await command.PrepareAsync();
                    await command.ExecuteNonQueryAsync();


                }

            });

        }

        public async Task<ThirdPartySystemTokenRecord> QueryByID(string userKey, Guid id)
        {
            var storeInfo = await GetDBAllStoreInfo(userKey, _thirdPartySystemTokenRecordHashGroupName);
            ThirdPartySystemTokenRecord result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, storeInfo[0], async (conn, transaction) =>
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
                    Transaction = sqlTran,
                })
                {
                    SqlParameter parameter;

                    command.CommandText = string.Format(@"SELECT {0} FROM [dbo].[{1}] WHERE id=@id;", StoreHelper.GetThirdPartySystemTokenRecordStoreSelectFields(string.Empty), storeInfo[1]);
                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    SqlDataReader reader = null;


                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new ThirdPartySystemTokenRecord();
                            StoreHelper.SetThirdPartySystemTokenRecordSelectFields(result, reader, string.Empty);
                            result.SystemLoginEndpoint=await _systemLoginEndpointStore.QueryById(result.SystemLoginEndpointID);
                            result.AuthorizationEndpoint = await _authorizationEndpointStore.QueryById(result.AuthorizationEndpointID);

                            if (result.AuthorizationEndpoint==null || result.SystemLoginEndpoint==null)
                            {
                                result = null;
                            }
                        }
                        await reader.CloseAsync();
                    }
                }
            });

            return result;
        }

        public async Task<ThirdPartySystemTokenRecord> QueryByUserKey(string userKey, Guid loginEndpointId, Guid authEndpointId)
        {
            var storeInfo = await GetDBAllStoreInfo(userKey, _thirdPartySystemTokenRecordHashGroupName);
            ThirdPartySystemTokenRecord result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, storeInfo[0], async (conn, transaction) =>
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
                    Transaction = sqlTran,
                })
                {
                    SqlParameter parameter;

                    command.CommandText = string.Format(@"SELECT {0} FROM [dbo].[{1}] WHERE userkey=@userkey and systemloginendpointid=@systemloginendpointid and authorizationendpointid=@authorizationendpointid;", StoreHelper.GetThirdPartySystemTokenRecordStoreSelectFields(string.Empty), storeInfo[1]);
                    parameter = new SqlParameter("@userkey", SqlDbType.NVarChar,150)
                    {
                        Value = userKey
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@systemloginendpointid", SqlDbType.UniqueIdentifier)
                    {
                        Value = loginEndpointId
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@authorizationendpointid", SqlDbType.UniqueIdentifier)
                    {
                        Value = authEndpointId
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    SqlDataReader reader = null;


                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new ThirdPartySystemTokenRecord();
                            StoreHelper.SetThirdPartySystemTokenRecordSelectFields(result, reader, string.Empty);
                            result.SystemLoginEndpoint = await _systemLoginEndpointStore.QueryById(result.SystemLoginEndpointID);
                            result.AuthorizationEndpoint = await _authorizationEndpointStore.QueryById(result.AuthorizationEndpointID);

                            if (result.AuthorizationEndpoint == null || result.SystemLoginEndpoint == null)
                            {
                                result = null;
                            }
                        }
                        await reader.CloseAsync();
                    }
                }
            });

            return result;
        }

        public async Task<QueryResult<ThirdPartySystemTokenRecord>> QueryByUserKeyPage(string userKey, int page, int pageSize)
        {
            var storeInfo = await GetDBAllStoreInfo(userKey, _thirdPartySystemTokenRecordHashGroupName);
            QueryResult<ThirdPartySystemTokenRecord> result =new QueryResult<ThirdPartySystemTokenRecord>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, storeInfo[0], async (conn, transaction) =>
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
                    Transaction = sqlTran,
                })
                {
                    SqlParameter parameter;

                    command.CommandText = string.Format(@"SET @currentpage = @page;
                                                    SELECT @count = COUNT(*)
                                                    FROM [dbo].[{1}]
                                                    WHERE [userkey]=@userkey                                                         
                                                    

                                                    SELECT {0}
                                                    FROM [dbo].[{1}]
                                                    WHERE [userkey]=@userkey 
                                                    ORDER BY sequence OFFSET (@pagesize * (@currentpage - 1)) ROWS FETCH NEXT @pagesize ROWS ONLY;", StoreHelper.GetThirdPartySystemTokenRecordStoreSelectFields(string.Empty), storeInfo[1]);
                    parameter = new SqlParameter("@userkey", SqlDbType.NVarChar, 150)
                    {
                        Value = userKey
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@currentpage", SqlDbType.Int)
                    {
                        Value = page
                    };
                    command.Parameters.Add(parameter);


                    parameter = new SqlParameter("@pagesize", SqlDbType.Int)
                    {
                        Value = pageSize
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@count", SqlDbType.Int)
                    {
                         Direction= ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);


                    await command.PrepareAsync();

                    SqlDataReader reader = null;


                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var record = new ThirdPartySystemTokenRecord();        
                            StoreHelper.SetThirdPartySystemTokenRecordSelectFields(record, reader, string.Empty);
                            record.SystemLoginEndpoint = await _systemLoginEndpointStore.QueryById(record.SystemLoginEndpointID);
                            record.AuthorizationEndpoint = await _authorizationEndpointStore.QueryById(record.AuthorizationEndpointID);
                            result.Results.Add(record);     
                        }
                        await reader.CloseAsync();

                        result.TotalCount = (int)command.Parameters["@count"].Value;
                        result.CurrentPage = page;

                    }
                }
            });

            return result;
        }

        public async Task Update(ThirdPartySystemTokenRecord record)
        {
            var storeInfo = await GetDBAllStoreInfo(record.UserKey, _thirdPartySystemTokenRecordHashGroupName);
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, storeInfo[0], async (conn, transaction) =>
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
                    Transaction = sqlTran,
                })
                {
                    SqlParameter parameter;

                    command.CommandText = string.Format(@"
                                                update {0}
                                                set [token]=@token,
                                                    [timeout]=@timeout,
                                                    [lastrefeshtime]=@lastrefeshtime,
                                                    [modifytime]=getutcdate()
                                                where [id] = @id", storeInfo[1]);

                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = record.ID
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@token", SqlDbType.NVarChar, record.Token.Length)
                    {
                        Value = record.Token
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@timeout", SqlDbType.Int)
                    {
                        Value = record.Timeout
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@lastrefeshtime", SqlDbType.DateTime)
                    {
                        Value = record.LastRefeshTime
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    await command.ExecuteNonQueryAsync();
                }

            });
        }

        public async Task UpdateToken(string userKey, Guid id, string token)
        {
            var storeInfo = await GetDBAllStoreInfo(userKey, _thirdPartySystemTokenRecordHashGroupName);
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, storeInfo[0], async (conn, transaction) =>
            {
              
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }
                await  using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                })
                {
                    SqlParameter parameter;

                    command.CommandText = string.Format(@"
                                                update {0}
                                                set [token]=@token,
                                                    [lastrefeshtime]=getutcdate(),
                                                    [modifytime]=getutcdate()
                                                where [id] = @id", storeInfo[1]);

                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@token", SqlDbType.NVarChar, token.Length)
                    {
                        Value = token
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    await command.ExecuteNonQueryAsync();
                }

            });
        }


        private async Task<string[]> GetDBAllStoreInfo(string userKey, string hashGroupName)
        {
            string[] info = new string[2];

            //获取前缀的哈希节点关键字,
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo( _storeInfoResolveService,_hashGroupRepository, hashGroupName, userKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.ThirdPartySystemTokenRecord, out string tableName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { hashGroupName, HashEntityNames.UploadFile }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }

            //获取连接字符串
            info[0] = _systemTokenConnectionFactory.CreateAllForThirdPartySystemToken(dbInfo.DBConnectionNames);
            info[1] = tableName;

            return info;
        }

    }
}
