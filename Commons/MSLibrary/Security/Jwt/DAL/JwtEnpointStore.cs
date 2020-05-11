using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using MSLibrary.DI;
using MSLibrary.Transaction;

namespace MSLibrary.Security.Jwt.DAL
{
    [Injection(InterfaceType = typeof(IJwtEnpointStore), Scope = InjectionScope.Singleton)]
    public class JwtEnpointStore : IJwtEnpointStore
    {
        private IJwtConnectionFactory _jwtConnectionFactory;

        public JwtEnpointStore(IJwtConnectionFactory jwtConnectionFactory)
        {
            _jwtConnectionFactory = jwtConnectionFactory;
        }

        public async Task Add(JwtEnpoint endpoint)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _jwtConnectionFactory.CreateAllForJwt(), async (conn, transaction) =>
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
                    if (endpoint.ID == Guid.Empty)
                    {
                        commond.CommandText = @"insert into JwtEndpoint ([id],[name],[createsignkeytype],[createsignkeyconfiguration],[validatesignkeytype],[validatesignkeyconfiguration],[createtime],[modifytime])
                                    values(default,@name,@createsignkeytype,@createsignkeyconfiguration,@validatesignkeytype,@validatesignkeyconfiguration,getutcdate(),getutcdate());
                                    select @newid=[id] from JwtEndpoint where [sequence]=SCOPE_IDENTITY()";
                    }
                    else
                    {
                        commond.CommandText = @"insert into JwtEndpoint ([id],[name],[createsignkeytype],[createsignkeyconfiguration],[validatesignkeytype],[validatesignkeyconfiguration],[createtime],[modifytime])
                                    values(@id,@name,@createsignkeytype,@createsignkeyconfiguration,@validatesignkeytype,@validatesignkeyconfiguration,getutcdate(),getutcdate())";
                    }

