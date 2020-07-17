using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.Survey.SurveyMonkey;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Survey.SurveyResponseExtension.SurveyMonkey
{
    /// <summary>
    /// 针对SurveyMonkey类型的配置
    /// </summary>
    [DataContract]
    public class EndpointConfiguration
    {
        /// <summary>
        /// SurveyMonkey终结点名称
        /// </summary>
        [DataMember]
        public string SurveyMonkeyEndpointName { get; set; } = null!;
        /// <summary>
        /// webhook回调地址
        /// </summary>
        [DataMember]
        public string? SubscriptionUrl { get; set; }
    }

    public static class EndpointConfigurationService
    {
        public static async Task<(EndpointConfiguration, SurveyMonkeyEndpoint)> GetSurveyMonkeyEndpoint(ISurveyMonkeyEndpointRepositoryCacheProxy surveyMonkeyEndpointRepositoryCacheProxy, string endpointConfiguration, CancellationToken cancellationToken)
        {
            var configurationObj = JsonSerializerHelper.Deserialize<EndpointConfiguration>(endpointConfiguration);

            var surveyMonkeyEndpoint = await surveyMonkeyEndpointRepositoryCacheProxy.QueryByName(configurationObj.SurveyMonkeyEndpointName, cancellationToken);

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
            return (configurationObj, surveyMonkeyEndpoint);
        }
    }

}
