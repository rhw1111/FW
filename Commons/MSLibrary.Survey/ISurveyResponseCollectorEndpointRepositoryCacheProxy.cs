using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.Survey
{
    public interface ISurveyResponseCollectorEndpointRepositoryCacheProxy
    {
        Task<SurveyEndpoint?> QueryByTypeName(string type, string name, CancellationToken cancellationToken = default);
    }
}
