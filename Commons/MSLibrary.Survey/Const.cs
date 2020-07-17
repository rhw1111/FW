using Microsoft.EntityFrameworkCore.Query.Internal;
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
        /// <summary>
        /// Survey查询
        /// </summary>
        public const string SurveyQuery = "SurveyQuery";
        /// <summary>
        /// Survey查询(单个)
        /// </summary>
        public const string SurveyQuerySingle = "SurveyQuerySingle";
        /// <summary>
        /// Survey关联的收集器查询
        /// </summary>
        public const string SurveyCollectorQuery = "SurveyCollectorQuery";
        /// <summary>
        /// Survey关联的消息查询
        /// </summary>
        public const string SurveyMessageQuery = "SurveyMessageQuery";
    }

    /// <summary>
    /// SurveyMonkey类型集合
    /// </summary>
    public static class SurveyMonkeyTypes
    {
        public const string OAuth = "OAuth";
    }

    /// <summary>
    /// SurveyMonkey事件类型集合
    /// </summary>
    public static class SurveyMonkeyEventTypes
    {
        public const string ResponseCompleted = "response_completed";
        public const string ResponseUpdated = "response_updated";
        public const string ResponseDisqualified = "response_disqualified";
        public const string ResponseCreated = "response_created";
        public const string ResponseDeleted = "response_deleted";
        public const string ResponseOverquota = "response_overquota";
        public const string CollectorCreated = "collector_created";
        public const string CollectorUpdated = "collector_updated";
        public const string CollectorDeleted = "collector_deleted";
        public const string SurveyCreated = "survey_created";
        public const string SurveyUpdated = "survey_updated";
        public const string SurveyDeleted = "survey_deleted";
    }

    /// <summary>
    /// SurveyMonkey对象类型集合
    /// </summary>
    public static class SurveyMonkeyObjectTypes
    {
        public const string Survey = "survey";
        public const string Collector = "collector";
    }

    /// <summary>
    /// SurveyMonkey的收集器状态集合
    /// </summary>
    public static class SurveyMonkeyCollectorStatuses
    {
        public const string Open = "open";
        public const string Closed = "closed";
    }

    /// <summary>
    ///  SurveyMonkey的收集器类型集合
    /// </summary>
    public static class SurveyMonkeyCollectorTypes
    {
        public const string SMS = "sms";
        public const string WebLink = "weblink";
        public const string Email = "email";
    }

    /// <summary>
    /// SurveyMonkey的消息类型集合
    /// </summary>
    public static class SurveyMonkeyMessageTypes
    {
        public const string SMS = "sms";
        public const string Invite = "invite";
        public const string Reminder = "reminder";
        public const string ThankYou = "thank_you";
    }

    /// <summary>
    /// SurveyMonkey的消息状态集合
    /// </summary>
    public static class SurveyMonkeyMessageStatuses
    {
        public const string Sent = "sent";
        public const string NotSent = "not_sent";
        public const string Processing = "processing";
    }


    /// <summary>
    /// SurveyMonkey的消息接收者状态集合
    /// </summary>
    public static class SurveyMonkeyMessageRecipientStatuses
    {
        public const string HasNotResponded = "has_not_responded";
        public const string PartiallyResponded = "partially_responded";
        public const string Completed = "completed";

    }
}
