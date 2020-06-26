using System;

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
        public const string Port = "Port";
        public const string DataSourceVars = "DataSourceVars";
        public const string ConnectInit = "ConnectInit";
        public const string Sendinit= "Sendinit";
        public const string AdditionFuncNames = "AdditionFuncNames";

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
    /// 声明上下文生成器类型集合
    /// </summary>
    public static class ClaimContextGeneratorTypes
    {
        public const string Default = "Default";
    }
    /// <summary>
    /// 环境声明生成器类型集合
    /// </summary>
    public static class EnvironmentClaimGeneratorTypes
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
    /// 环境声明生成器名称集合
    /// </summary>
    public static class EnvironmentClaimGeneratorNames
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

    /// <summary>
    /// 引擎类型
    /// </summary>
    public static class EngineTypes
    {
        public const string Http = "Http";
        public const string Tcp = "Tcp";
    }

    /// <summary>
    /// 运行时引擎类型
    /// </summary>
    public static class RuntimeEngineTypes
    {
        public const string Locust = "Locust";
    }

    /// <summary>
    /// InfluxDB数据库参数
    /// </summary>
    public static class InfluxDBParameters
    {
        /// <summary>
        /// EndpointName
        /// </summary>
        public const string EndpointName = "EndpointName";

        /// <summary>
        /// 数据库名
        /// </summary>
        public const string DBName = "Monitor";

        /// <summary>
        /// MasterData
        /// </summary>
        public const string MasterMeasurementName = "MasterData";

        /// <summary>
        /// SlaveData
        /// </summary>
        public const string SlaveMeasurementName = "SlaveData";
    }

    /// <summary>
    /// 标签参数处理器类型
    /// </summary>
    public static class LabelParameterTypes
    {
        public const string DataVarDeclareInit = "datavardeclareinit";
        public const string AdditionFunc = "additionfunc";
        public const string ConnectInit = "connectinit";
        public const string SendInit = "sendinit";
        public const string SendData = "senddata";
    }

    /// <summary>
    /// 标签参数处理器类型
    /// </summary>
    public static class DataSourceTypes
    {
        public const string String = "String";
        public const string Int = "Int";
        public const string Json = "Json";
    }
}
