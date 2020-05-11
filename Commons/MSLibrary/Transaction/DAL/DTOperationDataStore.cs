using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.DAL;
using MSLibrary.Serializer;
using MSLibrary.Storge;

namespace MSLibrary.Transaction.DAL
{
    [Injection(InterfaceType = typeof(IDTOperationDataStore), Scope = InjectionScope.Singleton)]
    public class DTOperationDataStore : IDTOperationDataStore
    {
        private const string _entityName = "DTOperationData";
        private ITransactionConnectionFactory _transactionConnectionFactory;
        private IStoreGroupRepositoryCacheProxy _storeGroupRepositoryCacheProxy;

        public DTOperationDataStore(ITransactionConnectionFactory transactionConnectionFactory, IStoreGroupRepositoryCacheProxy storeGroupRepositoryCacheProxy)
        {
            _transactionConnectionFactory = transactionConnectionFactory;
            _storeGroupRepositoryCacheProxy = storeGroupRepositoryCacheProxy;
        }


        public async Task Add(DTOperationData data)
        {
            string strConn = _transactionConnectionFactory.CreateAllForDTOperationData();
            
            var (connNames, tableName) = await getStoreInfo(data.StoreGroupName, data.HashInfo);
            if (connNames!=null)
            {
                strConn = _transactionConnectionFactory.CreateAllForDTOperationData(connNames);
            }
            else
            {
                tableName = "DTOperationData";
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
                    if (data.ID == Guid.Empty)
                    {
                        commond.CommandText = string.Format(@"insert into {0}([id],[recorduniquename],[name],[type],[data],[storegroupname],[hashinfo],[createtime],[modifytime])
                                    values(default,@recorduniquename,@name,@type,@data,@status,getutcdate(),getutcdate());
                                    select @newid=[id] from {0} where [sequence]=SCOPE_IDENTITY()", tableName);
                    }
                    else
                    {
                        commond.CommandText =string.Format(@"insert into {0}([id],[recorduniquename],[name],[type],[data],[storegroupname],[hashinfo],[createtime],[modifytime])
                                    values(@id,@recorduniquename,@name,@type,@data,@status,getutcdate(),getutcdate())", tableName);
                    }

                    SqlParameter parameter;
                    if (data.ID != Guid.Empty)
                    {
                        parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = data.ID
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

                    parameter = new SqlParameter("@recorduniquename", SqlDbType.VarChar, 150)
                    {
                        Value = data.RecordUniqueName
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@name", SqlDbType.VarChar, 150)
                    {
                        Value = data.Name
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@type", SqlDbType.VarChar, 150)
                    {
                        Value = data.Type
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@data", SqlDbType.NVarChar, data.Data.Length)
                    {
                        Value = data.Data
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@storegroupname", SqlDbType.VarChar, 150)
                    {
                        Value = data.StoreGroupName
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@hashinfo", SqlDbType.VarChar, 300)
                    {
                        Value = data.HashInfo
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@status", SqlDbType.Int)
                    {
                        Value = data.Status
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();

                    if (data.ID == Guid.Empty)
                    {
                        data.ID = (Guid)commond.Parameters["@newid"].Value;
                    }
                }
            });
        }

        public async Task Delete(string storeGroupName, string hashInfo,Guid id)
        {

            string strConn = _transactionConnectionFactory.CreateAllForDTOperationData();

            var (connNames, tableName) = await getStoreInfo(storeGroupName, hashInfo);
            if (connNames != null)
            {
                strConn = _transactionConnectionFactory.CreateAllForDTOperationData(connNames);
            }
            else
            {
                tableName = "DTOperationData";
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
                    CommandText = string.Format(@"delete from {0} where [id]=@id",tableName)
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

        public async Task<DTOperationData> QueryByID(string storeGroupName, string hashInfo,Guid id)
        {
            string strConn = _transactionConnectionFactory.CreateAllForDTOperationData();

            var (connNames, tableName) = await getStoreInfo(storeGroupName, hashInfo);
            if (connNames != null)
            {
                strConn = _transactionConnectionFactory.CreateAllForDTOperationData(connNames);
            }
            else
            {
                tableName = "DTOperationData";
            }


            DTOperationData data = null;

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
                    CommandText = string.Format(@"select {0} from {1} where [id]=@id", StoreHelper.GetDTOperationDataSelectFields(string.Empty),tableName)
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
                            data = new DTOperationData();
                            StoreHelper.SetDTOperationDataSelectFields(data, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return data;
        }

        public async Task QueryByRecordUniqueName(string storeGroupName, string hashInfo,string recordUniqueName, int status, Func<DTOperationData, Task> action)
        {
            string strConn = _transactionConnectionFactory.CreateAllForDTOperationData();

            var (connNames, tableName) = await getStoreInfo(storeGroupName, hashInfo);
            if (connNames != null)
            {
                strConn = _transactionConnectionFactory.CreateAllForDTOperationData(connNames);
            }
            else
            {
                tableName = "DTOperationData";
            }

            List<DTOperationData> dataList = new List<DTOperationData>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, strConn, async (conn, transaction) =>
            {
                Int64? sequence = null;
                int pageSize = 500;

                while (true)
                {
                    dataList.Clear();

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
                            commond.CommandText = string.Format(@"select top (@pagesize) {0} from {1} where status=@status and recorduniquename=@recorduniquename order by [sequence]", StoreHelper.GetDTOperationDataSelectFields(string.Empty),tableName);
                        }
                        else
                        {
                            commond.CommandText = string.Format(@"select top (@pagesize) {0} from {1} where status=@status and recorduniquename=@recorduniquename and [sequence]>@sequence order by [sequence]", StoreHelper.GetDTOperationDataSelectFields(string.Empty),tableName);
                        }

                        var parameter = new SqlParameter("@status", SqlDbType.Int)
                        {
                            Value = status
                        };
                        commond.Parameters.Add(parameter);

                        parameter = new SqlParameter("@recorduniquename", SqlDbType.VarChar,150)
                        {
                            Value = recordUniqueName
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
                                var data = new DTOperationData();
                                StoreHelper.SetDTOperationDataSelectFields(data, reader, string.Empty);
                                sequence = (Int64)reader["sequence"];
                                dataList.Add(data);
                            }

                            await reader.CloseAsync();
                        }


                    }

                    foreach (var dataItem in dataList)
                    {
                        await action(dataItem);
                    }

                    if (dataList.Count != pageSize)
                    {
                        break;
                    }

                }

            });

        }

        public async Task<bool> UpdateStatus(string storeGroupName, string hashInfo,Guid id, byte[] version, int status)
        {
            string strConn = _transactionConnectionFactory.CreateAllForDTOperationData();

            var (connNames, tableName) = await getStoreInfo(storeGroupName, hashInfo);
            if (connNames != null)
            {
                strConn = _transactionConnectionFactory.CreateAllForDTOperationData(connNames);
            }
            else
            {
                tableName = "DTOperationData";
            }

            bool result = true;

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
                                    where [id]=@id and [version]=@version",tableName)
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@version", SqlDbType.Timestamp)
                    {
                        Value = version
                    };
                    commond.Parameters.Add(parameter);


                    parameter = new SqlParameter("@status", SqlDbType.Int)
                    {
                        Value = status
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                   var executeResult= await commond.ExecuteNonQueryAsync();
                    if (executeResult==0)
                    {
                        result = false;
                    }
                }
            });

            return result;
        }



        private async Task<(DBConnectionNames, string)> getStoreInfo(string groupName, string hashInfo)
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


    }
}
