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
    [Injection(InterfaceType = typeof(ISQueueProcessGroupStore), Scope = InjectionScope.Singleton)]
    public class SQueueProcessGroupStore : ISQueueProcessGroupStore
    {
        private IMessageQueueConnectionFactory _messageQueueConnectionFactory;

        public SQueueProcessGroupStore(IMessageQueueConnectionFactory messageQueueConnectionFactory)
        {
            _messageQueueConnectionFactory = messageQueueConnectionFactory;
        }

        public async Task Add(SQueueProcessGroup group)
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
                    if (group.ID == Guid.Empty)
                    {
                        group.ID = Guid.NewGuid();
                    }

                        commond.CommandText = @"insert into squeueprocessgroup(id,name,createtime,modifytime)
                                    values(@id,@name,utc_timestamp(),utc_timestamp())";
                    

                    MySqlParameter parameter;


                        parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                        {
                            Direction = ParameterDirection.Output
                        };
                        commond.Parameters.Add(parameter);
                    


                    parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 150)
                    {
                        Value = group.Name
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
                    CommandText = @"delete from squeueprocessgroup where id=@id"
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

        public async Task<SQueueProcessGroup> QueryById(Guid id)
        {
            SQueueProcessGroup group = null;

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
                    CommandText = string.Format(@"select {0} from squeueprocessgroup where id=@id", StoreHelper.GetSQueueProcessGroupSelectFields(string.Empty))
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
                            group = new SQueueProcessGroup();
                            StoreHelper.SetSQueueProcessGroupSelectFields(group, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return group;
        }

        public async Task<SQueueProcessGroup> QueryByName(string name)
        {
            SQueueProcessGroup group = null;

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
                    CommandText = string.Format(@"select {0} from squeueprocessgroup where name=@name", StoreHelper.GetSQueueProcessGroupSelectFields(string.Empty))
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
                            group = new SQueueProcessGroup();
                            StoreHelper.SetSQueueProcessGroupSelectFields(group, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return group;
        }

        public async Task<QueryResult<SQueueProcessGroup>> QueryByName(string name, int page, int pageSize)
        {
            QueryResult<SQueueProcessGroup> result = new QueryResult<SQueueProcessGroup>();

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
                    CommandText = string.Format(@"select count(*) from squeueprocessgroup where name like @name

	                                   select {0} from squeueprocessgroup
                                                where sequence in
                                                (
                                                select t.sequence from
                                                (
                                                    select sequence from squeueprocessgroup 
                                                    where name like @name
                                                    order by sequence limit {1}, {2}                                                   
                                                ) as t
                                                )", StoreHelper.GetSQueueProcessGroupSelectFields(string.Empty), (page - 1) * pageSize, pageSize)
                })
                {


                    var parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 200)
                    {
                        Value = $"{name.ToMySqlLike()}%"
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
                                    var group = new SQueueProcessGroup();
                                    StoreHelper.SetSQueueProcessGroupSelectFields(group, reader, string.Empty);
                                    result.Results.Add(group);
                                }
                            }
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return result;
        }

        public async Task Update(SQueueProcessGroup group)
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
                    CommandText = @"update squeueprocessgroup set name=@name,modifytime=utc_timestamp()
                                    where id=@id"
                })
                {

                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = group.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 150)
                    {
                        Value = group.Name
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();

                }
            });
        }
    }
}
