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

namespace MSLibrary.SMS.DAL
{
    /// <summary>
    /// 短信发送终结点
    /// 负责短信的真正发送
    /// </summary>
    [Injection(InterfaceType = typeof(ISMSSendEndpointStore), Scope = InjectionScope.Singleton)]
    public class SMSSendEndpointStore : ISMSSendEndpointStore
    {
        private ISMSConnectionFactory _dbConnectionFactory;

        public SMSSendEndpointStore(ISMSConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task Add(SMSSendEndpoint endpoint)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateSMSRecordAllForSMS(), async (conn, transaction) =>
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
                    if (endpoint.ID == Guid.Empty)
                    {
                        command.CommandText = @"
                                                INSERT INTO [dbo].[SMSSendEndpoint]
                                                     (
		                                               [id]
                                                      ,[name]
                                                      ,[configuration]
                                                      ,[createtime]
                                                      ,[modifytime]
                                                     )
                                                VALUES
                                                    (default
                                                    , @name
                                                    , @Configuration
                                                    , GETUTCDATE()
                                                    , GETUTCDATE());
                                                select @newid =[id] from [dbo].[SMSSendEndpoint] where [sequence] = SCOPE_IDENTITY()";
                        parameter = new SqlParameter("@newid", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        command.CommandText = @"INSERT INTO [dbo].[SMSSendEndpoint]
                                                     (
		                                               [id]
                                                      ,[name]
                                                      ,[configuration]
                                                      ,[createtime]
                                                      ,[modifytime]
                                                     )
                                                VALUES
                                                    ( @id
                                                     , @name
                                                     , @configuration
                                                     , GETUTCDATE()
                                                     , GETUTCDATE())";

                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = endpoint.ID
                        };
                        command.Parameters.Add(parameter);
                    }
                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 500)
                    {
                        Value = endpoint.Name
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@configuration", SqlDbType.NVarChar, 4000)
                    {
                        Value = endpoint.Configuration
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
                                    Code = TextCodes.ExistSMSSendEndpointByName,
                                    DefaultFormatting = "短信发送终结点中存在相同的名称\"{0}\"数据",
                                    ReplaceParameters = new List<object>() { endpoint.Name }
                                };

                                throw new UtilityException((int)Errors.ExistSMSSendEndpointByName, fragment);
                            }
                            else
                            {
                                throw;
                            }
                        }
            
                    //如果用户未赋值ID则创建成功后返回ID
                    if (endpoint.ID == Guid.Empty)
                    {
                        endpoint.ID = (Guid)command.Parameters["@newid"].Value;
                    };
                }

            });
        }


        public async Task Delete(SMSSendEndpoint endpoint)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateSMSRecordAllForSMS(), async (conn, transaction) =>
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

                    command.CommandText = @"delete [dbo].[SMSSendEndpoint]
                                            where id = @id";

                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = endpoint.ID
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                        await command.ExecuteNonQueryAsync();
                    
                }
            });

        }

        public async Task Update(SMSSendEndpoint endpoint)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateSMSRecordAllForSMS(), async (conn, transaction) =>
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

                    command.CommandText = @"
                                                Update [dbo].[SMSSendEndpoint]
                                                set  [name] = @name
                                                    ,[configuration] = @Configuration
                                                    ,[modifytime] = GETUTCDATE()
                                                where id = @id";

                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = endpoint.ID
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 500)
                    {
                        Value = endpoint.Name
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@configuration", SqlDbType.NVarChar, 4000)
                    {
                        Value = endpoint.Configuration
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
                                    Code = TextCodes.ExistSMSSendEndpointByName,
                                    DefaultFormatting = "短信发送终结点中存在相同的名称\"{0}\"数据",
                                    ReplaceParameters = new List<object>() { endpoint.Name }
                                };

                                throw new UtilityException((int)Errors.ExistSMSSendEndpointByName, fragment);
                            }
                            else
                            {
                                throw;
                            }
                        }
                }
            });

        }
    }
}
