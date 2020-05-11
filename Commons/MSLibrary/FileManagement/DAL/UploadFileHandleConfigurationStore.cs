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


    [Injection(InterfaceType = typeof(IUploadFileHandleConfigurationStore), Scope = InjectionScope.Singleton)]
    public class UploadFileHandleConfigurationStore : IUploadFileHandleConfigurationStore
    {
        private IFileManagementConnectionFactory _fileManagementConnectionFactory;

        public UploadFileHandleConfigurationStore(IFileManagementConnectionFactory fileManagementConnectionFactory)
        {
            _fileManagementConnectionFactory = fileManagementConnectionFactory;
        }



        public async Task<UploadFileHandleConfiguration> QueryById(Guid id)
        {
            UploadFileHandleConfiguration conf = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _fileManagementConnectionFactory.CreateAllForUploadFileHandle(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"SELECT {0} FROM UploadFileHandleConfiguration WHERE id=@id", StoreHelper.GetUploadFileHandleConfigurationFields(string.Empty)),
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
                            conf = new UploadFileHandleConfiguration();

                            StoreHelper.SetUploadFileHandleConfigurationFields(conf, reader, string.Empty);
                        }
                        await reader.CloseAsync();
                    }
                }
            });

            return conf;
        }

        public async Task<UploadFileHandleConfiguration> QueryByName(string name)
        {
            UploadFileHandleConfiguration conf = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _fileManagementConnectionFactory.CreateAllForUploadFileHandle(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"SELECT {0} FROM UploadFileHandleConfiguration WHERE [name]=@name", StoreHelper.GetUploadFileHandleConfigurationFields(string.Empty)),
                    Transaction = sqlTran,
                })
                {
                    var parameter = new SqlParameter("@name", SqlDbType.NVarChar, 100)
                    {
                        Value = name
                    };
                    command.Parameters.Add(parameter);
                    await command.PrepareAsync();
                    SqlDataReader reader = null;

                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            conf = new UploadFileHandleConfiguration();

                            StoreHelper.SetUploadFileHandleConfigurationFields(conf, reader, string.Empty);
                        }
                        await reader.CloseAsync();
                    }
                }
            });

            return conf;
        }

        public async Task<UploadFileHandleConfiguration> QueryByNameStatus(string name, int status)
        {
            UploadFileHandleConfiguration conf = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _fileManagementConnectionFactory.CreateAllForUploadFileHandle(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"SELECT {0} FROM UploadFileHandleConfiguration WHERE [name]=@name and [status] = @status", StoreHelper.GetUploadFileHandleConfigurationFields(string.Empty)),
                    Transaction = sqlTran,
                })
                {
                    var parameter = new SqlParameter("@name", SqlDbType.NVarChar, 100)
                    {
                        Value = name
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
                            conf = new UploadFileHandleConfiguration();

                            StoreHelper.SetUploadFileHandleConfigurationFields(conf, reader, string.Empty);
                        }
                       await reader.CloseAsync();
                    }
                }
            });

            return conf;
        }
    }
}
