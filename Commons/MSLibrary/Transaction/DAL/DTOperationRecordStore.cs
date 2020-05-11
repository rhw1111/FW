using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using MSLibrary.DI;
using MSLibrary.DAL;
using MSLibrary.Collections.Hash;
using MSLibrary.LanguageTranslate;
using MSLibrary.Serializer;
using MSLibrary.Storge;

namespace MSLibrary.Transaction.DAL
{
    [Injection(InterfaceType = typeof(IDTOperationRecordStore), Scope = InjectionScope.Singleton)]
    public class DTOperationRecordStore : IDTOperationRecordStore
    {
        private const string _entityName = "DTOperationRecord";
        private ITransactionConnectionFactory _transactionConnectionFactory;
        private IStoreGroupRepositoryCacheProxy _storeGroupRepositoryCacheProxy;



        public DTOperationRecordStore(ITransactionConnectionFactory transactionConnectionFactory, IStoreGroupRepositoryCacheProxy storeGroupRepositoryCacheProxy)
        {
            _transactionConnectionFactory = transactionConnectionFactory;
            _storeGroupRepositoryCacheProxy = storeGroupRepositoryCacheProxy;
        }


        private async Task<(DBConnectionNames,string)> getStoreInfo(string groupName,string hashInfo)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                return (null, null);
            }

            var group = await _storeGroupRepositoryCacheProxy.QueryByName(groupName);

            if (group == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFounStoreGroupMemberByName,
                    DefaultFormatting = "找不到名称为{0}的存储组",
                    ReplaceParameters = new List<object>() { groupName }
                };

