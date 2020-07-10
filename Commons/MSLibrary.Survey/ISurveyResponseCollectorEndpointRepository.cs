using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.Survey
{
    public interface ISurveyResponseCollectorEndpointRepository
    {
        Task<SurveyResponseCollectorEndpoint?> QueryByTypeName(string type, string name, CancellationToken cancellationToken = default);
    }
}
