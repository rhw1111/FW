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
    /// Html执行函数
    /// 格式:{$htmlinvokefun(js方法名称，参数1，参数2，...)}
    /// </summary>
    [Injection(InterfaceType = typeof(LabelParameterHandlerForHtmlInvokeFun), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForHtmlInvokeFun : ILabelParameterHandler
    {
        public async Task<string> Execute(TemplateContext context, string[] parameters)
        {
            if (parameters.Length < 1)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.LabelParameterCountRequireMin,
                    DefaultFormatting = "标签{0}要求的参数个数至少为{1}，而实际参数个数为{2}",
                    ReplaceParameters = new List<object>() { "htmlinvokefun", "1", parameters.Length.ToString() }
                };

                throw new UtilityException((int)Errors.LabelParameterCountRequireMin, fragment);
            }

            string funName = parameters[0];
            if (parameters.Length > 1)
            {
                StringBuilder strParas = new StringBuilder();
                for (var index = 0; index <= parameters.Length - 2; index++)
                {
                    strParas.Append($"\\\"{parameters[index + 1].ToJS()}\\\"");
                    if (index!=parameters.Length-2)
                    {
                        strParas.Append("\\,");
                    }                     ;
                }
                return await Task.FromResult($"{funName.ToJS()}({strParas.ToString()});");
            }
            else
            {
                return await Task.FromResult($"{funName.ToJS()}();");
            }
        }

        public async Task<bool> IsIndividual()
        {
            return await Task.FromResult(false);
        }
    }
}
