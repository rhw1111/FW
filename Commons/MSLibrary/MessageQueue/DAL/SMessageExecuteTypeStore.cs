using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using MSLibrary.DI;
using MSLibrary.Transaction;

namespace MSLibrary.MessageQueue.DAL
{
    /// <summary>
    /// 消息处理类型数据操作
    /// 消息处理类型集中存储在消息队列的主信息数据库中
    /// </summary>
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

                    if (messageType.ID == Guid.Empty)
                    {
                        commond.CommandText = @"insert into SMessageExecuteType([id],[name],[createtime],[modifytime])
                                    values(default,@name,getutcdate(),getutcdate());
                                    select @newid=[id] from SMessageExecuteType where [sequence]=SCOPE_IDENTITY()";
                    }
                    else
                    {
                        commond.CommandText = @"insert into SMessageExecuteType([id],[name],[createtime],[modifytime])
                                    values(@id,@name,getutcdate(),getutcdate())";
                    }

                    SqlParameter parameter;
                    if (messageType.ID != Guid.Empty)
                    {
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = messageType.ID
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

                    parameter = new SqlParameter("@name", SqlDbType.VarChar, 150)
                    {
                        Value = messageType.Name
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();

                    if (messageType.ID == Guid.Empty)
                    {
                        messageType.ID = (Guid)commond.Parameters["@newid"].Value;
                    }
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
                    CommandText = @"declare @delnum int
                                    set @delnum = 1000
                                    while @delnum = 1000
                                    begin
                                        set @delnum = 0
                                        select top 1000 @delnum = @delnum + 1 from[dbo].[SMessageTypeListener] WITH(SNAPSHOT)
                                        where[messagetypeid] = @id
                                        delete from[dbo].[SMessageTypeListener] WITH(SNAPSHOT) from[dbo].[SMessageTypeListener] WITH(SNAPSHOT) join
                                        (
                                            select top 1000[id] from[dbo].[SMessageTypeListener] WITH(SNAPSHOT)
                                            where[messagetypeid] = @id
                                        ) as d
                                        on[dbo].[SMessageTypeListener].id = d.id
                                    end

                                    delete from[dbo].[SMessageExecuteType] where id = @id"
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

        public async Task<QueryResult<SMessageExecuteType>> Query(string name, int page, int pageSize)
        {
            QueryResult<SMessageExecuteType> result = new QueryResult<SMessageExecuteType>();

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
		                           select @count= count(*) from SMessageExecuteType where [name] like @name
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
	
                                    select {0} from SMessageExecuteType where [name] like @name
                                    order by [sequence] desc 
		                            offset (@pagesize * (@currentpage - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetSMessageExecuteTypeSelectFields(string.Empty))
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

                    parameter = new SqlParameter("@name", SqlDbType.VarChar, 200)
                    {
                        Value = string.Format("{0}%", name.ToSqlLike())
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
                            var type = new SMessageExecuteType();
                            StoreHelper.SetSMessageExecuteTypeSelectFields(type, reader, string.Empty);
                            result.Results.Add(type);
                        }

                         await reader.CloseAsync();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;
                    }
                }
            });

            return result;
        }

        public async Task<SMessageExecuteType> QueryById(Guid id)
        {
            SMessageExecuteType type = null;

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
                    CommandText = string.Format(@"select {0} from SMessageExecuteType where [id]=@id", StoreHelper.GetSMessageExecuteTypeSelectFields(string.Empty))
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
                    CommandText = string.Format(@"select {0} from SMessageExecuteType where [name]=@name", StoreHelper.GetSMessageExecuteTypeSelectFields(string.Empty))
                })
                {

                    var parameter = new SqlParameter("@name", SqlDbType.VarChar, 150)
                    {
                        Value = name
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    SqlDataReader reader = null;

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
                    CommandText = @"update SMessageExecuteType set [name]=@name,[modifytime]=getutcdate()
                                    where [id]=@id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = messageType.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@name", SqlDbType.VarChar, 150)
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
