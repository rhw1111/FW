using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Survey.SurveyMonkey;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;
using MSLibrary.Survey.SurveyMonkey.Message;

namespace MSLibrary.Survey.SurveyResponseExtension.SurveyMonkey
{
    [Injection(InterfaceType = typeof(SurveyCollectorBindServiceForSurveyMonkey), Scope = InjectionScope.Singleton)]
    public class SurveyCollectorBindServiceForSurveyMonkey : ISurveyCollectorBindService
    {
        private readonly ISurveyMonkeyEndpointRepositoryCacheProxy _surveyMonkeyEndpointRepositoryCacheProxy;

        public SurveyCollectorBindServiceForSurveyMonkey(ISurveyMonkeyEndpointRepositoryCacheProxy surveyMonkeyEndpointRepositoryCacheProxy)
        {
            _surveyMonkeyEndpointRepositoryCacheProxy = surveyMonkeyEndpointRepositoryCacheProxy;
        }

        public async Task<string?> Binding(string endpointConfiguration, SurveyRecord collector, CancellationToken cancellationToken = default)
        {
            var (configurationObj,surveyMonkeyEndpoint) =await EndpointConfigurationService.GetSurveyMonkeyEndpoint(_surveyMonkeyEndpointRepositoryCacheProxy, endpointConfiguration, cancellationToken);

            if (configurationObj.SubscriptionUrl != null)
            {
                WebhookRegisterRequest registerRequest = new WebhookRegisterRequest()
                {
                    Name = $"SurveyResponse_{collector.SurveyID}",
                    EventType = SurveyMonkeyEventTypes.ResponseCompleted,
                    ObjectType = SurveyMonkeyObjectTypes.Collector,
                    ObjectIds = new List<string>() { collector.SurveyID },
                    SubscriptionUrl = configurationObj.SubscriptionUrl
                };

                WebhookRegisterResponse? registerResponse = null;
                try
                {
                    registerResponse = (WebhookRegisterResponse)await surveyMonkeyEndpoint.Execute(registerRequest, cancellationToken);
                }
                catch (UtilityException ex)
                {
                    if (ex.Code != (int)SurveyErrorCodes.ExistsSurveyMonkeyWebhookByName)
                    {
                        throw;
                    }
                }
                if (registerResponse != null)
                {
                    return registerResponse.ID;
                }
            }
            return null;
        }

        public async Task UnBinding(string endpointConfiguration, SurveyRecord collector, CancellationToken cancellationToken = default)
        {
            var (configurationObj, surveyMonkeyEndpoint) = await EndpointConfigurationService.GetSurveyMonkeyEndpoint(_surveyMonkeyEndpointRepositoryCacheProxy, endpointConfiguration, cancellationToken);

            if (collector.BindingInfo!=null)
            {
                WebhookDeleteRequest deleteRequest = new WebhookDeleteRequest()
                {
                     ID= collector.BindingInfo
                };

                try
                {
                    var deleteResponse = (WebhookDeleteResponse)await surveyMonkeyEndpoint.Execute(deleteRequest, cancellationToken);
                }
                catch (UtilityException ex)
                {
                    if (ex.Code != (int)SurveyErrorCodes.NotFoundSurveyMonkeyWebhookByID)
                    {
                        throw;
                    }
                }
            }

        }


    }
}
