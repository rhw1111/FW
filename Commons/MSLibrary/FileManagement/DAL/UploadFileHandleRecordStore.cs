using MSLibrary.DAL;
using MSLibrary.DI;
using MSLibrary.Transaction;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.FileManagement.DAL
{
    [Injection(InterfaceType = typeof(IUploadFileHandleRecordStore), Scope = InjectionScope.Singleton)]

    public class UploadFileHandleRecordStore : IUploadFileHandleRecordStore
    {

        private IFileManagementConnectionFactory _fileManagementConnectionFactory;

        public UploadFileHandleRecordStore(IFileManagementConnectionFactory fileManagementConnectionFactory)
        {
            this._fileManagementConnectionFactory = fileManagementConnectionFactory;
        }


        public async Task Add(UploadFileHandleRecord record)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _fileManagementConnectionFactory.CreateAllForUploadFileHandle(), async (conn, transaction) =>
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
                    if (record.ID == Guid.Empty)
                    {
                        command.CommandText = @"
                                               insert into [UploadFileHandleRecord]
                                                (
	                                                  [id]
                                                      ,[UploadFileId]
                                                      ,[ConfigurationName]
                                                      ,[Error]
                                                      ,[Status]
                                                      ,[UploadFileRegardingType]
                                                      ,[UploadFileRegardingKey]
                                                      ,[ExtensionInfo]
                                                      ,[ResultInfo]
                                                      ,[createtime]
                                                      ,[modifytime]
                                                 )
                                                 values
                                                 (
	                                                 default
	                                                 ,@UploadFileId
                                                     ,@ConfigurationName
                                                     ,@Error
                                                     ,@Status
                                                     ,@UploadFileRegardingType
                                                     ,@UploadFileRegardingKey
                                                     ,@ExtensionInfo
                                                     ,@ResultInfo
	                                                 ,GETUTCDATE()
	                                                 ,GETUTCDATE()
                                                 );
                                                select @newid =[id] from UploadFileHandleRecord where [sequence] = SCOPE_IDENTITY()";
                        parameter = new SqlParameter("@newid", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        command.CommandText = @"insert into[UploadFileHandleRecord]
                                                (

                                                      [id]
                                                      ,[UploadFileId]
                                                      ,[ConfigurationName]
                                                      ,[Error]
                                                      ,[Status]
                                                      ,[UploadFileRegardingType]
                                                      ,[UploadFileRegardingKey]
                                                      ,[ExtensionInfo]
                                                      ,[ResultInfo]
                                                      ,[createtime]
                                                      ,[modifytime]
                                                 )
                                                 values
                                                 (
                                                     @id
                                                     , @UploadFileId
                                                     , @ConfigurationName
                                                     , @Error
                                                     , @Status
                                                     , @UploadFileRegardingType
                                                     , @UploadFileRegardingKey
                                                     , @ExtensionInfo
                                                     , @ResultInfo
                                                     , GETUTCDATE()
                                                     , GETUTCDATE()
                                                 )";

                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = record.ID
                        };
                        command.Parameters.Add(parameter);
                    }

                    parameter = new SqlParameter("@UploadFileId", SqlDbType.UniqueIdentifier)
                    {
                        Value = record.UploadFileId
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@ConfigurationName", SqlDbType.NVarChar, 2000)
                    {
                        Value = record.ConfigurationName
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@Error", SqlDbType.NVarChar, -1)
                    {
                        Value = (object)record.Error ?? DBNull.Value
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@Status", SqlDbType.Int)
                    {
                        Value = record.Status
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@UploadFileRegardingType", SqlDbType.NVarChar, 500)
                    {
                        Value = record.UploadFileRegardingType
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@UploadFileRegardingKey", SqlDbType.NVarChar, 500)
                    {
                        Value = record.UploadFileRegardingKey
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@ExtensionInfo", SqlDbType.NVarChar, 2000)
                    {
                        Value = (object)record.ExtensionInfo ?? DBNull.Value
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@ResultInfo", SqlDbType.NVarChar, 2000)
                    {
                        Value = (object)record.ResultInfo ?? DBNull.Value
                    };
                    command.Parameters.Add(parameter);


                    await command.PrepareAsync();


                    await command.ExecuteNonQueryAsync();
                 

                    //如果用户未赋值ID则创建成功后返回ID
                    if (record.ID == Guid.Empty)
                    {
                        record.ID = (Guid)command.Parameters["@newid"].Value;
                    };
                }

            });

        }

        public async Task Delete(Guid recordId)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _fileManagementConnectionFactory.CreateAllForUploadFileHandle(), async (conn, transaction) =>
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

                    command.CommandText = @"  delete [UploadFileHandleRecord] where id = @id  ";

                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = recordId
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();


                    await command.ExecuteNonQueryAsync();

                }
            });

        }

        public async Task<UploadFileHandleRecord> QueryId(Guid recordId)
        {
            UploadFileHandleRecord record = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _fileManagementConnectionFactory.CreateAllForUploadFileHandle(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }
               await  using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"SELECT {0} FROM UploadFileHandleRecord WHERE id=@id", StoreHelper.GetUploadFileHandleRecordSelectFields(string.Empty)),
                    Transaction = sqlTran,
                })
                {
                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = recordId
                    };
                    command.Parameters.Add(parameter);
                    await command.PrepareAsync();
                    SqlDataReader reader = null;

                    using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            record = new UploadFileHandleRecord();

                            StoreHelper.SetUploadFileHandleRecordFields(record, reader, string.Empty);
                        }
                        await reader.CloseAsync();
                    }
                }
            });

            return record;
        }

        public async Task UpdateStatus(Guid recordId, int status, string result, string error)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _fileManagementConnectionFactory.CreateAllForUploadFileHandle(), async (conn, transaction) =>
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

                    command.CommandText = @"  update [UploadFileHandleRecord] 
                                              set status = @status
                                              ,error = @error
                                              ,resultinfo = @result
                                              ,modifytime = GETUTCDATE()
                                              where id = @id  ";

                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = recordId
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@status", SqlDbType.Int)
                    {
                        Value = status
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@error", SqlDbType.NVarChar, -1)
                    {
                        Value = string.IsNullOrEmpty(error) ? "" : error
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@result", SqlDbType.NVarChar, 2000)
                    {
                        Value = (object)result ?? DBNull.Value
                    };
                    command.Parameters.Add(parameter);

                   await command.PrepareAsync();


                        await command.ExecuteNonQueryAsync();
                }
            });
        }
    }
}
