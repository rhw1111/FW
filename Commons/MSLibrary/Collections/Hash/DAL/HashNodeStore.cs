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

                    if (node.ID != Guid.Empty)
                    {
                        commond.CommandText = @"
                                                INSERT INTO [dbo].[HashNode](
                                                        [id]
                                                        ,[groupid]
                                                        ,[realnodeid]
                                                        ,[code]
                                                        ,[status]
                                                        ,[createtime]
                                                        ,[modifytime]
                                                        )
                                                VALUES(
                                                         @id
                                                        ,@groupid
                                                        ,@realnodeid
                                                        ,@code
                                                        ,@status
                                                        ,GETUTCDATE()
                                                        ,GETUTCDATE()
                                                )	
                                                SELECT @newid=@id	";

                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = node.ID
                        };
                        commond.Parameters.Add(parameter);
                    }
                    else
                    {

                        commond.CommandText = @" 
                                                INSERT INTO [dbo].[HashNode]
                                                (
                                                        [id]
                                                        ,[groupid]
                                                        ,[realnodeid]
                                                        ,[code]
                                                        ,[status]
                                                        ,[createtime]
                                                        ,[modifytime]
                                                )
                                                VALUES(
                                                        DEFAULT
                                                        ,@groupid
                                                        ,@realnodeid
                                                        ,@code
                                                        ,@status
                                                        ,GETUTCDATE()
                                                        ,GETUTCDATE()
                                                )
                                                SELECT @newid=[id] FROM [dbo].[HashNode] 
                                                WHERE [sequence]=SCOPE_IDENTITY()";


                        parameter = new SqlParameter("@newid", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };
                        commond.Parameters.Add(parameter);

                    }

                    parameter = new SqlParameter("@realnodeid", SqlDbType.UniqueIdentifier)
                    {
                        Value = node.RealNodeId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                    {
                        Value = node.GroupId
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@code", SqlDbType.BigInt)
                    {
                        Value = node.Code
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@status", SqlDbType.Int)
                    {
                        Value = node.Status
                    };
                    commond.Parameters.Add(parameter);


                    commond.Prepare();



                    await commond.ExecuteNonQueryAsync();



                    if (node.ID == Guid.Empty)
                    {
                        node.ID = (Guid)commond.Parameters["@newid"].Value;
                    }
                }
            });
        }

        public async Task DeleteByRelation(Guid groupId, Guid id)
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
                    CommandText = @"DELETE FROM [dbo].[HashNode]		  
		                            WHERE [id]=@id AND groupid=@groupid"
                })
                {

                    SqlParameter parameter;

                    parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                    {
                        Value = groupId
                    };
                    commond.Parameters.Add(parameter);

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

        public async Task<QueryResult<HashNode>> QueryByGroup(Guid groupId, int page, int pageSize)
        {
            QueryResult<HashNode> result = new QueryResult<HashNode>();

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
		                                          select @count= count(*) from HashNode as n join HashGroup as g
                                                  on n.groupid=g.id
                                                  join HashGroupStrategy as s  on g.strategyid=s.id 
                                                  join hashrealnode as r on (r.id = n.realnodeid)
                                                  where n.[groupid]=@groupid
		                                          
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
	
                                                  select {0},{1},{2},{3} from HashNode as n join HashGroup as g
                                                  on n.groupid=g.id
                                                  join HashGroupStrategy as s  on g.strategyid=s.id    
                                                  join hashrealnode as r on (r.id = n.realnodeid)
                                                  where n.[groupid]=@groupid
                                                  order by sequence
		                                          offset (@pagesize * (@currentpage - 1)) rows 
		                                          fetch next @pagesize rows only;",
                                                  StoreHelper.GetHashNodeSelectFields("n"),
                                                  StoreHelper.GetHashGroupSelectFields("g"),
                                                  StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                  StoreHelper.GetHashRealNodeSelectFields("r"))
                })
                {
                    var parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                    {
                        Value = groupId
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

                        reader.Close();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;
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

                await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, strConn, async (conn, transaction) =>
                {
                    SqlTransaction sqlTran = null;
                    if (transaction != null)
                    {
                        sqlTran = (SqlTransaction)transaction;
                    }
                    using (SqlCommand command = new SqlCommand()
                    {
                        Connection = (SqlConnection)conn,
                        CommandType = CommandType.Text,
                        CommandText = string.Format(@"
                                    select {0},{1},{2},{3} from HashNode as n join HashGroup as g
                                                                on n.groupid=g.id
                                                                join HashGroupStrategy as s 
                                                                on g.strategyid=s.id
                                                                join HashRealNode r on (r.id = n.realnodeid)
                                   where n.id = @id and groupid = @groupId",
                                       StoreHelper.GetHashNodeSelectFields("n"),
                                       StoreHelper.GetHashGroupSelectFields("g"),
                                       StoreHelper.GetHashGroupStrategySelectFields("s"),
                                       StoreHelper.GetHashRealNodeSelectFields("r")
                                       )
                    })
                    {
                        var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = nodeId
                        };
                        command.Parameters.Add(parameter);

                        parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                        {
                            Value = groupId
                        };


                        command.Parameters.Add(parameter);
                        command.Prepare();
                        SqlDataReader reader = null;

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
                strStatusCondition.Append("and [status] in(");
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
                    CommandText = string.Format(@"select top 1 {0},{1},{2},{3} from HashNode as n join HashGroup as g
                                                  on n.groupid=g.id
                                                  join HashGroupStrategy as s 
                                                  on g.strategyid=s.id  
                                                    join HashRealNode r on (r.id = n.realnodeid)
                                                  where n.[groupid]=@groupid {4}
		                                          order by n.code desc",
                                                  StoreHelper.GetHashNodeSelectFields("n"),
                                                  StoreHelper.GetHashGroupSelectFields("g"),
                                                  StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                  StoreHelper.GetHashRealNodeSelectFields("r"),
                                                  strStatusCondition.ToString())
                })
                {

                    var parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                    {
                        Value = groupId
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    SqlDataReader reader = null;

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
                    CommandText = string.Format(@"select top 1 {0},{1},{2},{3} from HashNode as n join HashGroup as g
                                                  on n.groupid=g.id
                                                  join HashGroupStrategy as s 
                                                  on g.strategyid=s.id       
                                                  join HashRealNode r on (r.id = n.realnodeid)
                                                  where n.[groupid]=@groupid
		                                          order by n.code desc",
                                                  StoreHelper.GetHashNodeSelectFields("n"),
                                                  StoreHelper.GetHashGroupSelectFields("g"),
                                                  StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                  StoreHelper.GetHashRealNodeSelectFields("r"))
                })
                {

                    var parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                    {
                        Value = groupId
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    SqlDataReader reader = null;

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
                strStatusCondition.Append("and [status] in(");
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
                    CommandText = string.Format(@"select top 1 {0},{1},{2},{3} from HashNode as n join HashGroup as g
                                                  on n.groupid=g.id
                                                  join HashGroupStrategy as s 
                                                  on g.strategyid=s.id 
                                                  join HashRealNode r on (r.id = n.realnodeid)
                                                  where n.[groupid]=@groupid {4}
		                                          order by n.code", StoreHelper.GetHashNodeSelectFields("n")
                                                  , StoreHelper.GetHashGroupSelectFields("g")
                                                  , StoreHelper.GetHashGroupStrategySelectFields("s")
                                                  , StoreHelper.GetHashRealNodeSelectFields("r")
                                                  , strStatusCondition.ToString())
                })
                {

                    var parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                    {
                        Value = groupId
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    SqlDataReader reader = null;

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
                    CommandText = string.Format(@"select top 1 {0},{1},{2},{3} from HashNode as n join HashGroup as g
                                                  on n.groupid=g.id
                                                  join HashGroupStrategy as s 
                                                  on g.strategyid=s.id         
                                                  join HashRealNode r on (r.id = n.realnodeid)                                       
                                                  where n.[groupid]=@groupid
		                                          order by n.code",
                                                  StoreHelper.GetHashNodeSelectFields("n"),
                                                  StoreHelper.GetHashGroupSelectFields("g"),
                                                  StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                  StoreHelper.GetHashRealNodeSelectFields("r")
                                                  )
                })
                {

                    var parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                    {
                        Value = groupId
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    SqlDataReader reader = null;

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
                strStatusCondition.Append("and [status] in(");
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


            DBTransactionHelper.SqlTransactionWork(DBTypes.SqlServer, true, false, strConn, (conn, transaction) =>
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
                   CommandText = string.Format(@"select top 1 {0},{1},{2},{3} from HashNode as n join HashGroup as g
                                                  on n.groupid=g.id
                                                  join HashGroupStrategy as s 
                                                  on g.strategyid=s.id    
                                                  join HashRealNode r on (r.id = n.realnodeid)                                            
                                                  where n.[groupid]=@groupid {4}
		                                          order by n.code",
                                                  StoreHelper.GetHashNodeSelectFields("n"),
                                                  StoreHelper.GetHashGroupSelectFields("g"),
                                                  StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                  StoreHelper.GetHashRealNodeSelectFields("r"),
                                                  strStatusCondition.ToString())
               })
               {

                   var parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                   {
                       Value = groupId
                   };
                   commond.Parameters.Add(parameter);

                   commond.Prepare();

                   SqlDataReader reader = null;

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
                        CommandText = string.Format(@"if @nodeid is null
                                                            begin
                                                                select top (@size) {0},{1},{2},{3} from HashNode as n join HashGroup as g
                                                                on n.groupid=g.id
                                                                join HashGroupStrategy as s 
                                                                on g.strategyid=s.id             
                                                                join HashRealNode r on (r.id = n.realnodeid)
                                                                where n.[groupid]=@groupid
                                                                order by n.id
                                                            end
                                                       else
                                                            begin
                                                                select top (@size) {0},{1},{2},{3} from HashNode as n join HashGroup as g
                                                                on n.groupid=g.id
                                                                join HashGroupStrategy as s 
                                                                on g.strategyid=s.id 
                                                                join HashRealNode r on (r.id = n.realnodeid)
                                                                where n.[groupid]=@groupid and n.id>@nodeid
                                                                order by n.id                                  
                                                            end",
                                                            StoreHelper.GetHashNodeSelectFields("n"),
                                                            StoreHelper.GetHashGroupSelectFields("g"),
                                                            StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                            StoreHelper.GetHashRealNodeSelectFields("r")
                                                            )
                    })
                    {
                        var parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                        {
                            Value = groupId
                        };
                        commond.Parameters.Add(parameter);

                        if (nodeId == null)
                        {
                            parameter = new SqlParameter("@nodeid", SqlDbType.UniqueIdentifier)
                            {
                                Value = DBNull.Value
                            };
                        }
                        else
                        {
                            parameter = new SqlParameter("@nodeid", SqlDbType.UniqueIdentifier)
                            {
                                Value = nodeId
                            };
                        }
                        commond.Parameters.Add(parameter);

                        parameter = new SqlParameter("@size", SqlDbType.Int)
                        {
                            Value = size
                        };
                        commond.Parameters.Add(parameter);


                        commond.Prepare();


                        SqlDataReader reader = null;


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
		                                          select @count= count(*) from HashNode as n join HashGroup as g
                                                  on n.groupid=g.id
                                                  join HashGroupStrategy as s  on g.strategyid=s.id 
                                                  join hashrealnode as r on (r.id = n.realnodeid)
                                                  where n.[groupid]=@groupid
                                                    and n.status = @status
		                                          
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
	
                                                  select {0},{1},{2},{3} from HashNode as n join HashGroup as g
                                                  on n.groupid=g.id
                                                  join HashGroupStrategy as s  on g.strategyid=s.id    
                                                  join hashrealnode as r on (r.id = n.realnodeid)
                                                  where n.[groupid]=@groupid
                                                    and n.status = @status
                                                  order by nsequence
		                                          offset (@pagesize * (@currentpage - 1)) rows 
		                                          fetch next @pagesize rows only;",
                                                  StoreHelper.GetHashNodeSelectFields("n"),
                                                  StoreHelper.GetHashGroupSelectFields("g"),
                                                  StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                  StoreHelper.GetHashRealNodeSelectFields("r"))
                })
                {
                    var parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                    {
                        Value = groupId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@status", SqlDbType.Int)
                    {
                        Value = status
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

                        reader.Close();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;
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
                strStatusCondition.Append("and [status] in(");
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
                    CommandText = string.Format(@"select top 1 {0},{1},{2},{3} from HashNode as n join HashGroup as g
                                                  on n.groupid=g.id
                                                  join HashGroupStrategy as s  on g.strategyid=s.id   
                                                  join HashRealNode r on (r.id = n.realnodeid)                                             
                                                  where n.[groupid]=@groupid and n.code>@code {4}
		                                          order by n.code",
                                                  StoreHelper.GetHashNodeSelectFields("n"),
                                                  StoreHelper.GetHashGroupSelectFields("g"),
                                                  StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                  StoreHelper.GetHashRealNodeSelectFields("r"),
                                                      strStatusCondition.ToString())
                })
                {

                    var parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                    {
                        Value = groupId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@code", SqlDbType.BigInt)
                    {
                        Value = code
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    SqlDataReader reader = null;

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
                strStatusCondition.Append("and [status] in(");
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


            DBTransactionHelper.SqlTransactionWork(DBTypes.SqlServer, true, false, strConn, (conn, transaction) =>
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
                    CommandText = string.Format(@"select top 1 {0},{1},{2},{3} from HashNode as n join HashGroup as g
                                                  on n.groupid=g.id
                                                  join HashGroupStrategy as s 
                                                  on g.strategyid=s.id        
                                                  join HashRealNode r on (r.id = n.realnodeid)                                        
                                                  where n.[groupid]=@groupid and n.code>@code {4}
		                                          order by n.code",
                                                  StoreHelper.GetHashNodeSelectFields("n"),
                                                  StoreHelper.GetHashGroupSelectFields("g"),
                                                  StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                  StoreHelper.GetHashRealNodeSelectFields("r"),
                                                  strStatusCondition.ToString())
                })
                {

                    var parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                    {
                        Value = groupId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@code", SqlDbType.BigInt)
                    {
                        Value = code
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    SqlDataReader reader = null;

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
                strStatusCondition.Append("and [status] in(");
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
                    CommandText = string.Format(@"select top 1 {0},{1},{2},{3} from HashNode as n join HashGroup as g
                                                  on n.groupid=g.id
                                                  join HashGroupStrategy as s 
                                                  on g.strategyid=s.id       
                                                  join HashRealNode r on (r.id = n.realnodeid)                                         
                                                  where n.[groupid]=@groupid and n.code<@code {4}
		                                          order by n.code desc",
                                                  StoreHelper.GetHashNodeSelectFields("n"),
                                                  StoreHelper.GetHashGroupSelectFields("g"),
                                                  StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                  StoreHelper.GetHashRealNodeSelectFields("r"),
                                                  strStatusCondition.ToString())
                })
                {

                    var parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                    {
                        Value = groupId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@code", SqlDbType.BigInt)
                    {
                        Value = code
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    SqlDataReader reader = null;

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
            HashNode node = null;

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
                    CommandText = string.Format(@"declare @code bigint=null
                                                  select @code=code from HashNode where [id]= @nodeid
                                                  if @code is not null
	                                                begin 
		                                                select top 1 {0},{1},{2},{3} from HashNode as n join HashGroup as g
                                                        on n.groupid=g.id
                                                        join HashGroupStrategy as s 
                                                        on g.strategyid=s.id  
                                                        join HashRealNode r on (r.id = n.realnodeid)
                                                        where n.[groupid]=@groupid and n.code>@code
		                                                order by n.code 
	                                                end
                                                  else
	                                                begin
		                                                select {0},{1},{2},{3} from HashNode as n join HashGroup as g
                                                        on n.groupid=g.id
                                                        join HashGroupStrategy as s 
                                                        on g.strategyid=s.id
                                                        join HashRealNode r on (r.id = n.realnodeid)
		                                                where 1=2 
	                                                end",
                                                    StoreHelper.GetHashNodeSelectFields("n"),
                                                    StoreHelper.GetHashGroupSelectFields("g"),
                                                    StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                    StoreHelper.GetHashRealNodeSelectFields("r"))
                })
                {

                    var parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                    {
                        Value = groupId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@nodeid", SqlDbType.UniqueIdentifier)
                    {
                        Value = nodeId
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    SqlDataReader reader = null;

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
            HashNode node = null;

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
                    CommandText = string.Format(@"declare @code bigint=null
                                                  select @code=code from HashNode where [id]= @nodeid
                                                  if @code is not null
	                                                begin 
		                                                select top 1 {0},{1},{2},{3} from HashNode as n join HashGroup as g
                                                        on n.groupid=g.id
                                                        join HashGroupStrategy as s 
                                                        on g.strategyid=s.id   
                                                        join HashRealNode r on (r.id = n.realnodeid)                                             
                                                        where n.[groupid]=@groupid and n.code<@code
		                                                order by n.code desc
	                                                end
                                                  else
	                                                begin
		                                                select {0},{1},{2},{3} from HashNode as n join HashGroup as g
                                                        on n.groupid=g.id
                                                        join HashGroupStrategy as s 
                                                        on g.strategyid=s.id
                          join HashRealNode r on (r.id = n.realnodeid)
		                                                where 1=2 
	                                                end", StoreHelper.GetHashNodeSelectFields("n"),
                                                    StoreHelper.GetHashGroupSelectFields("g"),
                                                    StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                    StoreHelper.GetHashRealNodeSelectFields("r")

                                                    )
                })
                {

                    var parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                    {
                        Value = groupId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@nodeid", SqlDbType.UniqueIdentifier)
                    {
                        Value = nodeId
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    SqlDataReader reader = null;

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
                    CommandText = string.Format(@"
                                                  select {0},{1},{2},{3} from HashNode as n join HashGroup as g
                                                  on n.groupid=g.id
                                                  join HashGroupStrategy as s  on g.strategyid=s.id    
                                                  join hashrealnode as r on (r.id = n.realnodeid)
                                                  where n.[groupid]=@groupid
                                                  order by ncode
		                                          offset (@skipNum) rows 
		                                          fetch next @takeNum rows only;",
                                                  StoreHelper.GetHashNodeSelectFields("n"),
                                                  StoreHelper.GetHashGroupSelectFields("g"),
                                                  StoreHelper.GetHashGroupStrategySelectFields("s"),
                                                  StoreHelper.GetHashRealNodeSelectFields("r"))
                })
                {
                    var parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                    {
                        Value = groupId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@skipNum", SqlDbType.Int)
                    {
                        Value = skipNum
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@takeNum", SqlDbType.Int)
                    {
                        Value = takeNum
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();


                    SqlDataReader reader = null;

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
                    CommandText = @" UPDATE [dbo].[HashNode] 
                                     SET[groupid] = @groupid,
                                        [code] = @code,
                                        [modifytime] = GETUTCDATE() 
                                     WHERE[id] = @id",
                    Transaction = sqlTran
                })
                {
                    SqlParameter parameter;
                    if (node.ID != Guid.Empty)
                    {
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = node.ID
                        };
                        commond.Parameters.Add(parameter);
                    }
                    else
                    {
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = DBNull.Value
                        };
                        commond.Parameters.Add(parameter);

                    }

                    parameter = new SqlParameter("@realnodeid", SqlDbType.UniqueIdentifier)
                    {
                        Value = node.RealNodeId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                    {
                        Value = node.GroupId
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@code", SqlDbType.BigInt)
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
                    CommandText = @"UPDATE [dbo].[HashNode]
		                            SET [status]=@status
                                    ,[modifytime]=GETUTCDATE()
		                            WHERE [id]=@nodeid",
                    Transaction = sqlTran
                })
                {
                    SqlParameter parameter;

                    parameter = new SqlParameter("@nodeid", SqlDbType.UniqueIdentifier)
                    {
                        Value = nodeId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@status", SqlDbType.Int)
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
