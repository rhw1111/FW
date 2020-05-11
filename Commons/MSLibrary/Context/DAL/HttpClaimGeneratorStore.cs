using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using MSLibrary;
using MSLibrary.Context;
using MSLibrary.Context.DAL;
using MSLibrary.DI;
using MSLibrary.Transaction;

namespace MSLibrary.Context.DAL
{
    [Injection(InterfaceType = typeof(IHttpClaimGeneratorStore), Scope = InjectionScope.Singleton)]
    public class HttpClaimGeneratorStore : IHttpClaimGeneratorStore
    {
        private IContextConnectionFactory _contextConnectionFactory;

        public HttpClaimGeneratorStore(IContextConnectionFactory contextConnectionFactory)
        {
            _contextConnectionFactory = contextConnectionFactory;
        }

        public async Task Add(HttpClaimGenerator generator)
        {
            //获取读写连接字符串
            var strConn = _contextConnectionFactory.CreateAllForContext();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, strConn, async (conn, transaction) =>
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

                    if (generator.ID == Guid.Empty)
                    {
                        commond.CommandText = @"insert into httpclaimgenerator([id],[name],[type],[createtime],[modifytime])
                                    values(default,@name,@tyep,getutcdate(),getutcdate());
                                    select @newid=[id] from httpclaimgenerator where [sequence]=SCOPE_IDENTITY()";
                    }
                    else
                    {
                        commond.CommandText = @"insert into httpclaimgenerator([id],[name],[type],[createtime],[modifytime])
                                    values(@id,@name,@type,getutcdate(),getutcdate())";
                    }

                    SqlParameter parameter;
                    if (generator.ID != Guid.Empty)
                    {
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = generator.ID
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
                        Value = generator.Name
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@type", SqlDbType.VarChar, 150)
                    {
                        Value = generator.Type
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();

                    if (generator.ID == Guid.Empty)
                    {
                        generator.ID = (Guid)commond.Parameters["@newid"].Value;
                    }
                }
            });
        }

        public async Task Delete(Guid id)
        {
            //获取读写连接字符串
            var strConn = _contextConnectionFactory.CreateAllForContext();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, strConn, async (conn, transaction) =>
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
                    CommandText = @"delete from HttpClaimGenerator where [id]=@id"
                })
                {

                    SqlParameter parameter;

                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();
                }
            });

        }

        public async Task<HttpClaimGenerator> QueryByID(Guid id)
        {
            HttpClaimGenerator generator = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _contextConnectionFactory.CreateReadForContext(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"select {0} from HttpClaimGenerator where [id]=@id", StoreHelper.GetHttpClaimGeneratorSelectFields(string.Empty)),
                    Transaction = sqlTran
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
                            generator = new HttpClaimGenerator();
                            StoreHelper.SetHttpClaimGeneratorSelectFields(generator, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });


            return generator;
        }

        public async Task<HttpClaimGenerator> QueryByName(string name)
        {
            HttpClaimGenerator generator = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _contextConnectionFactory.CreateReadForContext(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"select {0} from HttpClaimGenerator where [name]=@name", StoreHelper.GetHttpClaimGeneratorSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {

                    var parameter = new SqlParameter("@name", SqlDbType.VarChar, 150)
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
                            generator = new HttpClaimGenerator();
                            StoreHelper.SetHttpClaimGeneratorSelectFields(generator, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });


            return generator;
        }

        public async Task<QueryResult<HttpClaimGenerator>> QueryByPage(string name, int page, int pageSize)
        {
            QueryResult<HttpClaimGenerator> result = new QueryResult<HttpClaimGenerator>();

            //获取只读连接字符串
            var strConn = _contextConnectionFactory.CreateReadForContext();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, strConn, async (conn, transaction) =>
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
		                           select @count= count(*) from HttpClaimGenerator where [name] like @name

                                    select {0} from HttpClaimGenerator where [name] like @name
                                    order by sequence
		                            offset (@pagesize * (@page - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetHttpClaimGeneratorSelectFields(string.Empty))
                })
                {
                    var parameter = new SqlParameter("@name", SqlDbType.VarChar, 150)
                    {
                        Value = $"{name.ToSqlLike()}%"
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@page", SqlDbType.Int)
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

                    await commond.PrepareAsync();

                    SqlDataReader reader = null;


                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var generator = new HttpClaimGenerator();
                            StoreHelper.SetHttpClaimGeneratorSelectFields(generator, reader, string.Empty);
                            result.Results.Add(generator);
                        }

                        await reader.CloseAsync();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = page;
                    }
                }
            });

            return result;
        }

        public async Task Update(HttpClaimGenerator generator)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _contextConnectionFactory.CreateAllForContext(), async (conn, transaction) =>
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
                    CommandText = @"update HttpClaimGenerator set [name]=@name,[type]=@type,[modifytime]=getutcdate()
                                    where [id]=@id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = generator.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@name", SqlDbType.VarChar, 150)
                    {
                        Value = generator.Name
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@type", SqlDbType.VarChar, 150)
                    {
                        Value = generator.Type
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();
                }

            });
        }
    }
}
