using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.Template;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Template.LabelParameterHandlers
{
    /// <summary>
    /// 当前时间字符串
    /// 格式:{$currentdatetime(yyyyMMdd)}
    /// 参数为时间的格式化字符串
    /// </summary>
    [Injection(InterfaceType = typeof(LabelParameterHandlerForDateTime), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForDateTime : ILabelParameterHandler
    {
        public async Task<string> Execute(TemplateContext context, string[] parameters)
        {
            if (parameters.Length != 1)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.LabelParameterCountError,
                    DefaultFormatting = "标签{0}要求的参数个数为{1}，而实际参数个数为{2}",
                    ReplaceParameters = new List<object>() { "currentutcdatetime", "1", parameters.Length.ToString() }
                };

                throw new UtilityException((int)Errors.LabelParameterCountError, fragment);
            }
            var format = parameters[0];

            var strDate = DateTime.Now.ToString(format);

            return await Task.FromResult(strDate);
        }

        public async Task<bool> IsIndividual()
        {
            return await Task.FromResult(false);
        }
    }
}
