﻿using System;
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

namespace FW.TestPlatform.Main.Template.LabelParameterHandlers
{
    /// <summary>
    ///针对全局数据变量声明的标签参数处理
    ///格式:{$recvdata()}
    ///要求context中的Parameters中
    ///包含EngineType参数，参数类型为string
    ///包含DataSourceVars参数，参数类型为List<ConfigurationDataForDataSourceVar>
    [Injection(InterfaceType = typeof(LabelParameterHandlerForRecvData), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForRecvData : ILabelParameterHandler
    {
        private readonly ISelector<IFactory<IGenerateDataVarDeclareService>> _generateDataVarDeclareServiceFactorySelector;
        private readonly ISelector<IFactory<IGetSeparatorService>> _getSeparatorServiceSelector;

        public LabelParameterHandlerForRecvData(ISelector<IFactory<IGenerateDataVarDeclareService>> generateDataVarDeclareServiceFactorySelector, ISelector<IFactory<IGetSeparatorService>> getSeparatorServiceSelector)
        {
            _generateDataVarDeclareServiceFactorySelector = generateDataVarDeclareServiceFactorySelector;
            _getSeparatorServiceSelector = getSeparatorServiceSelector;
        }

        public async Task<string> Execute(TemplateContext context, string[] parameters)
        {
            StringBuilder strCode = new StringBuilder();

            strCode.Append($"self.recvdata");

            return strCode.ToString();
        }

        public async Task<bool> IsIndividual()
        {
            return await Task.FromResult(false);
        }
    }
}