using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Template.LabelParameterHandlers
{
    /// <summary>
    /// 多语言文本段
    /// {$languagetextsegment(编码值,默认值)}
    /// </summary>
    [Injection(InterfaceType = typeof(LabelParameterHandlerForLanguageTextSegment), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForLanguageTextSegment : ILabelParameterHandler
    {
        public async Task<string> Execute(TemplateContext context, string[] parameters)
        {

            string result = "";
            if (parameters.Length != 2)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.LabelParameterCountError,
                    DefaultFormatting = "标签{0}要求的参数个数为{1}，而实际参数个数为{2}",
                    ReplaceParameters = new List<object>() { "languagetextsegment", "2", parameters.Length.ToString() }
                };

                throw new UtilityException((int)Errors.LabelParameterCountError, fragment);
            }
            string code = parameters[0];
            string strDefault = parameters[1];
            result = StringLanguageTranslate.Translate(code, strDefault);
            return await Task.FromResult(result);
        }

        public async Task<bool> IsIndividual()
        {
            return await Task.FromResult(false);
        }
    }
}
