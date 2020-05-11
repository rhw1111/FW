using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using MSLibrary.DI;
using MSLibrary.Collections.Hash;
using MSLibrary.DAL;
using MSLibrary.Thread;
using MSLibrary.LanguageTranslate;
using MSLibrary.Transaction;

namespace MSLibrary.FileManagement.DAL
{
    [Injection(InterfaceType = typeof(IUploadFileStore), Scope = InjectionScope.Singleton)]
    public class UploadFileStore : IUploadFileStore
    {
        private static string _uploadFileDraftHashGroupName;
        /// <summary>
        /// 上传文件（草稿）需要用到的一致性哈希组的名称
        /// 需要在系统初始化时赋值
        /// </summary>
        public static string UploadFileDraftHashGroupName
        {
            set
            {
                _uploadFileDraftHashGroupName = value;
            }
        }

        private static string _uploadFileHashGroupName;
        /// <summary>
        /// 上传文件需要用到的一致性哈希组的名称
        /// 需要在系统初始化时赋值
        /// </summary>
        public static string UploadFileHashGroupName
        {
            set
            {
                _uploadFileHashGroupName = value;
            }
        }


        private static string _uploadFileDeleteHashGroupName;
        /// <summary>
        /// 上传文件(删除)需要用到的一致性哈希组的名称
        /// 需要在系统初始化时赋值
        /// </summary>
        public static string UploadFileDeleteHashGroupName
        {
            set
            {
                _uploadFileDeleteHashGroupName = value;
            }
        }


        private IHashGroupRepositoryCacheProxy _hashGroupRepository;
        private IFileManagementConnectionFactory _uploadFileConnectionFactory;

        private IStoreInfoResolveService _storeInfoResolveService;


        public UploadFileStore(IHashGroupRepositoryCacheProxy hashGroupRepository, IFileManagementConnectionFactory uploadFileConnectionFactory, IStoreInfoResolveService storeInfoResolveService)
        {
            _hashGroupRepository = hashGroupRepository;
            _uploadFileConnectionFactory = uploadFileConnectionFactory;
            _storeInfoResolveService = storeInfoResolveService;
        }


