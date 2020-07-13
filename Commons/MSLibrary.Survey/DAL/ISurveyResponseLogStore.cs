using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.Survey.DAL
{
    /// <summary>
    /// Survey响应日志数据操作
    /// </summary>
    public interface ISurveyResponseLogStore
    {
        Task Add(SurveyResponseLog log, CancellationToken cancellationToken = default);
        Task<SurveyResponseLog?> Query(string surveyType,string surveyID,string responseID, CancellationToken cancellationToken = default);
        Task<Guid?> QueryNoLock(string surveyType, string surveyID, string responseID, CancellationToken cancellationToken = default);

        Task Delete(string surveyType, string surveyID, DateTime maxCreateTime, CancellationToken cancellationToken = default);
        
    }
}
