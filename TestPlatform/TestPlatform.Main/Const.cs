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
        public const string AdditionFuncNames = "AdditionFuncNames";
        public const string DataSourceVars = "DataSourceVars";
        public const string ConnectInit = "ConnectInit";
        public const string SendInit = "SendInit";
        public const string StopInit = "StopInit";

        public const string CaseServiceBaseAddress = "CaseServiceBaseAddress";
        public const string CaseID = "CaseID";
    }

    /// <summary>
    /// 脚本模板名称集
    /// </summary>
    public static class ScriptTemplateNames
    {
        public const string LocustTcp = "LocustTcp";
        public const string LocustHttp = "LocustHttp";
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
        /// <summary>
        /// 测试监控地址
        /// {0}为TestCase.EngineType
        /// </summary>
        public const string TestMonitorAddress="{0}_TestMonitorAddress";
        public const string CaseServiceBaseAddress = "CaseServiceBaseAddress";
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
        public const string AdditionFunc = "additionfunc";
        public const string DataVarDeclareInit = "datavardeclareinit";
        public const string ConnectInit = "connectinit";
        public const string SendInit = "sendinit";
        public const string StopInit = "stopinit";
        public const string SendData = "senddata";
        public const string RecvData = "recvdata";
        public const string RequestBody = "requestbody";
        public const string HostName = "hostname";
        public const string CurConnectID = "curconnectid";
        public const string CurConnect = "curconnect";
        public const string DataSource = "datasource";
        public const string NameOnceJsonDataInvoke = "nameoncejsondatainvoke";
        public const string TcpRRInvoke = "tcprrinvoke";
        public const string TcpRRWithConnectInvoke = "tcprrwithconnectinvoke";
        public const string GetJsonRowDataInvoke = "getjsonrowdatainvoke";
        public const string GetNameSerialNoInvoke = "getnameserialnoinvoke";
        public const string NumberFill = "numberfill";
        public const string IntRange = "intrange";
        public const string DecimalRange = "decimalrange";
        public const string RanJsonData = "ranjsondata";
        public const string DesSecurity = "dessecurity";
        public const string VarKV = "varkv";
        public const string CurrConnectKV = "currconnectkv";
        public const string CaseID = "caseid";
        public const string CaseServiceBaseAddress = "caseservicebaseaddress";
        public const string FilterJsonDataInvoke = "filterjsondatainvoke";
        public const string CalcCheckSumInvoke = "calcchecksuminvoke";
        public const string SlaveName = "slavename";
        public const string GetJsonDataInvoke = "getjsondatainvoke";
        public const string SplitJsonDataInvoke = "splitjsondatainvoke";
        public const string PrintInvoke = "printinvoke";
        public const string HttpGetWithConnectInvoke = "httpgetwithconnectinvoke";
        public const string HttpPostWithConnectInvoke = "httppostwithconnectinvoke";
    }

    /// <summary>
    /// 标签参数处理器类型
    /// </summary>
    public static class DataSourceTypes
    {
        public const string Label = "Label";
        public const string String = "String";
        public const string Int = "Int";
        public const string Json = "Json";
    }

    /// <summary>
    /// 标签参数处理器类型
    /// </summary>
    public static class AdditionFuncNames
    {
        public const string NameOnceJsonData = "NameOnceJsonData";
        public const string TcpRR = "TcpRR";
        public const string TcpRRWithConnect = "TcpRRWithConnect";
        public const string GetJsonRowData = "GetJsonRowData";
        public const string GetNameSerialNo = "GetNameSerialNo";
        public const string NumberFill = "NumberFill";
        public const string IntRange = "IntRange";
        public const string DecimalRange = "DecimalRange";
        public const string RanJsonData = "RanJsonData";
        public const string DesSecurity = "DesSecurity";
        public const string FilterJsonData = "FilterJsonData";
        public const string CalcCheckSum = "CalcCheckSum";
        public const string GetJsonData = "GetJsonData";
        public const string SplitJsonData = "SplitJsonData";
        public const string Print = "Print";
        public const string HttpGetWithConnect = "HttpGetWithConnect";
        public const string HttpPostWithConnect = "HttpPostWithConnect";
    }

    /// <summary>
    /// SSHEndpoint类型
    /// </summary>
    public static class SSHEndpointTypes
    {
        public const string Default = "Default";
    }
}
