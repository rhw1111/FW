using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using MSLibrary.DI;
using MSLibrary.Transaction;

namespace MSLibrary.MessageQueue.DAL.SMessageStores
{
    /// <summary>
    /// 基于SQLServer实现的消息数据操作
    /// 
    /// </summary>
    [Injection(InterfaceType = typeof(SMessageStoreForSQLDB), Scope = InjectionScope.Singleton)]
    public class SMessageStoreForSQLDB : ISMessageStore
    {
        private const string _queueName = "Queue";

        private IMessageQueueConnectionFactory _messageQueueConnectionFactory;

        public SMessageStoreForSQLDB(IMessageQueueConnectionFactory messageQueueConnectionFactory)
        {
            _messageQueueConnectionFactory = messageQueueConnectionFactory;
        }

        public async Task Add(SQueue queue, SMessage message)
        {
            if (queue.IsDead)
            {
                throw new Exception(string.Format("SQueue {0}.{1} isdead, can't be used in SMessageStoreForSQLDB.Add", queue.GroupName, queue.Name));
            }

            //根据存储类型和服务器名称获取连接字符串
            var strConn = _messageQueueConnectionFactory.CreateAllForMessageQueue(queue.StoreType, queue.ServerName);
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, strConn, async (conn, transaction) =>
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
                    //Queue.Name为要存储消息的表名称

                    if (message.ID == Guid.Empty)
                    {
                        commond.CommandText = string.Format(@"insert into {0}([id],[key],[type],[data],[typelistenerid],[originalmessageid],[delaymessageid],[extensionmessage],[createtime],[expectationexecutetime],[lastexecutetime],[retrynumber],[exceptionmessage],[isdead])
                                    values(default,@key,@type,@data,@typelistenerid,@originalmessageid,@delaymessageid,@extensionmessage,getutcdate(),@expectationexecutetime,null,0,null,0);
                                    select @newid=[id] from {0} where [sequence]=SCOPE_IDENTITY()", queue.Name);
                    }
                    else
                    {
                        commond.CommandText = string.Format(@"insert into {0}([id],[key],[type],[data],[typelistenerid],[originalmessageid],[delaymessageid],[extensionmessage],[createtime],[expectationexecutetime],[lastexecutetime],[retrynumber],[exceptionmessage],[isdead])
                                    values(@id,@key,@type,@data,@typelistenerid,@originalmessageid,@delaymessageid,@extensionmessage,getutcdate(),@expectationexecutetime,null,0,null,0)", queue.Name);
                    }

                    SqlParameter parameter;
                    if (message.ID != Guid.Empty)
                    {
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = message.ID
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

                    parameter = new SqlParameter("@key", SqlDbType.VarChar, 150)
                    {
                        Value = message.Key
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@type", SqlDbType.VarChar, 150)
                    {
                        Value = message.Type
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@data", SqlDbType.NVarChar, message.Data.Length)
                    {
                        Value = message.Data
                    };
                    commond.Parameters.Add(parameter);

                    if (message.TypeListenerID.HasValue)
                    {
                        parameter = new SqlParameter("@typelistenerid", SqlDbType.UniqueIdentifier)
                        {
                            Value = message.TypeListenerID
                        };
                    }
                    else
                    {
                        parameter = new SqlParameter("@typelistenerid", SqlDbType.UniqueIdentifier)
                        {
                            Value = DBNull.Value
                        };
                    }
                    commond.Parameters.Add(parameter);


                    if (message.OriginalMessageID.HasValue)
                    {
                        parameter = new SqlParameter("@originalmessageid", SqlDbType.UniqueIdentifier)
                        {
                            Value = message.OriginalMessageID
                        };
                    }
                    else
                    {
                        parameter = new SqlParameter("@originalmessageid", SqlDbType.UniqueIdentifier)
                        {
                            Value = DBNull.Value
                        };
                    }
                    commond.Parameters.Add(parameter);

                    if (message.DelayMessageID.HasValue)
                    {
                        parameter = new SqlParameter("@delaymessageid", SqlDbType.UniqueIdentifier)
                        {
                            Value = message.DelayMessageID
                        };
                    }
                    else
                    {
                        parameter = new SqlParameter("@delaymessageid", SqlDbType.UniqueIdentifier)
                        {
                            Value = DBNull.Value
                        };
                    }
                    commond.Parameters.Add(parameter);



                    if (message.ExtensionMessage != null)
                    {
                        parameter = new SqlParameter("@extensionmessage", SqlDbType.NVarChar, message.ExtensionMessage.Length)
                        {
                            Value = message.ExtensionMessage
                        };
                    }
                    else
                    {
                        parameter = new SqlParameter("@extensionmessage", SqlDbType.NVarChar, 10)
                        {
                            Value = DBNull.Value
                        };
                    }
                    commond.Parameters.Add(parameter);




                    parameter = new SqlParameter("@expectationexecutetime", SqlDbType.DateTime)
                    {
                        Value = message.ExpectationExecuteTime
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();

                    if (message.ID == Guid.Empty)
                    {
                        message.ID = (Guid)commond.Parameters["@newid"].Value;
                    }
                }
            });


        }

        public async Task AddRetry(SQueue queue, Guid id, string exceptionMessage)
        {
            //根据存储类型和服务器名称获取连接字符串
            var strConn = _messageQueueConnectionFactory.CreateAllForMessageQueue(queue.StoreType, queue.ServerName);
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, strConn, async (conn, transaction) =>
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
                    CommandText = string.Format(@"update {0} set [retrynumber]=case when [retrynumber]>=10000 then 10000 else [retrynumber]+1 end,[exceptionmessage]=@exceptionmessage where [id]=@id", queue.Name)
                })
                {


                    SqlParameter parameter;

                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@exceptionmessage", SqlDbType.NVarChar, exceptionMessage.Length)
                    {
                        Value = exceptionMessage
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();
                    await commond.ExecuteNonQueryAsync();
                }
            });

        }

