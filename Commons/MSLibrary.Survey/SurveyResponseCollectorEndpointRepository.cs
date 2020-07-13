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
        private readonly ISurveyResponseCollectorEndpointStore _surveyResponseCollectorEndpointStore;

        public SurveyResponseCollectorEndpointRepository(ISurveyResponseCollectorEndpointStore surveyResponseCollectorEndpointStore)
        {
            _surveyResponseCollectorEndpointStore = surveyResponseCollectorEndpointStore;
        }
        public async Task<SurveyResponseCollectorEndpoint?> QueryByTypeName(string type, string name, CancellationToken cancellationToken = default)
        {
            return await _surveyResponseCollectorEndpointStore.QueryByTypeName(type, name,cancellationToken);
        }
    }
}
