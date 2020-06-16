using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<string> Generate(string funcUniqueName, string[] parameters)
        {
            //throw new NotImplementedException();

            // 格式:{$datasourcefuncinvoke(funcname,...)}

            if (parameters.Length < 1)
            {
                throw new ArgumentException();
            }

            string funcname = parameters[0];
            string parameters_str = string.Empty;

            if (parameters.Length > 1)
            {
                parameters_str = string.Join(", ", parameters.Skip(1).Take(parameters.Length).ToArray());
            }

            string code = string.Format("{0}({1})", funcname, parameters_str);

            return code;
        }
    }
}
