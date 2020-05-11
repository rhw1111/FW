using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using MSLibrary.DI;
using MSLibrary.Transaction;

namespace MSLibrary.Entity.DAL
{
    [Injection(InterfaceType = typeof(IEntityAttributeMapConfigurationDetailStore), Scope = InjectionScope.Singleton)]
    public class EntityAttributeMapConfigurationDetailStore : IEntityAttributeMapConfigurationDetailStore
    {
        private IEntityAttributeMapConfigurationConnectionFactory _dbConnectionFactory;

        public EntityAttributeMapConfigurationDetailStore(IEntityAttributeMapConfigurationConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task QueryAllByConfigurationId(Guid configurationId, Func<EntityAttributeMapConfigurationDetail, Task> callback)
        {
            int page = 0;
            int pageSize = 500;
            QueryResult<EntityAttributeMapConfigurationDetail> result = new QueryResult<EntityAttributeMapConfigurationDetail>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForEntityAttributeMapConfiguration(), async (conn,transaction) =>
            {

                while (true)
                {
                    page++;
                    result.Results.Clear();

                    SqlTransaction sqlTran = null;
                    if (transaction != null)
                    {
                        sqlTran = (SqlTransaction)transaction;
                    }


                    await using (SqlCommand commond = new SqlCommand()
                    {
                        Connection = (SqlConnection)conn,
                        CommandType = CommandType.Text,
                         Transaction=sqlTran,
                        CommandText = @"set @currentpage=@page
		                                select @count= count(*) from [dbo].[EntityAttributeMapConfigurationDetail] where configurationid=@configurationid
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
		
		
		                                declare @columns nvarchar(500),@prefix nvarchar(20)
		                                set @prefix=''
		                                set @columns=dbo.core_GetEntityAttributeMapConfigurationDetailQueryColumns(@prefix)

		                                select @prefix as prefix

		                                declare @execsql nvarchar(1000)
		                                set @execsql='select '+@columns+' from [dbo].[EntityAttributeMapConfigurationDetail]WITH (SNAPSHOT) where configurationid=@configurationid '
		                                +'order by [createtime] desc '
		                                +'offset  ( @pagesize * ( @currentpage - 1 )) rows '
		                                +'fetch next @pagesize rows only;'
		
		                                exec sp_executesql @execsql, N'@configurationid uniqueidentifier,@pagesize int,@currentpage int',@configurationid, @pagesize,@currentpage 	
                                "
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

                        parameter = new SqlParameter("@configurationid", SqlDbType.UniqueIdentifier)
                        {
                            Value = configurationId
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

                        await using (reader = await commond.ExecuteReaderAsync())
                        {
                            string prefix = string.Empty;
                            if (await reader.ReadAsync())
                            {
                                prefix = reader["prefix"].ToString();
                            }

                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    var detail = new EntityAttributeMapConfigurationDetail();
                                    StoreHelper.SetEntityAttributeMapConfigurationDetailSelectFields(detail, reader, prefix);

                                    result.Results.Add(detail);
                                }
                            }

                            await reader.CloseAsync();

                            result.TotalCount = (int)commond.Parameters["@count"].Value;
                            result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;
                        }
                    }

                    foreach (var item in result.Results)
                    {
                        await callback(item);
                    }

                    if (pageSize * result.CurrentPage >= result.TotalCount)
                    {
                        break;
                    }
                }
            });

        }

