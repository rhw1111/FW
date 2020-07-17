using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Survey.DAL;

namespace MSLibrary.Survey
{
    [Injection(InterfaceType = typeof(ISurveyResponseCollectorEndpointRepository), Scope = InjectionScope.Singleton)]
    public class SurveyResponseCollectorEndpointRepository : ISurveyResponseCollectorEndpointRepository
    {
        private readonly ISurveyEndpointStore _surveyEndpointStore;

        public SurveyResponseCollectorEndpointRepository(ISurveyEndpointStore surveyEndpointStore)
        {
            _surveyEndpointStore = surveyEndpointStore;
        }
        public async Task<SurveyEndpoint?> QueryByTypeName(string type, string name, CancellationToken cancellationToken = default)
        {
            return await _surveyEndpointStore.QueryByTypeName(type, name,cancellationToken);
        }
    }
}
