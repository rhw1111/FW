using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.Survey.DAL
{
    public interface ISurveyResponseCollectorEndpointStore
    {
        Task UpdateInitInfo(Guid id,string initInfo, CancellationToken cancellationToken = default);
        Task<SurveyResponseCollectorEndpoint?> QueryByTypeName(string type, string name, CancellationToken cancellationToken = default);
    }
}
