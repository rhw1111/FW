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
    [Injection(InterfaceType = typeof(ISQueueStore), Scope = InjectionScope.Singleton)]
    public class SQueueStore : ISQueueStore
    {
        private IMessageQueueConnectionFactory _messageQueueConnectionFactory;

        public SQueueStore(IMessageQueueConnectionFactory messageQueueConnectionFactory)
        {
            _messageQueueConnectionFactory = messageQueueConnectionFactory;
        }

        public async Task Add(SQueue queue)
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
                    if (queue.ID == Guid.Empty)
                    {
                        queue.ID = Guid.NewGuid();
                    }

                        commond.CommandText = @"insert into squeue(id,groupname,interval,storetype,servername,name,code,isdead,createtime,modifytime)
                                    values(@id,@groupname,@interval,@storetype,@servername,@name,@code,@isdead,utc_timestamp(),utc_timestamp())";
                    

                    MySqlParameter parameter;
     
                        parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                        {
                            Value = queue.ID
                        };
                        commond.Parameters.Add(parameter);


                    parameter = new MySqlParameter("@groupname", MySqlDbType.VarChar, 150)
                    {
                        Value = queue.GroupName
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@interval", MySqlDbType.Int32)
                    {
                        Value = queue.Interval
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new MySqlParameter("@storetype", MySqlDbType.Int32)
                    {
                        Value = queue.StoreType
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@servername", MySqlDbType.VarChar, 200)
                    {
                        Value = queue.ServerName
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 150)
                    {
                        Value = queue.Name
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@code", MySqlDbType.Int32)
                    {
                        Value = queue.Code
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@isdead", MySqlDbType.Bit)
                    {
                        Value = queue.IsDead
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();

                }
            });
        }

        public async Task AddProcessGroupRelation(Guid processGroupId, Guid queueId)
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
                    CommandText = @"update squeue set processgroupid=@processgroupid where id=@id and processgroupid is null"
                })
                {

                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = queueId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@processgroupid", MySqlDbType.Guid)
                    {
                        Value = processGroupId
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
                    CommandText = @"delete from squeue where id=@id"
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

        public async Task DeleteProcessGroupRelation(Guid processGroupId, Guid queueId)
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
                    CommandText = @"update squeue set processgroupid=null where id=@id and processgroupid=@processgroupid"
                })
                {

                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = queueId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@processgroupid", MySqlDbType.Guid)
                    {
                        Value = processGroupId
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();

                }
            });
        }

        public async Task<QueryResult<SQueue>> Query(int page, int pageSize)
        {
            QueryResult<SQueue> result = new QueryResult<SQueue>();

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
		                           select count(*) from squeue

	                                   select {0} from squeue
                                                where sequence in
                                                (
                                                select t.sequence from
                                                (
                                                    select sequence from squeue 
                                                    order by sequence limit {1}, {2}                                                   
                                                ) as t
                                                )", StoreHelper.GetSQueueSelectFields(string.Empty), (page - 1) * pageSize, pageSize)
                })
                {

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
                                    var queue = new SQueue();
                                    StoreHelper.SetSQueueSelectFields(queue, reader, string.Empty);
                                    result.Results.Add(queue);
                                }
                            }
                        }

                        await reader.CloseAsync();

                    }
                }
            });

            return result;
        }

        public async Task<SQueue> QueryByCode(string groupName, bool isDead, int code)
        {
            SQueue queue = null;

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
                    CommandText = string.Format(@"select {0} from squeue where groupname=@groupname and code=@code and isdead=@isdead", StoreHelper.GetSQueueSelectFields(string.Empty))
                })
                {

                    var parameter = new MySqlParameter("@groupname", MySqlDbType.VarChar, 150)
                    {
                        Value = groupName
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@code", MySqlDbType.Int32)
                    {
                        Value = code
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@isdead", MySqlDbType.Bit)
                    {
                        Value = isDead
                    };
                    commond.Parameters.Add(parameter);


                    await commond.PrepareAsync();

                    DbDataReader reader = null;

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            queue = new SQueue();
                            StoreHelper.SetSQueueSelectFields(queue, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return queue;
        }

        public async Task<QueryResult<SQueue>> QueryByGroup(string groupName, bool isDead, int page, int pageSize)
        {
            QueryResult<SQueue> result = new QueryResult<SQueue>();

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
		                           select count(*) from squeue where isdead=@isdead and groupname = @groupname

		                           select {0} from squeue
                                                where sequence in
                                                (
                                                select t.sequence from
                                                (
                                                    select sequence from squeue 
                                                    where isdead=@isdead and groupname = @groupname
                                                    order by sequence limit {1}, {2}                                                   
                                                ) as t
                                                )", StoreHelper.GetSQueueSelectFields(string.Empty), (page - 1) * pageSize, pageSize)
                })
                {



                    var parameter = new MySqlParameter("@isdead", MySqlDbType.Bit)
                    {
                        Value = isDead
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new MySqlParameter("@groupname", MySqlDbType.VarChar, groupName.Length)
                    {
                        Value = groupName
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
                                    var queue = new SQueue();
                                    StoreHelper.SetSQueueSelectFields(queue, reader, string.Empty);
                                    result.Results.Add(queue);
                                }
                            }
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return result;
        }

        public async Task<SQueue> QueryById(Guid id)
        {
            SQueue queue = null;

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
                    CommandText = string.Format(@"select {0} from squeue where id=@id", StoreHelper.GetSQueueSelectFields(string.Empty))
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
                            queue = new SQueue();
                            StoreHelper.SetSQueueSelectFields(queue, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return queue;
        }

        public async Task<QueryResult<SQueue>> QueryByNullProcessGroup(int page, int pageSize)
        {
            QueryResult<SQueue> result = new QueryResult<SQueue>();

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
                    CommandText = string.Format(@"select count(*) from squeue where processgroupid is null

			                           select {0} from squeue
                                                where sequence in
                                                (
                                                select t.sequence from
                                                (
                                                    select sequence from squeue 
                                                    where processgroupid is null
                                                    order by sequence limit {1}, {2}                                                   
                                                ) as t
                                                )", StoreHelper.GetSQueueSelectFields(string.Empty), (page - 1) * pageSize, pageSize)
                })
                {


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
                                    var queue = new SQueue();
                                    StoreHelper.SetSQueueSelectFields(queue, reader, string.Empty);
                                    result.Results.Add(queue);
                                }
                            }
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return result;
        }

        public async Task QueryByProceeGroup(Guid processGroupId, Func<SQueue, Task> callback)
        {
            List<SQueue> queueList = new List<SQueue>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _messageQueueConnectionFactory.CreateReadForMessageQueueMain(), async (conn, transaction) =>
            {
                Int64? sequence = null;
                int pageSize = 500;

                while (true)
                {
                    queueList.Clear();

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
                        if (!sequence.HasValue)
                        {
                            commond.CommandText = string.Format(@"select {0} from squeue where processgroupid=@processgroupid order by sequence limit {1}", StoreHelper.GetSQueueSelectFields(string.Empty),pageSize);
                        }
                        else
                        {
                            commond.CommandText = string.Format(@"select {0} from squeue where processgroupid=@processgroupid and sequence>@sequence order by sequence limit {1}", StoreHelper.GetSQueueSelectFields(string.Empty), pageSize);
                        }

                        var parameter = new MySqlParameter("@processgroupid", MySqlDbType.Guid)
                        {
                            Value = processGroupId
                        };
                        commond.Parameters.Add(parameter);

                        parameter = new MySqlParameter("@pagesize", MySqlDbType.Int32)
                        {
                            Value = pageSize
                        };
                        commond.Parameters.Add(parameter);

                        if (sequence.HasValue)
                        {
                            parameter = new MySqlParameter("@sequence", MySqlDbType.Int64)
                            {
                                Value = sequence
                            };
                            commond.Parameters.Add(parameter);
                        }

                        await commond.PrepareAsync();


                        DbDataReader reader = null;

                        await using (reader = await commond.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var queue = new SQueue();
                                StoreHelper.SetSQueueSelectFields(queue, reader, string.Empty);
                                sequence = (Int64)reader["sequence"];
                                queueList.Add(queue);
                            }

                            await reader.CloseAsync();
                        }


                    }

                    foreach (var queueItem in queueList)
                    {
                        await callback(queueItem);
                    }

                    if (queueList.Count != pageSize)
                    {
                        break;
                    }

                }

            });

        }

        public async Task QueryByProceeGroup(string processGroupName, Func<SQueue, Task> callback)
        {
            List<SQueue> queueList = new List<SQueue>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _messageQueueConnectionFactory.CreateReadForMessageQueueMain(), async (conn, transaction) =>
            {
                Int64? sequence = null;
                int pageSize = 500;

                while (true)
                {
                    queueList.Clear();

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
                        if (!sequence.HasValue)
                        {
                            commond.CommandText = string.Format(@"select {0} from squeue as q join squeueprocessgroup as g on q.processgroupid=g.id where g.name=@processgroupname order by sequence limit {1}", StoreHelper.GetSQueueSelectFields("q"),pageSize);
                        }
                        else
                        {
                            commond.CommandText = string.Format(@"select {0} from squeue as q join squeueprocessgroup as g on q.processgroupid=g.id where g.name=@processgroupname and q.sequence>@sequence order by sequence limit {1}", StoreHelper.GetSQueueSelectFields("q"), pageSize);
                        }

                        var parameter = new MySqlParameter("@processgroupname", MySqlDbType.VarChar, 150)
                        {
                            Value = processGroupName
                        };
                        commond.Parameters.Add(parameter);

                        parameter = new MySqlParameter("@pagesize", MySqlDbType.Int32)
                        {
                            Value = pageSize
                        };
                        commond.Parameters.Add(parameter);

                        if (sequence.HasValue)
                        {
                            parameter = new MySqlParameter("@sequence", MySqlDbType.Int64)
                            {
                                Value = sequence
                            };
                            commond.Parameters.Add(parameter);
                        }

                        await commond.PrepareAsync();


                        DbDataReader reader = null;

                        await using (reader = await commond.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var queue = new SQueue();
                                StoreHelper.SetSQueueSelectFields(queue, reader, string.Empty);
                                sequence = (Int64)reader["q.sequence"];
                                queueList.Add(queue);
                            }

                            await reader.CloseAsync();
                        }


                    }

                    foreach (var queueItem in queueList)
                    {
                        await callback(queueItem);
                    }

                    if (queueList.Count != pageSize)
                    {
                        break;
                    }

                }

            });

        }

        public async Task<QueryResult<SQueue>> QueryByProceeGroup(Guid processGroupId, int page, int pageSize)
        {
            QueryResult<SQueue> result = new QueryResult<SQueue>();

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
		                           select count(*) from squeue where processgroupid=@processgroupid

	
			                           select {0} from squeue
                                                where sequence in
                                                (
                                                select t.sequence from
                                                (
                                                    select sequence from squeue 
                                                    where processgroupid=@processgroupid
                                                    order by sequence limit {1}, {2}                                                   
                                                ) as t
                                                )", StoreHelper.GetSQueueSelectFields(string.Empty), (page - 1) * pageSize, pageSize)
                })
                {


                    var parameter = new MySqlParameter("@processgroupid", MySqlDbType.Guid)
                    {
                        Value = processGroupId
                    };
                    commond.Parameters.Add(parameter);


                    await commond.PrepareAsync();

                    DbDataReader reader = null;

                    using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result.TotalCount = reader.GetInt32(0);
                            result.CurrentPage = page;
                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    var queue = new SQueue();
                                    StoreHelper.SetSQueueSelectFields(queue, reader, string.Empty);
                                    result.Results.Add(queue);
                                }
                            }
                        }
                    }
                }
            });

            return result;
        }

        public async Task<QueryResult<SQueue>> QueryByProceeGroup(string processGroupName, int page, int pageSize)
        {
            QueryResult<SQueue> result = new QueryResult<SQueue>();

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
		                           select count(*) from squeue as q join squeueprocessgroup as g on q.processgroupid=g.id where g.name=@processgroupname


			                           select {0} from squeue
                                                where sequence in
                                                (
                                                select t.sequence from
                                                (
	                                                select q.sequence as sequence from squeue as q join squeueprocessgroup as g on q.processgroupid=g.id 
                                                    where g.name=@processgroupname
                                                    order by q.sequence limit {1}, {2}                                                   
                                                ) as t
                                                )", StoreHelper.GetSQueueSelectFields(string.Empty), (page - 1) * pageSize, pageSize)
                })
                {


                    var parameter = new MySqlParameter("@processgroupname", MySqlDbType.VarChar,100)
                    {
                        Value = processGroupName
                    };
                    commond.Parameters.Add(parameter);


                    await commond.PrepareAsync();

                    DbDataReader reader = null;

                    using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result.TotalCount = reader.GetInt32(0);
                            result.CurrentPage = page;
                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    var queue = new SQueue();
                                    StoreHelper.SetSQueueSelectFields(queue, reader, string.Empty);
                                    result.Results.Add(queue);
                                }
                            }
                        }
                    }
                }
            });

            return result;
        }

        public async Task Update(SQueue queue)
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
                    CommandText = @"update squeue set groupname=@groupname,interval=@interval,storetype=@storetype,servername=@servername,name=@name,code=@code,isdead=@isdead,modifytime=utc_timestamp()
                                    where id=@id"
                })
                {

                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = queue.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@groupname", MySqlDbType.VarChar, 150)
                    {
                        Value = queue.GroupName
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@interval", MySqlDbType.Int32)
                    {
                        Value = queue.Interval
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new MySqlParameter("@storetype", MySqlDbType.Int32)
                    {
                        Value = queue.StoreType
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@servername", MySqlDbType.VarChar, 200)
                    {
                        Value = queue.ServerName
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 150)
                    {
                        Value = queue.Name
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@code", MySqlDbType.Int32)
                    {
                        Value = queue.Code
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@isdead", MySqlDbType.Bit)
                    {
                        Value = queue.IsDead
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();

                }
            });
        }
    }
}
