using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using MSLibrary.DI;
using MSLibrary.Transaction;

namespace MSLibrary.Azure.DAL
{
    [Injection(InterfaceType = typeof(ITokenCredentialGeneratorStore), Scope = InjectionScope.Singleton)]
    public class TokenCredentialGeneratorStore : ITokenCredentialGeneratorStore
    {
        private IAzureInfoConnectionFactory _azureInfoConnectionFactory;

        public TokenCredentialGeneratorStore(IAzureInfoConnectionFactory azureInfoConnectionFactory)
        {
            _azureInfoConnectionFactory = azureInfoConnectionFactory;
        }
        public async Task<TokenCredentialGenerator> QueryByName(string name)
        {
            TokenCredentialGenerator generator = null;

            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _azureInfoConnectionFactory.CreateReadForAzureInfo(), async(conn, transaction) =>
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
                    CommandText = string.Format(@"select {0} from TokenCredentialGenerator where [name]=@name", StoreHelper.GetTokenCredentialGeneratorSelectFields(string.Empty)),
                    Transaction = sqlTran
                })
                {

                    var parameter = new SqlParameter("@name", SqlDbType.VarChar, 100)
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
                            generator = new TokenCredentialGenerator();
                            StoreHelper.SetTokenCredentialGeneratorSelectFields(generator, reader, string.Empty);
                        }

                        await reader.CloseAsync();
                    }
                }
            });


            return generator;
        }
    }
}
