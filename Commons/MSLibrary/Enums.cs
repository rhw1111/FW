using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary
{
    /// <summary>
    /// 错误码
    /// </summary>
    public enum Errors
    {
        /// <summary>
        /// 找不到指定消息名称的Crm消息处理器
        /// </summary>
        NotFoundCrmMessageExecuteHandleByName = 314710000,
        /// <summary>
        /// 消息请求类型不匹配
        /// </summary>
        CrmRequestMessageTypeNotMatch = 314710001,
        /// <summary>
        /// Crm执行实体属性值转换处理类型不匹配
        /// </summary>
        CrmExecuteEntityTypeHandleTypeNotMatch = 314710002,
        /// <summary>
        /// 找不到指定类型名称的Crm执行实体属性值转换处理
        /// </summary>
        NotFoundCrmExecuteEntityTypeHandleByTypeName = 314710003,
        /// <summary>
        /// 找不到指定请求类型名称的Crm消息处理
        /// </summary>
        NotFoundCrmMessageHandleByRequestTypeFullName = 314710004,
        /// <summary>
        /// Crm消息处理不支持指定的HttpMethod
        /// </summary>
        CrmMessageExecuteNotSupportMethod = 314710005,
        /// <summary>
        /// 找不到指定类型名称的Crm唯一键属性值转换处理
        /// </summary>
        NotFoundCrmAlternateKeyTypeHandleByTypeName = 314710006,
        /// <summary>
        /// Crm唯一键属性值转换处理类型不匹配
        /// </summary>
        CrmAlternateKeyTypeHandleTypeNotMatch = 314710007,
        /// <summary>
        /// 指定类型没有实现指定接口
        /// </summary>
        TypeNotImplimentInterface= 314710010,
        /// <summary>
        /// 指定类型不是所需要的类型
        /// </summary>
        TypeNotRequire = 314710011,
        /// <summary>
        /// 值不允许为null
        /// </summary>
        ValueNotAllowNull= 314710012,

        /// <summary>
        /// 在Crm服务令牌生成服务中找不到指定名称的参数
        /// </summary>
        NotFoundParameterInCrmServiceTokenGenerateService = 314710030,
        /// <summary>
        /// 在Crm服务令牌生成服务中指定参数的类型不匹配
        /// </summary>
        ParameterTypeNotMatchInCrmServiceTokenGenerateService = 314710031,
        /// <summary>
        /// 找不到指定名称的Crm服务令牌生成服务
        /// </summary>
        NotFoundCrmServiceTokenGenerateServiceByName = 314710032,
        /// <summary>
        /// 调用Crm的webapi出错
        /// </summary>
        CrmWebApiCommonError = 314710033,
        /// <summary>
        /// 调用Crm的webapi出现并发性错误
        /// </summary>
        CrmWebApiConcurrencyError = 314710034,
        /// <summary>
        /// 调用Crm的webapi出现限制性错误
        /// </summary>
        CrmWebApiLimitError = 314710035,
        /// <summary>
        /// 在Crm的Webapi响应中，找不到指定名称的头
        /// </summary>
        CrmWebApiHttpResponseNotFoundHeaderByName = 314710100,
        /// <summary>
        /// 在Crm的Webapi响应中,正则匹配失败
        /// </summary>
        CrmWebApiHttpResponseRegexMatchFail = 314710101,

        /// <summary>
        /// 指定的Crm的JToken转换服务中，传入的JToken类型不匹配
        /// </summary>
        CrmJTokenConvertNotMatch = 314710201,
        /// <summary>
        ///  指定的Crm的JToken转换服务中,传入的JToken中找不到指定属性
        /// </summary>
        CrmJTokenConvertEntityNotFoundAttribute = 314710202,
        /// <summary>
        /// 找不到指定类型的Crm查询结果JToken处理
        /// </summary>
        NotFoundCrmRetrieveJTokenHandleByType = 314710203,
        /// <summary>
        /// 指定的Crm查询结果JToken处理返回的结果的类型不匹配
        /// </summary>
        CrmRetrieveJTokenHandleResultTypeNotMatch = 314710204,
        /// <summary>
        /// 指定的Crm查询结果JToken处理缺少参数
        /// </summary>
        CrmRetrieveJTokenHandleMissParameter = 314710205,
        /// <summary>
        /// 指定的Crm查询结果JToken处理的指定参数的类型不匹配
        /// </summary>
        CrmRetrieveJTokenHandleParameterTypeNotMatch = 314710206,
        /// <summary>
        ///  指定的Crm的JToken转换成EntityReference服务中,传入的JToken中找不到指定属性
        /// </summary>
        CrmJTokenConvertEntityReferenceNotFoundAttribute = 314710207,
        /// <summary>
        /// 找不到指定类型的crm函数参数处理
        /// </summary>
        NotFoundCrmFunctionParameterHandleByType = 314710208,
        /// <summary>
        /// 找不到指定类型的crm动作参数处理
        /// </summary>
        NotFoundCrmActionParameterHandleByType = 314710209,
        /// <summary>
        /// Crm的Batch操作响应为空
        /// </summary>
        CrmBatchResponseIsEmpty = 314710230,
        /// <summary>
        /// 在Crm的Batch操作响应中找不到BatchCode
        /// </summary>
        NotFoundBatchCodeInCrmBatchResponse = 314710231,
        /// <summary>
        /// Crm的Batch操作响应项格式错误
        /// </summary>
        CrmBatchResponseItemFormatError = 314710232,
        /// <summary>
        /// 找不到指定类型的Crm属性元数据处理
        /// </summary>
        NotFoundCrmAttributeMetadataHandleByType= 314710233,

        /// <summary>
        /// 找不到指定类型的Crm服务工厂服务
        /// </summary>
        NotFoundCrmServiceFactoryServiceByType = 314710234,
        /// <summary>
        /// 找不到指定名称的Crm服务工厂
        /// </summary>
        NotFountCrmServiceFactorybyName= 314710235,
        /// <summary>
        /// 在指定消息的响应中，找不到指定名称的header
        /// </summary>
        NotFoundHeaderFromResponse= 314710236,
        /// <summary>
        /// 在查询获取的CrmEntity中的属性中，找不到指定名称的属性
        /// </summary>
        NotFoundAttributeNameInRetrieveCrmEntity= 314710237,

        /// <summary>
        /// 在Adfs的Oauth认证的响应中，找不到指定名称的参数
        /// </summary>
        AdfsOauthResponseNotFoundParameterByName = 314711000,
        /// <summary>
        /// Adfs的Oauth认证的响应出错
        /// </summary>
        AdfsOauthResponseError = 314711001,
        /// <summary>
        /// AAD的Oauth认证的响应出错
        /// </summary>
        AADOauthResponseError = 314711002,
        /// <summary>
        /// 找不到指定类型的身份信息Http头生成服务
        /// </summary>
        NotFoundAuthInfoHttpHeaderGeneratorServiceByType= 314711100,
        /// <summary>
        /// 找不到指定类型的声明上下文生成服务
        /// </summary>
        NotFoundClaimContextGeneratorServiceByType = 314711101,
        /// <summary>
        /// 找不到指定类型的Http声明生成服务
        /// </summary>
        NotFoundHttpClaimGeneratorServiceByType= 314711102,
        /// <summary>
        /// 找不到指定类型的环境声明生成服务
        /// </summary>
        NotFoundEnvironmentClaimGeneratorServiceByType= 314711103,
        /// <summary>
        /// 在声明集中找不到指定类型的声明
        /// </summary>
        NotFoundTypeInClaims = 314711200,

        /// <summary>
        /// 找不到指定名称的系统配置
        /// </summary>
        NotFoundSystemConfigurationByName= 314711250,
        /// <summary>
        /// 系统配置转换成指定类型失败
        /// </summary>
        SystemConfigurationConvertTypeFail= 314711251,


        /// <summary>
        /// 不是AccessToken
        /// </summary>
        NotAccessToken = 314720000,
        /// <summary>
        /// 不是RefreashAccessToken
        /// </summary>
        NotRefreashAccessToken = 314720001,
        /// <summary>
        /// 在httpheader中找不到Authorization
        /// </summary>
        NotFoundAuthorizationInHttpHeader = 314720002,
        /// <summary>
        /// 在httpheader中找不到SystemName
        /// </summary>
        NotFoundSystemNameInHttpHeader = 314720003,
        /// <summary>
        /// 白名单验证未通过
        /// </summary>
        WhitelistValidateFail = 314720004,
        /// <summary>
        /// 找不到指定名称的Http声明生成器
        /// </summary>
        NotFoundHttpClaimGeneratorByName = 314720005,
        /// <summary>
        /// 没有找到指定名称的环境声明生成器
        /// </summary>
        NotFoundEnvironmentClaimGeneratorByName = 314720006,
        /// <summary>
        /// 没有找到指定名称的上下文生成器
        /// </summary>
        NotFoundClaimContextGeneratorByName = 314720007,
        /// <summary>
        /// 用户授权失败
        /// </summary>
        AuthorizeFail = 314720008,
        /// <summary>
        /// 请求超出阈值
        /// </summary>
        RequestOverflow = 314720010,
        /// <summary>
        /// 消息执行出错
        /// </summary>
        SMessageExecuteError = 314720019,
        /// <summary>
        /// 消息类型监听者的类型不正确
        /// </summary>
        SMessageTypeListenerTypeError = 314720020,
        /// <summary>
        /// 按指定消息类型没有找到消息队列
        /// </summary>
        NotFoundSQueueByMessageType = 314720021,
        /// <summary>
        /// 没有找到指定名称的队列执行组
        /// </summary>
        NotFoundSQueueProcessGroupByName = 314720022,
        /// <summary>
        /// 客户端消息类型监听终结点签名错误
        /// </summary>
        ClientSMessageTypeListenerEndpointSignatureError = 314720023,
        /// <summary>
        /// 消息类型监听器与消息类型不匹配
        /// </summary>
        SMessageTypeListenerNotMatchSMessageType = 314720024,
        /// <summary>
        /// 消息类型监听器与消息体不匹配
        /// </summary>
        SMessageTypeListenerNotMatchSMessageBody = 314720025,
        /// <summary>
        /// 在指定消息类型下找不到指定监听ID的监听
        /// </summary>
        NotFoundSMessageTypeListenerFromTypeByID= 314720026,
        /// <summary>
        /// 找不到指定名称的消息类型
        /// </summary>
        NotFoundSMessageTypeByName = 314720027,

        /// <summary>
        /// 池长度超出阈值
        /// </summary>
        PoolLengthOverflow = 314720043,
        /// <summary>
        /// Tcp数据处理的类型不正确
        /// </summary>
        TcpDataExecuteTypeError = 314720050,
        /// <summary>
        /// Tcp客户端数据处理的类型不正确
        /// </summary>
        TcpClientDataExecuteTypeError = 314720051,
        /// <summary>
        /// 没有找到指定名称的Tcp客户端终结点
        /// </summary>
        NotFoundTcpClientEndpointByName = 314720052,
        /// <summary>
        /// 已经存在相同名称的Tcp监听器
        /// </summary>
        ExistSameNameTcpListener = 314720053,
        /// <summary>
        /// 没有找到指定名称的Tcp客户端终结点
        /// </summary>
        NotFoundTcpListenerByName = 314720054,

        /// <summary>
        /// Tcp双工数据处理的类型不正确
        /// </summary>
        TcpDuplexDataExecuteTypeError = 314720060,

        /// <summary>
        /// 没有找到指定哈希组的哈希节点
        /// </summary>
        NotFoundHashNodeByGroup = 314720071,
        /// <summary>
        /// 一致性哈希策略类型不正确
        /// </summary>
        HashGroupStrategyServiceFactoryTypeError = 314720072,
        /// <summary>
        /// 没有找到指定名称的一致性哈希组
        /// </summary>
        NotFoundHashGroupByName = 314720073,
        /// <summary>
        /// 在指定的哈希节点的关键信息中，找不到指定的键值
        /// </summary>
        NotFoundKeyInHashNodeKeyInfo = 314720074,
        /// <summary>
        /// 指定哈希组执行数据迁移时发生错误
        /// </summary>
        HashDataMigrateErrorByGroup = 314720075,
        /// <summary>
        /// 指定哈希组执行整体数据迁移时发生错误
        /// </summary>
        HashDataTotalMigrateErrorByGroup = 314720076,
        /// <summary>
        /// 将要转移的数据增加到目标真实节点时发生错误
        /// </summary>
        NotFoundHashDataMigrateTargetDB = 314720077,
        /// <summary>
        /// 删除目标节点数据时发生错误
        /// </summary>
        NotFoundDeleteTargetDB = 314720078,
        /// <summary>
        /// JWT格式错误
        /// </summary>
        JWTFormatError = 314720081,
        /// <summary>
        /// JWT签名错误
        /// </summary>
        JWTSignError = 314720082,
        /// <summary>
        /// JWT过期
        /// </summary>
        JWTExpire = 314720083,
        /// <summary>
        /// 没有找到指定类型的FillEntityService
        /// </summary>
        NotFoundFillEntityService = 314720100,
        /// <summary>
        /// 找不到指定名称的标签参数处理器
        /// </summary>
        NotFoundLabelParameterHandlerByName = 314720150,
        /// <summary>
        /// 指定标签参数的个数不正确
        /// </summary>
        LabelParameterCountError = 314720151,
        /// <summary>
        /// 指定标签参数的个数要求最小数量
        /// </summary>
        LabelParameterCountRequireMin = 314720152,
        /// <summary>
        /// 指定系统模板参数名称标签标签参数类型错误
        /// </summary>
        LabelParameterTypeError = 314720153,
        /// <summary>
        /// 指定系统模板参数名称标签标签最小值和最大值错误
        /// </summary>
        LabelParameterMinMaxError = 314720154,
        /// <summary>
        /// 指定系统模板参数名称标签标签加密算法Key错误
        /// </summary>
        LabelParameterDesSecurityKeyError = 314720155,
        /// <summary>
        /// 在模板上下文中找不到指定名称的参数
        /// </summary>
        NotFoundParameterInTemplateContextByName = 314720169,
        /// <summary>
        /// 找不到指定名称的系统模板
        /// </summary>
        NotFoundSystemTemplateParamaterNamesByName = 314720170,
        /// <summary>
        /// 指定系统模板参数名称标签转换类型出错
        /// </summary>
        LabelParameterSystemTemplateParamaterNamesError = 314720171,

        /// <summary>
        /// 找不到指定名称的系统模板记录名称
        /// </summary>
        NotFoundEntityAttributeByEntityName = 314720173,

        /// <summary>
        /// 与服务通信时出错
        /// </summary>
        CommunicationServiceError = 314720200,

        /// <summary>
        /// 找不到指定名称的协程容器
        /// </summary>
        NotFoundCoroutineContainerByName = 314720250,
        /// <summary>
        /// 协程本地数据尚未初始化
        /// </summary>
        CoroutineLocalNotInit = 314720251,
        /// <summary>
        /// 找不到指定名称的协程本地数据
        /// </summary>
        NotFoundCoroutineLocalByName = 314720252,
        /// <summary>
        /// 当前协程本地数据尚未指定
        /// </summary>
        NotFoundCoroutineLocalByCurrent = 314720253,

        /// <summary>
        /// 在条件元素的参数列表中，找不到指定名称的参数
        /// </summary>
        NotFoundParameterFromConditionParametersByName = 314720300,
        /// <summary>
        /// 在条件元素的参数列表中，指定名称的参数类型不匹配
        /// </summary>
        ParameterFromConditionParametersTypeNotMatchByName = 314720301,
        /// <summary>
        /// 在条件元素的参数列表中，使用指定名称的属性值做为指定名称的参数的数组索引，索引越界
        /// </summary>
        ParameterFromConditionParametersIndexOut = 314720302,
        /// <summary>
        /// 在条件元素中找不到指定属性名称的属性
        /// </summary>
        NotFoundAttributeInConditionElement = 314720303,
        /// <summary>
        /// 指定的条件元素执行发生错误
        /// </summary>
        ConditionElementError = 314720304,

        /// <summary>
        /// 找不到指定前缀的序列号记录
        /// </summary>
        NotFoundSerialNumberRecordByPrefix = 314720330,
        /// <summary>
        /// 指定前缀的序列号记录已经存在
        /// </summary>
        ExistFoundSerialNumberRecordByPrefix = 314720331,
        /// <summary>
        /// 找不到指定名称的序列号生成配置
        /// </summary>
        NotFoundSerialNumberGeneratorConfigurationByName = 314720332,
        /// <summary>
        /// 在指定的类型中找不到指定名称的属性
        /// </summary>
        NotFoundPropertyInTypeByName = 314720381,
        /// <summary>
        /// 找不到指定名称的选项集元数据
        /// </summary>
        NotFoundOptionSetValueMetadataByName = 314720410,
        /// <summary>
        /// 在指定名称的选项集元数据中找不到指定值的选项集项
        /// </summary>
        NotFoundOptionSetValueItemInMetadataByValue = 314720411,
        /// <summary>
        /// 指定名称的选项集元数据中存在相同值的选项
        /// </summary>
        OptionSetValueItemIsExistInMetadata = 314720412,
        /// <summary>
        /// 找不到指定名称的调度动作
        /// </summary>
        NotFoundScheduleActionByName = 314720421,
        /// <summary>
        /// 找不到指定名称的调度动作组
        /// </summary>
        NotFoundScheduleActionGroupByName = 314720422,
        /// <summary>
        /// 调度动作服务类型错误
        /// </summary>
        ScheduleActionServiceTypeError = 314720423,
        /// <summary>
        /// 找不到指定类型的调度动作初始化服务
        /// </summary>
        NotFoundScheduleActionInitServiceByType= 314720424,
        /// <summary>
        /// 找不到指定名称的调度主机配置
        /// </summary>
        NotFoundScheduleHostConfigurationByName = 314720430,
        /// <summary>
        /// 找不到指定类型的Factory<IValidateUserKeyService>
        /// </summary>
        NotFoundWorkflowValidateUserKeyServiceFactoryByType = 314720480,
        /// <summary>
        /// 找不到指定key的工作流资源
        /// </summary>
        NotFoundWorkflowResourceByKey = 314720481,
        /// <summary>
        /// 指定key的工作流资源未完成
        /// </summary>
        ExistWorkflowResourceStepByActionName = 314720482,
        /// <summary>
        /// 找不到工作流中需要审核的步骤;查询条件为资源类型、资源关键字、资源Id、动作名称、资源状态
        /// </summary>
        NotFoundWorkflowStepForAudit = 314720483,
        /// <summary>
        ///  找不到指定类型的Factory<IGetUserInfoFromWorkflowStepService>
        /// </summary>
        NotFoundWorkflowGetUserInfoServiceFactoryByType = 314720484,
        /// <summary>
        /// 找不到指定名称的系统登录终结点
        /// </summary>
        NotFoundSystemLoginEndpointByName = 314720500,
        /// <summary>
        /// 在指定名称的系统登录终结点中，找不到指定名称的关联认证终结点
        /// </summary>
        NotFoundAuthorizationEndpointInSystemLoginEndpointByName = 314720501,
        /// <summary>
        /// 在指定名称的系统登录终结点中，找不到可以处理从第三方认证系统回调请求的关联认证终结点
        /// </summary>
        NotFoundAuthorizationEndpointInSystemLoginEndpointCanExecuteCallback = 314720502,
        /// <summary>
        /// 在指定名称的系统登录终结点的第三方认证系统回调请求处理中，回调请求的Url中不包含returnurl参数
        /// </summary>
        NotFoundReturnUrlQuerystringInAuthRedirectUrl = 314720503,
        /// <summary>
        /// 在指定名称的系统登录终结点的第三方认证系统回调请求处理中，回调请求的Url中不包含authname参数
        /// </summary>
        NotFoundAuthNameQuerystringInAuthRedirectUrl = 314720504,
        /// <summary>
        /// 在第三方认证系统回调请求中，回调请求的Url中不包含sysname参数
        /// </summary>
        NotFoundSysNameQuerystringInCallbackRequest = 314720505,
        /// <summary>
        /// 指定名称的系统登录终结点验证令牌字符串失败
        /// </summary>
        SystemLoginEndpointTokenValidateError = 314720506,
        /// <summary>
        /// 指定名称的系统登录终结点的令牌中，找不到指定名称的信息
        /// </summary>
        NotFoundInfoInSystemLoginEndpointTokenByName = 314720507,
        /// <summary>
        /// 在通用令牌的JWT字符串中，找不到指定名称的键
        /// </summary>
        NotFoundKeyInCommonTokenJWT = 314720508,
        /// <summary>
        /// 通用令牌登出错误
        /// </summary>
        CommonTokenLogoutError = 314720509,
        /// <summary>
        /// 执行指定名称的应用时，JWT错误
        /// </summary>
        ExecuteAppJWTError = 314720510,

        /// <summary>
        /// 在指定的系统登录终结点中客户端重定向地址非法
        /// </summary>
        SystemLoginEndpointClientRedirectUrl = 314720511,

        /// <summary>
        /// 在第三方系统服务的Http请求中找不到指定名称的参数
        /// </summary>
        NotFoundParameterFromHttpRequestInThirdPartySystemService= 314720512,

        /// <summary>
        /// 在第三方系统服务令牌验证错误
        /// </summary>
        ThirdPartySystemServiceTokenValidateError = 314720513,
        /// <summary>
        /// 第三方系统服务令牌中缺少指定键值
        /// </summary>
        ThirdPartySystemServiceTokenNotContainKey = 314720514,
        /// <summary>
        /// 第三方系统服务不支持操作
        /// </summary>
        ThirdPartySystemServiceNotSupportOperate = 314720515,
        /// <summary>
        /// 在第三方系统后续操作服务中找不到需要的属性
        /// </summary>
        NotFoundNeedAttributeInThirdPartySystemPostExecuteService= 314720516,
        /// <summary>
        /// 在第三方系统后续操作服务中找不到Crm用户信息
        /// </summary>
        NotFoundCrmUserInfoInThirdPartySystemPostExecuteService = 314720517,

        /// <summary>
        /// 在键值对配置信息中，找不到指定键值的数据
        /// </summary>
        NotFoundKeyFromKVConfiguration = 314720600,
        /// <summary>
        /// 系统登录终结点中已存在相同的名称数据
        /// </summary>
        ExistSystemLoginEndpointByName = 314720630,
        /// <summary>
        /// 找不到指定类型的第三方系统服务
        /// </summary>
        NotFoundThirdPartySystemServiceByType= 314720631,
        /// <summary>
        /// 找不到指定类型的第三方系统后续处理服务
        /// </summary>
        NotFoundThirdPartySystemPostExecuteServiceByType = 314720632,

        /// <summary>
        /// 在第三方令牌及后续处理中获取的键值对中，找不到指定键的值
        /// </summary>
        NotFoundUserInfoKeyInThirdPartyTokenAttributes= 314720633,
        /// <summary>
        /// 存在相同的第三方系统令牌记录
        /// </summary>
        ExistSameThirdPartySystemTokenRecord= 314720634,
        /// <summary>
        /// 第三方系统服务HttpPost请求发生错误
        /// </summary>
        ThirdPartySystemServiceHttpPostError= 314720635,
        /// <summary>
        /// 找不到指定类型的令牌控制器服务
        /// </summary>
        NotFoundTokenControllerServiceByType= 314720640,
        /// <summary>
        /// 找不到指定名称的令牌控制器
        /// </summary>
        NotFoundTokenControllerByName= 314720641,

        /// <summary>
        /// 工作流用户动作中已存在相同的用户关键字数据
        /// </summary>
        ExistWorkflowStepUserActionByUserKey = 314720660,
        /// <summary>
        /// 调度动作中存在相同的名称数据
        /// </summary>
        ExistScheduleActionByName = 314720690,
        /// <summary>
        /// 短信发送终结点中存在相同的名称数据
        /// </summary>
        ExistSMSSendEndpointByName = 314796582,
        /// <summary>
        /// 白名单中存在相同的系统名称数据
        /// </summary>
        ExistWhitelistBySystemName = 314720720,
        /// <summary>
        /// 客户端白名单中存在相同的系统名称数据
        /// </summary>
        ExistClientWhitelistBySystemName = 314720750,
        /// <summary>
        /// 系统操作和白名单关联关系中存在相同的系统操作ID和白名单ID数据
        /// </summary>
        ExistSystemOperationWhitelistRelationByID = 314720780,
        /// <summary>
        ///  系统操作中存在相同的名称数据
        /// </summary>
        ExistSystemOperationByName = 314720800,
        /// <summary>
        ///  客户端消息类型监听终结点中存在相同的名称数据
        /// </summary>
        ExistClientSMessageTypeListenerEndpointByName = 314720830,
        /// <summary>
        /// 验证终结点数据中存在相同的名称数据
        /// </summary>
        ExistAuthorizationEndpointByName = 314720666,
        /// <summary>
        ///  客户端系统登陆终结点中存在相同的名称数据
        /// </summary>
        ExistClientSystemLoginEndpointByName = 314720860,
        /// <summary>
        /// 工作流资源已存在相同关键字(type+key)数据
        /// </summary>
        ExistWorkflowResourceKey = 314720890,
        /// <summary>
        /// 工作流步骤已存在相同关键字(resourceid+actionname+status+usertype+userkey)数据
        /// </summary>
        ExistWorkflowStepKey = 314720920,
        /// <summary>
        /// 调度作业组已存在相同名称数据
        /// </summary>
        ExistScheduleActionGroupName = 314720950,
        /// <summary>
        /// 短信模板已存在相同名称数据
        /// </summary>
        ExistSMSTemplateStoreByName = 314721020,
        /// <summary>
        /// Json反序列失败
        /// </summary>
        JsonDeserializeError = 314721070,
        /// <summary>
        /// ServiceFabric客户端终结点仓储已存在相同名称数据
        /// </summary>
        ExistServiceFabricClientEndpointStoreByName = 314721100,
        /// <summary>
        /// 在哈希数据迁移服务工厂键值对组中找不到指定存储类型的键
        /// </summary>
        NotFoundHashDataMigrateServiceFactoryDictionaryByStoreType = 314721601,
        /// <summary>
        /// 在哈希数据迁移服务工厂键值对中找不到指定策略名称的键
        /// </summary>
        NotFoundHashDataMigrateServiceFactoryByStrategyName = 314721602,
        /// <summary>
        /// 消息历史监听明细中存在相同的名称数据
        /// </summary>
        ExistSMessageHistoryDetailByName = 314721450,
        /// <summary>
        /// 当前请求已被锁定
        /// </summary>
        ExistLicationLock = 314721470,
        /// <summary>
        /// 找不到指定实体类型的EntityRepositoryService
        /// </summary>
        NotFoundEntityRepositoryServiceByEntityType = 314721600,
        /// <summary>
        /// 找不到指定实体类型和实体关键字的实体记录
        /// </summary>
        NotFoundEntityByEntityTypeAndKey = 314721601,
        /// <summary>
        /// 实体属性值无法到达，因为中间出现null值
        /// </summary>
        EntityAttributeCanNotArrive = 314721621,
        /// <summary>
        /// 实体属性链中的值必须基于ModelBase
        /// </summary>
        EntityAttributeNotModelBase = 314721622,
        /// <summary>
        /// 找不到指定操作符的EntityAttributeValueValidateService
        /// </summary>
        NotFoundEntityAttributeValueValidateServiceByOperator = 314721623,
        /// <summary>
        /// 实体属性值的类型与指定的类型不匹配
        /// </summary>
        EntityAttributeValueTypeNotMatch = 314721624,
        /// <summary>
        /// 要用来和实体属性的值相比较的值的类型不匹配
        /// </summary>
        EntityAttributeCheckValueTypeNotMatch = 314721625,
        /// <summary>
        /// 指定实体属性的值类型未定义
        /// </summary>
        NotFoundEntityAttributeValueType = 314721626,
        /// <summary>
        /// 指定实体属性的值类型与操作不匹配
        /// </summary>
        EntityAttributeValueTypeAndOeratorNotMatch = 314721627,
        /// <summary>
        /// 找不到指定指定备用关键字名称的备用关键字
        /// </summary>
        NotFoundEntityInfoAlternateKeyByName = 314721650,
        /// <summary>
        /// 在指定背用关键字下面找不到关联关系
        /// </summary>
        NotFoundEntityInfoAlternateKeyRelation = 314721651,
        /// <summary>
        /// 找不到指定属性类型的实体属性值转换成关键字字符串服务
        /// </summary>
        NotFoundEntityAttributeValueKeyConvertServiceByAttributeType = 314721652,
        /// <summary>
        /// 实体属性元数据的值类型与实体属性值转换成关键字字符串服务期望的不一致
        /// </summary>
        EntityAttributeMetadataValueTypeNotMatchEntityAttributeValueKeyConvertService = 314721653,
        /// <summary>
        /// 实体属性元数据的值类型与实际的类型不匹配
        /// </summary>
        EntityAttributeMetadataValueTypeNotMatchActual = 314721654,
        /// <summary>
        ///找不到主关键字的任何关联关系 
        /// </summary>
        NotFoundEntityInfoKeyRelation = 314721655,
        /// <summary>
        /// 实体关键字的值要求非空
        /// </summary>
        EntityKeyValueRequireNotNull = 314721656,
        /// <summary>
        /// 实体属性元数据的值类型与实体属性值转换成关键字字符串服务要求的不一致
        /// </summary>
        EntityAttributeMetadataValueTypeNotMatchEntityAttributeValueKeyConvertServiceRequire = 314721657,
        /// <summary>
        /// 实体关键字的值类型与实体属性元数据的值类型不匹配
        /// </summary>
        EntityKeyValueNotMatchEntityAttributeMetadataValueType = 314721658,
        /// <summary>
        /// 实体关键字分解的数量与备用关键字属性的数量不一致
        /// </summary>
        EntityKeyValueCountNotEqualAlternateKeyAtributeCount = 314721659,
        /// <summary>
        /// 实体关键字分解的数量与主关键字属性的数量不一致
        /// </summary>
        EntityKeyValueCountNotEqualKeyAtributeCount = 314721660,
        /// <summary>
        /// 找不到指定实体类型的实体元数据
        /// </summary>
        NotFoundEntityInfoByEntityType = 314721661,
        /// <summary>
        /// 找不到指定名称的通用审批配置节点获取待处理用户服务
        /// </summary>
        NotFoundCommonSignConfigurationNodeGetExecuteUserServiceByName = 314721680,
        /// <summary>
        /// 找不到指定审批类型的审批类型获取通用审批配置节点审批后处理服务
        /// </summary>
        NotFoundCommonSignConfigurationNodeSignExtensionServiceBySignType = 314721681,
        /// <summary>
        /// 找不到指定名称的通用审批配置入口服务
        /// </summary>
        NotFoundCommonSignConfigurationEntryServiceByName = 314721682,
        /// <summary>
        /// 找不到指定实体类型的工作流资源关键字与实体关键字转换服务
        /// </summary>
        NotFoundWorkflowResourceKeyEntityKeyConvertServiceByEntityType = 314721683,
        /// <summary>
        /// 在指定的通用审批配置中未设置入口阶段
        /// </summary>
        NotSetEntryStageInCommonSignConfiguration = 314721684,
        /// <summary>
        /// 在指定的通用审批配置阶段中找不到任何指定状态的节点
        /// </summary>
        NotFoundNodeInCommonSignConfigurationStageByStatus = 314721685,
        /// <summary>
        /// 找不到指定名称的通用审批完成后处理服务
        /// </summary>
        NotFoundCommonSignConfigurationCompleteServiceByName = 314721686,
        /// <summary>
        /// 找不到指定名称的通用审批配置入口节点寻找服务
        /// </summary>
        NotFoundCommonSignConfigurationEntryNodeFindServiceByName = 314721687,
        /// <summary>
        /// 通用审批配置入口节点状态错误
        /// </summary>
        CommonSignConfigurationEntryNodeStatusError = 314721688,

        /// <summary>
        /// 通用审批配置下一节点状态错误
        /// </summary>
        CommonSignConfigurationNextNodeStatusError = 314721689,

        /// <summary>
        /// 通用审批配置节点与流程下一个节点不是同一个配置
        /// </summary>
        CommonSignConfigurationNodeNextNodeNotSameConfiguration = 314721690,
        /// <summary>
        /// 通用审批配置入口节点的所属配置与当前配置不一致
        /// </summary>
        CommonSignConfigurationEntryNodeNotSameConfiguration = 314721691,
        /// <summary>
        /// 在指定的通用审批配置中，找不到指定名称的节点
        /// </summary>
        NotFoundCommonSignConfigurationNodeByName = 314721692,
        /// <summary>
        /// 在指定的通用审批配置中,入口操作不接受指定的动作
        /// </summary>
        CommonSignConfigurationEntryNotAcceptActionName = 314721693,
        /// <summary>
        /// 找不到指定名称的通用审批配置节点创建流程处理服务
        /// </summary>
        NotFoundCommonSignConfigurationNodeCreateFlowExecuteServiceByName = 314721694,
        /// <summary>
        /// 找不到指定动作名称的通用审批配置初始化动作
        /// </summary>
        NotFoundCommonSignConfigurationRootActionByActionName = 314721695,
        /// <summary>
        /// 找不到指定动作名称的通用审批配置节点动作
        /// </summary>
        NotFoundCommonSignConfigurationNodeActionByActionName = 314721696,
        /// <summary>
        /// 找不到指定名称的获取可以进行入口操作的用户列表服务
        /// </summary>
        NotFoundCommonSignConfigurationEntryGetExecuteUsersServiceByName = 314721697,
        /// <summary>
        /// 指定的用户不能执行指定的通用审批配置初始化动作的入口方法
        /// </summary>
        CommonSignConfigurationRootActionUserCanNotEntry = 314721698,
        /// <summary>
        /// 在指定名称的通用审批配置节点动作中，需要外部传入用户，但没有传入
        /// </summary>
        CommonSignConfigurationNodeActionManualUserEmpty = 314721699,
        /// <summary>
        /// 通用审批配置节点中下一节点接收的用户验证失败
        /// </summary>
        CommonSignConfigurationNodeNextNodeManaulUserServiceValidateError = 314721700,
        /// <summary>
        /// 找不到指定审批类型的通用审批配置节点动作转换审批类型配置对象服务
        /// </summary>
        NotFoundCommonSignConfigurationNodeActionSignTypeConfigurationConvertServiceBySignType = 314721701,
        /// <summary>
        /// 找不到指定工作流资源类型的通用审批配置
        /// </summary>
        NotFoundCommonSignConfigurationByWorkflowResourceType = 314721702,
        /// <summary>
        /// 在指定的通用审批配置中，指定名称的节点的状态不正确
        /// </summary>
        CommonSignConfigurationNodeStatusError = 314721703,

        /// <summary>
        /// 找不到指定名称的通用审批配置节点动作直接跳转处理服务
        /// </summary>
        NotFoundCommonSignConfigurationNodeDirectGoExecuteServiceByName = 314721704,

        /// <summary>
        /// 已存在相同名称的上传文件
        /// </summary>
        ExistUploadFileByName= 314721901,
        /// <summary>
        /// 找不到指定后缀名的文件后缀名与文件类型映射
        /// </summary>
        NotFoundFileSuffixFileTypeMapBySyffix= 314721902,
        /// <summary>
        /// 找不到指定文件类型的后缀名列表
        /// </summary>
        NotFoundSuffixsByFileType = 314721903,
        /// <summary>
        /// 找不到指定后缀名的文件后缀名与Mime映射
        /// </summary>
        NotFoundFileSuffixMimeMapBySyffix = 314721904,
        /// <summary>
        /// 找不到指定类型的上传文件源处理服务
        /// </summary>
        NotFoundUploadFileSourceExecuteServiceByType = 314721905,
        /// <summary>
        /// 找不到指定id的上传文件
        /// </summary>
        NotFoundUploadFileById= 314721906,
        /// <summary>
        /// 找不到指定key的上传文件处理服务
        /// </summary>
        NotFoundUploadFileHandleServiceByKey= 314721910,
        /// <summary>
        /// 找不到指定名称和状态的上传文件处理配置
        /// </summary>
        NotFoundUploadFileHandleConfigurationByNameAndStatus = 314721911,
        /// <summary>
        /// 找不到指定Id的上传文件处理记录
        /// </summary>
        NotFoundUploadFileHandleRecordById = 314721912,
        /// <summary>
        /// 指定的上传文件处理记录中不包含上传文件
        /// </summary>
        UploadFileHandleRecordNotHasFile = 314721913,


        /// <summary>
        /// api参数验证错误
        /// </summary>
        ApiModelValidateError = 314722100,

        /// <summary>
        /// BussinessAction配置中的recordTypeElement没有对应的实现
        /// </summary>
        RecordTypeElementError = 314722101,
        /// <summary>
        /// Sql执行超时
        /// </summary>
        SqlExecuteTimeout= 314723001,
        /// <summary>
        /// http请求出错
        /// </summary>
        HttpRequestError= 314724001,
        /// <summary>
        /// 找不到指定表达式类型的工作流活动参数处理
        /// </summary>
        NotFoundRealWorkfolwActivityParameterHandleByType= 314724100,
        /// <summary>
        /// 工作流活动参数数据处理验证失败
        /// </summary>
        RealWorkfolwActivityParameterDataHandleValidateError= 314724101,
        /// <summary>
        /// 工作流活动结果中的输出参数名称在工作流活动的输出参数中未定义
        /// </summary>
        NotFoundRealWorkfolwActivityOutputParameterByActivityResult = 314724102,

        /// <summary>
        /// 工作流活动输出参数数据处理验证失败
        /// </summary>
        RealWorkfolwActivityOutputParameterDataHandleValidateError = 314724103,
        /// <summary>
        /// 工作流活动参数处理出错
        /// </summary>
        RealWorkfolwActivityParameterHandleExecuteError= 314724104,
        /// <summary>
        /// 工作流活动配置转换成XML时出错
        /// </summary>
        RealWorkfolwActivityConfigurationParseXMLError = 314724105,
        /// <summary>
        /// 找不到指定名称的工作流活动解析
        /// </summary>
        NotFoundRealWorkfolwActivityResolveByName= 314724106,
        /// <summary>
        /// 找不到指定名称的工作流活动计算
        /// </summary>
        NotFoundRealWorkflowActivityCalculateByName = 314724107,
        /// <summary>
        /// 工作流活动描述的Data属性的类型不匹配
        /// </summary>
        RealWorkfolwActivityDescriptionDataTypeNotMatch= 314724108,
        /// <summary>
        /// 在工作流活动描述中找不到指定名称的输入参数
        /// </summary>
        NotFoundRealWorkfolwActivityDescriptionInputByName= 314724109,
        /// <summary>
        /// 在工作流活动描述中指定名称的输入参数的结果类型不匹配
        /// </summary>
        RealWorkfolwActivityDescriptionInputResultTypeNotMatch = 314724110,
        /// <summary>
        /// 工作流活动描述中指定的内部输入参数的计算结果类型不匹配
        /// </summary>
        RealWorkfolwActivityDescriptionInnerInputResultTypeNotMatch = 314724111,
        /// <summary>
        /// 工作流活动配置中，指定的属性转换为指定的类型时出错
        /// </summary>
        RealWorkfolwActivityConfigurationAttributeParseTypeError = 314724112,
        /// <summary>
        /// 找不到指定错误类型的错误重试检查处理工厂
        /// </summary>
        NotFoundExceptionRetryCheckHandleByType= 314725001,
        /// <summary>
        /// 找不到指定类型的Grpc服务端凭证生成服务
        /// </summary>
        NotFoundGrpcServerCredentialsGeneratorServiceByType= 314726001,
        /// <summary>
        /// 找不到指定类型的Grpc通道凭证生成服务
        /// </summary>
        NotFoundGrpcChannelCredentialsGeneratorServiceByType = 314726002,
        /// <summary>
        /// 找不到指定类型的OData客户端生成器
        /// </summary>
        NotFoundODataClientGeneratorByType = 314726101,
        /// <summary>
        /// 找不到指定配置类型的OData客户端初始化器
        /// </summary>
        NotFoundODataClientInitializationByConfiguration = 314726102,
        /// <summary>
        /// OData客户端初始化配置不正确
        /// </summary>
        ODataClientInitConfigurationNotCorrect = 314726103,

        /// <summary>
        /// 找不到指定类型的矩阵数据提供方服务
        /// </summary>
        NotFoundMatrixDataProviderServiceByType = 314726201,
        /// <summary>
        /// 找不到指定类型的矩阵数据处理方服务
        /// </summary>
        NotFoundMatrixDataHandlerServiceByType = 314726202,
        /// <summary>
        /// 矩阵行的列数超出类型映射配置数量
        /// </summary>
        MatrixDataRowColumnCountOutTypeMappingCount= 314726203,
        /// <summary>
        /// 找不到指定类型的矩阵数据类型转换服务
        /// </summary>
        NotFoundMatrixDataTypeConvertServiceByType= 314726204,

        /// <summary>
        /// 找不到指定类型的针对日志提供方的日志构建器处理
        /// </summary>
        NotFoundLoggingBuilderProviderHandlerByType= 314726300,
        /// <summary>
        /// 找不到指定类型的IEnumerableItemDisplayService
        /// </summary>
        NotFoundIEnumerableItemDisplayServiceByType = 314726400,
        /// <summary>
        /// 找不到指定类型的远程服务验证信息生成服务
        /// </summary>
        NotFoundRemoteServiceAuthInfoGeneratorServiceByType= 314726500,
        /// <summary>
        /// 找不到指定名称的远程服务描述
        /// </summary>
        NotFoundRemoteServiceDescriptionByName= 314726501,
        /// <summary>
        /// 找不到指定源信息和Id的分片存储信息
        /// </summary>
        NotFoundMultipartStorgeInfoBySourceInfoAndID= 314726600,
        /// <summary>
        /// 存在相同名称的未完成分片存储信息
        /// </summary>
        ExistRunMultipartStorgeInfoByName = 314726601,

        /// <summary>
        /// 阿里OSS分片单一文件超出最大限制
        /// </summary>
        AliOSSMultipartExceedTotalMaxSize = 314726700,
        /// <summary>
        /// 阿里OSS分片数量超出最大限制
        /// </summary>
        AliOSSMultipartExceedMaxNumer = 314726701,
        /// <summary>
        /// 阿里OSS分片每片大小超出最大限制
        /// </summary>
        AliOSSMultipartExceedMaxPerSize = 314726702,
        /// <summary>
        ///  阿里OSS分片每片大小低于最小限制
        /// </summary>
        AliOSSMultipartLessMaxPerSize = 314726703,
        /// <summary>
        /// 阿里OSS分片存储信息明细中的数据位置不正确
        /// </summary>
        AliOSSMultiparStorgeInfoDetailDataPositionNotCorrect= 314726704,
        /// <summary>
        /// 阿里OSS分片存储信息的状态不允许上传
        /// </summary>
        AliOSSMultiparStorgeInfoStatusNotAllowUpload = 314726705,
        /// <summary>
        /// 阿里OSS分片存储信息的状态不允许执行完成操作
        /// </summary>
        AliOSSMultiparStorgeInfoStatusNotAllowComplete = 314726706,
        /// <summary>
        /// 阿里OSS分片存储信息不允许执行完成操作,原因为包含有未完成的明细
        /// </summary>
        AliOSSMultiparStorgeInfoNotAllowCompleteForUnDoDetail = 314726707,
        /// <summary>
        /// 阿里OSS分片存储信息的状态不允许上传
        /// </summary>
        AliOSSMultiparStorgeInfoStatusNotAllowCopy = 314726708,
        /// <summary>
        /// 阿里OSS中找不到指定名称的文件
        /// </summary>
        AliOSSNotFoundObject= 314726709,
        /// <summary>
        /// 找不到指定类型的生成Jwt生成时使用的签名密钥服务
        /// </summary>
        NotFoundJwtGenerateCreateSignKeyServiceByType = 314726801,
        /// <summary>
        /// 找不到指定类型的生成Jwt验证时使用的签名密钥服务
        /// </summary>
        NotFoundJwtGenerateValidateSignKeyServiceByType = 314726802,
        /// <summary>
        /// 找不到指定类型的Jwt验证参数组装服务
        /// </summary>
        NotFoundjwtValidateParameterBuildServiceByType = 314726803,
        /// <summary>
        /// 找不到指定队列类型的队列实际处理服务
        /// </summary>
        NotFountCommonQueueRealExecuteServiceByType= 314727001,
        /// <summary>
        /// 找不到指定名称的Azure服务总线From转换服务
        /// </summary>
        NotFoundAzureServiceBusMessageConvertFromServiceByName= 314727011,
        /// <summary>
        /// 找不到指定名称的Azure服务总线To转换服务
        /// </summary>
        NotFoundAzureServiceBusMessageConvertToServiceByName = 314727012,
        /// <summary>
        /// 找不到指定类型名称的通用消息客户端类型
        /// </summary>
        NotFoundCommonMessageClientType= 314727021,
        /// <summary>
        /// 找不到指定消息类型的消息处理服务
        /// </summary>
        NotFoundCommonMessageHandleServiceByMessageType = 314727031,
        /// <summary>
        /// 在指定名称的表达式计算器中，要计算的表达式为空
        /// </summary>
        ExpressionEmptyInExpressionCalculatorByName= 314727101,
        /// <summary>
        /// 指定的表达式格式不正确
        /// </summary>
        ExpressionFormatError = 314727102,
        /// <summary>
        /// 在指定的表达式的存储之中，找不到指定键的值
        /// </summary>
        NotFoundValueInExpressionStoreValues = 314727103,
        /// <summary>
        /// 在指定的表达式中，找不到公式服务列表
        /// </summary>
        NotFoundFormulaServiceListFromExpression= 314727104,
        /// <summary>
        /// 在指定的表达式的公式服务列表中，找不到指定的公式服务
        /// </summary>
        NotFoundFormulaServiceFormServiceList = 314727105,
        /// <summary>
        /// 找不到指定类型的工作流活动服务
        /// </summary>
        NotFoundWorkflowActivityServiceByType= 314727111,
        /// <summary>
        /// 指定类型的工作流活动服务参数数量不正确
        /// </summary>
        WorkflowActivityServiceParameterCountError= 314727120,
        /// <summary>
        /// 指定类型的工作流活动服务的指定位数参数类型不正确
        /// </summary>
        WorkflowActivityServiceParameterTypeError = 314727121,
        /// <summary>
        /// 找不到指定缓存类型的实际KV缓存访问服务
        /// </summary>
        NotFoundRealKVCacheVisitServiceByCacheType= 314727201,
        /// <summary>
        /// 找不到指定版本名称的KV缓存版本服务
        /// </summary>
        NotFoundIKVCacheVersionServiceByName= 314727202,
        /// <summary>
        /// 业务动作验证失败
        /// </summary>
        BusinessActionValidateFail = 314727300,
        /// <summary>
        /// 找不到指定名称的业务动作
        /// </summary>
        NotFoundBusinessActionByName = 314727301,
        /// <summary>
        /// 找不到指定类型的机密数据服务
        /// </summary>
        NotFoundSecurityVaultServiceByType= 314727302,
        /// <summary>
        /// 找不到指定名称的国际化处理服务工厂
        /// </summary>
        NotFountInternationalizationHandleServiceFactoryByName = 314727351,

        /// <summary>
        /// 找不到指定名称的Http请求扩展上下文处理服务工厂
        /// </summary>
        NotFountHttpExtensionContextHandleServiceFactoryByName = 314727352,

        /// <summary>
        /// 不支持指定的AzureVault验证方式
        /// </summary>
        NotSupportAzureVaultAuthType = 314727401,
        /// <summary>
        /// 找不到指定类型的Azure令牌凭据生成服务
        /// </summary>
        NotFoundTokenCredentialGeneratorServiceByType= 314727501,
        /// <summary>
        /// 找不到指定名称的Azure令牌凭据生成器
        /// </summary>
        NotFoundTokenCredentialGeneratorByName = 314727502,
        /// <summary>
        /// 找不到指定类型的分布式操作记录服务
        /// </summary>
        NotFoundDTOperationRecordServiceByType= 314727601,
        /// <summary>
        /// 找不到指定类型的分布式操作数据服务
        /// </summary>
        NotFoundDTOperationDataServiceByType = 314727602,
        /// <summary>
        /// 在指定的存储组中找不到指定名称的组成员
        /// </summary>
        NotFounStoreGroupMemberByName= 314727701,
        /// <summary>
        /// 找不到指定名称的存储组
        /// </summary>
        NotFounStoreGroupByName = 314727702,
        /// <summary>
        /// 指定的存储组中指定名称的组成员的存储信息不是指定的类型
        /// </summary>
        StoreGroupMemberInfoTypeError = 314727703,
        /// <summary>
        /// 在指定的存储信息中找不到指定的实体表映射
        /// </summary>
        NotFoundEntityNameInStoreInfoFromStoreGroup = 314727704,
        /// <summary>
        /// 在指定的存储信息中找不到指定的实体表映射
        /// </summary>
        NotFoundEntityNameInStoreInfo = 314727705,
        /// <summary>
        /// 存储信息类型不正确
        /// </summary>
        StoreInfoTypeError= 314727706,
        /// <summary>
        /// 在指定存储组下找不到指定的成员
        /// </summary>
        NotFoundStoreGroupMemberInGroup= 314727707,
        /// <summary>
        /// 分布式操作数据在Cancel时发生并发错误
        /// </summary>
        DTOperationDataConcurrenceErrorInCancel = 314727708,
        /// <summary>
        /// 在指定的实体上下文类型对象中，找不到指定实体的实体表名映射键值对
        /// </summary>
        NotFoundEntityTableMappintForDBContext= 314727801,
        /// <summary>
        /// 找不到指定名称的Redis客户端工厂
        /// </summary>
        NotFoundRedisClientFactoryByName= 314727900,
        /// <summary>
        /// 找不到指定类型的Redis客户端生成服务
        /// </summary>
        NotFoundRedisClientGenerateServiceByType = 314727901,
        /// <summary>
        /// 生成Redis客户端错误
        /// </summary>
        GenerateRedisClientError= 314727902,
        /// <summary>
        /// 找不到指定类型的应用程序锁服务
        /// </summary>
        NotFoundApplicationLockServiceByType= 314727931,
        /// <summary>
        /// 获取Redis应用程序锁超时
        /// </summary>
        AcquireRedisApplicationLockExpire= 314727932,
        /// <summary>
        /// 找不到指定类型的应用程序限流服务
        /// </summary>
        NotFoundApplicationLimitServiceByType = 314727933,
        /// <summary>
        /// 从Redis限流令牌桶中获取令牌超时
        /// </summary>
        AcquireRedisLimitTokenExpire = 314727934,
        /// <summary>
        /// 找不到指定标签的替换内容生成服务
        /// </summary>
        NotFoundReplaceContentGenerateServiceByLabel= 314727950,

        /// <summary>
        /// 在容器中找不到指定名称的消息请求响应主机服务
        /// </summary>
        NotFoundSRRHostServiceInContainerByName = 314727960,
        /// <summary>
        /// 找不到指定请求类型的请求处理描述
        /// </summary>
        NotFoundSRRRequestHandlerDescriptionByType = 314727961,

        /// <summary>
        /// 实际类型与中间数据处理器要求的请求类型不匹配
        /// </summary>
        MiddleDataTypeNotMatchHandler = 314728001,
        /// <summary>
        /// 找不到指定请求类型名称的DAX消息处理
        /// </summary>
        NotFoundDAXMessageHandleByRequestTypeFullName = 314728101,
        /// <summary>
        /// 在DAX服务令牌生成服务中找不到指定名称的参数
        /// </summary>
        NotFoundParameterInDAXServiceTokenGenerateService = 314728121,
        /// <summary>
        /// 在DAX服务令牌生成服务中指定参数的类型不匹配
        /// </summary>
        ParameterTypeNotMatchInDAXServiceTokenGenerateService = 314728122,
        /// <summary>
        /// 找不到指定名称的DAX服务令牌生成服务
        /// </summary>
        NotFoundDAXServiceTokenGenerateServiceByName = 314728123,
        /// <summary>
        /// 找不到指定类型的DAX服务工厂服务
        /// </summary>
        NotFoundDAXServiceFactoryServiceByType = 314728124,
        /// <summary>
        /// 找不到指定名称的DAX服务工厂
        /// </summary>
        NotFountDAXServiceFactorybyName = 314728125,
        /// <summary>
        /// Azure复制Blob出错
        /// </summary>
        AzureCopyBlobError = 314728501,
    }

    /// <summary>
    /// 定义文件的类型
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// 图片
        /// </summary>
        Image=0,
        /// <summary>
        /// 视频
        /// </summary>
        Video=1,
        /// <summary>
        /// 音频
        /// </summary>
        Audio=2,
        /// <summary>
        /// 其他
        /// </summary>
        Other=3
    }

    /// <summary>
    /// 综合订单支付状态 0:不需要支付，1:等待支付，3:支付成功
    /// </summary>
    public enum PayStatus 
    {
        /// <summary>
        /// 不需要支付
        /// </summary>
        NoNeedPay = 0,
        /// <summary>
        /// 等待支付
        /// </summary>
        ToBePay = 1,
        /// <summary>
        /// 支付成功
        /// </summary>
        Paid = 3
    }

    /// <summary>
    /// 综合订单订单状态 1:待审批,2:审批中,3:已通过,4:已驳回,5:订单取消,6:订单退货,7:交易完成,8:退款中,9:退款完成
    /// </summary>
    public enum Tc_OrderState 
    {
        /// <summary>
        /// 待审批
        /// </summary>
        Pending = 1,
        /// <summary>
        /// 审批中
        /// </summary>
        UnderReview = 2,
        /// <summary>
        /// 已通过
        /// </summary>
        Passed = 3,
        /// <summary>
        /// 已驳回
        /// </summary>
        Dismissed = 4,
        /// <summary>
        /// 订单取消
        /// </summary>
        Cancel = 5,
        /// <summary>
        /// 订单退货
        /// </summary>
        Returns = 6,
        /// <summary>
        /// 交易完成
        /// </summary>
        Complete = 7,
        /// <summary>
        /// 退款中
        /// </summary>
        Refunding = 8,
        /// <summary>
        /// 退款完成
        /// </summary>
        RefundCompleted = 9
    }
}
