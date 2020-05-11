using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using MSLibrary.Transaction;
using MSLibrary.DI;

namespace MSLibrary.Security.BusinessSecurityRule.DAL
{
    /// <summary>
    /// 业务动作数据操作
    /// </summary>
    [Injection(InterfaceType = typeof(IBusinessActionStore), Scope = InjectionScope.Singleton)]
    public class BusinessActionStore : IBusinessActionStore
    {
        private IBusinessSecurityRuleConnectionFactory _businessSecurityRuleConnectionFactory;

        public BusinessActionStore(IBusinessSecurityRuleConnectionFactory businessSecurityRuleConnectionFactory)
        {
            _businessSecurityRuleConnectionFactory = businessSecurityRuleConnectionFactory;
        }

        public async Task Add(BusinessAction action)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _businessSecurityRuleConnectionFactory.CreateAllForBusinessSecurityRule(), async (conn,transaction) =>
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
                    if (action.ID == Guid.Empty)
                    {
                        commond.CommandText = @"insert into BusinessAction WITH (SNAPSHOT)([id],[name],[rule],[originalparametersfiltertype],[errorreplacetext],[createtime],[modifytime])
                                    values(default,@name,@rule,@originalparametersfiltertype,@errorreplacetext,getutcdate(),getutcdate());
                                    select @newid=[id] from BusinessAction where [sequence]=SCOPE_IDENTITY()";
                    }
                    else
                    {
                        commond.CommandText = @"insert into BusinessAction WITH (SNAPSHOT)([id],[name],[rule],[originalparametersfiltertype],[errorreplacetext],[createtime],[modifytime])
                                    values(@id,@name,@rule,@originalparametersfiltertype,@errorreplacetext,getutcdate(),getutcdate())";
                    }

