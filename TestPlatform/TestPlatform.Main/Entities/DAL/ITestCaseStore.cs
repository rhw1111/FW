using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;

namespace FW.TestPlatform.Main.Entities.DAL
{
    /// <summary>
    /// 测试案例数据操作
    /// </summary>
    public interface ITestCaseStore
    {
        Task Add(TestCase tCase, CancellationToken cancellationToken = default);
        Task Update(TestCase tCase, CancellationToken cancellationToken = default);
        Task UpdateStatus(Guid id,TestCaseStatus status, CancellationToken cancellationToken = default);
        Task Delete(Guid id, CancellationToken cancellationToken = default);
        Task DeleteMutiple(List<Guid> ids, CancellationToken cancellationToken = default);
        Task<QueryResult<TestCase>> QueryByPage(string matchName,int page,int pageSize, CancellationToken cancellationToken = default);
        Task<TestCase?> QueryByID(Guid id, CancellationToken cancellationToken = default);

        Task<List<TestCase>> QueryCountNolockByStatus(TestCaseStatus status, IList<Guid> hostIds, CancellationToken cancellationToken = default);

        Task<TestCase?> QueryByName(string name, CancellationToken cancellationToken = default);
        Task<Guid?> QueryByNameNoLock(string name, CancellationToken cancellationToken = default);
        Task<IList<TestCase>> QueryByNames(IList<string> names, CancellationToken cancellationToken = default);
        Task<QueryResult<TestCase>> QueryByPage(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<TestCaseStatus?> QueryStatusByID(Guid id, CancellationToken cancellationToken = default);
    }
}
