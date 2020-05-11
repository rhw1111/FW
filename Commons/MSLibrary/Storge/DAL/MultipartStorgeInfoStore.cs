using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using MSLibrary.DI;
using MSLibrary.Transaction;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Storge.DAL
{
    [Injection(InterfaceType = typeof(IMultipartStorgeInfoStore), Scope = InjectionScope.Singleton)]
    public class MultipartStorgeInfoStore : IMultipartStorgeInfoStore
    {
        private IStorgeConnectionFactory _dbConnectionFactory;

        public MultipartStorgeInfoStore(IStorgeConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task Add(MultipartStorgeInfo info)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, true, _dbConnectionFactory.CreateAllForStorge(), async (conn, transaction) =>
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
                    if (info.ID == Guid.Empty)
                    {
                        command.CommandText = @"declare @sequence bigint, @firstsequence bigint
                                                INSERT INTO [dbo].[MultipartStorgeInfo]
                                                    ([id],[name],[displayname],[suffix],[size],[sourceinfo],[credentialinfo],[extensioninfo],[completeextensioninfo],[status],[createtime],[modifytime])
                                                VALUES
                                                    (default, @name, @displayname, @suffix, @size, @sourceinfo, @credentialinfo, @extensioninfo,@completeextensioninfo, @status, GETUTCDATE(), GETUTCDATE());
                                                set @sequence=SCOPE_IDENTITY()
                                                select @newid =[id] from [dbo].[MultipartStorgeInfo] where [sequence] = @sequence

                                                select top 1 @firstsequence=[sequence] from [dbo].[MultipartStorgeInfo]with(nolock)
                                                where [status]=0 and [name]=@name 
                                                order by [sequence]

                                                if @firstsequence<>@sequence
                                                    begin
                                                        THROW 60101, '', 1;
                                                    end";
                                                    
                        parameter = new SqlParameter("@newid", SqlDbType.UniqueIdentifier)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(parameter);
                    }
                    else
                    {
                        command.CommandText = @"declare @sequence bigint, @firstsequence bigint
                                                INSERT INTO [dbo].[MultipartStorgeInfo]
                                                    ([id],[name],[displayname],[suffix],[size],[sourceinfo],[credentialinfo],[extensioninfo],[completeextensioninfo],[status],[createtime],[modifytime])
                                                VALUES
                                                    (@id, @name, @displayname, @suffix, @size, @sourceinfo, @credentialinfo, @extensioninfo,@completeextensioninfo, @status, GETUTCDATE(), GETUTCDATE());
                                                set @sequence=SCOPE_IDENTITY()
                                                select @newid =[id] from [dbo].[MultipartStorgeInfo] where [sequence] = @sequence

                                                select top 1 @firstsequence=[sequence] from [dbo].[MultipartStorgeInfo]with(nolock)
                                                where [status]=0 and [name]=@name 
                                                order by [sequence]

                                                if @firstsequence<>@sequence
                                                    begin
                                                        THROW 60101, '', 1;
                                                    end";
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = info.ID
                        };
                        command.Parameters.Add(parameter);
                    }
                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 500)
                    {
                        Value = info.Name
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@displayname", SqlDbType.NVarChar, 500)
                    {
                        Value = info.DisplayName
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@suffix", SqlDbType.VarChar, 50)
                    {
                        Value = info.Suffix
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@size", SqlDbType.BigInt)
                    {
                        Value = info.Size
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@sourceinfo", SqlDbType.NVarChar,500)
                    {
                        Value = info.SourceInfo
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@credentialinfo", SqlDbType.NVarChar, 500)
                    {
                        Value = info.CredentialInfo
                    };
                    command.Parameters.Add(parameter);


                    object objExtensionInfo = DBNull.Value;
                    int lenExtensionInfo = 0;
                    if (info.ExtensionInfo != null)
                    {
                        objExtensionInfo = info.ExtensionInfo;
                        lenExtensionInfo = info.ExtensionInfo.Length;
                    }
                    parameter = new SqlParameter("@extensioninfo", SqlDbType.NVarChar, lenExtensionInfo)
                    {
                        Value = objExtensionInfo
                    };
                    command.Parameters.Add(parameter);



                    object objCompleteExtensionInfo = DBNull.Value;
                    int lenCompleteExtensionInfo = 0;
                    if (info.CompleteExtensionInfo != null)
                    {
                        objCompleteExtensionInfo = info.CompleteExtensionInfo;
                        lenCompleteExtensionInfo = info.CompleteExtensionInfo.Length;
                    }
                    parameter = new SqlParameter("@completeextensioninfo", SqlDbType.NVarChar, lenCompleteExtensionInfo)
                    {
                        Value = objCompleteExtensionInfo
                    };
                    command.Parameters.Add(parameter);




                    parameter = new SqlParameter("@status", SqlDbType.Int)
                    {
                        Value = info.Status
                    };
                    command.Parameters.Add(parameter);


                    command.Prepare();

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (SqlException ex)
                    {
                        if (ex == null)
                        {
                            throw;
                        }
                        if (ex.Number == 60101)
                        {
                            var fragment = new TextFragment()
                            {
                                Code = TextCodes.ExistRunMultipartStorgeInfoByName,
                                DefaultFormatting = "已经存在名称为{0}的未完成分片存储信息",
                                ReplaceParameters = new List<object>() { info.Name }
                            };
                            throw new UtilityException((int)Errors.ExistRunMultipartStorgeInfoByName, fragment);
                        }
                        else
                        {
                            throw;
                        }
                    }
                    //如果用户未赋值ID则创建成功后返回ID
                    if (info.ID == Guid.Empty)
                    {
                        info.ID = (Guid)command.Parameters["@newid"].Value;
                    };
                }

            });
        }

        public async Task Delete(Guid infoID)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForStorge(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }
                await using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                })
                {
                    SqlParameter parameter;

                    command.CommandText = @"delete from MultipartStorgeInfo where [id]=@id";
                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = infoID
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();
                    await command.ExecuteNonQueryAsync();
                }

            });
        }

        public async Task DeleteBySourceID(string sourceInfo, Guid infoID)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForStorge(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }
                await using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                })
                {
                    SqlParameter parameter;

                    command.CommandText = @"delete from MultipartStorgeInfo where [id]=@id and [sourceinfo]=@sourceinfo";
                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = infoID
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@sourceinfo", SqlDbType.NVarChar, 500)
                    {
                        Value = sourceInfo
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();
                    await command.ExecuteNonQueryAsync();
                }

            });
        }

        public async Task<MultipartStorgeInfo> QueryByID(Guid id)
        {
            MultipartStorgeInfo result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateAllForStorge(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"select {0} from [dbo].[MultipartStorgeInfo] where [id]=@id", StoreHelper.GetMultipartStorgeInfoSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {
                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();


                    SqlDataReader reader = null;

                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new MultipartStorgeInfo();
                            StoreHelper.SetMultipartStorgeInfoSelectFields(result, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return result;
        }

        public async Task<QueryResult<MultipartStorgeInfo>> QueryByPage(string name, string displayName, string sourceInfo, string credentialInfo, int? status, int page, int pageSize)
        {
            string strStatusCondition = "1=1";
            if (status==null)
            {
                strStatusCondition = "status=@status";
            }
            QueryResult<MultipartStorgeInfo> result = new QueryResult<MultipartStorgeInfo>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForStorge(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"SET @currentpage = @page;
                                                SELECT @count = COUNT(*)
                                                FROM [dbo].[MultipartStorgeInfo]
                                                WHERE name like @name and displayname like @displayname and {1} and sourceinfo=@sourceinfo and credentialinfo=@credentialinfo;
                                                
                                                SELECT {0}
                                                FROM [dbo].[MultipartStorgeInfo]
                                                WHERE name like @name and displayname like @displayname and {1} and sourceinfo=@sourceinfo and credentialinfo=@credentialinfo
                                                ORDER BY sequence OFFSET (@pagesize * (@currentpage - 1)) ROWS FETCH NEXT @pagesize ROWS ONLY;",
                                                StoreHelper.GetMultipartStorgeInfoSelectFields(string.Empty), strStatusCondition),
                    Transaction = sqlTran
                })
                {
                    var parameter = new SqlParameter("@name", SqlDbType.NVarChar, 500)
                    {
                        Value = string.Format("{0}%", name.ToSqlLike())
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@displayname", SqlDbType.NVarChar, 500)
                    {
                        Value = string.Format("{0}%", displayName.ToSqlLike())
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@sourceinfo", SqlDbType.NVarChar, 500)
                    {
                        Value = string.Format("{0}%", sourceInfo.ToSqlLike())
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@credentialinfo", SqlDbType.NVarChar, 500)
                    {
                        Value = string.Format("{0}%", credentialInfo.ToSqlLike())
                    };
                    command.Parameters.Add(parameter);

                    if (status!=null)
                    {
                        parameter = new SqlParameter("@status", SqlDbType.Int)
                        {
                            Value = status.Value
                        };
                        command.Parameters.Add(parameter);
                    }

                    parameter = new SqlParameter("@page", SqlDbType.Int)
                    {
                        Value = page
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@pagesize", SqlDbType.Int)
                    {
                        Value = pageSize
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@count", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@currentpage", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    SqlDataReader reader = null;

                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var info = new MultipartStorgeInfo();
                            StoreHelper.SetMultipartStorgeInfoSelectFields(info, reader, string.Empty);

                            result.Results.Add(info);
                        }
                        await reader.CloseAsync();
                        result.TotalCount = (int)command.Parameters["@count"].Value;
                        result.CurrentPage = (int)command.Parameters["@currentpage"].Value;
                    }
                }
            });
            return result;
        }

        public async Task<MultipartStorgeInfo> QueryBySourceID(string sourceInfo, Guid id)
        {
            MultipartStorgeInfo result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForStorge(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"select {0} from [dbo].[MultipartStorgeInfo] where [id] = @id", StoreHelper.GetMultipartStorgeInfoSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {
                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    SqlDataReader reader = null;

                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new MultipartStorgeInfo();
                            StoreHelper.SetMultipartStorgeInfoSelectFields(result, reader, string.Empty);
                        }

                       await reader.CloseAsync();
                    }
                }
            });

            return result;
        }

        public async Task<QueryResult<MultipartStorgeInfo>> QueryBySourcePage(string sourceInfo, int? status, int page, int pageSize)
        {
            string strStatusCondition = "1=1";
            if (status == null)
            {
                strStatusCondition = "status=@status";
            }
            QueryResult<MultipartStorgeInfo> result = new QueryResult<MultipartStorgeInfo>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForStorge(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"SET @currentpage = @page;
                                                SELECT @count = COUNT(*)
                                                FROM [dbo].[MultipartStorgeInfo]
                                                WHERE  {1} and sourceinfo=@sourceinfo ;
                                                
                                                SELECT {0}
                                                FROM [dbo].[MultipartStorgeInfo]
                                                WHERE {1} and sourceinfo=@sourceinfo
                                                ORDER BY sequence OFFSET (@pagesize * (@currentpage - 1)) ROWS FETCH NEXT @pagesize ROWS ONLY;",
                                                StoreHelper.GetMultipartStorgeInfoSelectFields(string.Empty), strStatusCondition),
                    Transaction = sqlTran
                })
                {


                    var parameter = new SqlParameter("@sourceinfo", SqlDbType.NVarChar, 500)
                    {
                        Value = string.Format("{0}%", sourceInfo.ToSqlLike())
                    };
                    command.Parameters.Add(parameter);

                    if (status != null)
                    {
                        parameter = new SqlParameter("@status", SqlDbType.Int)
                        {
                            Value = status.Value
                        };
                        command.Parameters.Add(parameter);
                    }

                    parameter = new SqlParameter("@page", SqlDbType.Int)
                    {
                        Value = page
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@pagesize", SqlDbType.Int)
                    {
                        Value = pageSize
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@count", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@currentpage", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    SqlDataReader reader = null;

                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var info = new MultipartStorgeInfo();
                            StoreHelper.SetMultipartStorgeInfoSelectFields(info, reader, string.Empty);

                            result.Results.Add(info);
                        }
                        await reader.CloseAsync();
                        result.TotalCount = (int)command.Parameters["@count"].Value;
                        result.CurrentPage = (int)command.Parameters["@currentpage"].Value;
                    }
                }
            });
            return result;
        }

        public async Task<MultipartStorgeInfo> QueryRunByName(string name)
        {
            MultipartStorgeInfo result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForStorge(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

               await using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"select {0} from [dbo].[MultipartStorgeInfo] where [name] = @name and status=0", StoreHelper.GetMultipartStorgeInfoSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {
                    var parameter = new SqlParameter("@name", SqlDbType.NVarChar,500)
                    {
                        Value = name
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    SqlDataReader reader = null;

                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new MultipartStorgeInfo();
                            StoreHelper.SetMultipartStorgeInfoSelectFields(result, reader, string.Empty);
                        }

                       await reader.CloseAsync();
                    }
                }
            });

            return result;
        }

        public async Task<MultipartStorgeInfo> QueryRunBySourceName(string sourceInfo, string name)
        {
            MultipartStorgeInfo result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForStorge(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"select {0} from [dbo].[MultipartStorgeInfo] where [name] = @name and sourceinfo=@sourceinfo and status=0", StoreHelper.GetMultipartStorgeInfoSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {
                    var parameter = new SqlParameter("@name", SqlDbType.NVarChar, 500)
                    {
                        Value = name
                    };
                    command.Parameters.Add(parameter);

                    parameter = new SqlParameter("@sourceinfo", SqlDbType.NVarChar, 500)
                    {
                        Value = sourceInfo
                    };
                    command.Parameters.Add(parameter);

                    await command.PrepareAsync();

                    SqlDataReader reader = null;

                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new MultipartStorgeInfo();
                            StoreHelper.SetMultipartStorgeInfoSelectFields(result, reader, string.Empty);
                        }

                       await reader.CloseAsync();
                    }
                }
            });

            return result;
        }

        public async Task Updtae(MultipartStorgeInfo info)
        {
            SqlParameter parameter;
            List<SqlParameter> parameters = new List<SqlParameter>();
            StringBuilder strCondition = new StringBuilder();

            if (info.Attributes.ContainsKey("ExtensionInfo"))
            {
                strCondition.Append("extensioninfo=@extensioninfo,");
                if (info.ExtensionInfo==null)
                {
                    parameter = new SqlParameter("@extensioninfo", SqlDbType.NVarChar, 0)
                    {
                        Value = DBNull.Value
                    };
                }
                else
                {
                    parameter = new SqlParameter("@extensioninfo", SqlDbType.NVarChar, info.ExtensionInfo.Length)
                    {
                        Value = info.ExtensionInfo
                    };
                }

                parameters.Add(parameter);
            }

            if (info.Attributes.ContainsKey("CompleteExtensionInfo"))
            {
                strCondition.Append("completeextensioninfo=@completeextensioninfo,");
                if (info.CompleteExtensionInfo == null)
                {
                    parameter = new SqlParameter("@completeextensioninfo", SqlDbType.NVarChar, 0)
                    {
                        Value = DBNull.Value
                    };
                }
                else
                {
                    parameter = new SqlParameter("@completeextensioninfo", SqlDbType.NVarChar, info.CompleteExtensionInfo.Length)
                    {
                        Value = info.CompleteExtensionInfo
                    };
                }

                parameters.Add(parameter);
            }

            if (info.Attributes.ContainsKey("Status"))
            {
                strCondition.Append("status=@status,");

                parameter = new SqlParameter("@status", SqlDbType.Int)
                {
                    Value = info.Status
                };

                parameters.Add(parameter);
            }

            if (strCondition.Length>0)
            {
                strCondition.Remove(strCondition.Length - 1, 1);
            }

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionFactory.CreateAllForStorge(), async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }
                await using (SqlCommand command = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                })
                {

                    command.CommandText = @"update MultipartStorgeInfo set {0} where [id]=@id";
                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = info.ID
                    };
                    command.Parameters.Add(parameter);

                    foreach(var parameterItem in parameters)
                    {
                        command.Parameters.Add(parameterItem);
                    }

                    await command.PrepareAsync();
                    await command.ExecuteNonQueryAsync();
                }

            });
        }
    }
}
