﻿using System;

namespace FW.TestPlatform.Main
{
    /// <summary>
    /// 模板上下文参数名称集合
    /// </summary>
    public static class TemplateContextParameterNames
    {
        public const string RequestBody = "RequestBody";
        public const string EngineType = "EngineType";
        public const string DataSourceFuncs = "DataSourceFuncs";
        public const string ResponseSeparator = "ResponseSeparator";
        public const string ReadyTime = "ReadyTime";
        public const string Address = "Address";
    }

    /// <summary>
    /// 脚本模板名称集
    /// </summary>
    public static class ScriptTemplateNames
    {
        public const string LocustTcp = "LocustTcp";
    }

    /// <summary>
    /// 日志提供方类型集合
    /// </summary>
    public static class LoggerProviderTypes
    {
        public const string CommonLogLocal = "CommonLogLocal";
    }

    /// <summary>
    /// 文件配置名称集合
    /// </summary>
    public static class ConfigurationNames
    {
        public const string Host = "Host";
        public const string Application = "Application";
        public const string Logger = "Logger";
    }

    /// <summary>
    /// 系统配置项名称集合
    /// </summary>
    public static class SystemConfigurationItemNames
    {
        public const string DefaultUserID = "DefaultUserID";
        /// <summary>
        /// 应用程序跨域源
        /// {0}为应用程序名称
        /// </summary>
        public const string ApplicationCrosOrigin = "{0}_CrosOrigin";
    }

    /// <summary>
    /// 日志提供方处理器名称集合
    /// </summary>
    public static class LoggerProviderHandlerNames
    {
        public const string Local = "Local";
    }

    /// <summary>
    /// 键值缓存访问服务类型集合
    /// </summary>
    public static class KVCacheVisitServiceTypes
    {
        public const string Combination = "Combination";

        public const string LocalTimeout = "LocalTimeout";

        public const string LocalVersion = "LocalVersion";
    }

    /// <summary>
    /// Http扩展上下文处理服务名称集合
    /// </summary>
    public static class HttpExtensionContextHandleServiceNames
    {
        public const string Internationalization = "Internationalization";
    }

    /// <summary>
    /// 声明上下文生成器类型
    /// </summary>
    public static class ClaimContextGeneratorTypes
    {
        public const string Default = "Default";
    }

    /// <summary>
    /// 身份声明类型集合
    /// </summary>
    public static class ClaimsTypes
    {
        public const string User = "User";

        public const string UserID = "UserID";

        public const string Lcid = "Lcid";

        public const string TimezoneOffset = "TimezoneOffset";
    }

    /// <summary>
    /// 日志目录集合
    /// </summary>
    public static class LoggerCategoryNames
    {
        public const string TestPlatform_Portal_Api = "TestPlatform.Portal.Api";
        public const string DIWrapper = "DIWrapper";
        public const string HttpRequest = "HttpRequest";
        public const string ContextExtension = "ContextExtension";

    }

    /// <summary>
    /// 声明上下文生成器名称集合
    /// </summary>
    public static class ClaimContextGeneratorNames
    {
        public const string Default = "Default";
    }

    /// <summary>
    /// 身份验证Scheme集合
    /// </summary>
    public static class AuthenticationSchemes
    {
        public const string Default = "Default";
    }
}
