using System;
using System.Collections.Generic;
using System.Text;

namespace FW.TestPlatform.Main
{
    public enum TestPlatformErrorCodes
    {
        /// <summary>
        /// 找不到指定测试引擎的测试案例处理服务
        /// </summary>
        NotFoundTestCaseHandleServiceByEngine = 325710000,
        /// <summary>
        /// 找不到指定Id的测试主机
        /// </summary>
        NotFoundTestHostByID= 325710001,
        /// <summary>
        /// 在指定的测试案例中找不到指定的从测试主机
        /// </summary>
        NotFoundSlaveHostInCase= 325710002,
        /// <summary>
        /// 只能在指定状态下修改测试案例
        /// </summary>
        StatusErrorOnTestCaseUpdate = 325710003,
        /// <summary>
        /// 只能在指定状态下删除测试案例
        /// </summary>
        StatusErrorOnTestCaseDelete = 325710004,
        /// <summary>
        /// 只能在指定状态下运行测试案例
        /// </summary>
        StatusErrorOnTestCaseRun= 325710005,
        /// <summary>
        /// 只能在指定状态下停止测试案例
        /// </summary>
        StatusErrorOnTestCaseStop= 325710006,
        /// <summary>
        /// 指定的测试主机已经被执行
        /// </summary>
        TestHostHasRunning= 325710007,
        /// <summary>
        /// 找不到指定类型的Locust测试代码生成服务
        /// </summary>
        NotFoundLocustPyCodeGenerateServiceByType= 325710008,
        /// <summary>
        /// 找不到指定类型的测试数据源脚本方法生成服务
        /// </summary>
        NotFoundTestDataSourceScriptFuncGenerateServiceByType= 325710010,
        /// <summary>
        /// 找不到指定名称的脚本模板
        /// </summary>
        NotFoundScriptTemplateByName= 325710020,
        /// <summary>
        /// 找不到指定名称的数据源函数生成服务
        /// </summary>
        NotFoundGenerateDataSourceFuncServiceByName= 325710030,
        /// <summary>
        /// 找不到指定名称的数据变量声明代码块生成服务
        /// </summary>
        NotFoundGenerateDataVarDeclareServiceByName= 325710031,
        /// <summary>
        /// 找不到指定名称的附加函数生成服务
        /// </summary>
        NotFoundGenerateAdditionFuncServiceByName = 325710032,
        /// <summary>
        /// 找不到指定名称的函数调用生成服务
        /// </summary>
        NotFoundGenerateFuncInvokeServiceByName = 325710033,
        /// <summary>
        /// 找不到指定名称的变量设置声明代码块生成服务
        /// </summary>
        NotFoundGenerateVarSettingServiceByName = 325710034,
        /// <summary>
        /// 找不到指定名称的获取分隔符服务
        /// </summary>
        NotFoundGetSeparatorServiceByName = 325710035,
        /// <summary>
        /// 找不到指定名称的获取空格服务
        /// </summary>
        NotFoundGetSpaceServiceByName = 325710036,
        /// <summary>
        /// 找不到指定名称的变量调用生成服务
        /// </summary>
        NotFoundGenerateVarInvokeServiceByName = 325710037,
        /// <summary>
        /// 在上下文参数DataSourceFuncs中找不到指定函数名称的记录
        /// </summary>
        NotFoundFuncNameInDataSourceFuncsFormContext = 325710040,
        /// <summary>
        /// 找不到指定键值的数据源函数调用脚本生成服务
        /// </summary>
        NotFoundDataSourceInvokeScriptGenerateServiceByKey = 325710041,
        /// <summary>
        /// 已经存在指定名称的测试数据源
        /// </summary>
        ExistTestDataSourceByName= 325710101,
        /// <summary>
        /// 找不到指定名称的测试数据源
        /// </summary>
        NotFoundTestDataSourceByName = 325710102,
        /// <summary>
        /// 找不到指定Id的测试案例
        /// </summary>
        NotFoundTestCaseByID = 325710201,
        /// <summary>
        /// 找不到指定名称的InfluxDB数据源配置
        /// </summary>
        NotFoundInfluxDBEndpoint = 325710202,
        /// <summary>
        /// 已经存在指定名称的测试用例
        /// </summary>
        ExistTestCaseByName = 325710203,
        /// <summary>
        /// 找不到指定Id的测试历史用例
        /// </summary>
        NotFoundTestCaseHistoryById = 325710204,
        /// <summary>
        /// 已经存在指定名称的从测试主机
        /// </summary>
        ExistTestCaseSlaveName = 325710205,
        /// <summary>
        /// 找不到指定Id的测试数据源
        /// </summary>
        NotFoundTestCaseDataSourceByID = 325710206,
        /// <summary>
        /// 找不到指定Id的SSHEndPoint
        /// </summary>
        NotFoundSSHEndPointByID = 325710207,
        /// <summary>
        /// 已经存在指定名称的SSH终结点
        /// </summary>
        ExistSSHEndPointByName = 325710208,
        /// <summary>
        /// 已经存在指定名称的主机
        /// </summary>
        ExistTestHostByName = 325710209,
        /// <summary>
        /// 指定地址的主机正在被其它的测试用例使用，不能被删除
        /// </summary>
        TestHostIsUsedByTestCases = 325710210,
        /// <summary>
        /// 指定地址的主机正在被其它的从主机使用，不能被删除
        /// </summary>
        TestHostIsUsedBySlaves = 325710211,
        /// <summary>
        /// 指定名称的SSH终结点正在被其它的主机使用，不能被删除
        /// </summary>
        SSHEndPointIsUsedByTestHosts = 325710212,
        /// <summary>
        /// 从测试机上传测试文件超时
        /// </summary>
        SlaveHostUploadTestFileTimeout= 325710220,
        /// <summary>
        /// 找不到指定路径的从主机日志文件
        /// </summary>
        NotFoundLogFileByPath = 325710221
    }
}
