using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using MSLibrary.DI;
using MSLibrary.Transaction;

namespace MSLibrary.Security.PrivilegeManagement.DAL
{
    /// <summary>
    /// 权限的数据操作
    /// </summary>
    [Injection(InterfaceType = typeof(IPrivilegeStore), Scope = InjectionScope.Singleton)]
    public class PrivilegeStore : IPrivilegeStore
    {
        private IPrivilegeManagementConnectionFactory _privilegeManagementConnectionFactory;

        public PrivilegeStore(IPrivilegeManagementConnectionFactory privilegeManagementConnectionFactory)
        {
            _privilegeManagementConnectionFactory = privilegeManagementConnectionFactory;
        }

        public async Task Add(Privilege privilege)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _privilegeManagementConnectionFactory.CreateAllForPrivilegeManagement(), async (conn,transaction) =>
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
                    if (privilege.ID == Guid.Empty)
                    {
                        commond.CommandText = @"insert into Privilege([id],[name],[systemid],[description],[createtime],[modifytime])
                                    values(default,@name,@systemid,@description,getutcdate(),getutcdate());
                                    select @newid=[id] from Privilege where [sequence]=SCOPE_IDENTITY()";
                    }
                    else
                    {
                        commond.CommandText = @"insert into Privilege([id],[name],[systemid],[description],[createtime],[modifytime])
                                    values(@id,@name,@systemid,@description,getutcdate(),getutcdate())";
                    }

                    SqlParameter parameter;
                    if (privilege.ID != Guid.Empty)
                    {
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = privilege.ID
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
                        Value = privilege.Name
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@systemid", SqlDbType.UniqueIdentifier)
                    {
                        Value = privilege.SystemId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@description", SqlDbType.NVarChar, 1000)
                    {
                        Value = privilege.Description
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();


                    await commond.ExecuteNonQueryAsync();


                    if (privilege.ID == Guid.Empty)
                    {
                        privilege.ID = (Guid)commond.Parameters["@newid"].Value;
                    }
                }
            });
        }

