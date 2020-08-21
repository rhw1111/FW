using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using MSLibrary.DI;
using MSLibrary.Transaction;
using MSLibrary.MessageQueue.DAL;
using MSLibrary.MessageQueue;

namespace MSLibrary.MySqlStore.MessageQueue.DAL
{
    [Injection(InterfaceType = typeof(SMessageStoreForMySql), Scope = InjectionScope.Singleton)]
    public class SMessageStoreForMySql : ISMessageStore
    {
        private const string _queueName = "Queue";

        private IMessageQueueConnectionFactory _messageQueueConnectionFactory;

        public SMessageStoreForMySql(IMessageQueueConnectionFactory messageQueueConnectionFactory)
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
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, strConn, async (conn, transaction) =>
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
                    //Queue.Name为要存储消息的表名称

                    if (message.ID == Guid.Empty)
                    {
                        message.ID = Guid.NewGuid();
                    }

                        commond.CommandText = string.Format(@"insert into {0}(id,key,type,data,typelistenerid,originalmessageid,delaymessageid,extensionmessage,createtime,expectationexecutetime,lastexecutetime,retrynumber,exceptionmessage,isdead)
                                    values(@id,@key,@type,@data,@typelistenerid,@originalmessageid,@delaymessageid,@extensionmessage,utc_timestamp(),@expectationexecutetime,null,0,null,0)", queue.Name);
                    

                    MySqlParameter parameter;

                        parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                        {
                            Value = message.ID
                        };
                        commond.Parameters.Add(parameter);


                    parameter = new MySqlParameter("@key", MySqlDbType.VarChar, 150)
                    {
                        Value = message.Key
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@type", MySqlDbType.VarChar, 150)
                    {
                        Value = message.Type
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@data", MySqlDbType.MediumText, message.Data.Length)
                    {
                        Value = message.Data
                    };
                    commond.Parameters.Add(parameter);

                    if (message.TypeListenerID.HasValue)
                    {
                        parameter = new MySqlParameter("@typelistenerid", MySqlDbType.Guid)
                        {
                            Value = message.TypeListenerID
                        };
                    }
                    else
                    {
                        parameter = new MySqlParameter("@typelistenerid", MySqlDbType.Guid)
                        {
                            Value = DBNull.Value
                        };
                    }
                    commond.Parameters.Add(parameter);


                    if (message.OriginalMessageID.HasValue)
                    {
                        parameter = new MySqlParameter("@originalmessageid", MySqlDbType.Guid)
                        {
                            Value = message.OriginalMessageID
                        };
                    }
                    else
                    {
                        parameter = new MySqlParameter("@originalmessageid", MySqlDbType.Guid)
                        {
                            Value = DBNull.Value
                        };
                    }
                    commond.Parameters.Add(parameter);

                    if (message.DelayMessageID.HasValue)
                    {
                        parameter = new MySqlParameter("@delaymessageid", MySqlDbType.Guid)
                        {
                            Value = message.DelayMessageID
                        };
                    }
                    else
                    {
                        parameter = new MySqlParameter("@delaymessageid", MySqlDbType.Guid)
                        {
                            Value = DBNull.Value
                        };
                    }
                    commond.Parameters.Add(parameter);



                    if (message.ExtensionMessage != null)
                    {
                        parameter = new MySqlParameter("@extensionmessage", MySqlDbType.MediumText, message.ExtensionMessage.Length)
                        {
                            Value = message.ExtensionMessage
                        };
                    }
                    else
                    {
                        parameter = new MySqlParameter("@extensionmessage", MySqlDbType.MediumText, 10)
                        {
                            Value = DBNull.Value
                        };
                    }
                    commond.Parameters.Add(parameter);




                    parameter = new MySqlParameter("@expectationexecutetime", MySqlDbType.DateTime)
                    {
                        Value = message.ExpectationExecuteTime
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();

                }
            });

        }

