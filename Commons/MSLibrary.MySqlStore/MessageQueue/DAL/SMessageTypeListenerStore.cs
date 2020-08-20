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
    [Injection(InterfaceType = typeof(ISMessageTypeListenerStore), Scope = InjectionScope.Singleton)]
    public class SMessageTypeListenerStore : ISMessageTypeListenerStore
    {
        private IMessageQueueConnectionFactory _messageQueueConnectionFactory;

        public SMessageTypeListenerStore(IMessageQueueConnectionFactory messageQueueConnectionFactory)
        {
            _messageQueueConnectionFactory = messageQueueConnectionFactory;
        }

        public async Task Add(SMessageTypeListener listener)
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
                    if (listener.ID == Guid.Empty)
                    {
                        listener.ID = Guid.NewGuid();
                    }

                        commond.CommandText = @"insert into smessagetypelistener(id,messagetypeid,name,queuegroupname,mode,listenerfactorytype,listenerfactorytypeusedi,listenerweburl,listenerwebsignature,createtime,modifytime)
                                    values(@id,@messagetypeid,@name,@queuegroupname,@mode,@listenerfactorytype,@listenerfactorytypeusedi,@listenerweburl,@listenerwebsignature,utc_timestamp(),utc_timestamp())";
                    

                    MySqlParameter parameter;

                    parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = listener.ID
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new MySqlParameter("@messagetypeid", MySqlDbType.Guid)
                    {
                        Value = listener.MessageType.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 150)
                    {
                        Value = listener.Name
                    };
                    commond.Parameters.Add(parameter);

                    if (listener.QueueGroupName != null)
                    {
                        parameter = new MySqlParameter("@queuegroupname", MySqlDbType.VarChar, 150)
                        {
                            Value = listener.QueueGroupName
                        };
                    }
                    else
                    {
                        parameter = new MySqlParameter("@queuegroupname", MySqlDbType.VarChar, 150)
                        {
                            Value = DBNull.Value
                        };
                    }
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@mode", MySqlDbType.Int32)
                    {
                        Value = listener.Mode
                    };
                    commond.Parameters.Add(parameter);

                    if (listener.ListenerFactoryType != null)
                    {
                        parameter = new MySqlParameter("@listenerfactorytype", MySqlDbType.VarChar, 150)
                        {
                            Value = listener.ListenerFactoryType
                        };
                        commond.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new MySqlParameter("@listenerfactorytype", MySqlDbType.VarChar, 150)
                        {
                            Value = DBNull.Value
                        };
                        commond.Parameters.Add(parameter);
                    }

                    if (listener.ListenerFactoryTypeUseDI != null)
                    {
                        parameter = new MySqlParameter("@listenerfactorytypeusedi", MySqlDbType.Bit)
                        {
                            Value = listener.ListenerFactoryTypeUseDI
                        };
                        commond.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new MySqlParameter("@listenerfactorytypeusedi", MySqlDbType.Bit)
                        {
                            Value = DBNull.Value
                        };
                        commond.Parameters.Add(parameter);
                    }

                    if (listener.ListenerWebUrl != null)
                    {
                        parameter = new MySqlParameter("@listenerweburl", MySqlDbType.VarChar, 200)
                        {
                            Value = listener.ListenerWebUrl
                        };
                        commond.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new MySqlParameter("@listenerweburl", MySqlDbType.VarChar, 200)
                        {
                            Value = DBNull.Value
                        };
                        commond.Parameters.Add(parameter);
                    }


                    if (listener.ListenerWebSignature != null)
                    {
                        parameter = new MySqlParameter("@listenerwebsignature", MySqlDbType.VarChar, 150)
                        {
                            Value = listener.ListenerWebSignature
                        };
                        commond.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new MySqlParameter("@listenerwebsignature", MySqlDbType.VarChar, 150)
                        {
                            Value = DBNull.Value
                        };
                        commond.Parameters.Add(parameter);
                    }

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
                    CommandText = @"delete from smessagetypelistener where [id]=@id"
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

        public async Task DeleteByTypeRelation(Guid typeId, Guid listenerId)
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
                    CommandText = @"delete from smessagetypelistener where [id]=@id and [messagetypeid]=@messagetypeid"
                })
                {

                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = listenerId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@messagetypeid", MySqlDbType.Guid)
                    {
                        Value = typeId
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();


                    await commond.ExecuteNonQueryAsync();


                }
            });
        }

        public async Task<SMessageTypeListener> QueryById(Guid id)
        {
            SMessageTypeListener listener = null;

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
                    CommandText = string.Format(@"select {0},{1} from smessagetypelistener as listener join smessageexecutetype as mtype on listener.messagetypeid=mtype.id where listener.[id]=@id", StoreHelper.GetSMessageTypeListenerSelectFields("listener"), StoreHelper.GetSMessageExecuteTypeSelectFields("mtype"))
                })
                {

                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    DbDataReader reader = null;

                    reader = await commond.ExecuteReaderAsync();

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            listener = new SMessageTypeListener();
                            StoreHelper.SetSMessageTypeListenerSelectFields(listener, reader, "listener");
                            listener.MessageType = new SMessageExecuteType();
                            StoreHelper.SetSMessageExecuteTypeSelectFields(listener.MessageType, reader, "mtype");
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return listener;
        }

        public async Task QueryByType(Guid typeId, Func<SMessageTypeListener, Task> callback)
        {
            List<SMessageTypeListener> listenerList = new List<SMessageTypeListener>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _messageQueueConnectionFactory.CreateReadForMessageQueueMain(), async (conn, transaction) =>
            {
                Int64? sequence = null;
                int pageSize = 500;

                while (true)
                {
                    listenerList.Clear();

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
                            commond.CommandText = string.Format(@"select {0},{1} from smessagetypelistener as listener join smessageexecutetype as mtype on listener.messagetypeid=mtype.id where mtype.id=@typeid order by listener.sequence limit {2}", StoreHelper.GetSMessageTypeListenerSelectFields("listener"), StoreHelper.GetSMessageExecuteTypeSelectFields("mtype"),pageSize);
                        }
                        else
                        {
                            commond.CommandText = string.Format(@"select  {0},{1} from smessagetypelistener as listener join smessageexecutetype as mtype on listener.messagetypeid=mtype.id where mtype.id=@typeid and listener.sequence>@sequence order by listener.sequence limit {2}", StoreHelper.GetSMessageTypeListenerSelectFields("listener"), StoreHelper.GetSMessageExecuteTypeSelectFields("mtype"), pageSize);
                        }

                        var parameter = new MySqlParameter("@typeid", MySqlDbType.Guid)
                        {
                            Value = typeId
                        };
                        commond.Parameters.Add(parameter);

                        parameter = new MySqlParameter("@pagesize", MySqlDbType.Guid)
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
                                var listener = new SMessageTypeListener();
                                StoreHelper.SetSMessageTypeListenerSelectFields(listener, reader, "listener");
                                sequence = (Int64)reader["listenersequence"];
                                listener.MessageType = new SMessageExecuteType();
                                StoreHelper.SetSMessageExecuteTypeSelectFields(listener.MessageType, reader, "mtype");
                                listenerList.Add(listener);
                            }

                            await reader.CloseAsync();
                        }


                    }

                    foreach (var listenerItem in listenerList)
                    {
                        await callback(listenerItem);
                    }

                    if (listenerList.Count != pageSize)
                    {
                        break;
                    }

                }

            });
        }

        public async Task<QueryResult<SMessageTypeListener>> QueryByType(Guid typeId, int page, int pageSize)
        {
            QueryResult<SMessageTypeListener> result = new QueryResult<SMessageTypeListener>();

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
		                           select count(*) from smessagetypelistener where messagetypeid=@typeid	
                                    
                                   select {0},{1} from smessagetypelistener as listener join smessageexecutetype as mtype
                                                where listener.sequence in
                                                (
                                                select t.sequence from
                                                (
                                                    select sequence from smessagetypelistener 
                                                    where messagetypeid=@typeid
                                                    order by sequence limit {2}, {3}                                                   
                                                ) as t
                                                )", StoreHelper.GetSMessageTypeListenerSelectFields("listener"), StoreHelper.GetSMessageExecuteTypeSelectFields("mtype"),(page - 1) * pageSize, pageSize)
                })
                {


                    var parameter = new MySqlParameter("@typeid", MySqlDbType.Guid)
                    {
                        Value = typeId
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
                                    var listener = new SMessageTypeListener();
                                    StoreHelper.SetSMessageTypeListenerSelectFields(listener, reader, "listener");
                                    listener.MessageType = new SMessageExecuteType();
                                    StoreHelper.SetSMessageExecuteTypeSelectFields(listener.MessageType, reader, "mtype");
                                    result.Results.Add(listener);
                                }
                            }
                        }

                        await reader.CloseAsync();
                
                    }
                }
            });

            return result;
        }

        public async Task<SMessageTypeListener> QueryByTypeRelation(Guid typeId, Guid id)
        {
            SMessageTypeListener listener = null;

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
                    CommandText = string.Format(@"select {0},{1} from smessagetypelistener as listener join smessageexecutetype as mtype on listener.messagetypeid=mtype.id where listener.id=@id and mtype.id=@typeid", StoreHelper.GetSMessageTypeListenerSelectFields("listener"), StoreHelper.GetSMessageExecuteTypeSelectFields("mtype"))
                })
                {

                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new MySqlParameter("@typeid", MySqlDbType.Guid)
                    {
                        Value = typeId
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    DbDataReader reader = null;

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            listener = new SMessageTypeListener();
                            StoreHelper.SetSMessageTypeListenerSelectFields(listener, reader, "listener");
                            listener.MessageType = new SMessageExecuteType();
                            StoreHelper.SetSMessageExecuteTypeSelectFields(listener.MessageType, reader, "mtype");
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return listener;
        }

        public async Task UpdateByTypeRelation(SMessageTypeListener listener)
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
                    CommandText = @"update smessagetypelistener set name=@name,mode=@mode,listenerfactorytype=@listenerfactorytype,listenerfactorytypeusedi=@listenerfactorytypeusedi,listenerweburl=@listenerweburl,listenerwebsignature=@listenerwebsignature where id=@id and messagetypeid=@messagetypeid"
                })
                {

                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = listener.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@messagetypeid", MySqlDbType.Guid)
                    {
                        Value = listener.MessageType.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 150)
                    {
                        Value = listener.Name
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@mode", MySqlDbType.Int32)
                    {
                        Value = listener.Mode
                    };
                    commond.Parameters.Add(parameter);

                    if (listener.ListenerFactoryType != null)
                    {
                        parameter = new MySqlParameter("@listenerfactorytype", MySqlDbType.VarChar, 150)
                        {
                            Value = listener.ListenerFactoryType
                        };
                        commond.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new MySqlParameter("@listenerfactorytype", MySqlDbType.VarChar, 150)
                        {
                            Value = DBNull.Value
                        };
                        commond.Parameters.Add(parameter);
                    }

                    if (listener.ListenerFactoryTypeUseDI != null)
                    {
                        parameter = new MySqlParameter("@listenerfactorytypeusedi", MySqlDbType.Bit)
                        {
                            Value = listener.ListenerFactoryTypeUseDI
                        };
                        commond.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new MySqlParameter("@listenerfactorytypeusedi", MySqlDbType.Bit)
                        {
                            Value = DBNull.Value
                        };
                        commond.Parameters.Add(parameter);
                    }

                    if (listener.ListenerWebUrl != null)
                    {
                        parameter = new MySqlParameter("@listenerweburl", MySqlDbType.VarChar, 200)
                        {
                            Value = listener.ListenerWebUrl
                        };
                        commond.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new MySqlParameter("@listenerweburl", MySqlDbType.VarChar, 200)
                        {
                            Value = DBNull.Value
                        };
                        commond.Parameters.Add(parameter);
                    }


                    if (listener.ListenerWebSignature != null)
                    {
                        parameter = new MySqlParameter("@listenerwebsignature", MySqlDbType.VarChar, 150)
                        {
                            Value = listener.ListenerWebSignature
                        };
                        commond.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new MySqlParameter("@listenerwebsignature", MySqlDbType.VarChar, 150)
                        {
                            Value = DBNull.Value
                        };
                        commond.Parameters.Add(parameter);
                    }

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();



                }
            });
        }
    }
}
