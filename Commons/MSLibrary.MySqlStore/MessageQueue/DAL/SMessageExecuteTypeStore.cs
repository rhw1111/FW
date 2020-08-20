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
    [Injection(InterfaceType = typeof(ISMessageExecuteTypeStore), Scope = InjectionScope.Singleton)]
    public class SMessageExecuteTypeStore : ISMessageExecuteTypeStore
    {
        private IMessageQueueConnectionFactory _messageQueueConnectionFactory;

        public SMessageExecuteTypeStore(IMessageQueueConnectionFactory messageQueueConnectionFactory)
        {
            _messageQueueConnectionFactory = messageQueueConnectionFactory;
        }

        public async Task Add(SMessageExecuteType messageType)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _messageQueueConnectionFactory.CreateAllForMessageQueueMain(), async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }

                await using (MySqlCommand commond = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran
                })
                {

                    if (messageType.ID == Guid.Empty)
                    {
                        messageType.ID = Guid.NewGuid();
                    }
                
                        commond.CommandText = @"insert into smessageexecutetype(id,name,createtime,modifytime)
                                    values(@id,@name,utc_timestamp() ,utc_timestamp())";



                    MySqlParameter parameter;

                    parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = messageType.ID
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 150)
                    {
                        Value = messageType.Name
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();
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

                await using (MySqlCommand commond = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = @"delete from smessageexecutetype where id = @id"
                })
                {

                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();
                }
            });
        }

        public async Task<QueryResult<SMessageExecuteType>> Query(string name, int page, int pageSize)
        {
            QueryResult<SMessageExecuteType> result = new QueryResult<SMessageExecuteType>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _messageQueueConnectionFactory.CreateReadForMessageQueueMain(), async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }

                await using (MySqlCommand commond = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = string.Format(@"
		                           select count(*) from smessageexecutetype where name like @name
		                           
	
                                                select {0}
                                                from smessageexecutetype
                                                where sequence in
                                                (
                                                select t.sequence from
                                                (
                                                    select sequence
                                                    from smessageexecutetype
                                                    where name like @name
                                                    order by sequence limit {1}, {2}                                                   
                                                ) as t
                                                )", StoreHelper.GetSMessageExecuteTypeSelectFields(string.Empty), (page - 1) * pageSize, pageSize)
                })
                {



                    var parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 200)
                    {
                        Value = string.Format("{0}%", name.ToMySqlLike())
                    };
                    commond.Parameters.Add(parameter);


                    await commond.PrepareAsync();


                    DbDataReader reader = null;

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result.TotalCount = reader.GetInt32(0);
                            result.CurrentPage = page;

                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    var sMessageExecuteType = new SMessageExecuteType();
                                    StoreHelper.SetSMessageExecuteTypeSelectFields(sMessageExecuteType, reader, string.Empty);

                                    result.Results.Add(sMessageExecuteType);
                                }
                            }
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return result;
        }

        public async Task<SMessageExecuteType> QueryById(Guid id)
        {
            SMessageExecuteType type = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _messageQueueConnectionFactory.CreateReadForMessageQueueMain(), async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }

                await using (MySqlCommand commond = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = string.Format(@"select {0} from smessageexecutetype where id=@id", StoreHelper.GetSMessageExecuteTypeSelectFields(string.Empty))
                })
                {

                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    DbDataReader reader = null;

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            type = new SMessageExecuteType();
                            StoreHelper.SetSMessageExecuteTypeSelectFields(type, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return type;
        }

        public async Task<SMessageExecuteType> QueryByName(string name)
        {
            SMessageExecuteType type = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _messageQueueConnectionFactory.CreateReadForMessageQueueMain(), async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }

                await using (MySqlCommand commond = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = string.Format(@"select {0} from smessageexecutetype where name=@name", StoreHelper.GetSMessageExecuteTypeSelectFields(string.Empty))
                })
                {

                    var parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 150)
                    {
                        Value = name
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    DbDataReader reader = null;

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            type = new SMessageExecuteType();
                            StoreHelper.SetSMessageExecuteTypeSelectFields(type, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return type;
        }

        public async Task Update(SMessageExecuteType messageType)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _messageQueueConnectionFactory.CreateAllForMessageQueueMain(), async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }

                await using (MySqlCommand commond = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = @"update smessageexecutetype set name=@name,modifytime=utc_timestamp()
                                    where id=@id"
                })
                {

                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = messageType.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 150)
                    {
                        Value = messageType.Name
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();




                }
            });
        }
    }
}
