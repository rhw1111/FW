using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Survey.SurveyMonkey;
using MSLibrary.Survey.SurveyMonkey.Message;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Survey.SurveyResponseExtension.SurveyMonkey
{
    [Injection(InterfaceType = typeof(SurveyEndpointInitServiceForSurveyMonkey), Scope = InjectionScope.Singleton)]
    public class SurveyEndpointInitServiceForSurveyMonkey : ISurveyEndpointInitService
    {
        private readonly ISurveyMonkeyEndpointRepositoryCacheProxy _surveyMonkeyEndpointRepositoryCacheProxy;

        public SurveyEndpointInitServiceForSurveyMonkey(ISurveyMonkeyEndpointRepositoryCacheProxy surveyMonkeyEndpointRepositoryCacheProxy)
        {
            _surveyMonkeyEndpointRepositoryCacheProxy = surveyMonkeyEndpointRepositoryCacheProxy;
        }


        public async Task<string?> Execute(string endpointConfiguration, CancellationToken cancellationToken = default)
        {
            string? initInfo = null;
            //增加针对Survey的webhook
            var (configurationObj, surveyMonkeyEndpoint) = await EndpointConfigurationService.GetSurveyMonkeyEndpoint(_surveyMonkeyEndpointRepositoryCacheProxy, endpointConfiguration, cancellationToken);

            if (configurationObj.SubscriptionUrl != null)
            {
                List<string> webhookIDs = new List<string>();
                List<string> conflictEventTypes = new List<string>();

                await registerWebhook(configurationObj, surveyMonkeyEndpoint, SurveyMonkeyEventTypes.SurveyCreated, webhookIDs, conflictEventTypes, cancellationToken);
                await registerWebhook(configurationObj, surveyMonkeyEndpoint, SurveyMonkeyEventTypes.SurveyUpdated, webhookIDs, conflictEventTypes, cancellationToken);
                await registerWebhook(configurationObj, surveyMonkeyEndpoint, SurveyMonkeyEventTypes.SurveyDeleted, webhookIDs, conflictEventTypes, cancellationToken);

                if (conflictEventTypes.Count>0)
                {

                }




                initInfo = JsonSerializerHelper.Serializer(webhookIDs);
            }

            return initInfo;
        }

        private async Task registerWebhook(EndpointConfiguration configurationObj, SurveyMonkeyEndpoint endpoint, string eventType, List<string> webhookIDs, List<string> conflictEventTypes, CancellationToken cancellationToken = default)
        {
            WebhookRegisterRequest registerRequest = new WebhookRegisterRequest()
            {
                EventType = eventType,
                Name = $"{eventType}_{endpoint.ID}",
                SubscriptionUrl = configurationObj.SubscriptionUrl
            };

            WebhookRegisterResponse? registerResponse = null;
            try
            {
                registerResponse = (WebhookRegisterResponse)await endpoint.Execute(registerRequest, cancellationToken);
            }
            catch (UtilityException ex)
            {
                if (ex.Code == (int)SurveyErrorCodes.ExistsSurveyMonkeyWebhookByName)
                {
                    conflictEventTypes.Add(eventType);
                }

                throw ex;
            }

            if (registerResponse != null)
            {
                webhookIDs.Add(registerResponse.ID);
            }
        }


    }
}
