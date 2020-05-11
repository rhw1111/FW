using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Security;
using MSLibrary.Schedule.DAL;

namespace MSLibrary.Schedule
{
    /// <summary>
    /// 调度作业
    /// </summary>
    public class ScheduleAction : EntityBase<IScheduleActionIMP>
    {
        private static IFactory<IScheduleActionIMP> _scheduleActionIMPFactory;

        public static IFactory<IScheduleActionIMP> ScheduleActionIMPFactory
        {
            set
            {
                _scheduleActionIMPFactory = value;
            }
        }

        public override IFactory<IScheduleActionIMP> GetIMPFactory()
        {
            return _scheduleActionIMPFactory;
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
        /// 作业名称
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
        /// 触发条件
        /// </summary>
        public string TriggerCondition
        {
            get
            {
                return GetAttribute<string>("TriggerCondition");
            }
            set
            {
                SetAttribute<string>("TriggerCondition", value);
            }
        }

        /// <summary>
        /// 提供给具体服务使用的配置
        /// 不同服务有不同的格式
        /// </summary>
        public string Configuration
        {
            get
            {
                return GetAttribute<string>("Configuration");
            }
            set
            {
                SetAttribute<string>("Configuration", value);
            }
        }


        /// <summary>
        /// 动作调用模式
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
        /// 要执行的调度作业服务工厂类型的全名称（包含程序集名称）
        /// 该类型必须实现IFactory<IScheduleActionService>接口
        /// </summary>
        public string ScheduleActionServiceFactoryType
        {
            get
            {
                return GetAttribute<string>("ScheduleActionServiceFactoryType");
            }
            set
            {
                SetAttribute<string>("ScheduleActionServiceFactoryType", value);
            }
        }

        /// <summary>
        /// 调度作业服务工厂类型的实例化是否使用DI容器，只有Mode为0时才有意义
        /// 如果为false，则通过反射创建，这时实现类型必须是无参构造函数
        /// </summary>
        public bool? ScheduleActionServiceFactoryTypeUseDI
        {
            get
            {
                return GetAttribute<bool?>("ScheduleActionServiceFactoryTypeUseDI");
            }
            set
            {
                SetAttribute<bool?>("ScheduleActionServiceFactoryTypeUseDI", value);
            }
        }

        /// <summary>
        /// 要执行的web接口的地址，只有Mode为1时才有意义
        /// 执行程序将Post消息和签名到指定地址
        /// </summary>
        public string ScheduleActionServiceWebUrl
        {
            get
            {
                return GetAttribute<string>("ScheduleActionServiceWebUrl");
            }
            set
            {
                SetAttribute<string>("ScheduleActionServiceWebUrl", value);
            }
        }
        /// <summary>
        /// Post消息到web接口地址时要用到的签名密钥
        /// 用来验证是否合法
        /// </summary>
        public string WebSignature
        {
            get
            {
                return GetAttribute<string>("WebSignature");
            }
            set
            {
                SetAttribute<string>("WebSignature", value);
            }
        }

        /// <summary>
        /// 状态
        /// 0：不可用
        /// 1：可用
        /// </summary>
        public int Status
        {
            get
            {
                return GetAttribute<int>("Status");
            }
            set
            {
                SetAttribute<int>("Status", value);
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

        public async Task<IScheduleActionResult> Execute()
        {
            return await _imp.Execute(this);
        }

    }

    public interface IScheduleActionIMP
    {
        Task Add(ScheduleAction action);
        Task Update(ScheduleAction action);
        Task Delete(ScheduleAction action);


        /// <summary>
        /// 执行调度动作
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        Task<IScheduleActionResult> Execute(ScheduleAction action);
    }

    [Injection(InterfaceType = typeof(IScheduleActionIMP), Scope = InjectionScope.Transient)]
    public class ScheduleActionIMP : IScheduleActionIMP
    {
        private static Dictionary<string, IFactory<IScheduleActionService>> _actionServiceFactories = new Dictionary<string, IFactory<IScheduleActionService>>();

        private ISecurityService _securityService;
        private IScheduleActionStore _scheduleActionStore;

        public ScheduleActionIMP(ISecurityService securityService, IScheduleActionStore scheduleActionStore)
        {
            _securityService = securityService;
            _scheduleActionStore = scheduleActionStore;
        }

        public async Task Add(ScheduleAction action)
        {
            await _scheduleActionStore.Add(action);
        }

        public async Task Delete(ScheduleAction action)
        {
            await _scheduleActionStore.Delete(action.ID);
        }

        public async Task<IScheduleActionResult> Execute(ScheduleAction action)
        {
            //检查调度动作模式
            //不同的模式执行不同的处理
            if (action.Mode == 0)
            {
                return await ExecuteByMode1(action);
            }
            else
            {
                return await ExecuteByMode2(action);
            }
        }

        public async Task Update(ScheduleAction action)
        {
            await _scheduleActionStore.Update(action);
        }

        /// <summary>
        /// 通过模式1直接调用由工厂类创建的调度作业服务
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private async Task<IScheduleActionResult> ExecuteByMode1(ScheduleAction action)
        {
            if (!_actionServiceFactories.TryGetValue(action.ScheduleActionServiceFactoryType, out IFactory<IScheduleActionService> actionFactory))
            {
                lock (_actionServiceFactories)
                {
                    Type actionFactoryType = Type.GetType(action.ScheduleActionServiceFactoryType);

                    if (!_actionServiceFactories.TryGetValue(action.ScheduleActionServiceFactoryType, out actionFactory))
                    {
                        object objActionFactory;
                        if (action.ScheduleActionServiceFactoryTypeUseDI == true)
                        {
                            //通过DI容器创建
                            //objActionFactory = DIContainerContainer.Get(actionFactoryType);
                            objActionFactory = DIContainerGetHelper.Get().Get(actionFactoryType);
                        }
                        else
                        {
                            //通过反射创建
                            objActionFactory = actionFactoryType.Assembly.CreateInstance(actionFactoryType.FullName);
                        }

                        if (!(objActionFactory is IFactory<IScheduleActionService>))
                        {
                            var fragment = new TextFragment()
                            {
                                Code = TextCodes.ScheduleActionServiceTypeError,
                                DefaultFormatting = "调度作业{0}中，调度动作服务工厂的类型{1}未实现接口IFactory<IScheduleActionService>",
                                ReplaceParameters = new List<object>() { action.Name, action.ScheduleActionServiceFactoryType }
                            };

                            throw new UtilityException((int)Errors.ScheduleActionServiceTypeError, fragment);
                        }

                        actionFactory = (IFactory<IScheduleActionService>)objActionFactory;
                        _actionServiceFactories.Add(action.ScheduleActionServiceFactoryType, actionFactory);
                    }
                }
            }

            var actionObj = actionFactory.Create();

            return await actionObj.Execute(action.Configuration);
        }

        /// <summary>
        ///  通过模式2提交到web接口
        /// 通过HTTP协议Post消息到ScheduleActionServiceWebUrl的web接口，提交的对象类型为SMessagePostData
        /// 其中Signature为CurrentTime的字符串格式(yyyy-MM-dd hh:mm:ss)通过WebSignature签名后的字符串
        /// 接收方需要对此进行校验
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private async Task<IScheduleActionResult> ExecuteByMode2(ScheduleAction action)
        {
            ScheduleActionResultDefault result = new ScheduleActionResultDefault();
            var currentTime = DateTime.UtcNow;
            var strContent = $"{currentTime.ToString("yyyy-MM-dd hh:mm:ss")}";
            var strSignature = _securityService.SignByKey(strContent, action.WebSignature);
            await HttpClinetHelper.PostAsync(strSignature, action.ScheduleActionServiceWebUrl);
            return result;
        }
    }

    /// <summary>
    /// 调度作业服务
    /// </summary>

    public interface IScheduleActionService
    {
        Task<IScheduleActionResult> Execute(string configuration);
    }

    /// <summary>
    /// 调度作业结果
    /// </summary>
    public interface IScheduleActionResult
    {
        /// <summary>
        /// 是否是轮询操作
        /// </summary>
        /// <returns></returns>
        bool Polling { get; }
        /// <summary>
        /// 停止调度作业
        /// </summary>
        /// <returns></returns>
        Task Stop();
    }

    /// <summary>
    /// 默认调度作业结果
    /// 不做任何处理
    /// </summary>
    public class ScheduleActionResultDefault : IScheduleActionResult
    {
        public bool Polling => false;

        public async Task Stop()
        {
            await Task.FromResult(0);
        }
    }
}