                    SqlParameter parameter;
                    if (endpoint.ID != Guid.Empty)
                    {
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = endpoint.ID
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


                    parameter = new SqlParameter("@name", SqlDbType.VarChar, 150)
                    {
                        Value = endpoint.Name
                    };
                    commond.Parameters.Add(parameter);

                    object objCreateSignKeyType = DBNull.Value;
                    if (endpoint.CreateSignKeyType != null)
                    {
                        objCreateSignKeyType = endpoint.CreateSignKeyType;
                    }
                    parameter = new SqlParameter("@createsignkeytype", SqlDbType.VarChar, 150)
                    {
                        Value = objCreateSignKeyType
                    };
                    commond.Parameters.Add(parameter);

                    object objCreateSignKeyConfiguration = DBNull.Value;
                    if (endpoint.CreateSignKeyConfiguration != null)
                    {
                        objCreateSignKeyConfiguration = endpoint.CreateSignKeyConfiguration;
                    }
                    parameter = new SqlParameter("@createsignkeyconfiguration", SqlDbType.NVarChar, 1000)
                    {
                        Value = objCreateSignKeyConfiguration
                    };
                    commond.Parameters.Add(parameter);


                    object objValidateSignKeyType = DBNull.Value;
                    if (endpoint.ValidateSignKeyType != null)
                    {
                        objValidateSignKeyType = endpoint.ValidateSignKeyType;
                    }
                    parameter = new SqlParameter("@validatesignkeytype", SqlDbType.VarChar, 150)
                    {
                        Value = objValidateSignKeyType
                    };
                    commond.Parameters.Add(parameter);

                    object objValidateSignKeyConfiguration = DBNull.Value;
                    if (endpoint.ValidateSignKeyConfiguration != null)
                    {
                        objValidateSignKeyConfiguration = endpoint.ValidateSignKeyConfiguration;
                    }
                    parameter = new SqlParameter("@validatesignkeyconfiguration", SqlDbType.NVarChar, 1000)
                    {
                        Value = objValidateSignKeyConfiguration
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    await commond.ExecuteNonQueryAsync();


                    if (endpoint.ID == Guid.Empty)
                    {
                        endpoint.ID = (Guid)commond.Parameters["@newid"].Value;
                    }
                }
            });
        }

        public async Task Delete(Guid id)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _jwtConnectionFactory.CreateAllForJwt(), async (conn, transaction) =>
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

                    commond.CommandText = @"delete from JwtEndpoint where [id]=@id";


                    SqlParameter parameter;

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

        public async Task<JwtEnpoint> QueryByID(Guid id)
        {
            JwtEnpoint endpoint = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _jwtConnectionFactory.CreateReadForJwt(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"select {0} from JwtEndpoint where [id]=@id", StoreHelper.GetJwtEnpointSelectFields(string.Empty))
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
                            endpoint = new JwtEnpoint();
                            StoreHelper.SetJwtEnpointSelectFields(endpoint, reader, string.Empty);
                        }

                        reader.Close();
                    }
                }
            });

            return endpoint;
        }

        public async Task<JwtEnpoint> QueryByName(string name)
        {
            JwtEnpoint endpoint = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _jwtConnectionFactory.CreateReadForJwt(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"select {0} from JwtEndpoint where [name]=@name", StoreHelper.GetJwtEnpointSelectFields(string.Empty))
                })
                {

                    var parameter = new SqlParameter("@name", SqlDbType.VarChar,150)
                    {
                        Value = name
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    using (var reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            endpoint = new JwtEnpoint();
                            StoreHelper.SetJwtEnpointSelectFields(endpoint, reader, string.Empty);
                        }

                        reader.Close();
                    }
                }
            });

            return endpoint;
        }

        public async Task<QueryResult<JwtEnpoint>> QueryByPage(string name, int page, int pageSize)
        {
            QueryResult<JwtEnpoint> result = new QueryResult<JwtEnpoint>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _jwtConnectionFactory.CreateReadForJwt(), async (conn, transaction) =>
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
		                           select @count= count(*) from JwtEndpoint where [name] like @name
		                           	
                                    select {0} from JwtEndpoint where [name] like @name
                                    order by [sequence]
		                            offset (@pagesize * (@currentpage - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetJwtEnpointSelectFields(string.Empty))
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

                    parameter = new SqlParameter("@name", SqlDbType.VarChar,155)
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

                    using (var reader = await commond.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var endpoint = new JwtEnpoint();
                            StoreHelper.SetJwtEnpointSelectFields(endpoint, reader, string.Empty);
                            result.Results.Add(endpoint);
                        }

                        reader.Close();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;

                    }
                }
            });

            return result;
        }

        public async Task Updtae(JwtEnpoint endpoint)
        {
            StringBuilder strCondition = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter newParameter;
            object newParameterValue;
            if (endpoint.Attributes.ContainsKey(JwtEnpoint.Attribute_Name))
            {
                strCondition.Append("[name]=@name,");               
                newParameter = new SqlParameter("@name", SqlDbType.VarChar, 150)
                {
                    Value = endpoint.Name
                };

                parameters.Add(newParameter);
            }

            if (endpoint.Attributes.ContainsKey(JwtEnpoint.Attribute_CreateSignKeyType))
            {
                if (endpoint.CreateSignKeyType==null)
                {
                    newParameterValue = DBNull.Value;
                }
                else
                {
                    newParameterValue = endpoint.CreateSignKeyType;
                }
                strCondition.Append("[createsignkeytype]=@createsignkeytype,");

                newParameter = new SqlParameter("@createsignkeytype", SqlDbType.VarChar, 150)
                {
                    Value = newParameterValue
                };

                parameters.Add(newParameter);
            }


            if (endpoint.Attributes.ContainsKey(JwtEnpoint.Attribute_CreateSignKeyTypeConfiguration))
            {
                if (endpoint.CreateSignKeyConfiguration == null)
                {
                    newParameterValue = DBNull.Value;
                }
                else
                {
                    newParameterValue = endpoint.CreateSignKeyConfiguration;
                }
                strCondition.Append("[createsignkeyconfiguration]=@createsignkeyconfiguration,");

                newParameter = new SqlParameter("@createsignkeyconfiguration", SqlDbType.NVarChar, 1000)
                {
                    Value = newParameterValue
                };

                parameters.Add(newParameter);
            }

            if (endpoint.Attributes.ContainsKey(JwtEnpoint.Attribute_ValidateSignKeyType))
            {
                if (endpoint.ValidateSignKeyType == null)
                {
                    newParameterValue = DBNull.Value;
                }
                else
                {
                    newParameterValue = endpoint.ValidateSignKeyType;
                }
                strCondition.Append("[validatesignkeytype]=@validatesignkeytype,");

                newParameter = new SqlParameter("@validatesignkeytype", SqlDbType.VarChar, 150)
                {
                    Value = newParameterValue
                };

                parameters.Add(newParameter);
            }


            if (endpoint.Attributes.ContainsKey(JwtEnpoint.Attribute_ValidateSignKeyTypeConfiguration))
            {
                if (endpoint.ValidateSignKeyConfiguration == null)
                {
                    newParameterValue = DBNull.Value;
                }
                else
                {
                    newParameterValue = endpoint.ValidateSignKeyConfiguration;
                }
                strCondition.Append("[validatesignkeyconfiguration]=@validatesignkeyconfiguration,");

                newParameter = new SqlParameter("@validatesignkeyconfiguration", SqlDbType.NVarChar, 1000)
                {
                    Value = newParameterValue
                };

                parameters.Add(newParameter);
            }

            if (strCondition.Length>0)
            {
                strCondition = strCondition.Remove(strCondition.Length - 1, 1);
            }

            if (strCondition.Length>0)
            {
                await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _jwtConnectionFactory.CreateAllForJwt(), async (conn, transaction) =>
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

                            commond.CommandText = $@"update JwtEndpoint set {0} where [id]=@id";
             
   

                        SqlParameter parameter;

                            parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                            {
                                Value = endpoint.ID
                            };
                            commond.Parameters.Add(parameter);

                        foreach(var item in parameters)
                        {
                            commond.Parameters.Add(item);
                        }

                        commond.Prepare();

                        await commond.ExecuteNonQueryAsync();

                    }
                });
            }

        }
    }
}
