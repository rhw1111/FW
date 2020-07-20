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
    /// <summary>
    /// 基于SurveyMonkey的Survey收集器工厂
    /// 要求collectorData的格式为为
    /// {
    ///     "ID":"SurveyID"
    /// }
    /// </summary>
    [Injection(InterfaceType = typeof(SurveyCollectorFactoryForSurveyMonkey), Scope = InjectionScope.Singleton)]
    public class SurveyCollectorFactoryForSurveyMonkey : ISurveyCollectorFactory
    {
        private readonly ISurveyMonkeyEndpointRepositoryCacheProxy _surveyMonkeyEndpointRepositoryCacheProxy;

        public SurveyCollectorFactoryForSurveyMonkey(ISurveyMonkeyEndpointRepositoryCacheProxy surveyMonkeyEndpointRepositoryCacheProxy)
        {
            _surveyMonkeyEndpointRepositoryCacheProxy = surveyMonkeyEndpointRepositoryCacheProxy;
        }

        public async Task<SurveyRecord> Create(string endpointConfiguration, string collectorData, CancellationToken cancellationToken = default)
        {
            //var (endpointConfigurationObj,surveyMonkeyEndpoint)=await ResponseEndpointConfigurationService.GetSurveyMonkeyEndpoint(_surveyMonkeyEndpointRepositoryCacheProxy, endpointConfiguration, cancellationToken);
            var surveyData=JsonSerializerHelper.Deserialize<SurveyData>(collectorData);
            SurveyRecord collector = new SurveyRecord()
            {
                ID = Guid.NewGuid(),
                SurveyID = surveyData.ID,
                 Name=surveyData.Title            
            };

            return await Task.FromResult(collector);
        }

        public async Task<SurveyRecord> CreateFromDirect(string endpointConfiguration, string collectorData, CancellationToken cancellationToken = default)
        {
            var (configurationObj, surveyMonkeyEndpoint) = await EndpointConfigurationService.GetSurveyMonkeyEndpoint(_surveyMonkeyEndpointRepositoryCacheProxy, endpointConfiguration, cancellationToken);

            var webhookCallbackData = JsonSerializerHelper.Deserialize<WebhookCallbackData>(collectorData);

            if (webhookCallbackData.Resources.SurveyID==null)
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundInfoInSurveyMonkeyWebhookCallback,
                    DefaultFormatting = "在SurveyMonkey的webhook回调中找不到名称为{0}的信息，回调数据为{1}",
                    ReplaceParameters = new List<object>() { "survey_id", collectorData }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundInfoInSurveyMonkeyWebhookCallback, fragment, 1, 0);
            }

            SurveyQuerySingleRequest surveySingleQueryRequest = new SurveyQuerySingleRequest()
            {
                 ID= webhookCallbackData.Resources.SurveyID
            };

            var surveySingleQueryResponse=(SurveyQuerySingleResponse)await surveyMonkeyEndpoint.Execute(surveySingleQueryRequest,cancellationToken);

            SurveyRecord record = new SurveyRecord()
            {
                ID = Guid.NewGuid(),
                SurveyID = webhookCallbackData.Resources.SurveyID,
                Name = surveySingleQueryResponse.Title
            };

            return record;
        }
    }
}
