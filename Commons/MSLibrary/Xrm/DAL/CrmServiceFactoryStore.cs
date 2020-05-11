using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using MSLibrary.DI;
using MSLibrary.Transaction;

namespace MSLibrary.Xrm.DAL
{
    [Injection(InterfaceType = typeof(ICrmServiceFactoryStore), Scope = InjectionScope.Singleton)]
    public class CrmServiceFactoryStore : ICrmServiceFactoryStore
    {
        private IXrmConnectionFactory _dbConnectionFactory;

        public CrmServiceFactoryStore(IXrmConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<CrmServiceFactory> QueryByName(string name)
        {
            CrmServiceFactory result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionFactory.CreateReadForXrm(), async (conn, transaction) =>
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
                    CommandText = string.Format(@"select {0} from CrmServiceFactory where [name]=@name", StoreHelper.GetCrmServiceFactorySelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {

                    var parameter = new SqlParameter("@name", SqlDbType.VarChar,100)
                    {
                        Value = name
                    };
                    commond.Parameters.Add(parameter);

                    await commond.PrepareAsync();



                    SqlDataReader reader = null;


                    await using (reader = await commond.ExecuteReaderAsync())
                    {

                        if (await reader.ReadAsync())
                        {
                            result = new CrmServiceFactory();
                            StoreHelper.SetCrmServiceFactorySelectFields(result, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return result;
        }
    }
}
