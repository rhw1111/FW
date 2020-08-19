using System;
using System.Collections.Generic;
using System.Text;

namespace FW.TestPlatform.Main
{
    /// <summary>
    /// 测试案例状态
    /// </summary>
    public enum TestCaseStatus 
    {
        NoRun=0,
        Running=1,
        Stop=2
    }

    /// <summary>
    /// 网关文件分析状态
    /// </summary>
    public enum NetGatewayDataFileStatus
    {
        HasFileUnfinished = 0,
        NoFileUnfinished = 1
    }
}
