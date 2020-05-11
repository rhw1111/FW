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
    /// 业务动作组数据操作
    /// </summary>
    [Injection(InterfaceType = typeof(IBusinessActionGroupStore), Scope = InjectionScope.Singleton)]
    public class BusinessActionGroupStore : IBusinessActionGroupStore
    {
        private IBusinessSecurityRuleConnectionFactory _businessSecurityRuleConnectionFactory;

        public BusinessActionGroupStore(IBusinessSecurityRuleConnectionFactory businessSecurityRuleConnectionFactory)
        {
            _businessSecurityRuleConnectionFactory = businessSecurityRuleConnectionFactory;
        }
        public async Task Add(BusinessActionGroup group)
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
                     Transaction=sqlTran
                })
                {
                    if (group.ID == Guid.Empty)
                    {
                        commond.CommandText = @"insert into BusinessActionGroup WITH (SNAPSHOT) ([id],[name],[createtime],[modifytime])
                                    values(default,@name,getutcdate(),getutcdate());
                                    select @newid=[id] from BusinessActionGroup WITH (SNAPSHOT) where [sequence]=SCOPE_IDENTITY()";
                    }
                    else
                    {
                        commond.CommandText = @"insert into BusinessActionGroup WITH (SNAPSHOT) ([id],[name],[createtime],[modifytime])
                                    values(@id,@name,getutcdate(),getutcdate())";
                    }

                    SqlParameter parameter;
                    if (group.ID != Guid.Empty)
                    {
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = group.ID
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
                        Value = group.Name
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();


                    await commond.ExecuteNonQueryAsync();
                     

                    if (group.ID == Guid.Empty)
                    {
                        group.ID = (Guid)commond.Parameters["@newid"].Value;
                    }
                }
            });
        }

        public async Task Delete(Guid groupId)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, true, _businessSecurityRuleConnectionFactory.CreateAllForBusinessSecurityRule(), async (conn,transaction) =>
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
                    CommandText = @"declare @delnum int
                                    set @delnum=1000
                                    while @delnum=1000
                                    begin
                                        set @delnum=0
                                        select top 1000 @delnum=@delnum+1 from [dbo].[BusinessActionGroupRelation] WITH (SNAPSHOT)
                                        where [groupid]=@id
                                        delete from [dbo].[BusinessActionGroupRelation] WITH (SNAPSHOT) from [dbo].[BusinessActionGroupRelation] WITH (SNAPSHOT) join 
                                        (
                                            select top 1000 [groupid],[actionid] from [dbo].[BusinessActionGroupRelation] WITH (SNAPSHOT)
                                            where [groupid]=@id
                                        ) as d
                                        on [dbo].[BusinessActionGroupRelation].actionid=d.actionid and [dbo].[BusinessActionGroupRelation].groupid=d.groupid
   
                                    end

                                    delete from [dbo].[BusinessActionGroup] where id=@id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = groupId
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();


                            await commond.ExecuteNonQueryAsync();
    

                    
                }
            });
        }

        public async Task<QueryResult<BusinessActionGroup>> QueryByAction(Guid actionId, int page, int pageSize)
        {
            QueryResult<BusinessActionGroup> result = new QueryResult<BusinessActionGroup>();

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
		                           select @count= count(*) from BusinessActionGroup WITH (SNAPSHOT) as gro join BusinessActionGroupRelation WITH (SNAPSHOT) as relation on gro.id=relation.[groupid] where relation.[actionid]=@actionid 
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
	
                                    select {0} from BusinessActionGroup WITH (SNAPSHOT) as gro join BusinessActionGroupRelation WITH (SNAPSHOT) as relation on gro.id=relation.[groupid] where relation.[actionid]=@actionid  
                                    order by [grosequence]
		                            offset (@pagesize * (@currentpage - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetBusinessActionGroupSelectFields("gro"))
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

                    parameter = new SqlParameter("@actionid", SqlDbType.UniqueIdentifier)
                    {
                        Value = actionId
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
                            var group = new BusinessActionGroup();
                            StoreHelper.SetBusinessActionGroupSelectFields(group, reader, "gro");
                            result.Results.Add(group);
                        }

                        reader.Close();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;

                    }
                }
            });

            return result;
        }

        public async Task<BusinessActionGroup> QueryById(Guid id)
        {
            BusinessActionGroup group = null;

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
                    CommandText = string.Format(@"select {0} from BusinessActionGroup WITH (SNAPSHOT) where [id]=@id", StoreHelper.GetBusinessActionGroupSelectFields(string.Empty))
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
                            group = new BusinessActionGroup();
                            StoreHelper.SetBusinessActionGroupSelectFields(group, reader, string.Empty);
                        }

                        reader.Close();
                    }
                }
            });

            return group;
        }

        public async Task<QueryResult<BusinessActionGroup>> QueryByName(string name, int page, int pageSize)
        {
            QueryResult<BusinessActionGroup> result = new QueryResult<BusinessActionGroup>();

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
		                           select @count= count(*) from BusinessActionGroup WITH (SNAPSHOT) where [name] like @name 
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
	
                                    select {0} from BusinessActionGroup WITH (SNAPSHOT) where [name] like @name  
                                    order by [actsequence]
		                            offset (@pagesize * (@currentpage - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetBusinessActionGroupSelectFields(string.Empty))
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

                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 150)
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

                    SqlDataReader reader = null;

                    using (reader = await commond.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var group = new BusinessActionGroup();
                            StoreHelper.SetBusinessActionGroupSelectFields(group, reader, string.Empty);
                            result.Results.Add(group);
                        }

                        reader.Close();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;

                    }
                }
            });

            return result;
        }

        public async Task<BusinessActionGroup> QueryByName(string name)
        {
            BusinessActionGroup group = null;

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
                    CommandText = string.Format(@"select {0} from BusinessActionGroup WITH (SNAPSHOT) where [name]=@name", StoreHelper.GetBusinessActionGroupSelectFields(string.Empty))
                })
                {

                    var parameter = new SqlParameter("@name", SqlDbType.NVarChar, 150)
                    {
                        Value = name
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    SqlDataReader reader = null;

                    using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            group = new BusinessActionGroup();
                            StoreHelper.SetBusinessActionGroupSelectFields(group, reader, string.Empty);
                        }

                        reader.Close();
                    }
                }
            });

            return group;
        }

        public async Task Update(BusinessActionGroup group)
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
                    Transaction=sqlTran,
                    CommandText = @"update BusinessActionGroup WITH (SNAPSHOT) set [name]=@name,[modifytime]=getutcdate()
                                    where [id]=@id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = group.ID
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 150)
                    {
                        Value = group.Name
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    await commond.ExecuteNonQueryAsync();                  
                }
            });
        }
    }
}
