using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using MSLibrary.Logger;
using MSLibrary.Logger.DAL;
using MSLibrary.DI;
using MSLibrary.Transaction;

namespace MSLibrary.MySqlStore.Logger.DAL
{
    [Injection(InterfaceType = typeof(ICommonLogStore), Scope = InjectionScope.Singleton)]
    public class CommonLogStore : ICommonLogStore
    {
        private readonly ICommonLogConnectionFactory _commonLogConnectionFactory;

        public CommonLogStore(ICommonLogConnectionFactory commonLogConnectionFactory)
        {
            _commonLogConnectionFactory = commonLogConnectionFactory;
        }
        public Task Add(CommonLog log)
        {
            throw new NotImplementedException();
        }

        public async Task AddLocal(CommonLog log)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _commonLogConnectionFactory.CreateAllForLocalCommonLog(), async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }

                await using (MySqlCommand command = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran
                })
                {
                    MySqlParameter parameter;
                    int length;
                    if (log.ID==Guid.Empty)
                    {
                        log.ID = Guid.NewGuid();
                    }

                        command.CommandText = string.Format(@"insert into commonlog_local (id,parentid,prelevelid,currentlevelid,contextinfo,traceid,linkid,parentcontextinfo,categoryname,actionname,parentactionname,requestbody,responsebody,requesturi,message,root,level,duration,createtime,modifytime)
                                                VALUES (@id,@parentid,@prelevelid,@currentlevelid,@contextinfo,@traceid,@linkid,@categoryname,@actionname,@parentactionname,@requestbody,@responsebody,@requesturi,@message,@root,@level,@duration,utc_timestamp(),utc_timestamp())");

                        parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                        {
                            Value = log.ID
                        };
                        command.Parameters.Add(parameter);
                    


                    parameter = new MySqlParameter("@parentid", MySqlDbType.Guid)
                    {
                        Value = log.ParentID
                    };
                    command.Parameters.Add(parameter);

                    if (log.ContextInfo.Length == 0)
                    {
                        length = 10;
                    }
                    else
                    {
                        length = log.ContextInfo.Length;
                    }


                    parameter = new MySqlParameter("@prelevelid", MySqlDbType.Guid)
                    {
                        Value = log.PreLevelID
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@currentlevelid", MySqlDbType.Guid)
                    {
                        Value = log.CurrentLevelID
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@contextinfo", MySqlDbType.VarChar, length)
                    {
                        Value = log.ContextInfo
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@traceid", MySqlDbType.VarChar, length)
                    {
                        Value = log.TraceID
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@linkid", MySqlDbType.VarChar, length)
                    {
                        Value = log.LinkID
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@categoryname", MySqlDbType.VarChar, 300)
                    {
                        Value = log.CategoryName
                    };
                    command.Parameters.Add(parameter);


                    parameter = new MySqlParameter("@actionname", MySqlDbType.VarChar, 300)
                    {
                        Value = log.ActionName
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@parentactionname", MySqlDbType.VarChar, 300)
                    {
                        Value = log.ParentActionName
                    };
                    command.Parameters.Add(parameter);


                    if (log.RequestBody == null)
                    {
                        log.RequestBody = string.Empty;
                    }

                    if (log.RequestBody.Length == 0)
                    {
                        length = 10;
                    }
                    else
                    {
                        if (log.RequestBody.Length>6000)
                        {
                            log.RequestBody = log.RequestBody.Substring(0, 6000);
                        }
                        length = log.RequestBody.Length;
                    }

                    parameter = new MySqlParameter("@requestbody", MySqlDbType.VarChar, length)
                    {
                        Value = log.RequestBody
                    };
                    command.Parameters.Add(parameter);

                    if (log.ResponseBody == null)
                    {
                        log.ResponseBody = string.Empty;
                    }

                    if (log.ResponseBody.Length == 0)
                    {
                        length = 10;
                    }
                    else
                    {
                        if (log.ResponseBody.Length>6000)
                        {
                            log.ResponseBody = log.ResponseBody.Substring(0, 6000);
                        }
                        length = log.ResponseBody.Length;
                    }

                    parameter = new MySqlParameter("@responsebody", MySqlDbType.VarChar, length)
                    {
                        Value = log.ResponseBody
                    };
                    command.Parameters.Add(parameter);


                    parameter = new MySqlParameter("@requesturi", MySqlDbType.VarChar, 500)
                    {
                        Value = log.RequestUri
                    };
                    command.Parameters.Add(parameter);


                   
                    if (log.Message.Length == 0)
                    {
                        length = 10;
                    }
                    else
                    {
                        if (log.Message.Length>6000)
                        {
                            log.Message = log.Message.Substring(0, 6000);
                        }
                        length = log.Message.Length;
                    }

                 

                    parameter = new MySqlParameter("@message", MySqlDbType.VarChar, length)
                    {
                        Value = log.Message
                    };
                    command.Parameters.Add(parameter);


                    parameter = new MySqlParameter("@root", MySqlDbType.Bit)
                    {
                        Value = log.Root
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@level", MySqlDbType.Int32)
                    {
                        Value = log.Level
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@duration", MySqlDbType.Int64)
                    {
                        Value = log.Duration
                    };
                    command.Parameters.Add(parameter);


                    await command.PrepareAsync();

                    await command.ExecuteNonQueryAsync();
                }
            });
        }

        public Task<CommonLog> QueryByID(Guid id, Guid parentID, string parentAction)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<CommonLog>> QueryByParentId(Guid parentID, string parentAction, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<CommonLog>> QueryLocal(string message, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<CommonLog> QueryLocalByID(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<CommonLog>> QueryRootByConditionTop(string parentAction, int? level, int top)
        {
            throw new NotImplementedException();
        }
    }
}
