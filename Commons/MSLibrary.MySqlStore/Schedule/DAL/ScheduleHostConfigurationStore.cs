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
using System.Threading;

namespace MSLibrary.MySqlStore.Schedule.DAL
{
    [Injection(InterfaceType = typeof(IScheduleHostConfigurationStore), Scope = InjectionScope.Singleton)]
    public class ScheduleHostConfigurationStore : IScheduleHostConfigurationStore
    {
        private IScheduleConnectionFactory _dbConnectionFactory;

        public ScheduleHostConfigurationStore(IScheduleConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<ScheduleHostConfiguration> QueryByName(string name, CancellationToken cancellationToken)
        {
            ScheduleHostConfiguration result = null;
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
                    CommandText = string.Format(@"select {0} from schedulehostconfiguration where name = @name", StoreHelper.GetScheduleHostConfigurationSelectFields(string.Empty)),
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
                            result = new ScheduleHostConfiguration();
                            StoreHelper.SetScheduleHostConfigurationSelectFields(result, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return result;
        }
    }
}
