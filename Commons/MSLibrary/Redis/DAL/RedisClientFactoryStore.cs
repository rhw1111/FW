using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DAL;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Transaction;

namespace MSLibrary.Redis.DAL
{
    [Injection(InterfaceType = typeof(IRedisClientFactoryStore), Scope = InjectionScope.Singleton)]
    public class RedisClientFactoryStore : IRedisClientFactoryStore
    {
        private IRedisConnectionFactory _redisConnectionFactory;

        public RedisClientFactoryStore(IRedisConnectionFactory redisConnectionFactory)
        {
            _redisConnectionFactory = redisConnectionFactory;
        }
        public async Task<RedisClientFactory> QueryByName(string name)
        {
            RedisClientFactory factory = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _redisConnectionFactory.CreateReadForRedisClientFactory(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"SELECT {0} FROM [RedisClientFactory] WHERE name=@name;", StoreHelper.GetRedisClientFactoryStoreSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {
                    var parameter = new SqlParameter("@name", SqlDbType.VarChar, 150)
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
                            factory = new RedisClientFactory();
                            StoreHelper.SetRedisClientFactoryStoreSelectFields(factory, reader, string.Empty);
                        }
                        await reader.CloseAsync();
                    }
                }
            });
            return factory;
        }

        public RedisClientFactory QueryByNameSync(string name)
        {
            RedisClientFactory factory = null;
            DBTransactionHelper.SqlTransactionWork(DBTypes.SqlServer, true, false, _redisConnectionFactory.CreateReadForRedisClientFactory(),  (conn, transaction) =>
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
                    CommandText = string.Format(@"SELECT {0} FROM [RedisClientFactory] WHERE name=@name;", StoreHelper.GetRedisClientFactoryStoreSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {
                    var parameter = new SqlParameter("@name", SqlDbType.VarChar, 150)
                    {
                        Value = name
                    };
                    command.Parameters.Add(parameter);
                    command.Prepare();
                    SqlDataReader reader = null;

                    using (reader = command.ExecuteReader())
                    {
                        if ( reader.Read())
                        {
                            factory = new RedisClientFactory();
                            StoreHelper.SetRedisClientFactoryStoreSelectFields(factory, reader, string.Empty);
                        }
                        reader.Close();
                    }
                }
            });
            return factory;
        }
    }
}
