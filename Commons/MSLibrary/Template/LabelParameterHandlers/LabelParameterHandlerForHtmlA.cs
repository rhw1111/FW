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
    /// HTML中的A标签
    /// 格式:{$htmla(文本，class名称，onclick方法)}
    /// 参数1：要显示的文本
    /// 参数2：样式名称
    /// 参数3：onclick调用的js
    /// </summary>
    [Injection(InterfaceType = typeof(LabelParameterHandlerForHtmlA), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForHtmlA : ILabelParameterHandler
    {
        public async Task<string> Execute(TemplateContext context, string[] parameters)
        {
            if (parameters.Length != 3)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.LabelParameterCountError,
                    DefaultFormatting = "标签{0}要求的参数个数为{1}，而实际参数个数为{2}",
                    ReplaceParameters = new List<object>() { "htmla", "3", parameters.Length.ToString() }
                };

                throw new UtilityException((int)Errors.LabelParameterCountError, fragment);
            }
            string text = parameters[0];
            string className= parameters[1];
            string onclick = parameters[2];
            string strClassName = string.Empty;
            if (!string.IsNullOrEmpty(className))
            {
                strClassName=$"class=\"{className.ToHTML()}\"";
            }
            string strOnclick = string.Empty;
            if (!string.IsNullOrEmpty(onclick))
            {
                strOnclick = $"onclick=\"{onclick.ToHTML()}\"";
            }

            return await Task.FromResult($"<a {strClassName} {strOnclick}>{text.ToHTML()}</a>");
        }

        public async Task<bool> IsIndividual()
        {
            return await Task.FromResult(false);
        }
    }
}
