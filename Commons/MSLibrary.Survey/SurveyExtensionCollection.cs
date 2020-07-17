using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Survey
{
    /// <summary>
    /// Survey扩展点集合
    /// 系统初始化时需要赋值
    /// </summary>
    public static class SurveyExtensionCollection
    {
        /// <summary>
        /// Survey收集器绑定服务键值对
        /// 键为SurveyEndpoint.Type
        /// </summary>
        public static IDictionary<string, IFactory<ISurveyCollectorBindService>> SurveyCollectorBindServiceFactories { get; } = new Dictionary<string, IFactory<ISurveyCollectorBindService>>();

        /// <summary>
        /// Survey收集器工厂键值对
        /// 键为SurveyEndpoint.Type
        /// </summary>
        public static IDictionary<string, IFactory<ISurveyCollectorFactory>> SurveyCollectorFactories { get; } = new Dictionary<string, IFactory<ISurveyCollectorFactory>>();

        /// <summary>
        /// Survey收集器终结点终止服务工厂键值对
        /// 键为SurveyEndpoint.Type
        /// </summary>
        public static IDictionary<string, IFactory<ISurveyEndpointFinanlyService>> SurveyEndpointFinanlyServiceFactories { get; } = new Dictionary<string, IFactory<ISurveyEndpointFinanlyService>>();

        /// <summary>
        /// Survey收集器终结点初始化服务工厂键值对
        /// 键为SurveyEndpoint.Type
        /// </summary>
        public static IDictionary<string, IFactory<ISurveyEndpointInitService>> SurveyEndpointInitServiceFactories { get; } = new Dictionary<string, IFactory<ISurveyEndpointInitService>>();

        /// <summary>
        /// Survey收集器数据查询服务工厂键值对
        /// 键为SurveyEndpoint.Type
        /// </summary>
        public static IDictionary<string, IFactory<ISurveyCollectorDataQueryService>> SurveyCollectorDataQueryServiceFactories { get; } = new Dictionary<string, IFactory<ISurveyCollectorDataQueryService>>();

        /// <summary>
        /// Survey数据查询服务工厂键值对
        /// 键为SurveyEndpoint.Type
        /// </summary>
        public static IDictionary<string, IFactory<ISurveyResponseDataQueryService>> SurveyResponseDataQueryServiceFactories { get; } = new Dictionary<string, IFactory<ISurveyResponseDataQueryService>>();

        /// <summary>
        /// Survey响应数据的ID解析服务键值对
        /// 键为SurveyEndpoint.Type
        /// </summary>
        public static IDictionary<string, IFactory<ISurveyResponseIDResolveService>> SurveyResponseIDResolveServiceFactories { get; } = new Dictionary<string, IFactory<ISurveyResponseIDResolveService>>();
        /// <summary>
        /// Survey收集器可用性检查服务键值对
        /// 键为SurveyEndpoint.Type
        /// </summary>
        public static IDictionary<string, IFactory<ISurveyCollectorEnableCheckService>> SurveyCollectorEnableCheckServiceFactories { get; } = new Dictionary<string, IFactory<ISurveyCollectorEnableCheckService>>();

        /// <summary>
        /// Survey接收者生成服务键值对
        /// 键为SurveyEndpoint.Type+SurveyRecord.RecipientConfigurationType
        /// </summary>
        public static IDictionary<string, IFactory<ISurveyRecipientGenerateService>> SurveyRecipientGenerateServiceFactories { get; } = new Dictionary<string, IFactory<ISurveyRecipientGenerateService>>();



        public static ISurveyCollectorBindService GetSurveyCollectorBindService(string type)
        {
            if (!SurveyCollectorBindServiceFactories.TryGetValue(type, out IFactory<ISurveyCollectorBindService> surveyResponseCollectorBindServiceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyCollectorBindServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Survey收集器绑定服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() {type, "SurveyExtensionCollection.SurveyCollectorBindServiceFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyCollectorBindServiceByType, fragment, 1, 0);
            }

            return surveyResponseCollectorBindServiceFactory.Create();
        }


        public static ISurveyCollectorFactory GetSurveyCollectorFactory(string type)
        {
            if (!SurveyCollectorFactories.TryGetValue(type, out IFactory<ISurveyCollectorFactory> surveyCollectorFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyCollectorFactoryByType,
                    DefaultFormatting = "找不到类型为{0}的Survey收集器工厂，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type, "SurveyExtensionCollection.SurveyCollectorFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyCollectorFactoryByType, fragment, 1, 0);
            }

            return surveyCollectorFactory.Create();
        }

        public static ISurveyEndpointFinanlyService GetSurveyEndpointFinanlyService(string type)
        {
            if (!SurveyEndpointFinanlyServiceFactories.TryGetValue(type, out IFactory<ISurveyEndpointFinanlyService> surveyEndpointFinanlyServiceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyEndpointFinanlyServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Survey终结点终止服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type, "SurveyExtensionCollection.SurveyEndpointFinanlyServiceFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyEndpointFinanlyServiceByType, fragment, 1, 0);
            }
            return surveyEndpointFinanlyServiceFactory.Create();
        }

        public static ISurveyEndpointInitService GetSurveyEndpointInitService(string type)
        {
            if (!SurveyEndpointInitServiceFactories.TryGetValue(type, out IFactory<ISurveyEndpointInitService> surveyEndpointInitServiceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyEndpointInitServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Survey终结点初始化服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type, "SurveyExtensionCollection.SurveyEndpointInitServiceFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyEndpointInitServiceByType, fragment, 1, 0);
            }

            return surveyEndpointInitServiceFactory.Create();
        }

        public static ISurveyCollectorDataQueryService GetSurveyCollectorDataQueryService(string type)
        {
            if (!SurveyCollectorDataQueryServiceFactories.TryGetValue(type, out IFactory<ISurveyCollectorDataQueryService> surveyCollectorDataQueryServiceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyCollectorDataQueryServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Survey收集器数据查询服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type, "SurveyExtensionCollection.SurveyCollectorDataQueryServiceFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyCollectorDataQueryServiceByType, fragment, 1, 0);
            }

            return surveyCollectorDataQueryServiceFactory.Create();
        }

        public static ISurveyResponseDataQueryService GetSurveyResponseDataQueryService(string type)
        {
            if (!SurveyResponseDataQueryServiceFactories.TryGetValue(type, out IFactory<ISurveyResponseDataQueryService> surveyResponseDataQueryServiceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyResponseDataQueryServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Survey响应数据查询服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type, "SurveyResponseExtensionCollection.SurveyResponseDataQueryServiceFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyResponseDataQueryServiceByType, fragment, 1, 0);
            }

            return surveyResponseDataQueryServiceFactory.Create();
        }

        public static ISurveyResponseIDResolveService GetSurveyResponseIDResolveService(string type)
        {
            if (!SurveyResponseIDResolveServiceFactories.TryGetValue(type, out IFactory<ISurveyResponseIDResolveService> surveyResponseIDResolveServiceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyResponseDataIDResolveServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Survey响应数据ID解析服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type, "SurveyResponseExtensionCollection.SurveyResponseIDResolveServiceFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyResponseDataIDResolveServiceByType, fragment, 1, 0);
            }

            return surveyResponseIDResolveServiceFactory.Create();
        }

        public static ISurveyCollectorEnableCheckService GetSurveyCollectorEnableCheckService(string type)
        {
            if (!SurveyCollectorEnableCheckServiceFactories.TryGetValue(type, out IFactory<ISurveyCollectorEnableCheckService> surveyCollectorEnableCheckServiceeFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyCollectorEnableCheckServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Survey收集器可用性检查服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type, "SurveyExtensionCollection.SurveyCollectorEnableCheckServiceFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyCollectorEnableCheckServiceByType, fragment, 1, 0);
            }

            return surveyCollectorEnableCheckServiceeFactory.Create();
        }

        public static ISurveyRecipientGenerateService GetSurveyRecordRecipientGenerateService(string type)
        {
            if (!SurveyRecipientGenerateServiceFactories.TryGetValue(type, out IFactory<ISurveyRecipientGenerateService> surveyRecipientGenerateServiceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyRecipientGenerateServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Survey接收者生成服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type, "SurveyExtensionCollection.SurveyRecipientGenerateServiceFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyRecipientGenerateServiceByType, fragment, 1, 0);
            }

            return surveyRecipientGenerateServiceFactory.Create();
        }
    }


}
