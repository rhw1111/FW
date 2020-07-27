using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
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
                List<string> conflictNames = new List<string>();

                await registerWebhook(configurationObj, surveyMonkeyEndpoint, SurveyMonkeyEventTypes.SurveyCreated, webhookIDs, conflictNames, cancellationToken);
                await registerWebhook(configurationObj, surveyMonkeyEndpoint, SurveyMonkeyEventTypes.SurveyUpdated, webhookIDs, conflictNames, cancellationToken);
                await registerWebhook(configurationObj, surveyMonkeyEndpoint, SurveyMonkeyEventTypes.SurveyDeleted, webhookIDs, conflictNames, cancellationToken);

                if (conflictNames.Count>0)
                {
                    var allRegisters=await getALlWebhook(surveyMonkeyEndpoint, cancellationToken);
                    var ids = (from item in allRegisters
                               where conflictNames.Contains(item.Name)
                               select item.ID).ToList();

                    webhookIDs.AddRange(ids);
                }

                initInfo = JsonSerializerHelper.Serializer(webhookIDs);
            }

            return initInfo;
        }

        private async Task registerWebhook(EndpointConfiguration configurationObj, SurveyMonkeyEndpoint endpoint, string eventType, List<string> webhookIDs, List<string> conflictNames, CancellationToken cancellationToken = default)
        {
            WebhookRegisterRequest registerRequest = new WebhookRegisterRequest()
            {
                EventType = eventType,
                Name = $"{eventType}_{endpoint.ID}",
                SubscriptionUrl = configurationObj.SubscriptionUrl=null!
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
                    conflictNames.Add(registerRequest.Name);
                }

                throw ex;
            }

            if (registerResponse != null)
            {
                webhookIDs.Add(registerResponse.ID);
            }
        }


        private async Task<List<WebhookRegisterItem>> getALlWebhook(SurveyMonkeyEndpoint endpoint,CancellationToken cancellationToken = default)
        {
            int pageSize = 100;
            int page = 1;
            List<WebhookRegisterItem> result = new List<WebhookRegisterItem>();
            while (true)
            {
                WebhookQueryRequest queryRequest = new WebhookQueryRequest()
                {
                    Page = page,
                    PageSize = pageSize
                };

                var queryResponse = (WebhookQueryResponse)await endpoint.Execute(queryRequest, cancellationToken);
                result.AddRange(queryResponse.RegisterItems);
                if(queryResponse.RegisterItems.Count< pageSize)
                {
                    break;
                }
                page++;
            }

            return result;
        }
    }
}
