using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Survey.SurveyMonkey.DAL;

namespace MSLibrary.Survey.SurveyMonkey
{
    [Injection(InterfaceType = typeof(ISurveyMonkeyEndpointRepository), Scope = InjectionScope.Singleton)]
    public class SurveyMonkeyEndpointRepository : ISurveyMonkeyEndpointRepository
    {
        private readonly ISurveyMonkeyEndpointStore _surveyMonkeyEndpointStore;

        public SurveyMonkeyEndpointRepository(ISurveyMonkeyEndpointStore surveyMonkeyEndpointStore)
        {
            _surveyMonkeyEndpointStore = surveyMonkeyEndpointStore;
        }

        public async Task<SurveyMonkeyEndpoint?> QueryByName(string name, CancellationToken cancellationToken = default)
        {
            return await _surveyMonkeyEndpointStore.QueryByName(name, cancellationToken);
        }
    }
}
