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
    [Injection(InterfaceType = typeof(IEntityInfoKeyRelationStore), Scope = InjectionScope.Singleton)]
    public class EntityInfoKeyRelationStore : IEntityInfoKeyRelationStore
    {
        private IEntityMetadataConnectionFactory _entityMetadataConnectionFactory;

        public EntityInfoKeyRelationStore(IEntityMetadataConnectionFactory entityMetadataConnectionFactory)
        {
            _entityMetadataConnectionFactory = entityMetadataConnectionFactory;
        }

        /// <summary>
        /// 根据实体元数据ID查询该实体元数据关联的全部记录
        /// </summary>
        /// <param name="infoId"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task QueryAllByEntityInfoId(Guid infoId, Func<EntityInfoKeyRelation, Task> callback)
        {
            List<EntityInfoKeyRelation> itemList = new List<EntityInfoKeyRelation>();

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
                            commond.CommandText = string.Format(@"SELECT TOP (@pagesize) {0},{1},{2}
                                                                    FROM [EntityInfoKeyRelation] AS keyrelation INNER JOIN [EntityInfo] AS info ON keyrelation.[entityinfoid] = info.[id]
                                                                        INNER JOIN [EntityAttributeInfo] AS attribute ON keyrelation.[entityattributeinfoid] = attribute.[id]
                                                                    WHERE info.[id] = @id ORDER BY keyrelation.[order];",
                                                                    StoreHelper.GetEntityInfoKeyRelationSelectFields("keyrelation"),
                                                                    StoreHelper.GetEntityInfoSelectFields("info"),
                                                                    StoreHelper.GetEntityAttributeInfoSelectFields("attribute"));
                        }
                        else
                        {
                            commond.CommandText = string.Format(@"SELECT TOP (@pagesize) {0},{1},{2}
                                                                    FROM [EntityInfoKeyRelation] AS keyrelation INNER JOIN [EntityInfo] AS info ON keyrelation.[entityinfoid] = info.[id]
                                                                        INNER JOIN [EntityAttributeInfo] AS attribute ON keyrelation.[entityattributeinfoid] = attribute.[id]
                                                                    WHERE info.[id] = @id AND keyrelation.[order] > @sequence ORDER BY keyrelation.[order];",
                                                                    StoreHelper.GetEntityInfoKeyRelationSelectFields("keyrelation"),
                                                                    StoreHelper.GetEntityInfoSelectFields("info"),
                                                                    StoreHelper.GetEntityAttributeInfoSelectFields("attribute"));
                        }
                        var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                        {
                            Value = infoId
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
                                var item = new EntityInfoKeyRelation();
                                StoreHelper.SetEntityInfoKeyRelationSelectFields(item, reader, "keyrelation");
                                sequence = (int)reader["keyrelationorder"];
                                item.EntityInfo = new EntityInfo();
                                StoreHelper.SetEntityInfoSelectFields(item.EntityInfo, reader, "info");
                                item.EntityAttributeInfo = new EntityAttributeInfo();
                                StoreHelper.SetEntityAttributeInfoSelectFields(item.EntityAttributeInfo, reader, "attribute");
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
