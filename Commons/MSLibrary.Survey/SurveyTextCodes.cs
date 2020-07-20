using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Survey
{
    public static class SurveyTextCodes
    {
        /// <summary>
        /// 找不到指定类型的SurveyMonkey的Http鉴权处理服务
        /// 格式为“找不到类型为{0}的SurveyMonkey的Http鉴权处理服务，发生位置为{1}”
        /// {0}：类型
        /// {1}：发生的位置
        /// </summary>
        public static string NotFoundSurveyMonkeyHttpAuthHandleServiceByType = "NotFoundSurveyMonkeyHttpAuthHandleServiceByType";
        /// <summary>
        /// 找不到指定类型的SurveyMonkey的请求处理服务
        /// 格式为“找不到类型为{0}的SurveyMonkey的请求处理服务，发生位置为{1}”
        /// {0}：类型
        /// {1}：发生的位置
        /// </summary>
        public static string NotFoundSurveyMonkeyRequestHandleServiceByType = "NotFoundSurveyMonkeyRequestHandleServiceByType";
        /// <summary>
        /// SurveyMonkey鉴权错误
        /// 格式为“SurveyMonkey终结点{0}鉴权错误，错误信息为{1}”
        /// {0}：终结点名称
        /// {1}：错误信息
        /// </summary>
        public static string SurveyMonkeyAuthError = "SurveyMonkeyAuthError";
        /// <summary>
        /// SurveyMonkey请求处理错误
        /// 格式为“SurveyMonkey终结点{0}请求处理错误，错误信息为{1}”
        /// {0}：终结点名称
        /// {1}：错误信息
        /// </summary>
        public static string SurveyMonkeyRequestHandleError = "SurveyMonkeyRequestHandleError";
        /// <summary>
        /// SurveyMonkey的Webhook回调验证失败
        /// 格式为“SurveyMonkey的Webhook回调验证失败，错误信息为{0}”
        /// {0}：错误信息
        /// </summary>
        public static string SurveyMonkeyWebhookCallbackValidateError = "SurveyMonkeyWebhookCallbackValidateError";
        /// <summary>
        /// 找不到指定类型的SurveyMonkey的Webhook回调验证服务
        /// 格式为“找不到类型为{0}的Webhook回调验证服务，发生位置为{1}”
        /// {0}：类型
        /// {1}：发生位置
        /// </summary>
        public static string NotFoundSurveyMonkeyWebhookCallbackValidationServiceByType = "NotFoundSurveyMonkeyWebhookCallbackValidationServiceByType";
        /// <summary>
        /// 已经存在相同名称的SurveyMonkey的Webhook注册
        /// 格式为“已经存在名称为{0}的Webhook注册”
        /// {0}：名称
        /// </summary>
        public static string ExistsSurveyMonkeyWebhookByName = "ExistsSurveyMonkeyWebhookByName";
        /// <summary>
        /// 找不到指定ID的Webhook注册
        /// 格式为“找不到指定ID为{0}的Webhook注册”
        /// {0}：ID
        /// </summary>
        public static string NotFoundSurveyMonkeyWebhookByID = "NotFoundSurveyMonkeyWebhookByID";


        /// <summary>
        /// 找不到指定类型的Survey收集器工厂
        /// 格式为“找不到类型为{0}的Survey收集器工厂，发生位置为{1}”
        /// {0}：类型
        /// {1}：发生的位置
        /// </summary>
        public static string NotFoundSurveyCollectorFactoryByType = "NotFoundSurveyCollectorFactoryByType";
        /// <summary>
        /// 找不到指定类型的Survey收集器绑定服务
        /// 格式为“找不到类型为{0}的Survey收集器绑定服务，发生位置为{1}”
        /// {0}：类型
        /// {1}：发生的位置
        /// </summary>
        public static string NotFoundSurveyCollectorBindServiceByType = "NotFoundSurveyCollectorBindServiceByType";
        /// <summary>
        /// 找不到指定类型的Survey终结点终止服务
        /// 格式为“找不到类型为{0}的Survey终结点终止服务，发生位置为{1}”
        /// {0}：类型
        /// {1}：发生的位置
        /// </summary>
        public static string NotFoundSurveyEndpointFinanlyServiceByType = "NotFoundSurveyEndpointFinanlyServiceByType";
        /// <summary>
        /// 找不到指定类型的Survey终结点初始化服务
        /// 格式为“找不到类型为{0}的Survey终结点初始化服务，发生位置为{1}”
        /// {0}：类型
        /// {1}：发生的位置
        /// </summary>
        public static string NotFoundSurveyEndpointInitServiceByType = "NotFoundSurveyEndpointInitServiceByType";
        /// <summary>
        /// 找不到指定类型的Survey收集器数据查询服务
        /// 格式为“找不到类型为{0}的Survey收集器数据查询服务，发生位置为{1}”
        /// {0}：类型
        /// {1}：发生的位置
        /// </summary>
        public static string NotFoundSurveyCollectorDataQueryServiceByType = "NotFoundSurveyCollectorDataQueryServiceByType";
        /// <summary>
        /// 找不到指定类型的Survey响应数据查询服务
        /// 格式为“找不到类型为{0}的Survey响应数据查询服务，发生位置为{1}”
        /// {0}：类型
        /// {1}：发生的位置 
        /// </summary>
        public static string NotFoundSurveyResponseDataQueryServiceByType = "NotFoundSurveyResponseDataQueryServiceByType";
        /// <summary>
        /// 找不到指定类型的Survey响应数据ID解析服务
        /// 格式为“找不到类型为{0}的Survey响应数据ID解析服务，发生位置为{1}”
        /// {0}：类型
        /// {1}：发生的位置
        /// </summary>
        public static string NotFoundSurveyResponseDataIDResolveServiceByType = "NotFoundSurveyResponseDataIDResolveServiceByType";
        /// <summary>
        /// 找不到指定类型的Survey的Http回调解析服务
        /// 格式为“找不到类型为{0}的Survey的Http回调解析服务，发生位置为{1}”
        /// {0}：类型
        /// {1}：发生的位置
        /// </summary>
        public static string NotFoundSurveyHttpCallbackResolveServiceByType = "NotFoundSurveyHttpCallbackResolveServiceByType";
        /// <summary>
        /// 找不到指定类型的Survey的Http回调处理服务
        /// 格式为“找不到类型为{0}的Survey的Http回调处理服务，发生位置为{1}”
        /// {0}：类型
        /// {1}：发生的位置
        /// </summary>
        public static string NotFoundSurveyHttpCallbackHandleServiceByType = "NotFoundSurveyHttpCallbackHandleServiceByType";
        /// <summary>
        /// 找不到指定名称的SurveyMonkey终结点
        /// 格式为“找不到名称为{0}的SurveyMonkey终结点”
        /// {0}：终结点名称
        /// </summary>
        public static string NotFoundSurveyMonkeyEndpointByName = "NotFoundSurveyMonkeyEndpointByName";
        /// <summary>
        /// 找不到指定ID的SurveyMonkey的Survey
        /// 格式为“找不到ID为{0}的SurveyMonkey的Survey”
        /// {0}：SurveyID
        /// </summary>
        public static string NotFoundSurveyMonkeySurveyByID = "NotFoundSurveyMonkeySurveyByID";
        /// <summary>
        /// 找不到指定类型的Survey收集器可用性检查服务
        /// 格式为“找不到类型为{0}的Survey收集器可用性检查服务，发生位置为{1}”
        /// {0}：类型
        /// {1}：发生的位置
        /// </summary>
        public static string NotFoundSurveyCollectorEnableCheckServiceByType = "NotFoundSurveyCollectorEnableCheckServiceByType";
        /// <summary>
        /// 找不到指定类型的Survey接收者生成服务
        /// 格式为“找不到类型为{0}的Survey接收者生成服务，发生位置为{1}”
        /// {0}：类型
        /// {1}：发生的位置
        /// </summary>
        public static string NotFoundSurveyRecipientGenerateServiceByType = "NotFoundSurveyRecipientGenerateServiceByType";
        /// <summary>
        /// 找不到指定类型的Survey响应转换服务
        /// 格式为“找不到类型为{0}的Survey响应转换服务，发生位置为{1}”
        /// {0}：类型
        /// {1}：发生的位置
        /// </summary>
        public static string NotFoundSurveyResponseConvertServiceByType = "NotFoundSurveyResponseConvertServiceByType";
        /// <summary>
        /// 在SurveyMonkey的webhook回调中找不到指定的信息
        /// 格式为“在SurveyMonkey的webhook回调中找不到名称为{0}的信息，回调数据为{1}”
        /// {0}：名称
        /// {1}：回调数据
        /// </summary>
        public static string NotFoundInfoInSurveyMonkeyWebhookCallback = "NotFoundInfoInSurveyMonkeyWebhookCallback";
    }
}
