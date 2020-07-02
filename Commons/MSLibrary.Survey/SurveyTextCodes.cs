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
    }
}
