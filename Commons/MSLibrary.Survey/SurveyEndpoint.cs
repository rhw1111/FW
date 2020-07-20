using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using MongoDB.Libmongocrypt;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Transaction;
using MSLibrary.Thread;
using MSLibrary.Survey.DAL;

namespace MSLibrary.Survey
{
    /// <summary>
    /// /Survey响应收集器终结点
    /// Name+Type唯一
    /// </summary>
    public class SurveyEndpoint : EntityBase<ISurveyEndpointIMP>
    {
        private static IFactory<ISurveyEndpointIMP>? _surveyEndpointIMP;

        public static IFactory<ISurveyEndpointIMP>? SurveyEndpointIMPFactory
        {
            set
            {
                _surveyEndpointIMP = value;
            }
        }

        public override IFactory<ISurveyEndpointIMP>? GetIMPFactory()
        {
            return _surveyEndpointIMP;
        }

        /// <summary>
        /// Id
        /// </summary>
        public Guid ID
        {
            get
            {

                return GetAttribute<Guid>(nameof(ID));
            }
            set
            {
                SetAttribute<Guid>(nameof(ID), value);
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {

                return GetAttribute<string>(nameof(Name));
            }
            set
            {
                SetAttribute<string>(nameof(Name), value);
            }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type
        {
            get
            {

                return GetAttribute<string>(nameof(Type));
            }
            set
            {
                SetAttribute<string>(nameof(Type), value);
            }
        }

        /// <summary>
        /// 配置
        /// </summary>
        public string Configuration
        {
            get
            {

                return GetAttribute<string>(nameof(Configuration));
            }
            set
            {
                SetAttribute<string>(nameof(Configuration), value);
            }
        }

        /// <summary>
        /// 拥有的响应组名称集合
        /// </summary>
        public List<string> ResponseGroupNames
        {
            get
            {

                return GetAttribute<List<string>>(nameof(ResponseGroupNames));
            }
            set
            {
                SetAttribute<List<string>>(nameof(ResponseGroupNames), value);
            }
        }

        /// <summary>
        /// 初始化信息
        /// </summary>
        public string InitInfo
        {
            get
            {

                return GetAttribute<string>(nameof(InitInfo));
            }
            set
            {
                SetAttribute<string>(nameof(InitInfo), value);
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
        /// 管理收集器
        /// 完成Survey收集器的增、删、绑定
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task ManageCollector(Func<SurveyRecord, Task> addCallback, Func<SurveyRecord, Task> updateCallback, Func<SurveyRecord, Task> deleteCallback,CancellationToken cancellationToken = default)
        {
            await _imp.ManageCollector(this, addCallback,updateCallback,deleteCallback, cancellationToken);
        }

        /// <summary>
        /// 绑定收集器
        /// </summary>
        /// <param name="collectorData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task BindCollector(string collectorData, CancellationToken cancellationToken = default)
        {
            await _imp.BindCollector(this, collectorData, cancellationToken);
        }

        /// <summary>
        /// 解绑收集器
        /// </summary>
        /// <param name="collectorData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task UnBindCollector(string collectorData, CancellationToken cancellationToken = default)
        {
            await _imp.UnBindCollector(this, collectorData, cancellationToken);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Init(CancellationToken cancellationToken = default)
        {
            await _imp.Init(this, cancellationToken);
        }

        /// <summary>
        /// 终止处理
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Finanly(CancellationToken cancellationToken = default)
        {
            await _imp.Finanly(this, cancellationToken);
        }


        /// <summary>
        /// 获取指定surveyID的收集器
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<SurveyRecord?> GetCollector(string surveyID, CancellationToken cancellationToken = default)
        {
            return await _imp.GetCollector(this, surveyID, cancellationToken);
        }


        /// <summary>
        /// 分页获取收集器
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<QueryResult<SurveyRecord>> GetCollector(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _imp.GetCollector(this, page, pageSize, cancellationToken);
        }

        /// <summary>
        /// 获取所有收集器
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public IAsyncEnumerable<SurveyRecord> GetAllCollector(CancellationToken cancellationToken = default)
        {
            return _imp.GetAllCollector(this, cancellationToken);
        }

        /// <summary>
        /// 获取指定组下的所有收集器
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public IAsyncEnumerable<SurveyRecord> GetAllCollector(string groupName, CancellationToken cancellationToken = default)
        {
            return _imp.GetAllCollector(this, groupName, cancellationToken);
        }


    }

    public interface ISurveyEndpointIMP
    {
        /// <summary>
        /// 管理收集器
        /// 完成Survey收集器的增、删、绑定
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ManageCollector(SurveyEndpoint endpoint,Func<SurveyRecord,Task> addCallback,Func<SurveyRecord,Task> updateCallback,Func<SurveyRecord,Task> deleteCallback,CancellationToken cancellationToken = default);
        /// <summary>
        /// 绑定收集器
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="collectorData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task BindCollector(SurveyEndpoint endpoint,string collectorData, CancellationToken cancellationToken = default);
        /// <summary>
        /// 解绑收集器
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="collectorData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UnBindCollector(SurveyEndpoint endpoint, string collectorData, CancellationToken cancellationToken = default);
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Init(SurveyEndpoint endpoint, CancellationToken cancellationToken = default);
        /// <summary>
        /// 终止处理
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Finanly(SurveyEndpoint endpoint, CancellationToken cancellationToken = default);


        /// <summary>
        /// 获取指定surveyID的收集器
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SurveyRecord?> GetCollector(SurveyEndpoint endpoint, string surveyID, CancellationToken cancellationToken = default);

        /// <summary>
        /// 分页获取收集器
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<QueryResult<SurveyRecord>> GetCollector(SurveyEndpoint endpoint,int page,int pageSize, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取所有收集器
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IAsyncEnumerable<SurveyRecord> GetAllCollector(SurveyEndpoint endpoint, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取指定组下的所有收集器
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="groupName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IAsyncEnumerable<SurveyRecord> GetAllCollector(SurveyEndpoint endpoint,string groupName, CancellationToken cancellationToken = default);

    }


    /// <summary>
    /// Survey收集器工厂
    /// 负责从收集器数据生成收集器
    /// </summary>
    public interface ISurveyCollectorFactory
    {
        Task<SurveyRecord> Create(string endpointConfiguration,string collectorData, CancellationToken cancellationToken = default);
        Task<SurveyRecord> CreateFromDirect(string endpointConfiguration, string collectorData, CancellationToken cancellationToken = default);
    }



    /// <summary>
    /// Survey收集器绑定服务
    /// 负责Survey收集器的绑定、解除绑定操作
    /// </summary>
    public interface ISurveyCollectorBindService
    {
        Task<string?> Binding(string endpointConfiguration, SurveyRecord collector, CancellationToken cancellationToken = default);
        Task UnBinding(string endpointConfiguration, SurveyRecord collector, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Survey终结点终止服务
    /// 负责终结点的清理工作
    /// </summary>
    public interface ISurveyEndpointFinanlyService
    {
        Task Execute(string endpointConfiguration,string initInfo, CancellationToken cancellationToken = default);
    }
    /// <summary>
    /// Survey终结点初始化服务
    /// 负责终结点的初始化工作
    /// </summary>
    public interface ISurveyEndpointInitService
    {
        Task<string?> Execute(string endpointConfiguration, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Survey收集器数据查询服务
    /// 负责从源上获取所有指定Survey收集器数据
    /// </summary>
    public interface ISurveyCollectorDataQueryService
    {
        IAsyncEnumerable<string> QueryAll(string endpointConfiguration, CancellationToken cancellationToken = default);
        Task<bool> Exist(string endpointConfiguration,string surveyID, CancellationToken cancellationToken = default);
    }




    [Injection(InterfaceType = typeof(ISurveyEndpointIMP), Scope = InjectionScope.Transient)]
    public class SurveyEndpointIMP : ISurveyEndpointIMP
    {
        private readonly ISurveyRecordStore _surveyRecordStore;
        private readonly ISurveyEndpointStore _surveyEndpointStore;

        public SurveyEndpointIMP(ISurveyRecordStore surveyRecordStore, ISurveyEndpointStore surveyEndpointStore)
        {
            _surveyRecordStore = surveyRecordStore;
            _surveyEndpointStore = surveyEndpointStore;
        }

        private async Task addCollector(SurveyEndpoint endpoint, SurveyRecord collector, CancellationToken cancellationToken = default)
        {
            collector.EndpointID = endpoint.ID;
            await _surveyRecordStore.Add(collector,cancellationToken);
        }

        public async Task BindCollector(SurveyEndpoint endpoint, string collectorData, CancellationToken cancellationToken = default)
        {

            var collectorFactory = SurveyExtensionCollection.GetSurveyCollectorFactory(endpoint.Type);
            var collectorBindService= SurveyExtensionCollection.GetSurveyCollectorBindService(endpoint.Type);


            var collector = await collectorFactory.CreateFromDirect(endpoint.Configuration,collectorData, cancellationToken);

            var existCollector = await GetCollector(endpoint, collector.SurveyID, cancellationToken);

            if (existCollector == null)
            {
                await using (DBTransactionScope scope = new DBTransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    //为收集器的组属性赋值
                    collector.Group = getRanResponseGroupName(endpoint);
                    collector.Endpoint = endpoint;
                    collector.EndpointID = endpoint.ID;
                        
                    await addCollector(endpoint, collector, cancellationToken);
                    var bindintInfo= await collectorBindService.Binding(endpoint.Configuration, collector, cancellationToken);
                    if (bindintInfo != null)
                    {
                        await _surveyRecordStore.UpdateBindingInfo(collector.ID, bindintInfo, cancellationToken);
                    }
                    scope.Complete();
                }
            }
            else
            {
                existCollector.Name = collector.Name;
                await existCollector.Update(cancellationToken);
            }
        }

        private async Task deleteResponseCollector(SurveyEndpoint endpoint, Guid collectorID, CancellationToken cancellationToken = default)
        {
            await _surveyRecordStore.Delete(endpoint.ID, collectorID,cancellationToken);
        }

        public async Task Finanly(SurveyEndpoint endpoint, CancellationToken cancellationToken = default)
        {
            var collectorBindService = SurveyExtensionCollection.GetSurveyCollectorBindService(endpoint.Type);

            //解绑所有collector
            var currentCollectors = GetAllCollector(endpoint, cancellationToken);
            await ParallelHelper.ForEach(currentCollectors, 5,
                async (collector) =>
                {
                    await collectorBindService.UnBinding(endpoint.Configuration, collector, cancellationToken);
                    await deleteResponseCollector(endpoint, collector.ID, cancellationToken);
                }
            );


            var service=SurveyExtensionCollection.GetSurveyEndpointFinanlyService(endpoint.Type);
            await service.Execute(endpoint.Configuration, endpoint.InitInfo, cancellationToken);
        }

        public IAsyncEnumerable<SurveyRecord> GetAllCollector(SurveyEndpoint endpoint, CancellationToken cancellationToken = default)
        {
            return _surveyRecordStore.QueryAllByEndpoint(endpoint.ID, cancellationToken);
        }

        public IAsyncEnumerable<SurveyRecord> GetAllCollector(SurveyEndpoint endpoint, string groupName, CancellationToken cancellationToken = default)
        {
            return _surveyRecordStore.QueryAllByGroup(endpoint.ID, groupName, cancellationToken);
        }

        public async Task<QueryResult<SurveyRecord>> GetCollector(SurveyEndpoint endpoint, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _surveyRecordStore.QueryByPage(endpoint.ID, page, pageSize, cancellationToken);
        }

        public async Task<SurveyRecord?> GetCollector(SurveyEndpoint endpoint, string surveyID, CancellationToken cancellationToken = default)
        {
            return await _surveyRecordStore.Query(endpoint.ID, surveyID, cancellationToken);
        }

        public async Task Init(SurveyEndpoint endpoint, CancellationToken cancellationToken = default)
        {
            var service=SurveyExtensionCollection.GetSurveyEndpointInitService(endpoint.Type);

            await using (DBTransactionScope scope = new DBTransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
            {

                var initInfo = await service.Execute(endpoint.Configuration, cancellationToken);
                if (initInfo != null)
                {
                    await _surveyEndpointStore.UpdateInitInfo(endpoint.ID, initInfo, cancellationToken);
                }
                scope.Complete();
            }
          
        }

        public async Task ManageCollector(SurveyEndpoint endpoint, Func<SurveyRecord, Task> addCallback, Func<SurveyRecord, Task> updateCallback, Func<SurveyRecord, Task> deleteCallback, CancellationToken cancellationToken = default)
        {
            var collectorFactoryService = SurveyExtensionCollection.GetSurveyCollectorFactory(endpoint.Type);
            var collectorDataQueryService = SurveyExtensionCollection.GetSurveyCollectorDataQueryService(endpoint.Type);
            var collectorBindService = SurveyExtensionCollection.GetSurveyCollectorBindService(endpoint.Type);


            //获取现有的所有收集器
            var currentCollectors = GetAllCollector(endpoint, cancellationToken);
            ConcurrentBag<SurveyRecord> deleteCollectors = new ConcurrentBag<SurveyRecord>();

      
            await ParallelHelper.ForEach(currentCollectors, 5,
                async(collector)=>
                {
                    //检查在源中是否存在，如果不存在，则需要删除并解绑收集器
                    if (!await collectorDataQueryService.Exist(endpoint.Configuration, collector.SurveyID, cancellationToken))
                    {
                        deleteCollectors.Add(collector);
                    }
                }
                );


            await ParallelHelper.ForEach(deleteCollectors, 5,
                async(collector)=>
                {
                    await using (DBTransactionScope scope = new DBTransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                    {                
                        await collectorBindService.UnBinding(endpoint.Configuration, collector, cancellationToken);
                        await deleteCallback(collector);
                        await deleteResponseCollector(endpoint, collector.ID, cancellationToken);
                        scope.Complete();
                    }
                }
                );



            //从源获取所有关联的收集器数据
            var collectorDatas= collectorDataQueryService.QueryAll(endpoint.Configuration, cancellationToken);
            //转换成收集器，检查是否已经存在收集器，如果不存在，则新建，执行绑定
            await ParallelHelper.ForEach(collectorDatas, 5,
                async (data) =>
                {
                    var newCollector = await collectorFactoryService.Create(endpoint.Configuration,data, cancellationToken);

                    //为收集器的组属性赋值
                    newCollector.Group = getRanResponseGroupName(endpoint);
                    newCollector.Endpoint = endpoint;
                    newCollector.EndpointID = endpoint.ID;

                    var existCollector = await GetCollector(endpoint, newCollector.SurveyID, cancellationToken);
                    if (existCollector == null)
                    {
                        await using (DBTransactionScope scope = new DBTransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                        {

                            await addCollector(endpoint, newCollector, cancellationToken);
                            var bindingInfo=await collectorBindService.Binding(endpoint.Configuration, newCollector, cancellationToken);
                            if (bindingInfo != null)
                            {
                                await _surveyRecordStore.UpdateBindingInfo(newCollector.ID, bindingInfo, cancellationToken);
                            }

                            await addCallback(newCollector);
                            scope.Complete();
                        }
                    }
                    else
                    {
                        await using (DBTransactionScope scope = new DBTransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                        {

                            existCollector.Name = newCollector.Name;
                            await existCollector.Update(cancellationToken);
                            await addCallback(existCollector);
                            scope.Complete();
                        }
                    }
                }
                );

        }

        public async Task UnBindCollector(SurveyEndpoint endpoint, string collectorData, CancellationToken cancellationToken = default)
        {
            var collectorFactoryService = SurveyExtensionCollection.GetSurveyCollectorFactory(endpoint.Type);
            var collectorBindService = SurveyExtensionCollection.GetSurveyCollectorBindService(endpoint.Type);

            var collector = await collectorFactoryService.CreateFromDirect(endpoint.Configuration,collectorData, cancellationToken);

            collector.Endpoint = endpoint;
            collector.EndpointID = endpoint.ID;

            var existCollector = await GetCollector(endpoint, collector.SurveyID, cancellationToken);

            if (existCollector != null)
            {
                await using (DBTransactionScope scope = new DBTransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    await collectorBindService.UnBinding(endpoint.Configuration, existCollector, cancellationToken);
                    await deleteResponseCollector(endpoint, existCollector.ID, cancellationToken);
                    scope.Complete();
                }
            }
        }

        private string getRanResponseGroupName(SurveyEndpoint endpoint)
        {
            Random ran = new Random(DateTime.UtcNow.Millisecond);
            return endpoint.ResponseGroupNames[ran.Next(0, endpoint.ResponseGroupNames.Count)];
        }
    }
}
