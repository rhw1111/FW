using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Context;
using MSLibrary.LanguageTranslate;
using MSLibrary.Schedule.DAL;
using MSLibrary.Logger;
using Quartz;
using Quartz.Impl;


namespace MSLibrary.Schedule
{
    /// <summary>
    /// 调度作业组
    /// 管理各个调度作业，一个调度作业只能属于一个作业组
    /// </summary>
    public class ScheduleActionGroup : EntityBase<IScheduleActionGroupIMP>
    {
        private static IFactory<IScheduleActionGroupIMP> _scheduleActionGroupIMPFactory;

        public static IFactory<IScheduleActionGroupIMP> ScheduleActionGroupIMPFactory
        {
            set
            {
                _scheduleActionGroupIMPFactory = value;
            }
        }

        public override IFactory<IScheduleActionGroupIMP> GetIMPFactory()
        {
            return _scheduleActionGroupIMPFactory;
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
        /// 组名称
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
        /// 是否使用日志
        /// 该日志将记录每个调度动作的启动和停止
        /// </summary>
        public bool UseLog
        {
            get
            {
                return GetAttribute<bool>("UseLog");
            }
            set
            {
                SetAttribute<bool>("UseLog", value);
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
        /// 执行动作时初始化的类型
        /// </summary>
        public string ExecuteActionInitType
        {
            get
            {
                return GetAttribute<string>("ExecuteActionInitType");
            }
            set
            {
                SetAttribute<string>("ExecuteActionInitType", value);
            }
        }

        /// <summary>
        /// 执行动作时初始化的配置
        /// 不同的类型有不同的配置
        /// </summary>
        public string ExecuteActionInitConfiguration
        {
            get
            {
                return GetAttribute<string>("ExecuteActionInitConfiguration");
            }
            set
            {
                SetAttribute<string>("ExecuteActionInitConfiguration", value);
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
        /// 新增
        /// </summary>
        /// <returns></returns>
        public async Task Add()
        {
            await _imp.Add(this);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public async Task Update()
        {
            await _imp.Update(this);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public async Task Delete()
        {
            await _imp.Delete(this);
        }

        /// <summary>
        /// 加入调度动作
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public async Task AddAction(Guid actionId)
        {
            await _imp.AddAction(this, actionId);
        }

        /// <summary>
        /// 移除调度动作
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public async Task RemoveAction(Guid actionId)
        {
            await _imp.RemoveAction(this, actionId);
        }

        /// <summary>
        /// 获取该组下面指定状态的所有调度动作
        /// </summary>
        /// <param name="status"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task GetAllAction(int status, Func<ScheduleAction, Task> callback)
        {
            await _imp.GetAllAction(this, status, callback);
        }

        /// <summary>
        /// 分页获取该组下面的调度动作
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<QueryResult<ScheduleAction>> GetAction(int page, int pageSize)
        {
            return await _imp.GetAction(this, page, pageSize);
        }

        /// <summary>
        /// 开始执行组里的所有可用调度作业
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            await _imp.Start(this);
        }
        /// <summary>
        /// 停止组里正在运行的所有调度作业
        /// </summary>
        /// <returns></returns>
        public async Task Pause()
        {
            await _imp.Pause(this);
        }

        /// <summary>
        /// 恢复组里已经停止的所有调度动作
        /// </summary>
        /// <returns></returns>
        public async Task ResumeAll()
        {
            await _imp.ResumeAll(this);
        }

        /// <summary>
        /// 关闭组里正在运行的所有调度动作
        /// </summary>
        /// <returns></returns>
        public async Task Shutdown()
        {
            await _imp.Shutdown(this);
        }
    }

    public interface IScheduleActionGroupIMP
    {
        Task Add(ScheduleActionGroup group);
        Task Update(ScheduleActionGroup group);
        Task Delete(ScheduleActionGroup group);
        Task AddAction(ScheduleActionGroup group, Guid actionId);
        Task RemoveAction(ScheduleActionGroup group, Guid actionId);

        Task GetAllAction(ScheduleActionGroup group, int status, Func<ScheduleAction, Task> callback);

        Task<QueryResult<ScheduleAction>> GetAction(ScheduleActionGroup group, int page, int pageSize);
        /// <summary>
        /// 开始执行组里的所有可用调度作业
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task Start(ScheduleActionGroup group);
        /// <summary>
        /// 暂停正在执行的所有调度作业
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task Pause(ScheduleActionGroup group);
        /// <summary>
        /// 恢复被暂停的所有调度作业
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task ResumeAll(ScheduleActionGroup group);
        /// <summary>
        /// 关闭正在执行的所有调度作业
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task Shutdown(ScheduleActionGroup group);
    }

    public interface IScheduleActionInitGeneratorService
    {
        Task<IScheduleActionInit> Generator(string configiration);
    }
    public interface IScheduleActionInit
    {
        void Init();
    }

    [Injection(InterfaceType = typeof(IScheduleActionGroupIMP), Scope = InjectionScope.Transient)]
    public class ScheduleActionGroupIMP : IScheduleActionGroupIMP
    {
        /// <summary>
        /// 批处理状态
        /// 0：未运行，1：已运行，2：已暂停
        /// </summary>
        private int _scheduleStatus = 0;
        private IScheduler _scheduler;
        private IScheduleActionStore _scheduleActionStore;
        private IScheduleActionRepositoryCacheProxy _scheduleActionRepositoryCacheProxy;
        private IScheduleActionGroupStore _scheduleActionGroupStore;
        private IEnvironmentClaimGeneratorRepositoryCacheProxy _environmentClaimGeneratorRepository;
        private IClaimContextGeneratorRepositoryCacheProxy _claimContextGeneratorRepository;
        private List<ScheduleAction> _actions = new List<ScheduleAction>();

        private static string _informationCategory;
        private static string _errorCategory;

        public static IDictionary<string, IFactory<IScheduleActionInitGeneratorService>> ScheduleActionInitGeneratorServiceFactories = new Dictionary<string, IFactory<IScheduleActionInitGeneratorService>>();

        public static string InformationCategory
        {
            set
            {
                _informationCategory = value;
            }
        }

        public static string ErrorCategory
        {
            set
            {
                _errorCategory = value;
            }
        }



        public ScheduleActionGroupIMP(IScheduleActionStore scheduleActionStore, IScheduleActionRepositoryCacheProxy scheduleActionRepositoryCacheProxy, IScheduleActionGroupStore scheduleActionGroupStore, IEnvironmentClaimGeneratorRepositoryCacheProxy environmentClaimGeneratorRepository, IClaimContextGeneratorRepositoryCacheProxy claimContextGeneratorRepository)
        {
            _scheduleActionStore = scheduleActionStore;
            _scheduleActionRepositoryCacheProxy = scheduleActionRepositoryCacheProxy;
            _scheduleActionGroupStore = scheduleActionGroupStore;
            _environmentClaimGeneratorRepository = environmentClaimGeneratorRepository;
            _claimContextGeneratorRepository = claimContextGeneratorRepository;
        }

        public async Task Add(ScheduleActionGroup group)
        {
            await _scheduleActionGroupStore.Add(group);
        }

        public async Task AddAction(ScheduleActionGroup group, Guid actionId)
        {
            await _scheduleActionStore.AddActionGroupRelation(actionId, group.ID);
        }

        public async Task Delete(ScheduleActionGroup group)
        {
            await _scheduleActionStore.Delete(group.ID);
        }


        public async Task GetAllAction(ScheduleActionGroup group, int status, Func<ScheduleAction, Task> callback)
        {
            await _scheduleActionStore.QueryAllAction(group.ID, status, callback);
        }

        public async Task<QueryResult<ScheduleAction>> GetAction(ScheduleActionGroup group, int page, int pageSize)
        {
            return await _scheduleActionStore.QueryByPageGroup(group.ID, page, pageSize);
        }

        public async Task Pause(ScheduleActionGroup group)
        {
            if (_scheduleStatus == 1)
            {
                foreach (var item in _actions)
                {
                    await MainJob.Pause(item.Name);
                }

                await _scheduler.PauseAll();

                _scheduleStatus = 2;
            }
        }

        public Task RemoveAction(ScheduleActionGroup group, Guid actionId)
        {
            throw new NotImplementedException();
        }

        public async Task ResumeAll(ScheduleActionGroup group)
        {
            if (_scheduleStatus == 2)
            {
                await _scheduler.ResumeAll();

                _scheduleStatus = 1;
            }
        }

        public async Task Shutdown(ScheduleActionGroup group)
        {
            if (_scheduleStatus == 1)
            {
                foreach (var item in _actions)
                {
                    await MainJob.Shutdown(item.Name);
                }

                await _scheduler.Shutdown();

                _scheduleStatus = 0;
            }
        }

        public async Task Start(ScheduleActionGroup group)
        {
            if (_scheduleStatus == 0)
            {
                var props = new NameValueCollection();
                props["quartz.serializer.type"] = "binary";

                StdSchedulerFactory factory = new StdSchedulerFactory(props);
                _scheduler = await factory.GetScheduler();

                MainJob.ScheduleActionRepository = _scheduleActionRepositoryCacheProxy;
                MainJob.ScheduleActionInitGeneratorServiceFactories = ScheduleActionInitGeneratorServiceFactories;
                MainJob.ErrorCategory = _errorCategory;
                MainJob.InformationCategory = _informationCategory;

                _actions = new List<ScheduleAction>();

                await GetAllAction(group, 1, async (action) =>
                {
                    _actions.Add(action);

                    IJobDetail job = JobBuilder.Create<MainJob>()
                          .WithIdentity(action.Name, group.Name)
                          .UsingJobData("InitType", group.ExecuteActionInitType)
                          .UsingJobData("InitConfiguration", group.ExecuteActionInitConfiguration)
                          .UsingJobData("ActionName", action.Name)
                          .UsingJobData("GroupName", group.Name)
                          .Build();

                    ITrigger trigger = TriggerBuilder.Create()
                          .WithIdentity(action.Name, group.Name)
                          .StartNow()
                          .WithCronSchedule(action.TriggerCondition)
                          .Build();
                    await _scheduler.ScheduleJob(job, trigger);
                });

                await _scheduler.Start();
                _scheduleStatus = 1;
            }
        }

        public async Task Update(ScheduleActionGroup group)
        {
            await _scheduleActionGroupStore.Update(group);
        }




        /// <summary>
        /// 范围初始化
        /// </summary>
        /*private class JobScopeInitPlugin : IJobListener
        {
            private string _environmentClaimGeneratorName;
            private string _claimContextGeneratorName;
            private IEnvironmentClaimGeneratorRepository _environmentClaimGeneratorRepository;
            private IClaimContextGeneratorRepository _claimContextGeneratorRepository;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="environmentClaimGeneratorName"></param>
            /// <param name="claimContextGeneratorName"></param>
            /// <param name="environmentClaimGeneratorRepository"></param>
            /// <param name="claimContextGeneratorRepository"></param>
            public JobScopeInitPlugin(string environmentClaimGeneratorName, string claimContextGeneratorName, IEnvironmentClaimGeneratorRepository environmentClaimGeneratorRepository, IClaimContextGeneratorRepository claimContextGeneratorRepository)
            {
                _environmentClaimGeneratorName = environmentClaimGeneratorName;
                _claimContextGeneratorName = claimContextGeneratorName;
                _environmentClaimGeneratorName = environmentClaimGeneratorName;
                _claimContextGeneratorName = claimContextGeneratorName;
            }
            public string Name => "JobScopeInit";

            public async Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
            {
            }

            public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
            {
                var environmentClaimGenerator = await _environmentClaimGeneratorRepository.QueryByName(_environmentClaimGeneratorName);
                var claimContextGenerator = await _claimContextGeneratorRepository.QueryByName(_claimContextGeneratorName);



                if (environmentClaimGenerator == null)
                {
                    throw new UtilityException((int)Errors.NotFoundEnvironmentClaimGeneratorByName, string.Format(StringLanguageTranslate.Translate(TextCodes.NotFoundEnvironmentClaimGeneratorByName, "没有找到名称为{0}的上下文生成器"), _environmentClaimGeneratorName));
                }

                if (claimContextGenerator == null)
                {
                    throw new UtilityException((int)Errors.NotFoundClaimContextGeneratorByName, string.Format(StringLanguageTranslate.Translate(TextCodes.NotFoundClaimContextGeneratorByName, "没有找到名称为{0}的上下文生成器"), _claimContextGeneratorName));
                }

                var claims= await environmentClaimGenerator.Generate();
                claimContextGenerator.ContextInit(claims.Claims);

            }

            public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default(CancellationToken))
            {

            }
        }*/


        [DisallowConcurrentExecution]
        private class MainJob : IJob
        {
            private static bool _useLog;
            private static string _informationCategory;
            private static string _errorCategory;


            private static IScheduleActionRepositoryCacheProxy _scheduleActionRepository;
            private static Dictionary<string, ScheduleActionRunStatus> _actionRunStatuses = new Dictionary<string, ScheduleActionRunStatus>();

            private static IDictionary<string, IFactory<IScheduleActionInitGeneratorService>> _scheduleActionInitGeneratorServiceFactories;

            public static IDictionary<string, IFactory<IScheduleActionInitGeneratorService>> ScheduleActionInitGeneratorServiceFactories
            {
                set
                {
                    _scheduleActionInitGeneratorServiceFactories = value;
                }
            }

            public static bool UseLog
            {
                set
                {
                    _useLog = value;
                }
            }

            public static string InformationCategory
            {
                set
                {
                    _informationCategory = value;
                }
            }

            public static string ErrorCategory
            {
                set
                {
                    _errorCategory = value;
                }
            }

            public static IScheduleActionRepositoryCacheProxy ScheduleActionRepository
            {
                set
                {
                    _scheduleActionRepository = value;
                }
            }


            private IScheduleActionInitGeneratorService getInitGeneratorService(string type)
            {
                if (!_scheduleActionInitGeneratorServiceFactories.TryGetValue(type, out IFactory<IScheduleActionInitGeneratorService> serviceFactory))
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.NotFoundScheduleActionInitServiceByType,
                        DefaultFormatting = "找不到类型为{0}的调度动作初始化服务，发生位置为{1}",
                        ReplaceParameters = new List<object>() { type, $"{typeof(ScheduleActionGroupIMP).FullName}.ScheduleActionInitGeneratorServiceFactories" }
                    };

                    throw new UtilityException((int)Errors.NotFoundScheduleActionInitServiceByType, fragment);
                }

                return serviceFactory.Create();

            }



            public async Task Execute(IJobExecutionContext context)
            {
                //获取调度动作名称
                var actionName = context.JobDetail.JobDataMap.GetString("ActionName");
                //获取调度动作名称
                var groupName = context.JobDetail.JobDataMap.GetString("GroupName");
                //获取初始化类型
                var initType = context.JobDetail.JobDataMap.GetString("InitType");
                //获取初始化配置
                var initConfiguration = context.JobDetail.JobDataMap.GetString("InitConfiguration");


                try
                {
                    if (!_actionRunStatuses.TryGetValue(actionName, out ScheduleActionRunStatus runStatus))
                    {
                        var scheduleAction = await _scheduleActionRepository.QueryByName(actionName);
                        if (scheduleAction == null)
                        {
                            var fragment = new TextFragment()
                            {
                                Code = TextCodes.NotFoundScheduleActionByName,
                                DefaultFormatting = "找不到名称为{0}的调度动作",
                                ReplaceParameters = new List<object>() { actionName }
                            };

                            throw new UtilityException((int)Errors.NotFoundScheduleActionByName, fragment);
                        }
                        runStatus = new ScheduleActionRunStatus() { Action = scheduleAction, Status = 0 };
                        _actionRunStatuses.Add(actionName, runStatus);
                    }


                    if (runStatus.Status == 0)
                    {
                        /* var environmentClaimGenerator = await _environmentClaimGeneratorRepository.QueryByName(_environmentClaimGeneratorName);
                         var claimContextGenerator = await _claimContextGeneratorRepository.QueryByName(_claimContextGeneratorName);



                         if (environmentClaimGenerator == null)
                         {
                             var fragment = new TextFragment()
                             {
                                 Code = TextCodes.NotFoundEnvironmentClaimGeneratorByName,
                                 DefaultFormatting = "没有找到名称为{0}的上下文生成器",
                                 ReplaceParameters = new List<object>() { _environmentClaimGeneratorName }
                             };

                             throw new UtilityException((int)Errors.NotFoundEnvironmentClaimGeneratorByName, fragment);
                         }

                         if (claimContextGenerator == null)
                         {
                             var fragment = new TextFragment()
                             {
                                 Code = TextCodes.NotFoundClaimContextGeneratorByName,
                                 DefaultFormatting = "没有找到名称为{0}的上下文生成器",
                                 ReplaceParameters = new List<object>() { _claimContextGeneratorName }
                             };

                             throw new UtilityException((int)Errors.NotFoundClaimContextGeneratorByName, fragment);
                         }

                         var claims = await environmentClaimGenerator.Generate();
                         claimContextGenerator.ContextInit(claims.Claims);
                         */
                        using (var diContainer = DIContainerContainer.CreateContainer())
                        {
                            var orginialDI= ContextContainer.GetValue<IDIContainer>("DI");
                            try
                            {
                                ContextContainer.SetValue<IDIContainer>("DI", diContainer);

                                var initGeneratorService = getInitGeneratorService(initType);
                                var initService = await initGeneratorService.Generator(initConfiguration);

                                initService.Init();

                                if (_useLog)
                                {
                                    LoggerHelper.LogInformation(_informationCategory, $"ScheduleAction {actionName} in Grioup {groupName} start");
                                }
                                var actionResult = await runStatus.Action.Execute();
                                runStatus.Result = actionResult;

                                if (runStatus.Result.Polling)
                                {
                                    runStatus.Status = 1;
                                }
                                else
                                {
                                    await runStatus.Result.Stop();
                                    if (_useLog)
                                    {
                                        LoggerHelper.LogInformation(_informationCategory, $"ScheduleAction {actionName} in Grioup {groupName} stop");
                                    }
                                }
                            }
                            finally
                            {
                                ContextContainer.SetValue<IDIContainer>("DI", orginialDI);
                            }

                        }


                    }
                }
                catch (Exception ex)
                {
                    LoggerHelper.LogError(_errorCategory, $"ScheduleAction {actionName} in Grioup {groupName} start error,detail:{ex.Message},stacktrace:{ex.StackTrace}");
                    //throw;
                }
            }


            public static async Task Pause(string actionName)
            {
                if (_actionRunStatuses.TryGetValue(actionName, out ScheduleActionRunStatus runStatus))
                {
                    if (runStatus.Status == 1)
                    {
                        try
                        {
                            await runStatus.Result.Stop();
                        }
                        catch (Exception ex)
                        {
                            LoggerHelper.LogInformation(_errorCategory, $"ScheduleAction {actionName} stop error,detail:{ex.Message},stacktrace:{ex.StackTrace}");
                            throw;
                        }

                        runStatus.Status = 0;
                        if (_useLog)
                        {
                            LoggerHelper.LogInformation(_informationCategory, $"ScheduleAction {actionName} stop");
                        }
                    }
                }
            }


            public static async Task Shutdown(string actionName)
            {
                if (_actionRunStatuses.TryGetValue(actionName, out ScheduleActionRunStatus runStatus))
                {
                    if (runStatus.Status == 1)
                    {
                        try
                        {
                            await runStatus.Result.Stop();
                        }
                        catch (Exception ex)
                        {
                            LoggerHelper.LogInformation(_errorCategory, $"ScheduleAction {actionName} stop error,detail:{ex.Message},stacktrace:{ex.StackTrace}");
                            throw;
                        }

                        runStatus.Status = 0;
                        if (_useLog)
                        {
                            LoggerHelper.LogInformation(_informationCategory, $"ScheduleAction {actionName} stop");
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 调度动作运行状态
        /// </summary>
        private class ScheduleActionRunStatus
        {
            /// <summary>
            /// 调度动作
            /// </summary>
            public ScheduleAction Action { get; set; }
            /// <summary>
            /// 调度动作运行结果
            /// </summary>
            public IScheduleActionResult Result { get; set; }
            /// <summary>
            /// 执行状态
            /// 0：未运行，1：已运行
            /// </summary>
            public int Status { get; set; }
        }
    }





}
