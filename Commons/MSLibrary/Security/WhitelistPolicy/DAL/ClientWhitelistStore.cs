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

namespace MSLibrary.Security.WhitelistPolicy.DAL
{
    /// <summary>
    /// 客户端白名单数据操作
    /// </summary>

    [Injection(InterfaceType = typeof(IClientWhitelistStore), Scope = InjectionScope.Singleton)]
    public class ClientWhitelistStore : IClientWhitelistStore
    {

        private IWhitelistPolicyConnectionFactory _dbConnectionFactory;

        public ClientWhitelistStore(IWhitelistPolicyConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task Add(ClientWhitelist data)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForWhitelistPolicy(), async (conn, transaction) =>
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
                    Transaction = sqlTran,
                })
                {
                    SqlParameter parameter;
                    if (data.ID == Guid.Empty)
                    {
                        command.CommandText = @"INSERT INTO [dbo].[ClientWhiteList] 
                                                    ([id] ,[systemname] ,[systemsecret] ,[signatureexpire] ,[createtime] ,[modifytime]) 
                                                VALUES 
                                                    (default ,@systemname ,@systemsecret ,@signatureexpire , GETUTCDATE() , GETUTCDATE());
                                                select @newid =[id] from [dbo].[ClientWhiteList] where [sequence] = SCOPE_IDENTITY()";
                        parameter = new SqlParameter("@newid", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        command.CommandText = @"INSERT INTO [dbo].[ClientWhiteList] 
                                                    ([id] ,[systemname] ,[systemsecret] ,[signatureexpire] ,[createtime] ,[modifytime]) 
                                                VALUES 
                                                    (@id ,@systemname ,@systemsecret ,@signatureexpire , GETUTCDATE() , GETUTCDATE())";
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = data.ID
                        };
                        command.Parameters.Add(parameter);
                    }
                    parameter = new SqlParameter("@systemname", SqlDbType.NVarChar, 500)
                    {
                        Value = data.SystemName
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@systemsecret", SqlDbType.NVarChar, 500)
                    {
                        Value = data.SystemSecret
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@signatureexpire", SqlDbType.Int)
                    {
                        Value = data.SignatureExpire
                    };
                    command.Parameters.Add(parameter);

                    command.Prepare();

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
                                    Code = TextCodes.ExistClientWhitelistBySystemName,
                                    DefaultFormatting = "客户端白名单中存在相同的系统名称\"{0}\"数据",
                                    ReplaceParameters = new List<object>() { data.SystemName }
                                };

                                throw new UtilityException((int)Errors.ExistClientWhitelistBySystemName, fragment);
                            }
                            else
                            {
                                throw;
                            }
                        }
           
