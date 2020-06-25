using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;

namespace FW.TestPlatform.Main.Code.GenerateDataVarDeclareServices
{
    /// <summary>
    /// 针对Locust+Int的数据变量声明代码块生成服务
    /// </summary>
    [Injection(InterfaceType = typeof(GenerateDataVarDeclareServiceForLocustInt), Scope = InjectionScope.Singleton)]
    public class GenerateDataVarDeclareServiceForLocustInt : GenerateDataVarDeclareServiceForLocust
    {
    }

}
