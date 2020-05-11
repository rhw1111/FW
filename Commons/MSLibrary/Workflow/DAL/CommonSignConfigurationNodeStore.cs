using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DAL;
using MSLibrary.DI;
using MSLibrary.Transaction;

namespace MSLibrary.Workflow.DAL
{
    [Injection(InterfaceType = typeof(ICommonSignConfigurationNodeStore), Scope = InjectionScope.Singleton)]
    public class CommonSignConfigurationNodeStore : ICommonSignConfigurationNodeStore
    {
        private readonly IWorkflowConnectionFactory _dbConnectionFactory;

        public CommonSignConfigurationNodeStore(IWorkflowConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<CommonSignConfigurationNode> QueryByConfigurationName(Guid configurationId, string name)
        {
            CommonSignConfigurationNode result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForWorkflow(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (var command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = string.Format(@"SELECT {0},{1}
                                                    FROM [CommonSignConfigurationNode] AS cnode
                                                        INNER JOIN [CommonSignConfiguration] AS config
                                                            ON cnode.[configurationid] = config.[id]
                                                    WHERE cnode.[configurationid] = @configurationid
                                                          AND cnode.[name] = @name
                                                    ORDER BY cnode.sequence DESC; ",
                                                    StoreHelper.GetCommonSignConfigurationNodeSelectFields("cnode"),
                                                    StoreHelper.GetCommonSignConfigurationSelectFields("config"))
                })
                {
                    var parameter = new SqlParameter("@configurationid", SqlDbType.UniqueIdentifier)
                    {
                        Value = configurationId
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 500)
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
                            result = new CommonSignConfigurationNode();
                            StoreHelper.SetCommonSignConfigurationNodeSelectFields(result, reader, "cnode");
                            result.Configuration = new CommonSignConfiguration();
                            StoreHelper.SetCommonSignConfigurationSelectFields(result.Configuration, reader, "config");
                        }
                        await reader.CloseAsync();
                    }
                }
            });

            return result;
        }


        public async Task Lock(string lockName, Func<Task> callBack, int timeout = -1)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForWorkflow(), async (conn, transaction) =>
            {
                await SqlServerApplicationLockHelper.Execute((SqlConnection)conn, (SqlTransaction)transaction, lockName, callBack, timeout);

            });
        }

    }
}
