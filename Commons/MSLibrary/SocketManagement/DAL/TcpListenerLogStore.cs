using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using MSLibrary.DI;
using MSLibrary.Transaction;

namespace MSLibrary.SocketManagement.DAL
{
    [Injection(InterfaceType = typeof(ITcpListenerLogStore), Scope = InjectionScope.Singleton)]
    public class TcpListenerLogStore : ITcpListenerLogStore
    {
        private ISocketConnectionFactory _sockerConnectionFactory;

        public TcpListenerLogStore(ISocketConnectionFactory sockerConnectionFactory)
        {
            _sockerConnectionFactory = sockerConnectionFactory;
        }

        public async Task Add(TcpListenerLog log)
        {
            //获取读写连接字符串
            var strConn = _sockerConnectionFactory.CreateAllForTcpLog(log.ListenerName);
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, strConn, async (conn,transaction) =>
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

                    if (log.ID == Guid.Empty)
                    {
                        commond.CommandText = @"insert into TcpListenerLog([id],[listenername],[requesttime],[requestcontent],[executeduration],[responsecontent],[responsetime],[iserror],[errormessage])
                                    values(default,@listenername,@requesttime,@requestcontent,@executeduration,@responsecontent,@responsetime,@iserror,@errormessage);
                                    select @newid=[id] from TcpListenerLog where [sequence]=SCOPE_IDENTITY()";
                    }
                    else
                    {
                        commond.CommandText = @"insert into TcpListener([id],[listenername],[requesttime],[requestcontent],[executeduration],[responsecontent],[responsetime],[iserror],[errormessage])
                                    values(@id,@listenername,@requesttime,@requestcontent,@executeduration,@responsecontent,@responsetime,@iserror,@errormessage)";
                    }

                    SqlParameter parameter;
                    int parameterSize;
                    if (log.ID != Guid.Empty)
                    {
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = log.ID
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

                    parameter = new SqlParameter("@listenername", SqlDbType.NVarChar, 150)
                    {
                        Value = log.ListenerName
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@requesttime", SqlDbType.DateTime)
                    {
                        Value = log.RequestTime
                    };
                    commond.Parameters.Add(parameter);

                    if (log.RequestContent.Length == 0)
                    {
                        parameterSize = 500;
                    }
                    else
                    {
                        parameterSize = log.RequestContent.Length;
                    }
                    parameter = new SqlParameter("@requestcontent", SqlDbType.NVarChar, parameterSize)
                    {
                        Value = log.RequestContent
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@executeduration", SqlDbType.Int)
                    {
                        Value = log.ExecuteDuration
                    };
                    commond.Parameters.Add(parameter);

                    if (log.ResponseContent.Length == 0)
                    {
                        parameterSize = 500;
                    }
                    else
                    {
                        parameterSize = log.ResponseContent.Length;
                    }
                    parameter = new SqlParameter("@responsecontent", SqlDbType.NVarChar, parameterSize)
                    {
                        Value = log.ResponseContent
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@responsetime", SqlDbType.DateTime)
                    {
                        Value = log.ResponseTime
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@iserror", SqlDbType.Bit)
                    {
                        Value = log.IsError
                    };
                    commond.Parameters.Add(parameter);

                    if (log.ErrorMessage.Length == 0)
                    {
                        parameterSize = 500;
                    }
                    else
                    {
                        parameterSize = log.ErrorMessage.Length;
                    }
                    parameter = new SqlParameter("@errormessage", SqlDbType.NVarChar, parameterSize)
                    {
                        Value = log.ErrorMessage
                    };
                    commond.Parameters.Add(parameter);


                    await commond.PrepareAsync();


                    await commond.ExecuteNonQueryAsync();




                    if (log.ID == Guid.Empty)
                    {
                        log.ID = (Guid)commond.Parameters["@newid"].Value;
                    }
                }
            });
        }

        public async Task Delete(string listenerName,Guid id)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _sockerConnectionFactory.CreateAllForTcpLog(listenerName), async (conn,transaction) =>
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
                    CommandText = @"delete from TcpListenerLog where [id]=@id"
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

        public async Task<TcpListenerLog> QueryById(string listenerName,Guid id)
        {
            TcpListenerLog log = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _sockerConnectionFactory.CreateReadForTcpLog(listenerName), async (conn,transaction) =>
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
                     Transaction=sqlTran,
                    CommandText = string.Format(@"select {0} from TcpListenerLog where [id]=@id", StoreHelper.GetTcpListenerLogSelectFields(string.Empty))
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
                            log = new TcpListenerLog();
                            StoreHelper.SetTcpListenerLogSelectFields(log, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return log;
        }

        public async Task<QueryResult<TcpListenerLog>> QueryByListener(string listenerName, int page, int pageSize)
        {
            QueryResult<TcpListenerLog> result = new QueryResult<TcpListenerLog>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _sockerConnectionFactory.CreateReadForTcpLog(listenerName), async (conn,transaction) =>
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
                     Transaction=sqlTran,
                    CommandText = string.Format(@"set @currentpage=@page
		                           select @count= count(*) from TcpListenerLog where [listenername] like @listenername

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
	
                                    select {0} from TcpListenerLog where [listenername] like @listenername
                                    order by [sequence]
		                            offset (@pagesize * (@currentpage - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetTcpListenerLogSelectFields(string.Empty))
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

                    parameter = new SqlParameter("@listenername", SqlDbType.NVarChar, 150)
                    {
                        Value = $"{listenerName.ToSqlLike()}%"
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
                            var log = new TcpListenerLog();
                            StoreHelper.SetTcpListenerLogSelectFields(log, reader, string.Empty);
                            result.Results.Add(log);
                        }

                       await reader.CloseAsync();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;

                    }
                }
            });

            return result;
        }

        public async Task<List<TcpListenerLog>> QueryLatestByListener(string listenerName, DateTime requestTime, Guid? latestId, int size)
        {
            List<TcpListenerLog> result = new List<TcpListenerLog>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _sockerConnectionFactory.CreateReadForTcpLog(listenerName), async (conn,transaction) =>
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
                    if (latestId.HasValue)
                    {
                        commond.CommandText = string.Format(@"declare @num int
                                                set @num=0
                                                select @num=num1233 from
                                                (
                                                    select row_number() over(order by [requesttime]) as num1233,id from [TcpListenerLog]
                                                    where [requesttime]>=@requesttime
                                                ) as t
                                                where id=@latestid

                                                select top (@size) * from
                                                (
                                                    select row_number() over(order by [requesttime]) as num1233,{0} from [dbo].[TcpListenerLog]
                                                    where [requesttime]>=@requesttime
                                                ) as t
                                                where num1233>@num", StoreHelper.GetTcpListenerLogSelectFields(string.Empty));
                    }
                    else
                    {
                        commond.CommandText = string.Format(@"select top (@size) {0} from TcpListenerLog where [listenername]=@listenername and [requesttime]>=@requesttime
                                    order by [requesttime]", StoreHelper.GetTcpListenerLogSelectFields(string.Empty));
                    }
                    

                    var parameter = new SqlParameter("@size", SqlDbType.Int)
                    {
                        Value = size
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@requesttime", SqlDbType.DateTime)
                    {
                        Value = requestTime
                    };
                    commond.Parameters.Add(parameter);

                    if (latestId.HasValue)
                    {
                        parameter = new SqlParameter("@latestid", SqlDbType.UniqueIdentifier)
                        {
                            Value=latestId
                        };
                        commond.Parameters.Add(parameter);
                    }

                    await commond.PrepareAsync();

                 
                    SqlDataReader reader = null;

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var log = new TcpListenerLog();
                            StoreHelper.SetTcpListenerLogSelectFields(log, reader, string.Empty);
                            result.Add(log);
                        }

                        await reader.CloseAsync();

                    }
                }
            });

            return result;
        }
    }
}