                throw new UtilityException((int)Errors.NotFounStoreGroupByName, fragment);
            }
            //找到该记录的实际存储信息
            var groupMember = await group.ChooseMember(hashInfo);
            var storeInfo = await groupMember.GetStoreInfo<StoreInfo>();

            if (storeInfo.DBConnectionNames == null || storeInfo.TableNames == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.StoreGroupMemberInfoTypeError,
                    DefaultFormatting = "名称为{0}的存储组中名称为{1}的组成员的存储信息不是需要的类型{2}",
                    ReplaceParameters = new List<object>() { groupName, groupMember.Name, typeof(StoreInfo).ToString() }
                };

                throw new UtilityException((int)Errors.StoreGroupMemberInfoTypeError, fragment);
            }

            if (!storeInfo.TableNames.TryGetValue(_entityName, out string tableName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundEntityNameInStoreInfoFromStoreGroup,
                    DefaultFormatting = "名称为{0}的存储组中名称为{1}的组成员的存储信息中缺少实体名称为{2}的实体表映射",
                    ReplaceParameters = new List<object>() { groupName, groupMember.Name, _entityName }
                };

                throw new UtilityException((int)Errors.NotFoundEntityNameInStoreInfoFromStoreGroup, fragment);
            }

            return (storeInfo.DBConnectionNames, tableName);
        }

        public async Task Add(DTOperationRecord record)
        {
            string strConn = _transactionConnectionFactory.CreateAllForDTOperationRecord();

            var (connNames, tableName) = await getStoreInfo(record.StoreGroupName, record.HashInfo);
            if (connNames != null)
            {
                strConn = _transactionConnectionFactory.CreateAllForDTOperationRecord(connNames);
            }
            else
            {
                tableName = "DTOperationRecord";
            }



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
                    if (record.ID == Guid.Empty)
                    {
                        commond.CommandText = string.Format(@"insert into {0}([id],[uniquename],[type],[storegroupname],[hashinfo],[version],[errormessge],[status],[timeout],[createtime],[modifytime])
                                    values(default,@uniquename,@type,@storegroupname,@hashinfo,@version,'',@status,@timeout,getutcdate(),getutcdate());
                                    select @newid=[id] from {0} where [sequence]=SCOPE_IDENTITY()", tableName);
                    }
                    else
                    {
                        commond.CommandText = string.Format(@"insert into {0}([id],[uniquename],[type],[storegroupname],[hashinfo],[version],[errormessage],[status],[timeout],[createtime],[modifytime])
                                    values(@id,@uniquename,@type,@storegroupname,@hashinfo,@version,'',@status,@timeout,getutcdate(),getutcdate())", tableName);
                    }

                    SqlParameter parameter;
                    if (record.ID != Guid.Empty)
                    {
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = record.ID
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

                    parameter = new SqlParameter("@uniquename", SqlDbType.VarChar, 150)
                    {
                        Value = record.UniqueName
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@type", SqlDbType.VarChar, 150)
                    {
                        Value = record.Type
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@storegroupname", SqlDbType.VarChar, 150)
                    {
                        Value = record.StoreGroupName
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@hashinfo", SqlDbType.VarChar, 300)
                    {
                        Value = record.HashInfo
                    };
                    commond.Parameters.Add(parameter);



                    parameter = new SqlParameter("@version", SqlDbType.VarChar, 150)
                    {
                        Value = record.Version
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@status", SqlDbType.Int)
                    {
                        Value = record.Status
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@timeout", SqlDbType.Int)
                    {
                        Value = record.Timeout
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();

                    if (record.ID == Guid.Empty)
                    {
                        record.ID = (Guid)commond.Parameters["@newid"].Value;
                    }
                }
            });
        }

        public async Task Delete(string storeGroupName, string hashInfo, Guid id)
        {
            string strConn = _transactionConnectionFactory.CreateAllForDTOperationRecord();

            var (connNames, tableName) = await getStoreInfo(storeGroupName, hashInfo);
            if (connNames != null)
            {
                strConn = _transactionConnectionFactory.CreateAllForDTOperationRecord(connNames);
            }
            else
            {
                tableName = "DTOperationRecord";
            }


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
                    CommandText = string.Format(@"delete from {0} where [id]=@id", tableName)
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();

                }
            });
        }

        public async Task<DTOperationRecord> QueryByID(string storeGroupName, string hashInfo, Guid id)
        {
            string strConn = _transactionConnectionFactory.CreateAllForDTOperationRecord();

            var (connNames, tableName) = await getStoreInfo(storeGroupName, hashInfo);
            if (connNames != null)
            {
                strConn = _transactionConnectionFactory.CreateAllForDTOperationRecord(connNames);
            }
            else
            {
                tableName = "DTOperationRecord";
            }

            DTOperationRecord record = null;

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
                    CommandText = string.Format(@"select {0} from {1} where [id]=@id", StoreHelper.GetDTOperationRecordSelectFields(string.Empty),tableName)
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
                            record = new DTOperationRecord();
                            StoreHelper.SetDTOperationRecordSelectFields(record, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return record;
        }

        public async Task<DTOperationRecord> QueryByIDNoLock(string storeGroupName, string hashInfo, Guid id)
        {
            string strConn = _transactionConnectionFactory.CreateAllForDTOperationRecord();

            var (connNames, tableName) = await getStoreInfo(storeGroupName, hashInfo);
            if (connNames != null)
            {
                strConn = _transactionConnectionFactory.CreateAllForDTOperationRecord(connNames);
            }
            else
            {
                tableName = "DTOperationRecord";
            }

            DTOperationRecord record = null;

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
                    CommandText = string.Format(@"select {0} from {1} WITH(NOLOCK) where [id]=@id", StoreHelper.GetDTOperationRecordSelectFields(string.Empty),tableName)
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
                            record = new DTOperationRecord();
                            StoreHelper.SetDTOperationRecordSelectFields(record, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return record;
        }

        public async Task<List<DTOperationRecord>> QueryBySkip(string storeInfo, int skip, int take)
        {
            var info= JsonSerializerHelper.Deserialize<StoreInfo>(storeInfo);
            if (info.DBConnectionNames==null || info.TableNames==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.StoreInfoTypeError,
                    DefaultFormatting = "存储信息{0}要求的格式为{1}，发生位置{2}",
                    ReplaceParameters = new List<object>() { storeInfo, typeof(StoreInfo).ToString(),$"{this.GetType().FullName}.QueryBySkip" }
                };

                throw new UtilityException((int)Errors.StoreInfoTypeError, fragment);
            }

            if (!info.TableNames.TryGetValue(_entityName, out string tableName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundEntityNameInStoreInfo,
                    DefaultFormatting = "存储信息{0}中缺少实体名称为{1}的实体表映射",
                    ReplaceParameters = new List<object>() { storeInfo, _entityName }
                };

                throw new UtilityException((int)Errors.NotFoundEntityNameInStoreInfo, fragment);
            }


            List<DTOperationRecord> dataList = new List<DTOperationRecord>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _transactionConnectionFactory.CreateAllForDTOperationRecord(info.DBConnectionNames), async (conn, transaction) =>
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

                       commond.CommandText = string.Format(@"select {0} from {1} order by [sequence] OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY", StoreHelper.GetDTOperationRecordSelectFields(string.Empty));


                        var parameter = new SqlParameter("@skip", SqlDbType.Int)
                        {
                            Value = skip
                        };
                        commond.Parameters.Add(parameter);

                        parameter = new SqlParameter("@take", SqlDbType.Int)
                        {
                            Value = take
                        };
                        commond.Parameters.Add(parameter);


                        await commond.PrepareAsync();


                        SqlDataReader reader = null;

                        await using (reader = await commond.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var data = new DTOperationRecord();
                                StoreHelper.SetDTOperationRecordSelectFields(data, reader, string.Empty);
                                dataList.Add(data);
                            }

                            await reader.CloseAsync();
                        }


                    }


                

            });

            return dataList;
        }

        public async Task UpdateStatus(string storeGroupName, string hashInfo, Guid id, int status)
        {
            string strConn = _transactionConnectionFactory.CreateAllForDTOperationRecord();

            var (connNames, tableName) = await getStoreInfo(storeGroupName, hashInfo);
            if (connNames != null)
            {
                strConn = _transactionConnectionFactory.CreateAllForDTOperationRecord(connNames);
            }
            else
            {
                tableName = "DTOperationRecord";
            }

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
                    CommandText = string.Format(@"update {0} set [status]=@status,[modifytime]=getutcdate()
                                    where [id]=@id",tableName)
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@status", SqlDbType.Int)
                    {
                        Value = status
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();

                }
            });
        }

        public async Task UpdateVersion(string storeGroupName, string hashInfo, Guid id, string version)
        {
            string strConn = _transactionConnectionFactory.CreateAllForDTOperationRecord();

            var (connNames, tableName) = await getStoreInfo(storeGroupName, hashInfo);
            if (connNames != null)
            {
                strConn = _transactionConnectionFactory.CreateAllForDTOperationRecord(connNames);
            }
            else
            {
                tableName = "DTOperationRecord";
            }

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
                    CommandText = string.Format(@"update {0} set [version]=@version,[modifytime]=getutcdate()
                                    where [id]=@id",tableName)
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@version", SqlDbType.Int)
                    {
                        Value = version
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();

                }
            });
        }

        public async Task<DTOperationRecord> QueryByUniqueName(string storeGroupName, string hashInfo, string uniqueName)
        {
            string strConn = _transactionConnectionFactory.CreateAllForDTOperationRecord();

            var (connNames, tableName) = await getStoreInfo(storeGroupName, hashInfo);
            if (connNames != null)
            {
                strConn = _transactionConnectionFactory.CreateAllForDTOperationRecord(connNames);
            }
            else
            {
                tableName = "DTOperationRecord";
            }

            DTOperationRecord record = null;

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
                    CommandText = string.Format(@"select {0} from {1} where [uniquename]=@uniquename", StoreHelper.GetDTOperationRecordSelectFields(string.Empty), tableName)
                })
                {

                    var parameter = new SqlParameter("@uniquename", SqlDbType.VarChar,150)
                    {
                        Value = uniqueName
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    SqlDataReader reader = null;

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            record = new DTOperationRecord();
                            StoreHelper.SetDTOperationRecordSelectFields(record, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return record;
        }

        public async Task UpdateErroeMessage(string storeGroupName, string hashInfo, Guid id, string errorMessage)
        {
            string strConn = _transactionConnectionFactory.CreateAllForDTOperationRecord();

            var (connNames, tableName) = await getStoreInfo(storeGroupName, hashInfo);
            if (connNames != null)
            {
                strConn = _transactionConnectionFactory.CreateAllForDTOperationRecord(connNames);
            }
            else
            {
                tableName = "DTOperationRecord";
            }

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
                    CommandText = string.Format(@"update {0} set [errormessage]=@errormessage,[modifytime]=getutcdate()
                                    where [id]=@id", tableName)
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@errormessage", SqlDbType.NVarChar,errorMessage.Length)
                    {
                        Value = errorMessage
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();

                }
            });
        }

        public async Task<List<DTOperationRecord>> QueryBySkip(string storeInfo, int status, int skip, int take)
        {
            var info = JsonSerializerHelper.Deserialize<StoreInfo>(storeInfo);
            if (info.DBConnectionNames == null || info.TableNames == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.StoreInfoTypeError,
                    DefaultFormatting = "存储信息{0}要求的格式为{1}，发生位置{2}",
                    ReplaceParameters = new List<object>() { storeInfo, typeof(StoreInfo).ToString(), $"{this.GetType().FullName}.QueryBySkip" }
                };

                throw new UtilityException((int)Errors.StoreInfoTypeError, fragment);
            }

            if (!info.TableNames.TryGetValue(_entityName, out string tableName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundEntityNameInStoreInfo,
                    DefaultFormatting = "存储信息{0}中缺少实体名称为{1}的实体表映射",
                    ReplaceParameters = new List<object>() { storeInfo, _entityName }
                };

                throw new UtilityException((int)Errors.NotFoundEntityNameInStoreInfo, fragment);
            }


            List<DTOperationRecord> dataList = new List<DTOperationRecord>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _transactionConnectionFactory.CreateAllForDTOperationRecord(info.DBConnectionNames), async (conn, transaction) =>
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

                    commond.CommandText = string.Format(@"select {0} from {1} where [status]=@status order by [sequence] OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY", StoreHelper.GetDTOperationRecordSelectFields(string.Empty));


                    var parameter = new SqlParameter("@skip", SqlDbType.Int)
                    {
                        Value = skip
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@take", SqlDbType.Int)
                    {
                        Value = take
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@status", SqlDbType.Int)
                    {
                        Value = status
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();


                    SqlDataReader reader = null;

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var data = new DTOperationRecord();
                            StoreHelper.SetDTOperationRecordSelectFields(data, reader, string.Empty);
                            dataList.Add(data);
                        }

                        await reader.CloseAsync();
                    }


                }




            });

            return dataList;
        }
    }
}