        public async Task AddPrivilegeRelation(Guid roleId, Guid privilegeId)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _privilegeManagementConnectionFactory.CreateAllForPrivilegeManagement(), async (conn,transaction) =>
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
                    CommandText = @"insert into RolePrivilegeRelation([roleid],[privilegeid])
                                   value(@roleid,@privilegeid)"
                })
                {


                    SqlParameter parameter;

                    parameter = new SqlParameter("@roleid", SqlDbType.UniqueIdentifier)
                    {
                        Value = roleId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@privilegeid", SqlDbType.UniqueIdentifier)
                    {
                        Value = privilegeId
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();


                    await commond.ExecuteNonQueryAsync();
                }
            });
        }

        public async Task Delete(Guid id)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _privilegeManagementConnectionFactory.CreateAllForPrivilegeManagement(), async (conn,transaction) =>
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
                    CommandText = @"delete from Privilege where [id]=@id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    await commond.ExecuteNonQueryAsync();

                }
            });
        }

        public async Task DeletePrivilegeRelation(Guid roleId, Guid privilegeId)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _privilegeManagementConnectionFactory.CreateAllForPrivilegeManagement(), async (conn,transaction) =>
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
                    CommandText = @"delete from RolePrivilegeRelation([roleid],[privilegeid])
                                   value(@roleid,@privilegeid)"
                })
                {


                    SqlParameter parameter;

                    parameter = new SqlParameter("@roleid", SqlDbType.UniqueIdentifier)
                    {
                        Value = roleId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@privilegeid", SqlDbType.UniqueIdentifier)
                    {
                        Value = privilegeId
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();


                    await commond.ExecuteNonQueryAsync();
                }
            });
        }

        public async Task<QueryResult<Privilege>> QueryByCriteria(PrivilegeQueryCriteria criteria, int page, int pageSize)
        {
            SqlParameter parameter;
            List<SqlParameter> parameters = new List<SqlParameter>();
            StringBuilder strCondition = new StringBuilder();
            if (criteria.UseSystem)
            {
                strCondition.Append(" and systemid=@systemid ");

                parameter = new SqlParameter("@systemid", SqlDbType.UniqueIdentifier)
                {
                    Value = criteria.SystemId
                };
                parameters.Add(parameter);
            }

            if (criteria.UseName)
            {
                strCondition.Append(" and [name] like @name ");
                parameter = new SqlParameter("@name", SqlDbType.NVarChar, 150)
                {
                    Value = $"{criteria.Name.ToSqlLike()}%"
                };
                parameters.Add(parameter);
            }

            QueryResult<Privilege> result = new QueryResult<Privilege>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _privilegeManagementConnectionFactory.CreateReadForPrivilegeManagement(), async (conn,transaction) =>
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
		                           select @count= count(*) from Privilege where 1=1 {1}
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
	
                                    select {0} from Privilege where 1=1 {1}
                                    order by [sequence] desc 
		                            offset (@pagesize * (@currentpage - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetPrivilegeSelectFields(string.Empty),strCondition.ToString())
                })
                {

                    parameter = new SqlParameter("@page", SqlDbType.Int)
                    {
                        Value = page
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@pagesize", SqlDbType.Int)
                    {
                        Value = pageSize
                    };

                    foreach(var item in parameters)
                    {
                        commond.Parameters.Add(item);
                    }

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


                    SqlDataReader reader = null;


                    using (reader = await commond.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var privilege = new Privilege();
                            StoreHelper.SetPrivilegeSelectFields(privilege, reader, string.Empty);
                            result.Results.Add(privilege);
                        }

                        reader.Close();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;
                    }
                }
            });

            return result;
        }

        public async Task<Privilege> QueryById(Guid id)
        {
            Privilege privilege = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _privilegeManagementConnectionFactory.CreateReadForPrivilegeManagement(), async (conn,transaction) =>
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
                    CommandText = string.Format(@"select {0} from Privilege where [id]=@id", StoreHelper.GetPrivilegeSelectFields(string.Empty))
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
                            privilege = new Privilege();
                            StoreHelper.SetPrivilegeSelectFields(privilege, reader, string.Empty);
                        }

                        reader.Close();
                    }
                }
            });

            return privilege;
        }

        public async Task<Privilege> QueryByNameAndUser(string userKey,Guid systemId, string name)
        {
            Privilege privilege = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _privilegeManagementConnectionFactory.CreateReadForPrivilegeManagement(), async (conn,transaction) =>
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
                    CommandText = string.Format(@"select {0} from privilege as p join RolePrivilegeRelation as rpr
                                                  on p.id=rpr.privilegeid
                                                  join UserRoleRelation as urr
                                                  on rpr.roleid=urr.roleid
                                                  where urr.[userkey]=@userkey and urr.systemid=@systemid and p.[name]=@name", StoreHelper.GetPrivilegeSelectFields("p"))
                })
                {

                    var parameter = new SqlParameter("@userkey", SqlDbType.NVarChar,150)
                    {
                        Value = userKey
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@systemid", SqlDbType.UniqueIdentifier)
                    {
                        Value = systemId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 150)
                    {
                        Value = name
                    };
                    commond.Parameters.Add(parameter);


                    commond.Prepare();

 
                    SqlDataReader reader = null;

                    using (reader= await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            privilege = new Privilege();
                            StoreHelper.SetPrivilegeSelectFields(privilege, reader, "p");
                        }

                        reader.Close();
                    }
                }
            });

            return privilege;
        }

        public async Task<QueryResult<Privilege>> QueryByRole(Guid roleId, int page, int pageSize)
        {
            QueryResult<Privilege> result = new QueryResult<Privilege>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _privilegeManagementConnectionFactory.CreateReadForPrivilegeManagement(), async (conn,transaction) =>
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
		                           select @count= count(*) from Privilege as p join RolePrivilegeRelation as rpr
                                                  on p.id=rpr.privilegeid
                                                  where rpr.roleid=@roleid
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
	
                                    select {0} from Privilege as p join RolePrivilegeRelation as rpr
                                    on p.id=rpr.privilegeid where rpr.roleid=@roleid
                                    order by [psequence] desc 
		                            offset (@pagesize * (@currentpage - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetPrivilegeSelectFields("p"))
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

                    parameter = new SqlParameter("@roleid", SqlDbType.UniqueIdentifier)
                    {
                        Value = roleId
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
                            var privilege = new Privilege();
                            StoreHelper.SetPrivilegeSelectFields(privilege, reader, "p");
                            result.Results.Add(privilege);
                        }

                        reader.Close();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;
                    }
                }
            });

            return result;
        }

        public async Task QueryByRole(Guid roleId, Func<Privilege, Task> callback)
        {
            List<Privilege> queueList = new List<Privilege>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _privilegeManagementConnectionFactory.CreateReadForPrivilegeManagement(), async (conn,transaction) =>
            {
                Int64? sequence = null;
                int pageSize = 500;

                while (true)
                {
                    queueList.Clear();

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
                            commond.CommandText = string.Format(@"select top (@pagesize) {0} from [Privilege] as p join [dbo].[RolePrivilegeRelation] as rpr
                                                                  on p.id=rpr.privilegeid
                                                                  where rpr.roleid=@roleid order by p.[sequence]", StoreHelper.GetPrivilegeSelectFields("p"));
                        }
                        else
                        {
                            commond.CommandText = string.Format(@"select top (@pagesize) {0} from [Privilege] as p join [dbo].[RolePrivilegeRelation] as rpr
                                                                  on p.id=rpr.privilegeid
                                                                  where rpr.roleid=@roleid and p.[sequence]>@sequence order by p.[sequence]", StoreHelper.GetPrivilegeSelectFields("p"));
                        }

                        var parameter = new SqlParameter("@roleid", SqlDbType.UniqueIdentifier)
                        {
                            Value = roleId
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


                        SqlDataReader reader = null;

                        using (reader = await commond.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var privilege = new Privilege();
                                StoreHelper.SetPrivilegeSelectFields(privilege, reader, "p");
                                sequence = (Int64)reader["psequence"];
                                queueList.Add(privilege);
                            }

                            reader.Close();
                        }


                    }

                    foreach (var queueItem in queueList)
                    {
                        await callback(queueItem);
                    }

                    if (queueList.Count != pageSize)
                    {
                        break;
                    }

                }

            });

        }

        public async Task Update(Privilege privilege)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _privilegeManagementConnectionFactory.CreateAllForPrivilegeManagement(), async (conn,transaction) =>
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
                    CommandText = @"update Privilege set [name]=@name,[systemid]=@systemid,[description]=@description,[modifytime]=getutcdate()
                                    where [id]=@id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = privilege.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 150)
                    {
                        Value = privilege.Name
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@systemid", SqlDbType.UniqueIdentifier)
                    {
                        Value = privilege.SystemId
                    };
                    commond.Parameters.Add(parameter);


                    commond.Prepare();


                    await commond.ExecuteNonQueryAsync();             
                }
            });
        }
    }
}
