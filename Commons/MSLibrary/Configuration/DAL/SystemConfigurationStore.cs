using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Transaction;
using MSLibrary.DAL;

namespace MSLibrary.Configuration.DAL
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
          
            DBTransactionHelper.SqlTransactionWork(DBTypes.SqlServer, true, false, _dbConnectionMainFactory.CreateReadForSystemConfiguration(),  (conn, transaction) =>
            {
                SqlTransaction sqlTran = null;
                if (transaction != null)
                {
                    sqlTran = (SqlTransaction)transaction;
                }

                using (SqlCommand commond = new SqlCommand()
                {
                    Connection = (SqlConnection)conn,
                    CommandType = CommandType.Text,
                    CommandText = string.Format(@"select {0} from SystemConfiguration where [name]=@name", StoreHelper.GetSystemConfigurationSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {

                    var parameter = new SqlParameter("@name", SqlDbType.VarChar,100)
                    {
                        Value = name
                    };
                    commond.Parameters.Add(parameter);

                    commond.Prepare();


                    SqlDataReader reader = null;

                    using (reader =  commond.ExecuteReader())
                    {

                        if ( reader.Read())
                        {
                            configuration = new SystemConfiguration();
                            StoreHelper.SetSystemConfigurationSelectFields(configuration, reader, string.Empty);
                        }

                        reader.Close();
                    }
                }
            });


            return configuration;

        }

        public async Task UpdateContent(SystemConfiguration configuration)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, false, false, _dbConnectionMainFactory.CreateAllForSystemConfiguration(), async (conn, transaction) =>
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
                    CommandText = @"update systemconfiguration set [content]=@content,[modifytime]=getutcdate()
                                    where [id]=@id"
                })
                {

                    var parameter = new SqlParameter("@id", SqlDbType.UniqueIdentifier)
                    {
                        Value = configuration.ID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@content", SqlDbType.NVarChar, configuration.Content.Length)
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
