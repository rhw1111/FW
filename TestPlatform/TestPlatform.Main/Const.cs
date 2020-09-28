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
        public const string LocustWebSocket = "LocustWebSocket";
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
        public const string TestMonitorAddress = "{0}_TestMonitorAddress";
        public const string CaseServiceBaseAddress = "CaseServiceBaseAddress";
        /// <summary>
        /// 测试历史记录监控地址
        /// </summary>
        public const string TestCaseHistoryMonitorAddress = "TestHistoryMonitorAddress";
        /// <summary>
        /// 网关数据文件夹地址
        /// </summary>
        public const string NetGatewayDataFolder = "NetGatewayDataFolder";
        /// <summary>
        /// 网关数据临时文件夹地址
        /// </summary>
        public const string NetGatewayDataTempFolder = "NetGatewayDataTempFolder";
        /// <summary>
        /// 网关数据文件的SSHEndpoint
        /// </summary>
        public const string NetGatewayDataSSHEndpoint = "NetGatewayDataSSHEndpoint";
    }

    /// <summary>
    /// 日志提供方处理器名称集合
    /// </summary>
    public static class LoggerProviderHandlerNames
    {
        public const string Local = "Local";
        public const string Console = "Console";
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
        public const string WebSocket = "WebSocket";
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

        /// <summary>
        /// NetGatewayMasterData
        /// </summary>
        public const string NetGatewayMasterMeasurementName = "NetGatewayMasterData";

        /// <summary>
        /// NetGatewaySlaveData
        /// </summary>
        public const string NetGatewaySlaveMeasurementName = "NetGatewaySlaveData";

        /// <summary>
        /// NetGatewayTotalData
        /// </summary>
        public const string NetGatewayTotalMeasurementName = "NetGatewayTotalData";
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
        public const string Print = "print";
        public const string HttpGetWithConnectInvoke = "httpgetwithconnectinvoke";
        public const string HttpPostWithConnectInvoke = "httppostwithconnectinvoke";
        public const string Now = "now";
        public const string Time = "time";
        public const string Sleep = "sleep";
        public const string FireEventRequest = "fireeventrequest";
        public const string DateTimeFormate = "datetimeformate";
        public const string DateTimeAdd = "datetimeadd";
        public const string UserName = "username";
        public const string WebSocketWithConnectInvoke = "websocketwithconnectinvoke";
    }

    public static class LabelTypes
    {
        public const string LabelType0100 = "内部标签";
        public const string LabelType1000 = "脚本内部标签";
        public const string LabelType2000 = "数据源类标签";
        public const string LabelType3000 = "连接类标签";
        public const string LabelType4000 = "函数类标签";
        public const string LabelType5000 = "变量类标签";
        public const string LabelType6000 = "数值类标签";
        public const string LabelType7000 = "业务类标签";
        public const string LabelType9999 = "无效类标签";
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
        public const string FireEventRequest = "FireEventRequest";
        public const string DateTimeFormate = "DateTimeFormate";
        public const string DateTimeAdd = "DateTimeAdd";
        public const string HttpGetWithConnect = "HttpGetWithConnect";
        public const string HttpPostWithConnect = "HttpPostWithConnect";
        public const string WebSocketWithConnect = "WebSocketWithConnect";
    }

    /// <summary>
    /// SSHEndpoint类型
    /// </summary>
    public static class SSHEndpointTypes
    {
        public const string Default = "Default";
    }

    /// <summary>
    /// 环境变量名称集合
    /// </summary>
    public static class EnvironmentVarNames
    {
        public const string AppName = "appname";
    }

    /// <summary>
    /// 批处理动作初始化类型集合
    /// </summary>
    public static class ScheduleActionInitTypes
    {
        public const string Default = "Default";
    }

    /// <summary>
    /// 网关回包数据的格式
    /// </summary>
    public static class NetGatewayDataFormatTypes
    {
        //public const string APICreditUpdateReplyMsg = "APICreditUpdateReplyMsg";
        //public const string APICreditUpdateRequestMsg = "APICreditUpdateRequestMsg";
        //public const string ApiListMarketDataAck = "ApiListMarketDataAck";
        //public const string ApiMarketData = "ApiMarketData";
        //public const string ApiMarketDataRequest = "ApiMarketDataRequest";
        //public const string APIOcoOrderCancelReplyMsg = "APIOcoOrderCancelReplyMsg";
        //public const string APIOcoOrderCancelRequestMsg = "APIOcoOrderCancelRequestMsg";
        //public const string APIOcoOrderSumitReplyMsg = "APIOcoOrderSumitReplyMsg";
        //public const string APIOcoOrderSumitRequestMsg = "APIOcoOrderSumitRequestMsg";
        //public const string APIOrderCancelReplyMsg = "APIOrderCancelReplyMsg";
        //public const string APIOrderCancelRequestMsg = "APIOrderCancelRequestMsg";
        //public const string APIOrderSubmitReplyMsg = "APIOrderSubmitReplyMsg";
        //public const string APIOrderSubmitRequestMsg = "APIOrderSubmitRequestMsg";
        //public const string BridgeOrderSubmitRequestMsg = "BridgeOrderSubmitRequestMsg";
        //public const string TokenReplyMsg = "TokenReplyMsg";
        //public const string TokenRequestMsg = "TokenRequestMsg";
        //public const string EmptyMsg = "EmptyMsg";
        //public const string Standard = "Standard";
        //public const string Header = "Header";
        //public const string User = "User";
        public const string DSP = "DSP";
        public const string IMIX = "IMIX";
    }

    /// <summary>
    /// 树状实体数据服务类型集合
    /// </summary>
    public static class TreeEntityValueServiceTypes
    {
        public const int Folder = 1;
        public const int TestCase = 2;
        public const int TestDataSource = 3;
    }

    /// <summary>
    /// 实体树复制服务类型集合
    /// </summary>
    public static class EntityTreeCopyServiceTypes
    {
        public const string TestCase = "TestCase";
        public const string TestDataSource = "TestDataSource";
    }
}
