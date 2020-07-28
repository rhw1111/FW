using MSLibrary.DAL;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Transaction;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Schedule.DAL
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
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForSchedule(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }
                using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                })
                {
                    SqlParameter parameter;
                    if (action.ID == Guid.Empty)
                    {
                        command.CommandText = @"INSERT INTO [dbo].[ScheduleAction]
                                                    ([id],[name],[triggercondition],[configuration],[mode],[scheduleactionservicefactorytype],[scheduleactionservicefactorytypeusedi],[scheduleactionserviceweburl],[websignature],[status],[createtime],[modifytime])
                                                VALUES
                                                    (default, @name, @triggercondition,@configuration, @mode, @scheduleactionservicefactorytype, @scheduleactionservicefactorytypeusedi, @scheduleactionserviceweburl, @websignature, @status, GETUTCDATE(), GETUTCDATE());
                                                select @newid =[id] from [dbo].[ScheduleAction] where [sequence] = SCOPE_IDENTITY()";
                        parameter = new SqlParameter("@newid", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        command.CommandText = @"INSERT INTO [dbo].[ScheduleAction]
                                                    ([id],[name],[triggercondition],[configuration],[mode],[scheduleactionservicefactorytype],[scheduleactionservicefactorytypeusedi],[scheduleactionserviceweburl],[websignature],[status],[createtime],[modifytime])
                                                VALUES
                                                    (@id, @name, @triggercondition,@configuration, @mode, @scheduleactionservicefactorytype, @scheduleactionservicefactorytypeusedi, @scheduleactionserviceweburl, @websignature, @status, GETUTCDATE(), GETUTCDATE());";
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = action.ID
                        };
                        command.Parameters.Add(parameter);
                    }
                    parameter = new SqlParameter("@name", SqlDbType.VarChar, 150)
                    {
                        Value = action.Name
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@triggercondition", SqlDbType.VarChar, 200)
                    {
                        Value = action.TriggerCondition
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@configuration", SqlDbType.NVarChar, action.Configuration.Length)
                    {
                        Value = action.Configuration
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@mode", SqlDbType.Int)
                    {
                        Value = action.Mode
                    };
                    command.Parameters.Add(parameter);
                    if (action.ScheduleActionServiceFactoryType != null)
                    {
                        parameter = new SqlParameter("@scheduleactionservicefactorytype", SqlDbType.VarChar, 200)
                        {
                            Value = action.ScheduleActionServiceFactoryType
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new SqlParameter("@scheduleactionservicefactorytype", SqlDbType.VarChar, 200)
                        {
                            Value = DBNull.Value
                        };
                        command.Parameters.Add(parameter);
                    }
                    if (action.ScheduleActionServiceFactoryTypeUseDI != null)
                    {
                        parameter = new SqlParameter("@scheduleactionservicefactorytypeusedi", SqlDbType.Bit)
                        {
                            Value = action.ScheduleActionServiceFactoryTypeUseDI
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new SqlParameter("@scheduleactionservicefactorytypeusedi", SqlDbType.Bit)
                        {
                            Value = DBNull.Value
                        };
                        command.Parameters.Add(parameter);
                    }
                    if (action.ScheduleActionServiceWebUrl != null)
                    {
                        parameter = new SqlParameter("@scheduleactionserviceweburl", SqlDbType.VarChar, 200)
                        {
                            Value = action.ScheduleActionServiceWebUrl
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new SqlParameter("@scheduleactionserviceweburl", SqlDbType.VarChar, 200)
                        {
                            Value = DBNull.Value
                        };
                        command.Parameters.Add(parameter);
                    }
                    if (action.WebSignature != null)
                    {
                        parameter = new SqlParameter("@websignature", SqlDbType.VarChar, 200)
                        {
                            Value = action.WebSignature
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new SqlParameter("@websignature", SqlDbType.VarChar, 200)
                        {
                            Value = DBNull.Value
                        };
                        command.Parameters.Add(parameter);
                    }

                    parameter = new SqlParameter("@status", SqlDbType.Int)
                    {
                        Value = action.Status
                    };
                    command.Parameters.Add(parameter);
                    command.Prepare();

                        try
                        {
                            await command.ExecuteNonQueryAsync();
                        }
                        catch (SqlException ex)
                        {
                            if (ex == null)
                            {
                                throw;
                            }
                            if (ex.Number == 2601)
                            {
                                var fragment = new TextFragment()
                                {
                                    Code = TextCodes.ExistScheduleActionByName,
                                    DefaultFormatting = "调度动作中存在相同名称\"{0}\"数据",
                                    ReplaceParameters = new List<object>() { action.Name }
                                };

                                throw new UtilityException((int)Errors.ExistScheduleActionByName, fragment);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    //如果用户未赋值ID则创建成功后返回ID
                    if (action.ID == Guid.Empty)
                    {
                        action.ID = (Guid)command.Parameters["@newid"].Value;
                    };
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
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForSchedule(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }
                using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = @"UPDATE [dbo].[ScheduleAction]
                                   SET 
                                      [name] = @name,
                                      [triggercondition] = @triggercondition,
                                      [configuration]=@configuration,
                                      [mode] = @mode,
                                      [scheduleactionservicefactorytype] = @scheduleactionservicefactorytype,
                                      [scheduleactionservicefactorytypeusedi] = @scheduleactionservicefactorytypeusedi,
                                      [scheduleactionserviceweburl] = @scheduleactionserviceweburl,
                                      [websignature] = @websignature,
                                      [status] = @status,
                                      [modifytime] = GETUTCDATE()
                                 WHERE id=@id;"
                })
                {
                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = action.ID
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@name", SqlDbType.VarChar, 150)
                    {
                        Value = action.Name
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@triggercondition", SqlDbType.VarChar, 200)
                    {
                        Value = action.TriggerCondition
                    };
                    command.Parameters.Add(parameter);


                    parameter = new SqlParameter("@configuration", SqlDbType.NVarChar, action.Configuration.Length)
                    {
                        Value = action.Configuration
                    };
                    command.Parameters.Add(parameter);


                    parameter = new SqlParameter("@mode", SqlDbType.Int)
                    {
                        Value = action.Mode
                    };
                    command.Parameters.Add(parameter);
                    if (action.ScheduleActionServiceFactoryType != null)
                    {
                        parameter = new SqlParameter("@scheduleactionservicefactorytype", SqlDbType.VarChar, 200)
                        {
                            Value = action.ScheduleActionServiceFactoryType
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new SqlParameter("@scheduleactionservicefactorytype", SqlDbType.VarChar, 200)
                        {
                            Value = DBNull.Value
                        };
                        command.Parameters.Add(parameter);
                    }
                    if (action.ScheduleActionServiceFactoryTypeUseDI != null)
                    {
                        parameter = new SqlParameter("@scheduleactionservicefactorytypeusedi", SqlDbType.Bit)
                        {
                            Value = action.ScheduleActionServiceFactoryTypeUseDI
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new SqlParameter("@scheduleactionservicefactorytypeusedi", SqlDbType.Bit)
                        {
                            Value = DBNull.Value
                        };
                        command.Parameters.Add(parameter);
                    }
                    if (action.ScheduleActionServiceWebUrl != null)
                    {
                        parameter = new SqlParameter("@scheduleactionserviceweburl", SqlDbType.VarChar, 200)
                        {
                            Value = action.ScheduleActionServiceWebUrl
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new SqlParameter("@scheduleactionserviceweburl", SqlDbType.VarChar, 200)
                        {
                            Value = DBNull.Value
                        };
                        command.Parameters.Add(parameter);
                    }
                    if (action.WebSignature != null)
                    {
                        parameter = new SqlParameter("@websignature", SqlDbType.VarChar, 200)
                        {
                            Value = action.WebSignature
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new SqlParameter("@websignature", SqlDbType.VarChar, 200)
                        {
                            Value = DBNull.Value
                        };
                        command.Parameters.Add(parameter);
                    }
                    parameter = new SqlParameter("@status", SqlDbType.Int)
                    {
                        Value = action.Status
                    };
                    command.Parameters.Add(parameter);

                    command.Prepare();


                        try
                        {
                            await command.ExecuteNonQueryAsync();
                        }
                        catch (SqlException ex)
                        {
                            if (ex == null)
                            {
                                throw;
                            }
                            if (ex.Number == 2601)
                            {
                                var fragment = new TextFragment()
                                {
                                    Code = TextCodes.ExistScheduleActionByName,
                                    DefaultFormatting = "调度动作中存在相同名称\"{0}\"数据",
                                    ReplaceParameters = new List<object>() { action.Name }
                                };

                                throw new UtilityException((int)Errors.ExistScheduleActionByName, fragment);
                            }
                            else
                            {
                                throw;
                            }
                        }
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
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForSchedule(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = @"delete from [dbo].[ScheduleAction] where [id] = @id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
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
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForSchedule(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }
                using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = @"UPDATE [dbo].[ScheduleAction]
                                       SET [groupid]=@groupid,[modifytime] = GETUTCDATE()
                                     WHERE id=@id AND [groupid] IS NULL"
                })
                {
                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = actionId
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
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
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForSchedule(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }
                using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = @"UPDATE [dbo].[ScheduleAction]
                                       SET [groupid]=null,[modifytime] = GETUTCDATE()
                                     WHERE id=@id AND groupid =@groupid"
                })
                {
                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = actionId
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
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
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForSchedule(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"SELECT {0} FROM [dbo].[ScheduleAction] WHERE id=@id AND groupid=@groupid", StoreHelper.GetScheduleActionStoreSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {
                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                    {
                        Value = groupId
                    };
                    command.Parameters.Add(parameter);
                    command.Prepare();
                    SqlDataReader reader = null;

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
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForSchedule(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"SELECT {0} FROM [dbo].[ScheduleAction] WHERE id=@id;", StoreHelper.GetScheduleActionStoreSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {
                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);
                    command.Prepare();
                    SqlDataReader reader = null;

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
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForSchedule(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"SELECT {0} FROM [dbo].[ScheduleAction] WHERE name=@name;", StoreHelper.GetScheduleActionStoreSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {
                    var parameter = new SqlParameter("@name", SqlDbType.VarChar, 150)
                    {
                        Value = name
                    };
                    command.Parameters.Add(parameter);
                    command.Prepare();
                    SqlDataReader reader = null;

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
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForSchedule(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"SET @currentpage = @page;
                                                    SELECT @count = COUNT(*)
                                                    FROM [ScheduleAction]
                                                    WHERE [name] LIKE @name                                                          
                                                    IF @pagesize * @page >= @count
                                                    BEGIN
                                                        SET @currentpage = @count / @pagesize;
                                                        IF @count % @pagesize <> 0
                                                        BEGIN
                                                            SET @currentpage = @currentpage + 1;
                                                        END;
                                                        IF @currentpage = 0
                                                            SET @currentpage = 1;
                                                    END;
                                                    ELSE IF @page < 1
                                                    BEGIN
                                                        SET @currentpage = 1;
                                                    END;

                                                    SELECT {0}
                                                    FROM [ScheduleAction]
                                                    WHERE [name] LIKE @name
                                                    ORDER BY sequence OFFSET (@pagesize * (@currentpage - 1)) ROWS FETCH NEXT @pagesize ROWS ONLY;",
                                                    StoreHelper.GetScheduleActionStoreSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {
                    var parameter = new SqlParameter("@page", SqlDbType.Int)
                    {
                        Value = page
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@pagesize", SqlDbType.Int)
                    {
                        Value = pageSize
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@count", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@currentpage", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@name", SqlDbType.VarChar, 200)
                    {
                        Value = $"{name.ToSqlLike()}%"

                    };
                    command.Parameters.Add(parameter);
                    command.Prepare();
                    SqlDataReader reader = null;

                    using (reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var scheduleAction = new ScheduleAction();
                            StoreHelper.SetScheduleActionStoreSelectFields(scheduleAction, reader, string.Empty);
                            result.Results.Add(scheduleAction);
                        }
                        reader.Close();
                        result.TotalCount = (int)command.Parameters["@count"].Value;
                        result.CurrentPage = (int)command.Parameters["@currentpage"].Value;
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
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForSchedule(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"SET @currentpage = @page;
                                                    SELECT @count = COUNT(*)
                                                    FROM [ScheduleAction]
                                                    WHERE [groupid] = @groupid;
                                                    IF @pagesize * @page >= @count
                                                    BEGIN
                                                        SET @currentpage = @count / @pagesize;
                                                        IF @count % @pagesize <> 0
                                                        BEGIN
                                                            SET @currentpage = @currentpage + 1;
                                                        END;
                                                        IF @currentpage = 0
                                                            SET @currentpage = 1;
                                                    END;
                                                    ELSE IF @page < 1
                                                    BEGIN
                                                        SET @currentpage = 1;
                                                    END;

                                                    SELECT {0}
                                                    FROM [ScheduleAction]
                                                    WHERE [groupid] = @groupid
                                                    ORDER BY sequence OFFSET (@pagesize * (@currentpage - 1)) ROWS FETCH NEXT @pagesize ROWS ONLY;",
                                                    StoreHelper.GetScheduleActionStoreSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {
                    var parameter = new SqlParameter("@page", SqlDbType.Int)
                    {
                        Value = page
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@pagesize", SqlDbType.Int)
                    {
                        Value = pageSize
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@count", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@currentpage", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                    {
                        Value = groupId
                    };
                    command.Parameters.Add(parameter);
                    command.Prepare();
                    SqlDataReader reader = null;

                    using (reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var scheduleAction = new ScheduleAction();
                            StoreHelper.SetScheduleActionStoreSelectFields(scheduleAction, reader, string.Empty);

                            result.Results.Add(scheduleAction);
                        }
                        reader.Close();
                        result.TotalCount = (int)command.Parameters["@count"].Value;
                        result.CurrentPage = (int)command.Parameters["@currentpage"].Value;
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
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForSchedule(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"SET @currentpage = @page;
                                                    SELECT @count = COUNT(*)
                                                    FROM [ScheduleAction]
                                                    WHERE [name] LIKE @name
                                                          AND [groupid] IS NULL;
                                                    IF @pagesize * @page >= @count
                                                    BEGIN
                                                        SET @currentpage = @count / @pagesize;
                                                        IF @count % @pagesize <> 0
                                                        BEGIN
                                                            SET @currentpage = @currentpage + 1;
                                                        END;
                                                        IF @currentpage = 0
                                                            SET @currentpage = 1;
                                                    END;
                                                    ELSE IF @page < 1
                                                    BEGIN
                                                        SET @currentpage = 1;
                                                    END;

                                                    SELECT {0}
                                                    FROM [ScheduleAction]
                                                    WHERE [name] LIKE @name
                                                          AND [groupid] IS NULL
                                                    ORDER BY sequence OFFSET (@pagesize * (@currentpage - 1)) ROWS FETCH NEXT @pagesize ROWS ONLY;",
                                                    StoreHelper.GetScheduleActionStoreSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {
                    var parameter = new SqlParameter("@page", SqlDbType.Int)
                    {
                        Value = page
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@pagesize", SqlDbType.Int)
                    {
                        Value = pageSize
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@count", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@currentpage", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@name", SqlDbType.VarChar, 200)
                    {
                        Value = $"{name.ToSqlLike()}%"
                    };
                    command.Parameters.Add(parameter);
                    command.Prepare();
                    SqlDataReader reader = null;


                    using (reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var scheduleAction = new ScheduleAction();
                            StoreHelper.SetScheduleActionStoreSelectFields(scheduleAction, reader, string.Empty);
                            result.Results.Add(scheduleAction);
                        }
                        reader.Close();
                        result.TotalCount = (int)command.Parameters["@count"].Value;
                        result.CurrentPage = (int)command.Parameters["@currentpage"].Value;
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
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForSchedule(), async (conn, transaction) =>
            {
                long sequence = 0;
                int pageSize = 500;
                while (true)
                {
                    listAction.Clear();

                    SqlTransaction sqlTran = null;
                    if (transaction != null)
                    {
                        sqlTran = (SqlTransaction)transaction;
                    }

                    using (SqlCommand command = new SqlCommand()
                    {
                        Connection = (SqlConnection)conn,
                        CommandType = CommandType.Text,
                        CommandText = string.Format(@"SELECT top (@pagesize) {0} FROM [ScheduleAction] WHERE [groupid]=@groupid AND [status]=@status and sequence>@sequence ORDER BY sequence", StoreHelper.GetScheduleActionStoreSelectFields(string.Empty)),
                        Transaction = sqlTran
                    })
                    {
                        var parameter = new SqlParameter("@groupId", SqlDbType.UniqueIdentifier)
                        {
                            Value = groupId
                        };
                        command.Parameters.Add(parameter);
                        parameter = new SqlParameter("@status", SqlDbType.Int)
                        {
                            Value = status
                        };
                        command.Parameters.Add(parameter);
                        parameter = new SqlParameter("@pagesize", SqlDbType.Int)
                        {
                            Value = pageSize
                        };
                        command.Parameters.Add(parameter);
                        parameter = new SqlParameter("@sequence", SqlDbType.Int)
                        {
                            Value = sequence
                        };
                        command.Parameters.Add(parameter);
                        command.Prepare();
                        SqlDataReader reader = null;


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