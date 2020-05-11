using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using MSLibrary.Transaction;
using MSLibrary.DI;
using MSLibrary.DAL;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Security.WhitelistPolicy.DAL
{
    /// <summary>
    /// 白名单数据操作
    /// </summary>
    [Injection(InterfaceType = typeof(IWhitelistStore), Scope = InjectionScope.Singleton)]
    public class WhitelistStore : IWhitelistStore
    {
        private IWhitelistPolicyConnectionFactory _dbConnectionFactory;

        public WhitelistStore(IWhitelistPolicyConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task Add(Whitelist data)
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
                })
                {
                    SqlParameter parameter;
                    if (data.ID == Guid.Empty)
                    {
                        commond.CommandText = @"insert into whitelist
                                                    ([id],[systemname],[systemsecret],[trustips],[enableipvalidation],[createtime],[modifytime],[status])
                                                values
                                                    (default,@systemname,@systemsecret,@trustips,@enableipvalidation,getutcdate(),getutcdate(),@status);
                                                select @newid =[id] from whitelist where [sequence] = SCOPE_IDENTITY()";
                        parameter = new SqlParameter("@newid", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };
                        commond.Parameters.Add(parameter);
                    }
                    else
                    {
                        commond.CommandText = @"insert into whitelist
                                                    ([id],[systemname],[systemsecret],[trustips],[enableipvalidation],[createtime],[modifytime],[status])
                                                values
                                                    (@id,@systemname,@systemsecret,@trustips,@enableipvalidation,getutcdate(),getutcdate(),@status)";
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = data.ID
                        };
                        commond.Parameters.Add(parameter);

                    }
                    parameter = new SqlParameter("@systemname", SqlDbType.NVarChar, 50)
                    {
                        Value = data.SystemName
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@systemsecret", SqlDbType.NVarChar, 50)
                    {
                        Value = data.SystemSecret
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@trustips", SqlDbType.NVarChar, 1000)
                    {
                        Value = data.TrustIPs
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@enableipvalidation", SqlDbType.Bit)
                    {
                        Value = data.EnableIPValidation
                    };
                    commond.Parameters.Add(parameter);
      
                        parameter = new SqlParameter("@status", SqlDbType.Int)
                        {
                            Value = data.Status
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
                                    Code = TextCodes.ExistWhitelistBySystemName,
                                    DefaultFormatting = "白名单中存在相同的系统名称\"{0}\"数据",
                                    ReplaceParameters = new List<object>() { data.SystemName }
                                };

                                throw new UtilityException((int)Errors.ExistWhitelistBySystemName, fragment);
                            }
                            else
                            {
                                throw;
                            }
                        }
                
                    //如果用户未赋值ID则创建成功后返回ID
                    if (data.ID == Guid.Empty)
                    {
                        data.ID = (Guid)commond.Parameters["@newid"].Value;
                    };
                }

            });
        }

        public async Task Delete(Guid id)
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
                    CommandText = @"delete from whitelist where [id]=@id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();


                        await commond.ExecuteNonQueryAsync();
                  
                }

            });
        }

        public async Task<QueryResult<Whitelist>> QueryByPage(int page, int pageSize)
        {
            QueryResult<Whitelist> result = new QueryResult<Whitelist>();

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
                    CommandText = string.Format(@"set @currentpage=@page
		                           select @count= count(*) from WhiteList
		                           if @pagesize*@page>=@count
			                          begin
				                           set @currentpage= @count/@pagesize
				                           if @count%@pagesize<>0
					                           begin
						                            set @currentpage=@currentpage+1
					                           end
				                           if @currentpage=0
					                           set @currentpage=1
			                          end
		                            else if @page<1 
			                           begin 
				                           set @currentpage=1
			                           end
	
                                    select {0} from WhiteList                 
                                    order by sequence
		                            offset (@pagesize * (@currentpage - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetWhitelistSelectFields(string.Empty)),
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
                            var whitelist = new Whitelist();
                            StoreHelper.SetWhitelistSelectFields(whitelist, reader, string.Empty);

                            result.Results.Add(whitelist);
                        }

                        reader.Close();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;
                    }
                }
            });

            return result;
        }

        public async Task<Whitelist> QueryById(Guid id)
        {
            Whitelist result = null;
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
                    CommandText = string.Format(@"select {0} from WhiteList where [id]=@id", StoreHelper.GetWhitelistSelectFields(string.Empty)),
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
                            result = new Whitelist();
                            StoreHelper.SetWhitelistSelectFields(result, reader, string.Empty);
                        }

                        reader.Close();
                    }
                }
            });

            return result;
        }

        public async Task Update(Whitelist data)
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
                    CommandText = @"update whitelist set [systemname]=@systemname,[systemsecret]=@systemsecret,[trustips]=@trustips,[enableipvalidation]=@enableipvalidation,[modifytime]=getutcdate(),[status]=@status
                                    where [id]=@id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = data.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@systemname", SqlDbType.NVarChar, 50)
                    {
                        Value = data.SystemName
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@systemsecret", SqlDbType.NVarChar, 50)
                    {
                        Value = data.SystemSecret
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@trustips", SqlDbType.NVarChar, 1000)
                    {
                        Value = data.TrustIPs
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@enableipvalidation", SqlDbType.Bit)
                    {
                        Value = data.EnableIPValidation
                    };
                    commond.Parameters.Add(parameter);
     
                        parameter = new SqlParameter("@status", SqlDbType.Int)
                        {
                            Value = data.Status
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
                                    Code = TextCodes.ExistWhitelistBySystemName,
                                    DefaultFormatting = "白名单中存在相同的系统名称\"{0}\"数据",
                                    ReplaceParameters = new List<object>() { data.SystemName }
                                };

                                throw new UtilityException((int)Errors.ExistWhitelistBySystemName, fragment);
                            }
                            else
                            {
                                throw;
                            }
                        }
                }

            });
        }

        public async Task<Whitelist> QueryBySystemOperationRelation(Guid systemOperationId, Guid whitelistId)
        {
            Whitelist result = null;
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
                    CommandText = string.Format(@"select {0} from WhiteList as w join SystemOperationWhiteListRelation as r on w.id=r.whitelistid where w.id=@whitelistid and r.systemoperationid=@systemoperationid", StoreHelper.GetWhitelistSelectFields("w")),
                    Transaction = sqlTran
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


                    SqlDataReader reader = null;


                    using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new Whitelist();
                            StoreHelper.SetWhitelistSelectFields(result, reader, "w");
                        }
                    }
                }
            });

            return result;
        }

        public async Task<Whitelist> QueryBySystemOperationRelation(Guid systemOperationId, string systemName)
        {
            Whitelist result = null;
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
                    CommandText = string.Format(@"select {0} from WhiteList as w join SystemOperationWhiteListRelation as r on w.id=r.whitelistid where r.systemoperationid=@systemoperationid and w.systemname=@whitelistsystemname", StoreHelper.GetWhitelistSelectFields("w")),
                    Transaction = sqlTran
                })
                {

                    var parameter = new SqlParameter("@systemoperationid", SqlDbType.UniqueIdentifier)
                    {
                        Value = systemOperationId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@whitelistsystemname", SqlDbType.NVarChar, 500)
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
                            result = new Whitelist();
                            StoreHelper.SetWhitelistSelectFields(result, reader, "w");
                        }
                    }
                }
            });

            return result;
        }

        public async Task<Whitelist> QueryBySystemOperationRelation(Guid systemOperationId, string systemName, int status)
        {
            Whitelist result = null;
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
                    CommandText = string.Format(@"select {0} from WhiteList as w join SystemOperationWhiteListRelation as r on w.id=r.whitelistid where r.systemoperationid=@systemoperationid and w.systemname=@whitelistsystemname and w.status=@status", StoreHelper.GetWhitelistSelectFields("w")),
                    Transaction = sqlTran
                })
                {

                    var parameter = new SqlParameter("@systemoperationid", SqlDbType.UniqueIdentifier)
                    {
                        Value = systemOperationId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@whitelistsystemname", SqlDbType.NVarChar, 500)
                    {
                        Value = systemName
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@status", SqlDbType.Int)
                    {
                        Value = status
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    SqlDataReader reader = null;


                    using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new Whitelist();
                            StoreHelper.SetWhitelistSelectFields(result, reader, "w");
                        }
                    }
                }
            });

            return result;
        }
    }
}
