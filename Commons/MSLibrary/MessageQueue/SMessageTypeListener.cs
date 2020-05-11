using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using MSLibrary.DI;
using MSLibrary.Security;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;
using MSLibrary.MessageQueue.DAL;

namespace MSLibrary.MessageQueue
{
    /// <summary>
    /// 消息类型监听实体
    /// 存储要进行监听的应用程序信息
    /// 支持两种模式的监听程序
    /// 0：指定实现特定接口的类型工厂
    /// 1：指定实现特定Rest标准的web接口
    /// </summary>
    public class SMessageTypeListener : EntityBase<ISMessageTypeListenerIMP>
    {
        private static IFactory<ISMessageTypeListenerIMP> _sMessageTypeListenerIMPFactory;

        public static IFactory<ISMessageTypeListenerIMP> SMessageTypeListenerIMPFactory
        {
            set { _sMessageTypeListenerIMPFactory = value; }
        }

        public override IFactory<ISMessageTypeListenerIMP> GetIMPFactory()
        {
            return _sMessageTypeListenerIMPFactory;
        }


        /// <summary>
        /// id
        /// </summary>
        public Guid ID
        {
            get
            {
                return GetAttribute<Guid>("ID");
            }
            set
            {
                SetAttribute<Guid>("ID", value);
            }
        }

        /// <summary>
        /// 要监听的消息类型
        /// </summary>
        public SMessageExecuteType MessageType
        {
            get
            {
                return GetAttribute<SMessageExecuteType>("MessageType");
            }
            set
            {
                SetAttribute<SMessageExecuteType>("MessageType", value);
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {
                return GetAttribute<string>("Name");
            }
            set
            {
                SetAttribute<string>("Name", value);
            }
        }


        /// <summary>
        /// 关联队列组名称
        /// </summary>
        public string QueueGroupName
        {
            get
            {
                return GetAttribute<string>("QueueGroupName");
            }
            set
            {
                SetAttribute<string>("QueueGroupName", value);
            }
        }



        /// <summary>
        /// 监听模式
        /// 0：指定实现特定接口的类型工厂
        /// 1：指定实现特定Rest标准的web接口
        /// </summary>
        public int Mode
        {
            get
            {
                return GetAttribute<int>("Mode");
            }
            set
            {
                SetAttribute<int>("Mode", value);
            }
        }

        /// <summary>
        /// 要执行的监听工厂类型的全名称（包含程序集名称），只有Mode为0时才有意义
        /// 该类型必须实现IFactory<ISMessageListener>接口
        /// </summary>
        public string ListenerFactoryType
        {
            get
            {
                return GetAttribute<string>("ListenerFactoryType");
            }
            set
            {
                SetAttribute<string>("ListenerFactoryType", value);
            }
        }
        /// <summary>
        /// 监听工厂类型的实例化是否使用DI容器，只有Mode为0时才有意义
        /// 如果为false，则通过反射创建，这时实现类型必须是无参构造函数
        /// </summary>
        public bool? ListenerFactoryTypeUseDI
        {
            get
            {
                return GetAttribute<bool?>("ListenerFactoryTypeUseDI");
            }
            set
            {
                SetAttribute<bool?>("ListenerFactoryTypeUseDI", value);
            }
        }

        /// <summary>
        /// 要执行的web接口的地址，只有Mode为1时才有意义
        /// 执行程序将Post消息和签名到指定地址
        /// </summary>
        public string ListenerWebUrl
        {
            get
            {
                return GetAttribute<string>("ListenerWebUrl");
            }
            set
            {
                SetAttribute<string>("ListenerWebUrl", value);
            }
        }
        /// <summary>
        /// Post消息到web接口地址时要用到的签名密钥
        /// 用来验证是否合法
        /// </summary>
        public string ListenerWebSignature
        {
            get
            {
                return GetAttribute<string>("ListenerWebSignature");
            }
            set
            {
                SetAttribute<string>("ListenerWebSignature", value);
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>("CreateTime");
            }
            set
            {
                SetAttribute<DateTime>("CreateTime", value);
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>("ModifyTime");
            }
            set
            {
                SetAttribute<DateTime>("ModifyTime", value);
            }
        }

        /// <summary>
        /// 在模式为0的情况下调用ISMessageListener实现类的方法
        /// 在模式为1的情况下Post消息到监听地址
        /// </summary>
        /// <returns></returns>
        public async Task<ValidateResult> PostToListener(SMessage message)
        {
            return await _imp.PostToListener(this, message);
        }
    }


    public interface ISMessageTypeListenerIMP
    {
        Task<ValidateResult> PostToListener(SMessageTypeListener listener, SMessage message);
    }

    [Injection(InterfaceType = typeof(ISMessageTypeListenerIMP), Scope = InjectionScope.Transient)]
    public class SMessageTypeListenerIMP : ISMessageTypeListenerIMP
    {
        private static Dictionary<string, IFactory<ISMessageListener>> _listenerFactorys = new Dictionary<string, IFactory<ISMessageListener>>();
        private ISecurityService _securityService;

