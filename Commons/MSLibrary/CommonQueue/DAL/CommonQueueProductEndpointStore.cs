using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using MSLibrary.DI;
using MSLibrary.Transaction;

namespace MSLibrary.CommonQueue.DAL
{
    [Injection(InterfaceType = typeof(ICommonQueueProductEndpointStore), Scope = InjectionScope.Singleton)]
    public class CommonQueueProductEndpointStore : ICommonQueueProductEndpointStore
    {
        private ICommonQueueConnectionFactory _commonQueueConnectionFactory;

        public CommonQueueProductEndpointStore(ICommonQueueConnectionFactory commonQueueConnectionFactory)
        {
            _commonQueueConnectionFactory = commonQueueConnectionFactory;
        }

        public async Task Add(CommonQueueProductEndpoint endpoint)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _commonQueueConnectionFactory.CreateAllForCommonQueue(), async (conn, transaction) =>
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
                    Transaction = sqlTran
                })
                {

                    if (endpoint.ID == Guid.Empty)
                    {
                        commond.CommandText = @"insert into CommonProductEndpoint([id],[name],[queuetype],[queueconfiguration],[createtime],[modifytime])
                                    values(default,@name,@queuetype,@queueconfiguration,getutcdate(),getutcdate());
                                    select @newid=[id] from CommonProductEndpoint where [sequence]=SCOPE_IDENTITY()";
                    }
                    else
                    {
                        commond.CommandText = @"insert into CommonProductEndpoint([id],[name],[queuetype],[queueconfiguration],[createtime],[modifytime])
                                    values(@id,@name,@queuetype,@queueconfiguration,getutcdate(),getutcdate())";
                    }

                    SqlParameter parameter;
                    if (endpoint.ID != Guid.Empty)
                    {
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = endpoint.ID
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

                    parameter = new SqlParameter("@name", SqlDbType.VarChar, 150)
                    {
                        Value = endpoint.Name
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@queuetype", SqlDbType.VarChar, 150)
                    {
                        Value = endpoint.QueueType
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@queueconfiguration", SqlDbType.NVarChar, endpoint.QueueConfiguration.Length)
                    {
                        Value = endpoint.QueueConfiguration
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();

                    if (endpoint.ID == Guid.Empty)
                    {
                        endpoint.ID = (Guid)commond.Parameters["@newid"].Value;
                    }
                }
            });
        }

        public async Task Delete(Guid id)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _commonQueueConnectionFactory.CreateAllForCommonQueue(), async (conn, transaction) =>
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
                    CommandText = @"delete from [CommonQueueProductEndpoint] where id = @id"
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

        public async Task<CommonQueueProductEndpoint> QueryByID(Guid id)
        {
            CommonQueueProductEndpoint endpoint = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _commonQueueConnectionFactory.CreateReadForCommonQueue(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"select {0} from [CommonQueueProductEndpoint] where [id]=@id", StoreHelper.GetCommonQueueProductEndpointSelectFields(string.Empty))
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
                            endpoint = new CommonQueueProductEndpoint();
                            StoreHelper.SetCommonQueueProductEndpointSelectFields(endpoint, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return endpoint;
        }

        public async Task<QueryResult<CommonQueueProductEndpoint>> QueryByPage(string name, int page, int pageSize)
        {
            QueryResult<CommonQueueProductEndpoint> result = new QueryResult<CommonQueueProductEndpoint>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _commonQueueConnectionFactory.CreateReadForCommonQueue(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"
		                           select @count= count(*) from [CommonQueueProductEndpoint] where [name] like @name
	
                                    select {0} from [CommonQueueProductEndpoint] where [name] like @name
                                    order by [sequence]
		                            offset (@pagesize * (@page - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetCommonQueueProductEndpointSelectFields(string.Empty))
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

                    parameter = new SqlParameter("@name", SqlDbType.VarChar, 200)
                    {
                        Value = $"{name.ToSqlLike()}%"
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@count", SqlDbType.Int)
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
                            var endpoint = new CommonQueueProductEndpoint();
                            StoreHelper.SetCommonQueueProductEndpointSelectFields(endpoint, reader, string.Empty);
                            result.Results.Add(endpoint);
                        }

                        await reader.CloseAsync();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = page;
                    }
                }
            });

            return result;
        }

        public async Task Update(CommonQueueProductEndpoint endpoint)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _commonQueueConnectionFactory.CreateAllForCommonQueue(), async (conn, transaction) =>
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
                    CommandText = @"update [CommonQueueProductEndpoint] set [name]=@name,[queuetype]=@queuetype,[queueconfiguration]=@queueconfiguration,[modifytime]=getutcdate() where [id]=@id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = endpoint.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@name", SqlDbType.VarChar, 150)
                    {
                        Value = endpoint.Name
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@queuetype", SqlDbType.VarChar, 150)
                    {
                        Value = endpoint.QueueType
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@queueconfiguration", SqlDbType.NVarChar, endpoint.QueueConfiguration.Length)
                    {
                        Value = endpoint.QueueConfiguration
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();

                }
            });
        }
    }
}
