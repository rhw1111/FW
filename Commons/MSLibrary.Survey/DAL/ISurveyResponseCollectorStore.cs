using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.Survey.DAL
{
    public interface ISurveyResponseCollectorStore
    {
        Task Add(SurveyResponseCollector collector, CancellationToken cancellationToken = default);
        Task Delete(Guid endpointId, Guid id, CancellationToken cancellationToken = default);
        Task UpdateLatestHandleTime(Guid id, DateTime latestHandleTime, CancellationToken cancellationToken = default);
        IAsyncEnumerable<SurveyResponseCollector> QueryAllCollector(Guid endpointId, CancellationToken cancellationToken = default);
        Task<QueryResult<SurveyResponseCollector>> QueryByPage(Guid endpointId, int page, int pageSize, CancellationToken cancellationToken = default);
        Task<SurveyResponseCollector?> Query(Guid endpointId, Guid id, CancellationToken cancellationToken = default);

        Task<SurveyResponseCollector?> Query(Guid endpointId, string surveyID, CancellationToken cancellationToken = default);

        Task UpdateBindingInfo(Guid id, string bindingInfo, CancellationToken cancellationToken = default);

        IAsyncEnumerable<SurveyResponseCollector> QueryAllCollectorByGroup(Guid endpointId, string groupName, CancellationToken cancellationToken = default);

    }
}
