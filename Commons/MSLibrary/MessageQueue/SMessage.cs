using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using MSLibrary.Thread;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.MessageQueue.DAL;
using MSLibrary.MessageQueue.Extensions;
using MSLibrary.Transaction;

namespace MSLibrary.MessageQueue
{
    /// <summary>
    /// 消息实体
    /// 记录所有关于消息的信息
    /// </summary>
    public class SMessage : EntityBase<ISMessageIMP>
    {
        private static IFactory<ISMessageIMP> _sMessageFactory;

        public static IFactory<ISMessageIMP> SMessageFactory
        {
            set
            {
                _sMessageFactory = value;
            }
        }



        public override IFactory<ISMessageIMP> GetIMPFactory()
        {
            return _sMessageFactory;
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
        /// 消息关键字
        /// 将基于该关键字分布消息
        /// </summary>
        public string Key
        {
            get
            {
                return GetAttribute<string>("Key");
            }
            set
            {
                SetAttribute<string>("Key", value);
            }
        }

        /// <summary>
        /// 消息类型
        /// 处理器将根据不同的消息类型做不同处理
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
        /// 消息内容
        /// </summary>
        public string Data
        {
            get
            {
                return GetAttribute<string>("Data");
            }
            set
            {
                SetAttribute<string>("Data", value);
            }
        }

        /// <summary>
        /// 关联的监听ID
        /// </summary>
        public Guid? TypeListenerID
        {
            get
            {
                return GetAttribute<Guid?>("TypeListenerID");
            }
            set
            {
                SetAttribute<Guid?>("TypeListenerID", value);
            }
        }

        /// <summary>
        /// 初始消息ID
        /// </summary>
        public Guid? OriginalMessageID
        {
            get
            {
                return GetAttribute<Guid?>("OriginalMessageID");
            }
            set
            {
                SetAttribute<Guid?>("OriginalMessageID", value);
            }
        }

        /// <summary>
        /// 延迟消息所属的来源消息ID
        /// </summary>
        public Guid? DelayMessageID
        {
            get
            {
                return GetAttribute<Guid?>("DelayMessageID");
            }
            set
            {
                SetAttribute<Guid?>("DelayMessageID", value);
            }
        }



        /// <summary>
        /// 创建时间(UTC)
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
        /// 预期执行时间(UTC)
        /// 只有当前时间大于等于该时间，处理器才会执行
        /// </summary>
        public DateTime ExpectationExecuteTime
        {
            get
            {
                return GetAttribute<DateTime>("ExpectationExecuteTime");
            }
            set
            {
                SetAttribute<DateTime>("ExpectationExecuteTime", value);
            }
        }

        /// <summary>
        /// 最近一次执行时间
        /// </summary>
        public DateTime? LastExecuteTime
        {
            get
            {
                return GetAttribute<DateTime?>("LastExecuteTime");
            }
            set
            {
                SetAttribute<DateTime?>("LastExecuteTime", value);
            }
        }

        /// <summary>
        /// 重试次数
        /// </summary>
        public int RetryNumber
        {
            get
            {
                return GetAttribute<int>("RetryNumber");
            }
            set
            {
                SetAttribute<int>("RetryNumber", value);
            }
        }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ExceptionMessage
        {
            get
            {
                return GetAttribute<string>("ExceptionMessage");
            }
            set
            {
                SetAttribute<string>("ExceptionMessage", value);
            }
        }

        /// <summary>
        /// 附加信息
        /// 用来记录额外信息
        /// </summary>
        public string ExtensionMessage
        {
            get
            {
                return GetAttribute<string>("ExtensionMessage");
            }
            set
            {
                SetAttribute<string>("ExtensionMessage", value);
            }
        }

        /// <summary>
        /// 是否是死消息
        /// </summary>
        public bool IsDead
        {
            get
            {
                return GetAttribute<bool>("IsDead");
            }
            set
            {
                SetAttribute<bool>("IsDead", value);
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
        /// 删除
        /// </summary>
        /// <returns></returns>
        public async Task Delete()
        {
            await _imp.Delete(this);
        }
        /// <summary>
        /// 执行
        /// </summary>
        /// <returns>
        /// 返回状态
        /// 0：处理正确
        /// 1：尚未到执行时间
        /// 2：处理错误
        /// </returns>
        public async Task<StatusResult> Execute()
        {
            return await _imp.Execute(this);
        }
        /// <summary>
        /// 延迟当前消息
        /// </summary>
        /// <param name="delaySeconds"></param>
        /// <param name="extensionMessage"></param>
        /// <returns></returns>
        public async Task AddDelay(int delaySeconds, string extensionMessage)
        {
            await _imp.AddDelay(this, delaySeconds, extensionMessage);
        }
    }

    /// <summary>
    /// 消息具体实现接口
    /// </summary>
    public interface ISMessageIMP
    {
        Task Add(SMessage message);
        Task Delete(SMessage message);
        Task<StatusResult> Execute(SMessage message);
        /// <summary>
        /// 创建该消息的延迟消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task AddDelay(SMessage message,int delaySeconds,string extensionMessage);

    }

    [Injection(InterfaceType = typeof(ISMessageIMP), Scope = InjectionScope.Transient)]
    public class SMessageIMP : ISMessageIMP
    {
        /// <summary>
        /// 存储在SMessage的扩展属性中的队列信息的键值
        /// </summary>
        private const string _queueName = "Queue";
        private ISMessageStore _smessageStore;
        private ISQueueChooseService _sQueueChooseService;
        private ISMessageExecuteTypeRepository _smessageExecuteTypeRepository;
        private ISMessageTypeListenerPostContextFactory _smessageTypeListenerPostContextFactory;
        private ISMessageOperationContextFactory _smessageOperationContextFactory;
        private ISMessageHistoryStore _smessageHistoryStore;
        private ISMessageHistoryListenerDetailStore _smessageHistoryListenerDetailStore;
        private ISMessageExecuteTypeRepositoryCacheProxy _smessageExecuteTypeRepositoryCacheProxy;
        private IListenerMessageKeyGenerateService _listenerMessageKeyGenerateService;

        public SMessageIMP(ISMessageStore smessageStore, ISQueueChooseService sQueueChooseService, ISMessageExecuteTypeRepository smessageExecuteTypeRepository, ISMessageTypeListenerPostContextFactory smessageTypeListenerPostContextFactory, ISMessageOperationContextFactory smessageOperationContextFactory,
            ISMessageHistoryStore smessageHistoryStore, ISMessageHistoryListenerDetailStore smessageHistoryListenerDetailStore, ISMessageExecuteTypeRepositoryCacheProxy smessageExecuteTypeRepositoryCacheProxy, IListenerMessageKeyGenerateService listenerMessageKeyGenerateService

            )
        {
            _smessageStore = smessageStore;
            _sQueueChooseService = sQueueChooseService;
            _smessageExecuteTypeRepository = smessageExecuteTypeRepository;
            _smessageTypeListenerPostContextFactory = smessageTypeListenerPostContextFactory;
            _smessageOperationContextFactory = smessageOperationContextFactory;
            _smessageHistoryStore = smessageHistoryStore;
            _smessageHistoryListenerDetailStore = smessageHistoryListenerDetailStore;
            _smessageExecuteTypeRepositoryCacheProxy = smessageExecuteTypeRepositoryCacheProxy;
            _listenerMessageKeyGenerateService = listenerMessageKeyGenerateService;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task Add(SMessage message)
        {
            //根据key获取队列
            var queue=await _sQueueChooseService.Choose(message.Key);
            //执行存储
            await _smessageStore.Add(queue, message);

            //执行扩展
            var context=_smessageOperationContextFactory.Create(message, new Dictionary<string, object>());
            await MessageQueueExtensionDescription.OnSMessageAddExecuted(context);

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task Delete(SMessage message)
        {
            //尝试获取message扩展属性中存储的队列
            var queue=GetQueue(message);
            if (queue==null)
            {
                //如果扩展属性中的队列为空，则需要通过Key来获取队列
                //如果消息为死消息，则需要获取所属的死队列
                if (message.IsDead)
                {
                    queue=await _sQueueChooseService.ChooseDead(message.Key);
                }
                else
                {
                    queue = await _sQueueChooseService.Choose(message.Key);
                }
            }

            //执行删除
            await _smessageStore.Delete(queue, message.ID);
        }

        /// <summary>
        /// 执行消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns>0:成功，1:未到执行时间，2：失败，3:移除到死队列</returns>
        public async Task<StatusResult> Execute(SMessage message)
        {
            StatusResult result = new StatusResult()
            {
                Status = 0
            };

            //获取消息执行类型
            var executeType = await _smessageExecuteTypeRepositoryCacheProxy.QueryByName(message.Type);

            if (executeType==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundSMessageTypeByName,
                    DefaultFormatting = "找不到名称为{0}的消息类型",
                    ReplaceParameters = new List<object>() { message.Type }
                };
                throw new UtilityException((int)Errors.NotFoundSMessageTypeByName, fragment);
            }


            var queue = GetQueue(message);
            if (queue == null)
            {
                //如果扩展属性中的队列为空，则需要通过Key来获取队列
                queue = await _sQueueChooseService.Choose(message.Key);
            }

            //检查预计执行时间是否>=当前时间
            //如果是，则直接返回结果
            var nowUTC = DateTime.UtcNow;
            if (message.ExpectationExecuteTime >= nowUTC)
            {
                result.Status = 1;
                return await Task.FromResult(result);
            }


            //检查是否存在比期望执行时间早的相同key的消息
            //如果消息本身不是死消息，找到的消息是死消息，则直接将该消息转移到死消息队列中
            //返回结果3
            var beforeMessage = await GetBeforeExpectTimeMessage(message);
            if (beforeMessage != null)
            {
                result.Status = 1;
                if (!message.IsDead && beforeMessage.IsDead)
                {
                    var deadQueue = await _sQueueChooseService.ChooseDead(message.Key);
                    await using (var transactionScope = new DBTransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 30, 30) }))
                    {
                        await _smessageStore.Delete(queue, message.ID);
                        await _smessageStore.AddToDead(deadQueue, message);
                        result.Status = 3;
                        //执行扩展
                        var context = _smessageOperationContextFactory.Create(message, new Dictionary<string, object>());
                        await MessageQueueExtensionDescription.OnSMessageAddToDeadExecuted(context);

                        transactionScope.Complete();
                    }
                }


                return await Task.FromResult(result);
            }


            //增加消息历史
            var messageHistory = await _smessageHistoryStore.QueryById(message.ID);

            if (messageHistory == null)
            {
                messageHistory = new SMessageHistory()
                {
                    ID = message.ID,
                    CreateTime = DateTime.UtcNow,
                    ModifyTime = DateTime.UtcNow,
                    Type = message.Type,
                    Key = message.Key,
                    Data = message.Data,
                    OriginalMessageID = message.OriginalMessageID,
                    DelayMessageID = message.DelayMessageID
                };
                await messageHistory.Add();
            }

            Dictionary<Guid, ISMessageTypeListenerPostContext> postContextList = new Dictionary<Guid, ISMessageTypeListenerPostContext>();
            ISMessageTypeListenerPostContext postContext=null;



            //如果已经关联了监听，则直接执行监听的行为
            if (message.TypeListenerID.HasValue)
            {
                var listener=await executeType.GetListener(message.TypeListenerID.Value);
                if (listener==null)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TextCodes.NotFoundSMessageTypeListenerFromTypeByID,
                        DefaultFormatting = "名称为{0}的消息类型下，找不到监听ID为{1}的监听",
                        ReplaceParameters = new List<object>() { message.Type, message.TypeListenerID.Value.ToString() }
                    };

                    throw new UtilityException((int)Errors.NotFoundSMessageTypeListenerFromTypeByID, fragment);
                }

                Exception error = null;
                try
                {
                    //判断该监听是否已经完成
                    var listenerDetail = await _smessageHistoryListenerDetailStore.QueryByName(message.ID, listener.Name);
                    if (listenerDetail == null)
                    {
                        var execueResult = await listener.PostToListener(message);
                        if (!execueResult.Result)
                        {
                            throw new Exception(execueResult.Description);
                        }

                        postContext = _smessageTypeListenerPostContextFactory.Create(listener, message, null, new Dictionary<string, object>());

                        listenerDetail = new SMessageHistoryListenerDetail()
                        {
                            SMessageHistoryID = messageHistory.ID,
                            ListenerMode = listener.Mode,
                            ListenerFactoryType = listener.ListenerFactoryType,
                            ListenerName = listener.Name,
                            ListenerRealWebUrl = string.Empty,
                            ListenerWebUrl = listener.ListenerWebUrl,

                        };

                        await messageHistory.AddListenerDetail(listenerDetail);
                    }

                }
                catch(Exception ex)
                {
                    error = ex;
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                   
                    postContext = _smessageTypeListenerPostContextFactory.Create(listener, message, ex, new Dictionary<string, object>());              
                }

                if (postContext != null)
                {
                    await MessageQueueExtensionDescription.OnSMessageTypeListenerExecuted(postContext);
                }

                if (error!=null)
                {
                    string strError = $"{error.Message},{error.StackTrace}";
                        result.Status = 2;
                        result.Description = StringLanguageTranslate.Translate(TextCodes.SMessageExecuteError, string.Format("消息执行出错，消息key:{0},消息type:{1},消息内容:{2},错误内容:{3}", message.Key, message.Type, message.Data, strError));

                        //如果消息不是死消息，并且重试次数超过3次，则移除到死消息队列中
                        //否则增加重试次数
                        if (!message.IsDead && message.RetryNumber + 1 >= 3)
                        {
                            message.ExceptionMessage = result.Description;
                            var deadQueue = await _sQueueChooseService.ChooseDead(message.Key);

                            await using (var transactionScope = new DBTransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 30, 30) }))
                            {
                                await _smessageStore.Delete(queue, message.ID);
                                await _smessageStore.AddToDead(deadQueue, message);
                                result.Status = 3;
                                //执行扩展
                                var context = _smessageOperationContextFactory.Create(message, new Dictionary<string, object>());
                                await MessageQueueExtensionDescription.OnSMessageAddToDeadExecuted(context);

                                transactionScope.Complete();
                            }
                        }
                        else
                        {
                            await using (var transactionScope = new DBTransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 30, 30) }))
                            {

                                /*if (strError.Length > 3000)
                                {
                                    strError = strError.Remove(3000, strError.Length - 3000);
                                }*/

                                await _smessageStore.AddRetry(queue, message.ID, strError);
                                await _smessageStore.UpdateLastExecuteTime(queue, message.ID);

                                transactionScope.Complete();
                            }
                        }

                    

                    return await Task.FromResult(result);
                }
                else
                {
                    await messageHistory.Complete();
                }

            }
            else
            {




                //为每个监听者并行执行监听处理
                List<Task> tasks = new List<Task>();
                List<Exception> taskExceptions = new List<Exception>();
                ParallelHelper parallel = new ParallelHelper(10);


                await executeType.GetAllListener(async (listener) =>
                {

                    parallel.Run(tasks, async (exception) =>
                    {
                        while (exception.InnerException != null)
                        {
                            exception = exception.InnerException;
                        }
                        taskExceptions.Add(exception);
                        postContext = _smessageTypeListenerPostContextFactory.Create(listener, message, exception, new Dictionary<string, object>());
                        postContextList[listener.ID] = postContext;
                        await Task.FromResult(0);
                    },
                    async () =>
                    {
                        //判断该监听是否已经完成
                        var listenerDetail = await _smessageHistoryListenerDetailStore.QueryByName(message.ID, listener.Name);
                        if (listenerDetail == null)
                        {

                            //检查监听是否有所属队列，如果有，则为该监听创建新的消息，如果没有，则执行消息
                            if (!string.IsNullOrEmpty(listener.QueueGroupName))
                            {
                                //生成key
                                var newKey = await _listenerMessageKeyGenerateService.Generate(message.Key, listener);

                                //获取新消息所属队列
                                var newMessageQueue = await _sQueueChooseService.Choose(newKey);
                                var newMessageDeadQueue= await _sQueueChooseService.ChooseDead(newKey);

                                //检查是否已经存在消息
                                var existNewMessage = _smessageStore.QueryByOriginalID(newMessageQueue, message.ID, listener.ID);
                                if (existNewMessage==null)
                                {
                                    existNewMessage = _smessageStore.QueryByOriginalID(newMessageDeadQueue, message.ID, listener.ID);
                                }

                                if (existNewMessage == null)
                                {
                                    SMessage newMessage = new SMessage()
                                    {
                                        Data = message.Data,
                                        Key = newKey,
                                        IsDead = message.IsDead,
                                        ExpectationExecuteTime = message.ExpectationExecuteTime,
                                        Type = message.Type,
                                        TypeListenerID = listener.ID,
                                        OriginalMessageID=message.ID,
                                        RetryNumber = 0

                                    };

                                    await newMessage.Add();
                                }
                            }
                            else
                            {
                                var execueResult = await listener.PostToListener(message);
                                if (!execueResult.Result)
                                {
                                    throw new Exception(execueResult.Description);
                                }
                            }


                            postContext = _smessageTypeListenerPostContextFactory.Create(listener, message, null, new Dictionary<string, object>());
                            postContextList[listener.ID] = postContext;

                            listenerDetail = new SMessageHistoryListenerDetail()
                            {
                                SMessageHistoryID = messageHistory.ID,
                                ListenerMode = listener.Mode,
                                ListenerFactoryType = listener.ListenerFactoryType,
                                ListenerName = listener.Name,
                                ListenerRealWebUrl = string.Empty,
                                ListenerWebUrl = listener.ListenerWebUrl,

                            };

                            await messageHistory.AddListenerDetail(listenerDetail);
                        }
                    }
                    );
                    await Task.FromResult(0);
                });

                //等待所有监听者执行完成
                foreach (var taskItem in tasks)
                {
                    await taskItem;
                }


                //调用扩展介入类的OnSMessageTypeListenerExecuted方法
                foreach (var item in postContextList)
                {
                    await MessageQueueExtensionDescription.OnSMessageTypeListenerExecuted(item.Value);
                }

                if (taskExceptions.Count > 0)
                {
                    var strError = new StringBuilder();
                    foreach (var exceptionItem in taskExceptions)
                    {
                        strError.Append("Message:");
                        strError.Append(await exceptionItem.GetCurrentLcidMessage());
                        strError.Append("\n");
                        strError.Append("StackTrace");
                        strError.Append(exceptionItem.StackTrace);
                        strError.Append("\n\n");
                    }

                    message.ExceptionMessage = strError.ToString();
                    result.Status = 2;
                    result.Description = StringLanguageTranslate.Translate(TextCodes.SMessageExecuteError, string.Format("消息执行出错，消息key:{0},消息type:{1},消息内容:{2},错误内容:{3}", message.Key, message.Type, message.Data, strError));

                    //如果消息不是死消息，并且重试次数超过3次，则移除到死消息队列中
                    //否则增加重试次数
                    if (!message.IsDead && message.RetryNumber + 1 >= 3)
                    {
                        message.ExceptionMessage = result.Description;
                        var deadQueue = await _sQueueChooseService.ChooseDead(message.Key);

                        await using (var transactionScope = new DBTransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 30, 30) }))
                        {
                            await _smessageStore.Delete(queue, message.ID);
                            await _smessageStore.AddToDead(deadQueue, message);
                            result.Status = 3;
                            //执行扩展
                            var context = _smessageOperationContextFactory.Create(message, new Dictionary<string, object>());
                            await MessageQueueExtensionDescription.OnSMessageAddToDeadExecuted(context);

                            transactionScope.Complete();
                        }
                    }
                    else
                    {
                        await using (var transactionScope = new DBTransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 30, 30) }))
                        {

                            /*if (strError.Length > 3000)
                            {
                                strError = strError.Remove(3000, strError.Length - 3000);
                            }*/

                            await _smessageStore.AddRetry(queue, message.ID, strError.ToString());
                            await _smessageStore.UpdateLastExecuteTime(queue, message.ID);

                            transactionScope.Complete();
                        }
                    }

                }
                else
                {
                    await messageHistory.Complete();
                }




            }











            return await Task.FromResult(result);
        }


        private SQueue GetQueue(SMessage message)
        {
            if (!message.Extensions.TryGetValue(_queueName,out object objQueue))
            {
                return null;
            }

            return (SQueue)objQueue;
        }

        /// <summary>
        /// 获取与指定消息的key相同，且期望执行时间早于指定消息的消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task<SMessage> GetBeforeExpectTimeMessage(SMessage message)
        {
            //如果消息是死消息，则只需要在对应的队列中查找
            //否则还需要到对应的死队列中查找

            //首先在自身队列中查找
            //尝试获取message扩展属性中存储的队列
            var queue = GetQueue(message);
            if (queue == null)
            {
                //如果扩展属性中的队列为空，则需要通过Key来获取队列
                //如果消息为死消息，则需要获取所属的死队列
                if (message.IsDead)
                {
                    queue = await _sQueueChooseService.ChooseDead(message.Key);
                }
                else
                {
                    queue = await _sQueueChooseService.Choose(message.Key);
                }
            }

            var beforeMessage=await _smessageStore.QueryByKeyAndBeforeExpectTime(queue, message.Key, message.ExpectationExecuteTime);

            if (beforeMessage!=null)
            {
                return beforeMessage;
            }


            //在对应的死队列中查找
            if (!message.IsDead)
            {
                //获取对应死队列
                queue = await _sQueueChooseService.ChooseDead(message.Key);
                beforeMessage = await _smessageStore.QueryByKeyAndBeforeExpectTime(queue, message.Key, message.ExpectationExecuteTime);

                if (beforeMessage != null)
                {
                    return beforeMessage;
                }
            }

            return null;

        }

        public async Task AddDelay(SMessage message, int delaySeconds, string extensionMessage)
        {
            //尝试获取message扩展属性中存储的队列
            var queue = GetQueue(message);
            if (queue == null)
            {
                //如果扩展属性中的队列为空，则需要通过Key来获取队列
                //如果消息为死消息，则需要获取所属的死队列
                if (message.IsDead)
                {
                    queue = await _sQueueChooseService.ChooseDead(message.Key);
                }
                else
                {
                    queue = await _sQueueChooseService.Choose(message.Key);
                }
            }

            var existMessage=await _smessageStore.QueryByDelayID(queue, message.ID);
            if (existMessage==null)
            {
                var newMessage = new SMessage()
                {
                    Key = message.Key,
                    Type = message.Type,
                    OriginalMessageID = message.OriginalMessageID,
                    TypeListenerID = message.TypeListenerID,
                    ExtensionMessage = extensionMessage,
                    ExpectationExecuteTime = DateTime.UtcNow.AddSeconds(delaySeconds),
                    DelayMessageID = message.ID,
                    Data = message.Data

                };
                await message.Add();
            }
        }
    }
}