        public SMessageTypeListenerIMP(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        public async Task<ValidateResult> PostToListener(SMessageTypeListener listener, SMessage message)
        {
            //检查监听者模式
            //不同的模式执行不同的处理
            if (listener.Mode==0)
            {
                return await PostToListenerByMode1(listener, message);
            }
            else
            {
                return await PostToListenerByMode2(listener, message);
            }
        }


        /// <summary>
        /// 通过模式1提交到监听者
        /// 检查是否在缓存中已经存在相应的工厂类,
        //  如果不存在，根据ListenerFactoryTypeUseDI决定是通过DIContainerContainer获取工厂类，
        //  还是使用反射创建工厂类
        //  将创建出来的工厂类缓存在缓存中
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task<ValidateResult> PostToListenerByMode1(SMessageTypeListener listener, SMessage message)
        {
            if (!_listenerFactorys.TryGetValue(listener.ListenerFactoryType, out IFactory<ISMessageListener> listenerFactory))
            {
                lock (_listenerFactorys)
                {
                    Type listenerFactoryType = Type.GetType(listener.ListenerFactoryType);

                    if (!_listenerFactorys.TryGetValue(listener.ListenerFactoryType, out listenerFactory))
                    {
                        object objListenerFactory;
                        if (listener.ListenerFactoryTypeUseDI==true)
                        {
                            //通过DI容器创建
                            //objListenerFactory = DIContainerContainer.Get(listenerFactoryType);
                            objListenerFactory = DIContainerGetHelper.Get().Get(listenerFactoryType);
                        }
                        else
                        {
                            //通过反射创建
                            objListenerFactory = listenerFactoryType.Assembly.CreateInstance(listenerFactoryType.FullName);
                        }

                        if (!(objListenerFactory is IFactory<ISMessageListener>))
                        {
                            var fragment = new TextFragment()
                            {
                                Code = TextCodes.SMessageTypeListenerTypeError,
                                DefaultFormatting = "消息类型{0}中，名称为{1}的监听者工厂的类型{2}未实现接口IFactory<ISMessageListener>",
                                ReplaceParameters = new List<object>() { listener.MessageType.Name, listener.Name, listener.ListenerFactoryType }
                            };

                            throw new UtilityException((int)Errors.SMessageTypeListenerTypeError, fragment);
                        }

                        listenerFactory = (IFactory<ISMessageListener>)objListenerFactory;
                        _listenerFactorys.Add(listener.ListenerFactoryType, listenerFactory);
                    }
                }
            }

            var listenerObj= listenerFactory.Create();

            return await listenerObj.Execute(message);
        }

        /// <summary>
        /// 通过模式2提交到监听者
        /// 通过HTTP协议Post消息到ListenerWebUrl的web接口，提交的对象类型为SMessagePostData
        /// 其中Signature为Id+Type+Data+CurrentTime的字符串格式(yyyy-MM-dd hh:mm:ss)通过ListenerWebSignature签名后的字符串
        /// 接收方需要对此进行校验
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task<ValidateResult> PostToListenerByMode2(SMessageTypeListener listener, SMessage message)
        {
            ValidateResult result = new ValidateResult()
            {
                 Result=true
            };
            var expireTime = DateTime.UtcNow.AddSeconds(300);
            var strContent = $"{message.ID.ToString()}{message.Type}{message.Data}{expireTime.ToString("yyyy-MM-dd hh:mm:ss")}";
            var strSignature=_securityService.SignByKey(strContent, listener.ListenerWebSignature);
            SMessagePostData data = new SMessagePostData()
            {
                ID = message.ID,
                ExpireTime = expireTime,
                Type = message.Type,
                Data = message.Data,
                Signature = strSignature
            };
            try
            {
                await HttpClinetHelper.PostAsync(data, listener.ListenerWebUrl);
            }
            catch(Exception ex)
            {
                result.Result = false;
                result.Description = ex.Message;
            }

            return result;
        }
    }


    /// <summary>
    /// 监听执行者接口
    /// </summary>
    public interface ISMessageListener
    {
        Task<ValidateResult> Execute(SMessage message);
    }

    /// <summary>
    /// 用于Post到监听者的web地址的数据
    /// </summary>
    [DataContract]
    public class SMessagePostData
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        [DataMember]
        public Guid ID
        {
            get;set;
        }
        /// <summary>
        /// 消息类型
        /// </summary>
        [DataMember]
        public string Type
        {
            get;set;
        }
        /// <summary>
        /// 数据内容
        /// </summary>
        [DataMember]
        public string Data
        {
            get;set;
        }
        /// <summary>
        /// 过期时间(UTC)
        /// </summary>
        [DataMember]
        public DateTime ExpireTime
        {
            get;set;
        }
        /// <summary>
        /// 签名内容
        /// </summary>
        [DataMember]
        public string Signature
        {
            get;set;
        }
    }

}
