using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DAL;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Transaction;
using MSLibrary.Schedule;
using MSLibrary.Schedule.DAL;

namespace MSLibrary.MySqlStore.Schedule.DAL
{
    /// <summary>
    /// 调度动作数据操作
    /// </summary>
    [Injection(InterfaceType = typeof(IScheduleActionStore), Scope = InjectionScope.Singleton)]
    public class ScheduleActionStore : IScheduleActionStore
    {
        private IScheduleConnectionFactory _dbConnectionFactory;

        public ScheduleActionStore(IScheduleConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task Add(ScheduleAction action)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _dbConnectionFactory.CreateAllForSchedule(), async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }
                using (MySqlCommand command = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                })
                {
                    if (action.ID==Guid.Empty)
                    {
                        action.ID = Guid.NewGuid();
                    }
                    MySqlParameter parameter;

                        command.CommandText = @"INSERT INTO scheduleaction
                                                    (id,name,triggercondition,configuration,mode,scheduleactionservicefactorytype,scheduleactionservicefactorytypeusedi,scheduleactionserviceweburl,websignature,status,createtime,modifytime)
                                                VALUES
                                                    (@id, @name, @triggercondition,@configuration, @mode, @scheduleactionservicefactorytype, @scheduleactionservicefactorytypeusedi, @scheduleactionserviceweburl, @websignature, @status, utc_timestamp(), utc_timestamp());";
                        parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                        {
                            Value = action.ID
                        };
                        command.Parameters.Add(parameter);
                    
                    parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 150)
                    {
                        Value = action.Name
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@triggercondition", MySqlDbType.VarChar, 200)
                    {
                        Value = action.TriggerCondition
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@configuration", MySqlDbType.VarChar, action.Configuration.Length)
                    {
                        Value = action.Configuration
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@mode", MySqlDbType.Int32)
                    {
                        Value = action.Mode
                    };
                    command.Parameters.Add(parameter);
                    if (action.ScheduleActionServiceFactoryType != null)
                    {
                        parameter = new MySqlParameter("@scheduleactionservicefactorytype", MySqlDbType.VarChar, 200)
                        {
                            Value = action.ScheduleActionServiceFactoryType
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new MySqlParameter("@scheduleactionservicefactorytype", MySqlDbType.VarChar, 200)
                        {
                            Value = DBNull.Value
                        };
                        command.Parameters.Add(parameter);
                    }
                    if (action.ScheduleActionServiceFactoryTypeUseDI != null)
                    {
                        parameter = new MySqlParameter("@scheduleactionservicefactorytypeusedi", MySqlDbType.Bit)
                        {
                            Value = action.ScheduleActionServiceFactoryTypeUseDI
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new MySqlParameter("@scheduleactionservicefactorytypeusedi", MySqlDbType.Bit)
                        {
                            Value = DBNull.Value
                        };
                        command.Parameters.Add(parameter);
                    }
                    if (action.ScheduleActionServiceWebUrl != null)
                    {
                        parameter = new MySqlParameter("@scheduleactionserviceweburl", MySqlDbType.VarChar, 200)
                        {
                            Value = action.ScheduleActionServiceWebUrl
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new MySqlParameter("@scheduleactionserviceweburl", MySqlDbType.VarChar, 200)
                        {
                            Value = DBNull.Value
                        };
                        command.Parameters.Add(parameter);
                    }
                    if (action.WebSignature != null)
                    {
                        parameter = new MySqlParameter("@websignature", MySqlDbType.VarChar, 200)
                        {
                            Value = action.WebSignature
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new MySqlParameter("@websignature", MySqlDbType.VarChar, 200)
                        {
                            Value = DBNull.Value
                        };
                        command.Parameters.Add(parameter);
                    }

                    parameter = new MySqlParameter("@status", MySqlDbType.Int32)
                    {
                        Value = action.Status
                    };
                    command.Parameters.Add(parameter);
                    command.Prepare();

                    //try
                    //{
                        await command.ExecuteNonQueryAsync();
                    //}
                    //catch (SqlException ex)
                    //{
                    //    if (ex == null)
                    //    {
                    //        throw;
                    //    }
                    //    if (ex.Number == 2601)
                    //    {
                    //        var fragment = new TextFragment()
                    //        {
                    //            Code = TextCodes.ExistScheduleActionByName,
                    //            DefaultFormatting = "调度动作中存在相同名称\"{0}\"数据",
                    //            ReplaceParameters = new List<object>() { action.Name }
                    //        };

                    //        throw new UtilityException((int)Errors.ExistScheduleActionByName, fragment);
                    //    }
                    //    else
                    //    {
                    //        throw;
                    //    }
                    //}
                    //如果用户未赋值ID则创建成功后返回ID
                    //if (action.ID == Guid.Empty)
                    //{
                    //    action.ID = (Guid)command.Parameters["@newid"].Value;
                    //};
                }

            });
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task Update(ScheduleAction action)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _dbConnectionFactory.CreateAllForSchedule(), async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }
                using (MySqlCommand command = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = @"UPDATE scheduleaction
                                   SET 
                                      name = @name,
                                      triggercondition = @triggercondition,
                                      configuration=@configuration,
                                      mode = @mode,
                                      scheduleactionservicefactorytype = @scheduleactionservicefactorytype,
                                      scheduleactionservicefactorytypeusedi = @scheduleactionservicefactorytypeusedi,
                                      scheduleactionserviceweburl = @scheduleactionserviceweburl,
                                      websignature = @websignature,
                                      status = @status,
                                      modifytime = utc_timestamp()
                                 WHERE id=@id;"
                })
                {
                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = action.ID
                    };
                    command.Parameters.Add(parameter);
                    parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 150)
                    {
                        Value = action.Name
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@triggercondition", MySqlDbType.VarChar, 200)
                    {
                        Value = action.TriggerCondition
                    };
                    command.Parameters.Add(parameter);


                    parameter = new MySqlParameter("@configuration", MySqlDbType.VarChar, action.Configuration.Length)
                    {
                        Value = action.Configuration
                    };
                    command.Parameters.Add(parameter);


                    parameter = new MySqlParameter("@mode", MySqlDbType.Int32)
                    {
                        Value = action.Mode
                    };
                    command.Parameters.Add(parameter);
                    if (action.ScheduleActionServiceFactoryType != null)
                    {
                        parameter = new MySqlParameter("@scheduleactionservicefactorytype", MySqlDbType.VarChar, 200)
                        {
                            Value = action.ScheduleActionServiceFactoryType
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new MySqlParameter("@scheduleactionservicefactorytype", MySqlDbType.VarChar, 200)
                        {
                            Value = DBNull.Value
                        };
                        command.Parameters.Add(parameter);
                    }
                    if (action.ScheduleActionServiceFactoryTypeUseDI != null)
                    {
                        parameter = new MySqlParameter("@scheduleactionservicefactorytypeusedi", MySqlDbType.Bit)
                        {
                            Value = action.ScheduleActionServiceFactoryTypeUseDI
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new MySqlParameter("@scheduleactionservicefactorytypeusedi", MySqlDbType.Bit)
                        {
                            Value = DBNull.Value
                        };
                        command.Parameters.Add(parameter);
                    }
                    if (action.ScheduleActionServiceWebUrl != null)
                    {
                        parameter = new MySqlParameter("@scheduleactionserviceweburl", MySqlDbType.VarChar, 200)
                        {
                            Value = action.ScheduleActionServiceWebUrl
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new MySqlParameter("@scheduleactionserviceweburl", MySqlDbType.VarChar, 200)
                        {
                            Value = DBNull.Value
                        };
                        command.Parameters.Add(parameter);
                    }
                    if (action.WebSignature != null)
                    {
                        parameter = new MySqlParameter("@websignature", MySqlDbType.VarChar, 200)
                        {
                            Value = action.WebSignature
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new MySqlParameter("@websignature", MySqlDbType.VarChar, 200)
                        {
                            Value = DBNull.Value
                        };
                        command.Parameters.Add(parameter);
                    }
                    parameter = new MySqlParameter("@status", MySqlDbType.Int32)
                    {
                        Value = action.Status
                    };
                    command.Parameters.Add(parameter);

                    command.Prepare();


                    //try
                    //{
                        await command.ExecuteNonQueryAsync();
                    //}
                    //catch (SqlException ex)
                    //{
                    //    if (ex == null)
                    //    {
                    //        throw;
                    //    }
                    //    if (ex.Number == 2601)
                    //    {
                    //        var fragment = new TextFragment()
                    //        {
                    //            Code = TextCodes.ExistScheduleActionByName,
                    //            DefaultFormatting = "调度动作中存在相同名称\"{0}\"数据",
                    //            ReplaceParameters = new List<object>() { action.Name }
                    //        };

                    //        throw new UtilityException((int)Errors.ExistScheduleActionByName, fragment);
                    //    }
                    //    else
                    //    {
                    //        throw;
                    //    }
                    //}
                }

            });
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(Guid id)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _dbConnectionFactory.CreateAllForSchedule(), async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }

                using (MySqlCommand command = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = @"delete from scheduleaction where [id] = @id"
                })
                {

                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);
                    command.Prepare();

                    await command.ExecuteNonQueryAsync();

                }

            });
        }

        /// <summary>
        /// 增加动作与组的关联关系
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="queueId"></param>
        /// <returns></returns>
        public async Task AddActionGroupRelation(Guid actionId, Guid queueId)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _dbConnectionFactory.CreateAllForSchedule(), async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }
                using (MySqlCommand command = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = @"UPDATE scheduleaction
                                       SET groupid=@groupid,modifytime = utc_timestamp()
                                     WHERE id=@id AND groupid IS NULL"
                })
                {
                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = actionId
                    };
                    command.Parameters.Add(parameter);
                    parameter = new MySqlParameter("@groupid", MySqlDbType.Guid)
                    {
                        Value = queueId
                    };
                    command.Parameters.Add(parameter);
                    command.Prepare();


                    await command.ExecuteNonQueryAsync();

                }

            });
        }

        /// <summary>
        /// 删除动作与组的关联关系
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="queueId"></param>
        /// <returns></returns>
        public async Task DeleteActionGroupRelation(Guid actionId, Guid queueId)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _dbConnectionFactory.CreateAllForSchedule(), async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }
                using (MySqlCommand command = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = @"UPDATE scheduleaction
                                       SET groupid=null,modifytime = utc_timestamp()
                                     WHERE id=@id AND groupid =@groupid"
                })
                {
                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = actionId
                    };
                    command.Parameters.Add(parameter);
                    parameter = new MySqlParameter("@groupid", MySqlDbType.Guid)
                    {
                        Value = queueId
                    };
                    command.Parameters.Add(parameter);
                    command.Prepare();

                    await command.ExecuteNonQueryAsync();

                }

            });
        }

        /// <summary>
        /// 根据调度动作组Id查询
        /// </summary>
        /// <param name="id"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<ScheduleAction> QueryByGroup(Guid id, Guid groupId)
        {
            ScheduleAction result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _dbConnectionFactory.CreateReadForSchedule(), async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }

                using (MySqlCommand command = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"SELECT {0} FROM scheduleaction WHERE id=@id AND groupid=@groupid", StoreHelper.GetScheduleActionStoreSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {
                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);
                    parameter = new MySqlParameter("@groupid", MySqlDbType.Guid)
                    {
                        Value = groupId
                    };
                    command.Parameters.Add(parameter);
                    command.Prepare();
                    DbDataReader reader = null;

                    using (reader = await command.ExecuteReaderAsync())
                    {

                        if (await reader.ReadAsync())
                        {
                            result = new ScheduleAction();
                            StoreHelper.SetScheduleActionStoreSelectFields(result, reader, string.Empty);
                        }

                        reader.Close();
                    }
                }
            });
            return result;
        }

        /// <summary>
        /// 根据Id查询调度动作
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ScheduleAction> QueryByID(Guid id)
        {
            ScheduleAction result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _dbConnectionFactory.CreateReadForSchedule(), async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }

                using (MySqlCommand command = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"SELECT {0} FROM scheduleaction WHERE id=@id;", StoreHelper.GetScheduleActionStoreSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {
                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);
                    command.Prepare();
                    DbDataReader reader = null;

                    using (reader = await command.ExecuteReaderAsync())
                    {

                        if (await reader.ReadAsync())
                        {
                            result = new ScheduleAction();
                            StoreHelper.SetScheduleActionStoreSelectFields(result, reader, string.Empty);
                        }

                        reader.Close();
                    }
                }
            });
            return result;
        }

        /// <summary>
        /// 根据名称查询调度动作
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<ScheduleAction> QueryByName(string name)
        {
            ScheduleAction result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _dbConnectionFactory.CreateReadForSchedule(), async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }

                using (MySqlCommand command = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"SELECT {0} FROM scheduleaction WHERE name=@name;", StoreHelper.GetScheduleActionStoreSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {
                    var parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 150)
                    {
                        Value = name
                    };
                    command.Parameters.Add(parameter);
                    command.Prepare();
                    DbDataReader reader = null;

                    using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new ScheduleAction();
                            StoreHelper.SetScheduleActionStoreSelectFields(result, reader, string.Empty);
                        }
                        reader.Close();
                    }
                }
            });
            return result;
        }

        /// <summary>
        /// 根据名称匹配分页查询调度动作
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<QueryResult<ScheduleAction>> QueryByPage(string name, int page, int pageSize)
        {
            QueryResult<ScheduleAction> result = new QueryResult<ScheduleAction>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _dbConnectionFactory.CreateReadForSchedule(), async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }

                using (MySqlCommand command = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"
                                                    SELECT COUNT(*)
                                                    FROM scheduleaction
                                                    WHERE name LIKE @name;                                                         

                                                    SELECT {0}
                                                    FROM scheduleaction
                                                    where sequence in 
                                                    (
                                                        select t.sequence
                                                        from
                                                        (
                                                            SELECT sequence
                                                            FROM scheduleaction
                                                            WHERE name LIKE @name
                                                            ORDER BY sequence limit {1}, {2}
                                                        ) as t
                                                    )",
                                                    StoreHelper.GetScheduleActionStoreSelectFields(string.Empty),(page-1)*pageSize,pageSize), Transaction = sqlTran
                })
                {
                    var parameter = new MySqlParameter("@page", MySqlDbType.Int32)
                    {
                        Value = page
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@pagesize", MySqlDbType.Int32)
                    {
                        Value = pageSize
                    };
                    command.Parameters.Add(parameter);


                    parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 200)
                    {
                        Value = $"{name.ToMySqlLike()}%"

                    };
                    command.Parameters.Add(parameter);
                    command.Prepare();
                    DbDataReader reader = null;

                    using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result.TotalCount = reader.GetInt32(0);
                        }

                        if (await reader.NextResultAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var scheduleAction = new ScheduleAction();
                                StoreHelper.SetScheduleActionStoreSelectFields(scheduleAction, reader, string.Empty);
                                result.Results.Add(scheduleAction);
                            }
                        }
                        reader.Close();                       
                        result.CurrentPage = page;
                    }
                }
            });
            return result;
        }

        /// <summary>
        /// 根据调度动作组Id分页查询调度动作
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<QueryResult<ScheduleAction>> QueryByPageGroup(Guid groupId, int page, int pageSize)
        {
            QueryResult<ScheduleAction> result = new QueryResult<ScheduleAction>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _dbConnectionFactory.CreateReadForSchedule(), async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }

                using (MySqlCommand command = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"
                                                    SELECT COUNT(*)
                                                    FROM scheduleaction
                                                    WHERE groupid = @groupid;

                                                    SELECT {0}
                                                    FROM ScheduleAction
                                                    WHERE sequence in
                                                    (
                                                        select t.sequence
                                                        from 
                                                        (
                                                            SELECT sequence
                                                            FROM scheduleaction
                                                            WHERE groupid = @groupid
                                                            ORDER BY sequence limit {1}, {2}
                                                        ) as t
                                                    )",
                                                    StoreHelper.GetScheduleActionStoreSelectFields(string.Empty),(page-1)*pageSize,pageSize),Transaction = sqlTran
                })
                {
                    var parameter = new MySqlParameter("@page", MySqlDbType.Int32)
                    {
                        Value = page
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@pagesize", MySqlDbType.Int32)
                    {
                        Value = pageSize
                    };
                    command.Parameters.Add(parameter);



                    parameter = new MySqlParameter("@groupid", MySqlDbType.Guid)
                    {
                        Value = groupId
                    };
                    command.Parameters.Add(parameter);
                    command.Prepare();
                    DbDataReader reader = null;

                    using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result.TotalCount = reader.GetInt32(0);
                        }

                        if (await reader.NextResultAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var scheduleAction = new ScheduleAction();
                                StoreHelper.SetScheduleActionStoreSelectFields(scheduleAction, reader, string.Empty);
                                result.Results.Add(scheduleAction);
                            }
                        }
                        reader.Close();
                        
                        result.CurrentPage = page;
                    }
                }
            });
            return result;
        }

        /// <summary>
        /// 分页查询尚未分配组且匹配动作名称的调度动作
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<QueryResult<ScheduleAction>> QueryByNullGroup(string name, int page, int pageSize)
        {
            QueryResult<ScheduleAction> result = new QueryResult<ScheduleAction>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _dbConnectionFactory.CreateReadForSchedule(), async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }

                using (MySqlCommand command = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"
                                                    SELECT  COUNT(*)
                                                    FROM scheduleaction
                                                    WHERE name LIKE @name
                                                          AND groupid IS NULL;


                                                    SELECT {0}
                                                    FROM scheduleaction
                                                    where sequence in
                                                    (
                                                        select t.sequence
                                                        from 
                                                        (
                                                        SELECT sequence
                                                        FROM scheduleaction
                                                        WHERE name LIKE @name
                                                          AND groupid IS NULL
                                                        ORDER BY sequence limit {1}, {2}
                                                        ) as t
                                                    )",
                                                    StoreHelper.GetScheduleActionStoreSelectFields(string.Empty),(page-1)*pageSize,pageSize),
                    Transaction = sqlTran
                })
                {

                    var parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 200)
                    {
                        Value = $"{name.ToMySqlLike()}%"
                    };
                    command.Parameters.Add(parameter);
                    command.Prepare();
                    DbDataReader reader = null;


                    using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result.TotalCount = reader.GetInt32(0);
                        }

                        if (await reader.NextResultAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var scheduleAction = new ScheduleAction();
                                StoreHelper.SetScheduleActionStoreSelectFields(scheduleAction, reader, string.Empty);
                                result.Results.Add(scheduleAction);
                            }
                        }

                        reader.Close();
                        
                        result.CurrentPage = page;
                    }
                }
            });
            return result;
        }

        /// <summary>
        /// 查询调度动作id下面的所有指定状态的调度动作
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="status"></param> 
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task QueryAllAction(Guid groupId, int status, Func<ScheduleAction, Task> callback)
        {
            List<ScheduleAction> listAction = new List<ScheduleAction>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _dbConnectionFactory.CreateReadForSchedule(), async (conn, transaction) =>
            {
                long sequence = 0;
                int pageSize = 500;
                while (true)
                {
                    listAction.Clear();

                    MySqlTransaction sqlTran = null;
                    if (transaction != null)
                    {
                        sqlTran = (MySqlTransaction)transaction;
                    }

                    using (MySqlCommand command = new MySqlCommand()
                    {
                        Connection = (MySqlConnection)conn,
                        CommandType = CommandType.Text,
                        CommandText = string.Format(@"SELECT {0} FROM scheduleaction WHERE groupid=@groupid AND status=@status and sequence>@sequence ORDER BY sequence limit {1}", StoreHelper.GetScheduleActionStoreSelectFields(string.Empty),pageSize),
                        Transaction = sqlTran
                    })
                    {
                        var parameter = new MySqlParameter("@groupid", MySqlDbType.Guid)
                        {
                            Value = groupId
                        };
                        command.Parameters.Add(parameter);
                        parameter = new MySqlParameter("@status", MySqlDbType.Int32)
                        {
                            Value = status
                        };
                        command.Parameters.Add(parameter);
                        parameter = new MySqlParameter("@pagesize", MySqlDbType.Int32)
                        {
                            Value = pageSize
                        };
                        command.Parameters.Add(parameter);
                        parameter = new MySqlParameter("@sequence", MySqlDbType.Int64)
                        {
                            Value = sequence
                        };
                        command.Parameters.Add(parameter);
                        command.Prepare();
                        DbDataReader reader = null;


                        using (reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var scheduleAction = new ScheduleAction();
                                StoreHelper.SetScheduleActionStoreSelectFields(scheduleAction, reader, string.Empty);
                                listAction.Add(scheduleAction);
                                sequence = (long)reader["sequence"];
                            }
                            reader.Close();
                        }
                    }
                    foreach (var actionItem in listAction)
                    {
                        await callback(actionItem);
                    }
                    if (listAction.Count != pageSize)
                    {
                        break;
                    }
                }
            });
        }

    }
}
