using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.Survey.DAL
{
    public interface ISurveyEndpointStore
    {
        Task UpdateInitInfo(Guid id,string initInfo, CancellationToken cancellationToken = default);
        Task<SurveyEndpoint?> QueryByTypeName(string type, string name, CancellationToken cancellationToken = default);
    }
}
