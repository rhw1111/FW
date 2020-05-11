using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using MSLibrary.DAL;
using MSLibrary.DI;
using MSLibrary.Transaction;

namespace MSLibrary.Workflow.DAL
{
    [Injection(InterfaceType = typeof(IWorkflowEntityStore), Scope = InjectionScope.Singleton)]
    public class WorkflowEntityStore : IWorkflowEntityStore
    {
        private readonly IWorkflowConnectionFactory _dbConnectionFactory;

        public WorkflowEntityStore(IWorkflowConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        /// <summary>
        /// 查询审核实体的字段信息
        /// </summary>
        /// <param name="entityName">审核实体名称</param>
        /// <param name="entityColumnName">审核实体字段名称</param>
        /// <param name="entityKey">审核实体主键</param>
        /// <returns>审核实体的字段信息</returns>
        public async Task<string> QueryAuditStatusByKey(string entityName, string entityColumnName, Dictionary<string, string> entityKey)
        {
            string result = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForWorkflow(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                var conditionStr = string.Empty;
                foreach (var item in entityKey)
                {
                    conditionStr = $"{conditionStr}{item.Key} = @{item.Key} AND ";
                }

                await using (var command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = $@"SELECT {entityColumnName} 
                                     FROM {entityName}
                                     WHERE {conditionStr.TrimEnd('A', 'N', 'D', ' ')} "
                })
                {
                    foreach (var item in entityKey)
                    {
                        var parameter = new SqlParameter($"@{item.Key}", SqlDbType.NVarChar, 1000)
                        {
                            Value = item.Value
                        };
                        command.Parameters.Add(parameter);
                    }

                    await command.PrepareAsync();


                    var executeObj = await command.ExecuteScalarAsync();
                    result = executeObj?.ToString();

                }
            });

            return result;
        }
    }
}
