using MSLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MSLibrary.DI;
using MSLibrary.Transaction;
using FW.TestPlatform.Main.DAL;
using Microsoft.AspNetCore.Http.Features.Authentication;
using FW.TestPlatform.Main.DTOModel;

namespace FW.TestPlatform.Main.Entities.DAL
{
    [Injection(InterfaceType = typeof(ITestCaseSlaveHostStore), Scope = InjectionScope.Singleton)]
    public class TestCaseSlaveHostStore : ITestCaseSlaveHostStore
    {
        private readonly IMainDBConnectionFactory _mainDBConnectionFactory;
        private readonly IMainDBContextFactory _mainDBContextFactory;

        public TestCaseSlaveHostStore(IMainDBConnectionFactory mainDBConnectionFactory, IMainDBContextFactory mainDBContextFactory)
        {
            _mainDBConnectionFactory = mainDBConnectionFactory;
            _mainDBContextFactory = mainDBContextFactory;
        }

        public async Task Add(TestCaseSlaveHost source, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _mainDBConnectionFactory.CreateAllForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    if (source.ID == Guid.Empty)
                    {
                        source.ID = Guid.NewGuid();
                    }

                    source.CreateTime = DateTime.UtcNow;
                    source.ModifyTime = DateTime.UtcNow;

                    await dbContext.TestCaseSlaveHosts.AddAsync(source, cancellationToken);

                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });

        }

        public async Task Delete(Guid id, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _mainDBConnectionFactory.CreateAllForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    var deleteObj = new TestCaseSlaveHost() { ID = id };
                    dbContext.TestCaseSlaveHosts.Attach(deleteObj);
                    dbContext.TestCaseSlaveHosts.Remove(deleteObj);

                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public async Task Update(TestCaseSlaveHost source, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _mainDBConnectionFactory.CreateAllForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    source.ModifyTime = DateTime.UtcNow;
                    dbContext.TestCaseSlaveHosts.Attach(source);

                    var entry = dbContext.Entry(source);
                    foreach (var item in entry.Properties)
                    {
                        entry.Property(item.Metadata.Name).IsModified = true;
                    }
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public async Task<TestCaseSlaveHost?> QueryByCase(Guid caseId,Guid slaveId, CancellationToken cancellationToken = default)
        {
            TestCaseSlaveHost? result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }


                    result = await (from item in dbContext.TestCaseSlaveHosts
                                    where item.ID == slaveId
                                    select item).FirstOrDefaultAsync();
                }
            });

            return result;
        }

        public async IAsyncEnumerable<TestCaseSlaveHost> QueryByCase(Guid caseId, CancellationToken cancellationToken = default)
        {
            List<TestCaseSlaveHost> list = new List<TestCaseSlaveHost>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }


                    list = await (from item in dbContext.TestCaseSlaveHosts
                                    where item.TestCaseID == caseId
                                    select item).ToListAsync();
                }
            });
            foreach (var item in list)
            {
                yield return item;
            }
        }

        //public async Task<TestCase?> QueryByName(string name, CancellationToken cancellationToken = default)
        //{
        //    TestCase? result = null;
        //    await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
        //    {
        //        await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
        //        {
        //            if (transaction != null)
        //            {
        //                await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
        //            }


        //            result = await (from item in dbContext.TestCases
        //                            where item.Name == name
        //                            select item).FirstOrDefaultAsync();
        //        }
        //    });

        //    return result;
        //}

    }
}
