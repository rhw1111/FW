using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary
{
    /// <summary>
    /// 上下文类型
    /// </summary>
    public static class ContextTypes
    {
        /// <summary>
        /// 当前用户Id
        /// </summary>
        public const string CurrentUserId = "CurrentUserId";
        /// <summary>
        /// 当前用户语言编码
        /// </summary>
        public const string CurrentUserLcid = "CurrentUserLcid";
        /// <summary>
        /// 当前用户时区与UTC相差的小时数
        /// </summary>
        public const string CurrentUserTimezoneOffset = "CurrentUserTimezoneOffset";
        /// <summary>
        /// 字典
        /// </summary>
        public const string Dictionary = "Dictionary";
        /// <summary>
        /// DI服务提供方
        /// </summary>
        public const string ServiceProvider = "ServiceProvider";
        /// <summary>
        /// 当前使用的DI
        /// </summary>
        public const string DI = "DI";
    }

    /// <summary>
    /// 文本片段code
    /// </summary>
    public static class TextCodes
    {
        /// <summary>
        /// 不是AccessToken
        /// 格式为“传入的字符串不是AccessToken类型”
        /// </summary>
        public const string NotAccessToken = "NotAccessToken";
        /// <summary>
        /// 不是RefreashAccessToken
        /// 格式为“传入的字符串不是RefreashAccessToken类型”
        /// </summary>
        public const string NotRefreashAccessToken = "NotRefreashAccessToken";
        /// <summary>
        ///  指定类型没有实现指定接口
        ///  格式为“类型{0}没有实现接口{1}”
        ///  {0}：类型
        ///  {1}：接口
        /// </summary>
        public const string TypeNotImplimentInterface = "TypeNotImplimentInterface";
        /// <summary>
        /// 指定类型不是所需要的类型
        /// 格式为“类型{0}不是所需的类型，要求的类型为{1}，发生位置为{2}”
        /// {0}：当前类型
        /// {1}：要求的类型
        /// {1}：发生的位置
        /// </summary>
        public const string TypeNotRequire = "TypeNotRequire";
        /// <summary>
        /// 在httpheader中找不到WhitelistAuthorization
        /// 格式为“在http头中没有找到WhitelistAuthorization”
        /// </summary>
        public const string NotFoundWhitelistAuthorizationInHttpHeader = "NotFoundWhitelistAuthorizationInHttpHeader";
        /// <summary>
        /// 在httpheader中找不到SystemName
        /// 格式为“在http头中没有找到SystemName”
        /// </summary>
        public const string NotFoundSystemNameInHttpHeader = "NotFoundSystemNameInHttpHeader";
        /// <summary>
        ///白名单验证未通过
        ///格式为“系统操作{0}针对调用方系统{1}的白名单验证未通过，原因：{2}”
        ///{0}：系统操作名称
        ///{1}：调用方系统名称
        ///{2}：失败原因
        /// </summary>
        public const string WhitelistValidateFail = "WhitelistValidateFail";
        /// <summary>
        /// 找不到指定名称的白名单系统操作
        /// 格式为“找不到名称为{0}的白名单系统操作”
        /// {0}为系统操作名称
        /// </summary>
        public const string NotFoundWhitelistSystemOperationWithName = "NotFoundWhitelistSystemOperationWithName";
        /// <summary>
        /// 找不到指定名称和状态的白名单系统操作
        /// 格式为“找不到名称为{0}、状态为{1}的白名单系统操作”
        /// {0}为系统操作名称
        /// {1}为状态的标签值
        /// </summary>
        public const string NotFoundWhitelistSystemOperationWithNameStatus = "NotFoundWhitelistSystemOperationWithNameStatus";

        /// <summary>
        /// 在指定名称的系统操作中找不到指定系统名称的白名单
        /// 格式为“在系统操作{0}中找不到系统名称为{1}的白名单”
        /// {0}为系统操作名称
        /// {1}为白名单的系统名称
        /// </summary>
        public const string NotFoundWhitelistInSystemOperationWithName = "NotFoundWhitelistInSystemOperationWithName";

        /// <summary>
        /// 在指定名称的系统操作中找不到指定系统名称和状态的白名单
        /// 格式为“在系统操作{0}中找不到系统名称为{1}、状态为{2}的白名单”
        /// {0}为系统操作名称
        /// {1}为白名单的系统名称
        /// {2}为状态的标签值
        /// </summary>
        public const string NotFoundWhitelistInSystemOperationWithNameStatus = "NotFoundWhitelistInSystemOperationWithNameStatus";


        /// <summary>
        /// 在指定名称的系统操作验证方法中，签名不是正确的JWT格式
        /// 格式为“在系统操作{0}的验证方法中，签名{1}不是正确的JWT格式”
        /// {0}为系统操作名称
        /// {1}为签名内容
        /// </summary>
        public const string SignatureNotCorrectJWTFormatInSystemOperationValidation = "SignatureNotCorrectJWTFormatInSystemOperationValidation";
        /// <summary>
        /// 在指定名称的系统操作验证方法中，签名进行JWT验证失败
        /// 格式为“在系统操作{0}的验证方法中，签名{1}进行JWT验证失败”
        /// {0}为系统操作名称
        /// {1}为签名内容
        /// </summary>
        public const string SignatureJWTValidationFailInSystemOperation = "SignatureJWTValidationFailInSystemOperation";

        /// <summary>
        /// 在指定名称的系统操作验证方法中，JWT的Playload中找不到指定键的键值对
        /// 格式为“在系统操作{0}的验证方法中，JWT的Playload中找不到键为{1}的键值对”
        /// {0}为系统操作名称
        /// {1}为指定的键名称
        /// </summary>
        public const string NotFoundKeyNameInSystemOperation = "NotFoundKeyNameInSystemOperation";

        /// <summary>
        /// 在指定名称的系统操作验证方法中，JWT中的系统名称与传入的系统名称不相等
        /// 格式为“在系统操作{0}的验证方法中，签名中的系统名称为{1}，传入的系统名称为{2}，两者不相等”
        /// {0}为系统操作名称
        /// {1}为签名中的系统名称
        /// {2}为传入的系统名称
        /// </summary>
        public const string SystemNameNotEqualInSystemOperationValidation = "SystemNameNotEqualInSystemOperationValidation";
        /// <summary>
        /// 在指定名称的系统操作验证方法中，指定系统名称的白名单的JWT中的时间已经过期
        /// 格式为“在系统操作{0}的验证方法中，白名单系统名称为{1}的JWT中的过期时间为{2}，已经过期”
        /// {0}为系统操作名称
        /// {1}为白名单中的系统名称
        /// {2}为签名中的过期时间
        /// </summary>
        public const string JWTExpireInSystemOperationValidation = "JWTExpireInSystemOperationValidation";

        /// <summary>
        /// 在指定名称的系统操作验证方法中，指定系统名称的白名单IP检测失败
        /// 格式为“在系统操作{0}的验证方法中，白名单系统名称为{1}的合法IP为{2}，访问IP为{3}，两者不匹配”
        /// {0}为系统操作名称
        /// {1}为白名单中的系统名称
        /// {2}为白名单中合法IP
        /// {3}为传入的IP
        /// </summary>
        public const string IPFailInSystemOperationValidation = "IPFailInSystemOperationValidation";
        /// <summary>
        /// 找不到指定名称的Http声明生成器
        /// 格式为“找不到名称为{0}的http声明生成器”
        /// {0}为http声明生成器名称
        /// </summary>
        public const string NotFoundHttpClaimGeneratorByName = "NotFoundHttpClaimGeneratorByName";

        /// <summary>
        /// 内部错误
        /// 格式为“系统内部错误，请查看系统日志”
        /// </summary>
        public const string InnerError = "InnerError";
        /// <summary>
        /// 用户授权失败
        /// 格式为“用户授权失败，没有找到对应的身份信息”
        /// </summary>
        public const string AuthorizeFail = "AuthorizeFail";
        /// <summary>
        /// 令牌桶算法中，数值超出最大阈值
        /// 格式为“令牌桶超出最大阈值{0}”
        /// {0}为设定的最大阈值数
        /// </summary>
        public const string TokenBucketOverflow = "TokenBucketOverflow";
        /// <summary>
        /// 消息执行出错信息
        /// 格式为“消息执行出错，消息key:{0},消息type:{1},消息内容:{2},错误内容:{3}”
        /// {0}为消息的关键字
        /// {1}为消息类型
        /// {2}为消息内容
        /// {3}为错误内容
        /// </summary>
        public const string SMessageExecuteError = "SMessageExecuteError";
        /// <summary>
        /// 消息类型监听者的类型不正确
        /// 格式为“消息类型{0}中，名称为{1}的监听者工厂的类型{2}未实现接口IFactory<ISMessageListener>”
        /// {0}为消息类型名称
        /// {1}为监听者名称
        /// {2}监听者工厂的实际类型
        /// </summary>
        public const string SMessageTypeListenerTypeError = "SMessageTypeListenerTypeError";
        /// <summary>
        /// 按指定消息类型没有找到消息队列
        /// 格式为“消息类型为{0}，但没有找到对应的队列”
        /// {0}为消息类型
        /// </summary>
        public const string NotFoundSQueueByMessageType = "NotFoundSQueueByMessageType";
        /// <summary>
        /// 没有找到指定名称的队列执行组
        /// 格式为“没有找到名称为{0}的队列执行组”
        /// {0}为执行组名称
        /// </summary>
        public const string NotFoundSQueueProcessGroupByName = "NotFoundSQueueProcessGroupByName";
        /// <summary>
        /// 没有找到指定名称的环境声明生成器
        /// 格式为“没有找到名称为{0}的环境声明生成器”
        /// {0}为环境声明生成器名称
        /// </summary>
        public const string NotFoundEnvironmentClaimGeneratorByName = "NotFoundEnvironmentClaimGeneratorByName";
        /// <summary>
        /// 没有找到指定名称的上下文生成器
        /// 格式为“没有找到名称为{0}的上下文生成器”
        /// {0}为上下文生成器名称
        /// </summary>
        public const string NotFoundClaimContextGeneratorByName = "NotFoundClaimContextGeneratorByName";
        /// <summary>
        /// 已经达到池的最大长度
        /// 格式为“池{0}已经达到最大长度，最大长度为{1}”
        /// {0}为池的名称
        /// {1}为池的最大长度
        /// </summary>
        public const string PoolLengthOverflow = "PoolLengthOverflow";
        /// <summary>
        /// Tcp数据处理类型不正确
        /// 格式为“Tcp监听{0}中，数据处理的工厂类型{1}未实现接口IFactory<ITcpDataExecute>”
        /// {0}为监听器名称
        /// {1}为数据处理工厂类型
        /// </summary>
        public const string TcpDataExecuteTypeError = "TcpDataExecuteTypeError";

        /// <summary>
        /// Tcp客户端数据处理类型不正确
        /// 格式为“Tcp客户端{0}中，数据处理的工厂类型{1}未实现接口IFactory<ITcpClientDataExecute>”
        /// {0}为客户端名称
        /// {1}为数据处理工厂类型
        /// </summary>
        public const string TcpClientDataExecuteTypeError = "TcpClientDataExecuteTypeError";
        /// <summary>
        /// 没有找到指定名称的Tcp客户端终结点
        /// 格式为“没有找到名称为{0}的Tcp客户端终结点”
        /// {0}为客户端终结点名称
        /// </summary>
        public const string NotFoundTcpClientEndpointByName = "NotFoundTcpClientEndpointByName";
        /// <summary>
        /// 已经存在相同名称的Tcp监听器
        /// 格式为“名称为{0}的Tcp监听器已经存在”
        /// {0}为监听器名称
        /// </summary>
        public const string ExistSameNameTcpListener = "ExistSameNameTcpListener";
        /// <summary>
        /// 没有找到指定名称的Tcp监听器
        /// 格式为“没有找到名称为{0}的Tcp监听器”
        /// {0}为Tcp监听器名称
        /// </summary>
        public const string NotFoundTcpListenerByName = "NotFoundTcpListenerByName";
        /// <summary>
        /// 没有找到指定的哈希组的任何节点
        /// 格式为“没有找到名称为{0}的哈希组的任何节点”
        /// {0}为哈希组的名称
        /// </summary>
        public const string NotFoundHashNodeByGroup = "NotFoundHashNodeByGroup";
        /// <summary>
        /// 没有找到指定名称的一致性哈希组
        /// 格式为“没有找到名称为{0}的一致性哈希组”
        /// {0}为哈希组的名称
        /// </summary>
        public const string NotFoundHashGroupByName = "NotFoundHashGroupByName";
        /// <summary>
        /// 指定的JWT格式不正确
        /// 格式为“JWT字符串{0}的格式不正确”
        /// {0}为指定的JWT字符串
        /// </summary>
        public const string JWTFormatError = "JWTFormatError";
        /// <summary>
        /// 指定的JWT签名不正确
        /// 格式为“JWT字符串{0}的签名验证失败”
        /// {0}为指定的JWT字符串
        /// </summary>
        public const string JWTSignError = "JWTSignError";
        /// <summary>
        /// 指定的JWT过期
        /// 格式为“JWT字符串{0}已经过期，过期时间为{1}”
        /// {0}为指定的JWT字符串
        /// {1}为JWT中设定的过期时间
        /// </summary>
        public const string JWTExpire = "JWTExpire";
        /// <summary>
        /// 没有找到指定类型的FillEntityService
        /// 格式为“类型为{0}的FillEntityService未找到”
        /// {0}：指定的类型
        /// </summary>
        public const string NotFoundFillEntityService = "NotFoundFillEntityService";
        /// <summary>
        /// 条件直接被设置为Fasle
        /// 格式为“条件直接被设置为Fasle”
        /// </summary>
        public const string ConditionFalse = "ConditionFalse";
        /// <summary>
        /// 找不到指定名称的标签参数处理器
        /// 格式为“找不到名称为{0}的标签参数处理器”
        /// {0}：标签参数名称
        /// </summary>
        public const string NotFoundLabelParameterHandlerByName = "NotFoundLabelParameterHandlerByName";
        /// <summary>
        /// 与指定的服务通信时出错
        /// 格式为“与服务{0}通信时出现错误，错误内容为{1}”
        /// {0}：服务地址
        /// {1}：错误内容
        /// </summary>
        public const string CommunicationServiceError = "CommunicationServiceError";

        /// <summary>
        /// 找不到指定名称的协程容器
        /// 格式为“找不到名称为{0}的协程容器”
        /// {0}：协程容器名称
        /// </summary>
        public const string NotFoundCoroutineContainerByName = "NotFoundCoroutineContainerByName";
        /// <summary>
        /// 在条件元素的参数列表中，找不到指定名称的参数
        /// 格式为“在条件元素{0}的参数列表中，找不到名称为{1}的参数”
        /// {0}：条件元素的xml节点名称
        /// {1}：参数名称
        /// </summary>
        public const string NotFoundParameterFromConditionParametersByName = "NotFoundParameterFromConditionParametersByName";

        /// <summary>
        /// 在条件元素的参数中找不到recordTypeElement属性
        /// </summary>
        public const string RecordTypeElementError = "RecordTypeElementError";

        /// <summary>
        /// 在条件元素的参数列表中，使用指定名称的元素属性值做为指定名称的参数的数组索引，索引越界
        /// 格式为“在条件元素{0}的参数列表中，元素属性{1}的值{2}做为参数{3}的数组索引，索引越界，参数的数组长度为{4}”
        /// {0}：条件元素的xml节点名称
        /// {1}：元素属性名称
        /// {2}：元素属性的值
        /// {3}：参数名称
        /// {4}：参数数组的长度
        /// </summary>
        public const string ParameterFromConditionParametersIndexOut = "ParameterFromConditionParametersIndexOut";

        /// <summary>
        /// 在条件元素的参数列表中，指定名称的参数的类型不匹配
        /// 格式为“在条件元素{0}的参数列表中，名称为{1}的参数的期望类型为{2}，但实际类型为{3}”
        /// {0}：条件元素的xml节点名称
        /// {1}：参数名称
        /// {2}：参数的期望类型
        /// {3}：参数的实际类型
        /// </summary>
        public const string ParameterFromConditionParametersTypeNotMatchByName = "ParameterFromConditionParametersTypeNotMatchByName";

        /// <summary>
        /// 在条件元素中找不到指定属性名称的属性
        /// 格式为“在条件元素{0}中，找不到名称为{1}的属性”
        /// {0}：条件元素的xml节点名称
        /// {1}：xml属性名称
        /// </summary>
        public const string NotFoundAttributeInConditionElement = "NotFoundAttributeInConditionElement";

        /// <summary>
        /// 协程本地数据尚未初始化
        /// 格式为“协程本地数据尚未初始化”
        /// </summary>
        public const string CoroutineLocalNotInit = "CoroutineLocalNotInit";
        /// <summary>
        /// 找不到指定错误类型的错误重试检查处理工厂
        /// 格式为“找不到错误类型为{0}的错误重试检查处理工厂”
        /// {0}:错误的类型
        /// </summary>
        public const string NotFoundExceptionRetryCheckHandleByType = "NotFoundExceptionRetryCheckHandleByType";
        /// <summary>
        /// 找不到指定名称的协程本地数据
        /// 格式为“找不到名称为{0}的协程本地数据”
        /// {0}：协程本地数据的名称
        /// </summary>
        public const string NotFoundCoroutineLocalByName = "NotFoundCoroutineLocalByName";
        /// <summary>
        /// 当前协程本地数据尚未指定
        /// 格式为“当前协程本地数据尚未指定”
        /// </summary>
        public const string NotFoundCoroutineLocalByCurrent = "NotFoundCoroutineLocalByCurrent";

        /// <summary>
        /// Tcp双工数据处理类型不正确
        /// 格式为“双工Tcp监听{0}中，数据处理的工厂类型{1}未实现接口IFactory<ITcpDuplexDataExecute>”
        /// {0}为监听器名称
        /// {1}为数据处理工厂类型
        /// </summary>
        public const string TcpDuplexDataExecuteTypeError = "TcpDuplexDataExecuteTypeError";

        /// <summary>
        /// 一致性哈希策略类型不正确
        /// 格式为“在一致性哈希策略{0}中，策略服务类型{1}未实现接口IFactory<IHashGroupStrategyService>”
        /// {0}为策略名称
        /// {1}为策略服务实际类型
        /// </summary>
        public const string HashGroupStrategyServiceFactoryTypeError = "HashGroupStrategyServiceFactoryTypeError";

        /// <summary>
        /// 找不到指定名称的远程服务描述
        /// 格式为“找不到名称为{0}的远程服务描述”
        /// {0}：远程服务名称
        /// </summary>
        public const string NotFoundRemoteServiceDescriptionByName = "NotFoundRemoteServiceDescriptionByName";
        /// <summary>
        /// 找不到指定前缀的序列号记录
        /// 格式为“前缀为{0}的序列号记录不存在”
        /// {0}为序列号前缀
        /// </summary>
        public const string NotFoundSerialNumberRecordByPrefix = "NotFoundSerialNumberRecordByPrefix";
        /// <summary>
        /// 指定前缀的序列号记录已经存在
        /// 格式为“前缀为{0}的序列号记录已经存在”
        /// {0}为序列号前缀
        /// </summary>
        public const string ExistFoundSerialNumberRecordByPrefix = "ExistFoundSerialNumberRecordByPrefix";
        /// <summary>
        /// 指定标签的参数的个数不正确
        /// 格式为“标签{0}要求的参数个数为{1}，而实际参数个数为{2}”
        /// {0}为标签名称
        /// {1}为该标签要求的个数
        /// {2}为该标签实际的个数
        /// </summary>
        public const string LabelParameterCountError = "LabelParameterCountError";
        /// <summary>
        /// 指定标签参数的个数要求最小数量
        /// 格式为“标签{0}要求的参数个数至少为{1}，而实际参数个数为{2}”
        /// {0}为标签名称
        /// {1}为该标签要求的最小个数
        /// {2}为该标签实际的个数
        /// </summary>
        public const string LabelParameterCountRequireMin = "LabelParameterCountRequireMin";
        /// <summary>
        /// 在模板上下文中找不到指定名称的参数
        /// 格式为“在模板上下文中找不到名称为{0}的参数”
        /// {0}：参数名称
        /// </summary>
        public const string NotFoundParameterInTemplateContextByName = "NotFoundParameterInTemplateContextByName";
        /// <summary>
        /// 找不到指定名称的系统模板
        /// 格式为“找不到指定名称的系统模板，参数名称:{0}”
        /// {0}为系统模板参数名称
        /// </summary>
        public const string NotFoundSystemTemplateParamaterNamesByName = "NotFoundSystemTemplateParamaterNamesByName";
        /// <summary>
        /// 指定系统模板参数名称标签转换类型出错
        /// 格式为“指定系统模板参数名称标签转换类型出错:{0}”
        /// {0}为转换参数名称
        /// </summary>
        public const string LabelParameterSystemTemplateParamaterNamesError = "LabelParameterSystemTemplateParamaterNamesError";
        /// <summary>
        /// 指定系统模板参数名称标签标签参数类型错误
        /// 格式为“标签{0}要求的参数{1}应为{2}，参数类型错误”
        /// {0}为标签名称
        /// {1}为参数名称
        /// {2}为类型名称
        /// </summary>
        public const string LabelParameterTypeError = "LabelParameterTypeError";
        /// <summary>
        /// 指定系统模板参数名称标签标签最小值和最大值错误
        /// 格式为“标签{0}要求的参数最小值和最大值错误，而实际最小值为{1}，最大值为{2}”
        /// {0}为标签名称
        /// {1}为最小值
        /// {2}为最大值
        /// </summary>
        public const string LabelParameterMinMaxError = "LabelParameterMinMaxError";
        /// <summary>
        /// 指定系统模板参数名称标签标签加密算法Key错误
        /// 格式为“标签{0}要求的参数中，加密算法Key错误，应该为{1}位，而实际为{2}”
        /// {0}为标签名称
        /// {1}为最小值
        /// {2}为最大值
        /// </summary>
        public const string LabelParameterDesSecurityKeyError = "LabelParameterDesSecurityKeyError";

        /// <summary>
        /// 指定的上下文实体中找不到指定的系统模板记录名称
        /// 格式为“当前上下文实体中{0}不包含指定模板记录名称{1}”
        /// {0}为上下文
        /// {1}为模板记录名称
        /// </summary>
        public const string NotFoundEntityAttributeByEntityName = "NotFoundEntityAttributeByEntityName";
        /// <summary>
        /// 在指定的类型中找不到指定名称的属性
        /// 格式为“在类型{0}中，找不到名称为{1}的属性”
        /// {0}为类型名称
        /// {1}为属性名称
        /// </summary>
        public const string NotFoundPropertyInTypeByName = "NotFoundPropertyInTypeByName";

        /// <summary>
        /// 找不到指定名称的选项集元数据
        /// 格式为“找不到名称为{0}的选项集元数据”
        /// {0}为选项集名称
        /// </summary>
        public const string NotFoundOptionSetValueMetadataByName = "NotFoundOptionSetValueMetadataByName";
        /// <summary>
        /// 在指定名称的选项集元数据中找不到指定值的选项集项
        /// 格式为“在名称为{0}的选项集元数据下找不到值为{1}的选项集项”
        /// {0}为选项集名称
        /// {1}为选项集项的值
        /// </summary>
        public const string NotFoundOptionSetValueItemInMetadataByValue = "NotFoundOptionSetValueItemInMetadataByValue";

        /// <summary>
        /// 在指定名称的选项集元数据中找不到指定值的选项集项
        /// </summary>
        public const string OptionSetValueItemIsExistInMetadata = "OptionSetValueItemIsExistInMetadata";


        /// <summary>
        /// 找不到指定名称的调度动作
        /// 格式为“找不到名称为{0}的调度动作”
        /// {0}为调度动作名称
        /// </summary>
        public const string NotFoundScheduleActionByName = "NotFoundScheduleActionByName";
        /// <summary>
        /// 找不到指定类型的调度动作初始化服务
        /// 格式为“找不到类型为{0}的调度动作初始化服务，发生位置为{1}”
        /// {0}：服务类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundScheduleActionInitServiceByType = "NotFoundScheduleActionInitServiceByType";
        /// <summary>
        /// 找不到指定名称的调度动作组
        /// 格式为“找不到名称为{0}的调度动作组”
        /// {0}为调度动作组名称
        /// </summary>
        public const string NotFoundScheduleActionGroupByName = "NotFoundScheduleActionGroupByName";
        /// <summary>
        /// 调度动作服务类型错误
        /// 格式为“调度作业{0}中，调度动作服务工厂的类型{1}未实现接口IFactory<IScheduleActionService>”
        /// {0}为调度作业名称
        /// {1}为调度动作服务工厂类型
        /// </summary>
        public const string ScheduleActionServiceTypeError = "ScheduleActionServiceTypeError";
        /// <summary>
        /// 找不到指定类型的Factory<IValidateUserKeyService>
        /// 格式为“在工作流步骤的实现中，找不到类型为{0}的Factory<IValidateUserKeyService>”
        /// {0}为工作流步骤的用户类型
        /// </summary>
        public const string NotFoundWorkflowValidateUserKeyServiceFactoryByType = "NotFoundWorkflowValidateUserKeyServiceFactoryByType";
        /// <summary>
        /// 找不到指定类型的Factory<IGetUserInfoFromWorkflowStepService>
        /// 格式为“找不到类型为{0}的Factory<IGetUserInfoFromWorkflowStepService>”
        /// {0}为工作流步骤的用户类型
        /// </summary>
        public const string NotFoundWorkflowGetUserInfoServiceFactoryByType = "NotFoundWorkflowGetUserInfoServiceFactoryByType";
        /// <summary>
        /// 指定的工作流步骤已经是完成状态
        /// 格式为“ID为{0}，资源ID为{1}，动作名称{2},状态为{3},用户类型为{4}，用户关键字为{5}的工作流步骤已经是完成状态”
        /// {0}为工作流步骤的ID
        /// {1}为关联的资源ID
        /// {2}为动作名称
        /// {3}为状态
        /// {4}为用户类型
        /// {5}为用户关键字
        /// </summary>
        public const string WorkflowStepHasCompleted = "WorkflowStepHasCompleted";
        /// 指定的工作流步骤中指定的用户已经处理过
        /// 格式为“ID为{0}，资源ID为{1}，动作名称{2},状态为{3},用户类型为{4}，用户关键字为{5}的工作流步骤中用户信息为{6}的用户已经处理过”
        /// {0}为工作流步骤的ID
        /// {1}为关联的资源ID
        /// {2}为动作名称
        /// {3}为状态
        /// {4}为用户类型
        /// {5}为用户关键字
        /// {6}为用户信息
        public const string WorkflowStepUserHasAction = "WorkflowStepUserHasAction";

        /// <summary>
        /// 找不到指定名称的系统登录终结点
        /// 格式为“找不到名称为{0}的系统登录终结点”
        /// {0}为系统登陆终结点名称
        /// </summary>
        public const string NotFoundSystemLoginEndpointByName = "NotFoundSystemLoginEndpointByName";
        /// <summary>
        /// 找不到指定类型的身份信息Http头生成服务
        /// 格式为“找不到类型为{0}的身份信息Http头生成服务，发生位置：{1}”
        /// {0}：对应生成器类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundAuthInfoHttpHeaderGeneratorServiceByType = "NotFoundAuthInfoHttpHeaderGeneratorServiceByType";
        /// <summary>
        /// 找不到指定类型的远程服务验证信息生成服务
        /// 格式为“找不到类型为{0}的远程服务验证信息生成服务，发生位置：{1}”
        /// {0}：远程服务验证信息生成服务类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundRemoteServiceAuthInfoGeneratorServiceByType = "NotFoundRemoteServiceAuthInfoGeneratorServiceByType";
        /// <summary>
        /// 找不到指定类型的声明上下文生成服务
        /// 格式为“找不到类型为{0}的声明上下文生成服务，发生位置:{1}”
        /// {0}：对应生成器类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundClaimContextGeneratorServiceByType = "NotFoundClaimContextGeneratorServiceByType";

        /// <summary>
        /// 找不到指定类型的Http声明生成服务
        /// 格式为“找不到类型为{0}的Http声明生成服务，发生位置:{1}”
        /// {0}：对应生成器类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundHttpClaimGeneratorServiceByType = "NotFoundHttpClaimGeneratorServiceByType";
        /// <summary>
        /// 找不到指定类型的环境声明生成服务
        /// 格式为“找不到类型为{0}的环境声明生成服务，发生位置:{1}”
        /// {0}：对应生成器类型
        /// </summary>
        public const string NotFoundEnvironmentClaimGeneratorServiceByType = "NotFoundEnvironmentClaimGeneratorServiceByType";
        /// <summary>
        /// 在指定名称的系统登录终结点中，找不到指定名称的关联认证终结点
        /// 格式为“名称为{0}的系统登录终结点中，找不到名称为{1}的关联认证终结点”
        /// {0}为系统登陆终结点名称
        /// {1}为认证终结点名称
        /// </summary>
        public const string NotFoundAuthorizationEndpointInSystemLoginEndpointByName = "NotFoundAuthorizationEndpointInSystemLoginEndpointByName";
        /// <summary>
        /// 在指定名称的系统登录终结点中，找不到可以处理从第三方认证系统回调请求的关联认证终结点
        /// 格式为“名称为{0}的系统登录终结点中，找不到可以处理从第三方认证系统回调请求的关联认证终结点，请求url为{1}”
        /// {0}为系统登陆终结点名称
        /// {1}为回调请求的URL
        /// </summary>
        public const string NotFoundAuthorizationEndpointInSystemLoginEndpointCanExecuteCallback = "NotFoundAuthorizationEndpointInSystemLoginEndpointCanExecuteCallback";

        /// <summary>
        /// 在第三方系统服务的Http请求中找不到指定名称的参数
        /// 格式为“在第三方系统服务{0}中的Http请求中，配置信息为{1}，找不到名称为{2}的参数”
        /// {0}：系统服务类名
        /// {1}：服务配置
        /// {2}：参数名称
        /// </summary>
        public const string NotFoundParameterFromHttpRequestInThirdPartySystemService = "NotFoundParameterFromHttpRequestInThirdPartySystemService";
        /// <summary>
        /// 在指定名称的系统登录终结点的第三方认证系统回调请求处理中，回调请求的Url中不包含returnurl参数
        /// 格式为“名称为{0}的系统登录终结点的第三方认证系统回调请求处理中，回调请求的Url中不包含returnurl参数，回调请求的Url为{1}”
        /// {0}为系统登陆终结点名称
        /// {1}为回调请求的URL
        /// </summary>
        public const string NotFoundReturnUrlQuerystringInAuthRedirectUrl = "NotFoundReturnUrlQuerystringInAuthRedirectUrl";
        /// <summary>
        /// 在指定名称的系统登录终结点的第三方认证系统回调请求处理中，回调请求的Url中不包含authname参数
        /// 格式为“名称为{0}的系统登录终结点的第三方认证系统回调请求处理中，回调请求的Url中不包含authname参数，回调请求的Url为{1}”
        /// {0}为系统登陆终结点名称
        /// {1}为回调请求的URL
        /// </summary>
        public const string NotFoundAuthNameQuerystringInAuthRedirectUrl = "NotFoundAuthNameQuerystringInAuthRedirectUrl";
        /// <summary>
        /// 在第三方认证系统回调请求中，回调请求的Url中不包含sysname参数
        /// 格式为“在第三方认证系统回调请求处理中，回调请求的Url中不包含sysname参数，回调请求的Url为{0}”
        /// {0}为回调请求的URL
        /// </summary>
        public const string NotFoundSysNameQuerystringInCallbackRequest = "NotFoundSysNameQuerystringInCallbackRequest";
        /// <summary>
        /// 指定名称的系统登录终结点验证令牌字符串失败
        /// 格式为“名称为{0}的系统登录终结点验证令牌字符串{1}失败，失败原因{2}”
        /// {0}为系统登陆终结点名称
        /// {1}为要验证的令牌字符串
        /// {2}为失败原因
        /// </summary>
        public const string SystemLoginEndpointTokenValidateError = "SystemLoginEndpointTokenValidateError";
        /// <summary>
        /// 指定名称的系统登录终结点的令牌中，找不到指定名称的信息
        /// 格式为“名称为{0}的系统登录终结点验证令牌字符串{1}中，找不到名称为{2}的信息”
        /// {0}为系统登陆终结点名称
        /// {1}为令牌字符串
        /// {2}为信息的键值
        /// </summary>
        public const string NotFoundInfoInSystemLoginEndpointTokenByName = "NotFoundInfoInSystemLoginEndpointTokenByName";

        /// <summary>
        /// 在指定的系统登录终结点中客户端重定向地址非法
        /// 格式为“名称为{0}的系统登录终结点中，客户端重定向地址{1}非法，合法地址的基地址必须为{2}”
        /// {0}：系统登录终结点名称
        /// {1}：客户端重定向地址
        /// {2}：合法地址的基地址
        /// </summary>
        public const string SystemLoginEndpointClientRedirectUrl = "SystemLoginEndpointClientRedirectUrl";

        /// <summary>
        /// 找不到指定类型的IEnumerableItemDisplayService
        /// 格式为“找不到类型为{0}的IEnumerableItemDisplayService”
        /// {0}：类型
        /// </summary>
        public const string NotFoundIEnumerableItemDisplayServiceByType = "NotFoundIEnumeratorItemDisplayServiceByType";
        /// <summary>
        /// 在通用令牌的JWT字符串中，找不到指定名称的键
        /// 格式为“在通用令牌的JWT字符串{0}中，找不到名称为{1}的键”
        /// {0}为通用令牌的JWT字符串
        /// {1}为键的名称
        /// </summary>
        public const string NotFoundKeyInCommonTokenJWT = "NotFoundKeyInCommonTokenJWT";
        /// <summary>
        /// 通用令牌登出错误
        /// 格式为“通用令牌登出错误，错误内容：{0}”
        /// {0}为错误内容
        /// </summary>
        public const string CommonTokenLogoutError = "CommonTokenLogoutError";
        /// <summary>
        /// 执行指定名称的应用时，JWT错误
        /// 格式为“在执行应用{0}时，处理的JWT{1}错误，错误原因{2}”
        /// {0}为应用的名称
        /// {1}为JWT字符串
        /// {2}为错误原因
        /// </summary>
        public const string ExecuteAppJWTError = "ExecuteAppJWTError";
        /// <summary>
        /// 在键值对配置信息中，找不到指定键值的数据
        /// 格式为“在键值对配置信息中，找不到键为{0}的数据，请检查配置文件”
        /// {0}为指定的键
        /// </summary>
        public const string NotFoundKeyFromKVConfiguration = "NotFoundKeyFromKVConfiguration";
        /// <summary>
        /// 客户端消息类型监听终结点签名错误
        /// 格式为“客户端消息类型监听终结点{0}在解析消息数据时消息数据的签名错误，请检查客户端的签名与服务端对应的签名是否一致”
        /// {0}为客户端消息类型监听终结点的名称
        /// </summary>
        public const string ClientSMessageTypeListenerEndpointSignatureError = "ClientSMessageTypeListenerEndpointSignatureError";
        /// <summary>
        /// 客户端消息类型监听终结点接收的消息已过期
        /// 格式为“客户端消息类型监听终结点{0}接收的消息已过期，过期时间为{1}”
        /// {0}为客户端消息类型监听终结点的名称
        /// {1}为消息的过期时间
        /// </summary>
        public const string ClientSMessageTypeListenerEndpointMessageExpire = "ClientSMessageTypeListenerEndpointMessageExpire";
        /// <summary>
        /// 客户端系统登陆终结点签名错误
        /// 格式为“客户端系统登陆终结点{0}在解析请求数据时请求数据的签名错误，请检查客户端的签名与服务端对应的签名是否一致”
        /// {0}为客户端系统登陆终结点的名称
        /// </summary>
        public const string ClientSystemLoginEndpointSignatureError = "ClientSystemLoginEndpointSignatureError";
        /// <summary>
        /// 客户端系统登陆终结点接收的消息已过期
        /// 格式为“客户端系统登陆终结点{0}接收的请求已过期，过期时间为{1}”
        /// {0}为客户端系统登陆终结点的名称
        /// {1}为请求的过期时间
        /// </summary>
        public const string ClientSystemLoginEndpointRequestExpire = "ClientSystemLoginEndpointRequestExpire";


        /// <summary>
        /// 系统登录终结点中已存在相同的名称数据
        /// 格式为"系统登录终结点中已存在相同的名称{0}数据"
        /// {0}名称
        /// </summary>
        public const string ExistSystemLoginEndpointByName = "ExistSystemLoginEndpointByName";
        /// <summary>
        /// 工作流用户动作中已存在相同的用户关键字数据
        /// 格式为"工作流用户动作中已存在相同的关键字{0}数据"
        /// {0}用户关键字
        /// </summary>
        public const string ExistWorkflowStepUserActionByUserKey = "ExistWorkflowStepUserActionByUserKey";
        /// <summary>
        /// 调度动作中已存在相同的名称数据
        /// 格式为"调度动作中存在相同的名称{0}数据"
        /// {0}名称
        /// </summary>
        public const string ExistScheduleActionByName = "ExistScheduleActionByName";
        /// <summary>
        /// 短信发送终结点中存在相同的名称数据
        /// 格式为"短信发送终结点中存在相同的名称{0}数据"
        /// {0}名称
        /// </summary>
        public const string ExistSMSSendEndpointByName = "ExistSMSSendEndpointByName";
        /// <summary>
        /// 白名单中存在相同的系统名称数据
        /// 格式为"白名单中存在相同的系统名称{0}数据"
        /// {0}系统名称
        /// </summary>
        public const string ExistWhitelistBySystemName = "ExistWhitelistBySystemName";
        /// <summary>
        /// 客户端白名单中存在相同的系统名称数据
        /// 格式为"客户端白名单中存在相同的系统名称{0}数据"
        /// {0}系统名称
        /// </summary>
        public const string ExistClientWhitelistBySystemName = "ExistClientWhitelistBySystemName";
        /// <summary>
        /// 系统操作和白名单关联关系中存在相同的系统操作ID和白名单ID数据
        /// 格式为"系统操作和白名单关联关系中存在相同的系统操作ID{0}和白名单ID{1}数据"
        /// {0}系统操作ID
        /// {1}白名单ID
        /// </summary>
        public const string ExistSystemOperationWhitelistRelationByID = "ExistSystemOperationWhitelistRelationByID";
        /// <summary>
        /// 系统操作中存在相同的名称数据
        /// 格式为"系统操作中存在相同的名称{0}数据"
        /// {0}名称
        /// </summary>
        public const string ExistSystemOperationByName = "ExistSystemOperationByName";
        /// <summary>
        /// 客户端消息类型监听终结点中存在相同的名称数据
        /// 格式为"客户端消息类型监听终结点中存在相同的名称{0}数据"
        /// {0}名称
        /// </summary>
        public const string ExistClientSMessageTypeListenerEndpointByName = "ExistClientSMessageTypeListenerEndpointByName";
        /// <summary>
        /// 客户端系统登陆终结点中存在相同的名称数据
        /// 格式为"客户端系统登陆终结点中存在相同的名称{0}数据"
        /// {0}名称
        /// </summary>
        public const string ExistClientSystemLoginEndpointByName = "ExistClientSystemLoginEndpointByName";
        /// <summary>
        /// 验证终结点数据中存在相同的名称数据
        /// 格式为"验证终结点数据中存在相同的名称{0}数据"
        /// {0}名称
        /// </summary>
        public const string ExistAuthorizationEndpointByName = "ExistAuthorizationEndpointByName";
        /// <summary>
        /// 工作流资源已存在相同关键字(type+key)数据
        /// 格式为"工作流资源已存在相同关键字{0}数据"
        /// {0}关键字
        /// </summary>
        public const string ExistWorkflowResourceKey = "ExistWorkflowResourceKey";
        /// <summary>
        /// 工作流步骤已存在相同关键字(resourceid+actionname+status+usertype+userkey)数据
        /// 格式为"工作流步骤已存在相同关键字{0}数据"
        /// {0}关键字
        /// </summary>
        public const string ExistWorkflowStepKey = "ExistWorkflowStepKey";
        /// <summary>
        /// 调度作业组已存在相同名称数据
        /// 格式为"调度作业组已存在相同名称{0}数据"
        /// {0}名称
        /// </summary>
        public const string ExistScheduleActionGroupName = "ExistScheduleActionGroupName";
        /// <summary>
        /// 短信模板已存在相同名称数据
        /// 格式为"短信模板已存在相同名称{0}数据"
        /// {0}名称
        /// </summary>
        public const string ExistSMSTemplateStoreByName = "ExistSMSTemplateStoreByName";
        /// <summary>
        /// Json反序列化失败
        /// 格式为“类型{0}反序列化失败，要反序列化的字符串为{1}，发生位置{2}”
        /// {0}为反序列化的类型
        /// {1}用于反序列化的字符串
        /// {2}发生错误的位置
        /// </summary>
        public const string JsonDeserializeError = "JsonDeserializeError";
        /// <summary>
        /// ServiceFabric客户端终结点仓储已存在相同名称数据
        /// 格式为"ServiceFabric客户端终结点仓储已存在相同名称{0}数据"
        /// {0}名称
        /// </summary>
        public const string ExistServiceFabricClientEndpointStoreByName = "ExistServiceFabricClientEndpointStoreByName";
        /// <summary>
        /// 找不到指定key的工作流资源
        /// 格式为“找不到Type为{0}，Key为{1}的工作流资源”
        /// {0}为工作流资源类型
        /// {1}为工作流资源关键字
        /// </summary>
        public const string NotFoundWorkflowResourceByKey = "NotFoundWorkflowResourceByKey";
        /// <summary>
        /// 指定动作和状态工作流步骤未完成
        /// 格式为“指定资源Type为{0}，Key为{1}的步骤ActionName为{2}，Status为{3}未完成”
        /// {0}为工作流资源类型
        /// {1}为工作流资源关键字
        /// {2}为步骤动作
        /// {3}为步骤状态
        /// </summary>
        public const string ExistWorkflowResourceStepByActionName = "ExistWorkflowResourceStepByActionName";
        /// <summary>
        /// 在指定的哈希节点的关键信息中，找不到指定的键值
        /// 格式为“哈希组{0}中的哈希节点关键信息中找不到键值{1}”
        /// {0}为哈希组名称
        /// {1}为关键信息中的键值
        /// </summary>
        public const string NotFoundKeyInHashNodeKeyInfo = "NotFoundKeyInHashNodeKeyInfo";
        /// <summary>
        /// 在哈希数据迁移服务工厂键值对中找不到指定存储类型的键
        /// 格式为“在哈希数据迁移服务工厂键值对组中找不到存储类型为{0}的键”
        /// {0}为哈希组名称
        /// </summary>
        public const string NotFoundHashDataMigrateServiceFactoryDictionaryByStoreType = "NotFoundHashDataMigrateServiceFactoryDictionaryByStoreType";
        /// <summary>
        /// 在哈希数据迁移服务工厂键值对中找不到指定策略名称的键
        /// 格式为“在哈希数据迁移服务工厂键值对中找不到策略名称为{0}的键”
        /// {0}为哈希策略名称
        /// </summary>
        public const string NotFoundHashDataMigrateServiceFactoryByStrategyName = "NotFoundHashDataMigrateServiceFactoryByStrategyName";
        /// <summary>
        /// 指定哈希组执行数据迁移时发生错误
        /// 格式为“哈希组{0}执行数据迁移时发生错误，错误详细信息为{1}”
        /// {0}为哈希组名称
        /// {1}为错误详细信息
        /// </summary>
        public const string HashDataMigrateErrorByGroup = "HashDataMigrateErrorByGroup";
        /// <summary>
        /// 指定哈希组执行整体数据迁移时发生错误
        /// 格式为“哈希组{0}执行整体数据迁移时发生错误，错误详细信息为{1}”
        /// {0}为哈希组名称
        /// {1}为错误详细信息
        /// </summary>
        public const string HashDataTotalMigrateErrorByGroup = "HashDataTotalMigrateErrorByGroup";
        /// <summary>
        /// 消息历史监听明细中存在相同的名称数据
        /// 格式为"消息历史监听明细中存在相同的名称{0}数据"
        /// {0}名称
        /// </summary>
        public const string ExistSMessageHistoryDetailByName = "ExistSMessageHistoryDetailByName";
        /// <summary>
        /// 将要转移的数据增加到目标真实节点时发生错误
        /// 格式为“将要转移的数据增加到目标真实节点时发生错误，找不到目标数据库节点：{0}”
        /// {0}为目标数据库节点
        /// </summary>
        public const string NotFoundHashDataMigrateTargetDB = "NotFoundHashDataMigrateTargetDB";
        /// <summary>
        /// 删除目标节点数据时发生错误
        /// 格式为“删除目标节点数据时发生错误，找不到目标数据库节点：{0}”
        /// {0}为目标数据库节点
        /// </summary>
        public const string NotFoundDeleteTargetDB = "NotFoundDeleteTargetDB";
        /// <summary>
        /// 当前请求已被锁定
        /// 格式为“当前请求{0}已被锁定”
        /// {0}为请求
        /// </summary>
        public const string ExistLicationLock = "ExistLicationLock";
        /// <summary>
        /// 找不到指定实体类型的EntityRepositoryService
        /// 格式为“实体类型为{0}的EntityRepositoryService找不到，位置为{1}”
        /// {0}为实体类型
        /// {1}为发生错位的位置信息
        /// </summary>
        public const string NotFoundEntityRepositoryServiceByEntityType = "NotFoundEntityRepositoryServiceByEntityType";
        /// <summary>
        /// 找不到指定实体类型和实体关键字的实体记录
        /// 格式为“找不到实体类型为{0}，实体关键字为{1}的实体记录”
        /// {0}为实体类型
        /// {1}为实体关键字
        /// </summary>
        public const string NotFoundEntityByEntityTypeAndKey = "NotFoundEntityByEntityTypeAndKey";
        /// <summary>
        /// 时间转换格式标签
        /// </summary>
        public const string LabelCNDateTime = "LabelCNDateTime";
        /// <summary>
        /// 找不到工作流中需要审核的步骤;查询条件为资源类型:{0},资源关键字:{1},资源Id:{2},动作名称:{3},资源状态:{4}
        /// </summary>
        public const string NotFoundWorkflowStepForAudit = "NotFoundWorkflowStepForAudit";
        /// <summary>
        /// 实体属性值无法到达，因为中间出现null值
        /// 格式为“实体类型为{0}、实体关键字为{1}的实体记录，无法获取属性{2}的值，因为当执行到属性{3}时，值为null”
        /// {0}：实体类型
        /// {1}：实体关键字
        /// {2}：要获取的属性链
        /// {3}：出现null值的属性链
        /// </summary>
        public const string EntityAttributeCanNotArrive = "EntityAttributeCanNotArrive";
        /// <summary>
        /// 实体属性链中的值必须基于ModelBase
        /// 格式为“实体类型为{0}、实体关键字为{1}的实体记录，属性{2}的值的类型不是基于ModelBase，实际类型为{3}”
        /// {0}：实体类型
        /// {1}：实体关键字
        /// {2}：出问题的属性链
        /// {3}：属性的值类型
        /// </summary>
        public const string EntityAttributeNotModelBase = "EntityAttributeNotModelBase";
        /// <summary>
        /// 找不到指定操作符的EntityAttributeValueValidateService
        /// 格式为“找不到操作符为{0}的EntityAttributeValueValidateService，位置为{1}”
        /// {0}：操作符
        /// {1}：出错的位置
        /// </summary>
        public const string NotFoundEntityAttributeValueValidateServiceByOperator = "NotFoundEntityAttributeValueValidateServiceByOperator";
        /// <summary>
        /// 实体属性值验证操作中针对相等运算符验证正确的文本
        /// 格式为“实体类型为{0}、实体关键字为{1}的实体记录的属性{2}的值等于{3}”
        /// {0}：实体类型
        /// {1}：实体关键字
        /// {2}：实体的属性名称
        /// {3}：比较的值
        /// </summary>
        public const string EntityAttributeValueValidateOperatorEqualCorrectText = "EntityAttributeValueValidateOperatorEqualCorrectText";

        /// <summary>
        /// 实体属性值验证操作中针对相等运算符验证错误的文本
        /// 格式为“实体类型为{0}、实体关键字为{1}的实体记录的属性{2}的值不等于{3}”
        /// {0}：实体类型
        /// {1}：实体关键字
        /// {2}：实体的属性名称
        /// {3}：比较的值
        /// </summary>
        public const string EntityAttributeValueValidateOperatorEqualErrorText = "EntityAttributeValueValidateOperatorEqualErrorText";



        /// <summary>
        /// 实体属性值验证操作中针对大于运算符验证正确的文本
        /// 格式为“实体类型为{0}、实体关键字为{1}的实体记录的属性{2}的值大于{3}”
        /// {0}：实体类型
        /// {1}：实体关键字
        /// {2}：实体的属性名称
        /// {3}：比较的值
        /// </summary>
        public const string EntityAttributeValueValidateOperatorGreaterThanCorrectText = "EntityAttributeValueValidateOperatorGreaterThanCorrectText";

        /// <summary>
        /// 实体属性值验证操作中针对大于运算符验证错误的文本
        /// 格式为“实体类型为{0}、实体关键字为{1}的实体记录的属性{2}的值小于等于{3}”
        /// {0}：实体类型
        /// {1}：实体关键字
        /// {2}：实体的属性名称
        /// {3}：比较的值
        /// </summary>
        public const string EntityAttributeValueValidateOperatorGreaterThanErrorText = "EntityAttributeValueValidateOperatorGreaterThanErrorText";


        /// <summary>
        /// 实体属性值验证操作中针对大于等于运算符验证正确的文本
        /// 格式为“实体类型为{0}、实体关键字为{1}的实体记录的属性{2}的值大于等于{3}”
        /// {0}：实体类型
        /// {1}：实体关键字
        /// {2}：实体的属性名称
        /// {3}：比较的值
        /// </summary>
        public const string EntityAttributeValueValidateOperatorGreaterEqualCorrectText = "EntityAttributeValueValidateOperatorGreaterEqualCorrectText";

        /// <summary>
        /// 实体属性值验证操作中针对大于等于运算符验证错误的文本
        /// 格式为“实体类型为{0}、实体关键字为{1}的实体记录的属性{2}的值小于{3}”
        /// {0}：实体类型
        /// {1}：实体关键字
        /// {2}：实体的属性名称
        /// {3}：比较的值
        /// </summary>
        public const string EntityAttributeValueValidateOperatorGreaterEqualErrorText = "EntityAttributeValueValidateOperatorGreaterEqualErrorText";







        /// <summary>
        /// 实体属性值验证操作中针对小于运算符验证正确的文本
        /// 格式为“实体类型为{0}、实体关键字为{1}的实体记录的属性{2}的值小于{3}”
        /// {0}：实体类型
        /// {1}：实体关键字
        /// {2}：实体的属性名称
        /// {3}：比较的值
        /// </summary>
        public const string EntityAttributeValueValidateOperatorLessThanCorrectText = "EntityAttributeValueValidateOperatorLessThanCorrectText";

        /// <summary>
        /// 实体属性值验证操作中针对小于运算符验证错误的文本
        /// 格式为“实体类型为{0}、实体关键字为{1}的实体记录的属性{2}的值大于等于{3}”
        /// {0}：实体类型
        /// {1}：实体关键字
        /// {2}：实体的属性名称
        /// {3}：比较的值
        /// </summary>
        public const string EntityAttributeValueValidateOperatorLessThanErrorText = "EntityAttributeValueValidateOperatorLessThanErrorText";


        /// <summary>
        /// 实体属性值验证操作中针对小于等于运算符验证正确的文本
        /// 格式为“实体类型为{0}、实体关键字为{1}的实体记录的属性{2}的值小于等于{3}”
        /// {0}：实体类型
        /// {1}：实体关键字
        /// {2}：实体的属性名称
        /// {3}：比较的值
        /// </summary>
        public const string EntityAttributeValueValidateOperatorLessEqualCorrectText = "EntityAttributeValueValidateOperatorLessEqualCorrectText";

        /// <summary>
        /// 实体属性值验证操作中针对小于等于运算符验证错误的文本
        /// 格式为“实体类型为{0}、实体关键字为{1}的实体记录的属性{2}的值大于{3}”
        /// {0}：实体类型
        /// {1}：实体关键字
        /// {2}：实体的属性名称
        /// {3}：比较的值
        /// </summary>
        public const string EntityAttributeValueValidateOperatorLessEqualErrorText = "EntityAttributeValueValidateOperatorLessEqualErrorText";







        /// <summary>
        /// 实体属性值验证操作中针对null运算符验证正确的文本
        /// 格式为“实体类型为{0}、实体关键字为{1}的实体记录的属性{2}的值为Null”
        /// {0}：实体类型
        /// {1}：实体关键字
        /// {2}：实体的属性名称
        /// </summary>
        public const string EntityAttributeValueValidateOperatorNullCorrectText = "EntityAttributeValueValidateOperatorNullCorrectText";

        /// <summary>
        /// 实体属性值验证操作中针对null运算符验证错误的文本
        /// 格式为“实体类型为{0}、实体关键字为{1}的实体记录的属性{2}的值不为Null”
        /// {0}：实体类型
        /// {1}：实体关键字
        /// {2}：实体的属性名称
        /// </summary>
        public const string EntityAttributeValueValidateOperatorNullErrorText = "EntityAttributeValueValidateOperatorNullErrorText";


        /// <summary>
        /// 实体属性值验证操作中针对NotNull验证正确的文本
        /// 格式为“实体类型为{0}、实体关键字为{1}的实体记录的属性{2}的值不为Null”
        /// {0}：实体类型
        /// {1}：实体关键字
        /// {2}：实体的属性名称
        /// </summary>
        public const string EntityAttributeValueValidateOperatorNotNullCorrectText = "EntityAttributeValueValidateOperatorNotNullCorrectText";

        /// <summary>
        /// 实体属性值验证操作中针对小于等于运算符验证错误的文本
        /// 格式为“实体类型为{0}、实体关键字为{1}的实体记录的属性{2}的值为Null”
        /// {0}：实体类型
        /// {1}：实体关键字
        /// {2}：实体的属性名称
        /// </summary>
        public const string EntityAttributeValueValidateOperatorNotNullErrorText = "EntityAttributeValueValidateOperatorNotNullErrorText";






        /// <summary>
        /// 实体属性值的类型与指定的类型不匹配
        /// 格式为“实体类型为{0}、实体关键字为{1}的实体记录的属性{2}的值类型与配置的valuetype不匹配，valuetype设置的类型为{3}，实际的值类型为{4}”
        /// {0}：实体类型
        /// {1}：实体关键字
        /// {2}：实体的属性名称
        /// {3}：配置的valuetype
        /// {4}：实际的值类型
        /// </summary>
        public const string EntityAttributeValueTypeNotMatch = "EntityAttributeValueTypeNotMatch";
        /// <summary>
        /// 要用来和实体属性的值相比较的值的类型不匹配
        /// 格式为“要和实体类型为{0}、实体关键字为{1}的实体记录的属性{2}的值相比较的配置值与配置的valuetype不匹配，valuetype设置的类型为{3}，配置值为{4}”
        /// {0}：实体类型
        /// {1}：实体关键字
        /// {2}：实体的属性名称
        /// {3}：配置的valuetype
        /// {4}：配置值
        /// </summary>
        public const string EntityAttributeCheckValueTypeNotMatch = "EntityAttributeCheckValueTypeNotMatch";
        /// <summary>
        ///  指定实体属性的值类型未定义
        ///  格式为“值类型{0}未定义，必须在MSLibrary.EntityAttributeValueTypes中定义”
        ///  {0}为值类型
        /// </summary>
        public const string NotFoundEntityAttributeValueType = "NotFoundEntityAttributeValueType";
        /// <summary>
        /// 指定实体属性的值类型与操作不匹配
        /// 格式为“值类型{0}与操作{1}不匹配”
        /// {0}为值类型
        /// {1}为操作
        /// </summary>
        public const string EntityAttributeValueTypeAndOeratorNotMatch = "EntityAttributeValueTypeAndOeratorNotMatch";
        /// <summary>
        /// 找不到指定类型的第三方系统服务
        /// 格式为“找不到类型为{0}的第三方系统服务，发生位置：{1}”
        /// {0}：第三方系统服务类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundThirdPartySystemServiceByType = "NotFoundThirdPartySystemServiceByType";
        /// <summary>
        /// 找不到指定类型的第三方系统后续处理服务
        /// 格式为“找不到类型为{0}的第三方系统后续处理服务，发生位置：{1}”
        /// {0}：第三方系统后续处理服务类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundThirdPartySystemPostExecuteServiceByType = "NotFoundThirdPartySystemPostExecuteServiceByType";

        /// <summary>
        /// 在第三方令牌及后续处理中获取的键值对中，找不到指定键的值
        /// 格式为“在登录终结点{0}、验证终结点{1}的第三方及后续处理中获取的令牌键值对中，找不到键为{2}的值”
        /// {0}：登录终结点名称
        /// {1}：验证终结点名称
        /// {2}：键名称
        /// </summary>
        public const string NotFoundUserInfoKeyInThirdPartyTokenAttributes = "NotFoundUserInfoKeyInThirdPartyTokenAttributes";
        /// <summary>
        /// 存在相同的第三方系统令牌记录
        /// 格式为“已经存在登录终结点{0}、验证终结点{1}、用户关键字{2}的第三方系统令牌记录” 
        /// {0}：登录终结点ID
        /// {1}：验证终结点ID
        /// {2}：用户关键字
        /// </summary>
        public const string ExistSameThirdPartySystemTokenRecord = "ExistSameThirdPartySystemTokenRecord";
        /// <summary>
        /// 指定的条件元素执行发生错误
        /// 格式为“条件元素{0}执行发生错误，详细信息：{1}”
        /// {0}：条件完整的xml元素
        /// {1}：错误的详细信息
        /// </summary>
        public const string ConditionElementError = "ConditionElementError";
        /// <summary>
        ///  消息类型监听器与消息类型不匹配
        ///  格式为“消息类型监听器类型{0}期待的消息类型为{1}，但实际消息类型为{2}”
        ///  {0}：消息监听类型的类型
        ///  {1}：该监听器可以处理的消息类型
        ///  {2}：实际的消息类型
        /// </summary>
        public const string SMessageTypeListenerNotMatchSMessageType = "SMessageTypeListenerNotMatchSMessageType";
        /// <summary>
        /// 消息类型监听器与消息体不匹配
        /// 格式为“消息类型监听器类型{0}期待的消息体类型为{1}，但实际消息体为{2}”
        /// {0}：消息监听类型的类型
        /// {1}：该监听器可以处理的消息体类型
        /// {2}：实际的消息体内容
        /// </summary>
        public const string SMessageTypeListenerNotMatchSMessageBody = "SMessageTypeListenerNotMatchSMessageBody";

        /// <summary>
        /// 找不到指定备用关键字名称的备用关键字
        /// 格式为“实体类型为{0}的实体元数据下，找不到备用关键字名称为{1}的备用关键字”
        /// {0}：实体元数据的类型
        /// {1}：备用关键字的名称
        /// </summary>
        public const string NotFoundEntityInfoAlternateKeyByName = "NotFoundEntityInfoAlternateKeyByName";
        /// <summary>
        /// 在指定被用关键字下面找不到关联关系
        /// 格式为“实体类型为{0}的实体元数据下，备用关键字名称为{1}的备用关键字下，找不到任何备用关键字关联关系”
        /// {0}：实体元数据的类型
        /// {1}：备用关键字的名称
        /// </summary>
        public const string NotFoundEntityInfoAlternateKeyRelation = "NotFoundEntityInfoAlternateKeyRelation";

        /// <summary>
        /// 找不到指定属性类型的实体属性值转换成关键字字符串服务
        /// 格式为“找不到针对实体属性类型为{0}的实体属性值转换成关键字字符串服务”
        /// {0}：实体属性类型
        /// </summary>
        public const string NotFoundEntityAttributeValueKeyConvertServiceByAttributeType = "NotFoundEntityAttributeValueKeyConvertServiceByAttributeType";
        /// <summary>
        /// 实体属性元数据的值类型与实际的类型不匹配
        /// 格式为“实体类型为{0}的实体元数据的属性{1}的值类型为{2}，但实际值类型为{3}，两者不匹配，发生位置为{4}”
        /// {0}：实体元数据的类型
        /// {1}：实体属性名称
        /// {2}：实体属性的值类型
        /// {3}：实体属性的实际类型
        /// {4}：发生错误的位置 
        /// </summary>
        public const string EntityAttributeMetadataValueTypeNotMatchActual = "EntityAttributeMetadataValueTypeNotMatchActual";
        /// <summary>
        /// 实体属性元数据的值类型与实体属性值转换成关键字字符串服务期望的不一致
        /// 格式为“实体类型为{0}的实体元数据的属性{1}的值的实际类型为{2}，但实体属性值转换成关键字字符串服务{3}期待的类型为{4}，两者不匹配，发生位置为{5}”
        /// {0}：实体元数据的类型
        /// {1}：实体属性名称
        /// {2}：实体属性的值的实际类型
        /// {3}：实体属性值转换成关键字字符串服务的类型
        /// {4}：实体属性值转换成关键字字符串服务期待的类型
        /// {5}：发生错误的位置 
        /// </summary>
        public const string EntityAttributeMetadataValueTypeNotMatchEntityAttributeValueKeyConvertService = "EntityAttributeMetadataValueTypeNotMatchEntityAttributeValueKeyConvertService";
        /// <summary>
        /// 找不到主关键字的任何关联关系
        /// 格式为“实体类型为{0}的实体元数据下，找不到任何主关键字关联关系”
        /// {0}：实体元数据的类型
        /// </summary>
        public const string NotFoundEntityInfoKeyRelation = "NotFoundEntityInfoKeyRelation";
        /// <summary>
        /// 实体关键字的值要求非空
        /// 格式为“实体类型为{0}的实体元数据的属性{1}的值类型{2}要求转换的字符串不为空,发生位置为{3}”
        /// {0}：实体元数据的类型
        /// {1}：实体属性名称
        /// {2}：实体属性的值类型
        /// {3}：发生错误的位置 
        /// </summary>
        public const string EntityKeyValueRequireNotNull = "EntityKeyValueRequireNotNull";
        /// <summary>
        /// 实体属性元数据的值类型与实体属性值转换成关键字字符串服务要求的不一致
        /// 格式为“实体类型为{0}的实体元数据的属性{1}的值类型为{2}，但实体属性值转换成关键字字符串服务{3}期待的类型为{4}，两者不匹配,发生位置为{5}”
        /// {0}：实体元数据的类型
        /// {1}：实体属性名称
        /// {2}：实体属性的值类型
        /// {3}：实体属性值转换成关键字字符串服务的类型
        /// {4}：实体属性值转换成关键字字符串服务期待的类型
        /// {5}：发生错误的位置 
        /// </summary>
        public const string EntityAttributeMetadataValueTypeNotMatchEntityAttributeValueKeyConvertServiceRequire = "EntityAttributeMetadataValueTypeNotMatchEntityAttributeValueKeyConvertServiceRequire";
        /// <summary>
        /// 实体关键字的值类型与实体属性元数据的值类型不匹配
        /// 格式为“实体类型为{0}的实体元数据的属性{1}的值类型为{2}，但实体关键字{3}无法转换成该值类型,发生位置为{4}”
        /// {0}：实体元数据的类型
        /// {1}：实体属性名称
        /// {2}：实体属性的值类型
        /// {3}：实体关键字
        /// {4}：发生错误的位置
        /// </summary>
        public const string EntityKeyValueNotMatchEntityAttributeMetadataValueType = "EntityKeyValueNotMatchEntityAttributeMetadataValueType";

        /// <summary>
        /// 实体关键字分解的数量与备用关键字属性的数量不一致
        /// 格式为“实体类型为{0}的实体元数据的备用关键字{1}所包含的属性数量为{2}，但实体关键字{3}数量为{4}，两者不一致,发生位置为{5}”
        /// {0}：实体元数据的类型
        /// {1}：备用关键字的名称
        /// {2}：备用关键字包含的属性数量
        /// {3}：实体关键字字符串
        /// {4}：实体关键字解析成数组的长度
        /// {5}：发生错误的位置
        /// </summary>
        public const string EntityKeyValueCountNotEqualAlternateKeyAtributeCount = "EntityKeyValueCountNotEqualAlternateKeyAtributeCount";
        /// <summary>
        /// 实体关键字分解的数量与主关键字属性的数量不一致
        /// 格式为“实体类型为{0}的实体元数据的主关键字所包含的属性数量为{1}，但实体关键字{2}数量为{3}，两者不一致,发生位置为{4}”
        /// {0}：实体元数据的类型
        /// {1}：主关键字包含的属性数量
        /// {2}：实体关键字字符串
        /// {3}：实体关键字解析成数组的长度
        /// {4}：发生错误的位置
        /// </summary>
        public const string EntityKeyValueCountNotEqualKeyAtributeCount = "EntityKeyValueCountNotEqualKeyAtributeCount";
        /// <summary>
        /// 找不到指定名称的序列号生成配置
        /// 格式为“找不到名称为{0}的序列号生成配置”
        /// {0}：配置名称
        /// </summary>
        public const string NotFoundSerialNumberGeneratorConfigurationByName = "NotFoundSerialNumberGeneratorConfigurationByName";
        /// <summary>
        /// 找不到指定实体类型的实体元数据
        /// 格式为“找不到实体类型为{0}的实体元数据”
        /// {0}：实体类型
        /// </summary>
        public const string NotFoundEntityInfoByEntityType = "NotFoundEntityInfoByEntityType";
        /// <summary>
        /// 找不到指定名称的通用审批配置节点获取待处理用户服务
        /// 格式为“找不到名称为{0}的通用审批配置节点获取待处理用户服务，位置为{1}”
        /// {0}：服务名称
        /// {1}：发生位置
        /// </summary>
        public const string NotFoundCommonSignConfigurationNodeGetExecuteUserServiceByName = "NotFoundCommonSignConfigurationNodeGetExecuteUserServiceByName";
        /// <summary>
        /// 找不到指定审批类型的审批类型获取通用审批配置节点审批后处理服务
        /// 格式为“找不到审批类型为{0}的审批类型获取通用审批配置节点审批后处理服务，位置为{1}”
        /// {0}：审批类型
        /// {1}：发生位置
        /// </summary>
        public const string NotFoundCommonSignConfigurationNodeSignExtensionServiceBySignType = "NotFoundCommonSignConfigurationNodeSignExtensionServiceBySignType";
        /// <summary>
        /// 找不到指定名称的通用审批配置入口服务
        /// 格式为“找不到名称为{0}的通用审批配置入口服务，位置为{1}”
        /// {0}：服务名称
        /// {1}：发生位置
        /// </summary>
        public const string NotFoundCommonSignConfigurationEntryServiceByName = "NotFoundCommonSignConfigurationEntryServiceByName";
        /// <summary>
        /// 找不到指定实体类型的工作流资源关键字与实体关键字转换服务
        /// 格式为“找不到实体类型为{0}的工作流资源关键字与实体关键字转换服务，位置为{1}”
        /// {0}：服务名称
        /// {1}：发生位置
        /// </summary>
        public const string NotFoundWorkflowResourceKeyEntityKeyConvertServiceByEntityType = "NotFoundWorkflowResourceKeyEntityKeyConvertServiceByEntityType";
        /// <summary>
        /// 在指定的通用审批配置中未设置入口阶段
        /// 格式为“工作流资源类型为{0}的通用审批配置中未设置入口阶段”
        /// {0}：工作流资源类型
        /// </summary>
        public const string NotSetEntryStageInCommonSignConfiguration = "NotSetEntryStageInCommonSignConfiguration";
        /// <summary>
        /// 在指定的通用审批配置阶段中找不到任何指定状态的节点
        /// 格式为“工作流资源类型为{0}的通用审批配置中的阶段{1}中，找不到任何指定状态为{2}的节点”
        /// {0}：工作流资源类型
        /// {1}：阶段名称
        /// {2}：节点状态
        /// </summary>
        public const string NotFoundNodeInCommonSignConfigurationStageByStatus = "NotFoundNodeInCommonSignConfigurationStageByStatus";
        /// <summary>
        /// 找不到指定名称的通用审批完成后处理服务
        /// 格式为“找不到名称为{0}的通用审批完成后处理服务，位置为{1}”
        /// {0}：处理服务名称
        /// {1}：发生位置
        /// </summary>
        public const string NotFoundCommonSignConfigurationCompleteServiceByName = "NotFoundCommonSignConfigurationCompleteServiceByName";
        /// <summary>
        /// 找不到指定名称的通用审批配置入口节点寻找服务
        /// 格式为“找不到名称为{0}的通用审批配置入口节点寻找服务，位置为{1}”
        /// {0}：寻找服务名称
        /// {1}：发生位置
        /// </summary>
        public const string NotFoundCommonSignConfigurationEntryNodeFindServiceByName = "NotFoundCommonSignConfigurationEntryNodeFindServiceByName";
        /// <summary>
        /// 通用审批配置r入口节点状态错误
        /// 格式为“工作流资源类型为{0}的通用审批配置入口节点{1}的状态为{2}，但要求的状态为{3}”
        /// {0}：工作流资源类型
        /// {1}：节点名称
        /// {2}：节点的状态
        /// {3}：要求的状态
        /// </summary>
        public const string CommonSignConfigurationEntryNodeStatusError = "CommonSignConfigurationEntryNodeStatusError";
        /// <summary>
        ///  通用审批配置节点与流程下一个节点不是同一个配置
        ///  格式为“工作流资源类型为{0}的通用审批配置下的节点{1}和流转到的下一个节点{2}不属于同一个配置，下一个节点属于工作流资源类型为{3}的通用审批配置”
        ///  {0}：工作流资源类型
        ///  {1}：节点名称
        ///  {2}：下一个节点名称
        ///  {3}：下一个节点所属配置的工作流资源类型
        /// </summary>
        public const string CommonSignConfigurationNodeNextNodeNotSameConfiguration = "CommonSignConfigurationNodeNextNodeNotSameConfiguration";
        /// <summary>
        /// 通用审批配置入口节点的所属配置与当前配置不一致
        /// 格式为“工作流资源类型为{0}的通用审批配置的入口节点{1}的所属配置为工作流资源类型为{2}的通用审批配置，两者不一致”
        ///  {0}：工作流资源类型
        ///  {1}：入口节点名称
        ///  {2}：入口节点所属配置的工作流资源类型
        /// </summary>
        public const string CommonSignConfigurationEntryNodeNotSameConfiguration = "CommonSignConfigurationEntryNodeNotSameConfiguration";
        /// <summary>
        /// 在指定的通用审批配置中，找不到指定名称的节点
        /// 格式为“工作流资源类型为{0}的通用审批配置中找不到名称为{1}的节点”
        ///  {0}：工作流资源类型
        ///  {1}：节点名称
        /// </summary>
        public const string NotFoundCommonSignConfigurationNodeByName = "NotFoundCommonSignConfigurationNodeByName";
        /// <summary>
        /// 通用审批配置下一节点状态错误
        /// 格式为“工作流资源类型为{0}的通用审批配置中名称为{1}的节点的下一个流转节点{2}的状态为{3}，但要求的状态为{4}”
        ///  {0}：工作流资源类型
        ///  {1}：节点名称
        ///  {2}：下一节点名称
        ///  {3}：下一节点状态
        ///  {4}：要求的状态
        /// </summary>
        public const string CommonSignConfigurationNextNodeStatusError = "CommonSignConfigurationNextNodeStatusError";
        /// <summary>
        /// 在指定的通用审批配置中,入口操作不接受指定的动作
        /// 格式为“工作流资源类型为{0}的通用审批配置中，入口操作不接受动作名称为{1}的动作，请检查通用审批配置的AcceptNames属性”
        /// {0}：工作流资源类型
        /// {1}：动作名称
        /// </summary>
        public const string CommonSignConfigurationEntryNotAcceptActionName = "CommonSignConfigurationEntryNotAcceptActionName";

        /// <summary>
        /// 找不到指定名称的通用审批配置节点创建流程处理服务
        /// 格式为“找不到名称为{0}的通用审批配置节点创建流程处理服务，位置为{1}”
        /// {0}：服务名称
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundCommonSignConfigurationNodeCreateFlowExecuteServiceByName = "NotFoundCommonSignConfigurationNodeCreateFlowExecuteServiceByName";
        /// <summary>
        /// 找不到指定动作名称的通用审批配置初始化动作
        /// 格式为“工作流资源类型为{0}的通用审批配置下，找不到动作名称为{1}的初始化动作”
        /// {0}：工作流资源类型
        /// {1}：动作名称
        /// </summary>
        public const string NotFoundCommonSignConfigurationRootActionByActionName = "NotFoundCommonSignConfigurationRootActionByActionName";
        /// <summary>
        /// 找不到指定动作名称的通用审批配置节点动作
        /// 格式为“工作流资源类型为{0}的通用审批配置下的节点{1}中，找不到动作名称为{2}的节点动作”
        /// {0}：工作流资源类型
        /// {1}：节点名称
        /// {2}：动作名称
        /// </summary>
        public const string NotFoundCommonSignConfigurationNodeActionByActionName = "NotFoundCommonSignConfigurationNodeActionByActionName";
        /// <summary>
        /// 找不到指定名称的获取可以进行入口操作的用户列表服务
        /// 格式为“找不到名称为{0}的获取可以进行入口操作的用户列表服务，位置为{1}”
        /// {0}：服务名称
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundCommonSignConfigurationEntryGetExecuteUsersServiceByName = "NotFoundCommonSignConfigurationEntryGetExecuteUsersServiceByName";
        /// <summary>
        /// 指定的用户不能执行指定的通用审批配置初始化动作的入口方法
        /// 格式为“工作流资源类型为{0}的通用审批配置下动作名称为{1}的初始化动作，用户{2}不能执行,UserKeys:{3}”
        /// {0}：工作流资源类型
        /// {1}：初始化动作的动作名称
        /// {2}：执行用户的关键字
        /// {3]:要比较的用户关键字
        /// </summary>
        public const string CommonSignConfigurationRootActionUserCanNotEntry = "CommonSignConfigurationRootActionUserCanNotEntry";
        /// <summary>
        ///  在指定名称的通用审批配置节点动作中，需要外部传入用户，但没有传入
        ///  格式为“工作流资源类型为{0}的通用审批配置下名称为{1}的节点下的动作名称为{2}的节点动作需要外部传入用户，但没有传入”
        /// {0}：工作流资源类型
        /// {1}：节点名称
        /// {2}：节点动作名称
        /// </summary>
        public const string CommonSignConfigurationNodeActionManualUserEmpty = "CommonSignConfigurationNodeActionManualUserEmpty";
        /// <summary>
        /// 通用审批配置节点中下一节点接收的用户验证失败
        /// 格式为“工作流资源类型为{0}的通用审批配置下名称为{1}的节点下的动作名称为{2}的节点动作中，下一节点接收的用户验证失败，失败原因{3}”
        /// {0}：工作流资源类型
        /// {1}：节点名称
        /// {2}：节点动作名称
        /// {3}：错误原因
        /// </summary>
        public const string CommonSignConfigurationNodeNextNodeManaulUserServiceValidateError = "CommonSignConfigurationNodeNextNodeManaulUserServiceValidateError";
        /// <summary>
        /// 找不到指定审批类型的通用审批配置节点动作转换审批类型配置对象服务
        /// 格式为“找不到审批类型为{0}的通用审批配置节点动作转换审批类型配置对象服务，位置为{1}”
        /// {0}：审批类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundCommonSignConfigurationNodeActionSignTypeConfigurationConvertServiceBySignType = "NotFoundCommonSignConfigurationNodeActionSignTypeConfigurationConvertServiceBySignType";
        /// <summary>
        /// 找不到指定工作流资源类型的通用审批配置
        /// 格式为“找不到工作流资源类型为{0}的通用审批配置”
        /// {0}：工作流资源类型
        /// </summary>
        public const string NotFoundCommonSignConfigurationByWorkflowResourceType = "NotFoundCommonSignConfigurationByWorkflowResourceType";
        /// <summary>
        /// 在指定的通用审批配置中，指定名称的节点的状态不正确
        /// 格式为“工作流资源类型为{0}的通用审批配置下名称为{1}的节点的状态为{2}，但期待的状态为{3}”
        /// {0}：工作流资源类型
        /// {1}：节点名称
        /// {2}：节点状态
        /// {3}：期待的节点状态
        /// </summary>
        public const string CommonSignConfigurationNodeStatusError = "CommonSignConfigurationNodeStatusError";
        /// <summary>
        /// 找不到指定名称的通用审批配置节点动作直接跳转处理服务
        /// 格式为“找不到名称为{0}的通用审批配置节点动作直接跳转处理服务，位置为{1}”
        /// {0}：服务名称
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundCommonSignConfigurationNodeDirectGoExecuteServiceByName = "NotFoundCommonSignConfigurationNodeDirectGoExecuteServiceByName";
        /// <summary>
        /// 已存在相同名称的上传文件
        /// 格式为“上传文件中存在相同的名称{0}的数据”
        /// {0}：上传文件的UniqueName
        /// </summary>
        public const string ExistUploadFileByName = "ExistUploadFileByName";

        /// <summary>
        /// 找不到指定后缀名的文件后缀名与文件类型映射
        /// 格式为“找不到后缀名为{0}的文件后缀名与文件类型映射”
        /// {0}：文件后缀名
        /// </summary>
        public const string NotFoundFileSuffixFileTypeMapBySyffix = "NotFoundFileSuffixFileTypeMapBySyffix";
        /// <summary>
        ///  找不到指定文件类型的后缀名列表
        ///  格式为“找不到文件类型为{0}的后缀名列表”
        ///  {0}：文件类型
        /// </summary>
        public const string NotFoundSuffixsByFileType = "NotFoundSuffixsByFileType";

        /// <summary>
        /// 找不到指定后缀名的文件后缀名与Mime映射
        /// 格式为“找不到后缀名为{0}的文件后缀名与Mime映射”
        /// {0}：文件后缀名
        /// </summary>
        public const string NotFoundFileSuffixMimeMapBySyffix = "NotFoundFileSuffixMimeMapBySyffix";
        /// <summary>
        /// 找不到指定类型的上传文件源处理服务
        /// 格式为“找不到类型为{0}的上传文件源处理服务”
        /// {0}：文件类型
        /// </summary>
        public const string NotFoundUploadFileSourceExecuteServiceByType = "NotFoundUploadFileSourceExecuteServiceByType";
        /// <summary>
        /// 找不到指定id的上传文件
        /// 格式为“找不到关联类型为{0}，关联关键字为{1}，id为{2}的上传文件”
        /// {0}：关联类型
        /// {1}：关联关键字
        /// {2}：文件Id
        /// </summary>
        public const string NotFoundUploadFileById = "NotFoundUploadFileById";
        /// <summary>
        /// api参数验证错误
        /// 格式为“Api{0}中参数验证错误，详细信息为{1}”
        /// {0}：api的Action全路径名称
        /// {1}：参数错误的详细信息
        /// </summary>
        public const string ApiModelValidateError = "ApiModelValidateError";
        /// <summary>
        /// Sql执行超时
        /// 格式为“Sql执行超时”
        /// </summary>
        public const string SqlExecuteTimeout = "SqlExecuteTimeout";
        /// <summary>
        /// Http请求出错
        /// 格式为“Http请求出错，Url：{0}，Method：{1}，Data：{2}，详细信息：{3}”
        /// {0}：请求的Url
        /// {1}：Http Method
        /// {2}：请求的数据
        /// {3}：错误的详细信息
        /// </summary>
        public const string HttpRequestError = "HttpRequestError";

        /// <summary>
        /// 找不到指定key的上传文件处理服务
        /// 格式为“找不到指定key的上传文件处理服务，key为{0}，发生位置：{1}”
        /// {0}：key
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundUploadFileHandleServiceByKey = "NotFoundUploadFileHandleServiceByKey";

        /// <summary>
        /// 找不到指定名称和指定状态的上传文件处理配置
        /// 格式为“找不到名称为{0}，状态为{1}的上传文件处理配置”
        /// {0}：配置名称
        /// {1}：配置状态
        /// </summary>
        public const string NotFoundUploadFileHandleConfigurationByNameAndStatus = "NotFoundUploadFileHandleConfigurationByNameAndStatus";

        /// <summary>
        /// 找不到指定Id的上传文件处理记录
        /// 格式为“找不到id为{0}的上传文件处理记录”
        /// {0}：记录id
        /// </summary>
        public const string NotFoundUploadFileHandleRecordById = "NotFoundUploadFileHandleRecordById";
        /// <summary>
        /// 指定的上传文件处理记录中不包含上传文件
        /// 格式为“id为{0}的上传文件处理记录中不包含上传文件”
        /// {0}：记录id
        /// </summary>
        public const string UploadFileHandleRecordNotHasFile = "UploadFileHandleRecordNotHasFile";

        /// <summary>
        /// 找不到指定消息名称的Crm消息处理器
        /// 格式为“找不到消息名称为{0}的Crm消息处理器，位置为{1}”
        /// {0}：消息名称
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundCrmMessageExecuteHandleByName = "NotFoundCrmMessageExecuteHandleByName";

        /// <summary>
        /// 消息请求类型不匹配
        /// 格式为“消息请求类型不匹配，期待的类型为{0}，实际类型为{1}，位置为{2}”
        /// {0}：期待的类型
        /// {1}：实际的类型
        /// {2}：发生的位置
        /// </summary>
        public const string CrmRequestMessageTypeNotMatch = "CrmRequestMessageTypeNotMatch";
        /// <summary>
        /// Crm执行实体属性值转换处理类型不匹配
        /// 格式为“Crm执行实体属性值转换处理类型不匹配，期待的类型为{0}，实际类型为{1}，位置为{2}”
        /// {0}：期待的类型
        /// {1}：实际值的类型
        /// {2}：发生的位置
        /// </summary>
        public const string CrmExecuteEntityTypeHandleTypeNotMatch = "CrmExecuteEntityTypeHandleTypeNotMatch";
        /// <summary>
        /// 找不到指定类型名称的Crm执行实体属性值转换处理
        /// 格式为“找不到类型名称为{0}的Crm执行实体属性值转换处理，位置为{1}”
        /// {0}：类型名称
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundCrmExecuteEntityTypeHandleByTypeName = "NotFoundCrmExecuteEntityTypeHandleByTypeName";
        /// <summary>
        /// 在Crm服务令牌生成服务中找不到指定名称的参数
        /// 格式为“在Crm服务令牌生成服务{0}中，找不到名称为{1}的参数”
        /// {0}：服务的类名
        /// {1}：参数名称
        /// </summary>
        public const string NotFoundParameterInCrmServiceTokenGenerateService = "NotFoundParameterInCrmServiceTokenGenerateService";
        /// <summary>
        ///  在Crm服务令牌生成服务中指定参数的类型不匹配
        ///  格式为“在Crm服务令牌生成服务{0}中，名称为{1}的参数期望类型为{2}，而实际类型为{3}”
        ///  {0}：服务的类名
        ///  {1}：参数名称
        ///  {2}：参数期望类型
        ///  {3}：参数实际类型
        /// </summary>
        public const string ParameterTypeNotMatchInCrmServiceTokenGenerateService = "ParameterTypeNotMatchInCrmServiceTokenGenerateService";
        /// <summary>
        /// 找不到指定名称的Crm服务工厂
        /// 格式为"找不到名称为{0}的Crm服务工厂"
        /// {0}:工厂名称
        /// </summary>
        public const string NotFountCrmServiceFactorybyName = "NotFountCrmServiceFactorybyName";
        /// <summary>
        /// 找不到指定类型的Crm服务工厂服务
        /// 格式为“找不到类型为{0}的Crm服务工厂服务，发生位置：{1}”
        /// {0}：类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundCrmServiceFactoryServiceByType = "NotFoundCrmServiceFactoryServiceByType";

        /// <summary>
        /// 在Adfs的Oauth认证的响应中，找不到指定名称的参数
        /// 格式为“在Adfs的Oauth认证的响应中，找不到名称为{0}的参数，adfs的请求url为{1}，请求内容为{2}”
        /// {0}：参数名称
        /// {1}：请求Url
        /// {2}：请求内容
        /// </summary>
        public const string AdfsOauthResponseNotFoundParameterByName = "AdfsOauthResponseNotFoundParameterByName";
        /// <summary>
        /// Adfs的Oauth认证的响应出错
        /// 格式为“在Adfs的Oauth认证的响应出错，adfs的请求url为{0}，请求内容为{1},错误内容为{2}”
        /// {0}：请求Url
        /// {1}：请求内容
        /// {2}：错误内容
        /// </summary>
        public const string AdfsOauthResponseError = "AdfsOauthResponseError";
        /// <summary>
        /// 第三方系统服务HttpPost发生错误
        /// 格式为“第三方系统服务{0}的HttpPost发生错误，配置信息为{1}，PostUrl为{2}，Post内容为{3}，Post响应为{4}”
        /// {0}：第三方系统服务类名
        /// {1}：配置信息
        /// {2}：获取Token的地址
        /// {3}：提交的内容
        /// {4}：提交的响应
        /// </summary>
        public const string ThirdPartySystemServiceHttpPostError = "ThirdPartySystemServiceHttpPostError";

        /// <summary>
        /// 在第三方系统服务令牌验证错误
        /// 格式为“第三方系统服务{0}的令牌验证错误，PostUrl：{1}，令牌内容：{2}”
        /// {0}：第三方系统服务类名
        /// {1}：获取Token的地址
        /// {2}：令牌内容
        /// </summary>
        public const string ThirdPartySystemServiceTokenValidateError = "ThirdPartySystemServiceTokenValidateError";
        /// <summary>
        /// 第三方系统服务不支持操作
        /// 格式为“第三方系统服务{0}不支持{1}操作”
        /// {0}：第三方系统服务类名
        /// {1}：操作名称
        /// </summary>
        public const string ThirdPartySystemServiceNotSupportOperate = "ThirdPartySystemServiceNotSupportOperate";
        /// <summary>
        /// 第三方系统服务令牌中缺少指定键值
        /// 格式为“第三方系统服务{0}的令牌{1}中缺少键为{2}的键值对，PostUrl：{3}”
        /// {0}：第三方系统服务类名
        /// {1}：令牌内容
        /// {2}：缺少的令牌key
        /// {3}：获取Token的地址
        /// </summary>
        public const string ThirdPartySystemServiceTokenNotContainKey = "ThirdPartySystemServiceTokenNotContainKey";

        /// <summary>
        /// 在第三方系统后续操作服务中找不到需要的属性
        /// 格式为“在第三方系统后续操作服务{0}中，在上游第三方系统服务传入的键值对中，找不到键为{1}的值，请检查这两个服务设置是否匹配”
        /// {0}：第三方系统后续操作服务类名
        /// {1}：需要的键值对的键
        /// </summary>
        public const string NotFoundNeedAttributeInThirdPartySystemPostExecuteService = "NotFoundNeedAttributeInThirdPartySystemPostExecuteService";
        /// <summary>
        /// 在第三方系统后续操作服务中找不到Crm用户信息
        /// 格式为“在第三方系统后续操作服务{0}中找不到Crm用户信息,CrmServiceFactory名称：{1}，DomainName:{2}”
        /// {0}：第三方系统后续操作服务类名
        /// {1}：Crm服务工厂名称
        /// {2}：Crm中用户的DomainName
        /// </summary>
        public const string NotFoundCrmUserInfoInThirdPartySystemPostExecuteService = "NotFoundCrmUserInfoInThirdPartySystemPostExecuteService";

        /// <summary>
        /// AAD的Oauth认证的响应出错
        /// 格式为“在AAD的Oauth认证的响应出错，AAD的请求url为{0}，请求内容为{1},错误内容为{2}”
        /// {0}：请求Url
        /// {1}：请求内容
        /// {2}：错误内容
        /// </summary>
        public const string AADOauthResponseError = "AADOauthResponseError";


        /// <summary>
        /// 找不到指定名称的Crm服务令牌生成服务
        /// 格式为“找不到名称为{0}的Crm服务令牌生成服务，位置为{1}”
        /// {0}：服务名称
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundCrmServiceTokenGenerateServiceByName = "NotFoundCrmServiceTokenGenerateServiceByName";
        /// <summary>
        /// 调用Crm的webapi出错
        /// 格式为“调用Crm的Webapi出错，Uri:{0},Body：{1}，错误信息：{2}”
        /// {0}：请求的Uri
        /// {1}：请求的body
        /// {1}：错误信息
        /// </summary>
        public const string CrmWebApiCommonError = "CrmWebApiCommonError";
        /// <summary>
        /// 调用Crm的webapi出现并发性错误
        /// 格式为“调用Crm的webapi出现并发性错误，Uri:{0},Body：{1}，错误信息：{2}”
        /// {0}：请求的Uri
        /// {1}：请求的body
        /// {1}：错误信息
        /// </summary>
        public const string CrmWebApiConcurrencyError = "CrmWebApiConcurrencyError";
        /// <summary>
        /// 调用Crm的webapi出现限制性错误
        /// 格式为“调用Crm的webapi出现限制性错误，Uri:{0},Body：{1}，错误信息：{2}”
        /// {0}：请求的Uri
        /// {1}：请求的body
        /// {1}：错误信息
        /// </summary>
        public const string CrmWebApiLimitError = "CrmWebApiLimitError";

        /// <summary>
        /// 找不到指定请求类型名称的Crm消息处理
        /// 格式为“找不到请求类型名称为{0}的Crm消息处理，发生位置：{1}”
        /// {0}：请求类型名称
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundCrmMessageHandleByRequestTypeFullName = "NotFoundCrmMessageHandleByRequestTypeFullName";
        /// <summary>
        /// Crm消息处理不支持指定的HttpMethod
        /// 格式为“Crm消息处理不支持名称为{0}的HttpMethod”
        /// {0}：指定的HttpMethod
        /// </summary>
        public const string CrmMessageExecuteNotSupportMethod = "CrmMessageExecuteNotSupportMethod";
        /// <summary>
        /// 在Crm的Webapi响应中，找不到指定名称的头
        /// 格式为“在Crm的Webapi响应中，找不到名称为{0}的头，Uri:{1},Body：{2}”
        /// {0}：头名称
        /// {1}：请求的Uri
        /// {2}：请求的body
        /// </summary>
        public const string CrmWebApiHttpResponseNotFoundHeaderByName = "CrmWebApiHttpResponseNotFoundHeaderByName";
        /// <summary>
        /// 在Crm的Webapi响应中,正则匹配失败
        /// 格式为“在Crm的Webapi响应中，正则匹配失败，要匹配的字符串为{0}，表达式为{1}，Uri:{2},Body：{3}”
        /// {0}：要匹配的字符串
        /// {1}：匹配表达式
        /// {1}：请求的Uri
        /// {2}：请求的body
        /// </summary>
        public const string CrmWebApiHttpResponseRegexMatchFail = "CrmWebApiHttpResponseRegexMatchFail";
        /// <summary>
        /// 找不到指定类型名称的Crm唯一键属性值转换处理
        /// 格式为“找不到类型名称为{0}的Crm唯一键属性值转换处理，位置为{1}”
        /// {0}：属性值的类型名称
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundCrmAlternateKeyTypeHandleByTypeName = "NotFoundCrmAlternateKeyTypeHandleByTypeName";
        /// <summary>
        /// Crm唯一键属性值转换处理类型不匹配
        /// 格式为“Crm唯一键属性值转换处理类型不匹配，期待的类型为{0}，实际类型为{1}，发生的位置：{2}”
        /// {0}：期待类型
        /// {1}：实际类型
        /// {2}：发生的位置
        /// </summary>
        public const string CrmAlternateKeyTypeHandleTypeNotMatch = "CrmAlternateKeyTypeHandleTypeNotMatch";

        /// <summary>
        /// 指定的Crm的JToken转换服务中，传入的JToken类型不匹配
        /// 格式为“在Crm的JToken转换服务{0}中，传入的JToken类型不匹配，期待的类型为{1}，实际类型为{2}，JToken值为{3}”
        /// {0}：服务名称
        /// {1}：期待的类型
        /// {2}：实际类型
        /// {3}：传入的JToken值
        /// </summary>
        public const string CrmJTokenConvertNotMatch = "CrmJTokenConvertNotMatch";
        /// <summary>
        /// 指定的Crm的JToken转换服务中,传入的JToken中找不到指定属性
        /// 格式为“在Crm的JToken转换服务{0}中，传入的JToken中找不到属性{1}，实体名称为{2}，JToken值为{3}”
        /// {0}：服务名称
        /// {1}：属性名称
        /// {2}：实体名称
        /// {3}：传入的JToken值
        /// </summary>
        public const string CrmJTokenConvertEntityNotFoundAttribute = "CrmJTokenConvertEntityNotFoundAttribute";

        /// <summary>
        /// 找不到指定类型的Crm查询结果JToken处理
        /// 格式为“找不到类型为{0}的Crm查询结果JToken处理”
        /// {0}：类型全名
        /// </summary>
        public const string NotFoundCrmRetrieveJTokenHandleByType = "NotFoundCrmRetrieveJTokenHandleByType";
        /// <summary>
        /// 指定的Crm查询结果JToken处理返回的结果的类型不匹配
        /// 格式为“类型为{0}的Crm查询结果JToken处理返回的结果的类型与期望类型不匹配，期望类型为{1}，实际类型为{2}”
        /// {0}：处理类型全名
        /// {1}：期望结果的类型全名
        /// {2}：实际结果的类型全名
        /// </summary>
        public const string CrmRetrieveJTokenHandleResultTypeNotMatch = "CrmRetrieveJTokenHandleResultTypeNotMatch";
        /// <summary>
        /// 指定的Crm查询结果JToken处理缺少参数
        /// 格式为“类型为{0}的Crm查询结果JToken处理缺少参数{1}”
        /// {0}：处理类型全名
        /// {1}：参数名称
        /// </summary>
        public const string CrmRetrieveJTokenHandleMissParameter = "CrmRetrieveJTokenHandleMissParameter";
        /// <summary>
        /// 指定的Crm查询结果JToken处理的指定参数的类型不匹配
        /// 格式为“类型为{0}的Crm查询结果JToken处理的参数{1}的类型不匹配，期待的类型为{2}，实际类型为{3}”
        /// {0}：处理类型全名
        /// {1}：参数名称
        /// {2}：期待的类型全名
        /// {3}：实际的类型全名
        /// </summary>
        public const string CrmRetrieveJTokenHandleParameterTypeNotMatch = "CrmRetrieveJTokenHandleParameterTypeNotMatch";

        /// <summary>
        /// 指定的Crm的JToken转换成EntityReference服务中,传入的JToken中找不到指定属性
        /// 格式为“在Crm的JToken转换EntityReference服务{0}中，传入的JToken中找不到属性{1}，JToken值为{2}”
        /// {0}：服务的类全名
        /// {1}：属性名称
        /// {2}：JToken的值
        /// </summary>
        public const string CrmJTokenConvertEntityReferenceNotFoundAttribute = "CrmJTokenConvertEntityReferenceNotFoundAttribute";
        /// <summary>
        /// 找不到指定类型的Crm函数参数处理
        /// 格式为“找不到类型为{0}的Crm函数参数处理，发生位置：{1}”
        /// {0}：类型的全名称
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundCrmFunctionParameterHandleByType = "NotFoundCrmFunctionParameterHandleByType";
        /// <summary>
        /// 找不到指定类型的Crm动作参数处理
        /// 格式为“找不到类型为{0}的Crm动作参数处理，发生位置：{1}”
        /// {0}：类型的全名称
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundCrmActionParameterHandleByType = "NotFoundCrmActionParameterHandleByType";
        /// <summary>
        /// 在Batch响应中找不到BatchCode
        /// 格式为“在Crm的Batch操作响应中找不到BatchCode，响应内容首行为{0}”
        /// {0}：相应内容的首行
        /// </summary>
        public const string NotFoundBatchCodeInCrmBatchResponse = "NotFoundBatchCodeInCrmBatchResponse";
        /// <summary>
        /// Crm的Batch操作响应为空
        /// 格式为“Crm的Batch操作响应为空”
        /// </summary>
        public const string CrmBatchResponseIsEmpty = "CrmBatchResponseIsEmpty";
        /// <summary>
        /// Crm的Batch操作响应项格式错误
        /// 格式为“Crm的Batch操作响应项格式错误，响应项内容：{0}，错误提示：{1}”
        /// {0}：响应项内容
        /// {1}：错误内容
        /// </summary>
        public const string CrmBatchResponseItemFormatError = "CrmBatchResponseItemFormatError";
        /// <summary>
        /// 找不到指定类型的Crm属性元数据处理
        /// 格式为“找不到返回类型为{0}的Crm属性元数据处理”
        /// {0}：属性的返回类型
        /// </summary>
        public const string NotFoundCrmAttributeMetadataHandleByType = "NotFoundCrmAttributeMetadataHandleByType";
        /// <summary>
        /// 在指定消息的响应中，找不到指定名称的header
        /// 格式为“在消息{0}的响应中，找不到名称为{1}的header，返回的header名称集合为{2}”
        /// {0}：消息名称
        /// {1}：要找的Header名称
        /// {2}：实际响应中的Header名称集合
        /// </summary>
        public const string NotFoundHeaderFromResponse = "NotFoundHeaderFromResponse";

        /// <summary>
        /// 找不到指定表达式类型的工作流活动参数处理
        /// 格式为“找不到表达式类型为{0}的工作流活动参数处理”
        /// {0}：表达式类型
        /// </summary>
        public const string NotFoundRealWorkfolwActivityParameterHandleByType = "NotFoundRealWorkfolwActivityParameterHandleByType";
        /// <summary>
        /// 工作流活动参数数据处理验证失败
        /// 格式为“工作流活动参数数据处理验证失败，验证器数据类型为{0}，要验证的实际数据类型为{1}，参数名称为{2}”
        /// {0}：验证器数据类型
        /// {1}：实际数据的类型
        /// {2}：参数名称
        /// </summary>
        public const string RealWorkfolwActivityParameterDataHandleValidateError = "RealWorkfolwActivityParameterDataHandleValidateError";
        /// <summary>
        /// 工作流活动结果中的输出参数名称在工作流活动的输出参数中未定义
        /// 格式为“工作流活动{0}的结果中的输出参数名称{1}在工作流活动的输出参数中未定义，工作流id为{2}”
        /// {0}：工作流活动的Id
        /// {1}：输出参数的名称
        /// {2}：工作流的Id
        /// </summary>
        public const string NotFoundRealWorkfolwActivityOutputParameterByActivityResult = "NotFoundRealWorkfolwActivityOutputParameterByActivityResult";
        /// <summary>
        /// 工作流活动输出参数数据处理验证失败
        /// 格式为“工作流活动参数数据处理验证失败，验证器数据类型为{0}，要验证的实际数据类型为{1}，参数名称为{2},工作流活动Id为{3}，工作流Id为{4}”
        /// {0}：验证器数据类型
        /// {1}：实际数据的类型
        /// {2}：参数名称
        /// {3}：工作流活动的Id
        /// {4}：工作流的Id
        /// </summary>
        public const string RealWorkfolwActivityOutputParameterDataHandleValidateError = "RealWorkfolwActivityOutputParameterDataHandleValidateError";

        /// <summary>
        /// 工作流活动参数处理出错
        /// 格式为“工作流活动参数处理出错，错误原因：{0}，参数名称：{1}，参数数据类型：{2}，参数表达式类型：{3}，参数配置：{4}，发生位置：{5}”
        /// {0}：错误原因
        /// {1}：参数名称
        /// {2}：参数数据类型
        /// {3}：参数表达式类型
        /// {4}：参数配置
        /// {5}：发生位置
        /// </summary>
        public const string RealWorkfolwActivityParameterHandleExecuteError = "RealWorkfolwActivityParameterHandleExecuteError";
        /// <summary>
        /// 工作流活动参数的配置不是表达式
        /// 格式为“工作流活动参数的配置不是表达式”
        /// </summary>
        public const string RealWorkfolwActivityParameterConfigurationIsNotExpression = "RealWorkfolwActivityParameterConfigurationIsNotExpression";

        /// <summary>
        /// 工作流活动配置转换成XML时出错
        /// 格式为“工作流活动配置转换成XML时出错，活动配置：{0}，错误原因：{1}”
        /// {0}：工作流活动的配置
        /// {1}：错误原因
        /// </summary>
        public const string RealWorkfolwActivityConfigurationParseXMLError = "RealWorkfolwActivityConfigurationParseXMLError";
        /// <summary>
        /// 找不到指定名称的工作流活动解析
        /// 格式为“找不到名称为{0}的工作流活动解析”
        /// {0}：工作流活动名称
        /// </summary>
        public const string NotFoundRealWorkfolwActivityResolveByName = "NotFoundRealWorkfolwActivityResolveByName";

        /// <summary>
        /// 找不到指定名称的工作流活动计算
        /// 格式为“找不到名称为{0}的工作流活动计算”
        /// {0}：工作流活动名称
        /// </summary>
        public const string NotFoundRealWorkflowActivityCalculateByName = "NotFoundRealWorkflowActivityCalculateByName";

        /// <summary>
        /// 工作流活动配置缺少指定的XML节点
        /// 格式为“工作流活动配置缺少XML节点{0}”
        /// {0}：xml节点名称
        /// </summary>
        public const string RealWorkfolwActivityConfigurationMissXMLElement = "RealWorkfolwActivityConfigurationMissXMLElement";
        /// <summary>
        /// 工作流活动配置缺少指定的XML属性
        /// 格式为“工作流活动配置缺少XML属性{0}”
        /// {0}：xml属性名称
        /// </summary>
        public const string RealWorkfolwActivityConfigurationMissXMLAttribute = "RealWorkfolwActivityConfigurationMissXMLAttribute";
        /// <summary>
        /// 工作流活动配置指定的节点缺少指定的XML属性
        /// 格式为“工作流活动配置缺少XML属性{0}，属性所属节点内容：{1}”
        /// {0}：xml属性名称
        /// {1}：属性所属节点内容
        /// </summary>
        public const string RealWorkfolwActivityConfigurationMissXMLElementAttribute = "RealWorkfolwActivityConfigurationMissXMLElementAttribute";
        /// <summary>
        /// 工作流活动配置缺少指定的XML节点的指定子节点
        /// 格式为“工作流活动配置缺少XML节点{0}，节点所属的父节点内容：{1}”
        /// {0}：xml节点名称
        /// {1}：节点的父节点内容
        /// </summary>
        public const string RealWorkfolwActivityConfigurationMissXMLChildElement = "RealWorkfolwActivityConfigurationMissXMLChildElement";
        /// <summary>
        /// 工作流活动描述的Data属性的类型不匹配
        /// 格式为“工作流活动描述的Data属性的类型不匹配，期待的类型为{0}，实际类型为{1}，发生位置：{2}”
        /// {0}：期待的类型
        /// {1}：实际类型
        /// {2}：发生的位置
        /// </summary>
        public const string RealWorkfolwActivityDescriptionDataTypeNotMatch = "RealWorkfolwActivityDescriptionDataTypeNotMatch";
        /// <summary>
        /// 在工作流活动描述中找不到指定名称的输入参数
        /// 格式为“名称为{0}的工作流描述中找不到名称为{1}的输入参数”
        /// {0}：工作流描述的名称
        /// {1}：输入参数的名称
        /// </summary>
        public const string NotFoundRealWorkfolwActivityDescriptionInputByName = "NotFoundRealWorkfolwActivityDescriptionInputByName";
        /// <summary>
        /// 在工作流活动描述中指定名称的输入参数的结果类型不匹配
        /// 格式为“名称为{0}的工作流描述中名称为{1}的输入参数的结果值类型不匹配，期待的类型为{2}，实际类型为{3},参数配置为{4}”
        /// {0}：工作流描述的名称
        /// {1}：输入参数的名称
        /// {2}：期待的类型
        /// {3}：实际类型
        /// {4}：参数配置
        /// </summary>
        public const string RealWorkfolwActivityDescriptionInputResultTypeNotMatch = "RealWorkfolwActivityDescriptionInputResultTypeNotMatch";
        /// <summary>
        /// 工作流活动配置的Id属性不能转换成Guid
        /// 格式为“工作流活动配置的Id属性不能转换成Guid，当前的Id属性为{0}”
        /// {0}：Id属性的值
        /// </summary>
        public const string RealWorkfolwActivityConfigurationIdAttributeParseError = "RealWorkfolwActivityConfigurationIdAttributeParseError";

        /// <summary>
        /// 工作流活动描述中指定的内部输入参数的计算结果类型不匹配
        /// 格式为“名称为{0}的工作流描述中内部输入参数{1}的计算结果类型不匹配，期待类型为{2}，实际类型为{3}，参数配置为{4}”
        /// {0}：工作流描述的名称
        /// {1}：内部输入参数的名称
        /// {2}：参数计算结果的期待类型
        /// {3}：参数计算结果的实际类型
        /// {4}：参数配置
        /// </summary>
        public const string RealWorkfolwActivityDescriptionInnerInputResultTypeNotMatch = "RealWorkfolwActivityDescriptionInnerInputResultTypeNotMatch";
        /// <summary>
        /// 工作流活动配置中，指定的属性转换为指定的类型时出错
        /// 格式为“工作流活动配置中的属性{0}的值无法转换为类型{1}，属性值为{2}，所属节点的值为{3}”
        /// {0}：属性名称
        /// {1}：属性值要转换的类型全名
        /// {2}：属性值
        /// {3}：所属节点的值
        /// </summary>
        public const string RealWorkfolwActivityConfigurationAttributeParseTypeError = "RealWorkfolwActivityConfigurationAttributeParseTypeError";

        /// <summary>
        /// 找不到指定类型的Grpc服务端凭证生成服务
        /// 格式为“找不到类型为{0}的Grpc服务端凭证生成服务，发生位置：{1}”
        /// {0}：类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundGrpcServerCredentialsGeneratorServiceByType = "NotFoundGrpcServerCredentialsGeneratorServiceByType";

        /// <summary>
        /// 找不到指定类型的Grpc通道凭证生成服务
        /// 格式为“找不到类型为{0}的Grpc通道凭证生成服务，发生位置：{1}”
        /// {0}：类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundGrpcChannelCredentialsGeneratorServiceByType = "NotFoundGrpcChannelCredentialsGeneratorServiceByType";
        /// <summary>
        /// 找不到指定类型的OData客户端生成器
        /// 格式为“找不到类型为{0}的OData客户端生成器，发生位置：{1}”
        /// {0}：OData客户端的类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundODataClientGeneratorByType = "NotFoundODataClientGeneratorByType";
        /// <summary>
        /// 找不到指定配置类型的OData客户端初始化器
        /// 格式为“找不到指定配置类型为{0}的OData客户端初始化器，发生位置：{1}”
        /// {0}：配置类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundODataClientInitializationByConfiguration = "NotFoundODataClientInitializationByConfiguration";
        /// <summary>
        /// OData客户端初始化配置不正确
        /// 格式为“OData客户端初始化配置不正确，期待的格式为{0}，现在的内容为{1}，发生位置：{2}”
        /// {0}：期待的格式
        /// {1}：现在的内容
        /// {2}：发生的位置
        /// </summary>
        public const string ODataClientInitConfigurationNotCorrect = "ODataClientInitConfigurationNotCorrect";
        /// <summary>
        /// 找不到指定类型的矩阵数据提供方服务
        /// 格式为“找不到类型为{0}的矩阵数据提供方服务，发生位置：{1}”
        /// {0}：提供方类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundMatrixDataProviderServiceByType = "NotFoundMatrixDataProviderServiceByType";
        /// <summary>
        /// 找不到指定类型的矩阵数据处理方服务
        /// 格式为“找不到类型为{0}的矩阵数据处理方服务，发生位置：{1}”
        /// {0}：处理方类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundMatrixDataHandlerServiceByType = "NotFoundMatrixDataHandlerServiceByType";

        /// <summary>
        /// 矩阵行的列数超出类型映射配置数
        /// 格式为“矩阵行的列数为{0}，而该矩阵数据提供方{1}的类型映射配置长度为{2}，两者不匹配，发生位置：{3}”
        /// {0}：矩阵行的列数
        /// {1}：提供方名称
        /// {2}：类型映射配置的长度
        /// {3}：发生的位置
        /// </summary>
        public const string MatrixDataRowColumnCountOutTypeMappingCount = "MatrixDataRowColumnCountOutTypeMappingCount";
        /// <summary>
        /// 找不到指定类型的矩阵数据类型转换服务
        /// 格式为“找不到转换类型为{0}的矩阵数据类型转换服务，发生位置：{1}”
        /// {0}:要转换的类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundMatrixDataTypeConvertServiceByType = "NotFoundMatrixDataTypeConvertServiceByType";

        /// <summary>
        /// 在声明集中找不到指定类型的声明
        /// 格式为“找不到类型为{0}的声明，发生位置：{1}”
        /// {0}：指定的声明类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundTypeInClaims = "NotFoundTypeInClaims";

        /// <summary>
        /// 找不到指定名称的系统配置
        /// 格式为“找不到名称为{0}的系统配置”
        /// {0}：系统配置名称
        /// </summary>
        public const string NotFoundSystemConfigurationByName = "NotFoundSystemConfigurationByName";
        /// <summary>
        /// 系统配置转换成指定类型失败
        /// 格式为“系统配置{0}转成类型{1}失败，配置内容为{2}”
        /// {0}：系统配置名称
        /// {1}：要转换成的类型
        /// {2}：配置内容
        /// </summary>
        public const string SystemConfigurationConvertTypeFail = "SystemConfigurationConvertTypeFail";

        /// <summary>
        /// 找不到指定类型的针对日志提供方的日志构建器处理
        /// 格式为“找不到类型为{0}的针对日志提供方的日志构建器处理，发生位置：{1}”
        /// {0}：日志提供方类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundLoggingBuilderProviderHandlerByType = "NotFoundLoggingBuilderProviderHandlerByType";
        /// <summary>
        /// 找不到指定源信息和Id的分片存储信息
        /// 格式为“找不到源信息为{0}，Id为{1}的分片存储信息”
        /// {0}：源信息
        /// {1}：分片存储信息的id
        /// </summary>

        public const string NotFoundMultipartStorgeInfoBySourceInfoAndID = "NotFoundMultipartStorgeInfoBySourceInfoAndID";

        /// <summary>
        /// 阿里OSS分片单一文件超出最大限制
        /// 格式为“阿里OSS分片单一文件超出最大限制，最大限制：{0}字节，文件名称：{1}，实际大小：{2}”
        /// {0}：OSS单一文件最大字节数
        /// {1}：文件名称
        /// {2}：文件实际大小
        /// </summary>
        public const string AliOSSMultipartExceedTotalMaxSize = "AliOSSMultipartExceedTotalMaxSize";
        /// <summary>
        /// 阿里OSS分片数量超出最大限制
        /// 格式为“阿里OSS分片数量超出最大限制，最大限制：{0}，文件名称：{1}，实际分片数量：{2}”
        /// {0}：OSS分片上传最大分片数量
        /// {1}：文件名称
        /// {2}：实际分片数量
        /// </summary>
        public const string AliOSSMultipartExceedMaxNumer = "AliOSSMultipartExceedMaxNumer";
        /// <summary>
        /// 阿里OSS分片每片大小超出最大限制
        /// 格式为“阿里OSS分片每片大小超出最大限制，最大限制：{0}，文件名称：{1}，实际每片大小：{2}”
        /// {0}：OSS分片上传每片最大大小
        /// {1}：文件名称
        /// {2}：实际每片大小
        /// </summary>
        public const string AliOSSMultipartExceedMaxPerSize = "AliOSSMultipartExceedMaxPerSize";
        /// <summary>
        /// 阿里OSS分片每片大小低于最小限制
        /// 格式为“阿里OSS分片每片大小低于最小限制，最小限制：{0}，文件名称：{1}，实际每片大小：{2}”
        /// {0}：OSS分片上传每片最小大小
        /// {1}：文件名称
        /// {2}：实际每片大小
        /// </summary>
        public const string AliOSSMultipartLessMaxPerSize = "AliOSSMultipartLessMaxPerSize";
        /// <summary>
        /// 阿里OSS分片存储信息明细中的数据位置不正确
        /// 格式为“阿里OSS分片存储信息明细中的数据位置不正确，分片存储Id：{0}，分片存储明细编号：{1}，实际文件长度：{2}”
        /// {0}：分片存储Id
        /// {1}：分片存储明细编号
        /// {2}：文件实际长度
        /// </summary>
        public const string AliOSSMultiparStorgeInfoDetailDataPositionNotCorrect = "AliOSSMultiparStorgeInfoDetailDataPositionNotCorrect";
        /// <summary>
        /// 阿里OSS中找不到指定名称的文件
        /// 格式为“阿里OSS中找不到Bucket为{0}，Key为{1}的文件”
        /// {1}：Bucket名称
        /// {2}：文件名称
        /// </summary>
        public const string AliOSSNotFoundObject = "AliOSSNotFoundObject";
        /// <summary>
        /// 阿里OSS分片存储信息的状态不允许上传
        /// 格式为“阿里OSS分片存储信息的状态不允许上传，分片存储Id：{0}，状态：{1}”
        /// {0}：分片存储Id
        /// {1}：分片存储状态
        /// </summary>
        public const string AliOSSMultiparStorgeInfoStatusNotAllowUpload = "AliOSSMultiparStorgeInfoStatusNotAllowUpload";

        /// <summary>
        /// 阿里OSS分片存储信息的状态不允许复制
        /// 格式为“阿里OSS分片存储信息的状态不允许复制，分片存储Id：{0}，状态：{1}”
        /// {0}：分片存储Id
        /// {1}：分片存储状态
        /// </summary>
        public const string AliOSSMultiparStorgeInfoStatusNotAllowCopy = "AliOSSMultiparStorgeInfoStatusNotAllowCopy";

        /// <summary>
        /// 阿里OSS分片存储信息的状态不允许执行完成操作
        /// 格式为“阿里OSS分片存储信息的状态不允许执行完成操作，分片存储Id：{0}，状态：{1}”
        /// {0}：分片存储Id
        /// {1}：分片存储状态 
        /// </summary>
        public const string AliOSSMultiparStorgeInfoStatusNotAllowComplete = "AliOSSMultiparStorgeInfoStatusNotAllowComplete";
        /// <summary>
        /// 阿里OSS分片存储信息不允许执行完成操作,原因为包含有未完成的明细
        /// 格式为“阿里OSS分片存储信息不允许执行完成操作,原因为包含有未完成的明细，分片存储Id：{0}”
        /// {0}：分片存储Id
        /// </summary>
        public const string AliOSSMultiparStorgeInfoNotAllowCompleteForUnDoDetail = "AliOSSMultiparStorgeInfoNotAllowCompleteForUnDoDetail";
        /// <summary>
        /// 存在相同名称的未完成分片存储信息
        /// 格式为“已经存在名称为{0}的未完成分片存储信息”
        /// {0}:分片存储名称
        /// </summary>
        public const string ExistRunMultipartStorgeInfoByName = "ExistRunMultipartStorgeInfoByName";

        /// <summary>
        /// 找不到指定类型的生成Jwt生成时使用的签名密钥服务
        /// 格式为“找不到类型为{0}的生成Jwt生成时使用的签名密钥服务”
        /// {0}：生成时使用的签名类型
        /// </summary>
        public const string NotFoundJwtGenerateCreateSignKeyServiceByType = "NotFoundJwtGenerateCreateSignKeyServiceByType";
        /// <summary>
        /// 找不到指定类型的生成Jwt验证时使用的签名密钥服务
        /// 格式为“找不到类型为{0}的生成Jwt验证时使用的签名密钥服务”
        /// {0}：验证时使用的签名类型
        /// </summary>
        public const string NotFoundJwtGenerateValidateSignKeyServiceByType = "NotFoundJwtGenerateValidateSignKeyServiceByType";
        /// <summary>
        /// 找不到指定类型的Jwt验证参数组装服务
        /// 格式为“找不到类型为{0}的Jwt验证参数组装服务”
        /// {0}：Jwt验证类型
        /// </summary>
        public const string NotFoundjwtValidateParameterBuildServiceByType = "NotFoundjwtValidateParameterBuildServiceByType";

        /// <summary>
        /// 找不到指定队列类型的队列实际处理服务
        /// 格式为“找不到队列类型为{0}的队列实际处理服务，发生位置为{1}”
        /// {0}：指定的队列类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFountCommonQueueRealExecuteServiceByType = "NotFountCommonQueueRealExecuteServiceByType";
        /// <summary>
        /// 在指定消息类型下找不到指定监听ID的监听
        /// 格式为“名称为{0}的消息类型下，找不到监听ID为{1}的监听”
        /// {0}：消息类型名称
        /// {1}：监听ID
        /// </summary>
        public const string NotFoundSMessageTypeListenerFromTypeByID = "NotFoundSMessageTypeListenerFromTypeByID";
        /// <summary>
        /// 找不到指定名称的消息类型
        /// 格式为“找不到名称为{0}的消息类型”
        /// {0}：消息类型名称
        /// </summary>
        public const string NotFoundSMessageTypeByName = "NotFoundSMessageTypeByName";
        /// <summary>
        /// 找不到指定名称的Azure服务总线To转换服务
        /// 格式为“找不到名称为{0}的Azure服务总线To转换服务，发生位置为{1}”
        /// {0}：服务名称
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundAzureServiceBusMessageConvertToServiceByName = "NotFoundAzureServiceBusMessageConvertToServiceByName";
        /// <summary>
        /// 找不到指定名称的Azure服务总线From转换服务
        /// 格式为“找不到名称为{0}的Azure服务总线From转换服务，发生位置为{1}”
        /// {0}：服务名称
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundAzureServiceBusMessageConvertFromServiceByName = "NotFoundAzureServiceBusMessageConvertFromServiceByName";
        /// <summary>
        /// 找不到指定类型名称的通用消息客户端类型
        /// 格式为“找不到类型名称为{0}的通用消息客户端类型”
        /// {0}：类型名称
        /// </summary>
        public const string NotFoundCommonMessageClientType = "NotFoundCommonMessageClientType";
        /// <summary>
        /// 找不到指定消息类型的消息处理服务
        /// 格式为“找不到消息类型为{0}的消息处理服务，发生位置为{1}”
        /// {0}：消息类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundCommonMessageHandleServiceByMessageType = "NotFoundCommonMessageHandleServiceByMessageType";
        /// <summary>
        /// 在指定名称的表达式计算器中，要计算的表达式为空
        /// 格式为“在名称为{0}的表达式计算器中，要计算的表达式为空”
        /// {0}：表达式计算器的名称
        /// </summary>
        public const string ExpressionEmptyInExpressionCalculatorByName = "ExpressionEmptyInExpressionCalculatorByName";
        /// <summary>
        /// 指定的表达式格式不正确
        /// 格式为“在名称为{0}的表达式计算器中，表达式格式不正确，表达式为{1}”
        /// {0}：表达式计算器的名称
        /// {1}；表达式
        /// </summary>
        public const string ExpressionFormatError = "ExpressionFormatError";
        /// <summary>
        /// 在指定的表达式的存储之中，找不到指定键的值
        /// 格式为“在名称为{0}的表达式计算器中，找不到键为{1}的值，表达式为{2}”
        /// {0}：表达式计算器的名称
        /// {1}：键
        /// {2}；表达式
        /// </summary>
        public const string NotFoundValueInExpressionStoreValues = "NotFoundValueInExpressionStoreValues";
        /// <summary>
        ///  在指定的表达式中，找不到公式服务列表
        ///  格式为“找不到名称为{0}的表达式所对应的公式服务列表”
        ///  {0}：表达式计算器的名称
        /// </summary>
        public const string NotFoundFormulaServiceListFromExpression = "NotFoundFormulaServiceListFromExpression";
        /// <summary>
        /// 在指定的表达式的公式服务列表中，找不到指定的公式服务
        /// 格式为“名称为{0}的表达式所对应的公式服务列表中，找不到公式名称为{1}的公式服务”
        /// {0}：表达式计算器的名称
        /// {1}：公式名称
        /// </summary>
        public const string NotFoundFormulaServiceFormServiceList = "NotFoundFormulaServiceFormServiceList";
        /// <summary>
        /// 找不到指定类型的工作流活动服务
        /// 格式为“找不到类型为{0}的工作流活动服务，发生位置为{1}”
        /// {0}：工作流活动类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundWorkflowActivityServiceByType = "NotFoundWorkflowActivityServiceByType";
        /// <summary>
        /// 指定类型的工作流活动服务参数数量不正确
        /// 格式为“类型为{0}的工作流活动服务参数数量不正确，需要的数量为{1}，实际数量为{2}”
        /// {0}：活动类型
        /// {1}：需要的数量
        /// {2}：实际的数量
        /// </summary>
        public const string WorkflowActivityServiceParameterCountError = "WorkflowActivityServiceParameterCountError";
        /// <summary>
        /// 指定类型的工作流活动服务的指定位数参数类型不正确
        /// 格式为“类型为{0}的工作流活动服务的第{1}位参数类型不正确，期待的类型为{2}，实际类型为{3}”
        /// {0}：活动类型
        /// {1}：参数在列表中的所属位置
        /// {2}：期待的类型
        /// {3}：实际的类型
        /// </summary>
        public const string WorkflowActivityServiceParameterTypeError = "WorkflowActivityServiceParameterTypeError";
        /// <summary>
        /// 找不到指定缓存类型的实际KV缓存访问服务
        /// 格式为“找不到缓存类型为{0}的实际KV缓存访问服务，发生位置为{1}”
        /// {0}：缓存类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundRealKVCacheVisitServiceByCacheType = "NotFoundRealKVCacheVisitServiceByCacheType";
        /// <summary>
        /// 找不到指定版本名称的KV缓存版本服务
        /// 格式为“找不到版本名称为{0}的KV缓存版本服务，发生位置为{1}”
        /// {1}：版本名称
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundIKVCacheVersionServiceByName = "NotFoundIKVCacheVersionServiceByName";
        /// <summary>
        /// 找不到指定类型的令牌控制器服务
        /// 格式为“找不到类型为{0}的令牌控制器服务”
        /// {0}：控制器类型
        /// </summary>
        public const string NotFoundTokenControllerServiceByType = "NotFoundTokenControllerServiceByType";

        /// <summary>
        /// 找不到指定名称的令牌控制器
        /// 格式为“找不到名称为{0}的令牌控制器”
        /// {0}：控制器名称
        /// </summary>
        public const string NotFoundTokenControllerByName = "NotFoundTokenControllerByName";
        /// 业务动作验证失败
        /// 格式为“{0}”
        /// {0}：验证结果详细信息
        public const string BusinessActionValidateFail = "BusinessActionValidateFail";
        /// <summary>
        /// 找不到指定名称的业务动作
        /// 格式为“找不到名称为{0}的业务动作”
        /// {0}：业务动作名称
        /// </summary>
        public const string NotFoundBusinessActionByName = "NotFoundBusinessActionByName";
        /// <summary>
        /// 找不到指定名称的国际化处理服务工厂
        /// 格式为“找不到名称为{0}的国际化处理服务工厂，发生位置为{1}”
        /// {0}：名称
        /// {1}：发生的位置
        /// </summary>
        public const string NotFountInternationalizationHandleServiceFactoryByName = "NotFountInternationalizationHandleServiceFactoryByName";
        /// <summary>
        /// 找不到指定名称的Http请求扩展上下文处理服务工厂
        /// 格式为“找不到名称为{0}的Http请求扩展上下文处理服务工厂，发生位置为{1}”
        /// {0}：名称
        /// {1}：发生的位置
        /// </summary>
        public const string NotFountHttpExtensionContextHandleServiceFactoryByName = "NotFountHttpExtensionContextHandleServiceFactoryByName";
        /// <summary>
        /// 找不到指定类型的机密数据服务
        /// 格式为“找不到类型为{0}的机密数据服务，发生位置为{1}”
        /// {0}：类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundSecurityVaultServiceByType = "NotFoundSecurityVaultServiceByType";
        /// <summary>
        /// 不支持指定的AzureVault验证方式
        /// 格式为“不支持方式为{0}的AzureVault验证方式，发生位置为{1}”
        /// {0}：验证方式
        /// {1}：发生的位置
        /// </summary>
        public const string NotSupportAzureVaultAuthType = "NotSupportAzureVaultAuthType";
        /// <summary>
        /// 找不到指定类型的Azure令牌凭据生成服务
        /// 格式为“找不到类型为{0}的Azure令牌凭据生成服务，发生位置为{1}”
        /// {0}：类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundTokenCredentialGeneratorServiceByType = "NotFoundTokenCredentialGeneratorServiceByType";
        /// <summary>
        /// 找不到指定名称的Azure令牌凭据生成器
        /// 格式为“找不到名称为{0}的Azure令牌凭据生成器”
        /// {0}：Azure令牌凭据生成器名称
        /// </summary>
        public const string NotFoundTokenCredentialGeneratorByName = "NotFoundTokenCredentialGeneratorByName";
        /// <summary>
        /// 找不到指定类型的分布式操作记录服务
        /// 格式为“找不到类型为{0}的分布式操作记录服务，发生位置为{1}”
        /// {0}：类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundDTOperationRecordServiceByType = "NotFoundDTOperationRecordServiceByType";
        /// <summary>
        /// 找不到指定类型的分布式操作数据服务
        /// 格式为“找不到类型为{0}的分布式操作数据服务，发生位置为{1}”
        /// {0}：类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundDTOperationDataServiceByType = "NotFoundDTOperationDataServiceByType";
        /// <summary>
        /// 在指定的存储组中找不到指定名称的组成员
        /// 格式为“在id为{0}的存储组中找不到名称为{1}的组成员”
        /// {0}：组ID
        /// {1}：组成员名称
        /// </summary>
        public const string NotFounStoreGroupMemberByName = "NotFounStoreGroupMemberByName";
        /// <summary>
        /// 找不到指定名称的存储组
        /// 格式为“找不到名称为{0}的存储组”
        /// {0}：组名称
        /// </summary>
        public const string NotFounStoreGroupByName = "NotFounStoreGroupByName";
        /// <summary>
        /// 指定的存储组中指定名称的组成员的存储信息不是指定的类型
        /// 格式为“名称为{0}的存储组中名称为{1}的组成员的存储信息不是需要的类型{2}”
        /// {0}：存储组名称
        /// {1}：成员名称
        /// {2}：需要的类型
        /// </summary>
        public const string StoreGroupMemberInfoTypeError = "StoreGroupMemberInfoTypeError";
        /// <summary>
        /// 在指定的存储信息中找不到指定的实体表映射
        /// 格式为“名称为{0}的存储组中名称为{1}的组成员的存储信息中缺少实体名称为{2}的实体表映射”
        /// {0}：存储组名称
        /// {1}：成员名称
        /// {2}：实体名称
        /// </summary>
        public const string NotFoundEntityNameInStoreInfoFromStoreGroup = "NotFoundEntityNameInStoreInfoFromStoreGroup";
        /// <summary>
        /// 在指定的存储信息中找不到指定的实体表映射
        /// 格式为“存储信息{0}中缺少实体名称为{1}的实体表映射”
        /// {0}：存储信息序列化后的内容
        /// {1}：实体名称
        /// </summary>
        public const string NotFoundEntityNameInStoreInfo = "NotFoundEntityNameInStoreInfo";
        /// <summary>
        /// 存储信息类型不正确
        /// 格式为“存储信息{0}要求的格式为{1}，发生位置{2}”
        /// {0}:存储信息内容
        /// {1}：要求的格式
        /// {2}:发生的位置
        /// </summary>
        public const string StoreInfoTypeError = "StoreInfoTypeError";
        /// <summary>
        /// 在指定存储组下找不到指定的成员
        /// 格式为“在名称为{0}的存储组下找不到名称为{1}的成员”
        /// {0}：组名称
        /// {1}：成员名称
        /// </summary>
        public const string NotFoundStoreGroupMemberInGroup = "NotFoundStoreGroupMemberInGroup";
        /// <summary>
        /// 分布式操作数据在Cancel时发生并发错误
        /// 格式为“分布式操作数据在Cancel时发生并发错误，StoreGroupName:{0},HashInfo:{1},ID:{2}”
        /// {0}：数据的StoreGroupName
        /// {1}:数据的HashInfo
        /// {2}:数据的ID
        /// </summary>
        public const string DTOperationDataConcurrenceErrorInCancel = "DTOperationDataConcurrenceErrorInCancel";

        /// <summary>
        /// 在查询获取的CrmEntity中的属性中，找不到指定名称的属性
        /// 格式为“在查询获取的CrmEntity中的属性中，找不到名称为{0}的属性，实体名称为{1}”
        /// {0}：查询的属性名称
        /// {1}：实体名称
        /// </summary>
        public const string NotFoundAttributeNameInRetrieveCrmEntity = "NotFoundAttributeNameInRetrieveCrmEntity";
        /// <summary>
        /// 在指定的实体上下文类型对象中，找不到指定实体的实体表名映射键值对
        /// 格式为“在实体上下文类型{0}的对象中，找不到实体名称为{1}的实体表名映射键值对”
        /// {0}：实体上下文对象的类型
        /// {1}：实体名称
        /// </summary>
        public const string NotFoundEntityTableMappintForDBContext = "NotFoundEntityTableMappintForDBContext";
        /// <summary>
        /// 找不到指定类型的Redis客户端生成服务
        /// 格式为“找不到类型为{0}的Redis客户端生成服务,发生位置{1}”
        /// {0}：服务类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundRedisClientGenerateServiceByType = "NotFoundRedisClientGenerateServiceByType";
        /// <summary>
        /// 生成Redis客户端错误
        /// 格式为"生成Redis客户端错误,配置信息为{0}，错误内容为{1}"
        /// {0}：配置信息
        /// {1}：错误内容
        /// </summary>
        public const string GenerateRedisClientError = "GenerateRedisClientError";
        /// <summary>
        /// 找不到指定类型的应用程序锁服务
        /// 格式为“找不到类型为{0}的应用程序锁服务，发生位置{1}”
        /// {0}：服务类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundApplicationLockServiceByType = "NotFoundApplicationLockServiceByType";
        /// <summary>
        /// 找不到指定类型的应用程序限流服务
        /// 格式为“找不到类型为{0}的应用程序限流服务，发生位置{1}”
        /// {0}：服务类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundApplicationLimitServiceByType = "NotFoundApplicationLimitServiceByType";
        /// <summary>
        /// 请求获取Redis应用程序锁超时
        /// 格式为“请求获取Redis应用程序锁超时，请求的锁名称为{0}，Redis客户端工厂名称为{1}”
        /// {0}：锁名称
        /// {1}：Redis客户端工厂名称
        /// </summary>
        public const string AcquireRedisApplicationLockExpire = "AcquireRedisApplicationLockExpire";
        /// <summary>
        /// 从Redis限流令牌桶中获取令牌超时
        /// 格式为“从Redis限流令牌桶中获取令牌超时，请求的令牌桶名称为{0}，Redis客户端工厂名称为{1}”
        /// {0}：令牌桶名称
        /// {1}：Redis客户端工厂名称
        /// </summary>
        public const string AcquireRedisLimitTokenExpire = "AcquireRedisLimitTokenExpire";
        /// <summary>
        /// 找不到指定名称的Redis客户端工厂
        /// 格式为“找不到名称为{0}的Redis客户端工厂”
        /// {0}:工厂名称
        /// </summary>
        public const string NotFoundRedisClientFactoryByName = "NotFoundRedisClientFactoryByName";
        /// <summary>
        /// 找不到指定标签的替换内容生成服务
        /// 格式为“找不到标签为{0}的替换内容生成服务，发生位置：{1}”
        /// {0}：标签名称
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundReplaceContentGenerateServiceByLabel = "NotFoundReplaceContentGenerateServiceByLabel";

        /// <summary>
        /// 在容器中找不到指定名称的消息请求响应主机服务
        /// 格式为“找不到名称为{0}的消息请求响应主机服务，发生位置：{1}”
        /// {0}：服务名称
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundSRRHostServiceInContainerByName = "NotFoundSRRHostServiceInContainerByName";
        /// <summary>
        /// 找不到指定请求类型的请求处理描述
        /// 格式为“找不到请求类型为{0}的请求处理描述，发生位置：{1}”
        /// {0}：请求类型
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundSRRRequestHandlerDescriptionByType = "NotFoundSRRRequestHandlerDescriptionByType";







        /// <summary>
        /// 实际类型与中间数据处理器要求的请求类型不匹配
        /// 格式为“实际类型{0}与中间数据处理器{1}要求的请求类型{2}不匹配”
        /// {0}：请求的实际类型
        /// {1}：处理类型
        /// {2}：要求的请求类型
        /// </summary>
        public const string MiddleDataTypeNotMatchHandler = "MiddleDataTypeNotMatchHandler";

        /// <summary>
        /// 找不到指定请求类型名称的DAX消息处理
        /// 格式为“找不到请求类型名称为{0}的DAX消息处理，发生位置：{1}”
        /// {0}：请求类型名称
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundDAXMessageHandleByRequestTypeFullName = "NotFoundDAXMessageHandleByRequestTypeFullName";
        /// <summary>
        /// 找不到指定名称的DAX服务令牌生成服务
        /// 格式为“找不到名称为{0}的DAX服务令牌生成服务，位置为{1}”
        /// {0}：服务名称
        /// {1}：发生的位置
        /// </summary>
        public const string NotFoundDAXServiceTokenGenerateServiceByName = "NotFoundDAXServiceTokenGenerateServiceByName";
        /// <summary>
        /// 在DAX服务令牌生成服务中找不到指定名称的参数
        /// 格式为“在DAX服务令牌生成服务{0}中，找不到名称为{1}的参数”
        /// {0}：服务类名
        /// {1}：参数名称
        /// </summary>
        public const string NotFoundParameterInDAXServiceTokenGenerateService = "NotFoundParameterInDAXServiceTokenGenerateService";

        /// <summary>
        ///  在DAX服务令牌生成服务中指定参数的类型不匹配
        ///  格式为“在DAX服务令牌生成服务{0}中，名称为{1}的参数期望类型为{2}，而实际类型为{3}”
        ///  {0}：服务的类名
        ///  {1}：参数名称
        ///  {2}：参数期望类型
        ///  {3}：参数实际类型
        /// </summary>
        public const string ParameterTypeNotMatchInDAXServiceTokenGenerateService = "ParameterTypeNotMatchInDAXServiceTokenGenerateService";

    }


    public static class DBTypes
    {
        public const string SqlServer = "SqlServer";
        public const string Oracle = "Oracle";
        public const string MySql = "MySql";
        public const string MongoDB = "MongoDB";
    }


    /// <summary>
    /// 系统声明类型
    /// </summary>
    public static class SystemClaimTypes
    {
        public const string User = "User";
        public const string UserId = "UserId";
        public const string Lcid = "Lcid";
        public const string TimeZoneOffset = "TimeZoneOffset";
    }

    /// <summary>
    /// 参与哈希散列的实体名称集
    /// </summary>
    public static class HashEntityNames
    {
        /// <summary>
        /// SMessageHistory,SMessageHistoryListenerDetail 共用该配置
        /// </summary>
        public const string SMessageHistory = "SMessageHistory";
        public const string SMessageHistoryListenerDetail = "SMessageHistoryListenerDetail";
        public const string ApplicationLock = "ApplicationLock";
        public const string SerialNumber = "SerialNumber";
        public const string WorkflowResource = "WorkflowResource";
        public const string WorkflowStep = "WorkflowStep";
        public const string WorkflowStepUserAction = "WorkflowStepUserAction";
        public const string WorkflowOperationLog = "WorkflowOperationLog";
        public const string Audit = "Audit";
        public const string UploadFile = "UploadFile";
        public const string ThirdPartySystemTokenRecord = "ThirdPartySystemTokenRecord";
        public const string CommonLog = "CommonLog";
        public const string DTOperationRecord = "DTOperationRecord";
    }


    /// <summary>
    /// 操作符
    /// </summary>
    public static class Operators
    {
        /// <summary>
        /// 等于
        /// </summary>
        public const string Equal = "Equal";
        /// <summary>
        /// 不等于
        /// </summary>
        public const string NotEqual = "NotEqual";
        /// <summary>
        /// 大于
        /// </summary>
        public const string GreaterThan = "GreaterThan";
        /// <summary>
        /// 大于等于
        /// </summary>
        public const string GreaterEqual = "GreaterEqual";
        /// <summary>
        /// 小于
        /// </summary>
        public const string LessThan = "LessThan";
        /// <summary>
        /// 小于等于
        /// </summary>
        public const string LessEqual = "LessEqual";
        /// <summary>
        /// 值为Null
        /// </summary>
        public const string Null = "Null";

        /// <summary>
        /// 值不为Null
        /// </summary>
        public const string NotNull = "NotNull";

    }

    /// <summary>
    /// 实体属性值类型
    /// </summary>
    public static class EntityAttributeValueTypes
    {
        /// <summary>
        /// 字符串
        /// </summary>
        public const string String = "String";
        /// <summary>
        /// 整数型
        /// </summary>
        public const string Int = "Int";
        /// <summary>
        /// 长整数型
        /// </summary>
        public const string Long = "Long";
        /// <summary>
        /// 高精度浮点型
        /// </summary>
        public const string Decimal = "Decimal";
        /// <summary>
        /// 布尔型
        /// </summary>
        public const string Bool = "Bool";
        /// <summary>
        /// 日期类型
        /// </summary>
        public const string DateTime = "DateTime";

    }

    /// <summary>
    /// 条件服务中的参数名称集
    /// </summary>
    public class ConditionServiceParameterBaseNames
    {
        /// <summary>
        /// 实体属性检查条件中的实体关键字
        /// </summary>
        public const string EntityKeyForEntityAttribute = "EntityKeyForEntityAttribute";
    }

    /// <summary>
    /// 实体属性类型集
    /// </summary>
    public static class EntityAttributeTypes
    {
        /// <summary>
        /// 布尔值
        /// </summary>
        public const string Bool = "Bool";
        /// <summary>
        /// Guid
        /// </summary>
        public const string Guid = "Guid";
        /// <summary>
        /// 整型
        /// </summary>
        public const string Int = "Int";
        /// <summary>
        /// 长整型
        /// </summary>
        public const string Long = "Long";
        /// <summary>
        /// 高精度浮点型
        /// </summary>
        public const string Decimal = "Decimal";
        /// <summary>
        /// 时间日期
        /// </summary>
        public const string DateTime = "DateTime";
        /// <summary>
        /// 可控布尔值
        /// </summary>
        public const string BoolNullAble = "BoolNullAble";
        /// <summary>
        /// 可空Guid
        /// </summary>
        public const string GuidNullAble = "GuidNullAble";
        /// <summary>
        /// 可空整型
        /// </summary>
        public const string IntNullAble = "IntNullAble";
        /// <summary>
        /// 可空长整型
        /// </summary>
        public const string LongNullAble = "LongNullAble";
        /// <summary>
        /// 可空高精度浮点型
        /// </summary>
        public const string DecimalNullAble = "DecimalNullAble";
        /// <summary>
        /// 可空时间日期
        /// </summary>
        public const string DateTimeNullAble = "DateTimeNullAble";
        /// <summary>
        /// 字符串
        /// </summary>
        public const string String = "String";
        /// <summary>
        /// 实体
        /// </summary>
        public const string Entity = "Entity";
    }

    /// <summary>
    /// 通用审批配置节点状态集
    /// </summary>
    public static class CommonSignConfigurationNodeStatus
    {
        /// <summary>
        /// 可用
        /// </summary>
        public const int Enabled = 0;
        /// <summary>
        /// 不可用
        /// </summary>
        public const int Disabled = 1;
    }

    /// <summary>
    /// 应用程序锁名称集
    /// </summary>
    public static class ApplicationLockBaseNames
    {
        /// <summary>
        /// 针对通用审批配置的操作
        /// {0}：EntityType
        /// {1}：EntityKey
        /// </summary>
        public const string CommonSignConfiguration = "CommonSignConfiguration-{0}-{1}";

    }

    /// <summary>
    /// 内存缓存名称集
    /// </summary>
    public static class MemoryCacheNames
    {
        /// <summary>
        /// Crm服务工厂
        /// </summary>
        public const string CrmServiceFactory = "CrmServiceFactory";
    }

    /// <summary>
    /// Crm服务令牌生成服务中的参数名称集
    /// </summary>
    public static class CrmServiceTokenGenerateServiceParameterNames
    {
        public const string BaseUri = "BaseUri";
        public const string ApplicationId = "ApplicationId";
        public const string ApplicationKey = "ApplicationKey";
        public const string CrmUrl = "CrmUrl";
        public const string AADId = "AADId";

        public const string AdfsUrl = "AdfsUrl";
        public const string ClientId = "ClientId";
        public const string RedirectUri = "RedirectUri";
        public const string UserName = "UserName";
        public const string Password = "Password";

        public const string ClientSecret = "ClientSecret";

        public const string Domain = "Domain";
    }

    public static class CrmServiceTokenGenerateServiceNames
    {
        /// <summary>
        /// 基于AD的基本认证模式
        /// </summary>
        public const string AD = "AD";
        /// <summary>
        /// 基于ADFS Oauth的Code模式
        /// </summary>
        public const string ADFS = "ADFS";
        /// <summary>
        /// 基于ADFS Oauth的Password模式
        /// </summary>
        public const string ADFSPassword = "ADFSPassword";
        /// <summary>
        /// 基于AAD Oauth的客户端模式
        /// </summary>
        public const string S2S = "S2S";
    }


    public static class CrmWebApiResponseKeyNames
    {
        public const string RetryAfter = "Retry-After";
    }


    /// <summary>
    /// Crm查询结果JToken处理的附加参数名称集
    /// </summary>
    public static class CrmRetrieveJTokenHandleExtensionParameterNames
    {
        public const string EntityName = "EntityName";
    }

    /// <summary>
    /// 通用队列类型集
    /// </summary>
    public static class CommonQueueTypes
    {
        /// <summary>
        /// Azure服务总线
        /// </summary>
        public const string AzureServiceBus = "AzureServiceBus";
    }

    /// <summary>
    /// Azure服务总线消息转换To服务名称集
    /// </summary>
    public static class AzureServiceBusMessageConvertToServiceNames
    {
        public const string Default = "Default";
    }

    /// <summary>
    /// Azure服务总线消息转换From服务名称集
    /// </summary>
    public static class AzureServiceBusMessageConvertFromServiceNames
    {
        public const string Default = "Default";
    }

    /// <summary>
    /// 通用异常数据关键字集合
    /// </summary>
    public static class UtilityExceptionDataKeys
    {
        public const string Catch = "Catch";
    }

    /// <summary>
    /// KV缓存类型集合
    /// </summary>
    public static class KVCacheTypes
    {
        /// <summary>
        /// 本地超时
        /// </summary>
        public const string LocalTimeout = "LocalTimeout";
        /// <summary>
        /// 本地版本号
        /// </summary>
        public const string LocalVersion = "LocalVersion";
        /// <summary>
        /// 组合
        /// </summary>
        public const string Combination = "Combination";

    }

}
