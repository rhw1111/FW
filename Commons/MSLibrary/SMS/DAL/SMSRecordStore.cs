using MSLibrary.DAL;
using MSLibrary.DI;
using MSLibrary.Transaction;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SMS.DAL
{

    [Injection(InterfaceType = typeof(ISMSRecordStore), Scope = InjectionScope.Singleton)]
    public class SMSRecordStore : ISMSRecordStore
    {
        private ISMSConnectionFactory _dbConnectionFactory;

        public SMSRecordStore(ISMSConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public async Task Add(SMSRecord record)
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
                    if (record.ID == Guid.Empty)
                    {
                        command.CommandText = @"
                                                INSERT INTO [dbo].[SMSRecord]
                                                     (
		                                                [id]
                                                      ,[phonenumbers]
                                                      ,[content]
                                                      ,[sendendpointname]
                                                      ,[extensioninfo]
                                                      ,[statusdescription]
                                                      ,[status]
                                                      ,[createtime]
                                                      ,[modifytime]
                                                     )
                                                VALUES
                                                    (default
                                                    , @phonenumbers
                                                    , @content
                                                    , @sendendpointname
                                                    , @extensioninfo
                                                    , @statusdescription
                                                    , @status
                                                    , GETUTCDATE()
                                                    , GETUTCDATE());
                                                select @newid =[id] from [dbo].[SMSRecord] where [sequence] = SCOPE_IDENTITY()";
                        parameter = new SqlParameter("@newid", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        command.CommandText = @"
                                                INSERT INTO [dbo].[SMSRecord]
                                                     (
		                                                [id]
                                                      ,[phonenumbers]
                                                      ,[content]
                                                      ,[sendendpointname]
                                                      ,[extensioninfo]
                                                      ,[statusdescription]
                                                      ,[status]
                                                      ,[createtime]
                                                      ,[modifytime]
                                                     )
                                                VALUES
                                                    (@id
                                                    , @phonenumbers
                                                    , @content
                                                    , @sendendpointname
                                                    , @extensioninfo
                                                    , @statusdescription
                                                    , @status
                                                    , GETUTCDATE()
                                                    , GETUTCDATE())";

                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = record.ID
                        };
                        command.Parameters.Add(parameter);
                    }
                    parameter = new SqlParameter("@phonenumbers", SqlDbType.NVarChar, 4000)
                    {
                        Value = record.PhoneNumbers
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@content", SqlDbType.NVarChar, 4000)
                    {
                        Value = record.Content
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@sendendpointname", SqlDbType.NVarChar, 500)
                    {
                        Value = record.SendEndpointName
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@extensioninfo", SqlDbType.NVarChar, 4000)
                    {
                        Value = record.ExtensionInfo
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@statusdescription", SqlDbType.NVarChar, 4000)
                    {
                        Value = record.StatusDescription
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@status", SqlDbType.Int)
                    {
                        Value = record.Status
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

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="record"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task UpdateStatus(SMSRecord record, int status)
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

                    command.CommandText = @"Update [dbo].[SMSRecord]
                                                set [status] = @status
                                                   ,[modifytime] = GETUTCDATE()
                                            where [id] = @id ";

                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = record.ID
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@status", SqlDbType.Int)
                    {
                        Value = record.Status
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    await command.ExecuteNonQueryAsync();

                }

            });
        }
    }
}
