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
    [Injection(InterfaceType = typeof(ICommonMessageClientTypeStore), Scope = InjectionScope.Singleton)]
    public class CommonMessageClientTypeStore : ICommonMessageClientTypeStore
    {

        private ICommonQueueConnectionFactory _commonQueueConnectionFactory;

        public CommonMessageClientTypeStore(ICommonQueueConnectionFactory commonQueueConnectionFactory)
        {
            _commonQueueConnectionFactory = commonQueueConnectionFactory;
        }

        public async Task Add(CommonMessageClientType type)
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

                    if (type.ID == Guid.Empty)
                    {
                        commond.CommandText = @"insert into CommonMessageClientType([id],[name],[endpointid],[createtime],[modifytime])
                                    values(default,@name,@endpointid,getutcdate(),getutcdate());
                                    select @newid=[id] from CommonMessageClientType where [sequence]=SCOPE_IDENTITY()";
                    }
                    else
                    {
                        commond.CommandText = @"insert into CommonMessageClientType([id],[name],[endpointid],[createtime],[modifytime])
                                    values(@id,@name,@endpointid,getutcdate(),getutcdate())";
                    }

                    SqlParameter parameter;
                    if (type.ID != Guid.Empty)
                    {
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = type.ID
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
                        Value = type.Name
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@endpointid", SqlDbType.UniqueIdentifier)
                    {
                        Value = type.EndpointID
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();

                    if (type.ID == Guid.Empty)
                    {
                        type.ID = (Guid)commond.Parameters["@newid"].Value;
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
                    CommandText = @"delete from [CommonMessageClientType] where id = @id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                   await  commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();



                }
            });
        }

        public async Task<CommonMessageClientType> QueryByID(Guid id)
        {
            CommonMessageClientType type = null;

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
                    CommandText = string.Format(@"select {0},{1} from [CommonMessageClientType] as t join [CommonQueueProductEndpoint] as e on t.endpointid=e.id where t.[id]=@id", StoreHelper.GetCommonQueueConsumeEndpointSelectFields("t"), StoreHelper.GetCommonQueueProductEndpointSelectFields("e"))
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
                            type = new CommonMessageClientType();                          
                            StoreHelper.SetCommonMessageClientTypeSelectFields(type, reader, "t");
                            type.Endpoint = new CommonQueueProductEndpoint();
                            StoreHelper.SetCommonQueueProductEndpointSelectFields(type.Endpoint, reader, "e");
                        }

                       await reader.CloseAsync();
                    }
                }
            });

            return type;
        }

        public async Task<CommonMessageClientType> QueryByName(string name)
        {
            CommonMessageClientType type = null;

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
                    CommandText = string.Format(@"select {0},{1} from [CommonMessageClientType] as t join [CommonQueueProductEndpoint] as e on t.endpointid=e.id where t.[name]=@name", StoreHelper.GetCommonQueueConsumeEndpointSelectFields("t"), StoreHelper.GetCommonQueueProductEndpointSelectFields("e"))
                })
                {

                    var parameter = new SqlParameter("@name", SqlDbType.VarChar,150)
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
                            type = new CommonMessageClientType();
                            StoreHelper.SetCommonMessageClientTypeSelectFields(type, reader, "t");
                            type.Endpoint = new CommonQueueProductEndpoint();
                            StoreHelper.SetCommonQueueProductEndpointSelectFields(type.Endpoint, reader, "e");
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return type;
        }

        public async Task<QueryResult<CommonMessageClientType>> QueryByPage(string name, int page, int pageSize)
        {
            QueryResult<CommonMessageClientType> result = new QueryResult<CommonMessageClientType>();

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
		                           select @count= count(*) from [CommonMessageClientType] as t join [CommonQueueProductEndpoint] as e on t.endpointid=e.id where t.[name] like @name
	
                                    select {0},{1} from [CommonMessageClientType] as t join [CommonQueueProductEndpoint] as e on t.endpointid=e.id where t.[name] like @name
                                    order by t.[sequence]
		                            offset (@pagesize * (@page - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetCommonMessageClientTypeFields("t"), StoreHelper.GetCommonQueueProductEndpointSelectFields("e"))
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
                            var type = new CommonMessageClientType();
                            StoreHelper.SetCommonMessageClientTypeSelectFields(type, reader, "t");
                            type.Endpoint = new CommonQueueProductEndpoint();
                            StoreHelper.SetCommonQueueProductEndpointSelectFields(type.Endpoint, reader, "e");
                            result.Results.Add(type);
                        }

                        await reader.CloseAsync();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = page;
                    }
                }
            });

            return result;
        }

        public async Task Update(CommonMessageClientType type)
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
                    CommandText = @"update [CommonMessageClientType] set [name]=@name,[endpointid]=@endpointid,[modifytime]=getutcdate() where [id]=@id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = type.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@name", SqlDbType.VarChar, 150)
                    {
                        Value = type.Name
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@endpointid", SqlDbType.UniqueIdentifier)
                    {
                        Value = type.EndpointID
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();

                }
            });
        }
    }
}
