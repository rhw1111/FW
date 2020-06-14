using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using MSLibrary.Collections.Hash;
using MSLibrary.LanguageTranslate;
using MSLibrary.Transaction;
using MSLibrary.DAL;
using MSLibrary.DI;

namespace MSLibrary.Logger.DAL
{
    /// <summary>
    /// 通用日志数据操作
    /// 日志用CommonLog-{parentAction}作为组名称查找哈希组，
    /// 如果找不到，则使用CommonLogDefaultHashGroupName作为组名称查找哈希组，
    /// 以parentID作为key在找到的哈希组中找到对应的节点信息    
    /// </summary>
    [Injection(InterfaceType = typeof(ICommonLogStore), Scope = InjectionScope.Singleton)]
    public class CommonLogStore : ICommonLogStore
    {

        private const string _groupNameFormatting = "CommonLog-{0}";
        private const string _groupType = "CommonLog";

        private static string _commonLogDefaultHashGroupName;
        /// <summary>
        /// 通用日志默认哈希组名称
        /// 需要在系统初始化时赋值
        /// </summary>
        public static string CommonLogDefaultHashGroupName
        {
            set
            {
                _commonLogDefaultHashGroupName = value;
            }
        }


        private IHashGroupRepository _hashGroupRepository;
        private IStoreInfoResolveService _storeInfoResolveService;
        private ICommonLogConnectionFactory _commonLogConnectionFactory;
        private IStoreInfoResolveConnectionService _storeInfoResolveConnectionService;

        public CommonLogStore(IHashGroupRepository hashGroupRepository,IStoreInfoResolveService storeInfoResolveService, ICommonLogConnectionFactory commonLogConnectionFactory, IStoreInfoResolveConnectionService storeInfoResolveConnectionService)
        {
            _hashGroupRepository = hashGroupRepository;
            _storeInfoResolveService = storeInfoResolveService;
            _commonLogConnectionFactory = commonLogConnectionFactory;
            _storeInfoResolveConnectionService = storeInfoResolveConnectionService;
        }