        public async Task Add(UploadFile uploadFile)
        {
            var storeInfo = await GetDBAllStoreInfo(uploadFile.RegardingType, uploadFile.RegardingKey, _uploadFileDraftHashGroupName);
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, storeInfo[0], async (conn, transaction) =>
            {
                //新增
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
                    if (uploadFile.ID == Guid.Empty)
                    {
                        command.CommandText = string.Format(@"
                                                INSERT INTO {0}
                                                     (
		                                               [id]
                                                      ,[uniquename]
	                                                  ,[displayname]
	                                                  ,[filetype]
	                                                  ,[suffix]
	                                                  ,[size]
	                                                  ,[regardingtype]
	                                                  ,[regardingkey]
	                                                  ,[sourcetype]
	                                                  ,[sourcekey]
	                                                  ,[status]
                                                      ,[createtime]
                                                      ,[modifytime]
                                                     )
                                                VALUES
                                                    (default
                                                    , @uniquename
                                                    , @displayname
                                                    , @filetype
                                                    , @suffix
                                                    , @size
                                                    , @regardingtype
                                                    , @regardingkey
                                                    , @sourcetype
                                                    , @sourcekey
                                                    , @status
                                                    , GETUTCDATE()
                                                    , GETUTCDATE());
                                                select @newid =[id] from {0} where [sequence] = SCOPE_IDENTITY()", storeInfo[1]);
                        parameter = new SqlParameter("@newid", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        command.CommandText = string.Format(@"
                                                INSERT INTO {0}
                                                     (
                                                       [id]
                                                      ,[uniquename]
	                                                  ,[displayname]
	                                                  ,[filetype]
	                                                  ,[suffix]
	                                                  ,[size]
	                                                  ,[regardingtype]
	                                                  ,[regardingkey]
	                                                  ,[sourcetype]
	                                                  ,[sourcekey]
	                                                  ,[status]
                                                      ,[createtime]
                                                      ,[modifytime]
                                                     )
                                                VALUES
                                                    (
                                                      @id
                                                    , @uniquename
                                                    , @displayname
                                                    , @filetype
                                                    , @suffix
                                                    , @size
                                                    , @regardingtype
                                                    , @regardingkey
                                                    , @sourcetype
                                                    , @sourcekey
                                                    , @status
                                                    , GETUTCDATE()
                                                    , GETUTCDATE());", storeInfo[1]);

                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = uploadFile.ID
                        };
                        command.Parameters.Add(parameter);
                    }

                    parameter = new SqlParameter("@uniquename", SqlDbType.NVarChar, 400)
                    {
                        Value = uploadFile.UniqueName
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@displayname", SqlDbType.NVarChar, 400)
                    {
                        Value = uploadFile.DisplayName
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@filetype", SqlDbType.Int)
                    {
                        Value = uploadFile.FileType
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@suffix", SqlDbType.NVarChar, 50)
                    {
                        Value = uploadFile.Suffix
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@size", SqlDbType.BigInt)
                    {
                        Value = uploadFile.Size
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@regardingtype", SqlDbType.NVarChar, 500)
                    {
                        Value = uploadFile.RegardingType
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@regardingkey", SqlDbType.NVarChar, 500)
                    {
                        Value = uploadFile.RegardingKey
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@sourcetype", SqlDbType.NVarChar, 500)
                    {
                        Value = uploadFile.SourceType
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@sourcekey", SqlDbType.NVarChar, 500)
                    {
                        Value = uploadFile.SourceKey
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@status", SqlDbType.Int)
                    {
                        Value = uploadFile.Status
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
                                    Code = TextCodes.ExistUploadFileByName,
                                    DefaultFormatting = "上传文件中存在相同的名称{0}的数据",
                                    ReplaceParameters = new List<object>() { uploadFile.UniqueName }
                                };

                                throw new UtilityException((int)Errors.ExistUploadFileByName, fragment);
                            }
                            else
                            {
                                throw;
                            }
                        }
                
                    //如果用户未赋值ID则创建成功后返回ID
                    if (uploadFile.ID == Guid.Empty)
                    {
                        uploadFile.ID = (Guid)command.Parameters["@newid"].Value;
                    };
                }

            });

        }
        private async Task Add(UploadFile uploadFile, string[] storeInfo)
        {

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, storeInfo[0], async (conn, transaction) =>
            {
                //新增
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
                    if (uploadFile.ID == Guid.Empty)
                    {
                        command.CommandText = string.Format(@"
                                                INSERT INTO {0}
                                                     (
		                                               [id]
                                                      ,[uniquename]
	                                                  ,[displayname]
	                                                  ,[filetype]
	                                                  ,[suffix]
	                                                  ,[size]
	                                                  ,[regardingtype]
	                                                  ,[regardingkey]
	                                                  ,[sourcetype]
	                                                  ,[sourcekey]
	                                                  ,[status]
                                                      ,[createtime]
                                                      ,[modifytime]
                                                     )
                                                VALUES
                                                    (default
                                                    , @uniquename
                                                    , @displayname
                                                    , @filetype
                                                    , @suffix
                                                    , @size
                                                    , @regardingtype
                                                    , @regardingkey
                                                    , @sourcetype
                                                    , @sourcekey
                                                    , @status
                                                    , GETUTCDATE()
                                                    , GETUTCDATE());
                                                select @newid =[id] from {0} where [sequence] = SCOPE_IDENTITY()", storeInfo[1]);
                        parameter = new SqlParameter("@newid", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        command.CommandText = string.Format(@"
                                                INSERT INTO {0}
                                                     (
                                                       [id]
                                                      ,[uniquename]
	                                                  ,[displayname]
	                                                  ,[filetype]
	                                                  ,[suffix]
	                                                  ,[size]
	                                                  ,[regardingtype]
	                                                  ,[regardingkey]
	                                                  ,[sourcetype]
	                                                  ,[sourcekey]
	                                                  ,[status]
                                                      ,[createtime]
                                                      ,[modifytime]
                                                     )
                                                VALUES
                                                    (
                                                    @id
                                                    , @uniquename
                                                    , @displayname
                                                    , @filetype
                                                    , @suffix
                                                    , @size
                                                    , @regardingtype
                                                    , @regardingkey
                                                    , @sourcetype
                                                    , @sourcekey
                                                    , @status
                                                    , GETUTCDATE()
                                                    , GETUTCDATE());", storeInfo[1]);

                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = uploadFile.ID
                        };
                        command.Parameters.Add(parameter);
                    }

                    parameter = new SqlParameter("@uniquename", SqlDbType.NVarChar, 400)
                    {
                        Value = uploadFile.UniqueName
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@displayname", SqlDbType.NVarChar, 400)
                    {
                        Value = uploadFile.DisplayName
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@filetype", SqlDbType.Int)
                    {
                        Value = uploadFile.FileType
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@suffix", SqlDbType.NVarChar, 50)
                    {
                        Value = uploadFile.Suffix
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@size", SqlDbType.BigInt)
                    {
                        Value = uploadFile.Size
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@regardingtype", SqlDbType.NVarChar, 500)
                    {
                        Value = uploadFile.RegardingType
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@regardingkey", SqlDbType.NVarChar, 500)
                    {
                        Value = uploadFile.RegardingKey
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@sourcetype", SqlDbType.NVarChar, 500)
                    {
                        Value = uploadFile.SourceType
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@sourcekey", SqlDbType.NVarChar, 500)
                    {
                        Value = uploadFile.SourceKey
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@status", SqlDbType.Int)
                    {
                        Value = uploadFile.Status
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
                                    Code = TextCodes.ExistUploadFileByName,
                                    DefaultFormatting = "上传文件中存在相同的名称{0}的数据",
                                    ReplaceParameters = new List<object>() { uploadFile.UniqueName }
                                };

                                throw new UtilityException((int)Errors.ExistUploadFileByName, fragment);
                            }
                            else
                            {
                                throw;
                            }
                        }
                
                    //如果用户未赋值ID则创建成功后返回ID
                    if (uploadFile.ID == Guid.Empty)
                    {
                        uploadFile.ID = (Guid)command.Parameters["@newid"].Value;
                    };
                }

            });

        }

        public async Task Delete(string regardingType, string regardingKey, Guid id)
        {
            var draftStoreInfo = await GetDBAllStoreInfo(regardingType, regardingKey, _uploadFileDraftHashGroupName);
            var storeInfo = await GetDBAllStoreInfo(regardingType, regardingKey, _uploadFileHashGroupName);
            var deleteStoreInfo = await GetDBAllStoreInfo(regardingType, regardingKey, _uploadFileDeleteHashGroupName);
            //分别删除
            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 10) }))
            {
                await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, draftStoreInfo[0], async (conn, transaction) =>
                {
                    //删除
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

                        command.CommandText = string.Format(@"delete {0} where id = @id;", draftStoreInfo[1]);

                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = id
                        };
                        command.Parameters.Add(parameter);

                        await command.PrepareAsync();
  
                            await command.ExecuteNonQueryAsync();
                        
                    }
                });
                await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, storeInfo[0], async (conn, transaction) =>
                {
                    //删除
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

                        command.CommandText = string.Format(@"delete {0} where id = @id;", storeInfo[1]);

                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = id
                        };
                        command.Parameters.Add(parameter);

                        await command.PrepareAsync();

                        await command.ExecuteNonQueryAsync();

                    }


                });
                await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, deleteStoreInfo[0], async (conn, transaction) =>
                {
                    //删除
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

                        command.CommandText = string.Format(@"delete {0} where id = @id;", deleteStoreInfo[1]);

                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = id
                        };
                        command.Parameters.Add(parameter);

                        await command.PrepareAsync();

                        await command.ExecuteNonQueryAsync();

                    }
                });

                scope.Complete();
            }

        }


        private async Task Delete(Guid id, string[] storeInfo)
        {
            //分别删除
            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 10) }))
            {
                await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, storeInfo[0], async (conn, transaction) =>
                {
                    //删除
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

                        command.CommandText = string.Format(@"delete {0} where id = @id;", storeInfo[1]);

                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = id
                        };
                        command.Parameters.Add(parameter);

                        await command.PrepareAsync();
 
                        await command.ExecuteNonQueryAsync();
                       
                    }


                });
                scope.Complete();
            }

        }
        public async Task<UploadFile> QueryById(string regardingType, string regardingKey, Guid id)
        {
            var draftStoreInfo = await GetDBAllStoreInfo(regardingType, regardingKey, _uploadFileDraftHashGroupName);
            var storeInfo = await GetDBAllStoreInfo(regardingType, regardingKey, _uploadFileHashGroupName);
            var deleteStoreInfo = await GetDBAllStoreInfo(regardingType, regardingKey, _uploadFileDeleteHashGroupName);

            UploadFile uploadFile = new UploadFile();
            //分别查找，只要有一个找到就返回

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, draftStoreInfo[0], async (conn, transaction) =>
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
                    CommandText = string.Format(@"SELECT {0} FROM {1} WHERE id=@id", StoreHelper.GetUploadFileSelectFields(string.Empty), draftStoreInfo[1]),
                    Transaction = sqlTran,
                })
                {
                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
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
                            StoreHelper.SetUploadFileFields(uploadFile, reader, string.Empty);
                        }
                        await reader.CloseAsync();
                    }
                }

            });
            if (uploadFile.ID != Guid.Empty)
            {
                return uploadFile;
            }

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, storeInfo[0], async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }
                using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"SELECT {0} FROM {1} WHERE id=@id", StoreHelper.GetUploadFileSelectFields(string.Empty), storeInfo[1]),
                    Transaction = sqlTran,
                })
                {
                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
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
                            StoreHelper.SetUploadFileFields(uploadFile, reader, string.Empty);
                        }
                        await reader.CloseAsync();
                    }
                }
            });

            if (uploadFile.ID != Guid.Empty)
            {
                return uploadFile;
            }

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, deleteStoreInfo[0], async (conn, transaction) =>
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
                    CommandText = string.Format(@"SELECT {0} FROM {1} WHERE id=@id", StoreHelper.GetUploadFileSelectFields(string.Empty), deleteStoreInfo[1]),
                    Transaction = sqlTran,
                })
                {
                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
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
                            StoreHelper.SetUploadFileFields(uploadFile, reader, string.Empty);
                        }
                        await reader.CloseAsync();
                    }
                }

            });
            if (uploadFile.ID == Guid.Empty)
            {
                return null;
            }

            return uploadFile;
        }

        public UploadFile QueryByIdSync(string regardingType, string regardingKey, Guid id)
        {
            var draftStoreInfo = GetDBAllStoreInfoSync(regardingType, regardingKey, _uploadFileDraftHashGroupName);
            var storeInfo = GetDBAllStoreInfoSync(regardingType, regardingKey, _uploadFileHashGroupName);
            var deleteStoreInfo = GetDBAllStoreInfoSync(regardingType, regardingKey, _uploadFileDeleteHashGroupName);

            UploadFile uploadFile = new UploadFile();
            //分别查找，只要有一个找到就返回

            DBTransactionHelper.SqlTransactionWork(DBTypes.SqlServer, false, false, draftStoreInfo[0], (conn, transaction) =>
           {
               SqlTransaction sqlTran = null;
               if (transaction != null)
               {
                   sqlTran = (SqlTransaction)transaction;
               }
               using (SqlCommand command = new SqlCommand()
               {
                   Connection = (SqlConnection)conn,
                   CommandType = CommandType.Text,
                   CommandText = string.Format(@"SELECT {0} FROM {1} WHERE id=@id", StoreHelper.GetUploadFileSelectFields(string.Empty), draftStoreInfo[1]),
                   Transaction = sqlTran,
               })
               {
                   var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                   {
                       Value = id
                   };
                   command.Parameters.Add(parameter);
                   command.Prepare();
                   SqlDataReader reader = null;

                   using (reader = command.ExecuteReader())
                   {
                       if (reader.Read())
                       {
                           StoreHelper.SetUploadFileFields(uploadFile, reader, string.Empty);
                       }
                       reader.Close();
                   }
               }

           });
            if (uploadFile.ID != Guid.Empty)
            {
                return uploadFile;
            }

            DBTransactionHelper.SqlTransactionWork(DBTypes.SqlServer, false, false, storeInfo[0], (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }
                using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"SELECT {0} FROM {1} WHERE id=@id", StoreHelper.GetUploadFileSelectFields(string.Empty), storeInfo[1]),
                    Transaction = sqlTran,
                })
                {
                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);
                    command.Prepare();
                    SqlDataReader reader = null;

                    using (reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            StoreHelper.SetUploadFileFields(uploadFile, reader, string.Empty);
                        }
                        reader.Close();
                    }
                }
            });

            if (uploadFile.ID != Guid.Empty)
            {
                return uploadFile;
            }

            DBTransactionHelper.SqlTransactionWork(DBTypes.SqlServer, false, false, deleteStoreInfo[0], (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }
                using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"SELECT {0} FROM {1} WHERE id=@id", StoreHelper.GetUploadFileSelectFields(string.Empty), deleteStoreInfo[1]),
                    Transaction = sqlTran,
                })
                {
                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);
                    command.Prepare();
                    SqlDataReader reader = null;

                    using (reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            StoreHelper.SetUploadFileFields(uploadFile, reader, string.Empty);
                        }
                        reader.Close();
                    }
                }

            });
            if (uploadFile.ID == Guid.Empty)
            {
                return null;
            }

            return uploadFile;
        }


        public async Task<QueryResult<UploadFile>> QueryByRegarding(string regardingType, string regardingKey, int status, int page, int pageSize)
        {
            string[] storeInfo = null;
            switch (status)
            {
                case 0:
                    storeInfo = await GetDBReadStoreInfo(regardingType, regardingKey, _uploadFileDraftHashGroupName);
                    break;
                case 1:
                    storeInfo = await GetDBReadStoreInfo(regardingType, regardingKey, _uploadFileHashGroupName);
                    break;
                default:
                    storeInfo = await GetDBReadStoreInfo(regardingType, regardingKey, _uploadFileDeleteHashGroupName);
                    break;
            }

            QueryResult<UploadFile> result = new QueryResult<UploadFile>();

            //执行查询
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, storeInfo[0], async (conn, transaction) =>
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
                    CommandText = string.Format(@"SET @currentpage = @page;
                                                    SELECT @count = COUNT(*)
                                                    FROM [dbo].{0}
                                                    WHERE [status] = @status and [regardingtype]=@regardingtype and [regardingkey]=@regardingkey;
                                                    IF @pagesize * @page >= @count
                                                    BEGIN
                                                        SET @currentpage = @count / @pagesize;
                                                        IF @count % @pagesize <> 0
                                                        BEGIN
                                                            SET @currentpage = @currentpage + 1;
                                                        END;
                                                        IF @currentpage = 0
                                                            SET @currentpage = 1;
                                                    END;
                                                    ELSE IF @page < 1
                                                    BEGIN
                                                        SET @currentpage = 1;
                                                    END;

                                                    SELECT {1}
                                                    FROM [dbo].{0}
                                                    WHERE [status] = @status and [regardingtype]=@regardingtype and [regardingkey]=@regardingkey
                                                    ORDER BY sequence OFFSET (@pagesize * (@currentpage - 1)) ROWS FETCH NEXT @pagesize ROWS ONLY;", storeInfo[1], StoreHelper.GetUploadFileSelectFields(string.Empty)),
                    Transaction = sqlTran,
                })
                {
                    var parameter = new SqlParameter("@status", SqlDbType.Int)
                    {
                        Value = status
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@regardingtype", SqlDbType.NVarChar, 500)
                    {
                        Value = regardingType
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@regardingkey", SqlDbType.NVarChar, 500)
                    {
                        Value = regardingKey
                    };
                    command.Parameters.Add(parameter);


                    parameter = new SqlParameter("@page", SqlDbType.Int)
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

                    parameter = new SqlParameter("@currentpage", SqlDbType.Int)
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
                            var template = new UploadFile();
                            StoreHelper.SetUploadFileFields(template, reader, string.Empty);
                            result.Results.Add(template);
                        }
                        await reader.CloseAsync();
                        result.TotalCount = (int)command.Parameters["@count"].Value;
                        result.CurrentPage = (int)command.Parameters["@currentpage"].Value;
                    }
                }
            });



            return result;

        }

        public async Task<List<UploadFile>> QueryTop(int status, int top)
        {
            List<UploadFile> result = new List<UploadFile>();
            object lockObj = new object();
            List<string[]> storeInfo = null;
            switch (status)
            {
                case 0:
                    storeInfo = await GetDBReadStoreInfos(_uploadFileDraftHashGroupName);
                    break;
                case 1:
                    storeInfo = await GetDBReadStoreInfos(_uploadFileHashGroupName);
                    break;
                default:
                    storeInfo = await GetDBReadStoreInfos(_uploadFileDeleteHashGroupName);
                    break;
            }

            await ParallelHelper.ForEach(storeInfo, 10, async (item) =>
            {
                await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, item[0], async (conn, transaction) =>
                {
                    List<UploadFile> partResult = new List<UploadFile>();
                    //执行查询，填充结果partResult

                    SqlTransaction sqlTran = null;
                    if (transaction != null)
                    {
                        sqlTran = (SqlTransaction)transaction;
                    }
                    await using (SqlCommand command = new SqlCommand()
                    {
                        Connection = (SqlConnection)conn,
                        CommandType = CommandType.Text,
                        CommandText = string.Format(@"SELECT TOP (@top) {0} FROM {1} order by createtime", StoreHelper.GetUploadFileSelectFields(string.Empty), item[1]),
                        Transaction = sqlTran,
                    })
                    {
                        var parameter = new SqlParameter("@top", SqlDbType.Int)
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
                                var template = new UploadFile();
                                StoreHelper.SetUploadFileFields(template, reader, string.Empty);
                                partResult.Add(template);
                            }
                            await reader.CloseAsync();
                        }
                    }

                    //与result里的数据做比较
                    lock (lockObj)
                    {
                        result.AddRange(partResult);
                        result = result.GroupBy((gItem) => gItem.ID).Select((gItem) => gItem.FirstOrDefault()).ToList();

                        result = (from resultItem in result
                                  orderby resultItem.CreateTime
                                  select resultItem).Take(top).ToList();
                    }

                });
            });


            return result;
        }

        public async Task<List<UploadFile>> QueryTopBefore(int status, int top, DateTime dateTime)
        {
            List<UploadFile> result = new List<UploadFile>();
            object lockObj = new object();

            List<string[]> storeInfo = null;
            switch (status)
            {
                case 0:
                    storeInfo = await GetDBReadStoreInfos(_uploadFileDraftHashGroupName);
                    break;
                case 1:
                    storeInfo = await GetDBReadStoreInfos(_uploadFileHashGroupName);
                    break;
                default:
                    storeInfo = await GetDBReadStoreInfos(_uploadFileDeleteHashGroupName);
                    break;
            }

            await ParallelHelper.ForEach(storeInfo, 10, async (item) =>
            {
                await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, item[0], async (conn, transaction) =>
                {
                    List<UploadFile> partResult = new List<UploadFile>();
                    //执行查询，填充结果partResult

                    SqlTransaction sqlTran = null;
                    if (transaction != null)
                    {
                        sqlTran = (SqlTransaction)transaction;
                    }
                    await using (SqlCommand command = new SqlCommand()
                    {
                        Connection = (SqlConnection)conn,
                        CommandType = CommandType.Text,
                        CommandText = string.Format(@"SELECT TOP (@top) {0} FROM {1} where createtime <= @createtime order by createtime", StoreHelper.GetUploadFileSelectFields(string.Empty), item[1]),
                        Transaction = sqlTran,
                    })
                    {
                        var parameter = new SqlParameter("@top", SqlDbType.Int)
                        {
                            Value = top
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter("@createtime", SqlDbType.DateTime)
                        {
                            Value = dateTime
                        };
                        command.Parameters.Add(parameter);

                        await command.PrepareAsync();
                        SqlDataReader reader = null;

                        await using (reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var template = new UploadFile();
                                StoreHelper.SetUploadFileFields(template, reader, string.Empty);
                                partResult.Add(template);
                            }
                            await reader.CloseAsync();
                        }
                    }

                    //与result里的数据做比较
                    lock (lockObj)
                    {
                        result.AddRange(partResult);
                        result = result.GroupBy((gItem) => gItem.ID).Select((gItem) => gItem.FirstOrDefault()).ToList();

                        result = (from resultItem in result
                                  orderby resultItem.CreateTime
                                  select resultItem).Take(top).ToList();
                    }

                });
            });


            return result;
        }

        public async Task Update(UploadFile uploadFile)
        {
            string[] storeInfo = null;
            switch (uploadFile.Status)
            {
                case 0:
                    storeInfo = await GetDBAllStoreInfo(uploadFile.RegardingType, uploadFile.RegardingKey, _uploadFileDraftHashGroupName);
                    break;
                case 1:
                    storeInfo = await GetDBAllStoreInfo(uploadFile.RegardingType, uploadFile.RegardingKey, _uploadFileHashGroupName);
                    break;
                default:
                    storeInfo = await GetDBAllStoreInfo(uploadFile.RegardingType, uploadFile.RegardingKey, _uploadFileDeleteHashGroupName);
                    break;
            }

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, storeInfo[0], async (conn, transaction) =>
            {
                //修改
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

                    command.CommandText = string.Format(@"
                                                            update {0}
                                                            set uniquename=@uniquename
                                                                ,displayname=@displayname
                                                                ,filetype=@filetype
                                                                ,suffix=@suffix
                                                                ,size=@size
                                                                ,regardingtype=@regardingtype
                                                                ,regardingkey=@regardingkey
                                                                ,sourcetype=@sourcetype
                                                                ,sourcekey=@sourcekey
                                                                ,status=@status
                                                                ,[modifytime] = GETUTCDATE()
                                                            where  id = @id", storeInfo[1]);

                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = uploadFile.ID
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@uniquename", SqlDbType.NVarChar, 400)
                    {
                        Value = uploadFile.UniqueName
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@displayname", SqlDbType.NVarChar, 400)
                    {
                        Value = uploadFile.DisplayName
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@filetype", SqlDbType.Int)
                    {
                        Value = uploadFile.FileType
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@suffix", SqlDbType.NVarChar, 50)
                    {
                        Value = uploadFile.Suffix
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@size", SqlDbType.BigInt)
                    {
                        Value = uploadFile.Size
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@regardingtype", SqlDbType.NVarChar, 500)
                    {
                        Value = uploadFile.RegardingType
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@regardingkey", SqlDbType.NVarChar, 500)
                    {
                        Value = uploadFile.RegardingKey
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@sourcetype", SqlDbType.NVarChar, 500)
                    {
                        Value = uploadFile.SourceType
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@sourcekey", SqlDbType.NVarChar, 500)
                    {
                        Value = uploadFile.SourceKey
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@status", SqlDbType.Int)
                    {
                        Value = uploadFile.Status
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
                                    Code = TextCodes.ExistUploadFileByName,
                                    DefaultFormatting = "上传文件中存在相同的名称{0}的数据",
                                    ReplaceParameters = new List<object>() { uploadFile.UniqueName }
                                };

                                throw new UtilityException((int)Errors.ExistUploadFileByName, fragment);
                            }
                            else
                            {
                                throw;
                            }
                        }
               
                }

            });

        }

        public async Task UpdateStatus(UploadFile uploadFile, int status)
        {
            var draftStoreInfo = await GetDBAllStoreInfo(uploadFile.RegardingType, uploadFile.RegardingKey, _uploadFileDraftHashGroupName);

            var storeInfo = await GetDBAllStoreInfo(uploadFile.RegardingType, uploadFile.RegardingKey, _uploadFileHashGroupName);

            var deleteStoreInfo = await GetDBAllStoreInfo(uploadFile.RegardingType, uploadFile.RegardingKey, _uploadFileDeleteHashGroupName);

            //状态 0，草稿 1，已提交 2，已删除
            switch (uploadFile.Status)
            {
                case 0:
                    //由【草稿】状态更新为【已提交】
                    if (status == 1)
                    {
                        //从草稿状态中删除
                        await Delete(uploadFile.ID, draftStoreInfo);

                        //新增到提交状态
                        uploadFile.Status = status;
                        //uploadFile.ID = Guid.NewGuid();
                        await Add(uploadFile, storeInfo);
                    }
                    break;
                case 1:
                    //由【已提交】状态更新为【已删除】
                    if (status == 2)
                    {
                        //从提交状态中删除
                        await Delete(uploadFile.ID, storeInfo);

                        //新增到删除状态
                        uploadFile.Status = status;
                        //uploadFile.ID = Guid.NewGuid();
                        await Add(uploadFile, deleteStoreInfo);
                    }
                    break;
                default:
                    //删除状态，无法转成任何状态
                    break;
            }

        }


        private async Task<string[]> GetDBAllStoreInfo(string regardingType, string regardingKey, string hashGroupName)
        {
            string[] info = new string[2];

            //获取前缀的哈希节点关键字,
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo(_storeInfoResolveService,_hashGroupRepository, hashGroupName, regardingType, regardingKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.UploadFile, out string tableName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() {hashGroupName, HashEntityNames.UploadFile }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }

            //获取连接字符串
            info[0] = _uploadFileConnectionFactory.CreateAllForUploadFile(dbInfo.DBConnectionNames);
            info[1] = tableName;

            return info;
        }
        private string[] GetDBAllStoreInfoSync(string regardingType, string regardingKey, string hashGroupName)
        {
            string[] info = new string[2];

            //获取前缀的哈希节点关键字,
            var dbInfo = StoreInfoHelper.GetHashStoreInfoSync(_storeInfoResolveService,_hashGroupRepository, hashGroupName, regardingType, regardingKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.UploadFile, out string tableName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { hashGroupName, HashEntityNames.UploadFile }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }

            //获取连接字符串
            info[0] = _uploadFileConnectionFactory.CreateAllForUploadFile(dbInfo.DBConnectionNames);
            info[1] = tableName;

            return info;
        }

        private async Task<string[]> GetDBReadStoreInfo(string regardingType, string regardingKey, string hashGroupName)
        {
            string[] info = new string[2];

            //获取前缀的哈希节点关键字,
            var dbInfo = await StoreInfoHelper.GetHashStoreInfo( _storeInfoResolveService,_hashGroupRepository, hashGroupName, regardingType, regardingKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.UploadFile, out string tableName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { hashGroupName, HashEntityNames.UploadFile }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo,fragment);
            }


            //获取连接字符串
            info[0] = _uploadFileConnectionFactory.CreateReadForUploadFile(dbInfo.DBConnectionNames);
            info[1] = tableName;

            return info;
        }

        private string[] GetDBReadStoreInfoSync(string regardingType, string regardingKey, string hashGroupName)
        {
            string[] info = new string[2];

            //获取前缀的哈希节点关键字,
            var dbInfo = StoreInfoHelper.GetHashStoreInfoSync(_storeInfoResolveService,_hashGroupRepository, hashGroupName, regardingType, regardingKey);

            if (!dbInfo.TableNames.TryGetValue(HashEntityNames.UploadFile, out string tableName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { hashGroupName, HashEntityNames.UploadFile }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }


            //获取连接字符串
            info[0] = _uploadFileConnectionFactory.CreateReadForUploadFile(dbInfo.DBConnectionNames);
            info[1] = tableName;

            return info;
        }

        private async Task<List<string[]>> GetDBReadStoreInfos(string hashGroupName)
        {
            List<string[]> result = new List<string[]>();

            var group = await _hashGroupRepository.QueryByName(hashGroupName);
            if (group == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundHashGroupByName,
                    DefaultFormatting = "没有找到名称为{0}的一致性哈希组",
                    ReplaceParameters = new List<object>() { hashGroupName }
                };

                throw new UtilityException((int)Errors.NotFoundHashGroupByName, fragment);
            }


            var storeInfos = await StoreInfoHelper.GetHashStoreInfos(_storeInfoResolveService,_hashGroupRepository, hashGroupName);

            foreach (var infoItem in storeInfos)
            {
                if (!infoItem.TableNames.TryGetValue(HashEntityNames.UploadFile, out string tableName))
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                        DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                        ReplaceParameters = new List<object>() { hashGroupName, HashEntityNames.UploadFile }
                    };

                    throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
                }

                //获取连接字符串
                string[] info = new string[2];
                var strConn = _uploadFileConnectionFactory.CreateReadForUploadFile(infoItem.DBConnectionNames);
                info[0] = strConn;
                info[1] = tableName;
                result.Add(info);
            }

            return result;
        }
        
        public async Task<UploadFile> QueryByUniqueName(string regardingType, string regardingKey, int status, string uniqueName)
        {
            string[] storeInfo = null;
            switch (status)
            {
                case 0:
                    storeInfo = await GetDBAllStoreInfo(regardingType, regardingKey, _uploadFileDraftHashGroupName);
                    break;
                case 1:
                    storeInfo = await GetDBAllStoreInfo(regardingType, regardingKey, _uploadFileHashGroupName);
                    break;
                default:
                    storeInfo = await GetDBAllStoreInfo(regardingType, regardingKey, _uploadFileDeleteHashGroupName);
                    break;
            }

            UploadFile uploadFile = new UploadFile();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, storeInfo[0], async (conn, transaction) =>
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
                    CommandText = string.Format(@"SELECT {0} FROM {1} WHERE uniquename=@uniquename and [regardingtype]=@regardingtype and [regardingkey]=@regardingkey", StoreHelper.GetUploadFileSelectFields(string.Empty), storeInfo[1]),
                    Transaction = sqlTran,
                })
                {
                    var parameter = new SqlParameter("@uniquename", SqlDbType.NVarChar, 400)
                    {
                        Value = uniqueName
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@regardingtype", SqlDbType.NVarChar, 500)
                    {
                        Value = regardingType
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@regardingkey", SqlDbType.NVarChar, 500)
                    {
                        Value = regardingKey
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();
                    SqlDataReader reader = null;


                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            StoreHelper.SetUploadFileFields(uploadFile, reader, string.Empty);
                        }
                        await reader.CloseAsync();
                    }
                }

            });

            return uploadFile;
        }

        public async Task QueryByRegarding(string regardingType, string regardingKey, int status, Func<UploadFile, Task> callback)
        {
            string[] storeInfo = null;
            switch (status)
            {
                case 0:
                    storeInfo = await GetDBAllStoreInfo(regardingType, regardingKey, _uploadFileDraftHashGroupName);
                    break;
                case 1:
                    storeInfo = await GetDBAllStoreInfo(regardingType, regardingKey, _uploadFileHashGroupName);
                    break;
                default:
                    storeInfo = await GetDBAllStoreInfo(regardingType, regardingKey, _uploadFileDeleteHashGroupName);
                    break;
            }

            List<UploadFile> result = new List<UploadFile>();
            int size = 500;
            int pageSize = 500;
            int offset = 0;

            while (size == pageSize)
            {
                result.Clear();

                await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, storeInfo[0], async (conn, transaction) =>
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
                        CommandText = string.Format(@"
                                                        SELECT {0}
                                                        FROM {1}
                                                        WHERE [status] = @status and [regardingtype]=@regardingtype and [regardingkey]=@regardingkey
                                                        ORDER BY sequence OFFSET (@offset) ROWS FETCH NEXT @pagesize ROWS ONLY;"
                                                   , StoreHelper.GetUploadFileSelectFields(string.Empty), storeInfo[1]),
                        Transaction = sqlTran,
                    })
                    {
                        var parameter = new SqlParameter("@status", SqlDbType.Int)
                        {
                            Value = status
                        };
                        command.Parameters.Add(parameter);


                        parameter = new SqlParameter("@regardingtype", SqlDbType.NVarChar, 500)
                        {
                            Value = regardingType
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter("@regardingkey", SqlDbType.NVarChar, 500)
                        {
                            Value = regardingKey
                        };
                        command.Parameters.Add(parameter);


                        parameter = new SqlParameter("@pagesize", SqlDbType.Int)
                        {
                            Value = pageSize
                        };
                        command.Parameters.Add(parameter);
                        parameter = new SqlParameter("@offset", SqlDbType.Int)
                        {
                            Value = offset
                        };
                        command.Parameters.Add(parameter);
                        await command.PrepareAsync();
                        SqlDataReader reader = null;

                        using (reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var template = new UploadFile();
                                StoreHelper.SetUploadFileFields(template, reader, string.Empty);
                                result.Add(template);
                            }
                            await reader.CloseAsync();

                        }
                    }
                });

                offset += result.Count;
                pageSize = result.Count;

                foreach (var item in result)
                {
                    await callback(item);
                }
            }

        }
    }
}
