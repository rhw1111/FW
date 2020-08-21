using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using MSLibrary.DI;
using MSLibrary.Transaction;
using MSLibrary.DAL;
using MSLibrary.Collections.Hash;
using MSLibrary.Collections.Hash.DAL;

namespace MSLibrary.MySqlStore.Collections.Hash.DAL
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
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, strConn, async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }

                using (MySqlCommand commond = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran
                })
                {
                    MySqlParameter parameter;

                    if (strategy.ID == Guid.Empty)
                    {
                        strategy.ID = Guid.NewGuid();

                    }
                    else
                    
                        commond.CommandText = @"
                                                insert into hashgroupstrategy(
                                                    id
                                                    ,name
                                                    ,strategyservicefactorytype
                                                    ,strategyservicefactorytypeusedi
                                                    ,createtime
                                                    ,modifytime
                                                )
                                                values(
                                                    @id
                                                    ,@name
                                                    ,@strategyservicefactorytype
                                                    ,@strategyservicefactorytypeusedi
                                                    ,utc_timestamp()
                                                    ,utc_timestamp()
                                                ) 
                                            ";

                        parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                        {
                            Value = strategy.ID
                        };

                        commond.Parameters.Add(parameter);

                    


                    parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 100)
                    {
                        Value = strategy.Name
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new MySqlParameter("@strategyservicefactorytype", MySqlDbType.VarChar, 500)
                    {
                        Value = strategy.StrategyServiceFactoryType
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@strategyservicefactorytypeusedi", MySqlDbType.Bit)
                    {
                        Value = strategy.StrategyServiceFactoryTypeUseDI
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    await commond.ExecuteNonQueryAsync();

                }
            });
        }

        public async Task Delete(Guid id)
        {
            //获取读写连接字符串
            var strConn = _hashConnectionFactory.CreateAllForHash();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, strConn, async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }

                using (MySqlCommand commond = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = @"delete from hashgroupstrategy		  
		                            where id=@id"
                })
                {

                    MySqlParameter parameter;

                    parameter = new MySqlParameter("@id", MySqlDbType.Guid)
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


            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, strConn, async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }

                using (MySqlCommand commond = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = string.Format(@"select {0} from hashgroupstrategy where id=@id", StoreHelper.GetHashGroupStrategySelectFields(string.Empty))
                })
                {

                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    DbDataReader reader = null;


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

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, strConn, async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }

                using (MySqlCommand commond = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = string.Format(@"
		                           select count(*) from hashgroupstrategy where name like @name
	

	                                   select {0} from hashgroupstrategy
                                                where sequence in
                                                (
                                                select t.sequence from
                                                (
                                                    select sequence from hashgroupstrategy 
                                                    where name like @name
                                                    order by sequence limit {1}, {2}                                                   
                                                ) as t
                                                )", StoreHelper.GetHashGroupStrategySelectFields(string.Empty), (page - 1) * pageSize, pageSize)
                })
                {
                    var parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 100)
                    {
                        Value = $"{name.ToMySqlLike()}%"
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();


                    DbDataReader reader = null;


                    using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result.TotalCount = reader.GetInt32(0);
                            result.CurrentPage = page;
                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    var record = new HashGroupStrategy();
                                    StoreHelper.SetHashGroupStrategySelectFields(record, reader, string.Empty);
                                    result.Results.Add(record);
                                }
                            }
                        }

                        reader.Close();

                    }
                }
            });

            return result;
        }

        public async Task Update(HashGroupStrategy strategy)
        {
            //获取读写连接字符串
            var strConn = _hashConnectionFactory.CreateAllForHash();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, strConn, async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }

                using (MySqlCommand commond = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = @"update hashgroupstrategy
		                              set name=@name
                                        ,strategyservicefactorytype=@strategyservicefactorytype
                                        ,strategyservicefactorytypeusedi=@strategyservicefactorytypeusedi
                                        ,modifytime=utc_timestamp()
		                              where [id]=@id"
                })
                {

                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = strategy.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 100)
                    {
                        Value = strategy.Name
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@strategyservicefactorytype", MySqlDbType.VarChar, 500)
                    {
                        Value = strategy.StrategyServiceFactoryType
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@strategyservicefactorytypeusedi", MySqlDbType.Bit)
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