        public async Task Add(CommonLog log)
        {

            var group = await getHashGroup(log.ParentActionName);

            var dbInfo = await StoreInfoHelper.GetHashStoreInfo(group,_storeInfoResolveService, log.ParentID.ToString());

            var strConn = await _storeInfoResolveConnectionService.GetReadAndWrite(dbInfo);

            var tableNameCommonlog = getTableName(group.Name, dbInfo.TableNames);

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, strConn, async (conn, transaction) =>
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
                    int length;
             
                    if (log.ID == Guid.Empty)
                    {
                        command.CommandText = string.Format(@"insert into {0} ([id],[parentid],[prelevelid],[currentlevelid],[contextinfo],[categoryname],[actionname],[parentactionname],[requestbody],[responsebody],[requesturi],[message],[root],[level],[duration],[createtime],[modifytime])
                                                values (default,@parentid,@prelevelid,@currentlevelid,@contextinfo,@categoryname,@actionname,@parentactionname,@requestbody,@responsebody,@requesturi,@message,@root,@level,@duration,GETUTCDATE(),GETUTCDATE()); 
                                                SELECT @newid=[id] FROM {0} WHERE [sequence]=SCOPE_IDENTITY()", tableNameCommonlog);

                        parameter = new SqlParameter("@newid", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        command.CommandText = string.Format(@"insert into {0} ([id],[parentid],[prelevelid],[currentlevelid],[contextinfo],[actionname],[parentactionname],[requestbody],[responsebody],[requesturi],[message],[root],[level],[duration],[createtime],[modifytime])
                                                VALUES (@id,@parentid,@prelevelid,@currentlevelid,@contextinfo,@actionname,@parentactionname,@requestbody,@responsebody,@requesturi,@message,@root,@level,@duration,GETUTCDATE(),GETUTCDATE())", tableNameCommonlog);

                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = log.ID
                        };
                        command.Parameters.Add(parameter);
                    }


                    parameter = new SqlParameter("@parentid", SqlDbType.UniqueIdentifier)
                    {
                        Value = log.ParentID
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@prelevelid", SqlDbType.UniqueIdentifier)
                    {
                        Value = log.PreLevelID
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@currentlevelid", SqlDbType.UniqueIdentifier)
                    {
                        Value = log.CurrentLevelID
                    };
                    command.Parameters.Add(parameter);

                    if (log.ContextInfo.Length == 0)
                    {
                        length = 10;
                    }
                    else
                    {
                        length = log.ContextInfo.Length;
                    }

                    parameter = new SqlParameter("@contextinfo", SqlDbType.NVarChar, length)
                    {
                        Value = log.ContextInfo
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@categoryname", SqlDbType.NVarChar, 300)
                    {
                        Value = log.CategoryName
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@actionname", SqlDbType.NVarChar, 300)
                    {
                        Value = log.ActionName
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@parentactionname", SqlDbType.NVarChar, 300)
                    {
                        Value = log.ParentActionName
                    };
                    command.Parameters.Add(parameter);

                    if (log.RequestBody == null)
                    {
                        log.RequestBody = string.Empty;
                    }
                    if (log.RequestBody.Length == 0)
                    {
                        length = 10;
                    }
                    else
                    {
                        length = log.RequestBody.Length;
                    }

                    parameter = new SqlParameter("@requestbody", SqlDbType.NVarChar, length)
                    {
                        Value = log.RequestBody
                    };
                    command.Parameters.Add(parameter);

                    if (log.ResponseBody.Length == 0)
                    {
                        length = 10;
                    }
                    else
                    {
                        length = log.ResponseBody.Length;
                    }

                    if (log.ResponseBody == null)
                    {
                        log.ResponseBody = string.Empty;
                    }
                    parameter = new SqlParameter("@responsebody", SqlDbType.NVarChar, length)
                    {
                        Value = log.ResponseBody
                    };
                    command.Parameters.Add(parameter);


                    parameter = new SqlParameter("@requesturi", SqlDbType.NVarChar, 500)
                    {
                        Value = log.RequestUri
                    };
                    command.Parameters.Add(parameter);

                    if (log.Message.Length == 0)
                    {
                        length = 10;
                    }
                    else
                    {
                        length = log.Message.Length;
                    }

                    parameter = new SqlParameter("@message", SqlDbType.NVarChar, length)
                    {
                        Value = log.Message
                    };
                    command.Parameters.Add(parameter);


                    parameter = new SqlParameter("@root", SqlDbType.Bit)
                    {
                        Value = log.Root
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@level", SqlDbType.Int)
                    {
                        Value = log.Level
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@duration", SqlDbType.BigInt)
                    {
                        Value = log.Duration
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    await command.ExecuteNonQueryAsync();

                    if (log.ID == Guid.Empty)
                    {
                        log.ID = (Guid)command.Parameters["@newid"].Value;
                    }
                }
            });
        }

        public async Task<CommonLog> QueryByID(Guid id, Guid parentID, string parentActionName)
        {
            var group = await getHashGroup(parentActionName);

            var dbInfo = await StoreInfoHelper.GetHashStoreInfo(group, _storeInfoResolveService, parentID.ToString());

            var tableNameCommonlog = getTableName(group.Name, dbInfo.TableNames);

            var strConn = await _storeInfoResolveConnectionService.GetRead(dbInfo);


            CommonLog result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, strConn, async (conn, transaction) =>
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
                })
                {
                    SqlParameter parameter;

                    command.CommandText = string.Format(@"select {0} from [dbo].[{1}] where id=@id;", StoreHelper.GetCommonLogSelectFields(string.Empty), tableNameCommonlog);
                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    SqlDataReader reader = null;


                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new CommonLog();
                            StoreHelper.SetCommonLogSelectFields(result, reader, string.Empty);
                        }
                        await reader.CloseAsync();
                    }
                }
            });

            return result;
        }

        public async Task<List<CommonLog>> QueryRootByConditionTop(string parentActionName,int? level, int top)
        {
            List<CommonLog> logs = new List<CommonLog>();

            List<HashGroup> groups = new List<HashGroup>();

            if (parentActionName != null)
            {
                var group = await getHashGroup(parentActionName);
                await queryRootByConditionTop(group, parentActionName, level, top, logs);
            }
            else
            {
                await _hashGroupRepository.QueryByType(_groupType, async(group)=>
                {
                    await queryRootByConditionTop(group, parentActionName, level, top, logs);
                });

            }

            logs = (from item in logs
                    orderby item.CreateTime descending
                    select item).Take(top).ToList();

            return logs;
        }

        public async Task AddLocal(CommonLog log)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _commonLogConnectionFactory.CreateAllForLocalCommonLog(), async (conn, transaction) =>
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
                    int length;

                    if (log.ID == Guid.Empty)
                    {
                        command.CommandText = string.Format(@"insert into CommonLog_Local ([id],[parentid],[prelevelid],[currentlevelid],[contextinfo],[categoryname],[actionname],[parentactionname],[requestbody],[responsebody],[requesturi],[message],[root],[level],[duration],[createtime],[modifytime])
                                                values (default,@parentid,@prelevelid,@currentlevelid,@contextinfo,@categoryname,@actionname,@parentactionname,@requestbody,@responsebody,@requesturi,@message,@root,@level,@duration,GETUTCDATE(),GETUTCDATE()); 
                                                SELECT @newid=[id] FROM [dbo].[CommonLog_Local] WHERE [sequence]=SCOPE_IDENTITY()");

                        parameter = new SqlParameter("@newid", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        command.CommandText = string.Format(@"insert into CommonLog_Local ([id],[parentid],[prelevelid],[currentlevelid],[contextinfo],[categoryname],[actionname],[parentactionname],[requestbody],[responsebody],[requesturi],[message],[root],[level],[duration],[createtime],[modifytime])
                                                VALUES (@id,@parentid,@prelevelid,@currentlevelid,@contextinfo,@categoryname,@actionname,@parentactionname,@requestbody,@responsebody,@requesturi,@message,@root,@level,@duration,GETUTCDATE(),GETUTCDATE())");

                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = log.ID
                        };
                        command.Parameters.Add(parameter);
                    }


                    parameter = new SqlParameter("@parentid", SqlDbType.UniqueIdentifier)
                    {
                        Value = log.ParentID
                    };
                    command.Parameters.Add(parameter);

                    if (log.ContextInfo.Length==0)
                    {
                        length = 10;
                    }
                    else
                    {
                        length = log.ContextInfo.Length;
                    }


                    parameter = new SqlParameter("@prelevelid", SqlDbType.UniqueIdentifier)
                    {
                        Value = log.PreLevelID
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@currentlevelid", SqlDbType.UniqueIdentifier)
                    {
                        Value = log.CurrentLevelID
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@contextinfo", SqlDbType.NVarChar, length)
                    {
                        Value = log.ContextInfo
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@categoryname", SqlDbType.NVarChar, 300)
                    {
                        Value = log.CategoryName
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@actionname", SqlDbType.NVarChar, 300)
                    {
                        Value = log.ActionName
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@parentactionname", SqlDbType.NVarChar, 300)
                    {
                        Value = log.ParentActionName
                    };
                    command.Parameters.Add(parameter);


                    if (log.RequestBody == null)
                    {
                        log.RequestBody = string.Empty;
                    }

                    if (log.RequestBody.Length == 0)
                    {
                        length = 10;
                    }
                    else
                    {
                        length = log.RequestBody.Length;
                    }

                    parameter = new SqlParameter("@requestbody", SqlDbType.NVarChar, length)
                    {
                        Value = log.RequestBody
                    };
                    command.Parameters.Add(parameter);

                    if (log.ResponseBody == null)
                    {
                        log.ResponseBody = string.Empty;
                    }

                    if (log.ResponseBody.Length == 0)
                    {
                        length = 10;
                    }
                    else
                    {
                        length = log.ResponseBody.Length;
                    }

                    parameter = new SqlParameter("@responsebody", SqlDbType.NVarChar,length)
                    {
                        Value = log.ResponseBody
                    };
                    command.Parameters.Add(parameter);


                    parameter = new SqlParameter("@requesturi", SqlDbType.NVarChar, 500)
                    {
                        Value = log.RequestUri
                    };
                    command.Parameters.Add(parameter);


                    if (log.Message.Length == 0)
                    {
                        length = 10;
                    }
                    else
                    {
                        length = log.Message.Length;
                    }

                    parameter = new SqlParameter("@message", SqlDbType.NVarChar, length)
                    {
                        Value = log.Message
                    };
                    command.Parameters.Add(parameter);


                    parameter = new SqlParameter("@root", SqlDbType.Bit)
                    {
                        Value = log.Root
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@level", SqlDbType.Int)
                    {
                        Value = log.Level
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@duration", SqlDbType.BigInt)
                    {
                        Value = log.Duration
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    await command.ExecuteNonQueryAsync();

                    if (log.ID == Guid.Empty)
                    {
                        log.ID = (Guid)command.Parameters["@newid"].Value;
                    }
                }
            });
        }

        public async Task<CommonLog> QueryLocalByID(Guid id)
        {
            CommonLog result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _commonLogConnectionFactory.CreateReadForLocalCommonLog(), async (conn, transaction) =>
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
                })
                {
                    SqlParameter parameter;

                    command.CommandText = string.Format(@"select {0} from [dbo].[CommonLog_Local] where id=@id;", StoreHelper.GetCommonLogSelectFields(string.Empty));
                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    SqlDataReader reader = null;


                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new CommonLog();
                            StoreHelper.SetCommonLogSelectFields(result, reader, string.Empty);
                        }
                        await reader.CloseAsync();
                    }
                }
            }); 

            return result;
        }

        public async Task<QueryResult<CommonLog>> QueryLocal(string message, int page, int pageSize)
        {
            if (message==null)
            {
                message = string.Empty;
            }
            QueryResult<CommonLog> result = new QueryResult<CommonLog>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _commonLogConnectionFactory.CreateReadForLocalCommonLog(), async (conn, transaction) =>
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
                })
                {
                    SqlParameter parameter;
                    int length;

                    command.CommandText = string.Format(@"SELECT @count = COUNT(*)
                                                    FROM [dbo].[CommonLog_Local]
                                                    WHERE [message] like @message                                                         
                                                    
                                                    SELECT {0}
                                                    FROM [dbo].[CommonLog_Local]
                                                    WHERE [message] like @message  
                                                    ORDER BY [sequence] desc OFFSET (@pagesize * (@currentpage - 1)) ROWS FETCH NEXT @pagesize ROWS ONLY;", StoreHelper.GetCommonLogSelectFields(string.Empty));

                    string messageCondition = $"{message.ToSqlLike()}%";
                    parameter = new SqlParameter("@message", SqlDbType.NVarChar, messageCondition.Length)
                    {
                        Value = messageCondition
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@currentpage", SqlDbType.Int)
                    {
                        Value = page
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@pagesize", SqlDbType.Int)
                    {
                        Value = pageSize
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@count", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    SqlDataReader reader = null;


                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var log = new CommonLog();
                            StoreHelper.SetCommonLogSelectFields(log, reader, string.Empty);
                            result.Results.Add(log);

                        }
                        await reader.CloseAsync();

                        result.TotalCount = (int)command.Parameters["@count"].Value;
                        result.CurrentPage = page;
                    }
                }
            });

