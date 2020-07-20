using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Collections;
using MSLibrary.Survey.SurveyMonkey;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;
using MSLibrary.Survey.SurveyMonkey.Message;

namespace MSLibrary.Survey.SurveyResponseExtension.SurveyMonkey
{
    [Injection(InterfaceType = typeof(SurveyCollectorDataQueryServiceForSurveyMonkey), Scope = InjectionScope.Singleton)]
    public class SurveyCollectorDataQueryServiceForSurveyMonkey : ISurveyCollectorDataQueryService
    {
        private readonly int _pageSize = 800;
        private readonly ISurveyMonkeyEndpointRepositoryCacheProxy _surveyMonkeyEndpointRepositoryCacheProxy;

        public SurveyCollectorDataQueryServiceForSurveyMonkey(ISurveyMonkeyEndpointRepositoryCacheProxy surveyMonkeyEndpointRepositoryCacheProxy)
        {
            _surveyMonkeyEndpointRepositoryCacheProxy = surveyMonkeyEndpointRepositoryCacheProxy;
        }

        public async Task<bool> Exist(string endpointConfiguration, string surveyID, CancellationToken cancellationToken = default)
        {
            var (configurationObj, surveyMonkeyEndpoint) = await EndpointConfigurationService.GetSurveyMonkeyEndpoint(_surveyMonkeyEndpointRepositoryCacheProxy, endpointConfiguration, cancellationToken);

            SurveyQuerySingleRequest queryRequest = new SurveyQuerySingleRequest()
            {
                 ID= surveyID
            };

            SurveyQuerySingleResponse? queryResponse = null;
            try
            {
                queryResponse = (SurveyQuerySingleResponse)await surveyMonkeyEndpoint.Execute(queryRequest, cancellationToken);
            }
            catch (UtilityException ex)
            {
                if (ex.Code != (int)SurveyErrorCodes.NotFoundSurveyMonkeySurveyByID)
                {
                    return false;
                }
            }

            return true;

        }

        public IAsyncEnumerable<string> QueryAll(string endpointConfiguration, CancellationToken cancellationToken = default)
        {
            EndpointConfiguration? configurationObj=null;
            SurveyMonkeyEndpoint? surveyMonkeyEndpoint = null ;
            AsyncInteration<string> result = new AsyncInteration<string>(
                async(page)=>
                {
                    if (configurationObj == null || surveyMonkeyEndpoint == null)
                    {
                        (configurationObj, surveyMonkeyEndpoint) = await EndpointConfigurationService.GetSurveyMonkeyEndpoint(_surveyMonkeyEndpointRepositoryCacheProxy, endpointConfiguration, cancellationToken);
                    }

                    SurveyQueryRequest queryRequest = new SurveyQueryRequest()
                    {
                        Page = page,
                        PageSize = _pageSize
                    };

                    SurveyQueryResponse queryResponse = (SurveyQueryResponse)await surveyMonkeyEndpoint.Execute(queryRequest, cancellationToken);

                    var queryResult = (from item in queryResponse.SurveyItems
                                       select JsonSerializerHelper.Serializer(new SurveyData() { ID = item.ID, Title=item.Title, NickName=item.NickName })).ToList();

                    return queryResult;
                }
                );

            return result;
        }
    }
}
