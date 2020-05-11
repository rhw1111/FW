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
    /// 权限系统的数据操作
    /// </summary>
    [Injection(InterfaceType = typeof(IPrivilegeSystemStore), Scope = InjectionScope.Singleton)]
    public class PrivilegeSystemStore : IPrivilegeSystemStore
    {
        private IPrivilegeManagementConnectionFactory _privilegeManagementConnectionFactory;

        public PrivilegeSystemStore(IPrivilegeManagementConnectionFactory privilegeManagementConnectionFactory)
        {
            _privilegeManagementConnectionFactory = privilegeManagementConnectionFactory;
        }

        public async Task Add(PrivilegeSystem system)
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
                     Transaction=sqlTran
                })
                {
                    if (system.ID == Guid.Empty)
                    {
                        commond.CommandText = @"insert into PrivilegeSystem([id],[name],[createtime],[modifytime])
                                    values(default,@name,getutcdate(),getutcdate());
                                    select @newid=[id] from PrivilegeSystem where [sequence]=SCOPE_IDENTITY()";
                    }
                    else
                    {
                        commond.CommandText = @"insert into PrivilegeSystem([id],[name],[createtime],[modifytime])
                                    values(@id,@name,getutcdate(),getutcdate())";
                    }

                    SqlParameter parameter;
                    if (system.ID != Guid.Empty)
                    {
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = system.ID
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
                        Value = system.Name
                    };
                    commond.Parameters.Add(parameter);


                    commond.Prepare();

                    await commond.ExecuteNonQueryAsync();
          
                    if (system.ID == Guid.Empty)
                    {
                        system.ID = (Guid)commond.Parameters["@newid"].Value;
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
                    CommandText = @"delete from PrivilegeSystem where [id]=@id"
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

        public async Task<PrivilegeSystem> QueryById(Guid id)
        {
            PrivilegeSystem system = null;

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
                    CommandText = string.Format(@"select {0} from PrivilegeSystem where [id]=@id", StoreHelper.GetPrivilegeSystemSelectFields(string.Empty))
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
                            system = new PrivilegeSystem();
                            StoreHelper.SetPrivilegeSystemSelectFields(system, reader, string.Empty);
                        }

                        reader.Close();
                    }
                }
            });

            return system;
        }

        public async Task<PrivilegeSystem> QueryByName(string name)
        {
            PrivilegeSystem system = null;

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
                    CommandText = string.Format(@"select {0} from PrivilegeSystem where [name]=@name", StoreHelper.GetPrivilegeSystemSelectFields(string.Empty))
                })
                {

                    var parameter = new SqlParameter("@name", SqlDbType.NVarChar,150)
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
                            system = new PrivilegeSystem();
                            StoreHelper.SetPrivilegeSystemSelectFields(system, reader, string.Empty);
                        }

                        reader.Close();
                    }
                }
            });

            return system;
        }

        public async Task<QueryResult<PrivilegeSystem>> QueryByName(string name, int page, int pageSize)
        {
            QueryResult<PrivilegeSystem> result = new QueryResult<PrivilegeSystem>();

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
		                           select @count= count(*) from PrivilegeSystem where [name] like @name
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
	
                                    select {0} from PrivilegeSystem where [name] like @name
                                    order by [sequence] desc 
		                            offset (@pagesize * (@currentpage - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetPrivilegeSystemSelectFields(string.Empty))
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
                            var system = new PrivilegeSystem();
                            StoreHelper.SetPrivilegeSystemSelectFields(system, reader, string.Empty);
                            result.Results.Add(system);
                        }

                        reader.Close();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;
                    }
                }
            });

            return result;
        }

        public async Task Update(PrivilegeSystem system)
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
                    CommandText = @"update PrivilegeSystem set [name]=@name,[modifytime]=getutcdate()
                                    where [id]=@id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = system.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 150)
                    {
                        Value = system.Name
                    };
                    commond.Parameters.Add(parameter);


                    commond.Prepare();

                    await commond.ExecuteNonQueryAsync();
                }
            });
        }
    }
}
