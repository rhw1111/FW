using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Survey.SurveyMonkey;
using MSLibrary.Survey.SurveyMonkey.Message;
using MSLibrary.Serializer;
using MSLibrary.Thread;
using MSLibrary.LanguageTranslate;
using MSLibrary.Collections;

namespace MSLibrary.Survey.SurveyResponseExtension.SurveyMonkey
{
    [Injection(InterfaceType = typeof(SurveyResponseDataQueryServiceForSurverMonkey), Scope = InjectionScope.Singleton)]
    public class SurveyResponseDataQueryServiceForSurverMonkey : ISurveyResponseDataQueryService
    {
        private readonly ISurveyMonkeyEndpointRepositoryCacheProxy _surveyMonkeyEndpointRepositoryCacheProxy;

        public SurveyResponseDataQueryServiceForSurverMonkey(ISurveyMonkeyEndpointRepositoryCacheProxy surveyMonkeyEndpointRepositoryCacheProxy)
        {
            _surveyMonkeyEndpointRepositoryCacheProxy = surveyMonkeyEndpointRepositoryCacheProxy;
        }
        public IAsyncEnumerable<string> Query(string endpointConfiguration, string surveyID, DateTime minTime, CancellationToken cancellationToken = default)
        {

            AsyncInteration<string> interation = new AsyncInteration<string>(
                async (index) =>
                {
                    var (configurationObj, surveyMonkeyEndpoint) = await EndpointConfigurationService.GetSurveyMonkeyEndpoint(_surveyMonkeyEndpointRepositoryCacheProxy, endpointConfiguration, cancellationToken);


                    List<SurveyResponse>? responses = null;

                    SurveyResponseQueryRequest responseQueryRequest = new SurveyResponseQueryRequest()
                    {
                         Page=index+1,
                          PageSize=800,
                           StartModifiedAt= minTime
                    };



        return null;
    }
    );
            return interation;


            throw new NotImplementedException();
        }
    }
}
