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
    ///格式:{$connectinit(space)}
    ///要求context中的Parameters中
    ///包含EngineType参数，参数类型为string
    ///包含DataSourceVars参数，参数类型为List<ConfigurationDataForDataSourceVar>
    [Injection(InterfaceType = typeof(LabelParameterHandlerForConnectInit), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForConnectInit : ILabelParameterHandler
    {
        private readonly ISelector<IFactory<IGetSeparatorService>> _getSeparatorServiceSelector;
        private readonly ISelector<IFactory<IGetSpaceService>> _getSpaceServiceSelector;

        public LabelParameterHandlerForConnectInit(ISelector<IFactory<IGetSeparatorService>> getSeparatorServiceSelector, ISelector<IFactory<IGetSpaceService>> getSpaceServiceSelector)
        {
            _getSeparatorServiceSelector = getSeparatorServiceSelector;
            _getSpaceServiceSelector = getSpaceServiceSelector;
        }

        public async Task<string> Execute(TemplateContext context, string[] parameters)
        {
            if (!context.Parameters.TryGetValue(TemplateContextParameterNames.EngineType, out object? objEngineType))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundParameterInTemplateContextByName,
                    DefaultFormatting = "在模板上下文中找不到名称为{0}的参数",
                    ReplaceParameters = new List<object>() { TemplateContextParameterNames.EngineType }
                };

                throw new UtilityException((int)Errors.NotFoundParameterInTemplateContextByName, fragment, 1, 0);
            }

            var engineType = (string)objEngineType;

            if (!context.Parameters.TryGetValue(TemplateContextParameterNames.ConnectInit, out object? objVars))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundParameterInTemplateContextByName,
                    DefaultFormatting = "在模板上下文中找不到名称为{0}的参数",
                    ReplaceParameters = new List<object>() { TemplateContextParameterNames.ConnectInit }
                };

                throw new UtilityException((int)Errors.NotFoundParameterInTemplateContextByName, fragment, 1, 0);
            }

            var vars = ((ConfigurationDataForTcpConnectInit)objVars).VarSettings;

            StringBuilder strCode = new StringBuilder();
            var separatorService = _getSeparatorServiceSelector.Choose(engineType).Create();
            var strFuncSeparator = await separatorService.GetFuncSeparator();

            if (parameters.Length < 1)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.LabelParameterCountError,
                    DefaultFormatting = "标签{0}要求的参数个数为{1}，而实际参数个数为{2}",
                    ReplaceParameters = new List<object>() { "{$connectinit(space)}", 1, parameters.Length }
                };

                throw new UtilityException((int)Errors.LabelParameterCountError, fragment, 1, 0);
            }

            Regex regex = new Regex(@"^(\-|\+)?\d+$");

            if (!regex.IsMatch(parameters[0]))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.LabelParameterTypeError,
                    DefaultFormatting = "标签{0}要求的参数{1}应为{2}，参数类型错误",
                    ReplaceParameters = new List<object>() { "{$connectinit(space)}", "space", "Int" }
                };

                throw new UtilityException((int)Errors.LabelParameterTypeError, fragment, 1, 0);
            }

            var spaceService = _getSpaceServiceSelector.Choose(engineType).Create();
            var strSpace = await spaceService.GetSpace(int.Parse(parameters[0]));
            bool isFirstLine = true;

            foreach (var item in vars)
            {
                if (!isFirstLine)
                {
                    strCode.Append(strSpace);
                }

                // 如果Content里有换行的话，需事先处理一下
                string strContent = item.Content.Replace("\r\n", strFuncSeparator);
                // 加上缩进
                strContent = strContent.Replace(strFuncSeparator, strFuncSeparator + strSpace);

                if (string.IsNullOrEmpty(item.Name))
                {
                    strCode.Append($"{strContent}");
                }
                else
                {
                    strCode.Append($"{item.Name} = {strContent}");
                }

                strCode.Append(strFuncSeparator);
                isFirstLine = false;
            }

            return strCode.ToString();
        }

        public async Task<bool> IsIndividual()
        {
            return await Task.FromResult(false);
        }
    }
}
