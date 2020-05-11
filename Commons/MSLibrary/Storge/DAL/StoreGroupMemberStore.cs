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
    [Injection(InterfaceType = typeof(IStoreGroupMemberStore), Scope = InjectionScope.Singleton)]
    public class StoreGroupMemberStore : IStoreGroupMemberStore
    {
        private IStorgeConnectionFactory _storgeConnectionFactory;

        public StoreGroupMemberStore(IStorgeConnectionFactory storgeConnectionFactory)
        {
            _storgeConnectionFactory = storgeConnectionFactory;
        }


        public async Task<StoreGroupMember> QueryByName(Guid groupID, string memberName)
        {
            StoreGroupMember member = null;

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
                    CommandText = string.Format(@"select {0},{1} from StoreGroupMember as m join StoreGroup as g on m.groupid=g.id where g.[id]=@groupid and m.[name]=@name", StoreHelper.GetStoreGroupMemberSelectFields("m"), StoreHelper.GetStoreGroupSelectFields("g"))
                })
                {

                    var parameter = new SqlParameter("@groupid", SqlDbType.UniqueIdentifier)
                    {
                        Value = groupID
                    };
                    commond.Parameters.Add(parameter);

                    parameter = new SqlParameter("@name", SqlDbType.VarChar,150)
                    {
                        Value = memberName
                    };
                    commond.Parameters.Add(parameter);


                    commond.Prepare();
                    SqlDataReader reader = null;

                    await using (reader = await commond.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            member = new StoreGroupMember();
                            StoreHelper.SetStoreGroupMemberSelectFields(member, reader, "m");
                            member.Group = new StoreGroup();
                            StoreHelper.SetStoreGroupSelectFields(member.Group, reader, "g");

                        }

                       await  reader.CloseAsync();
                    }
                }
            });

            return member;
        }
    }
}
