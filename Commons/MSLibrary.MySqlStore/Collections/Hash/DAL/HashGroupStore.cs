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
    [Injection(InterfaceType = typeof(IHashGroupStore), Scope = InjectionScope.Singleton)]
    public class HashGroupStore : IHashGroupStore
    {
        private IHashConnectionFactory _hashConnectionFactory;

        public HashGroupStore(IHashConnectionFactory hashConnectionFactory)
        {
            _hashConnectionFactory = hashConnectionFactory;
        }


        public async Task Add(HashGroup group)
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
                    if (group.ID == Guid.Empty)
                    {
                        group.ID = Guid.NewGuid();
                    }

                        commond.CommandText = @"
                                                insert into hashgroup(id,name,count,strategyid,createtime,modifytime)
                                                values( @id,@name,@count,@strategyid,utc_timestamp(),utc_timestamp())	
		                                       ";

                        parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                        {
                            Value = group.ID
                        };
                        commond.Parameters.Add(parameter);

                    

                    parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 100)
                    {
                        Value = group.Name
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@type", MySqlDbType.VarChar, 100)
                    {
                        Value = group.Type
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@count", MySqlDbType.Int64)
                    {
                        Value = group.Count
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@strategyid", MySqlDbType.Guid)
                    {
                        Value = group.StrategyID
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
                    CommandText = @" delete from 
                                    hashgroup		  
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

        public async Task<HashGroup> QueryById(Guid id)
        {
            HashGroup group = null;

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
                    CommandText = string.Format(@"select {0},{1} from hashgroup as g join hashgroupstrategy as s 
                                                  on g.strategyid=s.id
                                                  where g.id=@id", StoreHelper.GetHashGroupSelectFields("g"), StoreHelper.GetHashGroupStrategySelectFields("s"))
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
                            group = new HashGroup();
                            StoreHelper.SetHashGroupSelectFields(group, reader, "g");
                            group.Strategy = new HashGroupStrategy();
                            StoreHelper.SetHashGroupStrategySelectFields(group.Strategy, reader, "s");
                        }

                        reader.Close();
                    }
                }
            });

            return group;
        }

        public async Task<QueryResult<HashGroup>> QueryByName(string name, int page, int pageSize)
        {
            QueryResult<HashGroup> result = new QueryResult<HashGroup>();

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
		                           select count(*) from hashgroup where name like @name

                                   select {0},{1} from hashgroup as g join hashgroupstrategy as s on g.strategyid=s.id where g.sequence in
                                                (
                                                select t.sequence from
                                                (
                                                    select sequence from hashgroup where name like @name
                                                    order by sequence limit {2}, {3}                                                   
                                                ) as t
                                                )", StoreHelper.GetHashGroupSelectFields("g"), StoreHelper.GetHashGroupSelectFields("s"), (page - 1) * pageSize, pageSize)
                })
                {
                    var parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 150)
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
                                    var record = new HashGroup();
                                    StoreHelper.SetHashGroupSelectFields(record, reader, "g");
                                    record.Strategy = new HashGroupStrategy();
                                    StoreHelper.SetHashGroupStrategySelectFields(record.Strategy, reader, "s");
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

        public async Task<HashGroup> QueryByName(string name)
        {

            HashGroup group = null;

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
                    CommandText = string.Format(@"select {0},{1} from hashgroup as g join hashgroupstrategy as s 
                                                  on g.strategyid=s.id
                                                  where g.[name]=@name", StoreHelper.GetHashGroupSelectFields("g"), StoreHelper.GetHashGroupStrategySelectFields("s"))
                })
                {

                    var parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 100)
                    {
                        Value = name
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    DbDataReader reader = null;

                    using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            group = new HashGroup();
                            StoreHelper.SetHashGroupSelectFields(group, reader, "g");
                            group.Strategy = new HashGroupStrategy();
                            StoreHelper.SetHashGroupStrategySelectFields(group.Strategy, reader, "s");
                        }

                        reader.Close();
                    }
                }
            });

            return group;
        }

        public async Task Update(HashGroup group)
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
                    CommandText = @"update hashgroup
                                    set 
                                        name=@name
                                        ,count=@count
                                        ,strategyid=@strategyid
                                        ,modifytime=utc_timestamp()
                                    where id=@id"
                })
                {

                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = group.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 100)
                    {
                        Value = group.Name
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@count", MySqlDbType.Int64)
                    {
                        Value = group.Count
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@strategyid", MySqlDbType.Guid)
                    {
                        Value = group.StrategyID
                    };
                    commond.Parameters.Add(parameter);


                    commond.Prepare();

                    await commond.ExecuteNonQueryAsync();
                }
            });
        }

        public HashGroup QueryByNameSync(string name)
        {
            HashGroup group = null;

            //获取只读连接字符串
            var strConn = _hashConnectionFactory.CreateReadForHash();


            DBTransactionHelper.SqlTransactionWork(DBTypes.MySql, true, false, strConn, (conn, transaction) =>
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
                    CommandText = string.Format(@"select {0},{1} from hashgroup as g join hashgroupstrategy as s 
                                                  on g.strategyid=s.id
                                                  where g.name=@name", StoreHelper.GetHashGroupSelectFields("g"), StoreHelper.GetHashGroupStrategySelectFields("s"))
                })
                {

                    var parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 100)
                    {
                        Value = name
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    DbDataReader reader = null;

                    using (reader = commond.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            group = new HashGroup();
                            StoreHelper.SetHashGroupSelectFields(group, reader, "g");
                            group.Strategy = new HashGroupStrategy();
                            StoreHelper.SetHashGroupStrategySelectFields(group.Strategy, reader, "s");
                        }

                        reader.Close();
                    }
                }
            });

            return group;
        }

        public async Task QueryByType(string type, Func<HashGroup, Task> action)
        {
            List<HashGroup> result = new List<HashGroup>();
            int? index = null;
            long count = 500;

            //获取只读连接字符串
            var strConn = _hashConnectionFactory.CreateReadForHash();

            while (true)
            {
                result.Clear();


                DBTransactionHelper.SqlTransactionWork(DBTypes.MySql, true, false, strConn, (conn, transaction) =>
                {
                    MySqlTransaction sqlTran = null;
                    if (transaction != null)
                    {
                        sqlTran = (MySqlTransaction)transaction;
                    }

                    string strSql = string.Format(@"select {0},{1} from hashgroup as g join hashgroupstrategy as s 
                                                  on g.strategyid=s.id
                                                  where g.type=@type order by g.sequence limit {2}", StoreHelper.GetHashGroupSelectFields("g"), StoreHelper.GetHashGroupStrategySelectFields("s"),count.ToString());
                    if (index != null)
                    {
                        strSql = string.Format(@"select {0},{1} from hashgroup as g join hashgroupstrategy as s 
                                                  on g.strategyid=s.id
                                                  where g.type=@type and g.sequence>@index order by g.sequence limit {2}", StoreHelper.GetHashGroupSelectFields("g"), StoreHelper.GetHashGroupStrategySelectFields("s"), count.ToString());
                    }

                    using (MySqlCommand commond = new MySqlCommand()
                    {
                        Connection = (MySqlConnection)conn,
                        CommandType = CommandType.Text,
                        Transaction = sqlTran,
                        CommandText = strSql
                    })
                    {

                        var parameter = new MySqlParameter("@top", MySqlDbType.Int32)
                        {
                            Value = count
                        };
                        commond.Parameters.Add(parameter);

                        parameter = new MySqlParameter("@type", MySqlDbType.VarChar, 100)
                        {
                            Value = type
                        };
                        commond.Parameters.Add(parameter);

                        if (index != null)
                        {
                            parameter = new MySqlParameter("@index", MySqlDbType.Int64)
                            {
                                Value = index
                            };
                            commond.Parameters.Add(parameter);
                        }

                        commond.Prepare();

                        DbDataReader reader = null;

                        using (reader = commond.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var group = new HashGroup();
                                StoreHelper.SetHashGroupSelectFields(group, reader, "g");
                                group.Strategy = new HashGroupStrategy();
                                StoreHelper.SetHashGroupStrategySelectFields(group.Strategy, reader, "s");
                                result.Add(group);
                            }

                            reader.Close();
                        }
                    }
                });



                foreach (var item in result)
                {
                    await action(item);
                }

                if (result.Count < count)
                {
                    break;
                }
            }
        }
    }
}
