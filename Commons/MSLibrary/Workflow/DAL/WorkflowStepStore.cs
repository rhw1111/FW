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
    [Injection(InterfaceType = typeof(IWorkflowStepStore), Scope = InjectionScope.Singleton)]
    public class WorkflowStepStore : IWorkflowStepStore
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

        private readonly IWorkflowConnectionFactory _workflowConnectionFactory;
        private IHashGroupRepositoryCacheProxy _hashGroupRepository;
        private IStoreInfoResolveService _storeInfoResolveService;


        public WorkflowStepStore(IWorkflowConnectionFactory dbConnectionMainFactory, IHashGroupRepositoryCacheProxy hashGroupRepository, IStoreInfoResolveService storeInfoResolveService)
        {
            _workflowConnectionFactory = dbConnectionMainFactory;
            _hashGroupRepository = hashGroupRepository;
            _storeInfoResolveService = storeInfoResolveService;
        }

        public async Task Add(string resourceType, string resourceKey, WorkflowStep step)
        {

            var dbInfo = await StoreInfoHelper.GetHashStoreInfo (_storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStep, out string tableName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() {  _hashGroupName, HashEntityNames.WorkflowStep}
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo
                    ,fragment);
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
                    Transaction = sqlTran
                })
                {
                    SqlParameter parameter;
                    if (step.ID == Guid.Empty)
                    {
                        command.CommandText = string.Format(@"INSERT INTO {0} ([id],[resourceid],[actionname],[status],[serialno],[usertype],[userkey],[usercount],[complete],[createtime],[modifytime]) 
                                                                          VALUES (default,@resourceid,@actionname,@status,@serialno,@usertype,@userkey,@usercount,@complete,getutcdate(),getutcdate()); 
                                                SELECT @newid=[id] FROM {0} WHERE [sequence]=SCOPE_IDENTITY()", tableName);

                        parameter = new SqlParameter("@newid", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        command.CommandText = string.Format(@"INSERT INTO {0} ([id],[resourceid],[actionname],[status],[serialno],[usertype],[userkey],[usercount],[complete],[createtime],[modifytime]) 
                                                                          VALUES (@id,@resourceid,@actionname,@status,@serialno,@usertype,@userkey,@usercount,@complete,getutcdate(),getutcdate())", tableName);

                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = step.ID
                        };
                        command.Parameters.Add(parameter);
                    }

                    parameter = new SqlParameter("@resourceid", SqlDbType.UniqueIdentifier)
                    {
                        Value = step.ResourceID
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@actionname", SqlDbType.NVarChar, 256)
                    {
                        Value = step.ActionName
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@status", SqlDbType.Int)
                    {
                        Value = step.Status
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@serialno", SqlDbType.NVarChar, 256)
                    {
                        Value = step.SerialNo
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@usertype", SqlDbType.NVarChar, 256)
                    {
                        Value = step.UserType
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@userkey", SqlDbType.NVarChar, 256)
                    {
                        Value = step.UserKey
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@usercount", SqlDbType.Int)
                    {
                        Value = step.UserCount
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@complete", SqlDbType.Bit)
                    {
                        Value = step.Complete
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
                                    Code = TextCodes.ExistWorkflowStepKey,
                                    DefaultFormatting = "工作流步骤已存在相同关键字\"{0},{1},{2},{3},{4}\"数据",
                                    ReplaceParameters = new List<object>() { step.ResourceID, step.ActionName, step.Status, step.UserType, step.UserKey }
                                };

                                throw new UtilityException((int)Errors.ExistWorkflowStepKey, fragment);
                            }

                            throw;
                        }
        

                    if (step.ID == Guid.Empty)
                    {
                        step.ID = (Guid)command.Parameters["@newid"].Value;
                    }
                }
            });
        }

        public async Task Delete(string resourceType, string resourceKey, Guid resourceId, Guid id)
        {
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo( _storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStep, out string tableName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowStep }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo
                    , fragment);
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
                    CommandText = string.Format("DELETE FROM {0} WHERE id = @id AND resourceid = @resourceid", tableName)
                })
                {
                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@resourceid", SqlDbType.UniqueIdentifier)
                    {
                        Value = resourceId
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();


                    await command.ExecuteNonQueryAsync();

                }
            });
        }

        public async Task Delete(string resourceType, string resourceKey, Guid resourceId, string actionName, int status)
        {
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo(_storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStep, out string tableName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowStep }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo
                    , fragment);
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
                    CommandText = string.Format("DELETE FROM {0} WHERE resourceid = @resourceid AND actionname = @actionname AND [status] = @status", tableName)
                })
                {
                    var parameter = new SqlParameter("@resourceid", SqlDbType.UniqueIdentifier)
                    {
                        Value = resourceId
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@actionname", SqlDbType.NVarChar, 256)
                    {
                        Value = actionName
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

        public async Task Delete(string resourceType, string resourceKey, Guid resourceId, int status, string serialNo, string excludeActionName)
        {
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo( _storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStep, out string tableName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowStep }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo
                    , fragment);
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
                    CommandText = string.Format(@"DELETE FROM {0}
                                    WHERE [resourceid] = @resourceid
                                          AND [status] = @status
                                          AND [serialno] = @serialno
                                          AND [actionname] <> @excludeActionName", tableName)
                })
                {
                    var parameter = new SqlParameter("@resourceid", SqlDbType.UniqueIdentifier)
                    {
                        Value = resourceId
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@status", SqlDbType.Int)
                    {
                        Value = status
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@serialno", SqlDbType.NVarChar, 256)
                    {
                        Value = status
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@excludeActionName", SqlDbType.NVarChar, 256)
                    {
                        Value = excludeActionName
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();


                    await command.ExecuteNonQueryAsync();

                }
            });
        }

        public async Task UpdateCompleteStatus(string resourceType, string resourceKey, Guid resourceId, string actionName, int status, bool completeStatus)
        {
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo( _storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStep, out string tableName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowStep }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo
                    , fragment);
            }

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, dbInfo.DBConnectionNames.ReadAndWrite, async (conn, transaction) =>
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
                    CommandText = string.Format(@"UPDATE {0} SET complete = @complete 
                                    WHERE resourceid = @resourceid AND actionname = @actionname AND [status] = @status", tableName)
                })
                {
                    var parameter = new SqlParameter("@complete", SqlDbType.Bit)
                    {
                        Value = completeStatus
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@resourceid", SqlDbType.UniqueIdentifier)
                    {
                        Value = resourceId
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@actionname", SqlDbType.NVarChar, 256)
                    {
                        Value = actionName
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

        public async Task UpdateCompleteStatus(string resourceType, string resourceKey, Guid resourceId, Guid id, bool completeStatus)
        {
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo( _storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStep, out string tableName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowStep }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo
                    , fragment);
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
                    CommandText = string.Format(@"UPDATE {0} SET complete = @complete 
                                    WHERE resourceid = @resourceid AND id = @id", tableName)
                })
                {
                    var parameter = new SqlParameter("@complete", SqlDbType.Bit)
                    {
                        Value = completeStatus
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@resourceid", SqlDbType.UniqueIdentifier)
                    {
                        Value = resourceId
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();


                    await command.ExecuteNonQueryAsync();

                }
            });
        }

        public async Task QueryByResource(string resourceType, string resourceKey, Guid resourceId, Func<WorkflowStep, Task> callback)
        {

            var dbInfo = await StoreInfoHelper.GetHashStoreInfo(_storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStep, out string tableNameStep))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowStep }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo
                    , fragment);
            }

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowResource, out string tableNameResource))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowResource }
                };
                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo
                    , fragment);
            }

            var workflowStepList = new List<WorkflowStep>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, dbInfo.DBConnectionNames.Read, async (conn, transaction) =>
            {
                long? sequence = null;
                const int pageSize = 1000;

                while (true)
                {
                    workflowStepList.Clear();

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
                            command.CommandText = string.Format(@"SELECT TOP (@pagesize) {0},{1}
                                                                  FROM {2} AS wfStep
                                                                       INNER JOIN {3} AS wfResource ON wfResource.id = wfStep.resourceid
                                                                  WHERE wfStep.resourceid = @resourceid
                                                                  ORDER BY wfStep.[sequence]", StoreHelper.GetWorkflowStepStoreSelectFields("wfStep"),
                                                                                               StoreHelper.GetWorkflowResourceStoreSelectFields("wfResource"), tableNameStep, tableNameResource);
                        }
                        else
                        {
                            command.CommandText = string.Format(@"SELECT TOP (@pagesize) {0},{1}
                                                                  FROM {2} AS wfStep
                                                                       INNER JOIN {3} AS wfResource ON wfResource.id = wfStep.resourceid
                                                                  WHERE wfStep.resourceid = @resourceid
                                                                        AND wfStep.[sequence] > @sequence
                                                                  ORDER BY wfStep.[sequence]", StoreHelper.GetWorkflowStepStoreSelectFields("wfStep"),
                                                                                               StoreHelper.GetWorkflowResourceStoreSelectFields("wfResource"), tableNameStep, tableNameResource);
                        }

                        var parameter = new SqlParameter("@resourceid", SqlDbType.UniqueIdentifier)
                        {
                            Value = resourceId
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
                                var workflowStep = new WorkflowStep();
                                StoreHelper.SetWorkflowStepStoreSelectFields(workflowStep, reader, "wfResource", "wfStep");
                                sequence = (long)reader["wfStepsequence"];
                                workflowStepList.Add(workflowStep);
                            }
                            await reader.CloseAsync();
                        }
                    }

                    foreach (var workflowStep in workflowStepList)
                    {
                        await callback(workflowStep);
                    }

                    if (workflowStepList.Count != pageSize)
                    {
                        break;
                    }
                }
            });
        }

        public async Task<WorkflowStep> QueryLatestByResource(string resourceType, string resourceKey, Guid resourceId, int status)
        {
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo(_storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStep, out string tableNameStep))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowStep }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo
                    , fragment);
            }

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowResource, out string tableNameResource))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowResource }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo
                    , fragment);
            }

            WorkflowStep result = null;
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
                    CommandText = string.Format(@"SELECT TOP 1 {0},{1}
                                                  FROM {2} AS wfStep
                                                       INNER JOIN {3} AS wfResource ON wfResource.id = wfStep.resourceid
                                                  WHERE wfStep.resourceid = @resourceid
                                                        AND wfStep.[status] = @status
                                                  ORDER BY wfStep.createtime DESC", StoreHelper.GetWorkflowStepStoreSelectFields("wfStep"),
                                                                                    StoreHelper.GetWorkflowResourceStoreSelectFields("wfResource"), tableNameStep, tableNameResource)
                })
                {
                    var parameter = new SqlParameter("@resourceid", SqlDbType.UniqueIdentifier)
                    {
                        Value = resourceId
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@status", SqlDbType.Int)
                    {
                        Value = status
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    SqlDataReader reader = null;

                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new WorkflowStep();
                            StoreHelper.SetWorkflowStepStoreSelectFields(result, reader, "wfResource", "wfStep");
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return result;
        }

        /// <summary>
        /// 指定步骤是否存在
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="resourceId"></param>
        /// <param name="status"></param>
        /// <param name="actionName"></param>
        /// <param name="userType"></param>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public async Task<bool> IsExistStepByKey(string resourceType, string resourceKey, Guid resourceId, int status, string actionName, string userType, string userKey)
        {
            var isExistStep = false;
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo( _storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStep, out string tableNameStep))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowStep }
                };

                throw new UtilityException((int) Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowResource, out string tableNameResource))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowResource }
                };

                throw new UtilityException((int) Errors.NotFoundKeyInHashNodeKeyInfo, 
                    fragment);
            }

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false,
                dbInfo.DBConnectionNames.Read,
                async (conn, transaction) =>
                {
                    SqlTransaction sqlTran = null;
                    if (transaction != null)
                    {
                        sqlTran = (SqlTransaction) transaction;
                    }

                    await using (var command = new SqlCommand()
                    {
                        Connection = (SqlConnection) conn,
                        CommandType = CommandType.Text,
                        Transaction = sqlTran,
                    })
                    {
                        command.CommandText = $@"SELECT count(1) AS total
                                                 FROM {tableNameStep} AS wfStep
                                                      INNER JOIN {tableNameResource} AS wfResource ON wfResource.id = wfStep.resourceid
                                                 WHERE wfStep.[resourceid] = @resourceid
                                                       AND wfStep.[status] = @status
                                                       AND wfStep.[actionname] = @actionname
                                                       AND wfStep.[usertype] = @usertype
                                                       AND wfStep.[userkey] = @userkey";

                        var parameter = new SqlParameter("@resourceid", SqlDbType.UniqueIdentifier)
                        {
                            Value = resourceId
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter("@status", SqlDbType.Int)
                        {
                            Value = status
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter("@actionname", SqlDbType.NVarChar, 256)
                        {
                            Value = actionName
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter("@usertype", SqlDbType.NVarChar, 256)
                        {
                            Value = userType
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter("@userkey", SqlDbType.NVarChar, 256)
                        {
                            Value = userKey
                        };
                        command.Parameters.Add(parameter);

                        await command.PrepareAsync();


                            isExistStep = (int) await command.ExecuteScalarAsync() > 0;
                     
                    }
                });

            return isExistStep;
        }
           
        public async Task QueryByResource(string resourceType, string resourceKey, Guid resourceId, string actionName, int status, Func<WorkflowStep, Task> callback)
        {
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo( _storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStep, out string tableNameStep))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowStep }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo
                    , fragment);
            }

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowResource, out string tableNameResource))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowResource }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo
                    , fragment);
            }


            var workflowStepList = new List<WorkflowStep>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, dbInfo.DBConnectionNames.Read, async (conn, transaction) =>
            {
                long? sequence = null;
                const int pageSize = 1000;

                while (true)
                {
                    workflowStepList.Clear();

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
                            command.CommandText = string.Format(@"SELECT TOP (@pageSize) {0},{1}
                                                                  FROM {2} AS wfStep
                                                                       INNER JOIN {3} AS wfResource ON wfResource.id = wfStep.resourceid
                                                                  WHERE wfStep.resourceid = @resourceid
                                                                        AND wfStep.[actionname] = @actionname
                                                                        AND wfStep.[status] = @status
                                                                  ORDER BY wfStep.[sequence]", StoreHelper.GetWorkflowStepStoreSelectFields("wfStep"),
                                                                                               StoreHelper.GetWorkflowResourceStoreSelectFields("wfResource"), tableNameStep, tableNameResource);
                        }
                        else
                        {
                            command.CommandText = string.Format(@"SELECT TOP (@pageSize) {0},{1}
                                                                  FROM {2} AS wfStep
                                                                       INNER JOIN {3} AS wfResource ON wfResource.id = wfStep.resourceid
                                                                  WHERE wfStep.resourceid = @resourceid
                                                                        AND wfStep.[actionname] = @actionname
                                                                        AND wfStep.[status] = @status
                                                                        AND wfStep.[sequence] > @sequence
                                                                  ORDER BY wfStep.[sequence]", StoreHelper.GetWorkflowStepStoreSelectFields("wfStep"),
                                                                                             StoreHelper.GetWorkflowResourceStoreSelectFields("wfResource"), tableNameStep, tableNameResource);
                        }

                        var parameter = new SqlParameter("@resourceid", SqlDbType.UniqueIdentifier)
                        {
                            Value = resourceId
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter("@actionname", SqlDbType.NVarChar, 256)
                        {
                            Value = actionName
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter("@status", SqlDbType.Int)
                        {
                            Value = status
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
                                var workflowStep = new WorkflowStep();
                                StoreHelper.SetWorkflowStepStoreSelectFields(workflowStep, reader, "wfResource", "wfStep");
                                sequence = (long)reader["wfStepsequence"];
                                workflowStepList.Add(workflowStep);
                            }
                            await reader.CloseAsync();
                        }
                    }

                    foreach (var workflowStep in workflowStepList)
                    {
                        await callback(workflowStep);
                    }

                    if (workflowStepList.Count != pageSize)
                    {
                        break;
                    }
                }
            });
        }

        public async Task QueryByResource(string resourceType, string resourceKey, Guid resourceId, string serialNo, Func<WorkflowStep, Task> callback)
        {
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo(_storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStep, out string tableNameStep))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowStep }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo
                    , fragment);
            }

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowResource, out string tableNameResource))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowResource }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo
                    , fragment);
            }

            var workflowStepList = new List<WorkflowStep>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, dbInfo.DBConnectionNames.Read, async (conn, transaction) =>
            {
                long? sequence = null;
                const int pageSize = 1000;

                while (true)
                {
                    workflowStepList.Clear();

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
                            command.CommandText = string.Format(@"SELECT TOP (@pageSize) {0},{1}
                                                                  FROM {2} AS wfStep
                                                                       INNER JOIN {3} AS wfResource ON wfResource.id = wfStep.resourceid
                                                                  WHERE wfStep.[resourceid] = @resourceid
                                                                        AND wfStep.[serialno] = @serialno
                                                                  ORDER BY wfStep.[sequence]", StoreHelper.GetWorkflowStepStoreSelectFields("wfStep"),
                                                                                             StoreHelper.GetWorkflowResourceStoreSelectFields("wfResource"), tableNameStep, tableNameResource);
                        }
                        else
                        {
                            command.CommandText = string.Format(@"SELECT TOP (@pageSize) {0},{1}
                                                                  FROM {2} AS wfStep
                                                                       INNER JOIN {3} AS wfResource ON wfResource.id = wfStep.resourceid
                                                                  WHERE wfStep.[resourceid] = @resourceid
                                                                        AND wfStep.[serialno] = @serialno
                                                                        AND wfStep.[sequence] > @sequence
                                                                  ORDER BY wfStep.[sequence]", StoreHelper.GetWorkflowStepStoreSelectFields("wfStep"),
                                                                                               StoreHelper.GetWorkflowResourceStoreSelectFields("wfResource"), tableNameStep, tableNameResource);
                        }

                        var parameter = new SqlParameter("@resourceid", SqlDbType.UniqueIdentifier)
                        {
                            Value = resourceId
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter("@serialno", SqlDbType.NVarChar, 256)
                        {
                            Value = serialNo
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
                                var workflowStep = new WorkflowStep();
                                StoreHelper.SetWorkflowStepStoreSelectFields(workflowStep, reader, "wfResource", "wfStep");
                                sequence = (long)reader["wfStepsequence"];
                                workflowStepList.Add(workflowStep);
                            }

                            await reader.CloseAsync();
                        }
                    }

                    foreach (var workflowStep in workflowStepList)
                    {
                        await callback(workflowStep);
                    }

                    if (workflowStepList.Count != pageSize)
                    {
                        break;
                    }
                }
            });
        }

        public async Task<WorkflowStep> QueryPreStep(string resourceType, string resourceKey, Guid resourceId)
        {
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo(_storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStep, out string tableNameStep))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowStep }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo
                    , fragment);
            }

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowResource, out string tableNameResource))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowResource }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo
                    , fragment);
            }


            WorkflowStep result = null;
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
                    CommandText = string.Format(@"--查询近创建的批次号；查询非当前批次号中最近创建的步骤
                                                  DECLARE @LatestSerialNo NVARCHAR(500);
                                                  SELECT TOP 1 @LatestSerialNo = wfStep.serialno
                                                  FROM {2} AS wfStep
                                                       INNER JOIN {3} AS wfResource ON wfResource.id = wfStep.resourceid
                                                  WHERE wfStep.resourceid = @resourceid
                                                  ORDER BY wfStep.createtime DESC;
                                                  
                                                  SELECT TOP 1 {0},{1}
                                                  FROM {2} AS wfStep
                                                       INNER JOIN {3} AS wfResource ON wfResource.id = wfStep.resourceid
                                                  WHERE wfStep.resourceid = @resourceid
                                                        AND wfStep.serialno <> @LatestSerialNo
                                                  ORDER BY wfStep.createtime DESC;", StoreHelper.GetWorkflowStepStoreSelectFields("wfStep"),
                                                                                    StoreHelper.GetWorkflowResourceStoreSelectFields("wfResource"), tableNameStep, tableNameResource)
                })
                {
                    var parameter = new SqlParameter("@resourceid", SqlDbType.UniqueIdentifier)
                    {
                        Value = resourceId
                    };
                    command.Parameters.Add(parameter);

                    command.Prepare();

                    SqlDataReader reader = null;

                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new WorkflowStep();
                            StoreHelper.SetWorkflowStepStoreSelectFields(result, reader, "wfResource", "wfStep");
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return result;
        }

        public async Task<WorkflowStep> QueryByStatus(string resourceType, string resourceKey, Guid resourceId, int status)
        {
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo( _storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStep, out string tableNameStep))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowStep }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo
                    , fragment);
            }

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowResource, out string tableNameResource))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowResource }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo
                    , fragment);
            }

            WorkflowStep result = null;
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
                    CommandText = string.Format(@"SELECT TOP 1 {0},{1}
                                                  FROM {2} AS wfStep
                                                       INNER JOIN {3} AS wfResource ON wfResource.id = wfStep.resourceid
                                                  WHERE wfStep.resourceid = @resourceid
                                                        AND wfStep.[status] = @status
                                                  ORDER BY wfStep.createtime DESC", StoreHelper.GetWorkflowStepStoreSelectFields("wfStep"),
                                                                                    StoreHelper.GetWorkflowResourceStoreSelectFields("wfResource"), tableNameStep, tableNameResource)
                })
                {
                    var parameter = new SqlParameter("@resourceid", SqlDbType.UniqueIdentifier)
                    {
                        Value = resourceId
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@status", SqlDbType.Int)
                    {
                        Value = status
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    SqlDataReader reader = null;

                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new WorkflowStep();
                            StoreHelper.SetWorkflowStepStoreSelectFields(result, reader, "wfResource", "wfStep");
                        }

                       await reader.CloseAsync();
                    }
                }
            });

            return result;
        }

        public async Task QueryByCreateTime(string resourceType, string resourceKey, Guid resourceId, string serialNo, DateTime createtime, Func<WorkflowStep, Task> callback)
        {
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo(_storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStep, out string tableNameStep))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowStep }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo
                    , fragment);
            }

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowResource, out string tableNameResource))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowResource }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo
                    , fragment);
            }

            var workflowStepList = new List<WorkflowStep>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, dbInfo.DBConnectionNames.Read, async (conn, transaction) =>
            {
                long? sequence = null;
                const int pageSize = 1000;

                while (true)
                {
                    workflowStepList.Clear();

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
                            command.CommandText = string.Format(@"SELECT TOP (@pageSize) {0},{1}
                                                                  FROM {2} AS wfStep
                                                                       INNER JOIN {3} AS wfResource ON wfResource.id = wfStep.resourceid
                                                                  WHERE wfStep.[resourceid] = @resourceid
                                                                        AND wfStep.[createtime] >= @createtime
                                                                        AND wfStep.[serialno] <> @serialno
                                                                  ORDER BY wfStep.[sequence]", StoreHelper.GetWorkflowStepStoreSelectFields("wfStep"),
                                                                                               StoreHelper.GetWorkflowResourceStoreSelectFields("wfResource"), tableNameStep, tableNameResource);
                        }
                        else
                        {
                            command.CommandText = string.Format(@"SELECT TOP (@pageSize) {0},{1}
                                                                  FROM {2} AS wfStep
                                                                       INNER JOIN {3} AS wfResource ON wfResource.id = wfStep.resourceid
                                                                  WHERE wfStep.[resourceid] = @resourceid
                                                                        AND wfStep.[createtime] >= @createtime
                                                                        AND wfStep.[serialno] <> @serialno
                                                                        AND wfStep.[sequence] > @sequence
                                                                  ORDER BY wfStep.[sequence]", StoreHelper.GetWorkflowStepStoreSelectFields("wfStep"),
                                                                                             StoreHelper.GetWorkflowResourceStoreSelectFields("wfResource"), tableNameStep, tableNameResource);
                        }

                        var parameter = new SqlParameter("@resourceid", SqlDbType.UniqueIdentifier)
                        {
                            Value = resourceId
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter("@createtime", SqlDbType.DateTime)
                        {
                            Value = createtime
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter("@serialno", SqlDbType.NVarChar, 256)
                        {
                            Value = serialNo
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
                                var workflowStep = new WorkflowStep();
                                StoreHelper.SetWorkflowStepStoreSelectFields(workflowStep, reader, "wfResource", "wfStep");
                                sequence = (long)reader["wfStepsequence"];
                                workflowStepList.Add(workflowStep);
                            }

                            await reader.CloseAsync();
                        }
                    }

                    foreach (var workflowStep in workflowStepList)
                    {
                        await callback(workflowStep);
                    }

                    if (workflowStepList.Count != pageSize)
                    {
                        break;
                    }
                }
            });
        }

        public async Task QueryByCompleteStatus(string resourceType, string resourceKey, Guid resourceId, string actionName, int status, bool completeStatus, Func<WorkflowStep, Task> callback)
        {
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo( _storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStep, out string tableNameStep))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowStep }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo
                    , fragment);
            }

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowResource, out string tableNameResource))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowResource }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo
                    , fragment);
            }

            var workflowStepList = new List<WorkflowStep>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, dbInfo.DBConnectionNames.Read, async (conn, transaction) =>
            {
                long? sequence = null;
                const int pageSize = 1000;

                while (true)
                {
                    workflowStepList.Clear();

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
                            command.CommandText = string.Format(@"SELECT TOP (@pagesize) {0},{1}
                                                                  FROM {2} AS wfStep
                                                                       INNER JOIN {3} AS wfResource ON wfResource.id = wfStep.resourceid
                                                                  WHERE wfStep.[resourceid] = @resourceid
                                                                        AND wfStep.[actionname] = @actionname
                                                                        AND wfStep.[status] = @status
                                                                        AND wfStep.[complete] = @completestatus
                                                                  ORDER BY wfStep.[sequence]", StoreHelper.GetWorkflowStepStoreSelectFields("wfStep"),
                                                                                             StoreHelper.GetWorkflowResourceStoreSelectFields("wfResource"), tableNameStep, tableNameResource);
                        }
                        else
                        {
                            command.CommandText = string.Format(@"SELECT TOP (@pagesize) {0},{1}
                                                                  FROM {2} AS wfStep
                                                                       INNER JOIN {3} AS wfResource ON wfResource.id = wfStep.resourceid
                                                                  WHERE wfStep.[resourceid] = @resourceid
                                                                        AND wfStep.[actionname] = @actionname
                                                                        AND wfStep.[status] = @status
                                                                        AND wfStep.[complete] = @completestatus
                                                                        AND wfStep.[sequence] > @sequence
                                                                  ORDER BY wfStep.[sequence]", StoreHelper.GetWorkflowStepStoreSelectFields("wfStep"),
                                                                                               StoreHelper.GetWorkflowResourceStoreSelectFields("wfResource"), tableNameStep, tableNameResource);
                        }

                        var parameter = new SqlParameter("@resourceid", SqlDbType.UniqueIdentifier)
                        {
                            Value = resourceId
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter("@actionname", SqlDbType.NVarChar, 256)
                        {
                            Value = actionName
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter("@status", SqlDbType.Int)
                        {
                            Value = status
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter("@completestatus", SqlDbType.Bit)
                        {
                            Value = completeStatus
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
                                var workflowStep = new WorkflowStep();
                                StoreHelper.SetWorkflowStepStoreSelectFields(workflowStep, reader, "wfResource", "wfStep");
                                sequence = (long)reader["wfStepsequence"];
                                workflowStepList.Add(workflowStep);
                            }

                            await reader.CloseAsync();
                        }
                    }

                    foreach (var workflowStep in workflowStepList)
                    {
                        await callback(workflowStep);
                    }

                    if (workflowStepList.Count != pageSize)
                    {
                        break;
                    }
                }
            });
        }

        public async Task QueryByResource(string resourceType, string resourceKey, Guid resourceId, int status, Func<WorkflowStep, Task> callback)
        {
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo( _storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStep, out string tableNameStep))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowStep }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo
                    , fragment);
            }

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowResource, out string tableNameResource))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.WorkflowResource }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo
                    , fragment);
            }


            var workflowStepList = new List<WorkflowStep>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, dbInfo.DBConnectionNames.Read, async (conn, transaction) =>
            {
                long? sequence = null;
                const int pageSize = 1000;

                while (true)
                {
                    workflowStepList.Clear();

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
                            command.CommandText = string.Format(@"SELECT TOP (@pageSize) {0},{1}
                                                                  FROM {2} AS wfStep
                                                                       INNER JOIN {3} AS wfResource ON wfResource.id = wfStep.resourceid
                                                                  WHERE wfStep.resourceid = @resourceid
                                                                        AND wfStep.[status] = @status
                                                                  ORDER BY wfStep.[sequence]", StoreHelper.GetWorkflowStepStoreSelectFields("wfStep"),
                                                                                               StoreHelper.GetWorkflowResourceStoreSelectFields("wfResource"), tableNameStep, tableNameResource);
                        }
                        else
                        {
                            command.CommandText = string.Format(@"SELECT TOP (@pageSize) {0},{1}
                                                                  FROM {2} AS wfStep
                                                                       INNER JOIN {3} AS wfResource ON wfResource.id = wfStep.resourceid
                                                                  WHERE wfStep.resourceid = @resourceid
                                                                        AND wfStep.[status] = @status
                                                                        AND wfStep.[sequence] > @sequence
                                                                  ORDER BY wfStep.[sequence]", StoreHelper.GetWorkflowStepStoreSelectFields("wfStep"),
                                                                                             StoreHelper.GetWorkflowResourceStoreSelectFields("wfResource"), tableNameStep, tableNameResource);
                        }

                        var parameter = new SqlParameter("@resourceid", SqlDbType.UniqueIdentifier)
                        {
                            Value = resourceId
                        };
                        command.Parameters.Add(parameter);


                        parameter = new SqlParameter("@status", SqlDbType.Int)
                        {
                            Value = status
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

                        await using (reader= await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var workflowStep = new WorkflowStep();
                                StoreHelper.SetWorkflowStepStoreSelectFields(workflowStep, reader, "wfResource", "wfStep");
                                sequence = (long)reader["wfStepsequence"];
                                workflowStepList.Add(workflowStep);
                            }
                            await reader.CloseAsync();
                        }
                    }

                    foreach (var workflowStep in workflowStepList)
                    {
                        await callback(workflowStep);
                    }

                    if (workflowStepList.Count != pageSize)
                    {
                        break;
                    }
                }
            });
        }
    }
}
