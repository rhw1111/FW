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

namespace FW.TestPlatform.Main.Entities.DAL
{
    [Injection(InterfaceType = typeof(ITestCaseHistoryStore), Scope = InjectionScope.Singleton)]
    public class TestCaseHistoryStore : ITestCaseHistoryStore
    {
        private readonly IMainDBConnectionFactory _mainDBConnectionFactory;
        private readonly IMainDBContextFactory _mainDBContextFactory;

        public TestCaseHistoryStore(IMainDBConnectionFactory mainDBConnectionFactory, IMainDBContextFactory mainDBContextFactory)
        {
            _mainDBConnectionFactory = mainDBConnectionFactory;
            _mainDBContextFactory = mainDBContextFactory;
        }

        public async Task Add(TestCaseHistory source, CancellationToken cancellationToken = default)
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

                    await dbContext.TestCaseHistories.AddAsync(source, cancellationToken);

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

                    var deleteObj = new TestCaseHistory() { ID = id};
                    dbContext.TestCaseHistories.Attach(deleteObj);
                    dbContext.TestCaseHistories.Remove(deleteObj);

                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        //public async Task<TestCaseHistory> QueryByID(Guid caseID, Guid id, CancellationToken cancellationToken = default)
        //{
        //    TestCaseHistory result = new TestCaseHistory();
        //    await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
        //    {
        //        await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
        //        {
        //            if (transaction != null)
        //            {
        //                await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
        //            }


        //            result = await (from item in dbContext.TestCaseHistories
        //                            where item.ID == id
        //                            select item).FirstOrDefaultAsync();
        //        }
        //    });

        //    return result;
        //}

        public async Task<TestCaseHistory?> QueryByCase(Guid caseId, Guid historyId, CancellationToken cancellationToken = default)
        {
            TestCaseHistory? result = null;
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    result = await (from item in dbContext.TestCaseHistories
                                    where item.ID == historyId && item.CaseID == caseId
                                    select item).FirstOrDefaultAsync();
                }
            });

            return result;
        }

        public async Task<QueryResult<TestCaseHistory>> QueryByPage(Guid caseID, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            QueryResult<TestCaseHistory> result = new QueryResult<TestCaseHistory>()
            {
                CurrentPage = page
            };
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    var count = await (from item in dbContext.TestCaseHistories
                                       where item.CaseID == caseID
                                       select item.ID).CountAsync();
                    result.TotalCount = count;
                    var ids = (from item in dbContext.TestCaseHistories
                               where item.CaseID == caseID
                               orderby item.CreateTime descending
                               select item.ID
                                        ).Skip((page - 1) * pageSize).Take(pageSize);

                    var datas = await (from item in dbContext.TestCaseHistories
                                       join idItem in ids
                                  on item.ID equals idItem
                                       orderby item.CreateTime descending
                                       select item).ToListAsync();
                    result.Results.AddRange(datas);
                }
            });
            return result;
        }

        public async Task DeleteHistories(List<Guid> ids, CancellationToken cancellationToken = default)
        {
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, false, false, _mainDBConnectionFactory.CreateAllForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }
                    List<TestCaseHistory> list = new List<TestCaseHistory>();
                    foreach (Guid id in ids)
                    {
                        var deleteObj = new TestCaseHistory() { ID = id };
                        list.Add(deleteObj);
                    }
                    dbContext.TestCaseHistories.AttachRange(list.ToArray());
                    dbContext.TestCaseHistories.RemoveRange(list.ToArray());
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            });
        }

        public async Task<List<TestCaseHistory>> QueryByCaseIdAndHistoryIds(Guid caseId, List<Guid> ids, CancellationToken cancellationToken = default)
        {
            List<TestCaseHistory> result = new List<TestCaseHistory>();
            await DBTransactionHelper.SqlTransactionWorkAsync(DBTypes.MySql, true, false, _mainDBConnectionFactory.CreateReadForMain(), async (conn, transaction) =>
            {
                await using (var dbContext = _mainDBContextFactory.CreateMainDBContext(conn))
                {
                    if (transaction != null)
                    {
                        await dbContext.Database.UseTransactionAsync(transaction, cancellationToken);
                    }

                    result = await (from item in dbContext.TestCaseHistories
                                    where item.CaseID == caseId && ids.Contains(item.ID)
                                    select item).ToListAsync();
                }
            });

            return result;
        }
    }
}
