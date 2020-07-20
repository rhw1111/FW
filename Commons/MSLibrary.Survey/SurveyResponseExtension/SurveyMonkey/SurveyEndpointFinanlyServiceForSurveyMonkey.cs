using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Survey.SurveyMonkey;
using MSLibrary.Survey.SurveyMonkey.Message;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Survey.SurveyResponseExtension.SurveyMonkey
{
    [Injection(InterfaceType = typeof(SurveyEndpointFinanlyServiceForSurveyMonkey), Scope = InjectionScope.Singleton)]
    public class SurveyEndpointFinanlyServiceForSurveyMonkey : ISurveyEndpointFinanlyService
    {
        private readonly ISurveyMonkeyEndpointRepositoryCacheProxy _surveyMonkeyEndpointRepositoryCacheProxy;

        public SurveyEndpointFinanlyServiceForSurveyMonkey(ISurveyMonkeyEndpointRepositoryCacheProxy surveyMonkeyEndpointRepositoryCacheProxy)
        {
            _surveyMonkeyEndpointRepositoryCacheProxy = surveyMonkeyEndpointRepositoryCacheProxy;
        }

        public async Task Execute(string endpointConfiguration, string initInfo, CancellationToken cancellationToken = default)
        {
            //解除Survey的Webhook
            var (configurationObj, surveyMonkeyEndpoint) = await EndpointConfigurationService.GetSurveyMonkeyEndpoint(_surveyMonkeyEndpointRepositoryCacheProxy, endpointConfiguration, cancellationToken);

            WebhookDeleteRequest request = new WebhookDeleteRequest()
            {
                ID = initInfo
            };

            try
            {
                await surveyMonkeyEndpoint.Execute(request, cancellationToken);
            }
            catch(UtilityException ex)
            {
                if (ex.Code!= (int)SurveyErrorCodes.NotFoundSurveyMonkeyWebhookByID)
                {
                    throw;
                }
            }
        }
    }
}
