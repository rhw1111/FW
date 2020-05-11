using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using MSLibrary.DI;
using MSLibrary.Transaction;

namespace MSLibrary.Cache.DAL
{
    [Injection(InterfaceType = typeof(ICacheRemoteConfigurationStore), Scope = InjectionScope.Singleton)]
    public class CacheRemoteConfigurationStore : ICacheRemoteConfigurationStore
    {
        private ICacheConnectionFactory _cacheConnectionFactory;

        public CacheRemoteConfigurationStore(ICacheConnectionFactory cacheConnectionFactory)
        {
            _cacheConnectionFactory = cacheConnectionFactory;
        }

        public async Task Add(CacheRemoteConfiguration configuration)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _cacheConnectionFactory.CreateAllForCache(), async (conn,transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction!=null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand commond = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran
                })
                {
                    if (configuration.ID == Guid.Empty)
                    {
                        commond.CommandText = @"insert into CacheRemoteConfiguration([id],[name],[remoteaddresses],[createtime],[modifytime])
                                    values(default,@name,@remoteaddresses,getutcdate(),getutcdate());
                                    select @newid=[id] from CacheRemoteConfiguration where [sequence]=SCOPE_IDENTITY()";
                    }
                    else
                    {
                        commond.CommandText = @"insert into CacheRemoteConfiguration([id],[name],[remoteaddresses],[createtime],[modifytime])
                                    values(@id,@name,@code,@remoteaddresses,getutcdate(),getutcdate())";
                    }

                    SqlParameter parameter;
                    if (configuration.ID != Guid.Empty)
                    {
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = configuration.ID
                        };
                        commond.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new SqlParameter("@newid", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };
                        commond.Parameters.Add(parameter);
                    }

                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 100)
                    {
                        Value = configuration.Name
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@remoteaddresses", SqlDbType.NChar,1000)
                    {
                        Value = configuration.RemoteAddresses
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();
                          
 
                    

                    if (configuration.ID == Guid.Empty)
                    {
                        configuration.ID = (Guid)commond.Parameters["@newid"].Value;
                    }
                }
            });
        }

        public async Task Delete(Guid id)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _cacheConnectionFactory.CreateReadForCache(), async (conn,transaction) =>
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
                    Transaction=sqlTran,
                    CommandText = @"delete from CacheRemoteConfiguration where [id]=@id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();                   
                }
            });
        }

        public async Task<CacheRemoteConfiguration> QueryById(Guid id)
        {
            CacheRemoteConfiguration configuration = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _cacheConnectionFactory.CreateReadForCache(), async (conn,transaction) =>
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
                    Transaction=sqlTran,
                    CommandText = string.Format(@"select {0} from CacheRemoteConfiguration where [id]=@id", StoreHelper.GetCacheRemoteConfigurationSelectFields(string.Empty))
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    SqlDataReader reader = null;

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            configuration = new CacheRemoteConfiguration();
                            StoreHelper.SetCacheRemoteConfigurationSelectFields(configuration, reader, string.Empty);
                        }

                        reader.Close();
                    }
                }
            });

            return configuration;
        }

        public async Task<CacheRemoteConfiguration> QueryByName(string name)
        {
            CacheRemoteConfiguration configuration = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _cacheConnectionFactory.CreateReadForCache(), async (conn,transaction) =>
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
                    Transaction=sqlTran,
                    CommandText = string.Format(@"select {0} from CacheRemoteConfiguration where [name]=@name", StoreHelper.GetCacheRemoteConfigurationSelectFields(string.Empty))
                })
                {

                    var parameter = new SqlParameter("@name", SqlDbType.NVarChar,150)
                    {
                        Value = name
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();


                    SqlDataReader reader = null;   

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            configuration = new CacheRemoteConfiguration();
                            StoreHelper.SetCacheRemoteConfigurationSelectFields(configuration, reader, string.Empty);
                        }

                        reader.Close();
                    }
                }
            });

            return configuration;
        }

        public async Task<QueryResult<CacheRemoteConfiguration>> QueryByName(string name, int page, int pageSize)
        {
            QueryResult<CacheRemoteConfiguration> result = new QueryResult<CacheRemoteConfiguration>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _cacheConnectionFactory.CreateReadForCache(), async (conn,transaction) =>
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
                     Transaction=sqlTran,
                    CommandText = string.Format(@"set @currentpage=@page
		                           select @count= count(*) from CacheRemoteConfiguration where [name] like @name
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
	
                                    select {0} from CacheRemoteConfiguration where [name] like @name
                                    order by [sequence]
		                            offset (@pagesize * (@currentpage - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetCacheRemoteConfigurationSelectFields(string.Empty))
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

                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 150)
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

                    await commond.PrepareAsync();

                    SqlDataReader reader = null;

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var configuration = new CacheRemoteConfiguration();
                            StoreHelper.SetCacheRemoteConfigurationSelectFields(configuration, reader, string.Empty);
                            result.Results.Add(configuration);
                        }

                        await reader.CloseAsync();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;
                    }
                }
            });

            return result;
        }

        public async Task Update(CacheRemoteConfiguration configuration)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _cacheConnectionFactory.CreateAllForCache(), async (conn,transaction) =>
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
                    Transaction=sqlTran,
                    CommandText = @"update SQueue set [name]=@name,[remoteaddresses]=@remoteaddresses,[modifytime]=getutcdate()
                                    where [id]=@id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = configuration.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 150)
                    {
                        Value = configuration.Name
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@remoteaddresses", SqlDbType.NVarChar,1000)
                    {
                        Value = configuration.RemoteAddresses
                    };
                    commond.Parameters.Add(parameter);


                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();
                }
            });
        }
    }
}
