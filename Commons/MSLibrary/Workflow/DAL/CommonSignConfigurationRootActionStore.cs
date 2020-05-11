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
    [Injection(InterfaceType = typeof(ICommonSignConfigurationRootActionStore), Scope = InjectionScope.Singleton)]
    public class CommonSignConfigurationRootActionStore : ICommonSignConfigurationRootActionStore
    {
        private readonly IWorkflowConnectionFactory _dbConnectionFactory;

        public CommonSignConfigurationRootActionStore(IWorkflowConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task QueryAll(Guid configurationId, Func<CommonSignConfigurationRootAction, Task> callback)
        {
            var resultList = new List<CommonSignConfigurationRootAction>();

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
                            command.CommandText = string.Format(@"SELECT TOP (@pagesize) {0},{1},{2},{3}
                                                                    FROM [CommonSignConfigurationRootAction] AS rootaction
                                                                    INNER JOIN [CommonSignConfiguration] AS config
                                                                        ON rootaction.configurationid = config.id
                                                                    LEFT JOIN [CommonSignConfigurationNode] AS node
                                                                        ON rootaction.entrynodeid = node.id
                                                                    LEFT JOIN [CommonSignConfiguration] AS nodeconfig
                                                                        ON nodeconfig.id = node.configurationid
                                                                    WHERE rootaction.configurationid = @configurationid
                                                                    ORDER BY rootaction.sequence;",
                                                                    StoreHelper.GetCommonSignConfigurationRootActionSelectFields("rootaction"),
                                                                    StoreHelper.GetCommonSignConfigurationSelectFields("config"),
                                                                    StoreHelper.GetCommonSignConfigurationNodeSelectFields("node"),
                                                                    StoreHelper.GetCommonSignConfigurationSelectFields("nodeconfig"));
                        }
                        else
                        {
                            command.CommandText = string.Format(@"SELECT TOP (@pagesize) {0},{1},{2},{3}
                                                                    FROM [CommonSignConfigurationRootAction] AS rootaction
                                                                            INNER JOIN [CommonSignConfiguration] AS config
                                                                                ON rootaction.configurationid = config.id
                                                                            LEFT JOIN [CommonSignConfigurationNode] AS node
                                                                                ON rootaction.entrynodeid = node.id
                                                                            LEFT JOIN [CommonSignConfiguration] AS nodeconfig
                                                                                ON nodeconfig.id = node.configurationid
                                                                    WHERE rootaction.configurationid = @configurationid
                                                                          AND rootaction.sequence > @sequence
                                                                    ORDER BY rootaction.sequence;",
                                                                    StoreHelper.GetCommonSignConfigurationRootActionSelectFields("rootaction"),
                                                                    StoreHelper.GetCommonSignConfigurationSelectFields("config"),
                                                                    StoreHelper.GetCommonSignConfigurationNodeSelectFields("node"),
                                                                    StoreHelper.GetCommonSignConfigurationSelectFields("nodeconfig"));
                        }

                        var parameter = new SqlParameter("@configurationid", SqlDbType.UniqueIdentifier)
                        {
                            Value = configurationId
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
                                var data = new CommonSignConfigurationRootAction();
                                StoreHelper.SetCommonSignConfigurationRootActionSelectFields(data, reader, "rootaction");
                                data.Configuration = new CommonSignConfiguration();
                                StoreHelper.SetCommonSignConfigurationSelectFields(data.Configuration, reader, "config");
                                if (reader[string.Format("{0}id", "node")] != DBNull.Value)
                                {
                                    data.EntryNode = new CommonSignConfigurationNode();
                                    StoreHelper.SetCommonSignConfigurationNodeSelectFields(data.EntryNode, reader, "node");
                                }
                                if (reader[string.Format("{0}id", "nodeconfig")] != DBNull.Value)
                                {
                                    data.EntryNode.Configuration = new CommonSignConfiguration();
                                    StoreHelper.SetCommonSignConfigurationSelectFields(data.EntryNode.Configuration, reader, "nodeconfig");
                                }
                                sequence = (long)reader["rootactionsequence"];
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

        public async Task<CommonSignConfigurationRootAction> QueryByActionName(Guid configurationId, string actionName)
        {
            CommonSignConfigurationRootAction result = null;
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
                    CommandText = string.Format(@"SELECT {0},{1},{2},{3}
                                                    FROM [CommonSignConfigurationRootAction] AS rootaction
                                                        INNER JOIN [CommonSignConfiguration] AS config
                                                            ON rootaction.configurationid = config.id
                                                        LEFT JOIN [CommonSignConfigurationNode] AS node
                                                            ON rootaction.entrynodeid = node.id
                                                        LEFT JOIN [CommonSignConfiguration] AS nodeconfig
                                                            ON nodeconfig.id = node.configurationid
                                                    WHERE rootaction.configurationid = @configurationid
                                                          AND rootaction.actionname = @actionname
                                                    ORDER BY rootaction.sequence DESC; ",
                                                    StoreHelper.GetCommonSignConfigurationRootActionSelectFields("rootaction"),
                                                    StoreHelper.GetCommonSignConfigurationSelectFields("config"),
                                                    StoreHelper.GetCommonSignConfigurationNodeSelectFields("node"),
                                                    StoreHelper.GetCommonSignConfigurationSelectFields("nodeconfig"))
                })
                {
                    var parameter = new SqlParameter("@configurationid", SqlDbType.UniqueIdentifier)
                    {
                        Value = configurationId
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
                            result = new CommonSignConfigurationRootAction();
                            StoreHelper.SetCommonSignConfigurationRootActionSelectFields(result, reader, "rootaction");
                            result.Configuration = new CommonSignConfiguration();
                            StoreHelper.SetCommonSignConfigurationSelectFields(result.Configuration, reader, "config");
                            if (reader[string.Format("{0}id", "node")] != DBNull.Value)
                            {
                                result.EntryNode = new CommonSignConfigurationNode();
                                StoreHelper.SetCommonSignConfigurationNodeSelectFields(result.EntryNode, reader, "node");
                            }
                            if (reader[string.Format("{0}id", "nodeconfig")] != DBNull.Value)
                            {
                                result.EntryNode.Configuration = new CommonSignConfiguration();
                                StoreHelper.SetCommonSignConfigurationSelectFields(result.EntryNode.Configuration, reader, "nodeconfig");
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
