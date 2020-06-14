using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace FW.TestPlatform.Main.Template.LabelParameterHandlers.DataSourceInvokeScriptGenerateServices
{
    /// <summary>
    /// 基于Locust的针对Json数据源的函数调用
    /// parameters中参数如下
    /// [0]:要获取的属性名称
    /// 
    /// </summary>
    [Injection(InterfaceType = typeof(DataSourceInvokeScriptGenerateServiceForLocustJson), Scope = InjectionScope.Singleton)]
    public class DataSourceInvokeScriptGenerateServiceForLocustJson : IDataSourceInvokeScriptGenerateService
    {
        public Task<string> Generate(string funcUniqueName, string[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