            return result;
        }

        public async Task<QueryResult<CommonLog>> QueryByParentId(Guid parentID, string parentActionName, int page, int pageSize)
        {
            var group = await getHashGroup(parentActionName);

            var dbInfo = await StoreInfoHelper.GetHashStoreInfo(group, _storeInfoResolveService, parentID.ToString());

            var tableNameCommonlog = getTableName(group.Name, dbInfo.TableNames);

            var strConn = await _storeInfoResolveConnectionService.GetRead(dbInfo);


            QueryResult<CommonLog> result = new QueryResult<CommonLog>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, strConn, async (conn, transaction) =>
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
                })
                {
                    SqlParameter parameter;

                    command.CommandText = string.Format(@"SELECT @count = COUNT(*)
                                                    FROM [dbo].[{1}] 
                                                    where [parentid]=@parentid
                                                    
                                                    SELECT {0}
                                                    FROM [dbo].[{1}] 
                                                    where [parentid]=@parentid
                                                    ORDER BY [sequence] OFFSET (@pagesize * (@currentpage - 1)) ROWS FETCH NEXT @pagesize ROWS ONLY;", StoreHelper.GetCommonLogSelectFields(string.Empty), tableNameCommonlog);


                    parameter = new SqlParameter("@parentid", SqlDbType.UniqueIdentifier)
                    {
                        Value = parentID
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@currentpage", SqlDbType.Int)
                    {
                        Value = page
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@pagesize", SqlDbType.Int)
                    {
                        Value = pageSize
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@count", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    SqlDataReader reader = null;


                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var log = new CommonLog();
                            StoreHelper.SetCommonLogSelectFields(log, reader, string.Empty);
                            result.Results.Add(log);

                        }
                        await reader.CloseAsync();

                        result.TotalCount = (int)command.Parameters["@count"].Value;
                        result.CurrentPage = page;
                    }
                }
            });

            return result;
        }


        private async Task queryRootByConditionTop(HashGroup group,string parentActionName, int? level, int top,List<CommonLog> logs)
        {
            var dbInfos = await StoreInfoHelper.GetHashStoreInfos(group, _storeInfoResolveService);


            foreach (var dbInfoItem in dbInfos)
            {
                var tableNameCommonlog = getTableName(group.Name, dbInfoItem.TableNames);

                var strConn = await _storeInfoResolveConnectionService.GetRead(dbInfoItem);


                await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, strConn, async (conn, transaction) =>
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
                    })
                    {
                        SqlParameter parameter;
                        string strConditionParentActionName = string.Empty;
                        if (strConditionParentActionName!=null)
                        {
                            strConditionParentActionName = "and [parentactionname]=@parentactionname";
                            parameter = new SqlParameter("@parentactionname", SqlDbType.NVarChar,300)
                            {
                                Value = parentActionName
                            };
                            command.Parameters.Add(parameter);
                        }

                        string strConditionLevel = string.Empty;
                        if (level!=null)
                        {
                            strConditionParentActionName = "and [level]=@level";
                            parameter = new SqlParameter("@level", SqlDbType.Int)
                            {
                                Value = level.Value
                            };
                            command.Parameters.Add(parameter);
                        }

                        command.CommandText = string.Format(@"select top (@top) {0} from [dbo].[{1}] where 1=1 {2} {3} order by [sequence] desc;", StoreHelper.GetCommonLogSelectFields(string.Empty), tableNameCommonlog,strConditionParentActionName,strConditionLevel);

                        parameter = new SqlParameter("@top", SqlDbType.Int)
                        {
                            Value = top
                        };
                        command.Parameters.Add(parameter);


                        await command.PrepareAsync();

                        SqlDataReader reader = null;


                        await using (reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var log = new CommonLog();
                                StoreHelper.SetCommonLogSelectFields(log, reader, string.Empty);
                                logs.Add(log);
                            }
                            await reader.CloseAsync();
                        }
                    }
                });

            }
        }
        private async Task<HashGroup> getHashGroup(string parentAction)
        {
            var hashGroupRepositoryFactory = HashGroupRepositoryHelperFactory.Create(_hashGroupRepository);

            var group = await hashGroupRepositoryFactory.QueryByName(string.Format(_groupNameFormatting, parentAction));

            if (group == null)
            {
                group = await hashGroupRepositoryFactory.QueryByName(_commonLogDefaultHashGroupName);
            }

            if (group == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundHashGroupByName,
                    DefaultFormatting = "没有找到名称为{0}的一致性哈希组",
                    ReplaceParameters = new List<object>() { _commonLogDefaultHashGroupName }
                };

                throw new UtilityException((int)Errors.NotFoundHashGroupByName, fragment);
            }

            return group;
        }





        private string getTableName(string groupName, Dictionary<string,string> tableNames)
        {
            if (!tableNames.TryGetValue(HashEntityNames.CommonLog, out string tableName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { groupName
                    , HashEntityNames.CommonLog }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }

            return tableName;
        }
    }
}
