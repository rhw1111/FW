using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Data;
using Microsoft.Data.SqlClient;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Transaction;

namespace MSLibrary.Schedule.DAL
{
    [Injection(InterfaceType = typeof(IScheduleHostConfigurationStore), Scope = InjectionScope.Singleton)]
    public class ScheduleHostConfigurationStore : IScheduleHostConfigurationStore
    {
        private readonly IScheduleConnectionFactory _scheduleConnectionFactory;

        public ScheduleHostConfigurationStore(IScheduleConnectionFactory scheduleConnectionFactory)
        {
            _scheduleConnectionFactory = scheduleConnectionFactory;
        }
        public async Task<ScheduleHostConfiguration> QueryByName(string name, CancellationToken cancellationToken)
        {
            ScheduleHostConfiguration result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _scheduleConnectionFactory.CreateReadForSchedule(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"select {0} from [dbo].[ScheduleHostConfiguration] where [name] = @name", StoreHelper.GetScheduleHostConfigurationSelectFields(string.Empty)),
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
