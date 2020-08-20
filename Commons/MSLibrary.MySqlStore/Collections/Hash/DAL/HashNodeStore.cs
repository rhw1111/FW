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
    [Injection(InterfaceType = typeof(IHashNodeStore), Scope = InjectionScope.Singleton)]
    public class HashNodeStore : IHashNodeStore
    {
        private IHashConnectionFactory _hashConnectionFactory;

        public HashNodeStore(IHashConnectionFactory hashConnectionFactory)
        {
            _hashConnectionFactory = hashConnectionFactory;
        }


        public async Task Add(HashNode node)
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

                    if (node.ID == Guid.Empty)
                    {
                        node.ID = Guid.NewGuid();
                    }


                    commond.CommandText = @"
                                                INSERT INTO hashnode(
                                                        id
                                                        ,groupid
                                                        ,realnodeid
                                                        ,code
                                                        ,status
                                                        ,createtime
                                                        ,modifytime
                                                        )
                                                VALUES(
                                                         @id
                                                        ,@groupid
                                                        ,@realnodeid
                                                        ,@code
                                                        ,@status
                                                        ,utc_timestamp()
                                                        ,utc_timestamp()
                                                )	
                                              ";

                    parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = node.ID
                    };
                    commond.Parameters.Add(parameter);



                    parameter = new MySqlParameter("@realnodeid", MySqlDbType.Guid)
                    {
                        Value = node.RealNodeId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@groupid", MySqlDbType.Guid)
                    {
                        Value = node.GroupId
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new MySqlParameter("@code", MySqlDbType.Int64)
                    {
                        Value = node.Code
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@status", MySqlDbType.Int32)
                    {
                        Value = node.Status
                    };
                    commond.Parameters.Add(parameter);


                    commond.Prepare();

                    await commond.ExecuteNonQueryAsync();
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

                using (MySqlCommand commond = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = @"DELETE FROM hashnode]		  
		                            WHERE [id]=@id AND groupid=@groupid"
                })
                {

                    MySqlParameter parameter;

                    parameter = new MySqlParameter("@groupid", MySqlDbType.Guid)
                    {
                        Value = groupId
                    };
                    commond.Parameters.Add(parameter);

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

        public async Task<QueryResult<HashNode>> QueryByGroup(Guid groupId, int page, int pageSize)
        {
            QueryResult<HashNode> result = new QueryResult<HashNode>();

            //获取只读连接字符串
            var strConn = _hashConnectionFactory.CreateReadForHash();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql , true, false, strConn, async (conn, transaction) =>
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
                    CommandText = string.Format(@"select count(*) from hashnode
                                                  where groupid=@groupid
		                                          
	
                                                  select {0},{1},{2},{3} from hashnode as n join hashgroup as g
                                                  on n.groupid=g.id
                                                  join hashgroupstrategy as s  on g.strategyid=s.id    
                                                  join hashrealnode as r on r.id = n.realnodeid
                                                  where n.sequence in
                                                (
                                                select t.sequence from
                                                (
                                                    select sequence from hashnode where where groupid=@groupid
                                                    order by sequence limit {4}, {5}                                                   
                                                ) as t
                                                )",
                                                  StoreHelper.GetHashNodeSelectFields("n"),
                                                  StoreHelper.GetHashGroupSelectFields("g"),
                                                  StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                  StoreHelper.GetHashRealNodeSelectFields("r"), (page - 1) * pageSize, pageSize)
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
                        if (await reader.ReadAsync())
                        {
                            result.TotalCount = reader.GetInt32(0);
                            result.CurrentPage = page;
                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    var record = new HashNode();
                                    StoreHelper.SetHashNodeSelectFields(record, reader, "n");
                                    record.Group = new HashGroup();
                                    StoreHelper.SetHashGroupSelectFields(record.Group, reader, "g");
                                    record.Group.Strategy = new HashGroupStrategy();
                                    StoreHelper.SetHashGroupStrategySelectFields(record.Group.Strategy, reader, "s");
                                    record.RealNode = new HashRealNode();
                                    StoreHelper.SetHashRealNodeSelectFields(record.RealNode, reader, "r");

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

        public async Task<HashNode> QueryByGroup(Guid groupId, Guid nodeId)
        {
            {
                //获取读写连接字符串
                var strConn = _hashConnectionFactory.CreateAllForHash();
                HashNode node = null;

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
                                    select {0},{1},{2},{3} from hashnode as n join hashgroup as g
                                                                on n.groupid=g.id
                                                                join hashgroupstrategy as s 
                                                                on g.strategyid=s.id
                                                                join hashrealnode r on (r.id = n.realnodeid)
                                   where n.id = @id and groupid = @groupId",
                                       StoreHelper.GetHashNodeSelectFields("n"),
                                       StoreHelper.GetHashGroupSelectFields("g"),
                                       StoreHelper.GetHashGroupStrategySelectFields("s"),
                                       StoreHelper.GetHashRealNodeSelectFields("r")
                                       )
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
                                StoreHelper.SetHashNodeSelectFields(node, reader, "n");

                                node.Group = new HashGroup();
                                StoreHelper.SetHashGroupSelectFields(node.Group, reader, "g");
                                node.Group.Strategy = new HashGroupStrategy();
                                StoreHelper.SetHashGroupStrategySelectFields(node.Group.Strategy, reader, "s");
                                node.RealNode = new HashRealNode();
                                StoreHelper.SetHashRealNodeSelectFields(node.RealNode, reader, "r");
                            }
                            reader.Close();
                        }
                    }
                });

                return node;
            }
        }

        public async Task<HashNode> QueryByMaxCode(Guid groupId, params int[] status)
        {
            HashNode node = null;

            StringBuilder strStatusCondition = new StringBuilder();

            if (status != null && status.Length > 0)
            {
                strStatusCondition.Append("and status in(");
                for (var index = 0; index <= status.Length - 1; index++)
                {
                    strStatusCondition.Append(status[index]);
                    if (index != status.Length - 1)
                    {
                        strStatusCondition.Append(",");
                    }
                }
                strStatusCondition.Append(")");
            }

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
                    CommandText = string.Format(@"select top 1 {0},{1},{2},{3} from hashnode as n join hashgroup as g
                                                  on n.groupid=g.id
                                                  join hashgroupstrategy as s 
                                                  on g.strategyid=s.id  
                                                    join hashrealnode r on r.id = n.realnodeid
                                                  where n.groupid=@groupid {4}
		                                          order by n.code desc",
                                                  StoreHelper.GetHashNodeSelectFields("n"),
                                                  StoreHelper.GetHashGroupSelectFields("g"),
                                                  StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                  StoreHelper.GetHashRealNodeSelectFields("r"),
                                                  strStatusCondition.ToString())
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
                        if (await reader.ReadAsync())
                        {
                            node = new HashNode();
                            StoreHelper.SetHashNodeSelectFields(node, reader, "n");
                            node.Group = new HashGroup();
                            StoreHelper.SetHashGroupSelectFields(node.Group, reader, "g");
                            node.Group.Strategy = new HashGroupStrategy();
                            StoreHelper.SetHashGroupStrategySelectFields(node.Group.Strategy, reader, "s");
                            node.RealNode = new HashRealNode();
                            StoreHelper.SetHashRealNodeSelectFields(node.RealNode, reader, "r");
                        }

                        reader.Close();
                    }
                }
            });

            return node;
        }

        public async Task<HashNode> QueryByMaxCode(Guid groupId)
        {
            HashNode node = null;

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
                    CommandText = string.Format(@"select {0},{1},{2},{3} from hashnode as n join hashgroup as g
                                                  on n.groupid=g.id
                                                  join hashgroupstrategy as s 
                                                  on g.strategyid=s.id       
                                                  join hashrealnode r on r.id = n.realnodeid
                                                  where n.groupid=@groupid
		                                          order by n.code desc limit 1",
                                                  StoreHelper.GetHashNodeSelectFields("n"),
                                                  StoreHelper.GetHashGroupSelectFields("g"),
                                                  StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                  StoreHelper.GetHashRealNodeSelectFields("r"))
                })
                {

                    var parameter = new MySqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                    {
                        Value = groupId
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    DbDataReader reader = null;

                    using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            node = new HashNode();
                            StoreHelper.SetHashNodeSelectFields(node, reader, "n");
                            node.Group = new HashGroup();
                            StoreHelper.SetHashGroupSelectFields(node.Group, reader, "g");
                            node.Group.Strategy = new HashGroupStrategy();
                            StoreHelper.SetHashGroupStrategySelectFields(node.Group.Strategy, reader, "s");
                            node.RealNode = new HashRealNode();
                            StoreHelper.SetHashRealNodeSelectFields(node.RealNode, reader, "r");

                        }

                        reader.Close();
                    }
                }
            });

            return node;
        }

        public async Task<HashNode> QueryByMinCode(Guid groupId, params int[] status)
        {
            HashNode node = null;

            StringBuilder strStatusCondition = new StringBuilder();

            if (status != null && status.Length > 0)
            {
                strStatusCondition.Append("and status in(");
                for (var index = 0; index <= status.Length - 1; index++)
                {
                    strStatusCondition.Append(status[index]);
                    if (index != status.Length - 1)
                    {
                        strStatusCondition.Append(",");
                    }
                }
                strStatusCondition.Append(")");
            }

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
                    CommandText = string.Format(@"select {0},{1},{2},{3} from hashnode as n join hashgroup as g
                                                  on n.groupid=g.id
                                                  join hashgroupstrategy as s 
                                                  on g.strategyid=s.id 
                                                  join hashrealnode r on r.id = n.realnodeid
                                                  where n.groupid=@groupid {4}
		                                          order by n.code limit 1", StoreHelper.GetHashNodeSelectFields("n")
                                                  , StoreHelper.GetHashGroupSelectFields("g")
                                                  , StoreHelper.GetHashGroupStrategySelectFields("s")
                                                  , StoreHelper.GetHashRealNodeSelectFields("r")
                                                  , strStatusCondition.ToString())
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
                        if (await reader.ReadAsync())
                        {
                            node = new HashNode();
                            StoreHelper.SetHashNodeSelectFields(node, reader, "n");
                            node.Group = new HashGroup();
                            StoreHelper.SetHashGroupSelectFields(node.Group, reader, "g");
                            node.Group.Strategy = new HashGroupStrategy();
                            StoreHelper.SetHashGroupStrategySelectFields(node.Group.Strategy, reader, "s");
                            node.RealNode = new HashRealNode();
                            StoreHelper.SetHashRealNodeSelectFields(node.RealNode, reader, "r");
                        }

                        reader.Close();
                    }
                }
            });

            return node;
        }

        public async Task<HashNode> QueryByMinCode(Guid groupId)
        {
            HashNode node = null;

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
                    CommandText = string.Format(@"select top 1 {0},{1},{2},{3} from hashnode as n join hashgroup as g
                                                  on n.groupid=g.id
                                                  join hashgroupstrategy as s 
                                                  on g.strategyid=s.id         
                                                  join hashrealnode r on r.id = n.realnodeid                                       
                                                  where n.groupid=@groupid
		                                          order by n.code",
                                                  StoreHelper.GetHashNodeSelectFields("n"),
                                                  StoreHelper.GetHashGroupSelectFields("g"),
                                                  StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                  StoreHelper.GetHashRealNodeSelectFields("r")
                                                  )
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
                        if (await reader.ReadAsync())
                        {
                            node = new HashNode();
                            StoreHelper.SetHashNodeSelectFields(node, reader, "n");
                            node.Group = new HashGroup();
                            StoreHelper.SetHashGroupSelectFields(node.Group, reader, "g");
                            node.Group.Strategy = new HashGroupStrategy();
                            StoreHelper.SetHashGroupStrategySelectFields(node.Group.Strategy, reader, "s");
                            node.RealNode = new HashRealNode();
                            StoreHelper.SetHashRealNodeSelectFields(node.RealNode, reader, "r");
                        }

                        reader.Close();
                    }
                }
            });

            return node;
        }

        public HashNode QueryByMinCodeSync(Guid groupId, params int[] status)
        {
            HashNode node = null;

            StringBuilder strStatusCondition = new StringBuilder();

            if (status != null && status.Length > 0)
            {
                strStatusCondition.Append("and status in(");
                for (var index = 0; index <= status.Length - 1; index++)
                {
                    strStatusCondition.Append(status[index]);
                    if (index != status.Length - 1)
                    {
                        strStatusCondition.Append(",");
                    }
                }
                strStatusCondition.Append(")");
            }

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
                    CommandText = string.Format(@"select top 1 {0},{1},{2},{3} from hashnode as n join hashgroup as g
                                                  on n.groupid=g.id
                                                  join hashgroupstrategy as s 
                                                  on g.strategyid=s.id    
                                                  join hashrealnode r on r.id = n.realnodeid                                            
                                                  where n.groupid=@groupid {4}
		                                          order by n.code",
                                                   StoreHelper.GetHashNodeSelectFields("n"),
                                                   StoreHelper.GetHashGroupSelectFields("g"),
                                                   StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                   StoreHelper.GetHashRealNodeSelectFields("r"),
                                                   strStatusCondition.ToString())
                })
                {

                    var parameter = new MySqlParameter("@groupid", MySqlDbType.Guid)
                    {
                        Value = groupId
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    DbDataReader reader = null;

                    using (reader = commond.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            node = new HashNode();
                            StoreHelper.SetHashNodeSelectFields(node, reader, "n");
                            node.Group = new HashGroup();
                            StoreHelper.SetHashGroupSelectFields(node.Group, reader, "g");
                            node.Group.Strategy = new HashGroupStrategy();
                            StoreHelper.SetHashGroupStrategySelectFields(node.Group.Strategy, reader, "s");
                            node.RealNode = new HashRealNode();
                            StoreHelper.SetHashRealNodeSelectFields(node.RealNode, reader, "r");
                        }

                        reader.Close();
                    }
                }
            });

            return node;
        }

        public async Task QueryByStatus(Guid groupId, int status, Func<HashNode, Task> callback)
        {
            List<HashNode> result = new List<HashNode>();
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
                                                                select {0},{1},{2},{3} from hashnode as n join hashgroup as g
                                                                on n.groupid=g.id
                                                                join hashgroupstrategy as s 
                                                                on g.strategyid=s.id             
                                                                join hashrealnode r on r.id = n.realnodeid
                                                                where n.groupid=@groupid
                                                                order by n.id limit {4}",
                                                            StoreHelper.GetHashNodeSelectFields("n"),
                                                            StoreHelper.GetHashGroupSelectFields("g"),
                                                            StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                            StoreHelper.GetHashRealNodeSelectFields("r"),
                                                            size
                                                            );
                        }
                        else
                        {
                            commond.CommandText = string.Format(@"                                                           
                                                                select {0},{1},{2},{3} from hashNode as n join hashgroup as g
                                                                on n.groupid=g.id
                                                                join hashgroupstrategy as s 
                                                                on g.strategyid=s.id 
                                                                join hashrealnode r on r.id = n.realnodeid
                                                                where n.groupid=@groupid and n.id>@nodeid
                                                                order by n.id limit {4}                                 
                                                            ",
                                                            StoreHelper.GetHashNodeSelectFields("n"),
                                                            StoreHelper.GetHashGroupSelectFields("g"),
                                                            StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                            StoreHelper.GetHashRealNodeSelectFields("r"),
                                                            size
                                                            );
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

                        parameter = new MySqlParameter("@size", MySqlDbType.Int32)
                        {
                            Value = size
                        };
                        commond.Parameters.Add(parameter);


                        commond.Prepare();


                        DbDataReader reader = null;


                        using (reader = await commond.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var record = new HashNode();
                                StoreHelper.SetHashNodeSelectFields(record, reader, "n");
                                record.Group = new HashGroup();
                                StoreHelper.SetHashGroupSelectFields(record.Group, reader, "g");
                                record.Group.Strategy = new HashGroupStrategy();
                                StoreHelper.SetHashGroupStrategySelectFields(record.Group.Strategy, reader, "s");
                                record.RealNode = new HashRealNode();
                                StoreHelper.SetHashRealNodeSelectFields(record.RealNode, reader, "r");

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

        public async Task<QueryResult<HashNode>> QueryByStatus(Guid groupId, int status, int page, int pageSize)
        {
            QueryResult<HashNode> result = new QueryResult<HashNode>();

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
		                                          select count(*) from hashnode
                                                  where groupid=@groupid
                                                    and status = @status
	
                                                  select {0},{1},{2},{3} from hashnode as n join hashgroup as g
                                                  on n.groupid=g.id
                                                  join hashgroupstrategy as s  on g.strategyid=s.id    
                                                  join hashrealnode as r on r.id = n.realnodeid
                                                where n.sequence in
                                                (
                                                select t.sequence from
                                                (
                                                    select sequence from hashnode
                                                    where groupid=@groupid
                                                    and status = @status
                                                    order by sequence limit {4}, {5}                                                   
                                                ) as t
                                                )",
                                                  StoreHelper.GetHashNodeSelectFields("n"),
                                                  StoreHelper.GetHashGroupSelectFields("g"),
                                                  StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                  StoreHelper.GetHashRealNodeSelectFields("r"),
                                                  (page - 1) * pageSize, pageSize
                                                  )
                })
                {
                    var parameter = new MySqlParameter("@groupid", MySqlDbType.Guid)
                    {
                        Value = groupId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@status", MySqlDbType.Int32)
                    {
                        Value = status
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
                                    var record = new HashNode();
                                    StoreHelper.SetHashNodeSelectFields(record, reader, "n");
                                    record.Group = new HashGroup();
                                    StoreHelper.SetHashGroupSelectFields(record.Group, reader, "g");
                                    record.Group.Strategy = new HashGroupStrategy();
                                    StoreHelper.SetHashGroupStrategySelectFields(record.Group.Strategy, reader, "s");
                                    record.RealNode = new HashRealNode();
                                    StoreHelper.SetHashRealNodeSelectFields(record.RealNode, reader, "r");

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

        public async Task<HashNode> QueryFirstByGreaterCode(Guid groupId, long code, params int[] status)
        {
            HashNode node = null;

            StringBuilder strStatusCondition = new StringBuilder();

            if (status != null && status.Length > 0)
            {
                strStatusCondition.Append("and status in(");
                for (var index = 0; index <= status.Length - 1; index++)
                {
                    strStatusCondition.Append(status[index]);
                    if (index != status.Length - 1)
                    {
                        strStatusCondition.Append(",");
                    }
                }
                strStatusCondition.Append(")");
            }

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
                    CommandText = string.Format(@"select {0},{1},{2},{3} from hashnode as n join hashgroup as g
                                                  on n.groupid=g.id
                                                  join hashgroupstrategy as s  on g.strategyid=s.id   
                                                  join hashrealnode r on r.id = n.realnodeid                                             
                                                  where n.groupid=@groupid and n.code>@code {4}
		                                          order by n.code limit 1",
                                                  StoreHelper.GetHashNodeSelectFields("n"),
                                                  StoreHelper.GetHashGroupSelectFields("g"),
                                                  StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                  StoreHelper.GetHashRealNodeSelectFields("r"),
                                                      strStatusCondition.ToString())
                })
                {

                    var parameter = new MySqlParameter("@groupid", MySqlDbType.Guid)
                    {
                        Value = groupId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@code", MySqlDbType.Int64)
                    {
                        Value = code
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    DbDataReader reader = null;

                    using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            node = new HashNode();
                            StoreHelper.SetHashNodeSelectFields(node, reader, "n");
                            node.Group = new HashGroup();
                            StoreHelper.SetHashGroupSelectFields(node.Group, reader, "g");
                            node.Group.Strategy = new HashGroupStrategy();
                            StoreHelper.SetHashGroupStrategySelectFields(node.Group.Strategy, reader, "s");
                            node.RealNode = new HashRealNode();
                            StoreHelper.SetHashRealNodeSelectFields(node.RealNode, reader, "r");
                        }

                        reader.Close();
                    }
                }
            });

            return node;
        }

        public HashNode QueryFirstByGreaterCodeSync(Guid groupId, long code, params int[] status)
        {
            HashNode node = null;

            StringBuilder strStatusCondition = new StringBuilder();

            if (status != null && status.Length > 0)
            {
                strStatusCondition.Append("and status in(");
                for (var index = 0; index <= status.Length - 1; index++)
                {
                    strStatusCondition.Append(status[index]);
                    if (index != status.Length - 1)
                    {
                        strStatusCondition.Append(",");
                    }
                }
                strStatusCondition.Append(")");
            }

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
                    CommandText = string.Format(@"select {0},{1},{2},{3} from hashnode as n join hashgroup as g
                                                  on n.groupid=g.id
                                                  join hashgroupstrategy as s 
                                                  on g.strategyid=s.id        
                                                  join hashrealnode r on (r.id = n.realnodeid)                                        
                                                  where n.groupid=@groupid and n.code>@code {4}
		                                          order by n.code limit 1",
                                                  StoreHelper.GetHashNodeSelectFields("n"),
                                                  StoreHelper.GetHashGroupSelectFields("g"),
                                                  StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                  StoreHelper.GetHashRealNodeSelectFields("r"),
                                                  strStatusCondition.ToString())
                })
                {

                    var parameter = new MySqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                    {
                        Value = groupId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@code", MySqlDbType.Int64)
                    {
                        Value = code
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    DbDataReader reader = null;

                    using (reader = commond.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            node = new HashNode();
                            StoreHelper.SetHashNodeSelectFields(node, reader, "n");
                            node.Group = new HashGroup();
                            StoreHelper.SetHashGroupSelectFields(node.Group, reader, "g");
                            node.Group.Strategy = new HashGroupStrategy();
                            StoreHelper.SetHashGroupStrategySelectFields(node.Group.Strategy, reader, "s");
                            node.RealNode = new HashRealNode();
                            StoreHelper.SetHashRealNodeSelectFields(node.RealNode, reader, "r");
                        }

                        reader.Close();
                    }
                }
            });

            return node;
        }

        public async Task<HashNode> QueryFirstByLessCode(Guid groupId, long code, params int[] status)
        {
            HashNode node = null;

            StringBuilder strStatusCondition = new StringBuilder();

            if (status != null && status.Length > 0)
            {
                strStatusCondition.Append("and status in(");
                for (var index = 0; index <= status.Length - 1; index++)
                {
                    strStatusCondition.Append(status[index]);
                    if (index != status.Length - 1)
                    {
                        strStatusCondition.Append(",");
                    }
                }
                strStatusCondition.Append(")");
            }

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
                    CommandText = string.Format(@"select  {0},{1},{2},{3} from hashnode as n join hashgroup as g
                                                  on n.groupid=g.id
                                                  join hashgroupstrategy as s 
                                                  on g.strategyid=s.id       
                                                  join hashrealnode r on r.id = n.realnodeid                                      
                                                  where n.groupid=@groupid and n.code<@code {4}
		                                          order by n.code desc limit 1",
                                                  StoreHelper.GetHashNodeSelectFields("n"),
                                                  StoreHelper.GetHashGroupSelectFields("g"),
                                                  StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                  StoreHelper.GetHashRealNodeSelectFields("r"),
                                                  strStatusCondition.ToString())
                })
                {

                    var parameter = new MySqlParameter("@groupid", MySqlDbType.Guid)
                    {
                        Value = groupId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@code", MySqlDbType.Int64)
                    {
                        Value = code
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    DbDataReader reader = null;

                    using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            node = new HashNode();
                            StoreHelper.SetHashNodeSelectFields(node, reader, "n");
                            node.Group = new HashGroup();
                            StoreHelper.SetHashGroupSelectFields(node.Group, reader, "g");
                            node.Group.Strategy = new HashGroupStrategy();
                            StoreHelper.SetHashGroupStrategySelectFields(node.Group.Strategy, reader, "s");
                            node.RealNode = new HashRealNode();
                            StoreHelper.SetHashRealNodeSelectFields(node.RealNode, reader, "r");
                        }

                        reader.Close();
                    }
                }
            });

            return node;
        }

        public async Task<HashNode> QueryFirstGreaterNode(Guid groupId, Guid nodeId)
        {
            var xNode=await QueryByGroup(groupId, nodeId);
            if (xNode==null)
            {
                return null;
            }
            HashNode node = null;

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
		                                                		                                               
		                                                select {0},{1},{2},{3} from hashnode as n join hashgroup as g
                                                        on n.groupid=g.id
                                                        join hashgroupstrategy as s 
                                                        on g.strategyid=s.id  
                                                        join hashrealnode r on r.id = n.realnodeid
                                                        where n.groupid=@groupid and n.code>@code
		                                                order by n.code limit 1 ",
                                                                            StoreHelper.GetHashNodeSelectFields("n"),
                                                                            StoreHelper.GetHashGroupSelectFields("g"),
                                                                            StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                                            StoreHelper.GetHashRealNodeSelectFields("r"))
            })
                {

                   var parameter = new MySqlParameter("@groupid", MySqlDbType.Guid)
                    {
                        Value = groupId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@code", MySqlDbType.Int64)
                    {
                        Value = xNode.Code
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    DbDataReader reader = null;

                    using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            node = new HashNode();
                            StoreHelper.SetHashNodeSelectFields(node, reader, "n");
                            node.Group = new HashGroup();
                            StoreHelper.SetHashGroupSelectFields(node.Group, reader, "g");
                            node.Group.Strategy = new HashGroupStrategy();
                            StoreHelper.SetHashGroupStrategySelectFields(node.Group.Strategy, reader, "s");

                            node.RealNode = new HashRealNode();
                            StoreHelper.SetHashRealNodeSelectFields(node.RealNode, reader, "r");
                        }

                        reader.Close();
                    }
                }
            });

            return node;
        }

        public async Task<HashNode> QueryFirstLessNode(Guid groupId, Guid nodeId)
        {
            var xNode = await QueryByGroup(groupId, nodeId);
            if (xNode == null)
            {
                return null;
            }

            HashNode node = null;

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
		                                                select {0},{1},{2},{3} from hashnode as n join hashgroup as g
                                                        on n.groupid=g.id
                                                        join hashgroupstrategy as s 
                                                        on g.strategyid=s.id   
                                                        join hashrealnode r on r.id = n.realnodeid                                            
                                                        where n.groupid=@groupid and n.code<@code
		                                                order by n.code desc limit 1", 
                                                    StoreHelper.GetHashNodeSelectFields("n"),
                                                    StoreHelper.GetHashGroupSelectFields("g"),
                                                    StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                    StoreHelper.GetHashRealNodeSelectFields("r")

                                                    )
                })
                {

                    var parameter = new MySqlParameter("@groupid", MySqlDbType.Guid)
                    {
                        Value = groupId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@code", MySqlDbType.Int64)
                    {
                        Value = xNode.Code
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    DbDataReader reader = null;

                    using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            node = new HashNode();
                            StoreHelper.SetHashNodeSelectFields(node, reader, "n");
                            node.Group = new HashGroup();
                            StoreHelper.SetHashGroupSelectFields(node.Group, reader, "g");
                            node.Group.Strategy = new HashGroupStrategy();
                            StoreHelper.SetHashGroupStrategySelectFields(node.Group.Strategy, reader, "s");
                            node.RealNode = new HashRealNode();
                            StoreHelper.SetHashRealNodeSelectFields(node.RealNode, reader, "r");
                        }

                        reader.Close();
                    }
                }
            });

            return node;
        }

        public async Task<List<HashNode>> QueryOrderByCode(Guid groupId, int skipNum, int takeNum)
        {
            List<HashNode> result = new List<HashNode>();

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
                                                  select {0},{1},{2},{3} from hashnode as n join hashgroup as g
                                                  on n.groupid=g.id
                                                  join hashgroupstrategy as s  on g.strategyid=s.id    
                                                  join hashrealnode as r on (r.id = n.realnodeid)
                                                  where n.sequence in
                                                    (
                                                    select t.sequence from
                                                    (
                                                        select sequence from hashnode
                                                        where groupid=@groupid
                                                     
                                                        order by code limit {4}, {5}                                                   
                                                    ) as t
                                                    )",
                                                  StoreHelper.GetHashNodeSelectFields("n"),
                                                  StoreHelper.GetHashGroupSelectFields("g"),
                                                  StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                  StoreHelper.GetHashRealNodeSelectFields("r"), skipNum, takeNum)
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
                            var record = new HashNode();
                            StoreHelper.SetHashNodeSelectFields(record, reader, "n");
                            record.Group = new HashGroup();
                            StoreHelper.SetHashGroupSelectFields(record.Group, reader, "g");
                            record.Group.Strategy = new HashGroupStrategy();
                            StoreHelper.SetHashGroupStrategySelectFields(record.Group.Strategy, reader, "s");
                            record.RealNode = new HashRealNode();
                            StoreHelper.SetHashRealNodeSelectFields(record.RealNode, reader, "r");

                            result.Add(record);
                        }

                        reader.Close();
                    }
                }
            });

            return result;
        }

        public async Task Update(Guid groupId, HashNode node)
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
                    CommandText = @" UPDATE hashnode
                                     SET groupid = @groupid,
                                        code = @code,
                                        modifytime = utc_timestamp()
                                     WHERE[id] = @id",
                    Transaction = sqlTran
                })
                {
                    MySqlParameter parameter;
    
                        parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                        {
                            Value = node.ID
                        };
                        commond.Parameters.Add(parameter);


                    parameter = new MySqlParameter("@realnodeid", MySqlDbType.Guid)
                    {
                        Value = node.RealNodeId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@groupid", MySqlDbType.Guid)
                    {
                        Value = node.GroupId
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new MySqlParameter("@code", MySqlDbType.Int64)
                    {
                        Value = node.Code
                    };
                    commond.Parameters.Add(parameter);


                    commond.Prepare();


                    await commond.ExecuteNonQueryAsync();

                }
            });
        }


        /// <summary>
        /// 更新HashNode 状态
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task UpdateStatus(Guid nodeId, int status)
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
                    CommandText = @"UPDATE hashnode
		                            SET status=@status
                                    ,modifytime=utc_timestamp()
		                            WHERE id=@nodeid",
                    Transaction = sqlTran
                })
                {
                    MySqlParameter parameter;

                    parameter = new MySqlParameter("@nodeid", MySqlDbType.Guid)
                    {
                        Value = nodeId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@status", MySqlDbType.Int32)
                    {
                        Value = status
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();


                    await commond.ExecuteNonQueryAsync();
                }
            });
        }

    }
}
