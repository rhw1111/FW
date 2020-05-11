using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using MSLibrary.DI;
using MSLibrary.Transaction;
using MSLibrary.DAL;

namespace MSLibrary.Storge.DAL
{

    [Injection(InterfaceType = typeof(IStoreGroupStore), Scope = InjectionScope.Singleton)]
    public class StoreGroupStore : IStoreGroupStore
    {
        private IStorgeConnectionFactory _storgeConnectionFactory;

        public StoreGroupStore(IStorgeConnectionFactory storgeConnectionFactory)
        {
            _storgeConnectionFactory = storgeConnectionFactory;
        }


        public async Task<StoreGroup> QueryByName(string name)
        {
            StoreGroup group = null;

            //获取只读连接字符串
            var strConn = _storgeConnectionFactory.CreateReadForStorge();


            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, strConn, async (conn, transaction) =>
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
                    CommandText = string.Format(@"select {0} StoreGroup where [name]=@name", StoreHelper.GetStoreGroupSelectFields(string.Empty))
                })
                {

                    var parameter = new SqlParameter("@name", SqlDbType.VarChar, 150)
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
                            group = new StoreGroup();
                            StoreHelper.SetStoreGroupSelectFields(group, reader, string.Empty);

                        }

                        await reader.CloseAsync();
                    }
                }
            });

            return group;
        }
    }
}
