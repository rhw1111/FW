using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Survey
{
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


        //public static ISurveyResponseCollectorBindService GetSurveyResponseCollectorBindService(string type)
        //{

        //}

    }
}
