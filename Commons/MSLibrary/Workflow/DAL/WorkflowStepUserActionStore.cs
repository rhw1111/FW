using MSLibrary.Collections.Hash;
using MSLibrary.DAL;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Transaction;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MSLibrary.Workflow.DAL
{
    /// <summary>
    /// 工作流用户动作数据操作 实现类
    /// </summary>
    [Injection(InterfaceType = typeof(IWorkflowStepUserActionStore), Scope = InjectionScope.Singleton)]
    public class WorkflowStepUserActionStore : IWorkflowStepUserActionStore
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

        private static string _resourceHashGroupName;
        /// <summary>
        /// 需要用到的针对工作流资源的一致性哈希组的名称
        /// 需要在系统初始化时赋值
        /// </summary>
        public static string ResourceHashGroupName
        {
            set
            {
                _resourceHashGroupName = value;
            }
        }


        private IWorkflowConnectionFactory _dbConnectionFactory;
        private IHashGroupRepositoryCacheProxy _hashGroupRepository;
        private IStoreInfoResolveService _storeInfoResolveService;

        public WorkflowStepUserActionStore(IWorkflowConnectionFactory dbConnectionFactory, IHashGroupRepositoryCacheProxy hashGroupRepository, IStoreInfoResolveService storeInfoResolveService)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _hashGroupRepository = hashGroupRepository;
            _storeInfoResolveService = storeInfoResolveService;
        }

        /// <summary>
        /// 新增用户动作
        /// </summary>
        /// <param name="resourceType">连接数据库类型</param>
        /// <param name="resourceKey">连接数据库key</param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task Add(string resourceType, string resourceKey, WorkflowStepUserAction action)
        {
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo( _storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStepUserAction, out string tableNameStepUserAction))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName
                    , HashEntityNames.WorkflowStepUserAction }
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

                await using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran
                })
                {
                    SqlParameter parameter;
                    //判断用户是否提交ID，如果ID为空
                    if (action.ID == Guid.Empty)
                    {
                        command.CommandText = string.Format(@"INSERT INTO {0} ([id],[stepid],[userkey],[result],[createtime],[modifytime])
                                                VALUES (default,@stepid,@userkey,@result,GETUTCDATE(),GETUTCDATE()); 
                                                SELECT @newid=[id] FROM {0} WHERE [sequence]=SCOPE_IDENTITY()", tableNameStepUserAction);

                        parameter = new SqlParameter("@newid", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        command.CommandText = string.Format(@"INSERT INTO {0} ([id],[stepid],[userkey],[result],[createtime],[modifytime])
                                                VALUES (@id,@stepid,@userkey,@result,GETUTCDATE(),GETUTCDATE())", tableNameStepUserAction);

                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = action.ID
                        };
                        command.Parameters.Add(parameter);
                    }
                    parameter = new SqlParameter("@stepid", SqlDbType.UniqueIdentifier)
                    {
                        Value = action.StepID
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@userkey", SqlDbType.NVarChar, 256)
                    {
                        Value = action.UserKey
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@result", SqlDbType.Int)
                    {
                        Value = action.Result
                    };
                    command.Parameters.Add(parameter);
                    await command.PrepareAsync();


                        try
                        {
                            await command.ExecuteNonQueryAsync();
                        }
                        catch (SqlException ex)
                        {
                            if (ex == null)
                            {
                                throw;
                            }
                            if (ex.Number == 2601)
                            {
                                var fragment = new TextFragment()
                                {
                                    Code = TextCodes.ExistWorkflowStepUserActionByUserKey,
                                    DefaultFormatting = "工作流用户动作中已存在相同关键字\"{0}\"数据",
                                    ReplaceParameters = new List<object>() { action.UserKey }
                                };

                                throw new UtilityException((int)Errors.ExistWorkflowStepUserActionByUserKey, fragment);
                            }
                            else
                            {
                                throw;
                            }
                        }

           

                    if (action.ID == Guid.Empty)
                    {
                        action.ID = (Guid)command.Parameters["@newid"].Value;
                    }
                }
            });
        }

        /// <summary>
        /// 删除用户动作
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="stepId">关联的步骤ID</param>
        /// <param name="actionId">工作流用户动作ID</param>
        /// <returns></returns>
        public async Task Delete(string resourceType, string resourceKey, Guid stepId, Guid actionId)
        {
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo( _storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStepUserAction, out string tableNameStepUserAction))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName
                    , HashEntityNames.WorkflowStepUserAction }
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

                await using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = string.Format(@"DELETE FROM {0} WHERE id=@id AND stepid=@stepid", tableNameStepUserAction)
                })
                {
                    SqlParameter parameter;

                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = actionId
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@stepid", SqlDbType.UniqueIdentifier)
                    {
                        Value = stepId
                    };
                    command.Parameters.Add(parameter);
                    await command.PrepareAsync();


                        await command.ExecuteNonQueryAsync();
                 
                }
            });
        }

        /// <summary>
        /// 删除指定步骤下面的所有用户动作
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="stepId">关联的步骤ID</param>
        /// <returns></returns>
        public async Task Delete(string resourceType, string resourceKey, Guid stepId)
        {
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo( _storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStepUserAction, out string tableNameStepUserAction))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName
                    , HashEntityNames.WorkflowStepUserAction }
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

                await using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = string.Format(@"DELETE FROM {0} WHERE stepid=@stepid", tableNameStepUserAction)
                })
                {
                    SqlParameter parameter;
                    parameter = new SqlParameter("@stepid", SqlDbType.UniqueIdentifier)
                    {
                        Value = stepId
                    };
                    command.Parameters.Add(parameter);
                    await command.PrepareAsync();


                        await command.ExecuteNonQueryAsync();
               
                }
            });
        }

        /// <summary>
        /// 查询指定步骤下的所有用户动作
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="stepId"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task QueryByStep(string resourceType, string resourceKey, Guid stepId, Func<WorkflowStepUserAction, Task> callback)
        {
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo( _storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStepUserAction, out string tableNameStepUserAction))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName
                    , HashEntityNames.WorkflowStepUserAction }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }

            List<WorkflowStepUserAction> listAction = new List<WorkflowStepUserAction>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, dbInfo.DBConnectionNames.Read, async (conn, transaction) =>
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
                    CommandText = string.Format(@"SELECT {0} FROM {1} WHERE stepid =@stepid ORDER BY sequence", StoreHelper.GetWorkflowStepUserActionStoreSelectFields(string.Empty), tableNameStepUserAction),
                    Transaction = sqlTran
                })
                {
                    var parameter = new SqlParameter("@stepid", SqlDbType.UniqueIdentifier)
                    {
                        Value = stepId
                    };
                    command.Parameters.Add(parameter);
                    await command.PrepareAsync();
                    SqlDataReader reader = null;

                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var workflowStepUserAction = new WorkflowStepUserAction();
                            StoreHelper.SetWorkflowStepUserActionStoreSelectFields(workflowStepUserAction, reader, string.Empty);
                            listAction.Add(workflowStepUserAction);
                        }
                        await reader.CloseAsync();
                    }
                }
                foreach (var actionItem in listAction)
                {
                    await callback(actionItem);
                }
            });
        }



        /// <summary>
        /// 查询指定步骤下的所有用户动作的数量
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="stepId"></param>
        /// <returns></returns>
        public async Task<int> QueryCountByStep(string resourceType, string resourceKey, Guid stepId)
        {
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo( _storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStepUserAction, out string tableNameStepUserAction))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName
                    , HashEntityNames.WorkflowStepUserAction }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }
            int result = 0;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, dbInfo.DBConnectionNames.Read, async (conn, transaction) =>
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
                    CommandText = string.Format(@"SELECT COUNT(0) FROM {0} WHERE stepid =@stepid ", tableNameStepUserAction),
                    Transaction = sqlTran
                })
                {
                    var parameter = new SqlParameter("@stepid", SqlDbType.UniqueIdentifier)
                    {
                        Value = stepId
                    };
                    command.Parameters.Add(parameter);
                    await command.PrepareAsync();


                        result = (int)await command.ExecuteScalarAsync();
                 
                }
            });
            return result;
        }

        /// <summary>
        /// 查询指定资源下面的指定动作名称指定状态的所有步骤下面的所有用户动作
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="resourceId"></param>
        /// <param name="actionName"></param>
        /// <param name="status"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task QueryByResource(string resourceType, string resourceKey, Guid resourceId, string actionName, int status, Func<WorkflowStepUserAction, Task> callback)
        {
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo( _storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStepUserAction, out string tableNameStepUserAction))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName
                    , HashEntityNames.WorkflowStepUserAction }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStep, out string tableNameStep))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName
                    , HashEntityNames.WorkflowStep }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowResource, out string tableNameResource))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName
                    , HashEntityNames.WorkflowResource }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }

            List<WorkflowStepUserAction> listAction = new List<WorkflowStepUserAction>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, dbInfo.DBConnectionNames.Read, async (conn, transaction) =>
            {
                int sequence = 0;
                int pageSize = 500;
                while (true)
                {
                    listAction.Clear();
                    SqlTransaction sqlTran = null;
                    if (transaction != null)
                    {
                        sqlTran = (SqlTransaction)transaction;
                    }
                    await using (SqlCommand command = new SqlCommand()
                    {
                        Connection = (SqlConnection)conn,
                        CommandType = CommandType.Text,
                        CommandText = string.Format(@"SELECT {0} 
                                                        FROM {1} AS action 
                                                        INNER JOIN {2} AS step ON action.stepid=step.id
                                                        INNER JOIN {3} AS resource ON resource.id = step.resourceid
                                                        WHERE resource.id=@resourceId AND  step.actionname=@actionName AND step.status=@status 
                                                        ORDER BY action.sequence OFFSET @sequence ROWS FETCH NEXT @pagesize ROWS ONLY;", StoreHelper.GetWorkflowStepUserActionStoreSelectFields("action")
                                                        , tableNameStepUserAction, tableNameStep, tableNameResource),
                        Transaction = sqlTran
                    })
                    {
                        var parameter = new SqlParameter("@resourceId", SqlDbType.UniqueIdentifier)
                        {
                            Value = resourceId
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter("@actionName", SqlDbType.NVarChar, 500)
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
                        parameter = new SqlParameter("@sequence", SqlDbType.Int)
                        {
                            Value = sequence
                        };
                        command.Parameters.Add(parameter);

                        await command.PrepareAsync();
                        SqlDataReader reader = null;

                        await using (reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var workflowStepUserAction = new WorkflowStepUserAction();
                                StoreHelper.SetWorkflowStepUserActionStoreSelectFields(workflowStepUserAction, reader, "action");
                                listAction.Add(workflowStepUserAction);
                            }
                            await reader.CloseAsync();
                        }
                    }
                    foreach (var actionItem in listAction)
                    {
                        await callback(actionItem);
                    }
                    if (listAction.Count != pageSize)
                    {
                        break;
                    }
                    else
                    {
                        sequence += listAction.Count;
                    }
                }
            });

        }

        /// <summary>
        /// 查询指定步骤下面指定用户信息的用户动作
        /// </summary>
        /// <param name="resourceType"></param>
        /// <param name="resourceKey"></param>
        /// <param name="stepId"></param>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public async Task<WorkflowStepUserAction> QueryByStepAndUser(string resourceType, string resourceKey, Guid stepId, string userKey)
        {
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo( _storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStepUserAction, out string tableNameStepUserAction))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName
                    , HashEntityNames.WorkflowStepUserAction }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStep, out string tableNameStep))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName
                    , HashEntityNames.WorkflowStep }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowResource, out string tableNameResource))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName
                    , HashEntityNames.WorkflowResource }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }

            WorkflowStepUserAction result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, dbInfo.DBConnectionNames.Read, async (conn, transaction) =>
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
                    CommandText = string.Format(@"SELECT {0} FROM {1} WHERE stepid =@stepid AND userkey=@userkey ;", StoreHelper.GetWorkflowStepUserActionStoreSelectFields(string.Empty), tableNameStepUserAction),
                    Transaction = sqlTran
                })
                {
                    var parameter = new SqlParameter("@stepid", SqlDbType.UniqueIdentifier)
                    {
                        Value = stepId
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@userkey", SqlDbType.NVarChar, 256)
                    {
                        Value = userKey
                    };
                    command.Parameters.Add(parameter);
                    await command.PrepareAsync();
                    SqlDataReader reader = null;

                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new WorkflowStepUserAction();
                            StoreHelper.SetWorkflowStepUserActionStoreSelectFields(result, reader, string.Empty);
                        }
                        await reader.CloseAsync();
                    }
                }
            });
            return result;
        }

        public async Task QueryByResource(string resourceType, string resourceKey, Guid resourceId, int status, Func<WorkflowStepUserAction, Task> callback)
        {
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo( _storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStepUserAction, out string tableNameStepUserAction))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName
                    , HashEntityNames.WorkflowStepUserAction }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStep, out string tableNameStep))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName
                    , HashEntityNames.WorkflowStep }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowResource, out string tableNameResource))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName
                    , HashEntityNames.WorkflowResource }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo,fragment);
            }

            List<WorkflowStepUserAction> listAction = new List<WorkflowStepUserAction>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, dbInfo.DBConnectionNames.Read, async (conn, transaction) =>
            {
                int sequence = 0;
                int pageSize = 500;
                while (true)
                {
                    listAction.Clear();
                    SqlTransaction sqlTran = null;
                    if (transaction != null)
                    {
                        sqlTran = (SqlTransaction)transaction;
                    }
                    await using (SqlCommand command = new SqlCommand()
                    {
                        Connection = (SqlConnection)conn,
                        CommandType = CommandType.Text,
                        CommandText = string.Format(@"SELECT {0} 
                                                        FROM {1} AS action 
                                                        INNER JOIN {2} AS step ON action.stepid=step.id
                                                        INNER JOIN {3} AS resource ON resource.id = step.resourceid
                                                        WHERE resource.id=@resourceId AND step.status=@status 
                                                        ORDER BY action.sequence OFFSET @sequence ROWS FETCH NEXT @pagesize ROWS ONLY;", StoreHelper.GetWorkflowStepUserActionStoreSelectFields("action")
                                                        , tableNameStepUserAction, tableNameStep, tableNameResource),
                        Transaction = sqlTran
                    })
                    {
                        var parameter = new SqlParameter("@resourceId", SqlDbType.UniqueIdentifier)
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
                        parameter = new SqlParameter("@sequence", SqlDbType.Int)
                        {
                            Value = sequence
                        };
                        command.Parameters.Add(parameter);

                        await command.PrepareAsync();
                        SqlDataReader reader = null;

                        await using (reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var workflowStepUserAction = new WorkflowStepUserAction();
                                StoreHelper.SetWorkflowStepUserActionStoreSelectFields(workflowStepUserAction, reader, "action");
                                listAction.Add(workflowStepUserAction);
                            }
                            await reader.CloseAsync();
                        }
                    }
                    foreach (var actionItem in listAction)
                    {
                        await callback(actionItem);
                    }
                    if (listAction.Count != pageSize)
                    {
                        break;
                    }
                    else
                    {
                        sequence += listAction.Count;
                    }
                }
            });
        }

        public async Task QueryByResource(string resourceType, string resourceKey, Guid resourceId, Func<WorkflowStepUserAction, Task> callback)
        {
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo(_storeInfoResolveService,_hashGroupRepository, _hashGroupName, resourceType, resourceKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStepUserAction, out string tableNameStepUserAction))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName
                    , HashEntityNames.WorkflowStepUserAction }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowStep, out string tableNameStep))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName
                    , HashEntityNames.WorkflowStep }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo,fragment);
            }

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.WorkflowResource, out string tableNameResource))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName
                    , HashEntityNames.WorkflowResource }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }

            List<WorkflowStepUserAction> listAction = new List<WorkflowStepUserAction>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, dbInfo.DBConnectionNames.Read, async (conn, transaction) =>
            {
                int sequence = 0;
                int pageSize = 500;
                while (true)
                {
                    listAction.Clear();
                    SqlTransaction sqlTran = null;
                    if (transaction != null)
                    {
                        sqlTran = (SqlTransaction)transaction;
                    }
                    await using (SqlCommand command = new SqlCommand()
                    {
                        Connection = (SqlConnection)conn,
                        CommandType = CommandType.Text,
                        CommandText = string.Format(@"SELECT {0} 
                                                        FROM {1} AS action 
                                                        INNER JOIN {2} AS step ON action.stepid=step.id
                                                        INNER JOIN {3} AS resource ON resource.id = step.resourceid
                                                        WHERE resource.id=@resourceId ORDER BY action.sequence OFFSET @sequence ROWS FETCH NEXT @pagesize ROWS ONLY;", StoreHelper.GetWorkflowStepUserActionStoreSelectFields("action")
                                                        , tableNameStepUserAction, tableNameStep, tableNameResource),
                        Transaction = sqlTran
                    })
                    {
                        var parameter = new SqlParameter("@resourceId", SqlDbType.UniqueIdentifier)
                        {
                            Value = resourceId
                        };
                        command.Parameters.Add(parameter);
                        parameter = new SqlParameter("@pagesize", SqlDbType.Int)
                        {
                            Value = pageSize
                        };
                        command.Parameters.Add(parameter);
                        parameter = new SqlParameter("@sequence", SqlDbType.Int)
                        {
                            Value = sequence
                        };
                        command.Parameters.Add(parameter);

                        await command.PrepareAsync();
                        SqlDataReader reader = null;

                        await using (reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var workflowStepUserAction = new WorkflowStepUserAction();
                                StoreHelper.SetWorkflowStepUserActionStoreSelectFields(workflowStepUserAction, reader, "action");
                                listAction.Add(workflowStepUserAction);
                            }
                            await reader.CloseAsync();
                        }
                    }
                    foreach (var actionItem in listAction)
                    {
                        await callback(actionItem);
                    }
                    if (listAction.Count != pageSize)
                    {
                        break;
                    }
                    else
                    {
                        sequence += listAction.Count;
                    }
                }
            });
        }
    }
}