        public async Task AddRetry(SQueue queue, Guid id, string exceptionMessage)
        {
            //根据存储类型和服务器名称获取连接字符串
            var strConn = _messageQueueConnectionFactory.CreateAllForMessageQueue(queue.StoreType, queue.ServerName);
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, strConn, async (conn, transaction) =>
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
                    CommandText = string.Format(@"update {0} set retrynumber=(case when retrynumber>=10000 then 10000 else retrynumber+1 end),exceptionmessage=@exceptionmessage where id=@id", queue.Name)
                })
                {


                    MySqlParameter parameter;

                    parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@exceptionmessage", MySqlDbType.MediumText, exceptionMessage.Length)
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
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, strConn, async (conn, transaction) =>
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
                    //Queue.Name为要存储消息的表名称

                    if (message.ID == Guid.Empty)
                    {
                        message.ID = Guid.NewGuid();
                    }

                        commond.CommandText = string.Format(@"insert into {0}(id,key,type,data,typelistenerid,originalmessageid,extensionmessage,createtime,expectationexecutetime,lastexecutetime,retrynumber,exceptionMessage,isdead)
                                    values(@id,@key,@type,@data,@typelistenerid,@originalmessageid,@extensionmessage,utc_timestamp(),@expectationexecutetime,null,0,@exceptionmessage,1)", queue.Name);
                    

                    MySqlParameter parameter;

                        parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                        {
                            Value = message.ID
                        };
                        commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@key", MySqlDbType.VarChar, 150)
                    {
                        Value = message.Key
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@type", MySqlDbType.VarChar, 150)
                    {
                        Value = message.Type
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@data", MySqlDbType.MediumText, message.Data.Length)
                    {
                        Value = message.Data
                    };
                    commond.Parameters.Add(parameter);

                    if (message.TypeListenerID.HasValue)
                    {
                        parameter = new MySqlParameter("@typelistenerid", MySqlDbType.Guid)
                        {
                            Value = message.TypeListenerID
                        };
                    }
                    else
                    {
                        parameter = new MySqlParameter("@typelistenerid", MySqlDbType.Guid)
                        {
                            Value = DBNull.Value
                        };
                    }
                    commond.Parameters.Add(parameter);


                    if (message.OriginalMessageID.HasValue)
                    {
                        parameter = new MySqlParameter("@originalmessageid", MySqlDbType.Guid)
                        {
                            Value = message.OriginalMessageID
                        };
                    }
                    else
                    {
                        parameter = new MySqlParameter("@originalmessageid", MySqlDbType.Guid)
                        {
                            Value = DBNull.Value
                        };
                    }
                    commond.Parameters.Add(parameter);


                    if (message.ExtensionMessage != null)
                    {
                        parameter = new MySqlParameter("@extensionmessage", MySqlDbType.MediumText, message.ExtensionMessage.Length)
                        {
                            Value = message.ExtensionMessage
                        };
                    }
                    else
                    {
                        parameter = new MySqlParameter("@extensionmessage", MySqlDbType.MediumText, 10)
                        {
                            Value = DBNull.Value
                        };
                    }
                    commond.Parameters.Add(parameter);


                    parameter = new MySqlParameter("@expectationexecutetime", MySqlDbType.DateTime)
                    {
                        Value = message.ExpectationExecuteTime
                    };
                    commond.Parameters.Add(parameter);

                    if (message.ExceptionMessage != null)
                    {
                        parameter = new MySqlParameter("@exceptionmessage", MySqlDbType.MediumText, message.ExceptionMessage.Length)
                        {
                            Value = message.ExceptionMessage
                        };
                    }
                    else
                    {
                        parameter = new MySqlParameter("@exceptionmessage", MySqlDbType.MediumText, 3000)
                        {
                            Value = DBNull.Value
                        };
                    }
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();
                }
            });

        }

