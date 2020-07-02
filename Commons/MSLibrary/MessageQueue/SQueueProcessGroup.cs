using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MSLibrary.DI;
using MSLibrary.Thread;
using MSLibrary.Logger;
using MSLibrary.MessageQueue.DAL;
using MSLibrary.Context;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.MessageQueue
{
    /// <summary>
    /// 队列执行组
    /// 负责分组管理要处理的队列，与队列的关系为一对多
    /// </summary>
    public class SQueueProcessGroup : EntityBase<ISQueueProcessGroupIMP>
    {
        private static IFactory<ISQueueProcessGroupIMP> _sQueueProcessGroupIMPFactory;

        public static IFactory<ISQueueProcessGroupIMP> SQueueProcessGroupIMPFactory
        {
            set
            {
                _sQueueProcessGroupIMPFactory = value;
            }
        }

        public override IFactory<ISQueueProcessGroupIMP> GetIMPFactory()
        {
            return _sQueueProcessGroupIMPFactory;
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
        /// 增加队列关联
        /// </summary>
        /// <param name="queueId">队列Id</param>
        /// <returns></returns>
        public async Task AddQueue(Guid queueId)
        {
            await _imp.AddQueue(this, queueId);
        }
        /// <summary>
        /// 移除队列关联
        /// </summary>
        /// <param name="queueId">队列Id</param>
        /// <returns></returns>
        public async Task RemoveQueue(Guid queueId)
        {
            await _imp.RemoveQueue(this,queueId);
        }
        /// <summary>
        /// 获取所有关联的队列
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task GetAllQueue(Func<SQueue,Task> callback)
        {
            await _imp.GetAllQueue(this, callback);
        }
        /// <summary>
        /// 分页获取关联的队列
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<QueryResult<SQueue>> GetQueue(int page, int pageSize)
        {
            return await _imp.GetQueue(this, page, pageSize);
        }
        /// <summary>
        /// 执行关联的队列
        /// </summary>
        /// <returns></returns>
        public async Task<ISQueueProcessGroupExecuteResult> Execute()
        {
            return await _imp.Execute(this);
        }


    }

    /// <summary>
    /// 队列执行组实现接口
    /// </summary>
    public interface ISQueueProcessGroupIMP
    {
        Task Add(SQueueProcessGroup group);
        Task Update(SQueueProcessGroup group);
        Task Delete(SQueueProcessGroup group);
        Task AddQueue(SQueueProcessGroup group, Guid queueId);
        Task RemoveQueue(SQueueProcessGroup group,Guid queueId);

        Task GetAllQueue(SQueueProcessGroup group,Func<SQueue, Task> callback);

        Task<QueryResult<SQueue>> GetQueue(SQueueProcessGroup group,int page, int pageSize);

        Task<ISQueueProcessGroupExecuteResult> Execute(SQueueProcessGroup group);
    }

    /// <summary>
    /// 队列执行组执行结果
    /// </summary>
    public interface ISQueueProcessGroupExecuteResult
    {
        /// <summary>
        /// 停止执行
        /// </summary>
        /// <returns></returns>
        Task Stop();
    }

    public class SQueueProcessGroupExecuteResultDefalut : ISQueueProcessGroupExecuteResult
    {
        private IAsyncPollingResult _pollingResult;
        public SQueueProcessGroupExecuteResultDefalut(IAsyncPollingResult pollingResult)
        {
            _pollingResult = pollingResult;
        }
        public async Task Stop()
        {
            await _pollingResult.Stop();
            await Task.FromResult(0);
        }
    }

    [Injection(InterfaceType = typeof(ISQueueProcessGroupIMP), Scope = InjectionScope.Transient)]
    public class SQueueProcessGroupIMP : ISQueueProcessGroupIMP
    {
        /// <summary>
        /// 错误日志的目录名称
        /// </summary>
        public static string ErrorLoggerCategoryName
        {
            get;set;
        }
        /// <summary>
        /// 管理员环境声明生成器名称
        /// </summary>
        public static string AdministratorClaimGeneratorName
        {
            get; set;
        }

        /// <summary>
        /// 管理员声明上下文生成器名称
        /// </summary>
        public static string AdministratorClaimContextGeneratorName
        {
            get; set;
        }


        private ILoggerFactory _loggerFactory;
        private ISMessageRepository _smessageRepository;
        private ISQueueProcessGroupStore _sQueueProcessGroupStore;
        private ISQueueStore _squeueStore;
        private IEnvironmentClaimGeneratorRepository _environmentClaimGeneratorRepository;
        private IClaimContextGeneratorRepository _claimContextGeneratorRepository;

        public SQueueProcessGroupIMP(ILoggerFactory loggerFactory, ISMessageRepository smessageRepository, ISQueueProcessGroupStore sQueueProcessGroupStore, ISQueueStore squeueStore, IEnvironmentClaimGeneratorRepository environmentClaimGeneratorRepository, IClaimContextGeneratorRepository claimContextGeneratorRepository)
        {
            _loggerFactory = loggerFactory;
            _smessageRepository = smessageRepository;
            _sQueueProcessGroupStore = sQueueProcessGroupStore;
            _squeueStore = squeueStore;
            _environmentClaimGeneratorRepository = environmentClaimGeneratorRepository;
            _claimContextGeneratorRepository = claimContextGeneratorRepository;
        }

        public async Task Add(SQueueProcessGroup group)
        {
            await _sQueueProcessGroupStore.Add(group);
        }

        public async Task Update(SQueueProcessGroup group)
        {
            await _sQueueProcessGroupStore.Update(group);
        }

        public async Task Delete(SQueueProcessGroup group)
        {
            await _sQueueProcessGroupStore.Delete(group.ID);
        }

        public async Task AddQueue(SQueueProcessGroup group, Guid queueId)
        {
            await _squeueStore.AddProcessGroupRelation(group.ID, queueId);
        }
        public async Task GetAllQueue(SQueueProcessGroup group, Func<SQueue, Task> callback)
        {
            await _squeueStore.QueryByProceeGroup(group.ID, callback);
        }

        public async Task<QueryResult<SQueue>> GetQueue(SQueueProcessGroup group,int page, int pageSize)
        {
            return await _squeueStore.QueryByProceeGroup(group.ID, page, pageSize);
        }

        public async Task RemoveQueue(SQueueProcessGroup group, Guid queueId)
        {
            await _squeueStore.DeleteProcessGroupRelation(group.ID,queueId);
        }

        /// <summary>
        /// 执行所有关联的队列
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task<ISQueueProcessGroupExecuteResult> Execute(SQueueProcessGroup group)
        {
        
            //声明一个轮询配置列表，队列的执行通过轮询处理帮助器管理，保证只有一个主控线程被占用
            List<PollingConfiguration> pollingConfigurations = new List<PollingConfiguration>();

            await GetAllQueue(group, async (queue) =>
            {


                pollingConfigurations.Add(new PollingConfiguration()
                {
                    Action = async () =>
                    {


                        try
                        {
                            if (AdministratorClaimGeneratorName != null && AdministratorClaimContextGeneratorName != null)
                            {
                                //生成上下文
                                var administratorClaimGenerator = await _environmentClaimGeneratorRepository.QueryByName(AdministratorClaimGeneratorName);
                                if (administratorClaimGenerator == null)
                                {
                                    var fragment = new TextFragment()
                                    {
                                        Code = TextCodes.NotFoundEnvironmentClaimGeneratorByName,
                                        DefaultFormatting = "没有找到名称为{0}的环境声明生成器",
                                        ReplaceParameters = new List<object>() { AdministratorClaimGeneratorName }
                                    };

                                    throw new UtilityException((int)Errors.NotFoundEnvironmentClaimGeneratorByName, fragment);
                                }

                                var claims = await administratorClaimGenerator.Generate();

                                var administratorClaimContextGenerator = await _claimContextGeneratorRepository.QueryByName(AdministratorClaimContextGeneratorName);
                                if (administratorClaimContextGenerator==null)
                                {
                                    var fragment = new TextFragment()
                                    {
                                        Code = TextCodes.NotFoundClaimContextGeneratorByName,
                                        DefaultFormatting = "没有找到名称为{0}的上下文生成器",
                                        ReplaceParameters = new List<object>() { AdministratorClaimContextGeneratorName }
                                    };

                                    throw new UtilityException((int)Errors.NotFoundClaimContextGeneratorByName, fragment);
                                }

                                administratorClaimContextGenerator.ContextInit(claims.Claims);
                            }

                            ConcurrentDictionary<Guid, Guid> errorMessageList = new ConcurrentDictionary<Guid, Guid>();

                            //获取队列中的消息
                            await _smessageRepository.QueryAllByQueue(queue, 500, async (messages) =>
                            {
                                bool needRestart = false;


                                foreach (var message in messages)
                                {
                                    StatusResult executeResult=new StatusResult() {  Status=2};

                                    try
                                    {
                                        using (var diContainer = DIContainerContainer.CreateContainer())
                                        {
                                            var orginialDI = ContextContainer.GetValue<IDIContainer>("DI");
                                            try
                                            {
                                                ContextContainer.SetValue<IDIContainer>("DI", diContainer);
                                                //对每个消息执行处理
                                                executeResult = await message.Execute();
                                            }
                                            finally
                                            {
                                                ContextContainer.SetValue<IDIContainer>("DI", orginialDI);
                                            }
                                        }

                                    }
                                    catch(Exception ex)
                                    {
                                        while (ex.InnerException != null)
                                        {
                                            ex = ex.InnerException;
                                        }
                                        //if (errorMessageList.Count<=100000)
                                        //{
                                            //if (!errorMessageList.ContainsKey(message.ID))
                                            //{
                                             //   errorMessageList.TryAdd(message.ID, message.ID);
                                                LoggerHelper.LogError(ErrorLoggerCategoryName,
                                                    $"SQueueProcessGroup {group.Name} Execute Error,message Type {message.Type},message id {message.ID.ToString()},ErrorMessage:{await ex.GetCurrentLcidMessage()},StackTrace:{ex.StackTrace}");
                                            //}
                                        //}
                                    }

                                    if (executeResult.Status == 0)
                                    {
                                        //执行成功
                                        needRestart = true;
                                        await message.Delete();

                                        //errorMessageList.TryRemove(message.ID, out Guid deleteId);
                                    }
                                    else
                                    {
                                        if (executeResult.Status == 3)
                                        {
                                            needRestart = true;
                                            //执行失败
                                            //LoggerHelper.LogError(ErrorLoggerCategoryName, $"SQueueProcessGroup Message Execute Error,Type:{message.Type},Key:{message.Key},Data:{message.Data},ErrorMessage:{executeResult.Description}");
                                        }
                                    }
                                }

                                if (needRestart)
                                {
                                    return await Task.FromResult(false);
                                }
                                else
                                {
                                    return await Task.FromResult(true);
                                }
                            });

                            //System.Threading.Thread.Sleep(1000);
                        }
                        catch (Exception ex)
                        {
                            //if (!errorLogRecord)
                            //{
                                while(ex.InnerException!=null)
                                {
                                    ex = ex.InnerException;
                                }
                                LoggerHelper.LogError(ErrorLoggerCategoryName,
                                    $"SQueueProcessGroup {group.Name} Execute Error,ErrorMessage:{await ex.GetCurrentLcidMessage()},StackTrace:{ex.StackTrace}");
                            //    errorLogRecord = true;
                            //}

                            await Task.Delay(1000 * 60*2);
                        }

                    },
                    Interval = queue.Interval
                }
                    );

                await Task.FromResult(0);
            });

            var pollingResult=PollingHelper.Polling(pollingConfigurations,
                async(ex)=>
                {
                    await Task.CompletedTask;
                    LoggerHelper.LogError(ErrorLoggerCategoryName,
                        $"PollingHelper Execute Error,ErrorMessage:{ex.Message},StackTrace:{ex.StackTrace}");
                }
                );
            return new SQueueProcessGroupExecuteResultDefalut(pollingResult);
        }

    }
}