                    SqlParameter parameter;
                    if (action.ID != Guid.Empty)
                    {
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = action.ID
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


                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 150)
                    {
                        Value = action.Name
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@rule", SqlDbType.NVarChar, action.Rule.Length)
                    {
                        Value = action.Rule
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@originalparametersfiltertype", SqlDbType.NVarChar, 150)
                    {
                        Value = action.OriginalParametersFilterType
                    };
                    commond.Parameters.Add(parameter);

                    if (action.ErrorReplaceText == null)
                    {
                        parameter = new SqlParameter("@errorreplacetext", SqlDbType.NVarChar, 1500)
                        {
                            Value = DBNull.Value
                        };
                    }
                    else
                    {
                        parameter = new SqlParameter("@errorreplacetext", SqlDbType.NVarChar, 1500)
                        {
                            Value = action.ErrorReplaceText
                        };
                    }
                    commond.Parameters.Add(parameter);

                    commond.Prepare();


                    await commond.ExecuteNonQueryAsync();



                    if (action.ID == Guid.Empty)
                    {
                        action.ID = (Guid)commond.Parameters["@newid"].Value;
                    }
                }
            });
        }

        public async Task AddGroupRelation(Guid actionId, Guid groupId)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _businessSecurityRuleConnectionFactory.CreateAllForBusinessSecurityRule(), async (conn,transaction) =>
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
                    Transaction=sqlTran,
                    CommandText = @"insert into BusinessActionGroupRelation WITH (SNAPSHOT)([groupid],[actionid])
                                    values(@groupid,@actionid)"
                })
                {


                    SqlParameter parameter;

                        parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                        {
                            Value = groupId
                        };
                        commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@actionid", SqlDbType.UniqueIdentifier)
                    {
                        Value = actionId
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    int reply = 3;
                    while (true)
                    {
                        try
                        {
                            await commond.ExecuteNonQueryAsync();
                            break;
                        }
                        catch (SqlException ex)
                        {

                            if (reply > 0 && (ex.Number == 41302 || ex.Number == 41305 || ex.Number == 41325 || ex.Number == 41301 || ex.Number == 1205))
                            {
                                reply--;
                                System.Threading.Thread.Sleep(1);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }

                }
            });
        }

        public async Task Delete(Guid actionId)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false,true, _businessSecurityRuleConnectionFactory.CreateAllForBusinessSecurityRule(), async (conn,transaction) =>
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
                    CommandText = @"declare @delnum int
                                    set @delnum=1000
                                    while @delnum=1000
                                    begin
                                        set @delnum=0
                                        select top 1000 @delnum=@delnum+1 from [dbo].[BusinessActionGroupRelation] WITH (SNAPSHOT)
                                        where [actionid]=@id
                                        delete from [dbo].[BusinessActionGroupRelation] WITH (SNAPSHOT) from [dbo].[BusinessActionGroupRelation] WITH (SNAPSHOT) join 
                                        (
                                            select top 1000 [groupid],[actionid] from [dbo].[BusinessActionGroupRelation] WITH (SNAPSHOT)
                                            where [actionid]=@id
                                        ) as d
                                        on [dbo].[BusinessActionGroupRelation].actionid=d.actionid and [dbo].[BusinessActionGroupRelation].groupid=d.groupid
   
                                    end

                                    delete from [dbo].[BusinessAction] where id=@id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = actionId
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();


                    await commond.ExecuteNonQueryAsync();
                }
            });
        }

        public async Task DeleteGroupRelation(Guid actionId, Guid groupId)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _businessSecurityRuleConnectionFactory.CreateAllForBusinessSecurityRule(), async (conn,transaction) =>
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
                    CommandText = @"delete from BusinessActionGroupRelation WITH (SNAPSHOT) where [groupid]=@groupid and [actionid]=@actionid"
                })
                {


                    SqlParameter parameter;

                    parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                    {
                        Value = groupId
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@actionid", SqlDbType.UniqueIdentifier)
                    {
                        Value = actionId
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    await commond.ExecuteNonQueryAsync();

                }
            });
        }

        public async Task QueryByGroup(Guid groupId, Func<BusinessAction, Task> callback)
        {
            List<BusinessAction> actionList = new List<BusinessAction>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _businessSecurityRuleConnectionFactory.CreateReadForBusinessSecurityRule(), async (conn,transaction) =>
            {
                Int64? sequence = null;
                int pageSize = 500;

                while (true)
                {
                    actionList.Clear();

                    SqlTransaction sqlTran = null;
                    if (transaction != null)
                    {
                        sqlTran = (SqlTransaction)transaction;
                    }

                    using (SqlCommand commond = new SqlCommand()
                    {
                        Connection = (SqlConnection)conn,
                        CommandType = CommandType.Text,
                         Transaction=sqlTran
                    })
                    {
                        if (!sequence.HasValue)
                        {
                            commond.CommandText = string.Format(@"select top (@pagesize) {0} from BusinessAction WITH (SNAPSHOT) as act join BusinessActionGroupRelation WITH (SNAPSHOT) as relation on act.id=relation.[actionid] where relation.[groupid]=@groupid order by act.[sequence]", StoreHelper.GetBusinessActionSelectFields("act"));
                        }
                        else
                        {
                            commond.CommandText = string.Format(@"select top (@pagesize) {0} from BusinessAction WITH (SNAPSHOT) as act join BusinessActionGroupRelation WITH (SNAPSHOT) as relation on act.id=relation.[actionid] where relation.[groupid]=@groupid and act.[sequence]>@sequence order by act.[sequence]", StoreHelper.GetBusinessActionSelectFields("act"));
                        }

                        var parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                        {
                            Value = groupId
                        };
                        commond.Parameters.Add(parameter);

                        parameter = new SqlParameter("@pagesize", SqlDbType.Int)
                        {
                            Value = pageSize
                        };
                        commond.Parameters.Add(parameter);

                        if (sequence.HasValue)
                        {
                            parameter = new SqlParameter("@sequence", SqlDbType.BigInt)
                            {
                                Value = sequence
                            };
                            commond.Parameters.Add(parameter);
                        }

                        commond.Prepare();

                        using (var reader = await commond.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var action= new BusinessAction();
                                StoreHelper.SetBusinessActionSelectFields(action, reader, "act");
                                sequence = (Int64)reader["actsequence"];
                                actionList.Add(action);
                            }

                            reader.Close();
                        }


                    }

                    foreach (var actionItem in actionList)
                    {
                        await callback(actionItem);
                    }

                    if (actionList.Count != pageSize)
                    {
                        break;
                    }

                }

            });

        }

        public async Task<QueryResult<BusinessAction>> QueryByGroup(Guid groupId, int page, int pageSize)
        {
            QueryResult<BusinessAction> result = new QueryResult<BusinessAction>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _businessSecurityRuleConnectionFactory.CreateReadForBusinessSecurityRule(), async (conn,transaction) =>
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
                     Transaction=sqlTran,
                    CommandText = string.Format(@"set @currentpage=@page
		                           select @count= count(*) from BusinessAction WITH (SNAPSHOT) as act join BusinessActionGroupRelation WITH (SNAPSHOT) as relation on act.id=relation.[actionid] where relation.[groupid]=@groupid 
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
	
                                    select {0} from BusinessAction WITH (SNAPSHOT) as act join BusinessActionGroupRelation WITH (SNAPSHOT) as relation on act.id=relation.[actionid] where relation.[groupid]=@groupid 
                                    order by [actsequence]
		                            offset (@pagesize * (@currentpage - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetBusinessActionSelectFields("act"))
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

                    parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                    {
                        Value = groupId
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

                    using (var reader = await commond.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var action = new BusinessAction();
                            StoreHelper.SetBusinessActionSelectFields(action, reader, "act");
                            result.Results.Add(action);
                        }

                        reader.Close();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;

                    }
                }
            });

            return result;
        }

        public async Task<BusinessAction> QueryById(Guid id)
        {
            BusinessAction action = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _businessSecurityRuleConnectionFactory.CreateReadForBusinessSecurityRule(), async (conn,transaction) =>
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
                    Transaction=sqlTran,
                    CommandText = string.Format(@"select {0} from BusinessAction WITH (SNAPSHOT) where [id]=@id", StoreHelper.GetBusinessActionSelectFields(string.Empty))
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    using (var reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            action = new BusinessAction();
                            StoreHelper.SetBusinessActionSelectFields(action, reader, string.Empty);
                        }

                        reader.Close();
                    }
                }
            });

            return action;
        }

        public async Task<BusinessAction> QueryByName(string name)
        {
            BusinessAction action = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _businessSecurityRuleConnectionFactory.CreateReadForBusinessSecurityRule(), async (conn,transaction) =>
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
                    Transaction=sqlTran,
                    CommandText = string.Format(@"select {0} from BusinessAction WITH (SNAPSHOT) where [name]=@name", StoreHelper.GetBusinessActionSelectFields(string.Empty))
                })
                {

                    var parameter = new SqlParameter("@name", SqlDbType.NVarChar,150)
                    {
                        Value = name
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    using (var reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            action = new BusinessAction();
                            StoreHelper.SetBusinessActionSelectFields(action, reader, string.Empty);
                        }

                        reader.Close();
                    }
                }
            });

            return action;
        }

        public async Task<QueryResult<BusinessAction>> QueryByName(string name, int page, int pageSize)
        {
            QueryResult<BusinessAction> result = new QueryResult<BusinessAction>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _businessSecurityRuleConnectionFactory.CreateReadForBusinessSecurityRule(), async (conn,transaction) =>
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
                     Transaction=sqlTran,
                    CommandText = string.Format(@"set @currentpage=@page
		                           select @count= count(*) from BusinessAction WITH (SNAPSHOT) where [name] like @name 
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
	
                                    select {0} from BusinessAction WITH (SNAPSHOT) where [name] like @name  
                                    order by [actsequence]
		                            offset (@pagesize * (@currentpage - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetBusinessActionSelectFields(string.Empty))
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

                    parameter = new SqlParameter("@name", SqlDbType.NVarChar,150)
                    {
                        Value = $"{name.ToSqlLike()}%"
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

                    using (var reader= await commond.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var action = new BusinessAction();
                            StoreHelper.SetBusinessActionSelectFields(action, reader, string.Empty);
                            result.Results.Add(action);
                        }

                        reader.Close();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;

                    }
                }
            });

            return result;
        }

        public async Task Update(BusinessAction action)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _businessSecurityRuleConnectionFactory.CreateReadForBusinessSecurityRule(), async (conn,transaction) =>
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
                    CommandText = @"update BusinessAction WITH (SNAPSHOT) set [name]=@name,[rule]=@rule,[originalparametersfiltertype]=@originalparametersfiltertype,[errorreplacetext]=@errorreplacetext,[modifytime]=getutcdate()
                                    where [id]=@id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = action.ID
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 150)
                    {
                        Value = action.Name
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@rule", SqlDbType.NVarChar, action.Rule.Length)
                    {
                        Value = action.Rule
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@originalparametersfiltertype", SqlDbType.NVarChar, 150)
                    {
                        Value = action.OriginalParametersFilterType
                    };
                    commond.Parameters.Add(parameter);

                    if (action.ErrorReplaceText == null)
                    {
                        parameter = new SqlParameter("@errorreplacetext", SqlDbType.NVarChar, 1500)
                        {
                            Value = DBNull.Value
                        };
                    }
                    else
                    {
                        parameter = new SqlParameter("@errorreplacetext", SqlDbType.NVarChar, 1500)
                        {
                            Value = action.ErrorReplaceText
                        };
                    }
                    commond.Parameters.Add(parameter);

                    commond.Prepare();


                    await commond.ExecuteNonQueryAsync();
                }
            });
        }

        public async Task<QueryResult<BusinessAction>> QueryByNullRelationGroup(Guid groupId, int page, int pageSize)
        {
            QueryResult<BusinessAction> result = new QueryResult<BusinessAction>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _businessSecurityRuleConnectionFactory.CreateReadForBusinessSecurityRule(), async (conn,transaction) =>
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
                     Transaction=sqlTran,
                    CommandText = string.Format(@"set @currentpage=@page
		                           select @count= count(*) from [dbo].[BusinessAction] WITH (SNAPSHOT) as act
                                   left outer join
                                   (
                                        select act.id as existid from [dbo].[BusinessAction] WITH (SNAPSHOT) as act join [dbo].[BusinessActionGroupRelation] WITH (SNAPSHOT) as relation
                                        on act.id=relation.actionid 
                                        where relation.groupid=@groupid
                                    )as t
                                    on act.id=t.existid
                                    where t.existid is null

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
	
                                    select {0} from [dbo].[BusinessAction] as act
                                   left outer join
                                   (
                                        select act.id as existid from [dbo].[BusinessAction] WITH (SNAPSHOT) as act join [dbo].[BusinessActionGroupRelation] WITH (SNAPSHOT)  as relation
                                        on act.id=relation.actionid 
                                        where relation.groupid=@groupid
                                    )as t
                                    on act.id=t.existid
                                    where t.existid is null  
                                    order by [actsequence]
		                            offset (@pagesize * (@currentpage - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetBusinessActionSelectFields("act"))
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

                    parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                    {
                        Value = groupId
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

                    using (var reader= await commond.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var action = new BusinessAction();
                            StoreHelper.SetBusinessActionSelectFields(action, reader, "act");
                            result.Results.Add(action);
                        }

                        reader.Close();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;

                    }
                }
            });

            return result;
        }
    }
}
