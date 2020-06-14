using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Transaction;
using MSLibrary.DAL;
using MSLibrary.Configuration;
using MSLibrary.Configuration.DAL;
using MySql.Data.MySqlClient;
using MySqlConnector;


namespace MSLibrary.MySqlStore.Configuration.DAL
{
    [Injection(InterfaceType = typeof(ISystemConfigurationStore), Scope = InjectionScope.Singleton)]
    public class SystemConfigurationStore : ISystemConfigurationStore
    {
        private ISystemConfigurationConnectionFactory _dbConnectionMainFactory;

        public SystemConfigurationStore(ISystemConfigurationConnectionFactory dbConnectionMainFactory)
        {
            _dbConnectionMainFactory = dbConnectionMainFactory;
        }

        public SystemConfiguration QueryByName(string name)
        {
            SystemConfiguration configuration = null;

            DBTransactionHelper.SqlTransactionWork(DBTypes.MySql, true, false, _dbConnectionMainFactory.CreateReadForSystemConfiguration(), (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }

                using (MySqlCommand commond = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"select {0} from SystemConfiguration where name=@name", NStoreHelper.GetSystemConfigurationSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {

                    var parameter = new MySqlParameter("@name", MySqlDbType.VarChar, 100)
                    {
                        Value = name
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();


                    MySqlDataReader reader = null;

                    using (reader = commond.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            configuration = new SystemConfiguration();
                            NStoreHelper.SetSystemConfigurationSelectFields(configuration, reader, string.Empty);
                        }

                        reader.Close();
                    }
                }
            });


            return configuration;
        }

        public async Task UpdateContent(SystemConfiguration configuration)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _dbConnectionMainFactory.CreateAllForSystemConfiguration(), async (conn, transaction) =>
            {
                MySqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (MySqlTransaction)transaction;
                }

                await using (MySqlCommand commond = new MySqlCommand()
                {
                    Connection = (MySqlConnection)conn,
                    CommandType = CommandType.Text,
                    Transaction = sqlTran,
                    CommandText = @"update systemconfiguration set content=@content,modifytime=utc_timestamp()
                                    where id=@id"
                })
                {

                    var parameter = new MySqlParameter("@id", MySqlDbType.Guid)
                    {
                        Value = configuration.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new MySqlParameter("@content", MySqlDbType.VarChar, 1000)
                    {
                        Value = configuration.Content
                    };
                    commond.Parameters.Add(parameter);


                    await commond.PrepareAsync();

                    await commond.ExecuteNonQueryAsync();
                }

            });
        }
    }
}
