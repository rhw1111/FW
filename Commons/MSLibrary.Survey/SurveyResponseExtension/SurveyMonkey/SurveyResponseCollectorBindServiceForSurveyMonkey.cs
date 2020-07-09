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
    [Injection(InterfaceType = typeof(SurveyResponseCollectorBindServiceForSurveyMonkey), Scope = InjectionScope.Singleton)]
    public class SurveyResponseCollectorBindServiceForSurveyMonkey : ISurveyResponseCollectorBindService
    {
        private readonly ISurveyMonkeyEndpointRepositoryCacheProxy _surveyMonkeyEndpointRepositoryCacheProxy;

        public SurveyResponseCollectorBindServiceForSurveyMonkey(ISurveyMonkeyEndpointRepositoryCacheProxy surveyMonkeyEndpointRepositoryCacheProxy)
        {
            _surveyMonkeyEndpointRepositoryCacheProxy = surveyMonkeyEndpointRepositoryCacheProxy;
        }

        public async Task<string?> Binding(string endpointConfiguration, SurveyResponseCollector collector, CancellationToken cancellationToken = default)
        {
            var (configurationObj,surveyMonkeyEndpoint) =await getSurveyMonkeyEndpoint(endpointConfiguration, cancellationToken);

            WebhookRegisterRequest registerRequest = new WebhookRegisterRequest()
            {
                Name = $"SurveyResponse_{collector.SurveyID}",
                EventType = SurveyMonkeyEventTypes.ResponseCompleted,
                ObjectType = SurveyMonkeyObjectTypes.Collector,
                ObjectIds = new List<string>() { collector.SurveyID },
                SubscriptionUrl = configurationObj.SubscriptionUrl
            };

            WebhookRegisterResponse? registerResponse=null;
            try
            {
                registerResponse = (WebhookRegisterResponse)await surveyMonkeyEndpoint.Execute(registerRequest, cancellationToken);
            }
            catch(UtilityException ex)
            {
                if (ex.Code!= (int)SurveyErrorCodes.ExistsSurveyMonkeyWebhookByName)
                {
                    throw;
                }
            }
            if (registerResponse!=null)
            {
                return registerResponse.ID;
            }
            return null;
        }

        public async Task UnBinding(string endpointConfiguration, SurveyResponseCollector collector, CancellationToken cancellationToken = default)
        {
            var (configurationObj, surveyMonkeyEndpoint) = await getSurveyMonkeyEndpoint(endpointConfiguration, cancellationToken);

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

        private async Task<(ResponseEndpointConfiguration,SurveyMonkeyEndpoint)> getSurveyMonkeyEndpoint(string endpointConfiguration, CancellationToken cancellationToken)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<ResponseEndpointConfiguration>(endpointConfiguration);

            var surveyMonkeyEndpoint = await _surveyMonkeyEndpointRepositoryCacheProxy.QueryByName(configurationObj.SurveyMonkeyEndpointName, cancellationToken);

            if (surveyMonkeyEndpoint == null)
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyMonkeyEndpointByName,
                    DefaultFormatting = "找不到名称为{0}的SurveyMonkey终结点",
                    ReplaceParameters = new List<object>() { configurationObj.SurveyMonkeyEndpointName }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyMonkeyEndpointByName, fragment, 1, 0);
            }
            return (configurationObj,surveyMonkeyEndpoint);
        }
    }
}
