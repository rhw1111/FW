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

namespace MSLibrary.Distribute.DAL
{
    [Injection(InterfaceType = typeof(IApplicationLockStore), Scope = InjectionScope.Singleton)]
    public class ApplicationLockStore : IApplicationLockStore
    {
        private IDistributeConnectionFactory _distributeConnectionFactory;

        public ApplicationLockStore(IDistributeConnectionFactory distributeConnectionFactory)
        {
            _distributeConnectionFactory = distributeConnectionFactory;
        }
        public async Task<ApplicationLock> QueryByName(string name)
        {
            ApplicationLock aLock = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _distributeConnectionFactory.CreateReadForDistributeCoordinator(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"SELECT {0} FROM [ApplicationLock] WHERE name=@name;", StoreHelper.GetApplicationLockStoreSelectFields(string.Empty)),
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

                    await using (reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            aLock = new ApplicationLock();
                            StoreHelper.SetApplicationLockStoreSelectFields(aLock, reader, string.Empty);
                        }
                        await reader.CloseAsync();
                    }
                }
            });
            return aLock;
        }

        public ApplicationLock QueryByNameSync(string name)
        {
            ApplicationLock aLock = null;
             DBTransactionHelper.SqlTransactionWork(DBTypes.SqlServer, true, false, _distributeConnectionFactory.CreateReadForDistributeCoordinator(),  (conn, transaction) =>
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
                    CommandText = string.Format(@"SELECT {0} FROM [ApplicationLock] WHERE name=@name;", StoreHelper.GetApplicationLockStoreSelectFields(string.Empty)),
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

                     using (reader =  command.ExecuteReader())
                    {
                        if ( reader.Read())
                        {
                            aLock = new ApplicationLock();
                            StoreHelper.SetApplicationLockStoreSelectFields(aLock, reader, string.Empty);
                        }
                        reader.Close();
                    }
                }
            });
            return aLock;
        }
    }
}
