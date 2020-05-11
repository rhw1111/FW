using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using MSLibrary.DI;
using MSLibrary.Transaction;
using MSLibrary.DAL;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Security.WhitelistPolicy.DAL
{
    [Injection(InterfaceType = typeof(ISystemOperationWhitelistRelationStore), Scope = InjectionScope.Singleton)]
    public class SystemOperationWhitelistRelationStore : ISystemOperationWhitelistRelationStore
    {
        private IWhitelistPolicyConnectionFactory _dbConnectionFactory;

        public SystemOperationWhitelistRelationStore(IWhitelistPolicyConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task Add(Guid systemOperationId, Guid whitelistId)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForWhitelistPolicy(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                using (SqlCommand commond = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = @"insert into systemoperationwhiteListrelation([systemoperationid],[whitelistid],[createtime],[modifytime])
                                    values(@systemoperationid,@whitelistid,getutcdate(),getutcdate())"
                })
                {

                    var parameter = new SqlParameter("@systemoperationid", SqlDbType.UniqueIdentifier)
                    {
                        Value = systemOperationId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@whitelistid", SqlDbType.UniqueIdentifier)
                    {
                        Value = whitelistId
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();


                        try
                        {
                            await commond.ExecuteNonQueryAsync();
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
                                    Code = TextCodes.ExistSystemOperationWhitelistRelationByID,
                                    DefaultFormatting = "系统操作和白名单关联关系中存在相同的系统操作ID\"{0}\"和白名单ID\"{1}\"数据",
                                    ReplaceParameters = new List<object>() { systemOperationId, whitelistId }
                                };

                                throw new UtilityException((int)Errors.ExistSystemOperationWhitelistRelationByID, fragment);
                            }
                            else
                            {
                                throw;
                            }
                        }
            
                }

            });
        }

        public async Task Delete(Guid systemOperationId, Guid whitelistId)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForWhitelistPolicy(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                using (SqlCommand commond = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = @"delete from systemoperationwhiteListrelation
                                    where systemoperationid=@systemoperationid and whitelistid=@whitelistid"
                })
                {

                    var parameter = new SqlParameter("@systemoperationid", SqlDbType.UniqueIdentifier)
                    {
                        Value = systemOperationId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@whitelistid", SqlDbType.UniqueIdentifier)
                    {
                        Value = whitelistId
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();


                    await commond.ExecuteNonQueryAsync();
                }

            });
        }
    }
}
