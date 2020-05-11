using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using MSLibrary.Transaction;
using MSLibrary.DI;
using MSLibrary.DAL;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Security.WhitelistPolicy.DAL
{
    /// <summary>
    /// 系统操作的数据操作
    /// </summary>
    [Injection(InterfaceType = typeof(ISystemOperationStore), Scope = InjectionScope.Singleton)]
    public class SystemOperationStore : ISystemOperationStore
    {
        private IWhitelistPolicyConnectionFactory _dbConnectionFactory;

        public SystemOperationStore(IWhitelistPolicyConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task Add(SystemOperation operation)
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
                    Transaction = sqlTran
                })
                {
                    SqlParameter parameter;
                    if (operation.ID == Guid.Empty)
                    {
                        commond.CommandText = @"insert into systemoperation
                                                ([id],[name],[createtime],[modifytime],[status])
                                            values
                                                (default,@name,getutcdate(),getutcdate(),@status);
                                            select @newid =[id] from systemoperation where [sequence] = SCOPE_IDENTITY()";
                        parameter = new SqlParameter("@newid", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };
                        commond.Parameters.Add(parameter);
                    }
                    else
                    {
                        commond.CommandText = @"insert into systemoperation
                                                ([id],[name],[createtime],[modifytime],[status])
                                            values
                                                (@id,@name,getutcdate(),getutcdate(),@status);";
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = operation.ID
                        };
                        commond.Parameters.Add(parameter);
                    }

                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 500)
                    {
                        Value = operation.Name
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@status", SqlDbType.Int)
                    {
                        Value = operation.Status
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
                                    Code = TextCodes.ExistSystemOperationByName,
                                    DefaultFormatting = "系统操作中存在相同的名称\"{0}\"数据",
                                    ReplaceParameters = new List<object>() { operation.Name }
                                };

                                throw new UtilityException((int)Errors.ExistSystemOperationByName, fragment);
                            }
                            else
                            {
                                throw;
                            }
                        }
         
                    //如果用户未赋值ID则创建成功后返回ID
                    if (operation.ID == Guid.Empty)
                    {
                        operation.ID = (Guid)commond.Parameters["@newid"].Value;
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
                    CommandText = @"delete from systemoperation where [id]=@id"
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

        public async Task<SystemOperation> QueryById(Guid id)
        {
            SystemOperation result = null;
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
                    CommandText = string.Format(@"select {0} from SystemOperation where [id]=@id", StoreHelper.GetSystemOperationSelectFields(string.Empty)),
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
                            result = new SystemOperation();
                            StoreHelper.SetSystemOperationSelectFields(result, reader, string.Empty);
                        }

                        reader.Close();
                    }
                }
            });

            return result;
        }

        public async Task<SystemOperation> QueryByName(string name)
        {
            SystemOperation result = null;
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
                    CommandText = string.Format(@"select {0} from SystemOperation where [name]=@name", StoreHelper.GetSystemOperationSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {

                    var parameter = new SqlParameter("@name", SqlDbType.NVarChar, 500)
                    {
                        Value = name
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();


                    SqlDataReader reader = null;

                    using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new SystemOperation();
                            StoreHelper.SetSystemOperationSelectFields(result, reader, string.Empty);
                        }

                        reader.Close();
                    }
                }
            });

            return result;
        }

        public async Task<SystemOperation> QueryByName(string name, int status)
        {
            SystemOperation result = null;
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
                    CommandText = string.Format(@"select {0} from SystemOperation where [name]=@name and [status]=@status", StoreHelper.GetSystemOperationSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {

                    var parameter = new SqlParameter("@name", SqlDbType.NVarChar, 500)
                    {
                        Value = name
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
                            result = new SystemOperation();
                            StoreHelper.SetSystemOperationSelectFields(result, reader, string.Empty);
                        }

                        reader.Close();
                    }
                }
            });

            return result;
        }

        public async Task<QueryResult<SystemOperation>> QueryByPage(int page, int pageSize)
        {
            QueryResult<SystemOperation> result = new QueryResult<SystemOperation>();

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
                                                FROM SystemOperation;
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
                                                FROM SystemOperation
                                                ORDER BY sequence OFFSET (@pagesize * (@currentpage - 1)) ROWS FETCH NEXT @pagesize ROWS ONLY;",
                                                StoreHelper.GetSystemOperationSelectFields(string.Empty)),
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
                            var systemOperation = new SystemOperation();
                            StoreHelper.SetSystemOperationSelectFields(systemOperation, reader, string.Empty);

                            result.Results.Add(systemOperation);
                        }



                        reader.Close();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;
                    }
                }
            });

            return result;
        }

        public async Task<QueryResult<SystemOperation>> QueryByPage(string name, int page, int pageSize)
        {
            QueryResult<SystemOperation> result = new QueryResult<SystemOperation>();

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
		                           select @count= count(*) from SystemOperation where [name] like @name
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
	
                                    select {0} from SystemOperation
                                    where [name] like @name
                                    order by sequence
		                            offset (@pagesize * (@currentpage - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetSystemOperationSelectFields(string.Empty)),
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

                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 100)
                    {
                        Value = $"{name.ToSqlLike()}%"
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
                            var systemOperation = new SystemOperation();
                            StoreHelper.SetSystemOperationSelectFields(systemOperation, reader, string.Empty);

                            result.Results.Add(systemOperation);
                        }



                        reader.Close();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;
                    }
                }
            });

            return result;
        }

        public async Task Update(SystemOperation operation)
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
                    CommandText = @"update systemoperation set [name]=@name,[status]=@status,[modifytime]=getutcdate()
                                    where [id]=@id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = operation.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 500)
                    {
                        Value = operation.Name
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@status", SqlDbType.Int)
                    {
                        Value = operation.Status
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
                                    Code = TextCodes.ExistSystemOperationByName,
                                    DefaultFormatting = "系统操作中存在相同的名称\"{0}\"数据",
                                    ReplaceParameters = new List<object>() { operation.Name }
                                };

                                throw new UtilityException((int)Errors.ExistSystemOperationByName, fragment);
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
