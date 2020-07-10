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
    /// Survey响应扩展点集合
    /// 系统初始化时需要赋值
    /// </summary>
    public static class SurveyResponseExtensionCollection
    {
        /// <summary>
        /// Survey响应收集器绑定服务键值对
        /// 键为SurveyResponseCollectorEndpoint.Type
        /// </summary>
        public static IDictionary<string, IFactory<ISurveyResponseCollectorBindService>> SurveyResponseCollectorBindServiceFactories { get; } = new Dictionary<string, IFactory<ISurveyResponseCollectorBindService>>();

        /// <summary>
        /// Survey响应收集器工厂键值对
        /// 键为SurveyResponseCollectorEndpoint.Type
        /// </summary>
        public static IDictionary<string, IFactory<ISurveyResponseCollectorFactory>> SurveyResponseCollectorFactories { get; } = new Dictionary<string, IFactory<ISurveyResponseCollectorFactory>>();

        /// <summary>
        /// Survey响应收集器终结点终止服务工厂键值对
        /// 键为SurveyResponseCollectorEndpoint.Type
        /// </summary>
        public static IDictionary<string, IFactory<ISurveyResponseCollectorEndpointFinanlyService>> SurveyResponseCollectorEndpointFinanlyServiceFactories { get; } = new Dictionary<string, IFactory<ISurveyResponseCollectorEndpointFinanlyService>>();

        /// <summary>
        /// Survey响应收集器终结点初始化服务工厂键值对
        /// 键为SurveyResponseCollectorEndpoint.Type
        /// </summary>
        public static IDictionary<string, IFactory<ISurveyResponseCollectorEndpointInitService>> SurveyResponseCollectorEndpointInitServiceFactories { get; } = new Dictionary<string, IFactory<ISurveyResponseCollectorEndpointInitService>>();

        /// <summary>
        /// Survey响应收集器数据查询服务工厂键值对
        /// 键为SurveyResponseCollectorEndpoint.Type
        /// </summary>
        public static IDictionary<string, IFactory<ISurveyResponseCollectorDataQueryService>> SurveyResponseCollectorDataQueryServiceFactories { get; } = new Dictionary<string, IFactory<ISurveyResponseCollectorDataQueryService>>();

        /// <summary>
        /// Survey响应数据查询服务工厂键值对
        /// 键为SurveyResponseCollectorEndpoint.Type
        /// </summary>
        public static IDictionary<string, IFactory<ISurveyResponseDataQueryService>> SurveyResponseDataQueryServiceFactories { get; } = new Dictionary<string, IFactory<ISurveyResponseDataQueryService>>();

        /// <summary>
        /// Survey响应数据的ID解析服务键值对
        /// 键为SurveyResponseCollectorEndpoint.Type
        /// </summary>
        public static IDictionary<string, IFactory<ISurveyResponseIDResolveService>> SurveyResponseIDResolveServiceFactories { get; } = new Dictionary<string, IFactory<ISurveyResponseIDResolveService>>();

        public static ISurveyResponseCollectorBindService GetSurveyResponseCollectorBindService(string type)
        {
            if (!SurveyResponseCollectorBindServiceFactories.TryGetValue(type, out IFactory<ISurveyResponseCollectorBindService> surveyResponseCollectorBindServiceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyResponseCollectorBindServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Survey响应收集器绑定服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() {type, "SurveyResponseExtensionCollection.SurveyResponseCollectorBindServiceFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyResponseCollectorBindServiceByType, fragment, 1, 0);
            }

            return surveyResponseCollectorBindServiceFactory.Create();
        }


        public static ISurveyResponseCollectorFactory GetSurveyResponseCollectorFactory(string type)
        {
            if (!SurveyResponseCollectorFactories.TryGetValue(type, out IFactory<ISurveyResponseCollectorFactory> surveyResponseCollectorFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyResponseCollectorFactoryByType,
                    DefaultFormatting = "找不到类型为{0}的Survey响应收集器工厂，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type, "SurveyResponseExtensionCollection.SurveyResponseCollectorFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyResponseCollectorFactoryByType, fragment, 1, 0);
            }

            return surveyResponseCollectorFactory.Create();
        }

        public static ISurveyResponseCollectorEndpointFinanlyService GetSurveyResponseCollectorEndpointFinanlyService(string type)
        {
            if (!SurveyResponseCollectorEndpointFinanlyServiceFactories.TryGetValue(type, out IFactory<ISurveyResponseCollectorEndpointFinanlyService> surveyResponseCollectorEndpointFinanlyServiceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyResponseCollectorEndpointFinanlyServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Survey响应收集器终结点终止服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type, "SurveyResponseExtensionCollection.SurveyResponseCollectorEndpointFinanlyServiceFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyResponseCollectorEndpointFinanlyServiceByType, fragment, 1, 0);
            }
            return surveyResponseCollectorEndpointFinanlyServiceFactory.Create();
        }

        public static ISurveyResponseCollectorEndpointInitService GetSurveyResponseCollectorEndpointInitService(string type)
        {
            if (!SurveyResponseCollectorEndpointInitServiceFactories.TryGetValue(type, out IFactory<ISurveyResponseCollectorEndpointInitService> surveyResponseCollectorEndpointInitServiceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyResponseCollectorEndpointInitServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Survey响应收集器终结点初始化服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type, "SurveyResponseExtensionCollection.SurveyResponseCollectorEndpointInitServiceFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyResponseCollectorEndpointInitServiceByType, fragment, 1, 0);
            }

            return surveyResponseCollectorEndpointInitServiceFactory.Create();
        }

        public static ISurveyResponseCollectorDataQueryService GetSurveyResponseCollectorDataQueryService(string type)
        {
            if (!SurveyResponseCollectorDataQueryServiceFactories.TryGetValue(type, out IFactory<ISurveyResponseCollectorDataQueryService> surveyResponseCollectorDataQueryServiceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyResponseCollectorDataQueryServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Survey响应收集器数据查询服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type, "SurveyResponseExtensionCollection.SurveyResponseCollectorDataQueryServiceFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyResponseCollectorDataQueryServiceByType, fragment, 1, 0);
            }

            return surveyResponseCollectorDataQueryServiceFactory.Create();
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
    }


}
