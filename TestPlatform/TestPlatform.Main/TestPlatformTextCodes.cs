using System;
using System.Collections.Generic;
using System.Text;

namespace FW.TestPlatform.Main
{
    public static class TestPlatformTextCodes
    {
        /// <summary>
        /// 找不到指定测试引擎的测试案例处理服务
        /// 格式为“找不到测试引擎为{0}的测试案例处理服务，发生位置为{1}”
        /// {0}：测试引擎类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundTestCaseHandleServiceByEngine = "NotFoundTestCaseHandleServiceByEngine";
        /// <summary>
        /// 找不到指定Id的测试主机
        /// 格式为“找不到Id为{0}的测试主机”
        /// {0}：测试主机ID
        /// </summary>
        public const string NotFoundTestHostByID = "NotFoundTestHostByID";
        /// <summary>
        /// 在指定的测试案例中找不到指定的从压测主机
        /// 格式为“在id为{0}的测试案例中找不到id为{1}的从测试主机”
        /// {0}：测试案例ID
        /// {1}：测试主机ID
        /// </summary>
        public const string NotFoundSlaveHostInCase = "NotFoundSlaveHostInCase";
        /// <summary>
        /// 只能在指定状态下修改测试案例
        /// 格式为“只能在状态{0}的时候允许修改测试案例，当前测试案例{1}的状态为{2}”
        /// {0}：允许进行修改时的状态
        /// {1}：测试案例ID
        /// {2}：测试案例实际状态
        /// </summary>
        public const string StatusErrorOnTestCaseUpdate = "StatusErrorOnTestCaseUpdate";
        /// <summary>
        /// 只能在指定状态下删除测试案例
        /// 格式为“只能在状态{0}的时候允许删除测试案例，当前测试案例{1}的状态为{2}”
        /// {0}：允许进行删除时的状态
        /// {1}：测试案例ID
        /// {2}：测试案例实际状态
        /// </summary>
        public const string StatusErrorOnTestCaseDelete = "StatusErrorOnTestCaseDelete";
        /// <summary>
        /// 只能在指定状态下运行测试案例
        /// 格式为“只能在状态{0}的时候允许运行测试案例，当前测试案例{1}的状态为{2}”
        /// {0}：允许进行运行时的状态
        /// {1}：测试案例ID
        /// {2}：测试案例实际状态
        /// </summary>
        public const string StatusErrorOnTestCaseRun = "StatusErrorOnTestCaseRun";
        /// <summary>
        /// 只能在指定状态下停止测试案例
        /// 格式为“只能在状态{0}的时候允许停止测试案例，当前测试案例{1}的状态为{2}”
        /// {0}：允许进行停止时的状态
        /// {1}：测试案例ID
        /// {2}：测试案例实际状态
        /// </summary>
        public const string StatusErrorOnTestCaseStop = "StatusErrorOnTestCaseStop";
        /// <summary>
        /// 指定的测试主机已经被执行
        /// 格式为“包含的测试主机已经被执行，相关测试案例为{0}”
        /// {0}:测试案例的名称列表
        /// </summary>
        public const string TestHostHasRunning = "TestHostHasRunning";
        /// <summary>
        /// 找不到指定类型的Locust测试代码生成服务
        /// 格式为“找不到类型为{0}的Locust测试代码生成服务，发生位置为{1}”
        /// {0}：测试类型
        /// {1}：发生位置
        /// </summary>
        public const string NotFoundLocustPyCodeGenerateServiceByType = "NotFoundLocustPyCodeGenerateServiceByType";
        /// <summary>
        /// 找不到指定类型的测试数据源脚本方法生成服务
        /// 格式为“找不到类型为{0}的测试数据源脚本方法生成服务，发生位置为{1}”
        /// {0}：引擎类型+数据源类型
        /// {1}：发生位置
        /// </summary>
        public const string NotFoundTestDataSourceScriptFuncGenerateServiceByType = "NotFoundTestDataSourceScriptFuncGenerateServiceByType";
        /// <summary>
        /// 找不到指定名称的脚本模板
        /// 格式为“找不到名称为{0}的脚本模板”
        /// {0}：模板名称
        /// </summary>
        public const string NotFoundScriptTemplateByName = "NotFoundScriptTemplateByName";
        /// <summary>
        /// 找不到指定名称的数据源函数生成服务
        /// 格式为“找不到名称为{0}的数据源函数生成服务，发生位置为{1}”
        /// {0}：名称
        /// {1}：发生位置
        /// </summary>
        public const string NotFoundGenerateDataSourceFuncServiceByName = "NotFoundGenerateDataSourceFuncServiceByName";
        /// <summary>
        /// 找不到指定名称的获取分隔符服务
        /// 格式为“找不到名称为{0}的获取分隔符服务，发生位置为{1}”
        /// {0}：名称
        /// {1}：发生位置
        /// </summary>
        public const string NotFoundGetSeparatorServiceByName = "NotFoundGetSeparatorServiceByName";
        /// <summary>
        /// 在上下文参数DataSourceFuncs中找不到指定函数名称的记录
        /// 格式为“在上下文参数DataSourceFuncs中找不到函数名称为{0}的记录，发生位置为{1}”
        /// {0}：函数名称
        /// {1}：发生位置
        /// </summary>
        public const string NotFoundFuncNameInDataSourceFuncsFormContext = "NotFoundFuncNameInDataSourceFuncsFormContext";
        /// <summary>
        /// 找不到指定键值的数据源函数调用脚本生成服务
        /// 格式为“找不到测试引擎类型为{0}、函数类型为{1}的数据源函数调用脚本生成服务，发生位置为{2}”
        /// {0}：测试引擎类型
        /// {1}：函数类型
        /// {2}：发生位置
        /// </summary>
        public const string NotFoundDataSourceInvokeScriptGenerateServiceByKey = "NotFoundDataSourceInvokeScriptGenerateServiceByKey";
        /// <summary>
        /// 已经存在指定名称的测试数据源
        /// 格式为“已经存在名称为{0}的测试数据源”
        /// {0}：名称
        /// </summary>
        public const string ExistTestDataSourceByName = "ExistTestDataSourceByName";

