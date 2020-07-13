using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Survey.SurveyResponseExtension.SurveyMonkey
{
    /// <summary>
    /// 针对SurveyMonkey类型的配置
    /// </summary>
    [DataContract]
    public class ResponseEndpointConfiguration
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
        public string SubscriptionUrl { get; set; } = null!;
    }
}
