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
        Task Query(string surveyType,string surveyID, CancellationToken cancellationToken = default);
        Task Delete(string surveyType, DateTime maxCreateTime, CancellationToken cancellationToken = default);
        
    }
}
