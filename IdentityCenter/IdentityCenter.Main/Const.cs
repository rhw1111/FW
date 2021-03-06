﻿using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityCenter.Main
{
    /// <summary>
    /// 用户账号声明类型集合
    /// </summary>
    public static class UserAccountClaimTypes
    {
        public const string Subject = "Subject";
        public const string Name = "Name";
    }

    /// <summary>
    /// 系统配置名称集合
    /// </summary>
    public static class SystemConfigurationNames
    {
        public const string LocalLoginSetting = "LocalLoginSetting";
        public const string ExternalIdentityCallbackUri = "ExternalIdentityCallbackUri";
        public const string ExternalIdentityBindPage = "ExternalIdentityBindPage";
    }

    /// <summary>
    /// 配置名称集
    /// </summary>
    public static class ConfigurationNames
    {
        /// <summary>
        /// 应用配置
        /// </summary>
        public const string Application = "Application";
        /// <summary>
        /// 主机配置
        /// </summary>
        public const string Host = "Host";
        /// <summary>
        /// 日志配置
        /// </summary>
        public const string Logger = "Logger";
    }


    /// <summary>
    /// 上下文扩展类型名称集
    /// </summary>
    public static class ContextExtensionTypes
    {
        /// <summary>
        /// DI容器上下文
        /// </summary>
        public const string DI = "DI";

    }

    /// <summary>
    /// 日志提供方处理名称集
    /// </summary>
    public static class LoggerProviderHandlerNames
    {
        /// <summary>
        /// ApplicationInsights
        /// </summary>
        public const string ApplicationInsights = "ApplicationInsights";
        /// <summary>
        /// 控制台
        /// </summary>
        public const string Console = "Console";
        /// <summary>
        /// ExceprionLess日志
        /// </summary>
        public const string ExceptionLess = "ExceptionLess";

    }


    /// <summary>
    /// 声明上下文生成名称集合
    /// </summary>
    public static class ClientClaimContextGeneratorNames
    {

        /// <summary>
        /// 默认
        /// </summary>
        public const string Default = "Default";
    }

    /// <summary>
    /// 声明上下文生成类型集合
    /// </summary>
    public static class ClientClaimContextGeneratorTypes
    {

        /// <summary>
        /// 默认
        /// </summary>
        public const string Default = "Default";
    }

    /// <summary>
    /// KV缓存访问服务类型集合
    /// </summary>
    public static class KVCacheVisitServiceTypes
    {
        /// <summary>
        /// 组合
        /// </summary>
        public const string Combination = "Combination";
        /// <summary>
        /// 基于本地超时
        /// </summary>
        public const string LocalTimeout = "LocalTimeout";
        /// <summary>
        /// 基于本地版本号
        /// </summary>
        public const string LocalVersion = "LocalVersion";
    }


    /// <summary>
    /// 国际化处理服务名称集合
    /// </summary>
    public static class InternationalizationHandleServiceNames
    {
        /// <summary>
        /// 默认
        /// </summary>
        public const string Default = "Default";
    }

    /// <summary>
    /// 声明上下文生成类型集合
    /// </summary>
    public static class ClaimContextGeneratorTypes
    {
        /// <summary>
        /// 默认
        /// </summary>
        public const string Default = "Default";
    }


    /// <summary>
    /// 环境声明生成器名称集合
    /// </summary>
    public static class EnvironmentClaimGeneratorNames
    {
        public const string Default = "Host";
    }

    /// <summary>
    /// 环境声明生成器类型集合
    /// </summary>
    public static class EnvironmentClaimGeneratorTypes
    {
        public const string Default = "Host";
    }


    /// <summary>
    /// 声明类型集合
    /// </summary>
    public static class ClaimsTypes
    {
        public const string User = "User";
        public const string Lcid = "Lcid";
        public const string TimezoneOffset = "TimezoneOffset";
    }

    /// <summary>
    /// OpenID协议使用到的Cookies名称集合
    /// </summary>
    public static class OpenIDCookiesNames
    {
        public const string State = "state_{0}";
        public const string LogoutState = "logoutstate_{0}";
        public const string RefreshToken = "refreshtoken_{0}";
    }

    /// <summary>
    ///  OpenID协议登出时使用到的参数名称集合
    /// </summary>
    public static class OpenIDLogoutParameterNames
    {
        public const string IDToken = "IDToken";
        public const string ReturnUrl = "Returnurl";
    }

    /// <summary>
    /// 认证客户端绑定的类型集合
    /// </summary>
    public static class IdentityClientBindingTypes
    {
        public const string None = "None";
        public const string OpenID = "OpenID";
    }
    

    /// <summary>
    /// 缓存键格式集合
    /// </summary>
    public static class CacheKeyFormats
    {
        public const string UserAccountID = "UserAccount_{0}";
        public const string UserAccountName = "UserAccountName_{0}";
        public const string UserAccountThirdParty= "UserAccountThirdParty_{0}_{1}";
        public const string UserPrivilege = "UserPrivilege_{0}";
        public const string UserRole = "UserRole_{0}";
    }

    /// <summary>
    /// 缓存键关系名称集合
    /// </summary>
    public static class CacheKeyRelationNames
    {
        public const string UserAccountIDName = "UserAccountIDName";
        public const string UserAccountIDThirdParty = "UserAccountIDThirdParty";
    }

    /// <summary>
    /// 缓存版本号名称集合
    /// </summary>
    public static class CacheVersionNames
    {
        public const string Role = "Role";
        public const string Privilege = "Privilege";
    }

    /// <summary>
    /// Grpc客户端拦截器描述名称集合
    /// </summary>
    public static class GrpcClinetInterceptorDescriptionNames
    {
        public const string ClinetExceptionWrapper = "ClinetExceptionWrapper";
        public const string ClientRequestTrace = "ClientRequestTrace";
    }

    /// <summary>
    /// Grpc服务端扩展上下文处理服务额外名称集合
    /// </summary>

    public static class GrpcServerExtensionContextHandleServiceExtraNames
    {

    }



}
