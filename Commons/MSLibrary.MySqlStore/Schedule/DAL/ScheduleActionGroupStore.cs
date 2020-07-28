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
using MSLibrary.Schedule;
using MSLibrary.Schedule.DAL;

namespace MSLibrary.MySqlStore.Schedule.DAL
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
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _dbConnectionFactory.CreateAllForSchedule(), async (conn, transaction) =>
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
                    Transaction = sqlTran,
                })
                {
                    if (group.ID==Guid.Empty)
                    {
                        group.ID = Guid.NewGuid();
                    }

                    MySqlParameter parameter;

                        command.CommandText = @"insert into scheduleactiongroup (id ,name ,createtime ,modifytime ,uselog,executeactioninittype,executeactioninitconfiguration) 
                                                values (@id ,@name ,utc_timestamp() ,utc_timestamp() ,@uselog)";
                        parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                        {
                            Value = group.ID
                        };
                        command.Parameters.Add(parameter);
                    

                    parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 150)
                    {
                        Value = group.Name
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@uselog", MySqlDbType.Bit)
                    {
                        Value = group.UseLog
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@executeactioninittype", MySqlDbType.VarChar, 150)
                    {
                        Value = group.ExecuteActionInitType
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@executeactioninitconfiguration", MySqlDbType.VarChar, group.ExecuteActionInitConfiguration.Length)
                    {
                        Value = group.ExecuteActionInitConfiguration
                    };
                    command.Parameters.Add(parameter);


                    await command.PrepareAsync();


                    //try
                    //{
                        await command.ExecuteNonQueryAsync();
                    //}
                    //catch (SqlException ex)
                    //{
                    //    if (ex.Number == 2601)
                    //    {
                    //        var fragment = new TextFragment()
                    //        {
                    //            Code = TextCodes.ExistScheduleActionGroupName,
                    //            DefaultFormatting = "调度作业组已存在相同名称\"{0}\"数据",
                    //            ReplaceParameters = new List<object>() { group.Name }
                    //        };

                    //        throw new UtilityException((int)Errors.ExistScheduleActionGroupName, fragment);
                    //    }

                    //    throw;
                    //}
                }
            });
        }

        public async Task Delete(Guid id)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _dbConnectionFactory.CreateAllForSchedule(), async (conn, transaction) =>
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
                    Transaction = sqlTran,
                    CommandText = @"delete from scheduleactiongroup
                                    where id=@id"
                })
                {
                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
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
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _dbConnectionFactory.CreateReadForSchedule(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"select {0} from scheduleactiongroup where id=@id", StoreHelper.GetScheduleActionGroupSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {
                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();


                    DbDataReader reader = null;

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
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _dbConnectionFactory.CreateReadForSchedule(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"select {0} from scheduleactiongroup where name = @name", StoreHelper.GetScheduleActionGroupSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {
                    var parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 150)
                    {
                        Value = name
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    DbDataReader reader = null;

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
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _dbConnectionFactory.CreateReadForSchedule(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"
                                                SELECT COUNT(*)
                                                FROM scheduleactiongroup
                                                WHERE name like @name;

                                                select {0}
                                                from scheduleactiongroup
                                                where sequence in
                                                (
                                                select t.sequence from
                                                (
                                                    select sequence
                                                    from scheduleactiongroup
                                                    where name like @name
                                                    order by sequence limit {1}, {2}                                                   
                                                ) as t
                                                )
                                                ",
                                                StoreHelper.GetScheduleActionGroupSelectFields(string.Empty),(page-1)*pageSize,pageSize),
                    Transaction = sqlTran
                })
                {
                    var parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 200)
                    {
                        Value = string.Format("{0}%", name.ToMySqlLike())
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@page", MySqlDbType.Int32)
                    {
                        Value = page
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@pagesize", MySqlDbType.Int32)
                    {
                        Value = pageSize
                    };
                    command.Parameters.Add(parameter);


                    await command.PrepareAsync();

                    DbDataReader reader = null;

                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result.TotalCount = reader.GetInt32(0);

                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    var scheduleActionGroup = new ScheduleActionGroup();
                                    StoreHelper.SetScheduleActionGroupSelectFields(scheduleActionGroup, reader, string.Empty);

                                    result.Results.Add(scheduleActionGroup);
                                }
                            }
                        }

                        await reader.CloseAsync();
                    }
                }
            });
            return result;
        }

        public async Task Update(ScheduleActionGroup group)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _dbConnectionFactory.CreateAllForSchedule(), async (conn, transaction) =>
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
                    Transaction = sqlTran,
                    CommandText = @"UPDATE scheduleactiongroup
                                    SET name = @name ,
                                        modifytime=utc_timestamp(),
                                        uselog = @uselog
                                    WHERE id=@id"
                })
                {
                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = group.ID
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 150)
                    {
                        Value = group.Name
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@uselog", MySqlDbType.Bit)
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
