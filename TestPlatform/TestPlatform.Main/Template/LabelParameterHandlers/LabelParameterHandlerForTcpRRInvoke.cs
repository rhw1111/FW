using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Template;
using MSLibrary.LanguageTranslate;
using FW.TestPlatform.Main.Code;
using FW.TestPlatform.Main.Entities;
using FW.TestPlatform.Main.Entities.TestCaseHandleServices;
using System.Text.RegularExpressions;

namespace FW.TestPlatform.Main.Template.LabelParameterHandlers
{
    /// <summary>
    ///针对全局数据变量声明的标签参数处理
    ///格式:{$tcprrinvoke(address, port,senddata,receivereg,name=None,self=None)}
    ///要求context中的Parameters中
    ///包含EngineType参数，参数类型为string
    [Injection(InterfaceType = typeof(LabelParameterHandlerForTcpRRInvoke), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForTcpRRInvoke : ILabelParameterHandler
    {
        private readonly ISelector<IFactory<IGetSeparatorService>> _getSeparatorServiceSelector;

        public LabelParameterHandlerForTcpRRInvoke(ISelector<IFactory<IGetSeparatorService>> getSeparatorServiceSelector)
        {
            _getSeparatorServiceSelector = getSeparatorServiceSelector;
        }

        public async Task<string> Execute(TemplateContext context, string[] parameters)
        {
            StringBuilder strCode = new StringBuilder();

            if (parameters.Length < 4)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.LabelParameterCountError,
                    DefaultFormatting = "标签{0}要求的参数个数为{1}，而实际参数个数为{2}",
                    ReplaceParameters = new List<object>() { "{$tcprrinvoke(address,port,senddata,receivereg,name=None,self=None)}", 4, parameters.Length }
                };

                throw new UtilityException((int)Errors.LabelParameterCountError, fragment, 1, 0);
            }

            //Regex regex = new Regex(@"^(\-|\+)?\d+$");

            //if (!regex.IsMatch(parameters[1]))
            //{
            //    var fragment = new TextFragment()
            //    {
            //        Code = TextCodes.LabelParameterTypeError,
            //        DefaultFormatting = "标签{0}要求的参数{1}应为{2}，参数类型错误",
            //        ReplaceParameters = new List<object>() { "{$tcprrinvoke(address,port,senddata,receivereg)}", "port", "Int" }
            //    };

            //    throw new UtilityException((int)Errors.LabelParameterTypeError, fragment, 1, 0);
            //}

            if (parameters.Length == 4)
            {
                strCode.Append($"TcpRR({parameters[0]}\\, {parameters[1]}\\, {parameters[2]}\\, {parameters[3]})");
            }
            else if(parameters.Length == 6)
            {
                strCode.Append($"TcpRR({parameters[0]}\\, {parameters[1]}\\, {parameters[2]}\\, {parameters[3]}\\, {parameters[4]}\\, {parameters[5]})");
            }

            return strCode.ToString();
        }

        public async Task<bool> IsIndividual()
        {
            return await Task.FromResult(false);
        }
    }
}