                    //如果用户未赋值ID则创建成功后返回ID
                    if (data.ID == Guid.Empty)
                    {
                        data.ID = (Guid)command.Parameters["@newid"].Value;
                    };
                }

            });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(Guid id)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForWhitelistPolicy(), async (conn, transaction) =>
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
                    Transaction = sqlTran,
                    CommandText = @"DELETE FROM [dbo].[ClientWhiteList]
                                     WHERE id=@id"
                })
                {
                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);
                    command.Prepare();


                        await command.ExecuteNonQueryAsync();
               
                }
            });

        }

        /// <summary>
        /// 根据id查询客户端白名单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ClientWhitelist> QueryById(Guid id)
        {
            ClientWhitelist result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForWhitelistPolicy(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"select {0} from [ClientWhiteList] where [id]=@id", StoreHelper.GetClientWhitelistStoreSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {
                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);
                    commond.Prepare();
                    SqlDataReader reader = null;

                    using (reader = await commond.ExecuteReaderAsync())
                    {

                        if (await reader.ReadAsync())
                        {
                            result = new ClientWhitelist();
                            StoreHelper.SetClientWhitelistStoreSelectFields(result, reader, string.Empty);
                        }

                        reader.Close();
                    }
                }
            });
            return result;
        }

        /// <summary>
        /// 分页查询客户端白名单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<QueryResult<ClientWhitelist>> QueryByPage(int page, int pageSize)
        {
            QueryResult<ClientWhitelist> result = new QueryResult<ClientWhitelist>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForWhitelistPolicy(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"SET @currentpage = @page;
                                                SELECT @count = COUNT(*)
                                                FROM [ClientWhiteList];
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

                                                SELECT {0}
                                                FROM [ClientWhiteList]
                                                ORDER BY sequence OFFSET (@pagesize * (@currentpage - 1)) ROWS FETCH NEXT @pagesize ROWS ONLY;",
                                                StoreHelper.GetClientWhitelistStoreSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {

                    var parameter = new SqlParameter("@page", SqlDbType.Int)
                    {
                        Value = page
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@pagesize", SqlDbType.Int)
                    {
                        Value = pageSize
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@count", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@currentpage", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    SqlDataReader reader = null;

                    using (reader = await commond.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var clientWhitelist = new ClientWhitelist();
                            StoreHelper.SetClientWhitelistStoreSelectFields(clientWhitelist, reader, string.Empty);

                            result.Results.Add(clientWhitelist);
                        }
                        reader.Close();
                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;
                    }
                }
            });
            return result;
        }

        /// <summary>
        /// 分页查询匹配系统名称客户端白名单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="systemName"></param>
        /// <returns></returns>
        public async Task<QueryResult<ClientWhitelist>> QueryByPage(int page, int pageSize, string systemName)
        {
            QueryResult<ClientWhitelist> result = new QueryResult<ClientWhitelist>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForWhitelistPolicy(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"SET @currentpage = @page;
                                                SELECT @count = COUNT(*)
                                                FROM [ClientWhiteList]
                                                WHERE [systemname] LIKE @systemname;
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

                                                SELECT {0}
                                                FROM [ClientWhiteList]
                                                WHERE [systemname] LIKE @systemname
                                                ORDER BY sequence OFFSET (@pagesize * (@currentpage - 1)) ROWS FETCH NEXT @pagesize ROWS ONLY;",
                                                StoreHelper.GetClientWhitelistStoreSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {

                    var parameter = new SqlParameter("@page", SqlDbType.Int)
                    {
                        Value = page
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@pagesize", SqlDbType.Int)
                    {
                        Value = pageSize
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@count", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@currentpage", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@systemname", SqlDbType.NVarChar, 100)
                    {
                        Value = $"{systemName.ToSqlLike()}%"
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    SqlDataReader reader = null;


                    using (reader = await commond.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var clientWhitelist = new ClientWhitelist();
                            StoreHelper.SetClientWhitelistStoreSelectFields(clientWhitelist, reader, string.Empty);

                            result.Results.Add(clientWhitelist);
                        }
                        reader.Close();
                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;
                    }
                }
            });
            return result;
        }

        /// <summary>
        /// 根据系统名称查询客户端白名单
        /// </summary>
        /// <param name="systemName"></param>
        /// <returns></returns>
        public async Task<ClientWhitelist> QueryBySystemName(string systemName)
        {
            ClientWhitelist result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForWhitelistPolicy(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"select {0} from [ClientWhiteList] where [systemname]=@systemname", StoreHelper.GetClientWhitelistStoreSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {
                    var parameter = new SqlParameter("@systemname", SqlDbType.NVarChar, 500)
                    {
                        Value = systemName
                    };
                    commond.Parameters.Add(parameter);
                    commond.Prepare();
                    SqlDataReader reader = null;

                    using (reader = await commond.ExecuteReaderAsync())
                    {

                        if (await reader.ReadAsync())
                        {
                            result = new ClientWhitelist();
                            StoreHelper.SetClientWhitelistStoreSelectFields(result, reader, string.Empty);
                        }

                        reader.Close();
                    }
                }
            });
            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task Update(ClientWhitelist data)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForWhitelistPolicy(), async (conn, transaction) =>
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
                    Transaction = sqlTran,
                    CommandText = @"UPDATE [dbo].[ClientWhiteList]
                                       SET [systemname] = @systemname ,[systemsecret] = @systemsecret ,[signatureexpire] = @signatureexpire ,[modifytime]=GETUTCDATE()
                                        WHERE id=@id"
                })
                {
                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = data.ID
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@systemname", SqlDbType.NVarChar, 500)
                    {
                        Value = data.SystemName
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@systemsecret", SqlDbType.NVarChar, 500)
                    {
                        Value = data.SystemSecret
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@signatureexpire", SqlDbType.Int)
                    {
                        Value = data.SignatureExpire
                    };
                    command.Parameters.Add(parameter);
                    command.Prepare();


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
                                    Code = TextCodes.ExistClientWhitelistBySystemName,
                                    DefaultFormatting = "客户端白名单中存在相同的系统名称\"{0}\"数据",
                                    ReplaceParameters = new List<object>() { data.SystemName }
                                };

                                throw new UtilityException((int)Errors.ExistClientWhitelistBySystemName, fragment);
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
