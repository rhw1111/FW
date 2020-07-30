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
    ///过滤数据
    ///格式:{$getjsondatainvoke(data,gettype,endtype)}
    ///参数：1，Json字符串 2，0=顺序取值，1=唯一取值，2=随机取值 3，0=结束测试，1=循环使用，2=循环停留最后一个
    ///返回值：过滤后数据
    [Injection(InterfaceType = typeof(LabelParameterHandlerForGetJsonDataInvoke), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForGetJsonDataInvoke : ILabelParameterHandler
    {
        private readonly ISelector<IFactory<IGetSeparatorService>> _getSeparatorServiceSelector;

        public LabelParameterHandlerForGetJsonDataInvoke(ISelector<IFactory<IGetSeparatorService>> getSeparatorServiceSelector)
        {
            _getSeparatorServiceSelector = getSeparatorServiceSelector;
        }

        public async Task<string> Execute(TemplateContext context, string[] parameters)
        {
            StringBuilder strCode = new StringBuilder();

            if (parameters.Length < 3)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.LabelParameterCountError,
                    DefaultFormatting = "标签{0}要求的参数个数为{1}，而实际参数个数为{2}",
                    ReplaceParameters = new List<object>() { "{$getjsondatainvoke(data,gettype,endtype)}", 3, parameters.Length }
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
            //        ReplaceParameters = new List<object>() { "{$getjsondatainvoke(data,gettype,endtype)}", "gettype", "Int" }
            //    };

            //    throw new UtilityException((int)Errors.LabelParameterTypeError, fragment, 1, 0);
            //}

            //if (!regex.IsMatch(parameters[2]))
            //{
            //    var fragment = new TextFragment()
            //    {
            //        Code = TextCodes.LabelParameterTypeError,
            //        DefaultFormatting = "标签{0}要求的参数{1}应为{2}，参数类型错误",
            //        ReplaceParameters = new List<object>() { "{$getjsondatainvoke(data,gettype,endtype)}", "endtype", "Int" }
            //    };

            //    throw new UtilityException((int)Errors.LabelParameterTypeError, fragment, 1, 0);
            //}

            strCode.Append($"GetJsonData({parameters[0]}\\, {parameters[1]}\\, {parameters[2]})");

            return strCode.ToString();
        }

        public async Task<bool> IsIndividual()
        {
            return await Task.FromResult(false);
        }
    }
}