        /// <summary>
        /// 找不到指定名称的数据变量声明代码块生成服务
        /// 格式为“找不到名称为{1}的数据变量声明代码块生成服务,发生位置为{1}”
        /// {0}：名称
        /// {1}：发生位置
        /// </summary>
        public const string NotFoundGenerateDataVarDeclareServiceByName = "NotFoundGenerateDataVarDeclareServiceByName";
        /// <summary>
        /// 找不到指定名称的测试数据源
        /// 格式为“找不到名称为{0}的测试数据源”
        /// {0}：数据源名称
        /// </summary>
        public const string NotFoundTestDataSourceByName = "NotFoundTestDataSourceByName";
        /// <summary>
        /// 找不到指定名称的附加函数生成服务
        /// 格式为“找不到名称为{0}的附件函数生成服务，发生位置为{1}”
        /// {0}：名称
        /// {1}：发生位置
        /// </summary>
        public const string NotFoundGenerateAdditionFuncServiceByName = "NotFoundGenerateAdditionFuncServiceByName";
        /// <summary>
        /// 找不到指定Id的测试案例
        /// 格式为“找不到Id为{0}的测试案例”
        /// {0}：测试案例ID
        /// </summary>
        public const string NotFoundTestCaseByID = "NotFoundTestCaseByID";

        /// <summary>
        /// 找不到指定名称的InfluxDB数据源配置
        /// 格式为“找不到指定名称{0}的InfluxDB数据源配置”
        /// {0}：名称
        /// </summary>
        public const string NotFoundInfluxDBEndpoint = "NotFoundInfluxDBEndpoint";

        /// <summary>
        /// 已经存在指定名称的测试用例
        /// 格式为“已经存在名称为{0}的测试用例”
        /// {0}：名称
        /// </summary>
        public const string ExistTestCaseByName = "ExistTestCaseByName";

        /// <summary>
        /// 找不到指定Id的测试历史
        /// 格式为“找不到测试历史Id为{0}并且测试用例Id为{1}的历史”
        /// {0}：Id
        /// </summary>
        public const string NotFoundTestCaseHistoryByID = "NotFoundTestCaseHistoryByID";

        /// <summary>
        /// 已经存在指定名称的从测试主机
        /// 格式为“已经存在名称为{0}的从测试主机”
        /// {0}：名称
        /// </summary>
        public const string ExistTestCaseSlaveByName = "ExistTestCaseSlaveByName";

        /// <summary>
        /// 已经存在指定名称的SSH终结点
        /// 格式为“已经存在名称为{0}的SSH终结点”
        /// {0}：名称
        /// </summary>
        public const string ExistSSHEndPointByName = "ExistSSHEndPointByName";

        /// <summary>
        /// 已经存在指定名称的主机
        /// 格式为“已经存在地址为{0}的主机”
        /// {0}：名称
        /// </summary>
        public const string ExistTestHostByName = "ExistTestHostByName";

        /// <summary>
        /// 找不到指定Id的测试数据源
        /// 格式为“找不到测试数据源Id为{0}的测试数据源”
        /// {0}：Id
        /// </summary>
        public const string NotFoundTestCaseDataSourceByID = "NotFoundTestCaseDataSourceByID";
        /// <summary>
        /// 找不到指定Id的SSH终结点
        /// 格式为“找不到SSH终结点Id为{0}的SSH终结点”
        /// {0}：Id
        /// </summary>
        public const string NotFoundSSHEndPointByID = "NotFoundSSHEndPointByID";
        /// <summary>
        /// 指定地址的主机不能被删除
        /// 格式为“地址为{0}的主机正在被其它的测试用例使用，不能被删除”
        /// {0}：地址
        /// </summary>
        public const string TestHostIsUsedByTestCases = "TestHostIsUsedByTestCases";
        /// <summary>
        /// 指定地址的主机不能被删除
        /// 格式为“地址为{0}的主机正在被其它的从主机使用，不能被删除”
        /// {0}：地址
        /// </summary>
        public const string TestHostIsUsedBySlaves = "TestHostIsUsedBySlaves";
        /// <summary>
        /// 指定名称的SSH终结点不能被删除
        /// 格式为“名称为{0}的SSH终结点正在被其它的主机使用，不能被删除”
        /// {0}：名称
        /// </summary>
        public const string SSHEndpointIsUsedByTestHosts = "SSHEndpointIsUsedByTestHosts";
        /// <summary>
        /// 从测试机上传测试文件超时
        /// 格式为“从测试机{0}上传测试文件超时”
        /// {0}：测试机名称
        /// </summary>
        public const string SlaveHostUploadTestFileTimeout = "SlaveHostUploadTestFileTimeout";
    }
}