        public async Task Delete(SQueue queue, Guid id)
        {
            //根据存储类型和服务器名称获取连接字符串
            var strConn = _messageQueueConnectionFactory.CreateAllForMessageQueue(queue.StoreType, queue.ServerName);
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, strConn, async (conn, transaction) =>
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
                    CommandText = string.Format(@"delete from {0} where id=@id", queue.Name)
                })
                {


                    MySqlParameter parameter;

                    parameter = new MySqlParameter("@id", MySqlDbType.Guid)
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

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, strConn, async (conn, transaction) =>
            {
                int page = 0;

                while (true)
                {
                    page++;
                    messageList.Clear();

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
                        CommandText = string.Format(@"select {0} from {1} as m
                                                      join
                                                        select t.sequence as sequence from
                                                        (
                                                            (
                                                                select sequence from {1} order by sequence
                                                                limit {2}, {3} 
                                                            ) as t
                                                        ) as n
                                                      on m.sequence=n.sequence
                                                      order by m.sequence", StoreHelper.GetSMessageSelectFields("m"), queue.Name, (page - 1) * pageSize, pageSize)
                    })
                    {




                        await commond.PrepareAsync();

                        DbDataReader reader = null;

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

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, strConn, async (conn, transaction) =>
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
                    CommandText = string.Format(@"select {0} from {1} where delaymessageid=@delaymessageid limit 1", StoreHelper.GetSMessageSelectFields(string.Empty), queue.Name)
                })
                {


                    var parameter = new MySqlParameter("@delaymessageid", MySqlDbType.Guid)
                    {
                        Value = delayMessageID
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();


                    DbDataReader reader = null;

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

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, strConn, async (conn, transaction) =>
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
                    CommandText = string.Format(@"select {0} from {1} where key=@key and expectationexecutetime<@expectationexecutetime limit 1", StoreHelper.GetSMessageSelectFields(string.Empty), queue.Name)
                })
                {


                    var parameter = new MySqlParameter("@key", MySqlDbType.VarChar, 150)
                    {
                        Value = key
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@expectationexecutetime", MySqlDbType.DateTime)
                    {
                        Value = expectTime
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();


                    DbDataReader reader = null;

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

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, strConn, async (conn, transaction) =>
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
                    CommandText = string.Format(@"select  {0} from {1} where originalmessageid=@originalmessageid and listenerid=@listenerid limit 1", StoreHelper.GetSMessageSelectFields(string.Empty), queue.Name)
                })
                {


                    var parameter = new MySqlParameter("@originalmessageid", MySqlDbType.Guid)
                    {
                        Value = originalMessageID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@listenerid", MySqlDbType.Guid)
                    {
                        Value = listenerID
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();


                    DbDataReader reader = null;

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

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, strConn, async (conn, transaction) =>
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
                    CommandText = string.Format(@"select count(*) from {1}
	
                                    select {0} from {1}                                    
                                    where sequence in
                                                (
                                                select t.sequence from
                                                (
                                                    select sequence from {1}
                                                    order by sequence limit {2}, {3}                                                   
                                                ) as t
                                                )                                  
                                    order by sequence;", StoreHelper.GetSMessageSelectFields("m"), queue.Name, (page - 1) * pageSize, pageSize)
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
                                    var message = new SMessage();
                                    StoreHelper.SetSMessageSelectFields(message, reader, "m");
                                    message.Extensions[_queueName] = queue;
                                    result.Results.Add(message);
                                }
                            }
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return result;
        }

        public async Task UpdateLastExecuteTime(SQueue queue, Guid id)
        {
            //根据存储类型和服务器名称获取连接字符串
            var strConn = _messageQueueConnectionFactory.CreateAllForMessageQueue(queue.StoreType, queue.ServerName);
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, strConn, async (conn, transaction) =>
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
                    CommandText = string.Format(@"update {0} set lastexecutetime=utc_timestamp() where id=@id", queue.Name)
                })
                {
                    MySqlParameter parameter;

                    parameter = new MySqlParameter("@id", MySqlDbType.Guid)
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
