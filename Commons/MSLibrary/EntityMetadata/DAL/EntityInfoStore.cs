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
    [Injection(InterfaceType = typeof(IEntityInfoStore), Scope = InjectionScope.Singleton)]
    public class EntityInfoStore : IEntityInfoStore
    {
        private IEntityMetadataConnectionFactory _entityMetadataConnectionFactory;

        public EntityInfoStore(IEntityMetadataConnectionFactory entityMetadataConnectionFactory)
        {
            _entityMetadataConnectionFactory = entityMetadataConnectionFactory;
        }
        /// <summary>
        /// 根据实体类型查询
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public async Task<EntityInfo> QueryByEntityType(string entityType)
        {
            EntityInfo info = null;
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
                    CommandText = string.Format(@"SELECT {0} FROM [EntityInfo] WHERE [entitytype] = @entitytype;", StoreHelper.GetEntityInfoSelectFields(string.Empty))
                })
                {
                    var parameter = new SqlParameter("@entitytype", SqlDbType.NVarChar, 256)
                    {
                        Value = entityType
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();
                    SqlDataReader reader = null;

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            info = new EntityInfo();
                            StoreHelper.SetEntityInfoSelectFields(info, reader, string.Empty);
                        }
                        await reader.CloseAsync();
                    }
                }
            });

            return info;
        }
    }
}
