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

namespace MSLibrary.Survey
{
    /// <summary>
    /// /Survey响应收集器终结点
    /// Name+Type唯一
    /// </summary>
    public class SurveyResponseCollectorEndpoint : EntityBase<ISurveyResponseCollectorEndpointIMP>
    {
        public override IFactory<ISurveyResponseCollectorEndpointIMP>? GetIMPFactory()
        {
            throw new NotImplementedException();
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
        /// 新增收集器
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="collector"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddCollector(SurveyResponseCollectorEndpoint endpoint,SurveyResponseCollector collector, CancellationToken cancellationToken = default);
        /// <summary>
        /// 删除收集器
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="collectorID"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteCollector(SurveyResponseCollectorEndpoint endpoint,Guid collectorID, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取指定surveyID的收集器
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
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
        Task Binding(string endpointConfiguration, SurveyResponseCollector collector, CancellationToken cancellationToken = default);
        Task UnBinding(string endpointConfiguration, SurveyResponseCollector collector, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Survey响应收集器终结点终止服务
    /// 负责终结点的清理工作
    /// </summary>
    public interface ISurveyResponseCollectorEndpointFinanlyService
    {
        Task Execute(string endpointConfiguration, CancellationToken cancellationToken = default);
    }
    /// <summary>
    /// Survey响应收集器终结点初始化服务
    /// 负责终结点的初始化工作
    /// </summary>
    public interface ISurveyResponseCollectorEndpointInitService
    {
        Task Execute(string endpointConfiguration, CancellationToken cancellationToken = default);
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
        /// <summary>
        /// Survey响应收集器绑定服务键值对
        /// 键为SurveyResponseCollectorEndpoint.Type
        /// </summary>
        public static IDictionary<string, IFactory<ISurveyResponseCollectorBindService>> SurveyResponseCollectorBindServiceFactories { get; } = new Dictionary<string, IFactory<ISurveyResponseCollectorBindService>>();

        /// <summary>
        /// Survey响应收集器工厂键值对
        /// 键为SurveyResponseCollectorEndpoint.Type
        /// </summary>
        public static IDictionary<string, IFactory<ISurveyResponseCollectorFactory>> SurveyResponseCollectorFactories { get; } = new Dictionary<string, IFactory<ISurveyResponseCollectorFactory>>();

        /// <summary>
        /// Survey响应收集器终结点终止服务工厂键值对
        /// 键为SurveyResponseCollectorEndpoint.Type
        /// </summary>
        public static IDictionary<string, IFactory<ISurveyResponseCollectorEndpointFinanlyService>> SurveyResponseCollectorEndpointFinanlyServiceFactories { get; } = new Dictionary<string, IFactory<ISurveyResponseCollectorEndpointFinanlyService>>();

        /// <summary>
        /// Survey响应收集器终结点初始化服务工厂键值对
        /// 键为SurveyResponseCollectorEndpoint.Type
        /// </summary>
        public static IDictionary<string, IFactory<ISurveyResponseCollectorEndpointInitService>> SurveyResponseCollectorEndpointInitServiceFactories { get; } = new Dictionary<string, IFactory<ISurveyResponseCollectorEndpointInitService>>();

        /// <summary>
        /// Survey响应收集器数据查询服务工厂键值对
        /// 键为SurveyResponseCollectorEndpoint.Type
        /// </summary>
        public static IDictionary<string, IFactory<ISurveyResponseCollectorDataQueryService>> SurveyResponseCollectorDataQueryServiceFactories { get; } = new Dictionary<string, IFactory<ISurveyResponseCollectorDataQueryService>>();

        public Task AddCollector(SurveyResponseCollectorEndpoint endpoint, SurveyResponseCollector collector, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task BindCollector(SurveyResponseCollectorEndpoint endpoint, string collectorData, CancellationToken cancellationToken = default)
        {
            if (!SurveyResponseCollectorFactories.TryGetValue(endpoint.Type,out IFactory<ISurveyResponseCollectorFactory> surveyResponseCollectorFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyResponseCollectorFactoryByType,
                    DefaultFormatting = "找不到类型为{0}的Survey响应收集器工厂，发生位置为{1}",
                    ReplaceParameters = new List<object>() { endpoint.Type, $"{this.GetType().FullName}.SurveyResponseCollectorFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyResponseCollectorFactoryByType, fragment, 1, 0);
            }

            if (!SurveyResponseCollectorBindServiceFactories.TryGetValue(endpoint.Type, out IFactory<ISurveyResponseCollectorBindService> surveyResponseCollectorBindServiceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyResponseCollectorBindServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Survey响应收集器绑定服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { endpoint.Type, $"{this.GetType().FullName}.SurveyResponseCollectorBindServiceFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyResponseCollectorBindServiceByType, fragment, 1, 0);
            }

            var collector = await surveyResponseCollectorFactory.Create().Create(collectorData, cancellationToken);

            var existCollector = await GetCollector(endpoint, collector.SurveyID, cancellationToken);

            if (existCollector == null)
            {
                await using (DBTransactionScope scope = new DBTransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                {

                    await AddCollector(endpoint, collector, cancellationToken);
                    await surveyResponseCollectorBindServiceFactory.Create().Binding(endpoint.Configuration, collector, cancellationToken);

                    scope.Complete();
                }
            }
        }

        public Task DeleteCollector(SurveyResponseCollectorEndpoint endpoint, Guid collectorID, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task Finanly(SurveyResponseCollectorEndpoint endpoint, CancellationToken cancellationToken = default)
        {
            if (!SurveyResponseCollectorEndpointFinanlyServiceFactories.TryGetValue(endpoint.Type, out IFactory<ISurveyResponseCollectorEndpointFinanlyService> surveyResponseCollectorEndpointFinanlyServiceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyResponseCollectorEndpointFinanlyServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Survey响应收集器终结点终止服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { endpoint.Type, $"{this.GetType().FullName}.SurveyResponseCollectorEndpointFinanlyServiceFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyResponseCollectorEndpointFinanlyServiceByType, fragment, 1, 0);
            }
            await surveyResponseCollectorEndpointFinanlyServiceFactory.Create().Execute(endpoint.Configuration,cancellationToken);
        }

        public IAsyncEnumerable<SurveyResponseCollector> GetAllCollector(SurveyResponseCollectorEndpoint endpoint, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<QueryResult<SurveyResponseCollector>> GetCollector(SurveyResponseCollectorEndpoint endpoint, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<SurveyResponseCollector?> GetCollector(SurveyResponseCollectorEndpoint endpoint, string surveyID, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task Init(SurveyResponseCollectorEndpoint endpoint, CancellationToken cancellationToken = default)
        {
            if (!SurveyResponseCollectorEndpointInitServiceFactories.TryGetValue(endpoint.Type, out IFactory<ISurveyResponseCollectorEndpointInitService> surveyResponseCollectorEndpointInitServiceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyResponseCollectorEndpointInitServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Survey响应收集器终结点初始化服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { endpoint.Type, $"{this.GetType().FullName}.SurveyResponseCollectorEndpointInitServiceFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyResponseCollectorEndpointInitServiceByType, fragment, 1, 0);
            }
            await surveyResponseCollectorEndpointInitServiceFactory.Create().Execute(endpoint.Configuration,cancellationToken);
        }

        public async Task ManageCollector(SurveyResponseCollectorEndpoint endpoint, CancellationToken cancellationToken = default)
        {
            if (!SurveyResponseCollectorFactories.TryGetValue(endpoint.Type, out IFactory<ISurveyResponseCollectorFactory> surveyResponseCollectorFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyResponseCollectorFactoryByType,
                    DefaultFormatting = "找不到类型为{0}的Survey响应收集器工厂，发生位置为{1}",
                    ReplaceParameters = new List<object>() { endpoint.Type, $"{this.GetType().FullName}.SurveyResponseCollectorFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyResponseCollectorFactoryByType, fragment, 1, 0);
            }

            if (!SurveyResponseCollectorDataQueryServiceFactories.TryGetValue(endpoint.Type, out IFactory<ISurveyResponseCollectorDataQueryService> surveyResponseCollectorDataQueryServiceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyResponseCollectorDataQueryServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Survey响应收集器数据查询服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { endpoint.Type, $"{this.GetType().FullName}.SurveyResponseCollectorDataQueryServiceFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyResponseCollectorDataQueryServiceByType, fragment, 1, 0);
            }

            if (!SurveyResponseCollectorBindServiceFactories.TryGetValue(endpoint.Type, out IFactory<ISurveyResponseCollectorBindService> surveyResponseCollectorBindServiceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyResponseCollectorBindServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Survey响应收集器绑定服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { endpoint.Type, $"{this.GetType().FullName}.SurveyResponseCollectorBindServiceFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyResponseCollectorBindServiceByType, fragment, 1, 0);
            }
            var surveyResponseCollectorDataQueryService = surveyResponseCollectorDataQueryServiceFactory.Create();

            //获取现有的所有收集器
            var currentCollectors = GetAllCollector(endpoint, cancellationToken);
            ConcurrentBag<SurveyResponseCollector> deleteCollectors = new ConcurrentBag<SurveyResponseCollector>();

      
            await ParallelHelper.ForEach(currentCollectors, 5,
                async(collector)=>
                {
                    //检查在源中是否存在，如果不存在，则需要删除并解绑收集器
                    if (!await surveyResponseCollectorDataQueryService.Exist(endpoint.Configuration, collector.SurveyID, cancellationToken))
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
                        await DeleteCollector(endpoint, collector.ID, cancellationToken);
                        await surveyResponseCollectorBindServiceFactory.Create().UnBinding(endpoint.Configuration, collector, cancellationToken);
                        scope.Complete();
                    }
                }
                );



            //从源获取所有关联的收集器数据
            var collectorDatas=surveyResponseCollectorDataQueryService.QueryAll(endpoint.Configuration, cancellationToken);
            //转换成收集器，检查是否已经存在收集器，如果不存在，则新建，执行绑定
            await ParallelHelper.ForEach(collectorDatas, 5,
                async (data) =>
                {
                    var newCollector = await surveyResponseCollectorFactory.Create().Create(data, cancellationToken);
                    var existCollector = await GetCollector(endpoint, newCollector.SurveyID, cancellationToken);
                    if (existCollector == null)
                    {
                        await using (DBTransactionScope scope = new DBTransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                        {

                            await AddCollector(endpoint, newCollector, cancellationToken);
                            await surveyResponseCollectorBindServiceFactory.Create().Binding(endpoint.Configuration, newCollector, cancellationToken);

                            scope.Complete();
                        }
                    }
                }
                );

        }

        public async Task UnBindCollector(SurveyResponseCollectorEndpoint endpoint, string collectorData, CancellationToken cancellationToken = default)
        {
            if (!SurveyResponseCollectorFactories.TryGetValue(endpoint.Type, out IFactory<ISurveyResponseCollectorFactory> surveyResponseCollectorFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyResponseCollectorFactoryByType,
                    DefaultFormatting = "找不到类型为{0}的Survey响应收集器工厂，发生位置为{1}",
                    ReplaceParameters = new List<object>() { endpoint.Type, $"{this.GetType().FullName}.SurveyResponseCollectorFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyResponseCollectorFactoryByType, fragment, 1, 0);
            }

            if (!SurveyResponseCollectorBindServiceFactories.TryGetValue(endpoint.Type, out IFactory<ISurveyResponseCollectorBindService> surveyResponseCollectorBindServiceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = SurveyTextCodes.NotFoundSurveyResponseCollectorBindServiceByType,
                    DefaultFormatting = "找不到类型为{0}的Survey响应收集器绑定服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { endpoint.Type, $"{this.GetType().FullName}.SurveyResponseCollectorBindServiceFactories" }
                };
                throw new UtilityException((int)SurveyErrorCodes.NotFoundSurveyResponseCollectorBindServiceByType, fragment, 1, 0);
            }

            var collector = await surveyResponseCollectorFactory.Create().Create(collectorData, cancellationToken);

            var existCollector = await GetCollector(endpoint, collector.SurveyID, cancellationToken);

            if (existCollector != null)
            {
                await using (DBTransactionScope scope = new DBTransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    await DeleteCollector(endpoint, existCollector.ID, cancellationToken);
                    await surveyResponseCollectorBindServiceFactory.Create().UnBinding(endpoint.Configuration, existCollector, cancellationToken);

                    scope.Complete();
                }
            }
        }
    }
}
