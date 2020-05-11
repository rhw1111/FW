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

namespace MSLibrary.Collections.Hash.DAL
{
    [Injection(InterfaceType = typeof(IHashGroupStrategyStore), Scope = InjectionScope.Singleton)]
    public class HashGroupStrategyStore : IHashGroupStrategyStore
    {
        private IHashConnectionFactory _hashConnectionFactory;

        public HashGroupStrategyStore(IHashConnectionFactory hashConnectionFactory)
        {
            _hashConnectionFactory = hashConnectionFactory;
        }

        public async Task Add(HashGroupStrategy strategy)
        {
            //获取读写连接字符串
            var strConn = _hashConnectionFactory.CreateAllForHash();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, strConn, async (conn, transaction) =>
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

                    if (strategy.ID != Guid.Empty)
                    {

                        commond.CommandText = @"
                                                insert into [dbo].[HashGroupStrategy](
                                                    [id]
                                                    ,[name]
                                                    ,[strategyservicefactorytype]
                                                    ,[strategyservicefactorytypeusedi]
                                                    ,[createtime]
                                                    ,[modifytime]
                                                )
                                                values(
                                                    @id
                                                    ,@name
                                                    ,@strategyservicefactorytype
                                                    ,@strategyservicefactorytypeusedi
                                                    ,getutcdate()
                                                    ,getutcdate()
                                                ) 
                                            ";

                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = strategy.ID
                        };

                        commond.Parameters.Add(parameter);
                    }
                    else
                    {
                        commond.CommandText = @"insert into [dbo].[HashGroupStrategy]
                                                (
                                                     [id]
                                                    ,[name]
                                                    ,[strategyservicefactorytype]
                                                    ,[strategyservicefactorytypeusedi]
                                                    ,[createtime]
                                                    ,[modifytime]
                                                )
                                                values(
                                                    default
                                                    ,@name
                                                    ,@strategyservicefactorytype
                                                    ,@strategyservicefactorytypeusedi
                                                    ,getutcdate()
                                                    ,getutcdate()
                                                )
                                                select @newid=[id] from [dbo].[HashGroupStrategy] 
                                                where [sequence]=SCOPE_IDENTITY()";

                        parameter = new SqlParameter("@newid", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };

                        commond.Parameters.Add(parameter);

                    }


                    parameter = new SqlParameter("@name", SqlDbType.VarChar, 100)
                    {
                        Value = strategy.Name
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@strategyservicefactorytype", SqlDbType.VarChar, 500)
                    {
                        Value = strategy.StrategyServiceFactoryType
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@strategyservicefactorytypeusedi", SqlDbType.Bit)
                    {
                        Value = strategy.StrategyServiceFactoryTypeUseDI
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    await commond.ExecuteNonQueryAsync();



                    if (strategy.ID == Guid.Empty)
                    {
                        strategy.ID = (Guid)commond.Parameters["@newid"].Value;
                    }
                }
            });
        }

        public async Task Delete(Guid id)
        {
            //获取读写连接字符串
            var strConn = _hashConnectionFactory.CreateAllForHash();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, strConn, async (conn, transaction) =>
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
                    CommandText = @"delete from [dbo].[HashGroupStrategy]		  
		                            where [id]=@id"
                })
                {

                    SqlParameter parameter;

                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();


                    await commond.ExecuteNonQueryAsync();



                }
            });
        }

        public async Task<HashGroupStrategy> QueryById(Guid id)
        {
            HashGroupStrategy strategy = null;

            //获取只读连接字符串
            var strConn = _hashConnectionFactory.CreateReadForHash();


            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, strConn, async (conn, transaction) =>
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
                    CommandText = string.Format(@"select {0} from HashGroupStrategy where [id]=@id", StoreHelper.GetHashGroupStrategySelectFields(string.Empty))
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
                            strategy = new HashGroupStrategy();
                            StoreHelper.SetHashGroupStrategySelectFields(strategy, reader, string.Empty);
                        }

                        reader.Close();
                    }
                }
            });

            return strategy;
        }

        public async Task<QueryResult<HashGroupStrategy>> QueryByName(string name, int page, int pageSize)
        {
            QueryResult<HashGroupStrategy> result = new QueryResult<HashGroupStrategy>();

            //获取只读连接字符串
            var strConn = _hashConnectionFactory.CreateReadForHash();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, strConn, async (conn, transaction) =>
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
                    CommandText = string.Format(@"set @currentpage=@page
		                           select @count= count(*) from HashGroupStrategy where [name] like @name
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
	
                                    select {0} from HashGroupStrategy where [name] like @name
                                    order by sequence
		                            offset (@pagesize * (@currentpage - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetHashGroupStrategySelectFields(string.Empty))
                })
                {
                    var parameter = new SqlParameter("@name", SqlDbType.VarChar, 100)
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
                            var record = new HashGroupStrategy();
                            StoreHelper.SetHashGroupStrategySelectFields(record, reader, string.Empty);
                            result.Results.Add(record);
                        }

                        reader.Close();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;
                    }
                }
            });

            return result;
        }

        public async Task Update(HashGroupStrategy strategy)
        {
            //获取读写连接字符串
            var strConn = _hashConnectionFactory.CreateAllForHash();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, strConn, async (conn, transaction) =>
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
                    CommandText = @"update [dbo].[HashGroupStrategy]
		                              set [name]=@name
                                        ,[strategyservicefactorytype]=@strategyservicefactorytype
                                        ,[strategyservicefactorytypeusedi]=@strategyservicefactorytypeusedi
                                        ,[modifytime]=getutcdate()
		                              where [id]=@id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = strategy.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@name", SqlDbType.VarChar, 100)
                    {
                        Value = strategy.Name
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@strategyservicefactorytype", SqlDbType.VarChar, 500)
                    {
                        Value = strategy.StrategyServiceFactoryType
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@strategyservicefactorytypeusedi", SqlDbType.Bit)
                    {
                        Value = strategy.StrategyServiceFactoryTypeUseDI
                    };
                    commond.Parameters.Add(parameter);


                    commond.Prepare();


                    await commond.ExecuteNonQueryAsync();


                }
            });
        }
    }
}
