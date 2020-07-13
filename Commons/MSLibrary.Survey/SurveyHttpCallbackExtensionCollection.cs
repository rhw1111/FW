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
    /// Survey的Http回调扩展点集合
    /// </summary>
    public static class SurveyHttpCallbackExtensionCollection
    {
        /// <summary>
        /// Survey的Http回调解析服务键值对
        /// 键为SurveyResponseCollectorEndpoint.Type
        /// </summary>
        public static IDictionary<string, IFactory<ISurveyHttpCallbackResolveService>> SurveyHttpCallbackResolveServiceFactories { get; } = new Dictionary<string, IFactory<ISurveyHttpCallbackResolveService>>();

        /// <summary>
        /// Survey的Http回调处理服务
        /// 键位SurveyResponseCollectorEndpoint.Type+SurveyHttpCallbackResolveData.Type
        /// </summary>
        public static IDictionary<string, IFactory<ISurveyHttpCallbackHandleService>> SurveyHttpCallbackHandleServiceFactories { get; } = new Dictionary<string, IFactory<ISurveyHttpCallbackHandleService>>();


        public static ISurveyHttpCallbackResolveService GetSurveyHttpCallbackResolveService(string type)
        {
            if (!SurveyHttpCallbackResolveServiceFactories.TryGetValue(type, out IFactory<ISurveyHttpCallbackResolveService> surveyHttpCallbackResolveServiceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyHttpCallbackResolveServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Survey的Http回调解析服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type, "SurveyHttpCallbackExtensionCollection.SurveyHttpCallbackHandleServiceFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyHttpCallbackResolveServiceByType, fragment, 1, 0);
            }

            return surveyHttpCallbackResolveServiceFactory.Create();
        }

        public static ISurveyHttpCallbackHandleService GetSurveyHttpCallbackHandleService(string type)
        {
            if (!SurveyHttpCallbackHandleServiceFactories.TryGetValue(type, out IFactory<ISurveyHttpCallbackHandleService> surveyHttpCallbackHandleServiceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyHttpCallbackHandleServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Survey的Http回调处理服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { type, "SurveyHttpCallbackExtensionCollection.SurveyHttpCallbackHandleServiceFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyHttpCallbackHandleServiceByType, fragment, 1, 0);
            }

            return surveyHttpCallbackHandleServiceFactory.Create();
        }
    }


    /// <summary>
    /// Survey的Http回调解析服务
    /// 解析出回调类型以及回调参数
    /// </summary>
    public interface ISurveyHttpCallbackResolveService
    {
        Task<SurveyHttpCallbackResolveData> Resolve(HttpContext httpContext, CancellationToken cancellationToken = default);
    }

    public class SurveyHttpCallbackResolveData
    {
        public string Type { get; set; } = null!;
        public IDictionary<string, object> Parameters { get; } = null!;
    }

    /// <summary>
    /// Survey的Http回调处理服务
    /// </summary>
    public interface ISurveyHttpCallbackHandleService
    {
        Task Execute(SurveyHttpCallbackResolveData data, CancellationToken cancellationToken = default);
    }
}