        public async Task AddToDead(SQueue queue, SMessage message)
        {
            if (!queue.IsDead)
            {
                throw new Exception(string.Format("SQueue {0}.{1} is not dead, can't be used in SMessageStoreForSQLDB.AddToDead", queue.GroupName, queue.Name));
            }

            //根据存储类型和服务器名称获取连接字符串
            var strConn = _messageQueueConnectionFactory.CreateAllForMessageQueue(queue.StoreType, queue.ServerName);
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, strConn, async (conn, transaction) =>
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
                    //Queue.Name为要存储消息的表名称

                    if (message.ID == Guid.Empty)
                    {
                        commond.CommandText = string.Format(@"insert into {0}([id],[key],[type],[data],[typelistenerid],[originalmessageid],[extensionmessage],[createtime],[expectationexecutetime],[lastexecutetime],[retrynumber],[exceptionMessage],[isdead])
                                    values(default,@key,@type,@data,@typelistenerid,@originalmessageid,@extensionmessage,getutcdate(),@expectationexecutetime,null,0,@exceptionmessage,1);
                                    select @newid=[id] from {0} where [sequence]=SCOPE_IDENTITY()", queue.Name);
                    }
                    else
                    {
                        commond.CommandText = string.Format(@"insert into {0}([id],[key],[type],[data],[typelistenerid],[originalmessageid],[extensionmessage],[createtime],[expectationexecutetime],[lastexecutetime],[retrynumber],[exceptionMessage],[isdead])
                                    values(@id,@key,@type,@data,@typelistenerid,@originalmessageid,@extensionmessage,getutcdate(),@expectationexecutetime,null,0,@exceptionmessage,1)", queue.Name);
                    }

                    SqlParameter parameter;
                    if (message.ID != Guid.Empty)
                    {
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = message.ID
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

                    parameter = new SqlParameter("@key", SqlDbType.VarChar, 150)
                    {
                        Value = message.Key
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@type", SqlDbType.VarChar, 150)
                    {
                        Value = message.Type
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@data", SqlDbType.NVarChar, message.Data.Length)
                    {
                        Value = message.Data
                    };
                    commond.Parameters.Add(parameter);

                    if (message.TypeListenerID.HasValue)
                    {
                        parameter = new SqlParameter("@typelistenerid", SqlDbType.UniqueIdentifier)
                        {
                            Value = message.TypeListenerID
                        };
                    }
                    else
                    {
                        parameter = new SqlParameter("@typelistenerid", SqlDbType.UniqueIdentifier)
                        {
                            Value = DBNull.Value
                        };
                    }
                    commond.Parameters.Add(parameter);


                    if (message.OriginalMessageID.HasValue)
                    {
                        parameter = new SqlParameter("@originalmessageid", SqlDbType.UniqueIdentifier)
                        {
                            Value = message.OriginalMessageID
                        };
                    }
                    else
                    {
                        parameter = new SqlParameter("@originalmessageid", SqlDbType.UniqueIdentifier)
                        {
                            Value = DBNull.Value
                        };
                    }
                    commond.Parameters.Add(parameter);


                    if (message.ExtensionMessage != null)
                    {
                        parameter = new SqlParameter("@extensionmessage", SqlDbType.NVarChar, message.ExtensionMessage.Length)
                        {
                            Value = message.ExtensionMessage
                        };
                    }
                    else
                    {
                        parameter = new SqlParameter("@extensionmessage", SqlDbType.NVarChar, 10)
                        {
                            Value = DBNull.Value
                        };
                    }
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@expectationexecutetime", SqlDbType.DateTime)
                    {
                        Value = message.ExpectationExecuteTime
                    };
                    commond.Parameters.Add(parameter);

                    if (message.ExceptionMessage != null)
                    {
                        parameter = new SqlParameter("@exceptionmessage", SqlDbType.NVarChar, message.ExceptionMessage.Length)
                        {
                            Value = message.ExceptionMessage
                        };
                    }
                    else
                    {
                        parameter = new SqlParameter("@exceptionmessage", SqlDbType.NVarChar, 3000)
                        {
                            Value = DBNull.Value
                        };
                    }
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();


                    if (message.ID == Guid.Empty)
                    {
                        message.ID = (Guid)commond.Parameters["@newid"].Value;
                    }
                }
            });

        }

        public async Task Delete(SQueue queue, Guid id)
        {
            //根据存储类型和服务器名称获取连接字符串
            var strConn = _messageQueueConnectionFactory.CreateAllForMessageQueue(queue.StoreType, queue.ServerName);
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, strConn, async (conn, transaction) =>
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
                    CommandText = string.Format(@"delete from {0} where [id]=@id", queue.Name)
                })
                {


                    SqlParameter parameter;

                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();
                }
            });

        }

        public async Task QueryAllByQueue(SQueue queue, int pageSize, Func<List<SMessage>, Task<bool>> callBack)
        {
            //根据存储类型和服务器名称获取连接字符串
            var strConn = _messageQueueConnectionFactory.CreateReadForMessageQueue(queue.StoreType, queue.ServerName);

            List<SMessage> messageList = new List<SMessage>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, strConn, async (conn, transaction) =>
            {
                int page = 0;

                while (true)
                {
                    page++;
                    messageList.Clear();

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
                        CommandText = string.Format(@"select {0} from {1} as m
                                                      join
                                                        (
                                                        select sequence from {1} order by sequence
                                                        offset ( @pagesize * (@page - 1 )) rows 
                                                        fetch next @pagesize rows only
                                                        ) as t
                                                      on m.sequence=t.sequence
                                                      order by m.sequence", StoreHelper.GetSMessageSelectFields("m"), queue.Name)
                    })
                    {


                        var parameter = new SqlParameter("@pagesize", SqlDbType.Int)
                        {
                            Value = pageSize
                        };
                        commond.Parameters.Add(parameter);

                        parameter = new SqlParameter("@page", SqlDbType.Int)
                        {
                            Value = page
                        };
                        commond.Parameters.Add(parameter);


                        await commond.PrepareAsync();

                        SqlDataReader reader = null;

                        await using (reader = await commond.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var message = new SMessage();
                                StoreHelper.SetSMessageSelectFields(message, reader, "m");
                                message.Extensions[_queueName] = queue;
                                messageList.Add(message);
                            }

                            await reader.CloseAsync();
                        }


                    }


                    var callbackResult = await callBack(messageList);


                    if (messageList.Count != pageSize || !callbackResult)
                    {
                        break;
                    }

                }

            });

        }

        public async Task<SMessage> QueryByDelayID(SQueue queue, Guid delayMessageID)
        {
            SMessage message = null;
            //根据存储类型和服务器名称获取连接字符串
            var strConn = _messageQueueConnectionFactory.CreateReadForMessageQueue(queue.StoreType, queue.ServerName);

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, strConn, async (conn, transaction) =>
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
                    CommandText = string.Format(@"select top 1 {0} from {1} where [delaymessageid]=@delaymessageid ", StoreHelper.GetSMessageSelectFields(string.Empty), queue.Name)
                })
                {


                    var parameter = new SqlParameter("@delaymessageid", SqlDbType.UniqueIdentifier)
                    {
                        Value = delayMessageID
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();


                    SqlDataReader reader = null;

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            message = new SMessage();
                            StoreHelper.SetSMessageSelectFields(message, reader, string.Empty);
                            message.Extensions[_queueName] = queue;
                        }

                        await reader.CloseAsync();
                    }


                }

            });

            return message;
        }

        public async Task<SMessage> QueryByKeyAndBeforeExpectTime(SQueue queue, string key, DateTime expectTime)
        {
            SMessage message = null;
            //根据存储类型和服务器名称获取连接字符串
            var strConn = _messageQueueConnectionFactory.CreateReadForMessageQueue(queue.StoreType, queue.ServerName);

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, strConn, async (conn, transaction) =>
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
                    CommandText = string.Format(@"select top 1 {0} from {1} where [key]=@key and [expectationexecutetime]<@expectationexecutetime ", StoreHelper.GetSMessageSelectFields(string.Empty), queue.Name)
                })
                {


                    var parameter = new SqlParameter("@key", SqlDbType.VarChar, 150)
                    {
                        Value = key
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@expectationexecutetime", SqlDbType.DateTime)
                    {
                        Value = expectTime
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();


                    SqlDataReader reader = null;

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            message = new SMessage();
                            StoreHelper.SetSMessageSelectFields(message, reader, string.Empty);
                            message.Extensions[_queueName] = queue;
                        }

                        await reader.CloseAsync();
                    }


                }

            });

            return message;
        }

        public async Task<SMessage> QueryByOriginalID(SQueue queue, Guid originalMessageID, Guid listenerID)
        {
            SMessage message = null;
            //根据存储类型和服务器名称获取连接字符串
            var strConn = _messageQueueConnectionFactory.CreateReadForMessageQueue(queue.StoreType, queue.ServerName);

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, strConn, async (conn, transaction) =>
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
                    CommandText = string.Format(@"select top 1 {0} from {1} where [originalmessageid]=@originalmessageid and [listenerid]=@listenerid ", StoreHelper.GetSMessageSelectFields(string.Empty), queue.Name)
                })
                {


                    var parameter = new SqlParameter("@originalmessageid", SqlDbType.UniqueIdentifier)
                    {
                        Value = originalMessageID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@listenerid", SqlDbType.UniqueIdentifier)
                    {
                        Value = listenerID
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();


                    SqlDataReader reader = null;

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            message = new SMessage();
                            StoreHelper.SetSMessageSelectFields(message, reader, string.Empty);
                            message.Extensions[_queueName] = queue;
                        }

                        await reader.CloseAsync();
                    }


                }

            });

            return message;
        }

        public async Task<QueryResult<SMessage>> QueryByQueue(SQueue queue, int page, int pageSize)
        {
            QueryResult<SMessage> result = new QueryResult<SMessage>();

            //根据存储类型和服务器名称获取连接字符串
            var strConn = _messageQueueConnectionFactory.CreateReadForMessageQueue(queue.StoreType, queue.ServerName);

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, strConn, async (conn, transaction) =>
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
		                           select @count= count(*) from {1}
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
	
                                    select {0} from {1} as m
                                    join
                                    (
                                        select sequence from {1} order by sequence
                                        offset ( @pagesize * (@page - 1 )) rows 
                                        fetch next @pagesize rows only
                                    ) as t
                                    on m.sequence=t.sequence
                                    order by m.sequence;", StoreHelper.GetSMessageSelectFields("m"), queue.Name)
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
                            var message = new SMessage();
                            StoreHelper.SetSMessageSelectFields(message, reader, "m");
                            message.Extensions[_queueName] = queue;
                            result.Results.Add(message);
                        }

                        await reader.CloseAsync();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;


                    }
                }
            });

            return result;
        }

        public async Task UpdateLastExecuteTime(SQueue queue, Guid id)
        {
            //根据存储类型和服务器名称获取连接字符串
            var strConn = _messageQueueConnectionFactory.CreateAllForMessageQueue(queue.StoreType, queue.ServerName);
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, strConn, async (conn, transaction) =>
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
                    CommandText = string.Format(@"update {0} set [lastexecutetime]=getutcdate() where [id]=@id", queue.Name)
                })
                {
                    SqlParameter parameter;

                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();

                }
            });

        }
    }
}
