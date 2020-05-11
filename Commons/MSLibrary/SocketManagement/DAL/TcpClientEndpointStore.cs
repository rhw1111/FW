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
    [Injection(InterfaceType = typeof(ITcpClientEndpointStore), Scope = InjectionScope.Singleton)]
    public class TcpClientEndpointStore : ITcpClientEndpointStore
    {
        private ISocketConnectionFactory _sockerConnectionFactory;

        public TcpClientEndpointStore(ISocketConnectionFactory sockerConnectionFactory)
        {
            _sockerConnectionFactory = sockerConnectionFactory;
        }

        public async Task Add(TcpClientEndpoint endpoint)
        {
            //获取读写连接字符串
            var strConn = _sockerConnectionFactory.CreateAllForSocket();
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
                     Transaction=sqlTran
                })
                {

                    if (endpoint.ID == Guid.Empty)
                    {
                        commond.CommandText = @"insert into TcpClientEndpoint([id],[name],[serveraddress],[serverport],[keepalive],[poolmaxsize],[executedatafactorytype],[executedatafactorytypeusedi],[heartbeatsenddata],[createtime],[modifytime])
                                    values(default,@name,@serveraddress,@serverport,@keepalive,@poolmaxsize,@executedatafactorytype,@executedatafactorytypeusedi,@heartbeatsenddata,getutcdate(),getutcdate());
                                    select @newid=[id] from TcpListenerLog where [sequence]=SCOPE_IDENTITY()";
                    }
                    else
                    {
                        commond.CommandText = @"insert into TcpClientEndpoint([id],[name],[serveraddress],[serverport],[keepalive],[poolmaxsize],[executedatafactorytype],[executedatafactorytypeusedi],[heartbeatsenddata],[createtime],[modifytime])
                                    values(@id,@name,@serveraddress,@serverport,@keepalive,@poolmaxsize,@executedatafactorytype,@executedatafactorytypeusedi,@heartbeatsenddata,getutcdate(),getutcdate())";
                    }

                    SqlParameter parameter;
                    if (endpoint.ID != Guid.Empty)
                    {
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = endpoint.ID
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

                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 150)
                    {
                        Value = endpoint.Name
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@serveraddress", SqlDbType.NVarChar, 150)
                    {
                        Value = endpoint.ServerAddress
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@serverport", SqlDbType.Int)
                    {
                        Value = endpoint.ServerPort
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@keepalive", SqlDbType.Bit)
                    {
                        Value = endpoint.KeepAlive
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@poolmaxsize", SqlDbType.Int)
                    {
                        Value = endpoint.PoolMaxSize
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@executedatafactorytype", SqlDbType.NVarChar,150)
                    {
                        Value = endpoint.ExecuteDataFactoryType
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@executedatafactorytypeusedi", SqlDbType.Bit)
                    {
                        Value = endpoint.ExecuteDataFactoryTypeUseDI
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@heartbeatsenddata", SqlDbType.NVarChar, 500)
                    {
                        Value = endpoint.HeartBeatSendData
                    };
                    commond.Parameters.Add(parameter);


                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();

                    if (endpoint.ID == Guid.Empty)
                    {
                        endpoint.ID = (Guid)commond.Parameters["@newid"].Value;
                    }
                }
            });
        }

        public async Task Delete(Guid id)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _sockerConnectionFactory.CreateAllForSocket(), async (conn,transaction) =>
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
                    CommandText = @"delete from TcpClientEndpoint where [id]=@id",
                     Transaction=sqlTran
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

        public async Task<TcpClientEndpoint> QueryById(Guid id)
        {
            TcpClientEndpoint endpoint = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _sockerConnectionFactory.CreateReadForSocket(), async (conn,transaction) =>
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
                    CommandText = string.Format(@"select {0} from TcpClientEndpoint where [id]=@id", StoreHelper.GetTcpClientEndpointSelectFields(string.Empty))
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    SqlDataReader reader = null;

                    await using (reader= await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            endpoint = new TcpClientEndpoint();
                            StoreHelper.SetTcpClientEndpointSelectFields(endpoint, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return endpoint;
        }

        public async Task<TcpClientEndpoint> QueryByName(string name)
        {
            TcpClientEndpoint endpoint = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _sockerConnectionFactory.CreateReadForSocket(), async (conn,transaction) =>
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
                    CommandText = string.Format(@"select {0} from TcpClientEndpoint where [name]=@name", StoreHelper.GetTcpClientEndpointSelectFields(string.Empty))
                })
                {

                    var parameter = new SqlParameter("@name", SqlDbType.NVarChar, 150)
                    {
                        Value = name
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    SqlDataReader reader = null;
                    reader = await commond.ExecuteReaderAsync();            

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            endpoint = new TcpClientEndpoint();
                            StoreHelper.SetTcpClientEndpointSelectFields(endpoint, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return endpoint;
        }

        public async Task<QueryResult<TcpClientEndpoint>> QueryByName(string name, int page, int pageSize)
        {
            QueryResult<TcpClientEndpoint> result = new QueryResult<TcpClientEndpoint>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _sockerConnectionFactory.CreateReadForSocket(), async (conn,transaction) =>
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
		                           select @count= count(*) from TcpClientEndpoint where [name] like @name

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
	
                                    select {0} from TcpClientEndpoint where [name] like @name  
                                    order by [sequence]
		                            offset (@pagesize * (@currentpage - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetTcpClientEndpointSelectFields(string.Empty))
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

                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 150)
                    {
                        Value = $"{name.ToSqlLike()}%"
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

                    await using (reader= await commond.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var endpoint = new TcpClientEndpoint();
                            StoreHelper.SetTcpClientEndpointSelectFields(endpoint, reader, string.Empty);
                            result.Results.Add(endpoint);
                        }

                        await reader.CloseAsync();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;

                    }
                }
            });

            return result;
        }

        public async Task Update(TcpClientEndpoint endpoint)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _sockerConnectionFactory.CreateAllForSocket(), async (conn,transaction) =>
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
                    CommandText = @"update TcpClientEndpoint set [name]=@name,[serveraddress]=@serveraddress,[serverport]=@serverport,[keepalive]=@keepalive,[poolmaxsize]=@poolmaxsize,[executedatafactorytype]=@executedatafactorytype,[executedatafactorytypeusedi]=@executedatafactorytypeusedi,[heartbeatsenddata]=@heartbeatsenddata,[modifytime]=getutcdate()
                                    where [id]=@id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = endpoint.ID
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 150)
                    {
                        Value = endpoint.Name
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@serveraddress", SqlDbType.NVarChar, 150)
                    {
                        Value = endpoint.ServerAddress
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@serverport", SqlDbType.Int)
                    {
                        Value = endpoint.ServerPort
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@keepalive", SqlDbType.Bit)
                    {
                        Value = endpoint.KeepAlive
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@poolmaxsize", SqlDbType.Int)
                    {
                        Value = endpoint.PoolMaxSize
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@executedatafactorytype", SqlDbType.NVarChar, 150)
                    {
                        Value = endpoint.ExecuteDataFactoryType
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@executedatafactorytypeusedi", SqlDbType.Bit)
                    {
                        Value = endpoint.ExecuteDataFactoryTypeUseDI
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@heartbeatsenddata", SqlDbType.NVarChar, 500)
                    {
                        Value = endpoint.HeartBeatSendData
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();
                    await commond.ExecuteNonQueryAsync();
                }
            });
        }
    }
}
