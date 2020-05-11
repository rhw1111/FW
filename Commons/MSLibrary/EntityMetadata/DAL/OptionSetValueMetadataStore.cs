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

namespace MSLibrary.EntityMetadata.DAL
{
    [Injection(InterfaceType = typeof(IOptionSetValueMetadataStore), Scope = InjectionScope.Singleton)]
    public class OptionSetValueMetadataStore : IOptionSetValueMetadataStore
    {
        private IEntityMetadataConnectionFactory _entityMetadataConnectionFactory;

        public OptionSetValueMetadataStore(IEntityMetadataConnectionFactory entityMetadataConnectionFactory)
        {
            _entityMetadataConnectionFactory = entityMetadataConnectionFactory;
        }

        public async Task Add(OptionSetValueMetadata metadata)
        {
            //获取读写连接字符串
            var strConn = _entityMetadataConnectionFactory.CreateAllForEntityMetadata();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, strConn, async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand commond = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran
                })
                {

                    if (metadata.ID == Guid.Empty)
                    {
                        commond.CommandText = @"insert into OptionSetValueMetadata([id],[name],[createtime],[modifytime])
                                    values(default,@name,getutcdate(),getutcdate());
                                    select @newid=[id] from OptionSetValueMetadata where [sequence]=SCOPE_IDENTITY()";
                    }
                    else
                    {
                        commond.CommandText = @"insert into OptionSetValueMetadata([id],[name],[createtime],[modifytime])
                                    values(@id,@name,getutcdate(),getutcdate())";
                    }

                    SqlParameter parameter;
                    if (metadata.ID != Guid.Empty)
                    {
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = metadata.ID
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
                        Value = metadata.Name
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();



                    await commond.ExecuteNonQueryAsync();
                


                    if (metadata.ID == Guid.Empty)
                    {
                        metadata.ID = (Guid)commond.Parameters["@newid"].Value;
                    }
                }
            });

        }

        public async Task Delete(Guid id)
        {
            //获取读写连接字符串
            var strConn = _entityMetadataConnectionFactory.CreateAllForEntityMetadata();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, strConn, async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand commond = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = @"
                                    declare @delnum int
                                    set @delnum=1000
                                    while @delnum=1000
                                    begin
                                        set @delnum=0
                                        select top 1000 @delnum=@delnum+1 from [dbo].[OptionSetValueItem] WITH (SNAPSHOT)
                                        where [optionsetvalueid]=@id
                                        delete from [dbo].[OptionSetValueItem] WITH (SNAPSHOT) from [dbo].[OptionSetValueItem] WITH (SNAPSHOT) join 
                                        (
                                            select top 1000 [id] from [dbo].[OptionSetValueItem] WITH (SNAPSHOT)
                                            where [optionsetvalueid]=@id
                                        ) as d
                                        on [dbo].[OptionSetValueItem].[id]=d.[id]
   
                                    end

                                    delete from [dbo].[OptionSetValueMetadata] where id=@id"
                })
                {

                    SqlParameter parameter;

                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();
                    


                }
            });
        }

        public async Task<OptionSetValueMetadata> QueryById(Guid id)
        {
            OptionSetValueMetadata metadata = null;

            //获取只读连接字符串
            var strConn = _entityMetadataConnectionFactory.CreateReadForEntityMetadata();


            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, strConn, async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand commond = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = string.Format(@"select {0} from OptionSetValueMetadata where [id]=@id", StoreHelper.GetOptionSetValueMetadataSelectFields(string.Empty))
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    SqlDataReader reader = null;




                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            metadata = new OptionSetValueMetadata();
                            StoreHelper.SetOptionSetValueMetadataSelectFields(metadata, reader, string.Empty);
                        }

                        reader.Close();
                    }
                }
            });

            return metadata;
        }

        public async Task<QueryResult<OptionSetValueMetadata>> QueryByName(string name, int page, int pageSize)
        {
            QueryResult<OptionSetValueMetadata> result = new QueryResult<OptionSetValueMetadata>();

            //获取只读连接字符串
            var strConn = _entityMetadataConnectionFactory.CreateReadForEntityMetadata();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, strConn, async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand commond = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = string.Format(@"set @currentpage=@page
		                           select @count= count(*) from OptionSetValueMetadata where [name] like @name
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
	
                                    select {0} from OptionSetValueMetadata where [name] like @name
                                    order by sequence
		                            offset (@pagesize * (@currentpage - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetOptionSetValueMetadataSelectFields(string.Empty))
                })
                {
                    var parameter = new SqlParameter("@name", SqlDbType.NVarChar, 150)
                    {
                        Value = $"{name.ToSqlLike()}%"
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

                    await commond.PrepareAsync();


                    SqlDataReader reader = null;

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var metadata = new OptionSetValueMetadata();
                            StoreHelper.SetOptionSetValueMetadataSelectFields(metadata, reader, string.Empty);
                            result.Results.Add(metadata);
                        }

                        await reader.CloseAsync();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;
                    }
                }
            });

            return result;
        }

        public async Task<OptionSetValueMetadata> QueryByName(string name)
        {
            OptionSetValueMetadata metadata = null;

            //获取只读连接字符串
            var strConn = _entityMetadataConnectionFactory.CreateReadForEntityMetadata();


            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, strConn, async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand commond = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = string.Format(@"select {0} from OptionSetValueMetadata where [name]=@name", StoreHelper.GetOptionSetValueMetadataSelectFields(string.Empty))
                })
                {

                    var parameter = new SqlParameter("@name", SqlDbType.NVarChar, 100)
                    {
                        Value = name
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    SqlDataReader reader = null;

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            metadata = new OptionSetValueMetadata();
                            StoreHelper.SetOptionSetValueMetadataSelectFields(metadata, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return metadata;
        }

        public async Task Update(OptionSetValueMetadata metadata)
        {
            //获取读写连接字符串
            var strConn = _entityMetadataConnectionFactory.CreateAllForEntityMetadata();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, strConn, async (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                await using (SqlCommand commond = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = @"update OptionSetValueMetadata set [name]=@name,[modifytime]=getutcdate()
                                    where [id]=@id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = metadata.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 100)
                    {
                        Value = metadata.Name
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();

                }
            });
        }
    }
}
