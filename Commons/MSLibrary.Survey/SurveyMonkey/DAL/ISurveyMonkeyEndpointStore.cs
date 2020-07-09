using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.Survey.SurveyMonkey.DAL
{
    public interface ISurveyMonkeyEndpointStore
    {
        Task<SurveyMonkeyEndpoint?> QueryByName(string name, CancellationToken cancellationToken = default);
    }
}
