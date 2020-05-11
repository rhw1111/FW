using MSLibrary.DAL;
using MSLibrary.DI;
using MSLibrary.Transaction;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Schedule.DAL
{
    [Injection(InterfaceType = typeof(IScheduleActionGroupStore), Scope = InjectionScope.Singleton)]
    public class ScheduleActionGroupStore : IScheduleActionGroupStore
    {
        private IScheduleConnectionFactory _dbConnectionFactory;

        public ScheduleActionGroupStore(IScheduleConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }


        public async Task Add(ScheduleActionGroup group)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForSchedule(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }
                await using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                })
                {
                    SqlParameter parameter;
                    if (group.ID == Guid.Empty)
                    {
                        command.CommandText = @"insert into [dbo].[ScheduleActionGroup] ([id] ,[name] ,[createtime] ,[modifytime] ,[uselog],[executeactioninittype],[executeactioninitconfiguration])
                                                values (default ,@name ,GETUTCDATE() ,GETUTCDATE() ,@uselog,@executeactioninittype,@executeactioninitconfiguration);
                                                select @newid =[id] from [dbo].[ScheduleActionGroup] where [sequence] = SCOPE_IDENTITY()";
                        parameter = new SqlParameter("@newid", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        command.CommandText = @"insert into [dbo].[ScheduleActionGroup] ([id] ,[name] ,[createtime] ,[modifytime] ,[uselog],[executeactioninittype],[executeactioninitconfiguration]) 
                                                values (@id ,@name ,GETUTCDATE() ,GETUTCDATE() ,@uselog)";
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = group.ID
                        };
                        command.Parameters.Add(parameter);
                    }

                    parameter = new SqlParameter("@name", SqlDbType.VarChar, 150)
                    {
                        Value = group.Name
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@uselog", SqlDbType.Bit)
                    {
                        Value = group.UseLog
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@executeactioninittype", SqlDbType.VarChar,150)
                    {
                        Value = group.ExecuteActionInitType
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@executeactioninitconfiguration", SqlDbType.NVarChar, group.ExecuteActionInitConfiguration.Length)
                    {
                        Value = group.ExecuteActionInitConfiguration
                    };
                    command.Parameters.Add(parameter);


                    await command.PrepareAsync();


                        try
                        {
                            await command.ExecuteNonQueryAsync();
                        }
                        catch (SqlException ex)
                        {
                            if (ex.Number == 2601)
                            {
                                var fragment = new TextFragment()
                                {
                                    Code = TextCodes.ExistScheduleActionGroupName,
                                    DefaultFormatting = "调度作业组已存在相同名称\"{0}\"数据",
                                    ReplaceParameters = new List<object>() { group.Name }
                                };

                                throw new UtilityException((int)Errors.ExistScheduleActionGroupName, fragment);
                            }

                            throw;
                        }


                    if (group.ID == Guid.Empty)
                    {
                        group.ID = (Guid)command.Parameters["@newid"].Value;
                    }
                }
            });
        }

        public async Task Delete(Guid id)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForSchedule(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }
                await using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = @"delete from [dbo].[ScheduleActionGroup]
                                    where id=@id"
                })
                {
                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    await command.ExecuteNonQueryAsync();
                }
            });
        }

        public async Task<ScheduleActionGroup> QueryByID(Guid id)
        {
            ScheduleActionGroup result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForSchedule(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"select {0} from [dbo].[ScheduleActionGroup] where [id]=@id", StoreHelper.GetScheduleActionGroupSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {
                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();


                    SqlDataReader reader = null;

                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new ScheduleActionGroup();
                            StoreHelper.SetScheduleActionGroupSelectFields(result, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return result;
        }

        public async Task<ScheduleActionGroup> QueryByName(string name)
        {
            ScheduleActionGroup result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForSchedule(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"select {0} from [dbo].[ScheduleActionGroup] where [name] = @name", StoreHelper.GetScheduleActionGroupSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {
                    var parameter = new SqlParameter("@name", SqlDbType.VarChar, 150)
                    {
                        Value = name
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    SqlDataReader reader = null;

                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new ScheduleActionGroup();
                            StoreHelper.SetScheduleActionGroupSelectFields(result, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return result;
        }

        public async Task<QueryResult<ScheduleActionGroup>> QueryByPage(string name, int page, int pageSize)
        {
            QueryResult<ScheduleActionGroup> result = new QueryResult<ScheduleActionGroup>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForSchedule(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"SET @currentpage = @page;
                                                SELECT @count = COUNT(*)
                                                FROM [dbo].[ScheduleActionGroup]
                                                WHERE name like @name;
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
                                                FROM [dbo].[ScheduleActionGroup]
                                                WHERE name like @name
                                                ORDER BY sequence OFFSET (@pagesize * (@currentpage - 1)) ROWS FETCH NEXT @pagesize ROWS ONLY;",
                                                StoreHelper.GetScheduleActionGroupSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {
                    var parameter = new SqlParameter("@name", SqlDbType.VarChar, 200)
                    {
                        Value = string.Format("{0}%", name.ToSqlLike())
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@page", SqlDbType.Int)
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

                    await command.PrepareAsync();

                    SqlDataReader reader = null;

                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var scheduleActionGroup = new ScheduleActionGroup();
                            StoreHelper.SetScheduleActionGroupSelectFields(scheduleActionGroup, reader, string.Empty);

                            result.Results.Add(scheduleActionGroup);
                        }
                        await reader.CloseAsync();
                        result.TotalCount = (int)command.Parameters["@count"].Value;
                        result.CurrentPage = (int)command.Parameters["@currentpage"].Value;
                    }
                }
            });
            return result;
        }

        public async Task Update(ScheduleActionGroup group)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForSchedule(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }
                await using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = @"UPDATE [dbo].[ScheduleActionGroup]
                                    SET [name] = @name ,
                                        [modifytime]=GETUTCDATE() ,
                                        [uselog] = @uselog
                                    WHERE id=@id"
                })
                {
                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = group.ID
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@name", SqlDbType.VarChar, 150)
                    {
                        Value = group.Name
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@uselog", SqlDbType.Bit)
                    {
                        Value = group.UseLog
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    await command.ExecuteNonQueryAsync();

                }
            });
        }
    }
}