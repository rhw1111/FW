using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DAL;
using MSLibrary.DI;
using MSLibrary.Transaction;

namespace MSLibrary.EntityMetadata.DAL
{
    [Injection(InterfaceType = typeof(IEntityInfoAlternateKeyStore), Scope = InjectionScope.Singleton)]
    public class EntityInfoAlternateKeyStore : IEntityInfoAlternateKeyStore
    {
        private IEntityMetadataConnectionFactory _entityMetadataConnectionFactory;

        public EntityInfoAlternateKeyStore(IEntityMetadataConnectionFactory entityMetadataConnectionFactory)
        {
            _entityMetadataConnectionFactory = entityMetadataConnectionFactory;
        }

        /// <summary>
        /// 根据实体元数据Id和备用关键字名称查询备用关键字
        /// </summary>
        /// <param name="infoId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<EntityInfoAlternateKey> QueryByEntityInfoIdAndName(Guid infoId, string name)
        {
            EntityInfoAlternateKey info = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _entityMetadataConnectionFactory.CreateReadForEntityMetadata(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"SELECT {0},{1}
                                                    FROM [EntityInfoAlternateKey] AS ekey
                                                        INNER JOIN [EntityInfo] AS info
                                                            ON ekey.[entityinfoid] = info.[id]
                                                    WHERE info.[id] = @infoid
                                                          AND ekey.[name] = @name;", StoreHelper.GetEntityInfoAlternateKeySelectFields("ekey"), StoreHelper.GetEntityInfoSelectFields("info"))
                })
                {
                    var parameter = new SqlParameter("@infoid", SqlDbType.UniqueIdentifier)
                    {
                        Value = infoId
                    };
                    commond.Parameters.Add(parameter);
                    parameter = new SqlParameter("@name", SqlDbType.NVarChar, 256)
                    {
                        Value = name
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();
                    SqlDataReader reader = null;

                    await using (reader= await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            info = new EntityInfoAlternateKey();
                            StoreHelper.SetEntityInfoAlternateKeySelectFields(info, reader, "ekey");
                            info.EntityInfo = new EntityInfo();
                            StoreHelper.SetEntityInfoSelectFields(info.EntityInfo, reader, "info");
                        }
                        await reader.CloseAsync();
                    }
                }
            });

            return info;
        }
    }
}
