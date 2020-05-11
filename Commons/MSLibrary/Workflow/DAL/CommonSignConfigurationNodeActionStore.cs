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
    [Injection(InterfaceType = typeof(ICommonSignConfigurationNodeActionStore), Scope = InjectionScope.Singleton)]
    public class CommonSignConfigurationNodeActionStore : ICommonSignConfigurationNodeActionStore
    {
        private readonly IWorkflowConnectionFactory _dbConnectionFactory;

        public CommonSignConfigurationNodeActionStore(IWorkflowConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }


        public async Task QueryByNode(Guid nodeId, Func<CommonSignConfigurationNodeAction, Task> callback)
        {
            var resultList = new List<CommonSignConfigurationNodeAction>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForWorkflow(), async (conn, transaction) =>
            {
                long? sequence = null;
                const int pageSize = 1000;

                while (true)
                {
                    resultList.Clear();

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
                            command.CommandText = string.Format(@"SELECT TOP (@pagesize) {0},{1},{2}
                                                                    FROM [CommonSignConfigurationNodeAction] AS nodeaction
                                                                        INNER JOIN [CommonSignConfigurationNode] AS cnode
                                                                            ON nodeaction.nodeid = cnode.id
                                                                        LEFT JOIN [CommonSignConfiguration] AS nodeconfig
                                                                            ON nodeconfig.id = cnode.configurationid
                                                                    WHERE nodeaction.[nodeid] = @nodeid
                                                                    ORDER BY nodeaction.[sequence];",
                                                                    StoreHelper.GetCommonSignConfigurationNodeActionSelectFields("nodeaction"),
                                                                    StoreHelper.GetCommonSignConfigurationNodeSelectFields("cnode"),
                                                                    StoreHelper.GetCommonSignConfigurationSelectFields("nodeconfig"));
                        }
                        else
                        {
                            command.CommandText = string.Format(@"SELECT TOP (@pagesize) {0},{1},{2}
                                                                    FROM [CommonSignConfigurationNodeAction] AS nodeaction
                                                                        INNER JOIN [CommonSignConfigurationNode] AS cnode
                                                                            ON nodeaction.nodeid = cnode.id
                                                                        LEFT JOIN [CommonSignConfiguration] AS nodeconfig
                                                                            ON nodeconfig.id = cnode.configurationid
                                                                    WHERE nodeaction.[nodeid] = @nodeid
                                                                          AND nodeaction.[sequence] > @sequence
                                                                    ORDER BY nodeaction.[sequence];",
                                                                    StoreHelper.GetCommonSignConfigurationNodeActionSelectFields("nodeaction"),
                                                                    StoreHelper.GetCommonSignConfigurationNodeSelectFields("cnode"),
                                                                    StoreHelper.GetCommonSignConfigurationSelectFields("nodeconfig"));
                        }

                        var parameter = new SqlParameter("@nodeid", SqlDbType.UniqueIdentifier)
                        {
                            Value = nodeId
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
                                var data = new CommonSignConfigurationNodeAction();
                                StoreHelper.SetCommonSignConfigurationNodeActionSelectFields(data, reader, "nodeaction");
                                if (reader[string.Format("{0}id", "cnode")] != DBNull.Value)
                                {
                                    data.Node = new CommonSignConfigurationNode();
                                    StoreHelper.SetCommonSignConfigurationNodeSelectFields(data.Node, reader, "cnode");
                                }

                                if (reader[string.Format("{0}id", "nodeconfig")] != DBNull.Value)
                                {
                                    data.Node.Configuration = new CommonSignConfiguration();
                                    StoreHelper.SetCommonSignConfigurationSelectFields(data.Node.Configuration, reader, "nodeconfig");
                                }
                                sequence = (long)reader["nodeactionsequence"];
                                resultList.Add(data);
                            }
                            await reader.CloseAsync();
                        }
                    }

                    foreach (var workflowStep in resultList)
                    {
                        await callback(workflowStep);
                    }

                    if (resultList.Count != pageSize)
                    {
                        break;
                    }
                }
            });
        }

        public async Task<CommonSignConfigurationNodeAction> QueryByNodeAndActionName(Guid nodeId, string actionName)
        {
            CommonSignConfigurationNodeAction result = null;
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
                    CommandText = string.Format(@"SELECT {0},{1},{2}
                                                    FROM [CommonSignConfigurationNodeAction] AS nodeaction
                                                                        INNER JOIN [CommonSignConfigurationNode] AS cnode
                                                                            ON nodeaction.nodeid = cnode.id
                                                                        LEFT JOIN [CommonSignConfiguration] AS nodeconfig
                                                                            ON nodeconfig.id = cnode.configurationid
                                                    WHERE nodeaction.[nodeid] = @nodeid
                                                          AND nodeaction.actionname = @actionname 
                                                    ORDER BY nodeaction.[sequence] DESC; ",
                                                    StoreHelper.GetCommonSignConfigurationNodeActionSelectFields("nodeaction"),
                                                    StoreHelper.GetCommonSignConfigurationNodeSelectFields("cnode"),
                                                    StoreHelper.GetCommonSignConfigurationSelectFields("nodeconfig"))
                })
                {
                    var parameter = new SqlParameter("@nodeid", SqlDbType.UniqueIdentifier)
                    {
                        Value = nodeId
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@actionname", SqlDbType.NVarChar, 500)
                    {
                        Value = actionName
                    };
                    command.Parameters.Add(parameter);
                    await command.PrepareAsync();

                    SqlDataReader reader = null;

                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new CommonSignConfigurationNodeAction();
                            StoreHelper.SetCommonSignConfigurationNodeActionSelectFields(result, reader, "nodeaction");
                            if (reader[string.Format("{0}id", "cnode")] != DBNull.Value)
                            {
                                result.Node = new CommonSignConfigurationNode();
                                StoreHelper.SetCommonSignConfigurationNodeSelectFields(result.Node, reader, "cnode");
                            }
                            if (reader[string.Format("{0}id", "nodeconfig")] != DBNull.Value)
                            {
                                result.Node.Configuration = new CommonSignConfiguration();
                                StoreHelper.SetCommonSignConfigurationSelectFields(result.Node.Configuration, reader, "nodeconfig");
                            }
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return result;
        }
    }
}
