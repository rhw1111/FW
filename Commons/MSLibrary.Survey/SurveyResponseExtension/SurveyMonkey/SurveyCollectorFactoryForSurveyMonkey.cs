using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Survey.SurveyMonkey;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;

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
                SurveyID = surveyData.ID
            };

            return await Task.FromResult(collector);
        }


    }
}
