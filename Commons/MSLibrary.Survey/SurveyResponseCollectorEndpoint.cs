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
using Microsoft.Azure.Amqp.Framing;

namespace MSLibrary.Survey
{
    /// <summary>
    /// /Survey响应收集器终结点
    /// Name+Type唯一
    /// </summary>
    public class SurveyResponseCollectorEndpoint : EntityBase<ISurveyResponseCollectorEndpointIMP>
    {
        private static IFactory<ISurveyResponseCollectorEndpointIMP>? _surveyResponseCollectorEndpointIMPFactory;

        public static IFactory<ISurveyResponseCollectorEndpointIMP>? SurveyResponseCollectorEndpointIMPFactory
        {
            set
            {
                _surveyResponseCollectorEndpointIMPFactory = value;
            }
        }

        public override IFactory<ISurveyResponseCollectorEndpointIMP>? GetIMPFactory()
        {
            return _surveyResponseCollectorEndpointIMPFactory;
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
        /// 拥有的组名称集合
        /// </summary>
        public List<string> GroupNames
        {
            get
            {

                return GetAttribute<List<string>>(nameof(GroupNames));
            }
            set
            {
                SetAttribute<List<string>>(nameof(GroupNames), value);
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
        public async Task ManageCollector(CancellationToken cancellationToken = default)
        {
            await _imp.ManageCollector(this, cancellationToken);
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
        public async Task<SurveyResponseCollector?> GetCollector(string surveyID, CancellationToken cancellationToken = default)
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
        public async Task<QueryResult<SurveyResponseCollector>> GetCollector(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _imp.GetCollector(this, page, pageSize, cancellationToken);
        }

        /// <summary>
        /// 获取所有收集器
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public IAsyncEnumerable<SurveyResponseCollector> GetAllCollector(CancellationToken cancellationToken = default)
        {
            return _imp.GetAllCollector(this, cancellationToken);
        }

        /// <summary>
        /// 获取指定组下的所有收集器
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public IAsyncEnumerable<SurveyResponseCollector> GetAllCollector(string groupName, CancellationToken cancellationToken = default)
        {
            return _imp.GetAllCollector(this, groupName, cancellationToken);
        }


    }

    public interface ISurveyResponseCollectorEndpointIMP
    {
        /// <summary>
        /// 管理收集器
        /// 完成Survey收集器的增、删、绑定
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ManageCollector(SurveyResponseCollectorEndpoint endpoint,CancellationToken cancellationToken = default);
        /// <summary>
        /// 绑定收集器
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="collectorData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task BindCollector(SurveyResponseCollectorEndpoint endpoint,string collectorData, CancellationToken cancellationToken = default);
        /// <summary>
        /// 解绑收集器
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="collectorData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UnBindCollector(SurveyResponseCollectorEndpoint endpoint, string collectorData, CancellationToken cancellationToken = default);
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Init(SurveyResponseCollectorEndpoint endpoint, CancellationToken cancellationToken = default);
        /// <summary>
        /// 终止处理
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Finanly(SurveyResponseCollectorEndpoint endpoint, CancellationToken cancellationToken = default);


        /// <summary>
        /// 获取指定surveyID的收集器
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SurveyResponseCollector?> GetCollector(SurveyResponseCollectorEndpoint endpoint, string surveyID, CancellationToken cancellationToken = default);

        /// <summary>
        /// 分页获取收集器
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<QueryResult<SurveyResponseCollector>> GetCollector(SurveyResponseCollectorEndpoint endpoint,int page,int pageSize, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取所有收集器
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IAsyncEnumerable<SurveyResponseCollector> GetAllCollector(SurveyResponseCollectorEndpoint endpoint, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取指定组下的所有收集器
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="groupName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IAsyncEnumerable<SurveyResponseCollector> GetAllCollector(SurveyResponseCollectorEndpoint endpoint,string groupName, CancellationToken cancellationToken = default);

    }


    /// <summary>
    /// Survey响应收集器工厂
    /// 负责从收集器数据生成收集器
    /// </summary>
    public interface ISurveyResponseCollectorFactory
    {
        Task<SurveyResponseCollector> Create(string collectorData, CancellationToken cancellationToken = default);
    }



    /// <summary>
    /// Survey响应收集器绑定服务
    /// 负责Survey响应收集器的绑定、解除绑定操作
    /// </summary>
    public interface ISurveyResponseCollectorBindService
    {
        Task<string?> Binding(string endpointConfiguration, SurveyResponseCollector collector, CancellationToken cancellationToken = default);
        Task UnBinding(string endpointConfiguration, SurveyResponseCollector collector, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Survey响应收集器终结点终止服务
    /// 负责终结点的清理工作
    /// </summary>
    public interface ISurveyResponseCollectorEndpointFinanlyService
    {
        Task Execute(string endpointConfiguration,string initInfo, CancellationToken cancellationToken = default);
    }
    /// <summary>
    /// Survey响应收集器终结点初始化服务
    /// 负责终结点的初始化工作
    /// </summary>
    public interface ISurveyResponseCollectorEndpointInitService
    {
        Task<string?> Execute(string endpointConfiguration, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Survey响应收集器数据查询服务
    /// 负责从源上获取所有Survey响应收集器数据
    /// </summary>
    public interface ISurveyResponseCollectorDataQueryService
    {
        IAsyncEnumerable<string> QueryAll(string endpointConfiguration, CancellationToken cancellationToken = default);
        Task<bool> Exist(string endpointConfiguration,string surveyID, CancellationToken cancellationToken = default);
    }




    [Injection(InterfaceType = typeof(ISurveyResponseCollectorEndpointIMP), Scope = InjectionScope.Transient)]
    public class SurveyResponseCollectorEndpointIMP : ISurveyResponseCollectorEndpointIMP
    {
        private readonly ISurveyResponseCollectorStore _surveyResponseCollectorStore;
        private readonly ISurveyResponseCollectorEndpointStore _surveyResponseCollectorEndpointStore;

        public SurveyResponseCollectorEndpointIMP(ISurveyResponseCollectorStore surveyResponseCollectorStore, ISurveyResponseCollectorEndpointStore surveyResponseCollectorEndpointStore)
        {
            _surveyResponseCollectorStore = surveyResponseCollectorStore;
            _surveyResponseCollectorEndpointStore = surveyResponseCollectorEndpointStore;
        }

        private async Task addCollector(SurveyResponseCollectorEndpoint endpoint, SurveyResponseCollector collector, CancellationToken cancellationToken = default)
        {
            collector.EndpointID = endpoint.ID;
            await _surveyResponseCollectorStore.Add(collector,cancellationToken);
        }

        public async Task BindCollector(SurveyResponseCollectorEndpoint endpoint, string collectorData, CancellationToken cancellationToken = default)
        {

            var collectorFactory = SurveyResponseExtensionCollection.GetSurveyResponseCollectorFactory(endpoint.Type);
            var collectorBindService= SurveyResponseExtensionCollection.GetSurveyResponseCollectorBindService(endpoint.Type);


            var collector = await collectorFactory.Create(collectorData, cancellationToken);

            var existCollector = await GetCollector(endpoint, collector.SurveyID, cancellationToken);

            if (existCollector == null)
            {
                await using (DBTransactionScope scope = new DBTransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    //为收集器的组属性赋值
                    collector.Group = getRanGroupName(endpoint);
                    await addCollector(endpoint, collector, cancellationToken);
                    var bindintInfo= await collectorBindService.Binding(endpoint.Configuration, collector, cancellationToken);
                    if (bindintInfo != null)
                    {
                        await _surveyResponseCollectorStore.UpdateBindingInfo(collector.ID, bindintInfo, cancellationToken);
                    }
                    scope.Complete();
                }
            }
        }

        private async Task deleteCollector(SurveyResponseCollectorEndpoint endpoint, Guid collectorID, CancellationToken cancellationToken = default)
        {
            await _surveyResponseCollectorStore.Delete(endpoint.ID, collectorID,cancellationToken);
        }

        public async Task Finanly(SurveyResponseCollectorEndpoint endpoint, CancellationToken cancellationToken = default)
        {
            var service=SurveyResponseExtensionCollection.GetSurveyResponseCollectorEndpointFinanlyService(endpoint.Type);
            await service.Execute(endpoint.Configuration, endpoint.InitInfo, cancellationToken);
        }

        public IAsyncEnumerable<SurveyResponseCollector> GetAllCollector(SurveyResponseCollectorEndpoint endpoint, CancellationToken cancellationToken = default)
        {
            return _surveyResponseCollectorStore.QueryAllCollector(endpoint.ID, cancellationToken);
        }

        public IAsyncEnumerable<SurveyResponseCollector> GetAllCollector(SurveyResponseCollectorEndpoint endpoint, string groupName, CancellationToken cancellationToken = default)
        {
            return _surveyResponseCollectorStore.QueryAllCollectorByGroup(endpoint.ID, groupName, cancellationToken);
        }

        public async Task<QueryResult<SurveyResponseCollector>> GetCollector(SurveyResponseCollectorEndpoint endpoint, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _surveyResponseCollectorStore.QueryByPage(endpoint.ID, page, pageSize, cancellationToken);
        }

        public async Task<SurveyResponseCollector?> GetCollector(SurveyResponseCollectorEndpoint endpoint, string surveyID, CancellationToken cancellationToken = default)
        {
            return await _surveyResponseCollectorStore.Query(endpoint.ID, surveyID, cancellationToken);
        }

        public async Task Init(SurveyResponseCollectorEndpoint endpoint, CancellationToken cancellationToken = default)
        {
            var service=SurveyResponseExtensionCollection.GetSurveyResponseCollectorEndpointInitService(endpoint.Type);

            await using (DBTransactionScope scope = new DBTransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
            {

                var initInfo = await service.Execute(endpoint.Configuration, cancellationToken);
                if (initInfo != null)
                {
                    await _surveyResponseCollectorEndpointStore.UpdateInitInfo(endpoint.ID, initInfo, cancellationToken);
                }
                scope.Complete();
            }
          
        }

        public async Task ManageCollector(SurveyResponseCollectorEndpoint endpoint, CancellationToken cancellationToken = default)
        {
            var collectorFactoryService = SurveyResponseExtensionCollection.GetSurveyResponseCollectorFactory(endpoint.Type);
            var collectorDataQueryService = SurveyResponseExtensionCollection.GetSurveyResponseCollectorDataQueryService(endpoint.Type);
            var collectorBindService = SurveyResponseExtensionCollection.GetSurveyResponseCollectorBindService(endpoint.Type);


            //获取现有的所有收集器
            var currentCollectors = GetAllCollector(endpoint, cancellationToken);
            ConcurrentBag<SurveyResponseCollector> deleteCollectors = new ConcurrentBag<SurveyResponseCollector>();

      
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
                        await deleteCollector(endpoint, collector.ID, cancellationToken);
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
                    var newCollector = await collectorFactoryService.Create(data, cancellationToken);

                    //为收集器的组属性赋值
                    newCollector.Group = getRanGroupName(endpoint);

                    var existCollector = await GetCollector(endpoint, newCollector.SurveyID, cancellationToken);
                    if (existCollector == null)
                    {
                        await using (DBTransactionScope scope = new DBTransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                        {

                            await addCollector(endpoint, newCollector, cancellationToken);
                            var bindingInfo=await collectorBindService.Binding(endpoint.Configuration, newCollector, cancellationToken);
                            if (bindingInfo != null)
                            {
                                await _surveyResponseCollectorStore.UpdateBindingInfo(newCollector.ID, bindingInfo, cancellationToken);
                            }
                            scope.Complete();
                        }
                    }
                }
                );

        }

        public async Task UnBindCollector(SurveyResponseCollectorEndpoint endpoint, string collectorData, CancellationToken cancellationToken = default)
        {
            var collectorFactoryService = SurveyResponseExtensionCollection.GetSurveyResponseCollectorFactory(endpoint.Type);
            var collectorBindService = SurveyResponseExtensionCollection.GetSurveyResponseCollectorBindService(endpoint.Type);

            var collector = await collectorFactoryService.Create(collectorData, cancellationToken);

            var existCollector = await GetCollector(endpoint, collector.SurveyID, cancellationToken);

            if (existCollector != null)
            {
                await using (DBTransactionScope scope = new DBTransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    await collectorBindService.UnBinding(endpoint.Configuration, existCollector, cancellationToken);
                    await deleteCollector(endpoint, existCollector.ID, cancellationToken);
                    scope.Complete();
                }
            }
        }

        private string getRanGroupName(SurveyResponseCollectorEndpoint endpoint)
        {
            Random ran = new Random(DateTime.UtcNow.Millisecond);
            return endpoint.GroupNames[ran.Next(0, endpoint.GroupNames.Count)];
        }
    }
}
