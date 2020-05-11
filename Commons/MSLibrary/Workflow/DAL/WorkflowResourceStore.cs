using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using MSLibrary.Collections.Hash;
using MSLibrary.DAL;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Transaction;

namespace MSLibrary.Workflow.DAL
{
    [Injection(InterfaceType = typeof(IWorkflowResourceStore), Scope = InjectionScope.Singleton)]
    public class WorkflowResourceStore : IWorkflowResourceStore
    {
        private static string _hashGroupName;
        /// <summary>
        /// 需要用到的一致性哈希组的名称
        /// 需要在系统初始化时赋值
        /// </summary>
        public static string HashGroupName
        {
            set
            {
                _hashGroupName = value;
            }
        }


        private readonly IWorkflowConnectionFactory _dbConnectionFactory;
        private IHashGroupRepositoryCacheProxy _hashGroupRepository;
        private IStoreInfoResolveService _storeInfoResolveService;

        public WorkflowResourceStore(IWorkflowConnectionFactory dbConnectionFactory, IHashGroupRepositoryCacheProxy hashGroupRepository, IStoreInfoResolveService storeInfoResolveService)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _hashGroupRepository = hashGroupRepository;
            _storeInfoResolveService = storeInfoResolveService;
        }

        public async Task Add(WorkflowResource resource)
        {

            var dbInfo = await StoreInfoHelper.GetHashStoreInfo( _storeInfoResolveService,_hashGroupRepository, _hashGroupName, resource.Type, resource.Key);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowResource, out string tableName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowResource }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo,fragment);
            }

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, dbInfo.DBConnectionNames.ReadAndWrite, async (conn, transaction) =>
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
                })
                {
                    SqlParameter parameter;
                    if (resource.ID == Guid.Empty)
                    {
                        command.CommandText = string.Format(@"INSERT INTO {0}
                                                       ([id], [type], [key], [status], [initstatus], [createtime], [modifytime])
                                                VALUES (default, @type, @key, @status, @initstatus, GETUTCDATE(), GETUTCDATE());
                                                SELECT @newid = [id] FROM {0} WHERE [sequence] = SCOPE_IDENTITY()", tableName);

                        parameter = new SqlParameter("@newid", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        command.CommandText = string.Format(@"INSERT INTO {0}
                                                       ([id], [type], [key], [status], [initstatus], [createtime], [modifytime])
                                                VALUES (@id, @type, @key, @status, @initstatus, GETUTCDATE(), GETUTCDATE())", tableName);

                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = resource.ID
                        };
                        command.Parameters.Add(parameter);

                    }

                    parameter = new SqlParameter("@type", SqlDbType.NVarChar, 256)
                    {
                        Value = resource.Type
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@key", SqlDbType.NVarChar, 256)
                    {
                        Value = resource.Key
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@status", SqlDbType.Int)
                    {
                        Value = resource.Status
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@initstatus", SqlDbType.Int)
                    {
                        Value = resource.InitStatus
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
                                    Code = TextCodes.ExistWorkflowResourceKey,
                                    DefaultFormatting = "工作流资源已存在相同关键字\"{0},{1}\"数据",
                                    ReplaceParameters = new List<object>() { resource.Type, resource.Key }
                                };

                                throw new UtilityException((int)Errors.ExistWorkflowResourceKey, fragment);
                            }

                            throw;
                        }
        

                    if (resource.ID == Guid.Empty)
                    {
                        resource.ID = (Guid)command.Parameters["@newid"].Value;
                    }
                }

            });
        }

        public async Task Delete(string resourceType, string resourceKey, Guid id)
        {
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo(_storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowResource, out string tableName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowResource }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, dbInfo.DBConnectionNames.ReadAndWrite, async (conn, transaction) =>
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
                    CommandText = string.Format(@"DELETE FROM {0} WHERE [id] = @id;", tableName)
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

        public async Task UpdateStatus(string resourceType, string resourceKey, Guid id, int status)
        {
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo( _storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowResource, out string tableName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowResource }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, dbInfo.DBConnectionNames.ReadAndWrite, async (conn, transaction) =>
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
                    CommandText = string.Format(@"UPDATE {0}
                                                         SET [status] = @status
                                                  WHERE  [id] = @id", tableName)
                })
                {
                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@status", SqlDbType.Int)
                    {
                        Value = status
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();


                    await command.ExecuteNonQueryAsync();

                }
            });
        }

        /// <summary>
        /// 根据type和Key查询
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<WorkflowResource> QueryByKey(string type, string key)
        {
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo(_storeInfoResolveService,_hashGroupRepository, _hashGroupName, type, key);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowResource, out string tableName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowResource }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo,fragment);
            }

            WorkflowResource result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, dbInfo.DBConnectionNames.Read, async (conn, transaction) =>
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
                                                  FROM {1}
                                                  WHERE [type] = @type
                                                        AND [key] = @key
                                                  ORDER BY createtime DESC ", StoreHelper.GetWorkflowResourceStoreSelectFields(string.Empty),
                                                                              tableName)
                })
                {
                    var parameter = new SqlParameter("@type", SqlDbType.NVarChar, 256)
                    {
                        Value = type
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@key", SqlDbType.NVarChar, 256)
                    {
                        Value = key
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    SqlDataReader reader = null;

                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new WorkflowResource();
                            StoreHelper.SetWorkflowResourceStoreSelectFields(result, reader, string.Empty);
                        }

                        reader.Close();
                    }
                }
            });

            return result;
        }


    }
}
