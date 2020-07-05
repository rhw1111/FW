using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Survey
{
    /// <summary>
    /// SurveyMonkey请求的类型集合
    /// </summary>
    public static class SurveyMonkeyRequestTypes
    {
        /// <summary>
        /// Webhook注册
        /// </summary>
        public const string WebhookRegister = "WebhookRegister";
        /// <summary>
        /// Webhook查询
        /// </summary>
        public const string WebhookQuery = "WebhookQuery";
        /// <summary>
        ///  Webhook查询(单一)
        /// </summary>
        public const string WebhookQuerySingle = "WebhookQuerySingle";
        /// <summary>
        /// Webhook删除
        /// </summary>
        public const string WebhookDelete = "WebhookDelete";
        /// <summary>
        /// Webhook回调
        /// </summary>
        public const string WebhookCallback = "WebhookCallback";
        /// <summary>
        /// Survey响应查询
        /// </summary>
        public const string SurveyResponseQuery = "SurveyResponseQuery";
        /// <summary>
        ///  Survey响应查询(单个)
        /// </summary>
        public const string SurveyResponseQuerySingle = "SurveyResponseQuerySingle";
    }

    /// <summary>
    /// SurveyMonkey类型集合
    /// </summary>
    public static class SurveyMonkeyTypes
    {
        public const string OAuth = "OAuth";
    }
}
