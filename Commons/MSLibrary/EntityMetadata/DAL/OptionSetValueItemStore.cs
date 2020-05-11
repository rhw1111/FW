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
    [Injection(InterfaceType = typeof(IOptionSetValueItemStore), Scope = InjectionScope.Singleton)]
    public class OptionSetValueItemStore : IOptionSetValueItemStore
    {
        private IEntityMetadataConnectionFactory _entityMetadataConnectionFactory;

        public OptionSetValueItemStore(IEntityMetadataConnectionFactory entityMetadataConnectionFactory)
        {
            _entityMetadataConnectionFactory = entityMetadataConnectionFactory;
        }

        public async Task Add(OptionSetValueItem item)
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

                    if (item.ID == Guid.Empty)
                    {
                        commond.CommandText = @"insert into OptionSetValueItem([id],[optionsetvalueid],[value],[stringvalue],[defaultlabel],[createtime],[modifytime])
                                    values(default,@optionsetvalueid,@value,@stringvalue,@defaultlabel,getutcdate(),getutcdate());
                                    select @newid=[id] from OptionSetValueItem where [sequence]=SCOPE_IDENTITY()";
                    }
                    else
                    {
                        commond.CommandText = @"insert into OptionSetValueItem([id],[optionsetvalueid],[value],[stringvalue],[defaultlabel],[createtime],[modifytime])
                                    values(@id,@optionsetvalueid,@value,@stringvalue,@defaultlabel,getutcdate(),getutcdate())";
                    }

                    SqlParameter parameter;
                    if (item.ID != Guid.Empty)
                    {
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = item.ID
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

                    parameter = new SqlParameter("@optionsetvalueid", SqlDbType.UniqueIdentifier)
                    {
                        Value = item.OptionSetValue.ID
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@value", SqlDbType.Int)
                    {
                        Value = item.Value
                    };
                    commond.Parameters.Add(parameter);

                    if (item.StringValue != null)
                    {
                        parameter = new SqlParameter("@stringvalue", SqlDbType.NVarChar, 100)
                        {
                            Value = item.StringValue
                        };
                    }
                    else
                    {
                        parameter = new SqlParameter("@stringvalue", SqlDbType.NVarChar, 100)
                        {
                            Value = DBNull.Value
                        };
                    }
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@defaultlabel", SqlDbType.NVarChar, 150)
                    {
                        Value = item.DefaultLabel
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();

                    if (item.ID == Guid.Empty)
                    {
                        item.ID = (Guid)commond.Parameters["@newid"].Value;
                    }
                }
            });
        }

        public async Task Delete(Guid optionSetValueId, Guid itemId)
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
                    CommandText = "delete from OptionSetValueItem where [optionsetvalueid]=@optionsetvalueid and [id]=@id"
                })
                {

                    SqlParameter parameter;

                    parameter = new SqlParameter("@optionsetvalueid", SqlDbType.UniqueIdentifier)
                    {
                        Value = optionSetValueId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = itemId
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();
                }
            });
        }

        public async Task QueryAll(Guid optionSetValueId, Func<OptionSetValueItem, Task> callback)
        {
            //获取只读连接字符串
            var strConn = _entityMetadataConnectionFactory.CreateReadForEntityMetadata();


            List<OptionSetValueItem> itemList = new List<OptionSetValueItem>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, strConn, async (conn, transaction) =>
            {
                Int64? sequence = null;
                int pageSize = 500;

                while (true)
                {
                    itemList.Clear();

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
                        if (!sequence.HasValue)
                        {
                            commond.CommandText = string.Format(@"select top (@pagesize) {0},{1} from OptionSetValueItem as item join OptionSetValueMetadata as metadata on item.optionsetvalueid=metadata.id where metadata.id=@optionsetvalueid order by item.[sequence]", StoreHelper.GetOptionSetValueItemSelectFields("item"), StoreHelper.GetOptionSetValueMetadataSelectFields("metadata"));
                        }
                        else
                        {
                            commond.CommandText = string.Format(@"select top (@pagesize) {0},{1} from OptionSetValueItem as item join OptionSetValueMetadata as metadata on listener.optionsetvalueid=metadata.id where metadata.id=@optionsetvalueid and item.[sequence]>@sequence order by item.[sequence]", StoreHelper.GetOptionSetValueItemSelectFields("item"), StoreHelper.GetOptionSetValueMetadataSelectFields("metadata"));
                        }

                        var parameter = new SqlParameter("@optionsetvalueid", SqlDbType.UniqueIdentifier)
                        {
                            Value = optionSetValueId
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

                        await commond.PrepareAsync();
                        SqlDataReader reader = null;

                        await using (reader = await commond.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var item = new OptionSetValueItem();
                                StoreHelper.SetOptionSetValueItemSelectFields(item, reader, "item");
                                sequence = (Int64)reader["itemsequence"];
                                item.OptionSetValue = new OptionSetValueMetadata();
                                StoreHelper.SetOptionSetValueMetadataSelectFields(item.OptionSetValue, reader, "metadata");
                                itemList.Add(item);
                            }
                            await reader.CloseAsync();
                        }


                    }

                    foreach (var item in itemList)
                    {
                        await callback(item);
                    }

                    if (itemList.Count != pageSize)
                    {
                        break;
                    }

                }

            });
        }

        public async Task<QueryResult<OptionSetValueItem>> QueryAll(Guid optionSetValueId, int page, int pageSize)
        {
            QueryResult<OptionSetValueItem> result = new QueryResult<OptionSetValueItem>();

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
		                           select @count= count(*) from OptionSetValueItem as item join OptionSetValueMetadata as metadata on item.optionsetvalueid=metadata.id where metadata.id=@optionsetvalueid
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
	
                                    select {0},{1} from OptionSetValueItem as item join OptionSetValueMetadata as metadata on item.optionsetvalueid=metadata.id where metadata.id=@optionsetvalueid
                                    order by sequence
		                            offset (@pagesize * (@currentpage - 1)) rows 
		                            fetch next @pagesize rows only;", StoreHelper.GetOptionSetValueItemSelectFields("item"), StoreHelper.GetOptionSetValueMetadataSelectFields("metadata"))
                })
                {
                    var parameter = new SqlParameter("@optionsetvalueid", SqlDbType.UniqueIdentifier)
                    {
                        Value = optionSetValueId
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
                            var item = new OptionSetValueItem();
                            StoreHelper.SetOptionSetValueItemSelectFields(item, reader, "item");
                            item.OptionSetValue = new OptionSetValueMetadata();
                            StoreHelper.SetOptionSetValueMetadataSelectFields(item.OptionSetValue, reader, "metadata");
                            result.Results.Add(item);
                        }

                        await reader.CloseAsync();

                        result.TotalCount = (int)commond.Parameters["@count"].Value;
                        result.CurrentPage = (int)commond.Parameters["@currentpage"].Value;
                    }
                }
            });

            return result;
        }

        public async Task QueryChildAll(Guid metadataId, Guid childMetadataId, Guid id, Func<OptionSetValueItem, Task> callback)
        {
            //获取只读连接字符串
            var strConn = _entityMetadataConnectionFactory.CreateReadForEntityMetadata();


            List<OptionSetValueItem> itemList = new List<OptionSetValueItem>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, strConn, async (conn, transaction) =>
            {
                Int64? sequence = null;
                int pageSize = 500;

                while (true)
                {
                    itemList.Clear();

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
                        if (!sequence.HasValue)
                        {
                            commond.CommandText = string.Format(@"select top (@pagesize) {0},{1} from OptionSetValueItem as item join OptionSetValueMetadata as metadata on item.optionsetvalueid=metadata.id join OptionSetValueItemRelation as itemR on item.id=itemR.fatherid join OptionSetValueItem as childItem on itemR.childid=childItem.id join OptionSetValueMetadataRelation as metadataR on metadata.id=metadataR.fatherid join OptionSetValueMetadata as childmetadata on metadataR.childid=childmetadata.id join OptionSetValueMetadata as childitemmetadata on childitemmetadata.id=childitem.optionsetvalueid where metadata.id=@metadataid and childitemmetadata.id=@childmetadataid and item.id=@id and childitemmetadata.id=childmetadata.id order by childItem.[sequence]", StoreHelper.GetOptionSetValueItemSelectFields("childItem"), StoreHelper.GetOptionSetValueMetadataSelectFields("childitemmetadata"));
                        }
                        else
                        {
                            commond.CommandText = string.Format(@"select top (@pagesize) {0},{1} from OptionSetValueItem as item join OptionSetValueMetadata as metadata on item.optionsetvalueid=metadata.id join OptionSetValueItemRelation as itemR on item.id=itemR.fatherid join OptionSetValueItem as childItem on itemR.childid=childItem.id join OptionSetValueMetadataRelation as metadataR on metadata.id=metadataR.fatherid join OptionSetValueMetadata as childmetadata on metadataR.childid=childmetadata.id join OptionSetValueMetadata as childitemmetadata on childitemmetadata.id=childitem.optionsetvalueid where metadata.id=@metadataid and childitemmetadata.id=@childmetadataid and item.id=@id and childitemmetadata.id=childmetadata.id and childItem.[sequence]>@sequence order by childItem.[sequence]", StoreHelper.GetOptionSetValueItemSelectFields("childItem"), StoreHelper.GetOptionSetValueMetadataSelectFields("childitemmetadata"));

                        }

                        var parameter = new SqlParameter("@metadataid", SqlDbType.UniqueIdentifier)
                        {
                            Value =metadataId
                        };
                        commond.Parameters.Add(parameter);

                        parameter = new SqlParameter("@childmetadataid", SqlDbType.UniqueIdentifier)
                        {
                            Value = childMetadataId
                        };
                        commond.Parameters.Add(parameter);

                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = id
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

                        await commond.PrepareAsync();
                        SqlDataReader reader = null;

                        await using (reader = await commond.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var item = new OptionSetValueItem();
                                StoreHelper.SetOptionSetValueItemSelectFields(item, reader, "childItem");
                                sequence = (Int64)reader["childitemsequence"];
                                item.OptionSetValue = new OptionSetValueMetadata();
                                StoreHelper.SetOptionSetValueMetadataSelectFields(item.OptionSetValue, reader, "childitemmetadata");
                                itemList.Add(item);
                            }
                            await reader.CloseAsync();
                        }


                    }

                    foreach (var item in itemList)
                    {
                        await callback(item);
                    }

                    if (itemList.Count != pageSize)
                    {
                        break;
                    }

                }

            });
        }

        public async Task<OptionSetValueItem> QueryById(Guid id)
        {
            OptionSetValueItem item = null;

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
                    CommandText = string.Format(@"select {0},{1} from OptionSetValueItem as item join OptionSetValueMetadata as metadata on item.optionsetvalueid=metadata.id where item.[id]=@id", StoreHelper.GetOptionSetValueMetadataSelectFields("metadata"),StoreHelper.GetOptionSetValueItemSelectFields("item"))
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
                            item = new OptionSetValueItem();
                            StoreHelper.SetOptionSetValueItemSelectFields(item, reader, "item");
                            item.OptionSetValue = new OptionSetValueMetadata();
                            StoreHelper.SetOptionSetValueMetadataSelectFields(item.OptionSetValue, reader, "metadata");

                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return item;
        }

        public async Task<OptionSetValueItem> QueryByValue(Guid optionSetValueId, int value)
        {
            OptionSetValueItem item = null;

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
                    CommandText = string.Format(@"select {0},{1} from OptionSetValueItem as item join OptionSetValueMetadata as metadata on item.optionsetvalueid=metadata.id where metadata.id=@optionsetvalueid and item.[value]=@value", StoreHelper.GetOptionSetValueItemSelectFields("item"), StoreHelper.GetOptionSetValueMetadataSelectFields("metadata"))
                })
                {

                    var parameter = new SqlParameter("@optionsetvalueid", SqlDbType.UniqueIdentifier)
                    {
                        Value = optionSetValueId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@value", SqlDbType.Int)
                    {
                        Value = value
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    SqlDataReader reader = null;

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            item = new OptionSetValueItem();
                            StoreHelper.SetOptionSetValueItemSelectFields(item, reader, "item");
                            item.OptionSetValue = new OptionSetValueMetadata();
                            StoreHelper.SetOptionSetValueMetadataSelectFields(item.OptionSetValue, reader, "metadata");

                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return item;
        }

        public async Task<OptionSetValueItem> QueryByValue(Guid optionSetValueId, string stringValue)
        {
            OptionSetValueItem item = null;

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
                    CommandText = string.Format(@"select {0},{1} from OptionSetValueItem as item join OptionSetValueMetadata as metadata on item.optionsetvalueid=metadata.id where metadata.id=@optionsetvalueid and item.[stringvalue]=@stringvalue", StoreHelper.GetOptionSetValueItemSelectFields("item"), StoreHelper.GetOptionSetValueMetadataSelectFields("metadata"))
                })
                {

                    var parameter = new SqlParameter("@optionsetvalueid", SqlDbType.UniqueIdentifier)
                    {
                        Value = optionSetValueId
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@stringvalue", SqlDbType.NVarChar, 100)
                    {
                        Value = stringValue
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();

                    SqlDataReader reader = null;

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            item = new OptionSetValueItem();
                            StoreHelper.SetOptionSetValueItemSelectFields(item, reader, "item");
                            item.OptionSetValue = new OptionSetValueMetadata();
                            StoreHelper.SetOptionSetValueMetadataSelectFields(item.OptionSetValue, reader, "metadata");

                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return item;
        }

        public async Task Update(OptionSetValueItem item)
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
                    CommandText = @"update OptionSetValueItem set [value]=@value,[stringvalue]=@stringvalue,[defaultlabel]=@defaultlabel,[modifytime]=getutcdate()
                                    where [optionsetvalueid]=@optionsetvalueid and [id]=@id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = item.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@optionsetvalueid", SqlDbType.UniqueIdentifier)
                    {
                        Value = item.OptionSetValue.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@value", SqlDbType.Int)
                    {
                        Value = item.Value
                    };
                    commond.Parameters.Add(parameter);

                    if (item.StringValue != null)
                    {
                        parameter = new SqlParameter("@stringvalue", SqlDbType.NVarChar, 100)
                        {
                            Value = item.StringValue
                        };
                    }
                    else
                    {
                        parameter = new SqlParameter("@stringvalue", SqlDbType.NVarChar, 100)
                        {
                            Value = DBNull.Value
                        };
                    }
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@defaultlabel", SqlDbType.NVarChar, 150)
                    {
                        Value = item.DefaultLabel
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();


                    await commond.ExecuteNonQueryAsync();


                }
            });
        }
    }
}
