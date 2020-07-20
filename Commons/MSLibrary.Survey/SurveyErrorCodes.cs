using System;

namespace MSLibrary.Survey
{
    public enum SurveyErrorCodes
    {
        /// <summary>
        /// 找不到指定类型的SurveyMonkey的Http鉴权处理服务
        /// </summary>
        NotFoundSurveyMonkeyHttpAuthHandleServiceByType = 315000301,
        /// <summary>
        /// 找不到指定类型的SurveyMonkey的请求处理服务
        /// </summary>
        NotFoundSurveyMonkeyRequestHandleServiceByType = 315000302,
        /// <summary>
        /// SurveyMonkey鉴权错误
        /// </summary>
        SurveyMonkeyAuthError = 315000310,

        /// <summary>
        /// SurveyMonkey请求处理错误
        /// </summary>
        SurveyMonkeyRequestHandleError = 315000311,
        /// <summary>
        /// SurveyMonkey的Webhook回调验证失败
        /// </summary>
        SurveyMonkeyWebhookCallbackValidateError = 315000320,
        /// <summary>
        /// 找不到指定类型的Webhook回调验证服务
        /// </summary>
        NotFoundSurveyMonkeyWebhookCallbackValidationServiceByType = 315000321,
        /// <summary>
        /// 已经存在相同名称的Webhook注册
        /// </summary>
        ExistsSurveyMonkeyWebhookByName = 315000330,
        /// <summary>
        /// 找不到指定ID的Webhook注册
        /// </summary>
        NotFoundSurveyMonkeyWebhookByID = 315000331,
        /// <summary>
        /// 找不到指定名称的SurveyMonkey终结点
        /// </summary>
        NotFoundSurveyMonkeyEndpointByName = 315000332,
        /// <summary>
        /// 找不到指定ID的SurveyMonkey的Survey
        /// </summary>
        NotFoundSurveyMonkeySurveyByID = 315000333,
        /// <summary>
        /// 在SurveyMonkey的webhook回调中找不到指定的信息
        /// </summary>
        NotFoundInfoInSurveyMonkeyWebhookCallback = 315000334,

        /// <summary>
        /// 找不到指定类型的Survey收集器工厂
        /// </summary>
        NotFoundSurveyCollectorFactoryByType = 315000340,
        /// <summary>
        /// 找不到指定类型的Survey收集器绑定服务
        /// </summary>
        NotFoundSurveyCollectorBindServiceByType = 315000341,
        /// <summary>
        /// 找不到指定类型的Survey终结点终止服务
        /// </summary>
        NotFoundSurveyEndpointFinanlyServiceByType = 315000342,
        /// <summary>
        /// 找不到指定类型的Survey终结点初始化服务
        /// </summary>
        NotFoundSurveyEndpointInitServiceByType = 315000343,
        /// <summary>
        /// 找不到指定类型的Survey收集器数据查询服务
        /// </summary>
        NotFoundSurveyCollectorDataQueryServiceByType = 315000344,
        /// <summary>
        /// 找不到指定类型的Survey响应数据查询服务
        /// </summary>
        NotFoundSurveyResponseDataQueryServiceByType = 315000345,
        /// <summary>
        /// 找不到指定类型的Survey响应数据ID解析服务
        /// </summary>
        NotFoundSurveyResponseDataIDResolveServiceByType = 315000346,
        /// <summary>
        /// 找不到指定类型的Survey的Http回调解析服务
        /// </summary>
        NotFoundSurveyHttpCallbackResolveServiceByType= 315000350,
        /// <summary>
        /// 找不到指定类型的Survey的Http回调处理服务
        /// </summary>
        NotFoundSurveyHttpCallbackHandleServiceByType = 315000351,
        /// <summary>
        /// 找不到指定类型的Survey收集器可用性检查服务
        /// </summary>
        NotFoundSurveyCollectorEnableCheckServiceByType = 315000352,
        /// <summary>
        /// 找不到指定类型的Survey接收者生成服务
        /// </summary>
        NotFoundSurveyRecipientGenerateServiceByType = 315000353,
        /// <summary>
        /// 找不到指定类型的Survey响应转换服务
        /// </summary>
        NotFoundSurveyResponseConvertServiceByType = 315000354,



    }
}
