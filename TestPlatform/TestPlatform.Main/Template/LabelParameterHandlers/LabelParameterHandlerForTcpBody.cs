using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Template;
using MSLibrary.LanguageTranslate;

namespace FW.TestPlatform.Main.Template.LabelParameterHandlers
{
    /// <summary>
    ///针对Tcp请求体的标签参数处理
    ///格式:{$tcpbody()}
    ///要求context中的Parameters中包含RequestBody参数，参数类型为string
    /// </summary>
    [Injection(InterfaceType = typeof(LabelParameterHandlerForTcpBody), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForTcpBody : ILabelParameterHandler
    {
        private readonly ITemplateService _templateService;

        public LabelParameterHandlerForTcpBody(ITemplateService templateService)
        {
            _templateService = templateService;
        }
        public async Task<string> Execute(TemplateContext context, string[] parameters)
        {
            if (!context.Parameters.TryGetValue(TemplateContextParameterNames.RequestBody,out object? objRequestBody))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundParameterInTemplateContextByName,
                    DefaultFormatting = "在模板上下文中找不到名称为{0}的参数",
                    ReplaceParameters = new List<object>() { TemplateContextParameterNames.RequestBody }
                };

                throw new UtilityException((int)Errors.NotFoundParameterInTemplateContextByName, fragment, 1, 0);
            }

            var requestBody = objRequestBody.ToString();

            return await _templateService.Convert(requestBody, context);
        }

        public async Task<bool> IsIndividual()
        {
            return await Task.FromResult(true);
        }
    }
}
