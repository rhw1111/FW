using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.Survey.DAL
{
    public interface ISurveyRecordStore
    {
        Task Add(SurveyRecord collector, CancellationToken cancellationToken = default);
        Task Delete(Guid endpointId, Guid id, CancellationToken cancellationToken = default);
        Task UpdateLatestHandleTime(Guid id, DateTime latestHandleTime, CancellationToken cancellationToken = default);
        IAsyncEnumerable<SurveyRecord> QueryAllByEndpoint(Guid endpointId, CancellationToken cancellationToken = default);
        Task<QueryResult<SurveyRecord>> QueryByPage(Guid endpointId, int page, int pageSize, CancellationToken cancellationToken = default);
        Task<SurveyRecord?> Query(Guid endpointId, Guid id, CancellationToken cancellationToken = default);

        Task<SurveyRecord?> Query(Guid endpointId, string surveyID, CancellationToken cancellationToken = default);

        Task UpdateBindingInfo(Guid id, string bindingInfo, CancellationToken cancellationToken = default);

        IAsyncEnumerable<SurveyRecord> QueryAllByGroup(Guid endpointId, string groupName, CancellationToken cancellationToken = default);

        Task UpdateLatestRecipientGenerateTime(Guid id,DateTime time, CancellationToken cancellationToken = default);

        Task UpdateRecipientConfiguration(Guid id,string type,string configuration, CancellationToken cancellationToken = default);

        Task Update(SurveyRecord collector, CancellationToken cancellationToken = default);
    }
}