        public void QueryAllByConfigurationIdSync(Guid configurationId, Action<EntityAttributeMapConfigurationDetail> callback)
        {
            int page = 0;
            int pageSize = 500;
            QueryResult<EntityAttributeMapConfigurationDetail> result = new QueryResult<EntityAttributeMapConfigurationDetail>();

            DBTransactionHelper.SqlTransactionWork(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForEntityAttributeMapConfiguration(), (conn,transaction) =>
            {

                while (true)
                {
                    page++;
                    result.Results.Clear();

                    SqlTransaction sqlTran = null;
                    if (transaction != null)
                    {
                        sqlTran = (SqlTransaction)transaction;
                    }

                    using (SqlCommand commond = new SqlCommand()
                    {
                        Connection = (SqlConnection)conn,
                        CommandType = CommandType.Text,
                        CommandText = @"set @currentpage=@page
		                            select @count= count(*) from [dbo].[EntityAttributeMapConfigurationDetail] where configurationid=@configurationid
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
		
		
		                            declare @columns nvarchar(500),@prefix nvarchar(20)
		                            set @prefix=''
		                            set @columns=dbo.core_GetEntityAttributeMapConfigurationDetailQueryColumns(@prefix)

		                            select @prefix as prefix

		                            declare @execsql nvarchar(1000)
		                            set @execsql='select '+@columns+' from [dbo].[EntityAttributeMapConfigurationDetail]WITH (SNAPSHOT) where configurationid=@configurationid '
		                            +'order by [createtime] desc '
		                            +'offset  ( @pagesize * ( @currentpage - 1 )) rows '
		                            +'fetch next @pagesize rows only;'
		
		                            exec sp_executesql @execsql, N'@configurationid uniqueidentifier,@pagesize int,@currentpage int',@configurationid, @pagesize,@currentpage 	
                            ",
                         Transaction=sqlTran
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

                        parameter = new SqlParameter("@configurationid", SqlDbType.UniqueIdentifier)
                        {
                            Value = configurationId
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

                        using (reader = commond.ExecuteReader())
                        {
                            string prefix = string.Empty;
                            if (reader.Read())
                            {
                                prefix = reader["prefix"].ToString();
                            }

                            if (reader.NextResult())
                            {
                                while ( reader.Read())
                                {
                                    var detail = new EntityAttributeMapConfigurationDetail();
                                    StoreHelper.SetEntityAttributeMapConfigurationDetailSelectFields(detail, reader, prefix);

                                    result.Results.Add(detail);
                                }
                            }

                            reader.Close();

                            result.TotalCount = (int)commond.Parameters["@count"].Value;
                            result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;
                        }
                    }

                    foreach (var item in result.Results)
                    {
                        callback(item);
                    }

                    if (pageSize * result.CurrentPage >= result.TotalCount)
                    {
                        break;
                    }
                }
            });

        }

        public async Task<QueryResult<EntityAttributeMapConfigurationDetail>> QueryByPage(Guid configurationId, int page, int pageSize)
        {
            QueryResult<EntityAttributeMapConfigurationDetail> result = new QueryResult<EntityAttributeMapConfigurationDetail>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForEntityAttributeMapConfiguration(), async (conn, transaction) =>
            {
                    SqlTransaction sqlTran = null;
                    if (transaction != null)
                    {
                        sqlTran = (SqlTransaction)transaction;
                    }


                    await using (SqlCommand commond = new SqlCommand()
                    {
                        Connection = (SqlConnection)conn,
                        CommandType = CommandType.Text,
                        Transaction = sqlTran,
                        CommandText = @"set @currentpage=@page
		                                select @count= count(*) from [dbo].[EntityAttributeMapConfigurationDetail] where configurationid=@configurationid
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
		
		
		                                declare @columns nvarchar(500),@prefix nvarchar(20)
		                                set @prefix=''
		                                set @columns=dbo.core_GetEntityAttributeMapConfigurationDetailQueryColumns(@prefix)

		                                select @prefix as prefix

		                                declare @execsql nvarchar(1000)
		                                set @execsql='select '+@columns+' from [dbo].[EntityAttributeMapConfigurationDetail]WITH (SNAPSHOT) where configurationid=@configurationid '
		                                +'order by [createtime] desc '
		                                +'offset  ( @pagesize * ( @currentpage - 1 )) rows '
		                                +'fetch next @pagesize rows only;'
		
		                                exec sp_executesql @execsql, N'@configurationid uniqueidentifier,@pagesize int,@currentpage int',@configurationid, @pagesize,@currentpage 	
                                "
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

                        parameter = new SqlParameter("@configurationid", SqlDbType.UniqueIdentifier)
                        {
                            Value = configurationId
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


                        await using (reader= await commond.ExecuteReaderAsync())
                        {
                            string prefix = string.Empty;
                            if (await reader.ReadAsync())
                            {
                                prefix = reader["prefix"].ToString();
                            }

                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    var detail = new EntityAttributeMapConfigurationDetail();
                                    StoreHelper.SetEntityAttributeMapConfigurationDetailSelectFields(detail, reader, prefix);

                                    result.Results.Add(detail);
                                }
                            }

                            await reader.CloseAsync();

                            result.TotalCount = (int)commond.Parameters["@count"].Value;
                            result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;
                        }
                    }              
            });

            return result;
        }

        public QueryResult<EntityAttributeMapConfigurationDetail> QueryByPageSync(Guid configurationId, int page, int pageSize)
        {
            QueryResult<EntityAttributeMapConfigurationDetail> result = new QueryResult<EntityAttributeMapConfigurationDetail>();

            DBTransactionHelper.SqlTransactionWork(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForEntityAttributeMapConfiguration(), (conn, transaction) =>
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
                        CommandText = @"set @currentpage=@page
		                                select @count= count(*) from [dbo].[EntityAttributeMapConfigurationDetail] where configurationid=@configurationid
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

		                                declare @columns nvarchar(500),@prefix nvarchar(20)
		                                set @prefix=''
		                                set @columns=dbo.core_GetEntityAttributeMapConfigurationDetailQueryColumns(@prefix)

		                                select @prefix as prefix

		                                declare @execsql nvarchar(1000)
		                                set @execsql='select '+@columns+' from [dbo].[EntityAttributeMapConfigurationDetail]WITH (SNAPSHOT) where configurationid=@configurationid '
		                                +'order by [createtime] desc '
		                                +'offset  ( @pagesize * ( @currentpage - 1 )) rows '
		                                +'fetch next @pagesize rows only;'
		
		                                exec sp_executesql @execsql, N'@configurationid uniqueidentifier,@pagesize int,@currentpage int',@configurationid, @pagesize,@currentpage 	",
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

                        parameter = new SqlParameter("@configurationid", SqlDbType.UniqueIdentifier)
                        {
                            Value = configurationId
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

                        using (reader= commond.ExecuteReader())
                        {
                            string prefix = string.Empty;
                            if (reader.Read())
                            {
                                prefix = reader["prefix"].ToString();
                            }

                            if (reader.NextResult())
                            {
                                while (reader.Read())
                                {
                                    var detail = new EntityAttributeMapConfigurationDetail();
                                    StoreHelper.SetEntityAttributeMapConfigurationDetailSelectFields(detail, reader, prefix);

                                    result.Results.Add(detail);
                                }
                            }

                            reader.Close();

                            result.TotalCount = (int)commond.Parameters["@count"].Value;
                            result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;
                        }
                    }

         
            });

            return result;
        }
    }
}
