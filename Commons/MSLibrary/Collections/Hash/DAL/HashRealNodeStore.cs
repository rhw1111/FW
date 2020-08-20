using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DAL;
using MSLibrary.DI;
using MSLibrary.Transaction;

namespace MSLibrary.Collections.Hash.DAL
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
                    Transaction = sqlTran,
                })
                {
                    SqlParameter parameter;
                    if (node.ID == Guid.Empty)
                    {
                        command.CommandText = @"
                                                    INSERT INTO [HASHREALNODE]
                                                      (
	                                                     [ID]
                                                        ,[Name]
	                                                    ,[GROUPID]
	                                                    ,[NODEKEY]
                                                        ,[CREATETIME]
                                                        ,[MODIFYTIME]
                                                      )VALUES(
	                                                      DEFAULT
                                                          ,@name
	                                                      ,@groupid	  
	                                                      ,@nodekey
	                                                      ,GETUTCDATE()
	                                                      ,GETUTCDATE()
                                                      );
                                                     select @newid =[id] from HASHREALNODE where [sequence] = SCOPE_IDENTITY()";
                        parameter = new SqlParameter("@newid", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        command.CommandText = @"
                                                INSERT INTO [HASHREALNODE]
                                                      (
	                                                     [ID]
                                                        ,[Name]
	                                                    ,[GROUPID]
	                                                    ,[NODEKEY]
                                                        ,[CREATETIME]
                                                        ,[MODIFYTIME]
                                                      )VALUES(
	                                                      @id
                                                          ,@name
	                                                      ,@groupid	  
	                                                      ,@nodekey
	                                                      ,GETUTCDATE()
	                                                      ,GETUTCDATE()
                                                      )";

                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = node.ID
                        };
                        command.Parameters.Add(parameter);
                    }

                    parameter = new SqlParameter("@name", SqlDbType.VarChar, 100)
                    {
                        Value = node.Name
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                    {
                        Value = node.GroupId
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@nodekey", SqlDbType.VarChar, 4000)
                    {
                        Value = node.NodeKey
                    };
                    command.Parameters.Add(parameter);

                    command.Prepare();

                    await command.ExecuteNonQueryAsync();


                    //如果用户未赋值ID则创建成功后返回ID
                    if (node.ID == Guid.Empty)
                    {
                        node.ID = (Guid)command.Parameters["@newid"].Value;
                    };
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
                using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = @"
                                   delete HashRealNode      
                                   where n.id = @id and groupid = @groupId"
                })
                {
                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
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
                                                                select top (@size) {0},{1},{2} from HashRealNode as n join HashGroup as g
                                                                on n.groupid=g.id
                                                                join HashGroupStrategy as s 
                                                                on g.strategyid=s.id                                                
                                                                where n.[groupid]=@groupid
                                                                order by n.id
                                                            end
                                                       else
                                                            begin
                                                                select top (@size) {0},{1},{2} from HashRealNode as n join HashGroup as g
                                                                on n.groupid=g.id
                                                                join HashGroupStrategy as s 
                                                                on g.strategyid=s.id                                                
                                                                where n.[groupid]=@groupid and n.id>@nodeid
                                                                order by n.id                                  
                                                            end", StoreHelper.GetHashRealNodeSelectFields("n"), StoreHelper.GetHashGroupSelectFields("g"), StoreHelper.GetHashGroupStrategySelectFields("s"))
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
                                    select {0},{1},{2} from HashRealNode as n join HashGroup as g
                                                                on n.groupid=g.id
                                                                join HashGroupStrategy as s 
                                                                on g.strategyid=s.id        
                                   where n.id = @id and groupid = @groupId",
                                   StoreHelper.GetHashRealNodeSelectFields("n"),
                                   StoreHelper.GetHashGroupSelectFields("g"),
                                   StoreHelper.GetHashGroupStrategySelectFields("s"))
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
		                                          select @count= count(*) from HashRealNode as n join HashGroup as g
                                                  on n.groupid=g.id
                                                  join HashGroupStrategy as s 
                                                  on g.strategyid=s.id 
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
	
                                                  select {0},{1},{2} from HashRealNode as n join HashGroup as g
                                                  on n.groupid=g.id            
                                                  join HashGroupStrategy as s 
                                                  on g.strategyid=s.id           
                                                  where n.[groupid]=@groupid
                                                  order by n.sequence
		                                          offset (@pagesize * (@currentpage - 1)) rows 
		                                          fetch next @pagesize rows only;", StoreHelper.GetHashRealNodeSelectFields("n"), StoreHelper.GetHashGroupSelectFields("g"), StoreHelper.GetHashGroupStrategySelectFields("s"))
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
                            var record = new HashRealNode();
                            StoreHelper.SetHashRealNodeSelectFields(record, reader, "n");
                            record.Group = new HashGroup();
                            StoreHelper.SetHashGroupSelectFields(record.Group, reader, "g");
                            record.Group.Strategy = new HashGroupStrategy();
                            StoreHelper.SetHashGroupStrategySelectFields(record.Group.Strategy, reader, "s");
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

        public async Task<List<HashRealNode>> QueryHashRealNodeByCreateTime(Guid groupId, int skipNum, int takeNum)
        {

            List<HashRealNode> result = new List<HashRealNode>();

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
                                                  select {0},{1},{2} from HashRealNode as n join HashGroup as g
                                                  on n.groupid=g.id            
                                                  join HashGroupStrategy as s 
                                                  on g.strategyid=s.id           
                                                  where n.[groupid]=@groupid
                                                  order by n.sequence
		                                           offset (@skipNum) rows 
		                                          fetch next (@takeNum) rows only;", StoreHelper.GetHashRealNodeSelectFields("n"), StoreHelper.GetHashGroupSelectFields("g"), StoreHelper.GetHashGroupStrategySelectFields("s"))
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
                    Transaction = sqlTran,
                })
                {
                    SqlParameter parameter;

                    command.CommandText = @"
                                             update [HASHREALNODE]
                                               
                                             set nodekey = @nodekey
                                               ,name = @name
                                               ,modifytime = GETUTCDATE()

                                             where id = @id and groupid = @groupId";

                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = node.ID
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                    {
                        Value = node.GroupId
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@name", SqlDbType.VarChar, 100)
                    {
                        Value = node.Name
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@nodekey", SqlDbType.VarChar, 4000)
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
