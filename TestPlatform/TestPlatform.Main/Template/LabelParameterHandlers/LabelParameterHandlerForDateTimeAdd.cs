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
    ///格式:{$datetimeadd(dt,years=0,months=0,weeks=0,days=0,hours=0,minutes=0,seconds=0,microseconds=0,milliseconds=0)}
    ///要求context中的Parameters中
    ///包含EngineType参数，参数类型为string
    [Injection(InterfaceType = typeof(LabelParameterHandlerForDateTimeAdd), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForDateTimeAdd : ILabelParameterHandler
    {
        private readonly ISelector<IFactory<IGetSeparatorService>> _getSeparatorServiceSelector;

        public LabelParameterHandlerForDateTimeAdd(ISelector<IFactory<IGetSeparatorService>> getSeparatorServiceSelector)
        {
            _getSeparatorServiceSelector = getSeparatorServiceSelector;
        }

        public async Task<string> Execute(TemplateContext context, string[] parameters)
        {
            StringBuilder strCode = new StringBuilder();

            if (parameters.Length < 2)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.LabelParameterCountError,
                    DefaultFormatting = "标签{0}要求的参数个数为{1}，而实际参数个数为{2}",
                    ReplaceParameters = new List<object>() { "{$datetimeadd(dt,years=0,months=0,weeks=0,days=0,hours=0,minutes=0,seconds=0,microseconds=0,milliseconds=0)}", 2, parameters.Length }
                };

                throw new UtilityException((int)Errors.LabelParameterCountError, fragment, 1, 0);
            }

            strCode.Append($"DateTimeAdd({parameters[0]}\\, {parameters[1]})");

            return strCode.ToString();
        }

        public async Task<bool> IsIndividual()
        {
            return await Task.FromResult(false);
        }
    }
}
