using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using MSLibrary.DI;
using MSLibrary.Transaction;
using System.Runtime.InteropServices;
using MongoDB.Libmongocrypt;

namespace MSLibrary.MessageQueue.DAL
{
    /// <summary>
    /// 队列数据操作
    /// 队列集中存储在消息队列的主信息数据库中
    /// </summary>
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
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _messageQueueConnectionFactory.CreateAllForMessageQueueMain(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand commond = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran
                })
                {
                    if (queue.ID == Guid.Empty)
                    {
                        commond.CommandText = @"insert into SQueue([id],[groupname],[interval],[storetype],[servername],[name],[code],[isdead],[createtime],[modifytime])
                                    values(default,@groupname,@interval,@storetype,@servername,@name,@code,@isdead,getutcdate(),getutcdate());
                                    select @newid=[id] from SQueue where [sequence]=SCOPE_IDENTITY()";
                    }
                    else
                    {
                        commond.CommandText = @"insert into SQueue([id],[groupname],[interval],[storetype],[servername],[name],[code],[isdead],[createtime],[modifytime])
                                    values(@id,@groupname,@interval,@storetype,@servername,@name,@code,@isdead,getutcdate(),getutcdate())";
                    }

                    SqlParameter parameter;
                    if (queue.ID != Guid.Empty)
                    {
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = queue.ID
                        };
                        commond.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new SqlParameter("@newid", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };
                        commond.Parameters.Add(parameter);
                    }

                    parameter = new SqlParameter("@groupname", SqlDbType.VarChar, 150)
                    {
                        Value = queue.GroupName
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@interval", SqlDbType.Int)
                    {
                        Value = queue.Interval
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@storetype", SqlDbType.Int)
                    {
                        Value = queue.StoreType
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@servername", SqlDbType.VarChar, 200)
                    {
                        Value = queue.ServerName
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@name", SqlDbType.VarChar, 150)
                    {
                        Value = queue.Name
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@code", SqlDbType.Int)
                    {
                        Value = queue.Code
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@isdead", SqlDbType.Bit)
                    {
                        Value = queue.IsDead
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();

                    if (queue.ID == Guid.Empty)
                    {
                        queue.ID = (Guid)commond.Parameters["@newid"].Value;
                    }
                }
            });
        }

        public async Task AddProcessGroupRelation(Guid processGroupId, Guid queueId)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _messageQueueConnectionFactory.CreateAllForMessageQueueMain(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand commond = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = @"update SQueue set processgroupid=@processgroupid where [id]=@id and processgroupid is null"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = queueId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@processgroupid", SqlDbType.UniqueIdentifier)
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
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _messageQueueConnectionFactory.CreateAllForMessageQueueMain(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand commond = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = @"delete from SQueue where [id]=@id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
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
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _messageQueueConnectionFactory.CreateAllForMessageQueueMain(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand commond = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = @"update SQueue set processgroupid=null where [id]=@id and processgroupid=@processgroupid"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = queueId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@processgroupid", SqlDbType.UniqueIdentifier)
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

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _messageQueueConnectionFactory.CreateReadForMessageQueueMain(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand commond = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = string.Format(@"set @currentpage=@page
		                           select @count= count(*) from SQueue
		                           if @pagesize*@page>=@count
			                          begin
				                           set @currentpage= @count/@pagesize
				                           if @count%@pagesize<>0
					                           begin
						                            set @currentpage=@currentpage+1
					                           end
				                           if @currentpage=0
					                           set @currentpage=1
			                          end
		                            else if @page<1 
			                           begin 
				                           set @currentpage=1
			                           end
	
                                    select {0} from SQueue
                                    order by [sequence] desc 
		                            offset (@pagesize * (@currentpage - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetSQueueSelectFields(string.Empty))
                })
                {

                    var parameter = new SqlParameter("@page", SqlDbType.Int)
                    {
                        Value = page
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@pagesize", SqlDbType.Int)
                    {
                        Value = pageSize
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@count", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@currentpage", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();


                    SqlDataReader reader = null;

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var queue = new SQueue();
                            StoreHelper.SetSQueueSelectFields(queue, reader, string.Empty);
                            result.Results.Add(queue);
                        }

                        await reader.CloseAsync();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;
                    }
                }
            });

            return result;
        }

        public async Task<SQueue> QueryByCode(string groupName, bool isDead, int code)
        {
            SQueue queue = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _messageQueueConnectionFactory.CreateReadForMessageQueueMain(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand commond = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = string.Format(@"select {0} from SQueue where [groupname]=@groupname and [code]=@code and [isdead]=@isdead", StoreHelper.GetSQueueSelectFields(string.Empty))
                })
                {

                    var parameter = new SqlParameter("@groupname", SqlDbType.VarChar, 150)
                    {
                        Value = groupName
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@code", SqlDbType.Int)
                    {
                        Value = code
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@isdead", SqlDbType.Bit)
                    {
                        Value = isDead
                    };
                    commond.Parameters.Add(parameter);


                    await commond.PrepareAsync();

                    SqlDataReader reader = null;

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

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _messageQueueConnectionFactory.CreateReadForMessageQueueMain(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand commond = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = string.Format(@"set @currentpage=@page
		                           select @count= count(*) from SQueue where [isdead]=@isdead and [groupname] = @groupname
		                           if @pagesize*@page>=@count
			                          begin
				                           set @currentpage= @count/@pagesize
				                           if @count%@pagesize<>0
					                           begin
						                            set @currentpage=@currentpage+1
					                           end
				                           if @currentpage=0
					                           set @currentpage=1
			                          end
		                            else if @page<1 
			                           begin 
				                           set @currentpage=1
			                           end
	
                                    select {0} from SQueue where [isdead]=@isdead and [groupname] = @groupname
                                    order by [sequence] desc 
		                            offset (@pagesize * (@currentpage - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetSQueueSelectFields(string.Empty))
                })
                {

                    var parameter = new SqlParameter("@page", SqlDbType.Int)
                    {
                        Value = page
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@pagesize", SqlDbType.Int)
                    {
                        Value = pageSize
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@isdead", SqlDbType.Bit)
                    {
                        Value = isDead
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@groupname", SqlDbType.VarChar, groupName.Length)
                    {
                        Value = groupName
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@count", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@currentpage", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    SqlDataReader reader = null;

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var queue = new SQueue();
                            StoreHelper.SetSQueueSelectFields(queue, reader, string.Empty);
                            result.Results.Add(queue);
                        }

                        await reader.CloseAsync();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;
                    }
                }
            });

            return result;
        }

        public async Task<SQueue> QueryById(Guid id)
        {
            SQueue queue = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _messageQueueConnectionFactory.CreateReadForMessageQueueMain(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand commond = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = string.Format(@"select {0} from SQueue where [id]=@id", StoreHelper.GetSQueueSelectFields(string.Empty))
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    SqlDataReader reader = null;

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

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _messageQueueConnectionFactory.CreateReadForMessageQueueMain(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand commond = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = string.Format(@"set @currentpage=@page
		                           select @count= count(*) from SQueue where processgroupid is null
		                           if @pagesize*@page>=@count
			                          begin
				                           set @currentpage= @count/@pagesize
				                           if @count%@pagesize<>0
					                           begin
						                            set @currentpage=@currentpage+1
					                           end
				                           if @currentpage=0
					                           set @currentpage=1
			                          end
		                            else if @page<1 
			                           begin 
				                           set @currentpage=1
			                           end
	
                                    select {0} from SQueue where processgroupid is null
                                    order by [sequence]
		                            offset (@pagesize * (@currentpage - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetSQueueSelectFields(string.Empty))
                })
                {

                    var parameter = new SqlParameter("@page", SqlDbType.Int)
                    {
                        Value = page
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@pagesize", SqlDbType.Int)
                    {
                        Value = pageSize
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@count", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@currentpage", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    SqlDataReader reader = null;

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var queue = new SQueue();
                            StoreHelper.SetSQueueSelectFields(queue, reader, string.Empty);
                            result.Results.Add(queue);
                        }

                        await reader.CloseAsync();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;
                    }
                }
            });

            return result;
        }

        public async Task QueryByProceeGroup(Guid processGroupId, Func<SQueue, Task> callback)
        {
            List<SQueue> queueList = new List<SQueue>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _messageQueueConnectionFactory.CreateReadForMessageQueueMain(), async (conn, transaction) =>
            {
                Int64? sequence = null;
                int pageSize = 500;

                while (true)
                {
                    queueList.Clear();

                    SqlTransaction sqlTran = null;
                    if (transaction != null)
                    {
                        sqlTran = (SqlTransaction)transaction;
                    }

                    await using (SqlCommand commond = new SqlCommand()
                    {
                        Connection = (SqlConnection)conn,
                        CommandType = CommandType.Text,
                        Transaction = sqlTran
                    })
                    {
                        if (!sequence.HasValue)
                        {
                            commond.CommandText = string.Format(@"select top (@pagesize) {0} from SQueue where processgroupid=@processgroupid order by [sequence]", StoreHelper.GetSQueueSelectFields(string.Empty));
                        }
                        else
                        {
                            commond.CommandText = string.Format(@"select top (@pagesize) {0} from SQueue where processgroupid=@processgroupid and [sequence]>@sequence order by [sequence]", StoreHelper.GetSQueueSelectFields(string.Empty));
                        }

                        var parameter = new SqlParameter("@processgroupid", SqlDbType.UniqueIdentifier)
                        {
                            Value = processGroupId
                        };
                        commond.Parameters.Add(parameter);

                        parameter = new SqlParameter("@pagesize", SqlDbType.Int)
                        {
                            Value = pageSize
                        };
                        commond.Parameters.Add(parameter);

                        if (sequence.HasValue)
                        {
                            parameter = new SqlParameter("@sequence", SqlDbType.BigInt)
                            {
                                Value = sequence
                            };
                            commond.Parameters.Add(parameter);
                        }

                        await commond.PrepareAsync();


                        SqlDataReader reader = null;

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

        public async Task<QueryResult<SQueue>> QueryByProceeGroup(Guid processGroupId, int page, int pageSize)
        {
            QueryResult<SQueue> result = new QueryResult<SQueue>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _messageQueueConnectionFactory.CreateReadForMessageQueueMain(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand commond = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = string.Format(@"set @currentpage=@page
		                           select @count= count(*) from SQueue where processgroupid=@processgroupid
		                           if @pagesize*@page>=@count
			                          begin
				                           set @currentpage= @count/@pagesize
				                           if @count%@pagesize<>0
					                           begin
						                            set @currentpage=@currentpage+1
					                           end
				                           if @currentpage=0
					                           set @currentpage=1
			                          end
		                            else if @page<1 
			                           begin 
				                           set @currentpage=1
			                           end
	
                                    select {0} from SQueue where processgroupid=@processgroupid
                                    order by [sequence]
		                            offset (@pagesize * (@currentpage - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetSQueueSelectFields(string.Empty))
                })
                {

                    var parameter = new SqlParameter("@page", SqlDbType.Int)
                    {
                        Value = page
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@pagesize", SqlDbType.Int)
                    {
                        Value = pageSize
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@processgroupid", SqlDbType.UniqueIdentifier)
                    {
                        Value = processGroupId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@count", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@currentpage", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    SqlDataReader reader = null;

                    using (reader = await commond.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var queue = new SQueue();
                            StoreHelper.SetSQueueSelectFields(queue, reader, string.Empty);
                            result.Results.Add(queue);
                        }

                        await reader.CloseAsync();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;
                    }
                }
            });

            return result;
        }

        public async Task<QueryResult<SQueue>> QueryByProceeGroup(string processGroupName, int page, int pageSize)
        {

            QueryResult<SQueue> result = new QueryResult<SQueue>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _messageQueueConnectionFactory.CreateReadForMessageQueueMain(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand commond = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = string.Format(@"set @currentpage=@page
		                           select @count= count(*) from SQueue as q join SQueueProcessGroup as g on q.processgroupid=g.id where g.[name]=@processgroupname
		                           if @pagesize*@page>=@count
			                          begin
				                           set @currentpage= @count/@pagesize
				                           if @count%@pagesize<>0
					                           begin
						                            set @currentpage=@currentpage+1
					                           end
				                           if @currentpage=0
					                           set @currentpage=1
			                          end
		                            else if @page<1 
			                           begin 
				                           set @currentpage=1
			                           end
	
                                    select {0} from SQueue as q join SQueueProcessGroup as g on q.processgroupid=g.id where g.[name]=@processgroupname
                                    order by [sequence]
		                            offset (@pagesize * (@currentpage - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetSQueueSelectFields("q"))
                })
                {

                    var parameter = new SqlParameter("@page", SqlDbType.Int)
                    {
                        Value = page
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@pagesize", SqlDbType.Int)
                    {
                        Value = pageSize
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@processgroupname", SqlDbType.NVarChar, 100)
                    {
                        Value = processGroupName
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@count", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@currentpage", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    SqlDataReader reader = null;

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var queue = new SQueue();
                            StoreHelper.SetSQueueSelectFields(queue, reader, string.Empty);
                            result.Results.Add(queue);
                        }

                        await reader.CloseAsync();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;
                    }
                }
            });

            return result;
        }

        public async Task QueryByProceeGroup(string processGroupName, Func<SQueue, Task> callback)
        {
            List<SQueue> queueList = new List<SQueue>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _messageQueueConnectionFactory.CreateReadForMessageQueueMain(), async (conn, transaction) =>
            {
                Int64? sequence = null;
                int pageSize = 500;

                while (true)
                {
                    queueList.Clear();

                    SqlTransaction sqlTran = null;
                    if (transaction != null)
                    {
                        sqlTran = (SqlTransaction)transaction;
                    }

                    await using (SqlCommand commond = new SqlCommand()
                    {
                        Connection = (SqlConnection)conn,
                        CommandType = CommandType.Text,
                        Transaction = sqlTran
                    })
                    {
                        if (!sequence.HasValue)
                        {
                            commond.CommandText = string.Format(@"select top (@pagesize) {0} from SQueue as q join SQueueProcessGroup as g on q.processgroupid=g.id where g.[name]=@processgroupname order by [sequence]", StoreHelper.GetSQueueSelectFields("q"));
                        }
                        else
                        {
                            commond.CommandText = string.Format(@"select top (@pagesize) {0} from SQueue as q join SQueueProcessGroup as g on q.processgroupid=g.id where g.[name]=@processgroupname and [q.sequence]>@sequence order by [sequence]", StoreHelper.GetSQueueSelectFields("q"));
                        }

                        var parameter = new SqlParameter("@processgroupname", SqlDbType.VarChar, 150)
                        {
                            Value = processGroupName
                        };
                        commond.Parameters.Add(parameter);

                        parameter = new SqlParameter("@pagesize", SqlDbType.Int)
                        {
                            Value = pageSize
                        };
                        commond.Parameters.Add(parameter);

                        if (sequence.HasValue)
                        {
                            parameter = new SqlParameter("@sequence", SqlDbType.BigInt)
                            {
                                Value = sequence
                            };
                            commond.Parameters.Add(parameter);
                        }

                        await commond.PrepareAsync();


                        SqlDataReader reader = null;

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

        public async Task Update(SQueue queue)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _messageQueueConnectionFactory.CreateAllForMessageQueueMain(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand commond = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = @"update SQueue set [groupname]=@groupname,[interval]=@interval,[storetype]=@storetype,[servername]=@servername,[name]=@name,[code]=@code,[isdead]=@isdead,[modifytime]=getutcdate()
                                    where [id]=@id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = queue.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@groupname", SqlDbType.VarChar, 150)
                    {
                        Value = queue.GroupName
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@interval", SqlDbType.Int)
                    {
                        Value = queue.Interval
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@storetype", SqlDbType.Int)
                    {
                        Value = queue.StoreType
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@servername", SqlDbType.VarChar, 200)
                    {
                        Value = queue.ServerName
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@name", SqlDbType.VarChar, 150)
                    {
                        Value = queue.Name
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@code", SqlDbType.Int)
                    {
                        Value = queue.Code
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@isdead", SqlDbType.Bit)
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
