using Microsoft.EntityFrameworkCore;
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

namespace MSLibrary.SystemToken.DAL
{
    /// <summary>
    /// 系统登录终结点数据操作
    /// </summary>
    [Injection(InterfaceType = typeof(ISystemLoginEndpointStore), Scope = InjectionScope.Singleton)]
    public class SystemLoginEndpointStore : ISystemLoginEndpointStore
    {
        private ISystemTokenConnectionFactory _dbConnectionFactory;

        public SystemLoginEndpointStore(ISystemTokenConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public async Task Add(SystemLoginEndpoint endpoint)
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
                    Transaction = sqlTran
                })
                {
                    SqlParameter parameter;
                    if (endpoint.ID == Guid.Empty)
                    {
                        command.CommandText = @"INSERT INTO [dbo].[SystemLoginEndpoint]
                                                (
                                                    [id],
                                                    [name],
                                                    [secretkey],
                                                    [expiresecond],
                                                    [clientredirectbaseurls],
                                                    [baseurl],
                                                    [userinfokey],
                                                    [createtime],
                                                    [modifytime]
                                                )
                                                VALUES
                                                (DEFAULT, @name, @secretkey, @expiresecond, @clientredirectbaseurls, @baseurl, @userinfokey, GETUTCDATE(), GETUTCDATE());
                                                SELECT @newid = id FROM [dbo].[SystemLoginEndpoint] WHERE [sequence] = SCOPE_IDENTITY();";

                        parameter = new SqlParameter("@newid", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        command.CommandText = @"INSERT INTO [dbo].[SystemLoginEndpoint]
                                                (
                                                    [id],
                                                    [name],
                                                    [secretkey],
                                                    [expiresecond],
                                                    [clientredirectbaseurls],
                                                    [baseurl],
                                                    [userinfokey],
                                                    [createtime],
                                                    [modifytime]
                                                )
                                                VALUES
                                                (@id, @name, @secretkey, @expiresecond, @clientredirectbaseurls, @baseurl, @userinfokey, GETUTCDATE(), GETUTCDATE());";

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

                    parameter = new SqlParameter("@secretkey", SqlDbType.VarChar, 200)
                    {
                        Value = endpoint.SecretKey
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@expiresecond", SqlDbType.Int)
                    {
                        Value = endpoint.ExpireSecond
                    };
                    command.Parameters.Add(parameter);

                    if (endpoint.ClientRedirectBaseUrls != null)
                    {
                        StringBuilder sUrl = new StringBuilder();
                        for (int i = 0; i < endpoint.ClientRedirectBaseUrls.Length; i++)
                        {
                            sUrl.Append(endpoint.ClientRedirectBaseUrls[i] + ",");
                        };
                        parameter = new SqlParameter("@clientredirectbaseurls", SqlDbType.VarChar, sUrl.Length)
                        {
                            Value = sUrl.ToString().TrimEnd(',')
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new SqlParameter("@clientredirectbaseurls", SqlDbType.VarChar, 1000)
                        {
                            Value = DBNull.Value
                        };
                        command.Parameters.Add(parameter);
                    }


                    parameter = new SqlParameter("@baseurl", SqlDbType.VarChar, 500)
                    {
                        Value = endpoint.BaseUrl
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@userinfokey", SqlDbType.VarChar, 150)
                    {
                        Value = endpoint.UserInfoKey
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
                                    Code = TextCodes.ExistSystemLoginEndpointByName,
                                    DefaultFormatting = "系统登录终结点中已存在相同名称\"{0}\"数据",
                                    ReplaceParameters = new List<object>() { endpoint.Name }
                                };

                                throw new UtilityException((int)Errors.ExistSystemLoginEndpointByName,fragment);
                            }
                            else
                            {
                                throw;
                            }
                        }

          
                    if (endpoint.ID == Guid.Empty)
                    {
                        endpoint.ID = (Guid)command.Parameters["@newid"].Value;
                    };
                }
            });

        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public async Task Update(SystemLoginEndpoint endpoint)
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
                    CommandText = @"UPDATE [dbo].[SystemLoginEndpoint]
                                       SET 
                                           [name] = @name
                                          ,[secretkey] = @secretkey
                                          ,[expiresecond] = @expiresecond
                                          ,[clientredirectbaseurls] = @clientredirectbaseurls
                                          ,[baseurl] = @baseurl
                                          ,[userinfokey] = @userinfokey
                                          ,[modifytime] = GETUTCDATE()
                                     WHERE id=@id;"
                })
                {
                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = endpoint.ID
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@name", SqlDbType.VarChar, 150)
                    {
                        Value = endpoint.Name
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@secretkey", SqlDbType.VarChar, 200)
                    {
                        Value = endpoint.SecretKey
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@expiresecond", SqlDbType.Int)
                    {
                        Value = endpoint.ExpireSecond
                    };
                    command.Parameters.Add(parameter);

                    if (endpoint.ClientRedirectBaseUrls != null)
                    {
                        StringBuilder sUrl = new StringBuilder();
                        for (int i = 0; i < endpoint.ClientRedirectBaseUrls.Length; i++)
                        {
                            sUrl.Append(endpoint.ClientRedirectBaseUrls[i] + ",");
                        };
                        parameter = new SqlParameter("@clientredirectbaseurls", SqlDbType.VarChar, sUrl.Length)
                        {
                            Value = sUrl.ToString().TrimEnd(',')
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new SqlParameter("@clientredirectbaseurls", SqlDbType.VarChar, 1000)
                        {
                            Value = DBNull.Value
                        };
                        command.Parameters.Add(parameter);
                    }


                    parameter = new SqlParameter("@baseurl", SqlDbType.VarChar, 500)
                    {
                        Value = endpoint.BaseUrl
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@userinfokey", SqlDbType.VarChar, 150)
                    {
                        Value = endpoint.UserInfoKey
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
                                    Code = TextCodes.ExistSystemLoginEndpointByName,
                                    DefaultFormatting = "系统登录终结点中已存在相同名称\"{0}\"数据",
                                    ReplaceParameters = new List<object>() { endpoint.Name }
                                };

                                throw new UtilityException((int)Errors.ExistSystemLoginEndpointByName,fragment);
                            }
                            else
                            {
                                throw;
                            }
                        }
 
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
                    CommandText = @"DELETE FROM [dbo].[SystemLoginEndpoint] WHERE id=@id;"
                })
                {
                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
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
        /// 根据Id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SystemLoginEndpoint> QueryById(Guid id)
        {
            SystemLoginEndpoint result = null;
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
                    CommandText = string.Format(@"SELECT {0} FROM [dbo].[SystemLoginEndpoint] WHERE id=@id;", StoreHelper.GetSystemLoginEndpointStoreSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {
                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
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
                            result = new SystemLoginEndpoint();
                            StoreHelper.SetSystemLoginEndpointStoreSelectFields(result, reader, string.Empty);
                        }
                        await reader.CloseAsync();
                    }
                }
            });
            return result;
        }

        /// <summary>
        /// 根据名称查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<SystemLoginEndpoint> QueryByName(string name)
        {
            SystemLoginEndpoint result = null;
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
                    CommandText = string.Format(@"SELECT {0} FROM [dbo].[SystemLoginEndpoint] WHERE [name]=@name;", StoreHelper.GetSystemLoginEndpointStoreSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {
                    var parameter = new SqlParameter("@name", SqlDbType.NVarChar, 500)
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
                            result = new SystemLoginEndpoint();
                            StoreHelper.SetSystemLoginEndpointStoreSelectFields(result, reader, string.Empty);
                        }
                        await reader.CloseAsync();
                    }
                }
            });
            return result;
        }

        /// <summary>
        /// 根据名称匹配分页查询
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<QueryResult<SystemLoginEndpoint>> QueryByPage(string name, int page, int pageSize)
        {
            QueryResult<SystemLoginEndpoint> result = new QueryResult<SystemLoginEndpoint>();
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
                                                    FROM [SystemLoginEndpoint]
                                                    WHERE [name] LIKE @name;
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
                                                    FROM [SystemLoginEndpoint]
                                                    WHERE [name] LIKE @name
                                                    ORDER BY sequence OFFSET (@pagesize * (@currentpage - 1)) ROWS FETCH NEXT @pagesize ROWS ONLY;",
                                                    StoreHelper.GetSystemLoginEndpointStoreSelectFields(string.Empty)),
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
                        Value = $"{name.ToSqlLike()}%"

                    };
                    command.Parameters.Add(parameter);
                    await command.PrepareAsync();
                    SqlDataReader reader = null;

                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var systemLoginEndpoint = new SystemLoginEndpoint();
                            StoreHelper.SetSystemLoginEndpointStoreSelectFields(systemLoginEndpoint, reader, string.Empty);
                            result.Results.Add(systemLoginEndpoint);
                        }
                        await reader.CloseAsync();
                        result.TotalCount = (int)command.Parameters["@count"].Value;
                        result.CurrentPage = (int)command.Parameters["@currentpage"].Value;
                    }
                }
            });
            return result;
        }


    }
}
