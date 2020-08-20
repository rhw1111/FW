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
using MSLibrary.MessageQueue;
using MSLibrary.MessageQueue.DAL;

namespace MSLibrary.MySqlStore.MessageQueue.DAL
{
    [Injection(InterfaceType = typeof(IClientSMessageTypeListenerEndpointStore), Scope = InjectionScope.Singleton)]
    public class ClientSMessageTypeListenerEndpointStore : IClientSMessageTypeListenerEndpointStore
    {
        private IMessageQueueConnectionFactory _messageQueueConnectionFactory;

        public ClientSMessageTypeListenerEndpointStore(IMessageQueueConnectionFactory messageQueueConnectionFactory)
        {
            _messageQueueConnectionFactory = messageQueueConnectionFactory;
        }


        public async Task Add(ClientSMessageTypeListenerEndpoint endpoint)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _messageQueueConnectionFactory.CreateAllForMessageQueueMain(), async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (sqlTran != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }
                await using (MySqlCommand command = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran
                })
                {
                    MySqlParameter parameter;
                    if (endpoint.ID == Guid.Empty)
                    {
                        endpoint.ID = Guid.NewGuid();
                    }

                        command.CommandText = @"INSERT INTO clientsmessagetypelistenerendpoint
                                                       (id
                                                       ,name
                                                       ,signaturekey
                                                       ,createtime
                                                       ,modifytime)
                                                 VALUES
                                                       (@id
                                                       ,@name
                                                       ,@signaturekey
                                                       ,utc_timestamp()
                                                       ,utc_timestamp());";
                        parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                        {
                            Value = endpoint.ID
                        };
                        command.Parameters.Add(parameter);
                    
                    parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 500)
                    {
                        Value = endpoint.Name
                    };
                    command.Parameters.Add(parameter);
                    parameter = new MySqlParameter("@signaturekey", MySqlDbType.VarChar, 150)
                    {
                        Value = endpoint.SignatureKey
                    };
                    command.Parameters.Add(parameter);
                    await command.PrepareAsync();

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (MySqlException ex)
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

        public async Task Delete(Guid id)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _messageQueueConnectionFactory.CreateAllForMessageQueueMain(), async (conn, transaction) =>
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
                    Transaction = sqlTran,
                    CommandText = @"DELETE FROM clientsmessagetypelistenerendpoint WHERE id=@id;"
                })
                {
                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    await command.ExecuteNonQueryAsync();

                }

            });

        }

        public async Task<ClientSMessageTypeListenerEndpoint> QueryById(Guid id)
        {
            ClientSMessageTypeListenerEndpoint result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _messageQueueConnectionFactory.CreateReadForMessageQueueMain(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"SELECT {0} FROM clientsmessagetypelistenerendpoint where id=@id;", StoreHelper.GetClientSMessageTypeListenerEndpointSelectFields(string.Empty)),
                    Transaction = sqlTran
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
                            result = new ClientSMessageTypeListenerEndpoint();
                            StoreHelper.SetClientSMessageTypeListenerEndpointSelectFields(result, reader, string.Empty);
                        }
                        await reader.CloseAsync();
                    }
                }
            });
            return result;
        }

        public async Task<ClientSMessageTypeListenerEndpoint> QueryByName(string name)
        {
            ClientSMessageTypeListenerEndpoint result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _messageQueueConnectionFactory.CreateReadForMessageQueueMain(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"SELECT {0} FROM clientsmessagetypelistenerendpoint where name=@name;", StoreHelper.GetClientSMessageTypeListenerEndpointSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {
                    var parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 500)
                    {
                        Value = name
                    };
                    command.Parameters.Add(parameter);
                    await command.PrepareAsync();
                    DbDataReader reader = null;


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

        public async Task<QueryResult<ClientSMessageTypeListenerEndpoint>> QueryByPage(string name, int page, int pageSize)
        {
            QueryResult<ClientSMessageTypeListenerEndpoint> result = new QueryResult<ClientSMessageTypeListenerEndpoint>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _messageQueueConnectionFactory.CreateReadForMessageQueueMain(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"  SELECT  COUNT(*)
                                                    FROM clientsmessagetypelistenerendpoint
                                                    WHERE name LIKE @name;

                                                select {0}
                                                from clientsmessagetypelistenerendpoint
                                                where sequence in
                                                (
                                                select t.sequence from
                                                (
                                                    select sequence
                                                    from clientsmessagetypelistenerendpoint
                                                    where name like @name
                                                    order by sequence limit {1}, {2}                                                   
                                                ) as t
                                                )",
                                                    StoreHelper.GetClientSMessageTypeListenerEndpointSelectFields(string.Empty), (page - 1) * pageSize, pageSize),
                    Transaction = sqlTran
                })
                {


                   var parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 500)
                    {
                        Value = $"{name.ToMySqlLike()}%"

                    };
                    command.Parameters.Add(parameter);
                    await command.PrepareAsync();
                    DbDataReader reader = null;

                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result.TotalCount = reader.GetInt32(0);
                            result.CurrentPage = page;

                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    var endpoint = new ClientSMessageTypeListenerEndpoint();
                                    StoreHelper.SetClientSMessageTypeListenerEndpointSelectFields(endpoint, reader, string.Empty);
                                    result.Results.Add(endpoint);
                                }
                            }
                        }
                        await reader.CloseAsync();
                    }
                }
            });
            return result;
        }

        public async Task Update(ClientSMessageTypeListenerEndpoint endpoint)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _messageQueueConnectionFactory.CreateAllForMessageQueueMain(), async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (sqlTran != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }
                await using (MySqlCommand command = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran
                })
                {
                    command.CommandText = @"UPDATE clientsmessagetypelistenerendpoint
                                               SET 
                                                  name =@name
                                                  ,signaturekey = @signaturekey
                                                  ,modifytime = utc_timestamp()
                                             WHERE id=@id;";
                    MySqlParameter parameter;
                    parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = endpoint.ID
                    };
                    command.Parameters.Add(parameter);
                    parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 500)
                    {
                        Value = endpoint.Name
                    };
                    command.Parameters.Add(parameter);
                    parameter = new MySqlParameter("@signaturekey", MySqlDbType.VarChar, 150)
                    {
                        Value = endpoint.SignatureKey
                    };
                    command.Parameters.Add(parameter);
                    await command.PrepareAsync();

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (MySqlException ex)
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
    }
}
