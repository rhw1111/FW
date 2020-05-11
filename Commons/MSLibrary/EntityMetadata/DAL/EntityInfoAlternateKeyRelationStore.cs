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
    [Injection(InterfaceType = typeof(IEntityInfoAlternateKeyRelationStore), Scope = InjectionScope.Singleton)]
    public class EntityInfoAlternateKeyRelationStore : IEntityInfoAlternateKeyRelationStore
    {
        private IEntityMetadataConnectionFactory _entityMetadataConnectionFactory;

        public EntityInfoAlternateKeyRelationStore(IEntityMetadataConnectionFactory entityMetadataConnectionFactory)
        {
            _entityMetadataConnectionFactory = entityMetadataConnectionFactory;
        }


        /// <summary>
        /// 根据实体元树备用关键字Id查询全部记录
        /// </summary>
        /// <param name="alternateKeyId"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task QueryAllByAlternateKeyId(Guid alternateKeyId, Func<EntityInfoAlternateKeyRelation, Task> callback)
        {
            List<EntityInfoAlternateKeyRelation> itemList = new List<EntityInfoAlternateKeyRelation>();

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _entityMetadataConnectionFactory.CreateReadForEntityMetadata(), async (conn, transaction) =>
            {
                int? sequence = null;
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
                            commond.CommandText = string.Format(@"SELECT TOP (@pagesize) {0},{1},{2},{3}
                                                                    FROM [EntityInfoAlternateKeyRelation] AS relation 
                                                                        INNER JOIN [EntityInfoAlternateKey] AS ekey ON relation.[entityinfoAlternatekeyid] = ekey.[id]
                                                                        INNER JOIN [EntityAttributeInfo] AS attribute ON relation.[entityattributeinfoid] = attribute.[id]
                                                                        INNER JOIN [EntityInfo] AS info ON attribute.[entityinfoid] = info.[id]
                                                                    WHERE ekey.[id] = @alternateKeyId ORDER BY relation.[order];",
                                                                    StoreHelper.GetEntityInfoAlternateKeyRelationSelectFields("relation"),
                                                                    StoreHelper.GetEntityInfoAlternateKeySelectFields("ekey"),
                                                                    StoreHelper.GetEntityAttributeInfoSelectFields("attribute"),
                                                                    StoreHelper.GetEntityInfoSelectFields("info"));
                        }
                        else
                        {
                            commond.CommandText = string.Format(@"SELECT TOP (@pagesize) {0},{1},{2},{3}
                                                                    FROM [EntityInfoAlternateKeyRelation] AS relation 
                                                                        INNER JOIN [EntityInfoAlternateKey] AS ekey ON relation.[entityinfoAlternatekeyid] = ekey.[id]
                                                                        INNER JOIN [EntityAttributeInfo] AS attribute ON relation.[entityattributeinfoid] = attribute.[id]
                                                                        INNER JOIN [EntityInfo] AS info ON attribute.[entityinfoid] = info.[id]
                                                                    WHERE ekey.[id] = @alternateKeyId  AND relation.[order] > @sequence ORDER BY relation.[order];",
                                                                    StoreHelper.GetEntityInfoAlternateKeyRelationSelectFields("relation"),
                                                                    StoreHelper.GetEntityInfoAlternateKeySelectFields("ekey"),
                                                                    StoreHelper.GetEntityAttributeInfoSelectFields("attribute"),
                                                                    StoreHelper.GetEntityInfoSelectFields("info"));
                        }
                        var parameter = new SqlParameter("@alternateKeyId", SqlDbType.UniqueIdentifier)
                        {
                            Value = alternateKeyId
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
                                var item = new EntityInfoAlternateKeyRelation();
                                StoreHelper.SetEntityInfoAlternateKeyRelationSelectFields(item, reader, "relation");
                                sequence = (int)reader["relationorder"];
                                item.EntityInfoAlternateKey = new EntityInfoAlternateKey();
                                StoreHelper.SetEntityInfoAlternateKeySelectFields(item.EntityInfoAlternateKey, reader, "ekey");
                                item.EntityAttributeInfo = new EntityAttributeInfo();
                                StoreHelper.SetEntityAttributeInfoSelectFields(item.EntityAttributeInfo, reader, "attribute");
                                item.EntityAttributeInfo.EntityInfo = new EntityInfo();
                                StoreHelper.SetEntityInfoSelectFields(item.EntityAttributeInfo.EntityInfo, reader, "info");
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
    }
}
