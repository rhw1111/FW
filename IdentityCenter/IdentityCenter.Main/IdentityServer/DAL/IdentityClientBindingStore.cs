using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using IdentityCenter.Main.Entities.DAL;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Transaction;
using MSLibrary.Collections;
using IdentityCenter.Main.DAL;

namespace IdentityCenter.Main.IdentityServer.DAL
{
    [Injection(InterfaceType = typeof(IIdentityClientBindingStore), Scope = InjectionScope.Singleton)]
    public class IdentityClientBindingStore : IIdentityClientBindingStore
    {
        private readonly IDBConnectionMainFactory _dbConnectionMainFactory;

        public IdentityClientBindingStore(IDBConnectionMainFactory dbConnectionMainFactory)
        {
            _dbConnectionMainFactory = dbConnectionMainFactory;
        }

        public IAsyncEnumerable<IdentityClientBinding> QueryAll(CancellationToken cancellationToken = default)
        {
            int size = 500;
            long? sequence = null;
            List<IdentityClientBinding> datas=new List<IdentityClientBinding>();
            AsyncInteration<IdentityClientBinding> result = new AsyncInteration<IdentityClientBinding>(
                async(index)=>
                {
                    
                    await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.SqlServer, true, false, _dbConnectionMainFactory.CreateReadForIdentityConfiguration(), async (conn, transaction) =>
                    {

                        await using (var dbContext = EntityDBContextFactory.CreateConfigurationDBContext(conn))
                        {
                            if (transaction != null)
                            {
                                await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                            }

                          
                           if (sequence==null)
                            {
                                datas= await (from item in dbContext.IdentityClientBindings
                                              orderby EF.Property<long>(item, "Sequence")
                                              select item).Take(size).ToListAsync(cancellationToken);
                            }
                           else
                            {
                                datas = await (from item in dbContext.IdentityClientBindings
                                               where EF.Property<long>(item, "Sequence")> sequence
                                               orderby EF.Property<long>(item, "Sequence")
                                               select item).Take(size).ToListAsync(cancellationToken);
                            }

                          if (datas.Count>0)
                            {
                                sequence = EF.Property<long>(datas[datas.Count - 1], "Sequence");
                            }
                        }
                    });

                    return datas;
                }

                );

            return result;
        }
    }
}
