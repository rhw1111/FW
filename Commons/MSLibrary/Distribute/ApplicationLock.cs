using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Distribute
{
    /// <summary>
    /// 分布式锁
    /// </summary>
    public class ApplicationLock : EntityBase<IApplicationLockIMP>
    {
        private static IFactory<IApplicationLockIMP> _applicationLockIMPFactory;

        public static IFactory<IApplicationLockIMP> ApplicationLockIMPFactory
        {
            set
            {
                _applicationLockIMPFactory = value;
            }
        }
        public override IFactory<IApplicationLockIMP> GetIMPFactory()
        {
            return _applicationLockIMPFactory;
        }

        /// <summary>
        /// Id
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
        /// 类型
        /// </summary>
        public string Type
        {
            get
            {
                return GetAttribute<string>("Type");
            }
            set
            {
                SetAttribute<string>("Type", value);
            }
        }

        /// <summary>
        /// 类型对应配置
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
        /// 串行化执行action(同步)
        /// </summary>
        /// <param name="lockName">资源名称</param>
        /// <param name="expireMillisecond">超时毫秒数，超过时间未获得锁，则抛出异常</param>
        /// <param name="maxLockMillisecond">资源最大锁定时间</param>
        /// <param name="action"></param>
        public void LockSync(string lockName, int expireMillisecond, int maxLockMillisecond, Action action)
        {
            _imp.LockSync(this,lockName,expireMillisecond,maxLockMillisecond, action);
        }
        /// <summary>
        /// 串行化执行action(同步)
        /// </summary>
        /// <param name="lockName">资源名称</param>
        /// <param name="expireMillisecond">超时毫秒数，超过时间未获得锁，则抛出异常</param>
        /// <param name="maxLockMillisecond">资源最大锁定时间</param>
        /// <param name="action"></param>
        public async Task Lock( string lockName, int expireMillisecond, int maxLockMillisecond, Func<Task> action)
        {
            await _imp.Lock(this, lockName,expireMillisecond,maxLockMillisecond, action);
        }
    }

    public interface IApplicationLockIMP
    {
        /// <summary>
        /// 同步锁
        /// </summary>
        /// <param name="appLock"></param>
        /// <param name="lockName"></param>
        /// <param name="action"></param>
        void LockSync(ApplicationLock appLock,string lockName, int expireMillisecond,int maxLockMillisecond, Action action);
        /// <summary>
        /// 异步锁
        /// </summary>
        /// <param name="appLock"></param>
        /// <param name="lockName"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        Task Lock(ApplicationLock appLock, string lockName, int expireMillisecond, int maxLockMillisecond, Func<Task> action);
    }

    public interface IApplicationLockService
    {
        void LockSync(string configuration, string lockName,int expireMillisecond, int maxLockMillisecond, Action action);
        Task Lock(string configuration, string lockName, int expireMillisecond, int maxLockMillisecond, Func<Task> action);
    }


    [Injection(InterfaceType = typeof(IApplicationLockIMP), Scope = InjectionScope.Transient)]
    public class ApplicationLockIMP : IApplicationLockIMP
    {
        public static IDictionary<string, IFactory<IApplicationLockService>> ApplicationLockServiceFactories { get; } = new Dictionary<string, IFactory<IApplicationLockService>>();
        public async Task Lock(ApplicationLock appLock, string lockName, int expireMillisecond, int maxLockMillisecond, Func<Task> action)
        {
         
            var service = getService(appLock.Type);
            await service.Lock(appLock.Configuration, lockName,expireMillisecond,maxLockMillisecond, action);
        }

        public void LockSync(ApplicationLock appLock, string lockName, int expireMillisecond, int maxLockMillisecond, Action action)
        {
            var service = getService(appLock.Type);
            service.LockSync(appLock.Configuration, lockName,expireMillisecond,maxLockMillisecond, action);
        }

        private IApplicationLockService getService(string type)
        {
            if (!ApplicationLockServiceFactories.TryGetValue(type,out IFactory<IApplicationLockService> serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundApplicationLockServiceByType,
                    DefaultFormatting = "找不到类型为{0}的应用程序锁服务，发生位置{1}",
                    ReplaceParameters = new List<object>() { type, $"{this.GetType().FullName}.ApplicationLockServiceFactories" }
                };
                throw new UtilityException((int)Errors.NotFoundApplicationLockServiceByType, fragment);
            }

            return serviceFactory.Create(); 
        }
    }
}
