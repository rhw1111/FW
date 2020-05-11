using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Template.LabelParameterHandlers
{

    /// <summary>
    /// 字符串格式化
    /// {$stringformat(格式化的字符串，参数1，参数2，...)}
    /// </summary>
    [Injection(InterfaceType = typeof(LabelParameterHandlerForStringFormat), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForStringFormat : ILabelParameterHandler
    {
        public async Task<string> Execute(TemplateContext context, string[] parameters)
        {
            if (parameters.Length < 1)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.LabelParameterCountRequireMin,
                    DefaultFormatting = "标签{0}要求的参数个数至少为{1}，而实际参数个数为{2}",
                    ReplaceParameters = new List<object>() { "stringformat", "1", parameters.Length.ToString() }
                };

                throw new UtilityException((int)Errors.LabelParameterCountRequireMin,fragment);
            }
            string code = parameters[0];
            if (parameters.Length>1)
            {
                var newParas = new string[parameters.Length - 1];
                for(var index=0;index<= parameters.Length-2;index++)
                {
                    newParas[index] = parameters[index + 1];
                }
                return await Task.FromResult(string.Format(code.ToFormat(), newParas));
            }
            else
            {
                return await Task.FromResult(parameters[0]);
            }
        }

        public async Task<bool> IsIndividual()
        {
            return await Task.FromResult(false);
        }
    }
}
