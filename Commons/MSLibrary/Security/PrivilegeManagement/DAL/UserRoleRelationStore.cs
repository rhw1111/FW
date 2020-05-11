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

    [Injection(InterfaceType = typeof(IUserRoleRelationStore), Scope = InjectionScope.Singleton)]
    public class UserRoleRelationStore : IUserRoleRelationStore
    {
        private IPrivilegeManagementConnectionFactory _privilegeManagementConnectionFactory;

        public UserRoleRelationStore(IPrivilegeManagementConnectionFactory privilegeManagementConnectionFactory)
        {
            _privilegeManagementConnectionFactory = privilegeManagementConnectionFactory;
        }
        public async Task Add(UserRoleRelation relation)
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
                    if (relation.ID == Guid.Empty)
                    {
                        commond.CommandText = @"insert into [UserRoleRelation]([id],[userkey],[systemid],[roleid],[createtime],[modifytime])
                                    values(default,@userkey,@systemid,getutcdate(),getutcdate());
                                    select @newid=[id] from [UserRoleRelation] where [sequence]=SCOPE_IDENTITY()";
                    }
                    else
                    {
                        commond.CommandText = @"insert into [UserRoleRelation]([id],[userkey],[systemid],[roleid],[createtime],[modifytime])
                                    values(@id,@userkey,@systemid,@roleid,getutcdate(),getutcdate())";
                    }

                    SqlParameter parameter;
                    if (relation.ID != Guid.Empty)
                    {
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = relation.ID
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

                    parameter = new SqlParameter("@userkey", SqlDbType.NVarChar, 150)
                    {
                        Value = relation.UserKey
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@systemid", SqlDbType.UniqueIdentifier)
                    {
                        Value = relation.System.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@roleid", SqlDbType.UniqueIdentifier)
                    {
                        Value = relation.Role.ID
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();


                    await commond.ExecuteNonQueryAsync();


                    

                    if (relation.ID == Guid.Empty)
                    {
                        relation.ID = (Guid)commond.Parameters["@newid"].Value;
                    }
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
                    Transaction=sqlTran,
                    CommandText = @"delete from [UserRoleRelation] where [id]=@id"
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

        public async Task Delete(Guid roleId, Guid id)
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
                    CommandText = @"delete from [UserRoleRelation] where [id]=@id and [roleid]=@roleid"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@roleid", SqlDbType.UniqueIdentifier)
                    {
                        Value = roleId
                    };
                    commond.Parameters.Add(parameter);



                    commond.Prepare();

                    await commond.ExecuteNonQueryAsync();
                }
            });
        }

        public async Task<UserRoleRelation> QueryById(Guid id)
        {
            UserRoleRelation relation = null;

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
                    Transaction = sqlTran,
                    CommandText = string.Format(@"select {0},{1},{2} from [UserRoleRelation] as urr join [role] as r
                                                  on urr.[roleid]=r.[id] join [PrivilegeSystem] as s
                                                  on urr.[systemid]=s.[id] where urr.[id]=@id", StoreHelper.GetUserRoleRelationSelectFields("urr"), StoreHelper.GetRoleSelectFields("r"), StoreHelper.GetPrivilegeSystemSelectFields("s"))
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
                            relation = new UserRoleRelation();
                            StoreHelper.SetUserRoleRelationSelectFields(relation, reader, "urr");
                            relation.Role = new Role();
                            StoreHelper.SetRoleSelectFields(relation.Role, reader, "r");
                            relation.System = new PrivilegeSystem();
                            StoreHelper.SetPrivilegeSystemSelectFields(relation.System, reader, "s");
                        }

                        reader.Close();
                    }
                }
            });

            return relation;
        }

        public async Task<QueryResult<UserRoleRelation>> QueryByRole(Guid roleId, int page, int pageSize)
        {
            QueryResult<UserRoleRelation> result = new QueryResult<UserRoleRelation>();

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
		                           select @count= count(*) from [UserRoleRelation] as urr join [Role] as r
                                   on urr.[roleid]=r.[id] join [PrivilegeSystem] as s
                                   on urr.[systemid]=s.[id] where r.[id]=@roleid
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
	
                                    select {0},{1},{2} from [UserRoleRelation] as urr join [Role] as r
                                    on urr.[roleid]=r.[id] join [PrivilegeSystem] as s
                                    on urr.[systemid]=s.[id] where r.[id]=@roleid
                                    order by urr.[sequence] desc 
		                            offset (@pagesize * (@currentpage - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetUserRoleRelationSelectFields("urr"),StoreHelper.GetRoleSelectFields("r"),StoreHelper.GetPrivilegeSystemSelectFields("s"))
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
                            var relation = new UserRoleRelation();
                            StoreHelper.SetUserRoleRelationSelectFields(relation, reader, "urr");
                            relation.Role = new Role();
                            StoreHelper.SetRoleSelectFields(relation.Role, reader, "r");
                            relation.System = new PrivilegeSystem();
                            StoreHelper.SetPrivilegeSystemSelectFields(relation.System,reader,"s");
                            result.Results.Add(relation);
                        }

                        reader.Close();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;
                    }
                }
            });

            return result;
        }

        public async Task QueryByRole(Guid roleId, Func<UserRoleRelation, Task> callback)
        {
            List<UserRoleRelation> relationList = new List<UserRoleRelation>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _privilegeManagementConnectionFactory.CreateReadForPrivilegeManagement(), async (conn,transaction) =>
            {
                Int64? sequence = null;
                int pageSize = 500;

                while (true)
                {
                    relationList.Clear();

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
                            commond.CommandText = string.Format(@"select top (@pagesize) {0} from [UserRoleRelation] as urr join [Role] as r
                                                                  on urr.[roleid]=r.[id] join [PrivilegeSystem] as s
                                                                  on urr.[systemid]=s.[id] where r.[id]=@roleid order by urr.[sequence]", StoreHelper.GetUserRoleRelationSelectFields("urr"),StoreHelper.GetRoleSelectFields("r"),StoreHelper.GetPrivilegeSystemSelectFields("s"));
                        }
                        else
                        {
                            commond.CommandText = string.Format(@"select top (@pagesize) {0} from [UserRoleRelation] as urr join [Role] as r
                                                                  on urr.[roleid]=r.[id] join [PrivilegeSystem] as s
                                                                  on urr.[systemid]=s.[id] where r.[id]=@roleid and urr.[sequence]>@sequence order by urr.[sequence]", StoreHelper.GetUserRoleRelationSelectFields("urr"), StoreHelper.GetRoleSelectFields("r"), StoreHelper.GetPrivilegeSystemSelectFields("s"));
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
                                var relation = new UserRoleRelation();
                                StoreHelper.SetUserRoleRelationSelectFields(relation, reader, "urr");
                                sequence = (Int64)reader["urrsequence"];
                                relation.Role = new Role();
                                StoreHelper.SetRoleSelectFields(relation.Role,reader,"r");
                                relation.System = new PrivilegeSystem();
                                StoreHelper.SetPrivilegeSystemSelectFields(relation.System, reader, "s");
                                relationList.Add(relation);
                            }

                            reader.Close();
                        }


                    }

                    foreach (var relationItem in relationList)
                    {
                        await callback(relationItem);
                    }

                    if (relationList.Count != pageSize)
                    {
                        break;
                    }

                }

            });
        }

        public async Task<UserRoleRelation> QueryByRole(Guid roleId, Guid id)
        {
            UserRoleRelation relation = null;

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
                    CommandText = string.Format(@"select {0},{1},{2} from [UserRoleRelation] as urr join [role] as r
                                                  on urr.[roleid]=r.[id] join [PrivilegeSystem] as s
                                                  on urr.[systemid]=s.[id] where urr.[id]=@id and urr.[roleid]", StoreHelper.GetUserRoleRelationSelectFields("urr"), StoreHelper.GetRoleSelectFields("r"), StoreHelper.GetPrivilegeSystemSelectFields("s"))
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@roleid", SqlDbType.UniqueIdentifier)
                    {
                        Value = roleId
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    SqlDataReader reader = null;

                    using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            relation = new UserRoleRelation();
                            StoreHelper.SetUserRoleRelationSelectFields(relation, reader, "urr");
                            relation.Role = new Role();
                            StoreHelper.SetRoleSelectFields(relation.Role, reader, "r");
                            relation.System = new PrivilegeSystem();
                            StoreHelper.SetPrivilegeSystemSelectFields(relation.System, reader, "s");
                        }

                        reader.Close();
                    }
                }
            });

            return relation;
        }

        public async Task<QueryResult<UserRoleRelation>> QueryByRoleAndUserKey(Guid roleId, string userKey, int page, int pageSize)
        {
            QueryResult<UserRoleRelation> result = new QueryResult<UserRoleRelation>();

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
		                           select @count= count(*) from [UserRoleRelation] as urr join [Role] as r
                                   on urr.[roleid]=r.[id] join [PrivilegeSystem] as s
                                   on urr.[systemid]=s.[id] where r.[id]=@roleid and urr.[userkey] like @userkey

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
	
                                    select {0},{1},{2} from [UserRoleRelation] as urr join [Role] as r
                                    on urr.[roleid]=r.[id] join [PrivilegeSystem] as s
                                    on urr.[systemid]=s.[id] where r.[id]=@roleid and urr.[userkey] like @userkey
                                    order by urr.[sequence] desc 
		                            offset (@pagesize * (@currentpage - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetUserRoleRelationSelectFields("urr"), StoreHelper.GetRoleSelectFields("r"), StoreHelper.GetPrivilegeSystemSelectFields("s"))
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

                    parameter = new SqlParameter("@userKey", SqlDbType.NVarChar, 150)
                    {
                        Value = $"{userKey.ToSqlLike()}%" 
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
                            var relation = new UserRoleRelation();
                            StoreHelper.SetUserRoleRelationSelectFields(relation, reader, "urr");
                            relation.Role = new Role();
                            StoreHelper.SetRoleSelectFields(relation.Role, reader, "r");
                            relation.System = new PrivilegeSystem();
                            StoreHelper.SetPrivilegeSystemSelectFields(relation.System, reader, "s");
                            result.Results.Add(relation);
                        }

                        reader.Close();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;
                    }
                }
            });

            return result;
        }

        public async Task Update(UserRoleRelation relation)
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
                    CommandText = @"update [UserRoleRelation] set [userkey]=@userkey,[systemid]=@systemid,[roleid]=@roleid,[modifytime]=getutcdate()
                                    where [id]=@id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = relation.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@userkey", SqlDbType.NVarChar, 150)
                    {
                        Value = relation.UserKey
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@systemid", SqlDbType.UniqueIdentifier)
                    {
                        Value = relation.System.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@roleid", SqlDbType.UniqueIdentifier)
                    {
                        Value = relation.Role.ID
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();


                    await commond.ExecuteNonQueryAsync();            
                }
            });
        }
    }
}
