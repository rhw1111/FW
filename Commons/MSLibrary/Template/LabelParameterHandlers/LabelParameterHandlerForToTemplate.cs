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
    /// 转义模板字符串
    /// 格式:{$totemplate(文本)}
    /// 参数为要转义的文本
    /// </summary>
    [Injection(InterfaceType = typeof(LabelParameterHandlerForToTemplate), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForToTemplate : ILabelParameterHandler
    {
        public async Task<string> Execute(TemplateContext context, string[] parameters)
        {
            if (parameters.Length != 1)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.LabelParameterCountError,
                    DefaultFormatting = "标签{0}要求的参数个数为{1}，而实际参数个数为{2}",
                    ReplaceParameters = new List<object>() { "totemplate", "1", parameters.Length.ToString() }
                };

                throw new UtilityException((int)Errors.LabelParameterCountError, fragment);
            }
            var text = parameters[0];        

            return await Task.FromResult(text.ToTemplate());
        }

        public async Task<bool> IsIndividual()
        {
            return await Task.FromResult(false);
        }
    }
}
