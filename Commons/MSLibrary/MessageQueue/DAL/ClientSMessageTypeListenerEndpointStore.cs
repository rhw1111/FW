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

namespace MSLibrary.MessageQueue.DAL
{
    /// <summary>
    /// 客户端消息类型监听终结点数据操作
    /// </summary>
    [Injection(InterfaceType = typeof(IClientSMessageTypeListenerEndpointStore), Scope = InjectionScope.Singleton)]
    public class ClientSMessageTypeListenerEndpointStore : IClientSMessageTypeListenerEndpointStore
    {
        private IMessageQueueConnectionFactory _messageQueueConnectionFactory;

        public ClientSMessageTypeListenerEndpointStore(IMessageQueueConnectionFactory messageQueueConnectionFactory)
        {
            _messageQueueConnectionFactory = messageQueueConnectionFactory;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public async Task Add(ClientSMessageTypeListenerEndpoint endpoint)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _messageQueueConnectionFactory.CreateAllForMessageQueueMain(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (sqlTran != null)
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
                        command.CommandText = @"INSERT INTO [dbo].[ClientSMessageTypeListenerEndpoint]
                                                       ([id]
                                                       ,[name]
                                                       ,[signaturekey]
                                                       ,[createtime]
                                                       ,[modifytime])
                                                 VALUES
                                                       (DEFAULT
                                                       ,@name
                                                       ,@signaturekey
                                                       ,GETUTCDATE()
                                                       ,GETUTCDATE());
                                                SELECT @newid = id FROM [dbo].[ClientSMessageTypeListenerEndpoint] WHERE [sequence] = SCOPE_IDENTITY();";

                        parameter = new SqlParameter("@newid", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        command.CommandText = @"INSERT INTO [dbo].[ClientSMessageTypeListenerEndpoint]
                                                       ([id]
                                                       ,[name]
                                                       ,[signaturekey]
                                                       ,[createtime]
                                                       ,[modifytime])
                                                 VALUES
                                                       (@id
                                                       ,@name
                                                       ,@signaturekey
                                                       ,GETUTCDATE()
                                                       ,GETUTCDATE());";
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = endpoint.ID
                        };
                        command.Parameters.Add(parameter);
                    }
                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 500)
                    {
                        Value = endpoint.Name
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@signaturekey", SqlDbType.NVarChar, 150)
                    {
                        Value = endpoint.SignatureKey
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
                                    Code = TextCodes.ExistClientSMessageTypeListenerEndpointByName,
                                    DefaultFormatting = "客户端消息类型监听终结点中存在相同的名称\"{0}\"数据",
                                    ReplaceParameters = new List<object>() { endpoint.Name }
                                };

                                throw new UtilityException((int)Errors.ExistClientSMessageTypeListenerEndpointByName, fragment);
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
        public async Task Update(ClientSMessageTypeListenerEndpoint endpoint)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _messageQueueConnectionFactory.CreateAllForMessageQueueMain(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (sqlTran != null)
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
                    command.CommandText = @"UPDATE [dbo].[ClientSMessageTypeListenerEndpoint]
                                               SET 
                                                  [name] =@name
                                                  ,[signaturekey] = @signaturekey
                                                  ,[modifytime] = GETUTCDATE()
                                             WHERE id=@id;";
                    SqlParameter parameter;
                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = endpoint.ID
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 500)
                    {
                        Value = endpoint.Name
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@signaturekey", SqlDbType.NVarChar, 150)
                    {
                        Value = endpoint.SignatureKey
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
                                    Code = TextCodes.ExistClientSMessageTypeListenerEndpointByName,
                                    DefaultFormatting = "客户端消息类型监听终结点中存在相同的名称\"{0}\"数据",
                                    ReplaceParameters = new List<object>() { endpoint.Name }
                                };

                                throw new UtilityException((int)Errors.ExistClientSMessageTypeListenerEndpointByName, fragment);
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
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _messageQueueConnectionFactory.CreateAllForMessageQueueMain(), async (conn, transaction) =>
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
                    CommandText = @"DELETE FROM [dbo].[ClientSMessageTypeListenerEndpoint] WHERE id=@id;"
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
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ClientSMessageTypeListenerEndpoint> QueryById(Guid id)
        {
            ClientSMessageTypeListenerEndpoint result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _messageQueueConnectionFactory.CreateReadForMessageQueueMain(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"SELECT {0} FROM [dbo].[ClientSMessageTypeListenerEndpoint] WHERE id=@id;", StoreHelper.GetClientSMessageTypeListenerEndpointSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {
                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);
                    command.Prepare();
                    SqlDataReader reader = null;

                    await using (reader= await command.ExecuteReaderAsync())
                    {

                        if (await reader.ReadAsync())
                        {
                            result = new ClientSMessageTypeListenerEndpoint();
                            StoreHelper.SetClientSMessageTypeListenerEndpointSelectFields(result, reader, string.Empty);
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
        public async Task<ClientSMessageTypeListenerEndpoint> QueryByName(string name)
        {
            ClientSMessageTypeListenerEndpoint result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _messageQueueConnectionFactory.CreateReadForMessageQueueMain(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"SELECT {0} FROM [dbo].[ClientSMessageTypeListenerEndpoint] WHERE name=@name;", StoreHelper.GetClientSMessageTypeListenerEndpointSelectFields(string.Empty)),
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
                            result = new ClientSMessageTypeListenerEndpoint();
                            StoreHelper.SetClientSMessageTypeListenerEndpointSelectFields(result, reader, string.Empty);
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
        public async Task<QueryResult<ClientSMessageTypeListenerEndpoint>> QueryByPage(string name, int page, int pageSize)
        {
            QueryResult<ClientSMessageTypeListenerEndpoint> result = new QueryResult<ClientSMessageTypeListenerEndpoint>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _messageQueueConnectionFactory.CreateReadForMessageQueueMain(), async (conn, transaction) =>
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
                                                    FROM [ClientSMessageTypeListenerEndpoint]
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
                                                    FROM [ClientSMessageTypeListenerEndpoint]
                                                    WHERE [name] LIKE @name
                                                    ORDER BY sequence OFFSET (@pagesize * (@currentpage - 1)) ROWS FETCH NEXT @pagesize ROWS ONLY;",
                                                    StoreHelper.GetClientSMessageTypeListenerEndpointSelectFields(string.Empty)),
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
                            var endpoint = new ClientSMessageTypeListenerEndpoint();
                            StoreHelper.SetClientSMessageTypeListenerEndpointSelectFields(endpoint, reader, string.Empty);
                            result.Results.Add(endpoint);
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
