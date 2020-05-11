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
    [Injection(InterfaceType = typeof(ICommonSignConfigurationStore), Scope = InjectionScope.Singleton)]
    public class CommonSignConfigurationStore : ICommonSignConfigurationStore
    {
        private readonly IWorkflowConnectionFactory _dbConnectionFactory;

        public CommonSignConfigurationStore(IWorkflowConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task Lock(string lockName, Func<Task> callBack, int timeout = -1)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForWorkflow(), async (conn, transaction) =>
            {
                await SqlServerApplicationLockHelper.Execute((SqlConnection)conn, (SqlTransaction)transaction, lockName, callBack, timeout);

            });
        }

        public async Task QueryByEntityType(string entityType, Func<CommonSignConfiguration, Task> callback)
        {
            var configurationList = new List<CommonSignConfiguration>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForWorkflow(), async (conn, transaction) =>
            {
                long? sequence = null;
                const int pageSize = 1000;

                while (true)
                {
                    configurationList.Clear();

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
                    })
                    {
                        if (!sequence.HasValue)
                        {
                            command.CommandText = string.Format(@"SELECT TOP (@pagesize) {0}                                                                    
                                                                FROM [CommonSignConfiguration]
                                                                WHERE [entitytype] = @entitytype 
                                                                ORDER BY [sequence]", StoreHelper.GetCommonSignConfigurationSelectFields(string.Empty));
                        }
                        else
                        {
                            command.CommandText = string.Format(@"SELECT TOP (@pagesize) {0}                                                                    
                                                                FROM [CommonSignConfiguration]
                                                                WHERE [entitytype] = @entitytype
                                                                      AND [sequence] > @sequence
                                                                ORDER BY [sequence];", StoreHelper.GetCommonSignConfigurationSelectFields(string.Empty));
                        }

                        var parameter = new SqlParameter("@entitytype", SqlDbType.NVarChar, 500)
                        {
                            Value = entityType
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter("@pagesize", SqlDbType.Int)
                        {
                            Value = pageSize
                        };
                        command.Parameters.Add(parameter);

                        if (sequence.HasValue)
                        {
                            parameter = new SqlParameter("@sequence", SqlDbType.BigInt)
                            {
                                Value = sequence
                            };
                            command.Parameters.Add(parameter);
                        }

                        await command.PrepareAsync();

                        SqlDataReader reader = null;

                        await using (reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var date = new CommonSignConfiguration();
                                StoreHelper.SetCommonSignConfigurationSelectFields(date, reader, string.Empty);
                                sequence = (long)reader["sequence"];
                                configurationList.Add(date);
                            }
                            await reader.CloseAsync();
                        }
                    }

                    foreach (var workflowStep in configurationList)
                    {
                        await callback(workflowStep);
                    }

                    if (configurationList.Count != pageSize)
                    {
                        break;
                    }
                }
            });
        }

        public async Task<CommonSignConfiguration> QueryByWorkflowResourceType(string workflowResourceType)
        {
            CommonSignConfiguration result = null;
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
                    CommandText = string.Format(@"SELECT {0}
                                                FROM [CommonSignConfiguration]
                                                WHERE [workflowresourcetype] = @workflowresourcetype
                                                ORDER BY [sequence] DESC; ", StoreHelper.GetCommonSignConfigurationSelectFields(string.Empty))
                })
                {
                    var parameter = new SqlParameter("@workflowresourcetype", SqlDbType.NVarChar, 500)
                    {
                        Value = workflowResourceType
                    };
                    command.Parameters.Add(parameter);
                    await  command.PrepareAsync();

                    SqlDataReader reader = null;

                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new CommonSignConfiguration();
                            StoreHelper.SetCommonSignConfigurationSelectFields(result, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return result;
        }
    }
}
