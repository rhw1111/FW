using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using MSLibrary.DI;
using MSLibrary.Transaction;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.SocketManagement.DAL
{
    [Injection(InterfaceType = typeof(ITcpListenerStore), Scope = InjectionScope.Singleton)]
    public class TckListenerStore : ITcpListenerStore
    {
        private ISocketConnectionFactory _sockerConnectionFactory;

        public TckListenerStore(ISocketConnectionFactory sockerConnectionFactory)
        {
            _sockerConnectionFactory = sockerConnectionFactory;
        }
        public async Task Add(TcpListener listener)
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

                    if (listener.ID == Guid.Empty)
                    {
                        commond.CommandText = @"insert into TcpListener([id],[name],[port],[keepalive],[maxconcurrencycount],[maxbuffercount],[executedatafactorytype],[executedatafactorytypeusedi],[heartbeatsenddata],[description],[createtime],[modifytime])
                                    values(default,@name,@port,@keepalive,@maxconcurrencycount,@maxbuffercount,@executedatafactorytype,@executedatafactorytypeusedi,@heartbeatsenddata,@description,getutcdate(),getutcdate());
                                    select @newid=[id] from TcpListener where [sequence]=SCOPE_IDENTITY()";
                    }
                    else
                    {
                        commond.CommandText = @"insert into TcpListener([id],[name],[port],[keepalive],[maxconcurrencycount],[maxbuffercount],[executedatafactorytype],[executedatafactorytypeusedi],[heartbeatsenddata],[description],[createtime],[modifytime])
                                    values(@id,@name,@port,@keepalive,@maxconcurrencycount,@maxbuffercount,@executedatafactorytype,@executedatafactorytypeusedi,@heartbeatsenddata,@description,getutcdate(),getutcdate())";
                    }

                    SqlParameter parameter;
                    if (listener.ID != Guid.Empty)
                    {
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = listener.ID
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

                    parameter = new SqlParameter("@name", SqlDbType.NVarChar,150)
                    {
                        Value = listener.Name
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@port", SqlDbType.Int)
                    {
                        Value = listener.Port
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@keepalive", SqlDbType.Bit)
                    {
                        Value = listener.KeepAlive
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@maxconcurrencycount", SqlDbType.Int)
                    {
                        Value = listener.MaxConcurrencyCount
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@maxbuffercount", SqlDbType.Int)
                    {
                        Value = listener.MaxBufferCount
                    };
                    commond.Parameters.Add(parameter);

                    
                    parameter = new SqlParameter("@executedatafactorytype", SqlDbType.NVarChar,500)
                    {
                        Value = listener.ExecuteDataFactoryType
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@executedatafactorytypeusedi", SqlDbType.Bit)
                    {
                        Value = listener.ExecuteDataFactoryTypeUseDI
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@heartbeatsenddata", SqlDbType.NVarChar,500)
                    {
                        Value = listener.HeartBeatSendData
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@description", SqlDbType.NVarChar, 1000)
                    {
                        Value = listener.Description
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    int reply = 3;
                    while (true)
                    {
                        try
                        {
                            await commond.ExecuteNonQueryAsync();
                            break;
                        }
                        catch (SqlException ex)
                        {

                            if (reply > 0 && (ex.Number == 41302 || ex.Number == 41305 || ex.Number == 41325 || ex.Number == 41301 || ex.Number == 1205))
                            {
                                reply--;
                                System.Threading.Thread.Sleep(1);
                            }
                            else
                            {
                                if (ex.Number == 2601)
                                {
                                    var fragment = new TextFragment()
                                    {
                                        Code = TextCodes.ExistSameNameTcpListener,
                                        DefaultFormatting = "名称为{0}的Tcp监听器已经存在",
                                        ReplaceParameters = new List<object>() { listener.Name }
                                    };

                                    throw new UtilityException((int)Errors.ExistSameNameTcpListener, fragment);
                                }
                               
                                throw;
                            }
                        }
                    }

                    if (listener.ID == Guid.Empty)
                    {
                        listener.ID = (Guid)commond.Parameters["@newid"].Value;
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
                    CommandText = @"delete from TcpListener where [id]=@id",
                     Transaction=sqlTran
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    int reply = 3;
                    while (true)
                    {
                        try
                        {
                            await commond.ExecuteNonQueryAsync();
                            break;
                        }
                        catch (SqlException ex)
                        {

                            if (reply > 0 && (ex.Number == 41302 || ex.Number == 41305 || ex.Number == 41325 || ex.Number == 41301 || ex.Number == 1205))
                            {
                                reply--;
                                System.Threading.Thread.Sleep(1);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                }
            });
        }

        public async Task<TcpListener> QueryById(Guid id)
        {
            TcpListener listener = null;

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
                    CommandText = string.Format(@"select {0} from TcpListener where [id]=@id", StoreHelper.GetTcpListenerSelectFields(string.Empty))
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    int reply = 3;
                    SqlDataReader reader = null;
                    while (true)
                    {
                        if (reader != null && !reader.IsClosed)
                        {
                            try
                            {
                                await reader.CloseAsync();
                            }
                            catch
                            {

                            }
                        }

                        try
                        {
                            reader = await commond.ExecuteReaderAsync();
                            break;
                        }
                        catch (SqlException ex)
                        {

                            if (reply > 0 && (ex.Number == 41302 || ex.Number == 41305 || ex.Number == 41325 || ex.Number == 41301 || ex.Number == 1205))
                            {
                                reply--;
                                System.Threading.Thread.Sleep(1);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }

                    await using (reader)
                    {
                        if (await reader.ReadAsync())
                        {
                            listener = new TcpListener();
                            StoreHelper.SetTcpListenerSelectFields(listener, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return listener;
        }



        public async Task<TcpListener> QueryByName(string name)
        {
            TcpListener listener = null;

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
                    CommandText = string.Format(@"select {0} from TcpListener where [name]=@name", StoreHelper.GetTcpListenerSelectFields(string.Empty))
                })
                {

                    var parameter = new SqlParameter("@name", SqlDbType.NVarChar,150)
                    {
                        Value = name
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    int reply = 3;
                    SqlDataReader reader = null;
                    while (true)
                    {
                        if (reader != null && !reader.IsClosed)
                        {
                            try
                            {
                                await reader.CloseAsync();
                            }
                            catch
                            {

                            }
                        }

                        try
                        {
                            reader = await commond.ExecuteReaderAsync();
                            break;
                        }
                        catch (SqlException ex)
                        {

                            if (reply > 0 && (ex.Number == 41302 || ex.Number == 41305 || ex.Number == 41325 || ex.Number == 41301 || ex.Number == 1205))
                            {
                                reply--;
                                System.Threading.Thread.Sleep(1);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }

                    await using (reader)
                    {
                        if (await reader.ReadAsync())
                        {
                            listener = new TcpListener();
                            StoreHelper.SetTcpListenerSelectFields(listener, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return listener;
        }

        public async Task<QueryResult<TcpListener>> QueryByName(string name, int page, int pageSize)
        {
            QueryResult<TcpListener> result = new QueryResult<TcpListener>();

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
		                           select @count= count(*) from TcpListener where [name] like @name

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
	
                                    select {0} from TcpListener where [name] like @name  
                                    order by [sequence]
		                            offset (@pagesize * (@currentpage - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetTcpListenerSelectFields(string.Empty))
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

                    int reply = 3;
                    SqlDataReader reader = null;
                    while (true)
                    {
                        if (reader != null && !reader.IsClosed)
                        {
                            try
                            {
                                await reader.CloseAsync();
                            }
                            catch
                            {

                            }
                        }

                        try
                        {
                            reader = await commond.ExecuteReaderAsync();
                            break;
                        }
                        catch (SqlException ex)
                        {

                            if (reply > 0 && (ex.Number == 41302 || ex.Number == 41305 || ex.Number == 41325 || ex.Number == 41301 || ex.Number == 1205))
                            {
                                reply--;
                                System.Threading.Thread.Sleep(1);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }

                    await using (reader)
                    {
                        while (await reader.ReadAsync())
                        {
                            var listener = new TcpListener();
                            StoreHelper.SetTcpListenerSelectFields(listener, reader, string.Empty);
                            result.Results.Add(listener);
                        }

                        await reader.CloseAsync();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;

                    }
                }
            });

            return result;
        }



        public async Task Update(TcpListener listener)
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
                     Transaction=sqlTran,
                    CommandText = @"update TcpListener set [name]=@name,[port]=@port,[keepalive]=@keepalive,[maxconcurrencycount]=@maxconcurrencycount,[maxbuffercount]=@maxbuffercount,[executedatafactorytype]=@executedatafactorytype,[executedatafactorytypeusedi]=@executedatafactorytypeusedi,[heartbeatsenddata]=@heartbeatsenddata,[description]=@description,[modifytime]=getutcdate()
                                    where [id]=@id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = listener.ID
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 150)
                    {
                        Value = listener.Name
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@port", SqlDbType.Int)
                    {
                        Value = listener.Port
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@keepalive", SqlDbType.Bit)
                    {
                        Value = listener.KeepAlive
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@maxconcurrencycount", SqlDbType.Int)
                    {
                        Value = listener.MaxConcurrencyCount
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@maxbuffercount", SqlDbType.Int)
                    {
                        Value = listener.MaxBufferCount
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@executedatafactorytype", SqlDbType.NVarChar, 500)
                    {
                        Value = listener.ExecuteDataFactoryType
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@executedatafactorytypeusedi", SqlDbType.Bit)
                    {
                        Value = listener.ExecuteDataFactoryTypeUseDI
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@heartbeatsenddata", SqlDbType.NVarChar, 500)
                    {
                        Value = listener.HeartBeatSendData
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@description", SqlDbType.NVarChar, 1000)
                    {
                        Value = listener.Description
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    int reply = 3;
                    while (true)
                    {
                        try
                        {
                            await commond.ExecuteNonQueryAsync();
                            break;
                        }
                        catch (SqlException ex)
                        {

                            if (reply > 0 && (ex.Number == 41302 || ex.Number == 41305 || ex.Number == 41325 || ex.Number == 41301 || ex.Number == 1205))
                            {
                                reply--;
                                System.Threading.Thread.Sleep(1);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                }
            });
        }




    }
}
