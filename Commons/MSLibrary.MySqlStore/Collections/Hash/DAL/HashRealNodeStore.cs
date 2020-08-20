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
    [Injection(InterfaceType = typeof(IHashRealNodeStore), Scope = InjectionScope.Singleton)]
    public class HashRealNodeStore : IHashRealNodeStore
    {

        private IHashConnectionFactory _hashConnectionFactory;

        public HashRealNodeStore(IHashConnectionFactory hashConnectionFactory)
        {
            _hashConnectionFactory = hashConnectionFactory;
        }


        public async Task Add(HashRealNode node)
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

                using (MySqlCommand command = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                })
                {
                    MySqlParameter parameter;
                    if (node.ID == Guid.Empty)
                    {
                        node.ID = Guid.NewGuid();
                    }
                    else
                    {
                        command.CommandText = @"
                                                INSERT INTO hashrealnode
                                                      (
	                                                     id
                                                        ,name
	                                                    ,groupid
	                                                    ,nodekey
                                                        ,createtime
                                                        ,modifytime
                                                      )VALUES(
	                                                      @id
                                                          ,@name
	                                                      ,@groupid	  
	                                                      ,@nodekey
	                                                      ,utc_timestamp()
	                                                      ,utc_timestamp()
                                                      )";

                        parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                        {
                            Value = node.ID
                        };
                        command.Parameters.Add(parameter);
                    }

                    parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 100)
                    {
                        Value = node.Name
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@groupid", MySqlDbType.Guid)
                    {
                        Value = node.GroupId
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@nodekey", MySqlDbType.VarChar, 4000)
                    {
                        Value = node.NodeKey
                    };
                    command.Parameters.Add(parameter);

                    command.Prepare();

                    await command.ExecuteNonQueryAsync();

                }
            });
        }

        public async Task DeleteByRelation(Guid groupId, Guid id)
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
                using (MySqlCommand command = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = @"
                                   delete hashrealnode      
                                   where n.id = @id and groupid = @groupId"
                })
                {
                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@groupid", MySqlDbType.Guid)
                    {
                        Value = groupId
                    };
                    command.Parameters.Add(parameter);

                    command.Prepare();
                    await command.ExecuteNonQueryAsync();

                }
            });

        }

        public async Task QueryByAll(Guid groupId, Func<HashRealNode, Task> callback)
        {
            List<HashRealNode> result = new List<HashRealNode>();
            int size = 500;
            int perSize = 500;
            Guid? nodeId = null;

            //获取只读连接字符串
            var strConn = _hashConnectionFactory.CreateReadForHash();
            while (perSize == size)
            {
                result.Clear();

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
                        Transaction = sqlTran
                    })
                    {
                        if (nodeId==null)
                        {
                            commond.CommandText = string.Format(@"                                                            
                                                                select {0},{1},{2} from hashrealnode as n join hashgroup as g
                                                                on n.groupid=g.id
                                                                join hashgroupstrategy as s 
                                                                on g.strategyid=s.id                                                
                                                                where n.groupid=@groupid
                                                                order by n.id limit {3}", StoreHelper.GetHashRealNodeSelectFields("n"), StoreHelper.GetHashGroupSelectFields("g"), StoreHelper.GetHashGroupStrategySelectFields("s"),size);
                        }
                        else
                        {
                            commond.CommandText = string.Format(@"
                                                                select top (@size) {0},{1},{2} from HashRealNode as n join HashGroup as g
                                                                on n.groupid=g.id
                                                                join HashGroupStrategy as s 
                                                                on g.strategyid=s.id                                                
                                                                where n.[groupid]=@groupid and n.id>@nodeid
                                                                order by n.id limit {3}", StoreHelper.GetHashRealNodeSelectFields("n"), StoreHelper.GetHashGroupSelectFields("g"), StoreHelper.GetHashGroupStrategySelectFields("s"),size);
                        }

                        var parameter = new MySqlParameter("@groupid", MySqlDbType.Guid)
                        {
                            Value = groupId
                        };
                        commond.Parameters.Add(parameter);

                        if (nodeId == null)
                        {
                            parameter = new MySqlParameter("@nodeid", MySqlDbType.Guid)
                            {
                                Value = DBNull.Value
                            };
                        }
                        else
                        {
                            parameter = new MySqlParameter("@nodeid", MySqlDbType.Guid)
                            {
                                Value = nodeId
                            };
                        }
                        commond.Parameters.Add(parameter);



                        commond.Prepare();


                        DbDataReader reader = null;

                        using (reader = await commond.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var record = new HashRealNode();
                                StoreHelper.SetHashRealNodeSelectFields(record, reader, "n");
                                record.Group = new HashGroup();
                                StoreHelper.SetHashGroupSelectFields(record.Group, reader, "g");
                                record.Group.Strategy = new HashGroupStrategy();
                                StoreHelper.SetHashGroupStrategySelectFields(record.Group.Strategy, reader, "s");
                                result.Add(record);

                                nodeId = record.ID;
                            }

                            reader.Close();
                        }
                    }
                });

                perSize = result.Count;

                foreach (var item in result)
                {
                    await callback(item);
                }
            }
        }

        public async Task<HashRealNode> QueryByGroup(Guid groupId, Guid nodeId)
        {
            //获取读写连接字符串
            var strConn = _hashConnectionFactory.CreateAllForHash();
            HashRealNode node = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, strConn, async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }
                using (MySqlCommand command = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"
                                    select {0},{1},{2} from hashrealnode as n join hashgroup as g
                                                                on n.groupid=g.id
                                                                join hashgroupstrategy as s 
                                                                on g.strategyid=s.id        
                                   where n.id = @id and groupid = @groupId",
                                   StoreHelper.GetHashRealNodeSelectFields("n"),
                                   StoreHelper.GetHashGroupSelectFields("g"),
                                   StoreHelper.GetHashGroupStrategySelectFields("s"))
                })
                {
                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = nodeId
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@groupid", MySqlDbType.Guid)
                    {
                        Value = groupId
                    };


                    command.Parameters.Add(parameter);
                    command.Prepare();
                    DbDataReader reader = null;
                    using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            StoreHelper.SetHashRealNodeSelectFields(node, reader, "n");

                            node.Group = new HashGroup();
                            StoreHelper.SetHashGroupSelectFields(node.Group, reader, "g");
                            node.Group.Strategy = new HashGroupStrategy();
                            StoreHelper.SetHashGroupStrategySelectFields(node.Group.Strategy, reader, "s");

                        }
                        reader.Close();
                    }
                }
            });

            return node;
        }

        public async Task<QueryResult<HashRealNode>> QueryByGroup(Guid groupId, int page, int pageSize)
        {
            QueryResult<HashRealNode> result = new QueryResult<HashRealNode>();

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
                    CommandText = string.Format(@"select count(*) from hashrealnode 
                                                  where groupid=@groupid
		                                          
	
                                                  select {0},{1},{2} from hashrealnode as n join hashgroup as g
                                                  on n.groupid=g.id            
                                                  join hashgroupstrategy as s 
                                                  on g.strategyid=s.id  
                                                  where n.sequence in
                                                    (
                                                    select t.sequence from
                                                    (
                                                        select sequence from hashrealnode
                                                         where groupid=@groupid                                                    
                                                        order by code limit {3}, {4}                                                   
                                                    ) as t
                                                    )", StoreHelper.GetHashRealNodeSelectFields("n"), StoreHelper.GetHashGroupSelectFields("g"), StoreHelper.GetHashGroupStrategySelectFields("s"), (page - 1) * pageSize, pageSize)
                })
                {
                    var parameter = new MySqlParameter("@groupid", MySqlDbType.Guid)
                    {
                        Value = groupId
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    MySqlDataReader reader = null;

                    using (reader = await commond.ExecuteReaderAsync())
                    {
                        result.TotalCount = reader.GetInt32(0);
                        result.CurrentPage = page;
                        if (await reader.NextResultAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var record = new HashRealNode();
                                StoreHelper.SetHashRealNodeSelectFields(record, reader, "n");
                                record.Group = new HashGroup();
                                StoreHelper.SetHashGroupSelectFields(record.Group, reader, "g");
                                record.Group.Strategy = new HashGroupStrategy();
                                StoreHelper.SetHashGroupStrategySelectFields(record.Group.Strategy, reader, "s");
                                result.Results.Add(record);
                            }
                        }

                        reader.Close();


                    }
                }
            });

            return result;
        }

        public async Task<List<HashRealNode>> QueryHashRealNodeByCreateTime(Guid groupId, int skipNum, int takeNum)
        {

            List<HashRealNode> result = new List<HashRealNode>();

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
                                                  select {0},{1},{2} from hashrealnode as n join hashgroup as g
                                                  on n.groupid=g.id            
                                                  join HashGroupStrategy as s 
                                                  on g.strategyid=s.id           
                                                  where n.groupid=@groupid
                                                  order by nsequence
		                                          limit {3},{4}", StoreHelper.GetHashRealNodeSelectFields("n"), StoreHelper.GetHashGroupSelectFields("g"), StoreHelper.GetHashGroupStrategySelectFields("s"),skipNum.ToString(),takeNum.ToString())
                })
                {
                    var parameter = new MySqlParameter("@groupid", MySqlDbType.Guid)
                    {
                        Value = groupId
                    };
                    commond.Parameters.Add(parameter);


                    commond.Prepare();

                    DbDataReader reader = null;

                    using (reader = await commond.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var record = new HashRealNode();
                            StoreHelper.SetHashRealNodeSelectFields(record, reader, "n");
                            record.Group = new HashGroup();
                            StoreHelper.SetHashGroupSelectFields(record.Group, reader, "g");
                            record.Group.Strategy = new HashGroupStrategy();
                            StoreHelper.SetHashGroupStrategySelectFields(record.Group.Strategy, reader, "s");
                            result.Add(record);
                        }
                        reader.Close();
                    }
                }
            });

            return result;
        }

        public async Task Update(Guid groupId, HashRealNode node)
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

                using (MySqlCommand command = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                })
                {
                    MySqlParameter parameter;

                    command.CommandText = @"
                                             update hashrealnode
                                               
                                             set nodekey = @nodekey
                                               ,name = @name
                                               ,modifytime = utc_timestamp()

                                             where id = @id and groupid = @groupId";

                    parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = node.ID
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@groupid", MySqlDbType.Guid)
                    {
                        Value = node.GroupId
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 100)
                    {
                        Value = node.Name
                    };
                    command.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@nodekey", MySqlDbType.VarChar, 4000)
                    {
                        Value = node.NodeKey
                    };
                    command.Parameters.Add(parameter);

                    command.Prepare();

                    await command.ExecuteNonQueryAsync();


                }
            });
        }

    }
}
