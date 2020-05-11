using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using MSLibrary.DI;
using MSLibrary.DAL;
using MSLibrary.Collections.Hash;
using MSLibrary.LanguageTranslate;
using MSLibrary.Transaction;

namespace MSLibrary.SerialNumber.DAL
{
    /// <summary>
    /// 序列号记录数据操作的默认实现
    /// 使用一致性哈希组来散列记录，要求哈希节点的具有三种状态
    /// 0：新节点，1：原有节点，2：原有节点，但需要重新计算
    /// </summary>
    [Injection(InterfaceType = typeof(ISerialNumberRecordStore), Scope = InjectionScope.Singleton)]
    public class SerialNumberRecordStore : ISerialNumberRecordStore
    {
        private static string _hashGroupName;
        /// <summary>
        /// 需要用到的一致性哈希组的名称
        /// 需要在系统初始化时赋值
        /// </summary>
        public static string HashGroupName
        {
            set
            {
                _hashGroupName = value;
            }
        }



        private IHashGroupRepositoryCacheProxy _hashGroupRepository;
        private ISerialNumberConnectionFactory _serialNumberConnectionFactory;

        private IStoreInfoResolveService _storeInfoResolveService;
        public SerialNumberRecordStore(IHashGroupRepositoryCacheProxy hashGroupRepository, ISerialNumberConnectionFactory serialNumberConnectionFactory, IStoreInfoResolveService storeInfoResolveService)
        {
            _hashGroupRepository = hashGroupRepository;
            _serialNumberConnectionFactory = serialNumberConnectionFactory;
            _storeInfoResolveService = storeInfoResolveService;
        }

        public async Task Add(SerialNumberRecord record)
        {
            record.Value = 1;

            var storeInfo = await StoreInfoHelper.GetHashStoreInfo( _storeInfoResolveService,_hashGroupRepository, _hashGroupName, record.Prefix);

            if (!storeInfo.TableNames.TryGetValue(HashEntityNames.SerialNumber, out string tableName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.SerialNumber }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo,fragment);
            }

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, storeInfo.DBConnectionNames.ReadAndWrite, async (conn, transaction) =>
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
                        commond.CommandText = string.Format(@"insert into {0}([id],[prefix],[value],[createtime],[modifytime])
                                    values(default,@prefix,1,getutcdate(),getutcdate());
                                    select @newid=[id] from {0} where [sequence]=SCOPE_IDENTITY()", tableName);
                    }
                    else
                    {
                        commond.CommandText = string.Format(@"insert into {0}([id],[prefix],[value],[createtime],[modifytime])
                                    values(@id,@prefix,1,getutcdate(),getutcdate())", tableName);
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


                    parameter = new SqlParameter("@prefix", SqlDbType.NVarChar, 1000)
                    {
                        Value = record.Prefix
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();



                        try
                        {
                            await commond.ExecuteNonQueryAsync();
                        }
                        catch (SqlException ex)
                        {

                            if (ex.Number == 2601)
                            {
                                var fragment = new TextFragment()
                                {
                                    Code = TextCodes.ExistFoundSerialNumberRecordByPrefix,
                                    DefaultFormatting = "前缀为{0}的序列号记录已经存在",
                                    ReplaceParameters = new List<object>() { record.Prefix }
                                };

                                throw new UtilityException((int)Errors.ExistFoundSerialNumberRecordByPrefix,fragment);
                            }
                            else
                            {
                                throw;
                            }
                        }


                    if (record.ID == Guid.Empty)
                    {
                        record.ID = (Guid)commond.Parameters["@newid"].Value;
                    }

                }
            });

        }

        public async Task UpdateValue(SerialNumberRecord record)
        {
            var storeInfo = await StoreInfoHelper.GetHashStoreInfo( _storeInfoResolveService,_hashGroupRepository, _hashGroupName, record.Prefix);

            if (!storeInfo.TableNames.TryGetValue(HashEntityNames.SerialNumber, out string tableName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.SerialNumber }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, storeInfo.DBConnectionNames.ReadAndWrite, async (conn, transaction) =>
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
                    CommandText = string.Format(@"update {0} set [value]=[value]+1 output inserted.[value] where [id]=@id;", tableName)

                })
                {
                    SqlParameter parameter;
                    parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = record.ID
                    };
                    commond.Parameters.Add(parameter);
                    await commond.PrepareAsync();

                    long serialValue = 0;

                    serialValue = (long)await commond.ExecuteScalarAsync();


                    record.Value = serialValue;

                }
            });
        }




        public async Task<SerialNumberRecord> QueryByPrefix(string prefix)
        {
            SerialNumberRecord record = null;

            var storeInfo = await StoreInfoHelper.GetHashStoreInfo( _storeInfoResolveService,_hashGroupRepository, _hashGroupName, prefix);

            if (!storeInfo.TableNames.TryGetValue(HashEntityNames.SerialNumber, out string tableName))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundKeyInHashNodeKeyInfo,
                    DefaultFormatting = "哈希组{0}中的哈希节点关键信息中找不到键值{1}",
                    ReplaceParameters = new List<object>() { _hashGroupName, HashEntityNames.SerialNumber }
                };

                throw new UtilityException((int)Errors.NotFoundKeyInHashNodeKeyInfo, fragment);
            }

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, storeInfo.DBConnectionNames.Read, async (conn, transaction) =>
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
                    CommandText = string.Format(@"select {0} from {1} where [prefix]=@prefix", StoreHelper.GetSerialNumberRecordSelectFields(string.Empty), tableName)
                })
                {

                    var parameter = new SqlParameter("@prefix", SqlDbType.NVarChar, 1000)
                    {
                        Value = prefix
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();

                    SqlDataReader reader = null;

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            record = new SerialNumberRecord();
                            StoreHelper.SetSerialNumberRecordSelectFields(record, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return record;
        }
    }

    /// <summary>
    /// 序列号记录数据存储信息服务
    /// 负责解析文本，返回服务器信息和数据表信息
    /// </summary>
    public interface ISerialNumberRecordDBStoreInfoService
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<SerialNumberRecordDBStoreInfoResult> Execute(string info);
    }
    /// <summary>
    /// 序列号记录数据存储信息服务结果
    /// </summary>
    public class SerialNumberRecordDBStoreInfoResult
    {
        /// <summary>
        /// 服务器信息
        /// </summary>
        public string ServerInto { get; set; }
        /// <summary>
        /// 数据表信息
        /// </summary>
        public string TableInfo { get; set; }
    }
}
