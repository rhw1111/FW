using MSLibrary.Cache.DAL;
using MSLibrary.DAL;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Transaction;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace MSLibrary.SystemToken.DAL
{

    /// <summary>
    /// 验证终结点数据操作
    /// </summary>
    [Injection(InterfaceType = typeof(IAuthorizationEndpointStore), Scope = InjectionScope.Singleton)]
    public class AuthorizationEndpointStore : IAuthorizationEndpointStore
    {

        private ISystemTokenConnectionFactory _dbConnectionFactory;

        public AuthorizationEndpointStore(ISystemTokenConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public async Task Add(AuthorizationEndpoint endpoint)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForSystemToken(), async (conn, transaction) =>
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
                if (endpoint.ID == Guid.Empty)
                {
                    command.CommandText = @"insert into [dbo].[AuthorizationEndpoint]
                                                  (
                                                       [id]
                                                      ,[name]
                                                      ,[thirdpartytype]
                                                      ,[thirdpartypostexecutetype]
                                                      ,[thirdpartyconfiguration]
                                                      ,[thirdpartypostconfiguration]
                                                      ,[keepthirdpartytoken]
                                                      ,[authmodes]
                                                      ,[createtime]
                                                      ,[modifytime]
	                                              )
	                                              values
                                                  (
	                                                  default
	                                                  ,@name
                                                      ,@thirdpartytype
                                                      ,@thirdpartypostexecutetype
                                                      ,@thirdpartyconfiguration
                                                      ,@thirdpartypostconfiguration
	                                                  ,@keepthirdpartytoken
                                                      ,@authmodes
	                                                  ,getutcdate()
	                                                  ,getutcdate()
	                                              )
                                                select @newid =[id] from [dbo].[AuthorizationEndpoint] where [sequence] = SCOPE_IDENTITY()";
                    parameter = new SqlParameter("@newid", SqlDbType.UniqueIdentifier)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);
                }
                else
                {
                    command.CommandText = @"insert into [dbo].[AuthorizationEndpoint]
                                                  (
                                                       [id]
                                                      ,[name]
                                                      ,[thirdpartytype]
                                                      ,[thirdpartypostexecutetype]
                                                      ,[thirdpartyconfiguration]
                                                      ,[thirdpartypostconfiguration]
                                                      ,[keepthirdpartytoken]
                                                      ,[authmodes]
                                                      ,[createtime]
                                                      ,[modifytime]
	                                              )
	                                              values
                                                  (
	                                                  @id
	                                                  ,@name
                                                      ,@thirdpartytype
                                                      ,@thirdpartypostexecutetype
                                                      ,@thirdpartyconfiguration
                                                      ,@thirdpartypostconfiguration
	                                                  ,@keepthirdpartytoken
                                                      ,@authmodes
	                                                  ,getutcdate()
	                                                  ,getutcdate()
	                                              )
                                             ";
                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = endpoint.ID
                    };
                    command.Parameters.Add(parameter);
                }
                parameter = new SqlParameter("@name", SqlDbType.VarChar, 150)
                {
                    Value = endpoint.Name
                };
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("@thirdpartytype", SqlDbType.VarChar, 150)
                {
                    Value = endpoint.ThirdPartyType
                };
                command.Parameters.Add(parameter);

                object thirdPartyPostExecuteType = DBNull.Value;
                if (endpoint.ThirdPartyPostExecuteType != null)
                {
                    thirdPartyPostExecuteType = endpoint.ThirdPartyPostExecuteType;
                }
                parameter = new SqlParameter("@thirdpartypostexecutetype", SqlDbType.VarChar, 150)
                {
                    Value = thirdPartyPostExecuteType
                };
                command.Parameters.Add(parameter);


                parameter = new SqlParameter("@thirdpartyconfiguration", SqlDbType.NVarChar, endpoint.ThirdPartyConfiguration.Length)
                {
                    Value = endpoint.ThirdPartyConfiguration
                };
                command.Parameters.Add(parameter);

                object thirdPartyPostConfiguration = DBNull.Value;
                int thirdPartyPostConfigurationLength = 100;
                if (endpoint.ThirdPartyPostConfiguration != null)
                {
                    thirdPartyPostConfiguration = endpoint.ThirdPartyPostConfiguration;
                    thirdPartyPostConfigurationLength = endpoint.ThirdPartyPostConfiguration.Length;
                }
                parameter = new SqlParameter("@thirdpartypostconfiguration", SqlDbType.NVarChar, thirdPartyPostConfigurationLength)
                {
                    Value = thirdPartyPostConfiguration
                };
                command.Parameters.Add(parameter);

                parameter = new SqlParameter("@keepthirdpartytoken", SqlDbType.Bit)
                {
                    Value = endpoint.KeepThirdPartyToken
                };
                command.Parameters.Add(parameter);



                    var strAuthmodes = await endpoint.AuthModes.ToDisplayString(async (item) => await Task.FromResult(item.ToString()), async () => await Task.FromResult(","));

                    parameter = new SqlParameter("@authmodes", SqlDbType.VarChar, strAuthmodes.Length)
                    {
                        Value = strAuthmodes
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
                            //违反主键约束
                            if (ex.Number == 2601)
                            {
                                var fragment = new TextFragment()
                                {
                                    Code = TextCodes.ExistAuthorizationEndpointByName,
                                    DefaultFormatting = "验证终结点数据中存在相同名称\"{0}\"数据",
                                    ReplaceParameters = new List<object>() { endpoint.Name }
                                };

                                throw new UtilityException((int)Errors.ExistAuthorizationEndpointByName, fragment);
                            }
                            else
                            {
                                throw;
                            }
                        }

                    //如果用户未赋值ID则创建成功后返回ID
                    if (endpoint.ID == Guid.Empty)
                    {
                        endpoint.ID = (Guid)command.Parameters["@newid"].Value;
                    };
                }

            });

        }

        /// <summary>
        /// 新增与系统登录终结点的关联关系
        /// </summary>
        /// <param name="authorizationId"></param>
        /// <param name="systemLoginEndpointId"></param>
        /// <returns></returns>
        public async Task AddSystemLoginEndpointRelation(Guid authorizationId, Guid systemLoginEndpointId)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForSystemToken(), async (conn, transaction) =>
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

                    command.CommandText = @"  insert into [dbo].[SystemLoginEndpointAuthorizationEndpointRelation]
                                                  (
	                                                   [systemloginendpointid]
                                                      ,[authorizationendpointid]
                                                      ,[createtime]
                                                  )
                                                  values
                                                  (
                                                          @systemloginendpointid
                                                        ,@authorizationendpointid
                                                        ,getutcdate()
                                                  )";
                    parameter = new SqlParameter("@systemloginendpointid", SqlDbType.UniqueIdentifier)
                    {
                        Value = systemLoginEndpointId
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@authorizationendpointid", SqlDbType.UniqueIdentifier)
                    {
                        Value = authorizationId
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    await command.ExecuteNonQueryAsync();

                }

            });

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(Guid id)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForSystemToken(), async (conn, transaction) =>
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

                    command.CommandText = @"delete [dbo].[AuthorizationEndpoint]  where id = @id";
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

        /// <summary>
        /// 删除与系统登陆终结点的关联关系
        /// </summary>
        /// <param name="authorizationId"></param>
        /// <param name="systemLoginEndpointId"></param>
        /// <returns></returns>
        public async Task DeleteSystemLoginEndpointRelation(Guid authorizationId, Guid systemLoginEndpointId)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForSystemToken(), async (conn, transaction) =>
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

                    command.CommandText = @"delete [dbo].[SystemLoginEndpointAuthorizationEndpointRelation]
                                            where [systemloginendpointid] = @systemloginendpointid
                                              and [authorizationendpointid] = @authorizationendpointid";
                    parameter = new SqlParameter("@systemloginendpointid", SqlDbType.UniqueIdentifier)
                    {
                        Value = systemLoginEndpointId
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@authorizationendpointid", SqlDbType.UniqueIdentifier)
                    {
                        Value = authorizationId
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    await command.ExecuteNonQueryAsync();

                }

            });

        }

        /// <summary>
        /// 通过ID进行查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AuthorizationEndpoint> QueryById(Guid id)
        {
            AuthorizationEndpoint result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateReadForSystemToken(), async (conn, transaction) =>
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

                    command.CommandText = string.Format(@"SELECT {0} FROM [dbo].[AuthorizationEndpoint] WHERE id=@id;", StoreHelper.GetAuthorizationEndpointStoreSelectFields(string.Empty));
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
                            result = new AuthorizationEndpoint();
                            StoreHelper.SetAuthorizationEndpointSelectFields(result, reader, string.Empty);
                        }
                        await reader.CloseAsync();
                    }
                }
            });

            return result;
        }

        /// <summary>
        /// 通过Name进行查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<AuthorizationEndpoint> QueryByName(string name)
        {
            AuthorizationEndpoint result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateReadForSystemToken(), async (conn, transaction) =>
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

                    command.CommandText = string.Format(@"SELECT {0} FROM [dbo].[AuthorizationEndpoint] WHERE name=@name;", StoreHelper.GetAuthorizationEndpointStoreSelectFields(string.Empty));
                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 500)
                    {
                        Value = name
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    SqlDataReader reader = null;

                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new AuthorizationEndpoint();
                            StoreHelper.SetAuthorizationEndpointSelectFields(result, reader, string.Empty);
                        }
                        await reader.CloseAsync();
                    }
                }
            });

            return result;
        }

        /// <summary>
        /// 通过Name进行分页查询
        /// </summary>
        /// <param name="authorizationName"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<QueryResult<AuthorizationEndpoint>> QueryByPage(string authorizationName, int page, int pageSize)
        {
            QueryResult<AuthorizationEndpoint> result = new QueryResult<AuthorizationEndpoint>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForSystemToken(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"SET @currentpage = @page;
                                                    SELECT @count = COUNT(*)
                                                    FROM [dbo].[AuthorizationEndpoint]
                                                    WHERE [name] LIKE @name                                                          
                                                    IF @pagesize * @page >= @count
                                                    BEGIN
                                                        SET @currentpage = @count / @pagesize;
                                                        IF @count % @pagesize <> 0
                                                        BEGIN
                                                            SET @currentpage = @currentpage + 1;
                                                        END;
                                                        IF @currentpage = 0
                                                            SET @currentpage = 1;
                                                    END;
                                                    ELSE IF @page < 1
                                                    BEGIN
                                                        SET @currentpage = 1;
                                                    END;

                                                    SELECT {0}
                                                    FROM [dbo].[AuthorizationEndpoint]
                                                    WHERE [name] LIKE @name
                                                    ORDER BY sequence OFFSET (@pagesize * (@currentpage - 1)) ROWS FETCH NEXT @pagesize ROWS ONLY;", StoreHelper.GetAuthorizationEndpointStoreSelectFields(string.Empty)),
                    Transaction = sqlTran

                })
                {
                    var parameter = new SqlParameter("@page", SqlDbType.Int)
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
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@currentpage", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 500)
                    {
                        Value = $"{authorizationName.ToSqlLike()}%"

                    };
                    command.Parameters.Add(parameter);
                    await command.PrepareAsync();
                    SqlDataReader reader = null;


                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var authorizationEndpoint = new AuthorizationEndpoint();
                            StoreHelper.SetAuthorizationEndpointSelectFields(authorizationEndpoint, reader, string.Empty);
                            result.Results.Add(authorizationEndpoint);
                        }
                        await reader.CloseAsync();
                        result.TotalCount = (int)command.Parameters["@count"].Value;
                        result.CurrentPage = (int)command.Parameters["@currentpage"].Value;
                    }
                };
            });

            return result;
        }

        /// <summary>
        /// 根据关联的系统登录终结点查询指定Id的关联验证终结点
        /// </summary>
        /// <param name="systemLoginEndpointId"></param>
        /// <param name="authorizationId"></param>
        /// <returns></returns>
        public async Task<AuthorizationEndpoint> QueryBySystemLoginEndpointRelation(Guid systemLoginEndpointId, Guid authorizationId)
        {
            AuthorizationEndpoint result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateReadForSystemToken(), async (conn, transaction) =>
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

                    command.CommandText = string.Format(@"  SELECT 
	                                                             {0}
                                                            FROM [DBO].[AUTHORIZATIONENDPOINT] A JOIN DBO.SYSTEMLOGINENDPOINTAUTHORIZATIONENDPOINTRELATION R
                                                            ON(A.ID = R.AUTHORIZATIONENDPOINTID AND R.SYSTEMLOGINENDPOINTID = @systemLoginEndpointId
	                                                            AND R.AUTHORIZATIONENDPOINTID = @authorizationId
                                                            );", StoreHelper.GetAuthorizationEndpointStoreSelectFields("A"));

                    parameter = new SqlParameter("@systemLoginEndpointId", SqlDbType.UniqueIdentifier)
                    {
                        Value = systemLoginEndpointId
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@authorizationId", SqlDbType.UniqueIdentifier)
                    {
                        Value = authorizationId
                    };
                    command.Parameters.Add(parameter);

                    command.Prepare();

                    SqlDataReader reader = null;


                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new AuthorizationEndpoint();
                            StoreHelper.SetAuthorizationEndpointSelectFields(result, reader, "A");
                        }
                        await reader.CloseAsync();
                    }
                }
            });

            return result;

        }

        /// <summary>
        /// 根据关联的系统登录终结点查询指定名称的关联验证终结点
        /// </summary>
        /// <param name="systemLoginEndpointId"></param>
        /// <param name="authorizationName"></param>
        /// <returns></returns>
        public async Task<AuthorizationEndpoint> QueryBySystemLoginEndpointRelation(Guid systemLoginEndpointId, string authorizationName)
        {
            AuthorizationEndpoint result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateReadForSystemToken(), async (conn, transaction) =>
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

                    command.CommandText = string.Format(@"  SELECT 
	                                                             {0}
                                                            FROM [DBO].[AUTHORIZATIONENDPOINT] A JOIN DBO.SYSTEMLOGINENDPOINTAUTHORIZATIONENDPOINTRELATION R
                                                            ON(A.ID = R.AUTHORIZATIONENDPOINTID AND R.SYSTEMLOGINENDPOINTID = @systemLoginEndpointId
	                                                            AND A.Name = @name
                                                            );", StoreHelper.GetAuthorizationEndpointStoreSelectFields("A"));

                    parameter = new SqlParameter("@systemLoginEndpointId", SqlDbType.UniqueIdentifier)
                    {
                        Value = systemLoginEndpointId
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 500)
                    {
                        Value = authorizationName
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    SqlDataReader reader = null;


                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new AuthorizationEndpoint();
                            StoreHelper.SetAuthorizationEndpointSelectFields(result, reader, "A");
                        }
                        await reader.CloseAsync();
                    }
                }
            });

            return result;

        }

        /// <summary>
        /// 根据关联的系统登录终结点分页查询匹配名称的关联验证终结点
        /// </summary>
        /// <param name="systemLoginEndpointId"></param>
        /// <param name="authorizationName"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<QueryResult<AuthorizationEndpoint>> QueryBySystemLoginEndpointRelationPage(Guid systemLoginEndpointId, string authorizationName, int page, int pageSize)
        {
            QueryResult<AuthorizationEndpoint> result = new QueryResult<AuthorizationEndpoint>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForSystemToken(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"SET @currentpage = @page;
                                                    SELECT @count = COUNT(*)
                                                    FROM [DBO].[AUTHORIZATIONENDPOINT] A JOIN DBO.SYSTEMLOGINENDPOINTAUTHORIZATIONENDPOINTRELATION R
                                                            ON(A.ID = R.AUTHORIZATIONENDPOINTID AND R.SYSTEMLOGINENDPOINTID = @systemLoginEndpointId
	                                                            AND A.Name like @name)

                                                    IF @pagesize * @page >= @count
                                                    BEGIN
                                                        SET @currentpage = @count / @pagesize;
                                                        IF @count % @pagesize <> 0
                                                        BEGIN
                                                            SET @currentpage = @currentpage + 1;
                                                        END;
                                                        IF @currentpage = 0
                                                            SET @currentpage = 1;
                                                    END;
                                                    ELSE IF @page < 1
                                                    BEGIN
                                                        SET @currentpage = 1;
                                                    END;

                                                    SELECT 
	                                                             {0}
                                                            FROM [DBO].[AUTHORIZATIONENDPOINT] A JOIN DBO.SYSTEMLOGINENDPOINTAUTHORIZATIONENDPOINTRELATION R
                                                            ON(A.ID = R.AUTHORIZATIONENDPOINTID AND R.SYSTEMLOGINENDPOINTID = @systemLoginEndpointId
	                                                            AND A.Name like @name)
                                                    ORDER BY [Asequence] OFFSET (@pagesize * (@currentpage - 1)) ROWS FETCH NEXT @pagesize ROWS ONLY;", StoreHelper.GetAuthorizationEndpointStoreSelectFields("A")),
                    Transaction = sqlTran

                })
                {
                    var parameter = new SqlParameter("@page", SqlDbType.Int)
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
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@currentpage", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@systemLoginEndpointId", SqlDbType.UniqueIdentifier)
                    {
                        Value = systemLoginEndpointId
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 500)
                    {
                        Value = $"{authorizationName.ToSqlLike()}%"
                    };
                    command.Parameters.Add(parameter);
                    await command.PrepareAsync();
                    SqlDataReader reader = null;

                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var authorizationEndpoint = new AuthorizationEndpoint();
                            StoreHelper.SetAuthorizationEndpointSelectFields(authorizationEndpoint, reader, "A");
                            result.Results.Add(authorizationEndpoint);
                        }
                        await reader.CloseAsync();
                        result.TotalCount = (int)command.Parameters["@count"].Value;
                        result.CurrentPage = (int)command.Parameters["@currentpage"].Value;
                    }
                };
            });

            return result;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public async Task Update(AuthorizationEndpoint endpoint)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForSystemToken(), async (conn, transaction) =>
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

                    command.CommandText = @"update [dbo].[AuthorizationEndpoint]
                                                   set name = @name
                                                      ,thirdpartytype=@thirdpartytype
                                                      ,thirdpartypostexecutetype=@thirdpartypostexecutetype
                                                      ,thirdpartyconfiguration=@thirdpartyconfiguration
                                                      ,thirdpartypostconfiguration=@thirdpartypostconfiguration
                                                      ,keepthirdpartytoken=@keepthirdpartytoken
                                                      ,authmodes=@authmodes
                                                      ,modifytime = getutcdate()
                                                   where id = @id";
                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = endpoint.ID
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@name", SqlDbType.VarChar, 150)
                    {
                        Value = endpoint.Name
                    };
                    command.Parameters.Add(parameter);


                    parameter = new SqlParameter("@thirdpartytype", SqlDbType.VarChar, 150)
                    {
                        Value = endpoint.ThirdPartyType
                    };
                    command.Parameters.Add(parameter);

                    object thirdPartyPostExecuteType = DBNull.Value;
                    if (endpoint.ThirdPartyPostExecuteType != null)
                    {
                        thirdPartyPostExecuteType = endpoint.ThirdPartyPostExecuteType;
                    }
                    parameter = new SqlParameter("@thirdpartypostexecutetype", SqlDbType.VarChar, 150)
                    {
                        Value = thirdPartyPostExecuteType
                    };
                    command.Parameters.Add(parameter);


                    parameter = new SqlParameter("@thirdpartyconfiguration", SqlDbType.NVarChar, endpoint.ThirdPartyConfiguration.Length)
                    {
                        Value = endpoint.ThirdPartyConfiguration
                    };
                    command.Parameters.Add(parameter);

                    object thirdPartyPostConfiguration = DBNull.Value;
                    int thirdPartyPostConfigurationLength = 100;
                    if (endpoint.ThirdPartyPostConfiguration != null)
                    {
                        thirdPartyPostConfiguration = endpoint.ThirdPartyPostConfiguration;
                        thirdPartyPostConfigurationLength = endpoint.ThirdPartyPostConfiguration.Length;
                    }
                    parameter = new SqlParameter("@thirdpartypostconfiguration", SqlDbType.NVarChar, thirdPartyPostConfigurationLength)
                    {
                        Value = thirdPartyPostConfiguration
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@keepthirdpartytoken", SqlDbType.Bit)
                    {
                        Value = endpoint.KeepThirdPartyToken
                    };
                    command.Parameters.Add(parameter);

                    var strAuthmodes = await endpoint.AuthModes.ToDisplayString(async (item) => await Task.FromResult(item.ToString()), async () => await Task.FromResult(","));

                    parameter = new SqlParameter("@authmodes", SqlDbType.VarChar, strAuthmodes.Length)
                    {
                        Value = strAuthmodes
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    await command.ExecuteNonQueryAsync();

                }

            });

        }
    }
}
