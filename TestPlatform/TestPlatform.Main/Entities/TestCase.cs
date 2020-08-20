using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Collections;
using MSLibrary.LanguageTranslate;
using MSLibrary.Transaction;
using FW.TestPlatform.Main.Entities.DAL;
using FW.TestPlatform.Main.DTOModel;
using MSLibrary.Serializer;
using FW.TestPlatform.Main.Configuration;
using System.Linq;
using MSLibrary.CommandLine.SSH.DAL;
using MSLibrary.CommandLine.SSH;

namespace FW.TestPlatform.Main.Entities
{
    /// <summary>
    /// 测试案例
    /// </summary>
    public class TestCase : EntityBase<ITestCaseIMP>
    {
        private static IFactory<ITestCaseIMP>? _testCaseIMPFactory;

        public static IFactory<ITestCaseIMP> TestCaseIMPFactory
        {
            set 
            {
                _testCaseIMPFactory = value;
            }
        }
        public override IFactory<ITestCaseIMP>? GetIMPFactory()
        {
            return _testCaseIMPFactory;
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
        /// 测试引擎类型
        /// </summary>
        public string EngineType
        {
            get
            {

                return GetAttribute<string>(nameof(EngineType));
            }
            set
            {
                SetAttribute<string>(nameof(EngineType), value);
            }
        }

        /// <summary>
        /// 测试配置
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
        /// Master主机ID
        /// </summary>
        public Guid MasterHostID
        {
            get
            {

                return GetAttribute<Guid>(nameof(MasterHostID));
            }
            set
            {
                SetAttribute<Guid>(nameof(MasterHostID), value);
            }
        }
        /// <summary>
        /// Master主机
        /// </summary>
        public TestHost MasterHost
        {
            get
            {

                return GetAttribute<TestHost>(nameof(MasterHost));
            }
            set
            {
                SetAttribute<TestHost>(nameof(MasterHost), value);
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public TestCaseStatus Status
        {
            get
            {

                return GetAttribute<TestCaseStatus>(nameof(Status));
            }
            set
            {
                SetAttribute<TestCaseStatus>(nameof(Status), value);
            }
        }

        /// <summary>
        /// 所有者ID
        /// </summary>
        public Guid OwnerID
        {
            get
            {

                return GetAttribute<Guid>(nameof(OwnerID));
            }
            set
            {
                SetAttribute<Guid>(nameof(OwnerID), value);
            }
        }

        /// <summary>
        /// 所有者
        /// </summary>
        public User Owner
        {
            get
            {

                return GetAttribute<User>(nameof(Owner));
            }
            set
            {
                SetAttribute<User>(nameof(Owner), value);
            }
        }

        /// <summary>
        /// 测试案例ID
        /// </summary>
        public Guid? TestCaseHistoryID
        {
            get
            {

                return GetAttribute<Guid?>(nameof(TestCaseHistoryID));
            }
            set
            {
                SetAttribute<Guid?>(nameof(TestCaseHistoryID), value);
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return GetAttribute<DateTime>(nameof(CreateTime));
            }
            set
            {
                SetAttribute<DateTime>(nameof(CreateTime), value);
            }
        }



        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>(nameof(ModifyTime));
            }
            set
            {
                SetAttribute<DateTime>(nameof(ModifyTime), value);
            }
        }

        public IAsyncEnumerable<TestCaseSlaveHost> GetAllSlaveHosts(CancellationToken cancellationToken = default)
        {
            return _imp.GetAllSlaveHosts(this, cancellationToken);
        }

        public async Task Add(CancellationToken cancellationToken = default)
        {
            await _imp.Add(this,cancellationToken);
        }


        public async Task AddHistory(TestCaseHistory history, CancellationToken cancellationToken = default)
        {
            await _imp.AddHistory(this, history);
        }

        public async Task Update(CancellationToken cancellationToken = default)
        {
            await _imp.Update(this, cancellationToken);
        }


        public async Task Delete(CancellationToken cancellationToken = default)
        {
            await _imp.Delete(this, cancellationToken);
        }

        //public async Task DeleteMultiple(List<TestCase> list, CancellationToken cancellationToken = default)
        //{
        //    await _imp.DeleteMultiple(list, cancellationToken);
        //}

        public async Task Run(CancellationToken cancellationToken = default)
        {
            await _imp.Run(this, cancellationToken);
        }
        public async Task Stop(CancellationToken cancellationToken = default)
        {
            await _imp.Stop(this, cancellationToken);
        }
        public async Task<bool> IsEngineRun(CancellationToken cancellationToken = default)
        {
            return await _imp.IsEngineRun(this, cancellationToken);
        }
        public async Task<string> GetMasterLog(CancellationToken cancellationToken = default)
        {
            return await _imp.GetMasterLog(this, cancellationToken);
        }
        public async Task<string> GetSlaveLog(Guid slaveID, int idx, CancellationToken cancellationToken = default)
        {
            return await _imp.GetSlaveLog(this, slaveID, idx, cancellationToken);
        }
        public async Task AddSlaveHost(TestCaseSlaveHost slaveHost, CancellationToken cancellationToken = default)
        {
            await _imp.AddSlaveHost(this, slaveHost, cancellationToken);
        }
        public IAsyncEnumerable<TestCaseSlaveHost> GetAllSlaveHosts(TestCase tCase, CancellationToken cancellationToken = default)
        {
            return _imp.GetAllSlaveHosts(this, cancellationToken);
        }
        public async Task UpdateSlaveHost(TestCaseSlaveHost slaveHost, CancellationToken cancellationToken = default)
        {
            await _imp.UpdateSlaveHost(this, slaveHost, cancellationToken);
        }
        public async Task UpdateNetGatewayDataFormat(TestCaseHistory history, CancellationToken cancellationToken = default)
        {
            await _imp.UpdateNetGatewayDataFormat(this, history, cancellationToken);
        }
        public async Task<int> TransferNetGatewayDataFile(Guid historyId, CancellationToken cancellationToken = default)
        {
            return await _imp.TransferNetGatewayDataFile(this, historyId, cancellationToken);
        }

        public async Task<string> CheckNetGatewayDataAnalysisStatus(Guid historyId, CancellationToken cancellationToken = default)
        {
            return await _imp.CheckNetGatewayDataAnalysisStatus(this, historyId, cancellationToken);
        }
        public async Task<TestCaseSlaveHost?> GetSlaveHost(Guid slaveHostId, CancellationToken cancellationToken = default)
        {
            return await _imp.GetSlaveHost(this, slaveHostId, cancellationToken);
        }
        public async Task<QueryResult<TestCaseHistory>> GetHistories(Guid caseID, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _imp.GetHistories(caseID, page, pageSize, cancellationToken);
        }
        public async Task DeleteHistory(Guid historyID, CancellationToken cancellationToken = default)
        {
            await _imp.DeleteHistory(this, historyID, cancellationToken);
        }
        public async Task DeleteSlaveHost(Guid slaveHostID, CancellationToken cancellationToken = default)
        {
            await _imp.DeleteSlaveHost(this, slaveHostID, cancellationToken);
        }

        public async Task<TestCaseHistory?> GetHistory(Guid historyID, CancellationToken cancellationToken = default)
        {
            return await _imp.GetHistory(this, historyID, cancellationToken);
        }
        public async Task DeleteSlaveHosts(List<Guid> ids, CancellationToken cancellationToken = default)
        {
            await _imp.DeleteSlaveHosts(this, ids, cancellationToken);
        }
        public async Task DeleteHistories(List<Guid> ids, CancellationToken cancellationToken = default)
        {
            await _imp.DeleteHistories(this, ids, cancellationToken);
        }
        public async Task<List<TestCaseHistory>> GetHistoriesByCaseIdAndIds(List<Guid> ids, CancellationToken cancellationToken = default)
        {
            return await _imp.GetHistoriesByCaseIdAndIds(this, ids, cancellationToken);
        }
    }

    public interface ITestCaseIMP
    {
        Task Add(TestCase tCase, CancellationToken cancellationToken = default);
        Task Delete(TestCase tCase, CancellationToken cancellationToken = default);
        //Task DeleteMultiple(List<TestCase> list, CancellationToken cancellationToken = default);
        Task Update(TestCase tCase, CancellationToken cancellationToken = default);
        Task AddSlaveHost(TestCase tCase,TestCaseSlaveHost slaveHost, CancellationToken cancellationToken = default);
        Task DeleteSlaveHost(TestCase tCase,Guid slaveHostID, CancellationToken cancellationToken = default);
        Task UpdateSlaveHost(TestCase tCase, TestCaseSlaveHost slaveHost, CancellationToken cancellationToken = default);
        Task UpdateNetGatewayDataFormat(TestCase tCase, TestCaseHistory history, CancellationToken cancellationToken = default);
        Task<int> TransferNetGatewayDataFile(TestCase tCase, Guid historyId, CancellationToken cancellationToken = default);
        Task<string> CheckNetGatewayDataAnalysisStatus(TestCase tCase, Guid historyId, CancellationToken cancellationToken = default);
        IAsyncEnumerable<TestCaseSlaveHost> GetAllSlaveHosts(TestCase tCase, CancellationToken cancellationToken = default);
        Task<TestCaseSlaveHost?> GetSlaveHost(TestCase tCase,Guid slaveHostID, CancellationToken cancellationToken = default);
        Task AddHistory(TestCase tCase, TestCaseHistory history, CancellationToken cancellationToken = default);
        Task UpdateHistory(TestCase tCase, TestCaseHistory history, CancellationToken cancellationToken = default);
        Task DeleteHistory(TestCase tCase, Guid historyID, CancellationToken cancellationToken = default);
        Task<TestCaseHistory?> GetHistory(TestCase tCase, Guid historyID, CancellationToken cancellationToken = default);
        Task<QueryResult<TestCaseHistory>> GetHistories(Guid caseID, int page, int pageSize, CancellationToken cancellationToken = default);
        Task Run(TestCase tCase, CancellationToken cancellationToken = default);
        Task Stop(TestCase tCase, CancellationToken cancellationToken = default);
        Task<bool> IsEngineRun(TestCase tCase, CancellationToken cancellationToken = default);
        Task<string> GetMasterLog(TestCase tCase, CancellationToken cancellationToken = default);
        Task<string> GetSlaveLog(TestCase tCase,Guid slaveID,int idx, CancellationToken cancellationToken = default);
        Task DeleteSlaveHosts(TestCase tCase, List<Guid> ids, CancellationToken cancellationToken = default);
        Task DeleteHistories(TestCase tCase, List<Guid> ids, CancellationToken cancellationToken = default);
        Task<List<TestCaseHistory>> GetHistoriesByCaseIdAndIds(TestCase tCase, List<Guid> ids, CancellationToken cancellationToken = default); 
    }


    [Injection(InterfaceType = typeof(ITestCaseIMP), Scope = InjectionScope.Transient)]
    public class TestCaseIMP : ITestCaseIMP
    {
        public static Dictionary<string, IFactory<ITestCaseHandleService>> HandleServiceFactories = new Dictionary<string, IFactory<ITestCaseHandleService>>();

        private ITestCaseStore _testCaseStore;
        private ITestCaseSlaveHostStore _testCaseSlaveHostStore;
        private ITestHostRepository _testHostRepository;
        private ITestCaseHistoryStore _testCaseHistoryStore;
        private ISystemConfigurationService _systemConfigurationService;
        private ISSHEndpointStore _sSHEndpointStore;

        public TestCaseIMP(ITestCaseStore testCaseStore, ITestCaseSlaveHostStore testCaseSlaveHostStore, ITestHostRepository testHostRepository, ITestCaseHistoryStore testCaseHistoryStore, ISystemConfigurationService systemConfigurationService, ISSHEndpointStore sSHEndpointStore)
        {
            _testCaseStore = testCaseStore;
            _testCaseSlaveHostStore = testCaseSlaveHostStore;
            _testHostRepository = testHostRepository;
            _testCaseHistoryStore = testCaseHistoryStore;
            _systemConfigurationService = systemConfigurationService;
            _sSHEndpointStore = sSHEndpointStore;
        }

        public async Task Add(TestCase tCase, CancellationToken cancellationToken = default)
        {
            var handleService = getHandleService(tCase.EngineType);
            var host=await _testHostRepository.QueryByID(tCase.MasterHostID, cancellationToken);
            if (host==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestHostByID,
                    DefaultFormatting = "找不到Id为{0}的测试主机",
                    ReplaceParameters = new List<object>() { tCase.MasterHostID.ToString() }
                };
                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestHostByID, fragment, 1, 0);
            }
            //检查是否有名称重复的
            var newId = await _testCaseStore.QueryByNameNoLock(tCase.Name, cancellationToken);
            if (newId != null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.ExistTestCaseByName,
                    DefaultFormatting = "已经存在名称为{0}的测试用例",
                    ReplaceParameters = new List<object>() { tCase.Name }
                };
                throw new UtilityException((int)TestPlatformErrorCodes.ExistTestCaseByName, fragment, 1, 0);
            }
            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel=System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {
                tCase.OwnerID = await _systemConfigurationService.GetDefaultUserIDAsync();
                await _testCaseStore.Add(tCase, cancellationToken);
                //检查是否有名称重复的
                newId = await _testCaseStore.QueryByNameNoLock(tCase.Name, cancellationToken);
                if (newId != tCase.ID)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TestPlatformTextCodes.ExistTestCaseByName,
                        DefaultFormatting = "已经存在名称为{0}的测试用例",
                        ReplaceParameters = new List<object>() { tCase.Name }
                    };
                    throw new UtilityException((int)TestPlatformErrorCodes.ExistTestCaseByName, fragment, 1, 0);
                }
                scope.Complete();
            }   
        }

        //public async Task AddHistory(TestCase tCase, TestCaseHistory history, CancellationToken cancellationToken = default)
        //{
        //    history.CaseID = tCase.ID;
        //    await _testCaseHistoryRepository.Add(history, cancellationToken);
        //}

        public async Task AddHistory(TestCase tCase, TestCaseHistory history, CancellationToken cancellationToken = default)
        {
            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {

                //修改case状态为完成
                await _testCaseStore.UpdateStatus(tCase.ID, TestCaseStatus.NoRun);

               //插入历史数据
                await _testCaseHistoryStore.Add(history);

                scope.Complete();
            }
        }

        public async Task AddSlaveHost(TestCase tCase, TestCaseSlaveHost slaveHost, CancellationToken cancellationToken = default)
        {
            var host = await _testHostRepository.QueryByID(slaveHost.HostID, cancellationToken);
            if (host == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestHostByID,
                    DefaultFormatting = "找不到Id为{0}的测试主机",
                    ReplaceParameters = new List<object>() { slaveHost.HostID.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestHostByID, fragment, 1, 0);
            }
            Guid? newId = await _testCaseSlaveHostStore.QueryByNameNoLock(tCase.ID, slaveHost.SlaveName, cancellationToken);
            if(newId != null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.ExistTestCaseSlaveByName,
                    DefaultFormatting = "已经存在名称为{0}的主测试从机",
                    ReplaceParameters = new List<object>() { slaveHost.SlaveName }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.ExistTestCaseSlaveName, fragment, 1, 0);
            }
            await using(DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel= System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {
                slaveHost.TestCaseID = tCase.ID;
                await _testCaseSlaveHostStore.Add(slaveHost, cancellationToken);
                newId = await _testCaseSlaveHostStore.QueryByNameNoLock(tCase.ID, slaveHost.SlaveName, cancellationToken);
                if (newId != slaveHost.ID)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TestPlatformTextCodes.ExistTestCaseSlaveByName,
                        DefaultFormatting = "已经存在名称为{0}的主测试从机",
                        ReplaceParameters = new List<object>() { slaveHost.SlaveName }
                    };
                    throw new UtilityException((int)TestPlatformErrorCodes.ExistTestCaseSlaveName, fragment, 1, 0);
                }
                scope.Complete();
            }
        }

        public async Task Delete(TestCase tCase, CancellationToken cancellationToken = default)
        {
            if (tCase.Status != TestCaseStatus.NoRun)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.StatusErrorOnTestCaseDelete,
                    DefaultFormatting = "只能在状态{0}的时候允许删除测试案例，当前测试案例{1}的状态为{2}",
                    ReplaceParameters = new List<object>() { TestCaseStatus.NoRun.ToString(), tCase.ID.ToString(), tCase.Status.ToString() }
                };
                throw new UtilityException((int)TestPlatformErrorCodes.StatusErrorOnTestCaseDelete, fragment, 1, 0);
            }
            await _testCaseStore.Delete(tCase.ID, cancellationToken);
        }

        public async Task DeleteHistory(TestCase tCase, Guid historyID, CancellationToken cancellationToken = default)
        {
            TestCaseHistory? history = await _testCaseHistoryStore.QueryByCase(tCase.ID, historyID,cancellationToken);
            if(history == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseHistoryByID,
                    DefaultFormatting = "找不到测试历史Id为{0}并且测试用例Id为{1}的历史",
                    ReplaceParameters = new List<object>() { tCase.ID.ToString(),historyID.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseHistoryById, fragment, 1, 0);
            }
            await _testCaseHistoryStore.Delete(historyID, cancellationToken);
        }

        public async Task UpdateNetGatewayDataFormat(TestCase tCase, TestCaseHistory tHistory, CancellationToken cancellationToken = default)
        { 
            await _testCaseHistoryStore.UpdateNetGatewayDataFormat(tHistory, cancellationToken);
        }

        public async Task DeleteSlaveHost(TestCase tCase, Guid slaveHostID, CancellationToken cancellationToken = default)
        {
            var slaveHost = await _testCaseSlaveHostStore.QueryByCase(tCase.ID, slaveHostID, cancellationToken);
            if (slaveHost == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundSlaveHostInCase,
                    DefaultFormatting = "在id为{0}的测试案例中找不到id为{1}的从测试主机",
                    ReplaceParameters = new List<object>() { tCase.ID.ToString(), slaveHostID .ToString()}
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundSlaveHostInCase, fragment, 1, 0);
            }
            if (tCase.Status != TestCaseStatus.NoRun)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.StatusErrorOnTestCaseDelete,
                    DefaultFormatting = "只能在状态{0}的时候允许删除测试案例，当前测试案例{1}的状态为{2}",
                    ReplaceParameters = new List<object>() { TestCaseStatus.NoRun.ToString(), tCase.ID.ToString(), tCase.Status.ToString() }
                };
                throw new UtilityException((int)TestPlatformErrorCodes.StatusErrorOnTestCaseDelete, fragment, 1, 0);
            }
            await _testCaseSlaveHostStore.Delete(slaveHost.ID, cancellationToken);    
        }

        public IAsyncEnumerable<TestCaseSlaveHost> GetAllSlaveHosts(TestCase tCase, CancellationToken cancellationToken = default)
        {
            return _testCaseSlaveHostStore.QueryByCase(tCase.ID, cancellationToken);
        }

        public async Task<QueryResult<TestCaseHistory>> GetHistories(Guid caseID, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _testCaseHistoryStore.QueryByPage(caseID, page, pageSize, cancellationToken);
        }

        public async Task<TestCaseHistory?> GetHistory(TestCase tCase, Guid historyID, CancellationToken cancellationToken = default)
        {
            TestCaseHistory? testHistory;          
            testHistory = await _testCaseHistoryStore.QueryByCase(tCase.ID, historyID, cancellationToken);
            if(testHistory == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseHistoryByID,
                    DefaultFormatting = "找不到测试历史Id为{0}并且测试用例Id为{1}的历史",
                    ReplaceParameters = new List<object>() { tCase.ID.ToString(), historyID.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseHistoryById, fragment, 1, 0);
            }
            return testHistory;
        }

        public async Task<string> GetMasterLog(TestCase tCase, CancellationToken cancellationToken = default)
        {
            var handleService = getHandleService(tCase.EngineType);
            return await handleService.GetMasterLog(tCase.MasterHost, cancellationToken);
        }

        public async Task<TestCaseSlaveHost?> GetSlaveHost(TestCase tCase, Guid slaveHostID, CancellationToken cancellationToken = default)
        {
            var host= await _testCaseSlaveHostStore.QueryByCase(tCase.ID, slaveHostID, cancellationToken);
            if (host == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundSlaveHostInCase,
                    DefaultFormatting = "在id为{0}的测试案例中找不到id为{1}的从测试主机",
                    ReplaceParameters = new List<object>() { tCase.ID.ToString(), slaveHostID.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundSlaveHostInCase, fragment, 1, 0);
            }

            return host;
        }

        public async Task<string> GetSlaveLog(TestCase tCase, Guid slaveID, int idx, CancellationToken cancellationToken = default)
        {
            var slaveHost=await GetSlaveHost(tCase, slaveID, cancellationToken);
            if (slaveHost==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundSlaveHostInCase,
                    DefaultFormatting = "在id为{0}的测试案例中找不到id为{1}的从测试主机",
                    ReplaceParameters = new List<object>() { tCase.ID.ToString(), slaveID.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundSlaveHostInCase, fragment, 1, 0);
            }
            //开始运行获取log的程序
            var handleService = getHandleService(tCase.EngineType);
            return await handleService.GetSlaveLog(slaveHost.Host,idx, cancellationToken);
        }

        public async Task<bool> IsEngineRun(TestCase tCase, CancellationToken cancellationToken = default)
        {
            var handleService = getHandleService(tCase.EngineType);
            return await handleService.IsEngineRun(tCase, cancellationToken);
        }

        public async Task Run(TestCase tCase, CancellationToken cancellationToken = default)
        {
            if (tCase.Status != TestCaseStatus.NoRun)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.StatusErrorOnTestCaseRun,
                    DefaultFormatting = "只能在状态{0}的时候允许运行测试案例，当前测试案例{1}的状态为{2}",
                    ReplaceParameters = new List<object>() { TestCaseStatus.NoRun.ToString(), tCase.ID.ToString(), tCase.Status.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.StatusErrorOnTestCaseRun, fragment, 1, 0);
            }

            var handleService = getHandleService(tCase.EngineType);

            List<Guid> hostIDs = new List<Guid>();
            var slaveHosts = GetAllSlaveHosts(tCase, cancellationToken);

            await foreach (var item in slaveHosts)
            {
                hostIDs.Add(item.HostID);
            }

            hostIDs.Add(tCase.MasterHostID);


            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
            {
                var existsCases = await _testCaseStore.QueryCountNolockByStatus(TestCaseStatus.Running, hostIDs, cancellationToken);

                if (existsCases.Count >= 1)
                {

                    var fragment = new TextFragment()
                    {
                        Code = TestPlatformTextCodes.TestHostHasRunning,
                        DefaultFormatting = "包含的测试主机已经被执行，相关测试案例为{0}",
                        ReplaceParameters = new List<object>() { existsCases.ToDisplayString((c) => c.Name, () => ",") }
                    };

                    throw new UtilityException((int)TestPlatformErrorCodes.TestHostHasRunning, fragment, 1, 0);
                }

                await handleService.Run(tCase, cancellationToken);
                //await _testCaseStore.UpdateStatus(tCase.ID, TestCaseStatus.Running, cancellationToken);
                await _testCaseStore.UpdateHistoryIdAndStatus(tCase.ID, Guid.NewGuid(), TestCaseStatus.Running, cancellationToken);

                scope.Complete();
            }   
        }

        public async Task Stop(TestCase tCase, CancellationToken cancellationToken = default)
        {
            if (tCase.Status != TestCaseStatus.Running)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.StatusErrorOnTestCaseStop,
                    DefaultFormatting = "只能在状态{0}的时候允许停止测试案例，当前测试案例{1}的状态为{2}",
                    ReplaceParameters = new List<object>() { TestCaseStatus.Running.ToString(), tCase.ID.ToString(), tCase.Status.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.StatusErrorOnTestCaseStop, fragment, 1, 0);
            }

            var handleService = getHandleService(tCase.EngineType);

            await handleService.Stop(tCase, cancellationToken);

            await _testCaseStore.UpdateStatus(tCase.ID, TestCaseStatus.NoRun, cancellationToken);
        }

        public async Task Update(TestCase tCase, CancellationToken cancellationToken = default)
        {
            if (tCase.Status != TestCaseStatus.NoRun)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.StatusErrorOnTestCaseUpdate,
                    DefaultFormatting = "只能在状态{0}的时候修改测试案例，当前测试案例{1}的状态为{2}",
                    ReplaceParameters = new List<object>() { TestCaseStatus.NoRun.ToString(), tCase.ID.ToString(), tCase.Status.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.StatusErrorOnTestCaseUpdate, fragment, 1, 0);

            }
            var handleService = getHandleService(tCase.EngineType);
            var host = await _testHostRepository.QueryByID(tCase.MasterHostID, cancellationToken);
            if (host == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestHostByID,
                    DefaultFormatting = "找不到Id为{0}的测试主机",
                    ReplaceParameters = new List<object>() { tCase.MasterHostID.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestHostByID, fragment, 1, 0);
            }
            //检查是否有名称重复的
            var newId = await _testCaseStore.QueryByNameNoLock(tCase.Name, cancellationToken);
            if (newId != null && tCase.ID != newId)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.ExistTestCaseByName,
                    DefaultFormatting = "已经存在名称为{0}的测试案例",
                    ReplaceParameters = new List<object>() { tCase.Name }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.ExistTestCaseByName, fragment, 1, 0);

            }

            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {
                await _testCaseStore.Update(tCase, cancellationToken);
                //检查是否有名称重复的
                newId = await _testCaseStore.QueryByNameNoLock(tCase.Name, cancellationToken);
                if (newId != null && tCase.ID != newId)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TestPlatformTextCodes.ExistTestCaseByName,
                        DefaultFormatting = "已经存在名称为{0}的测试数据源",
                        ReplaceParameters = new List<object>() { tCase.Name }
                    };

                    throw new UtilityException((int)TestPlatformErrorCodes.ExistTestCaseByName, fragment, 1, 0);
                }
                scope.Complete();
            }
        }

        public Task UpdateHistory(TestCase tCase, TestCaseHistory history, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateSlaveHost(TestCase tCase, TestCaseSlaveHost slaveHost, CancellationToken cancellationToken = default)
        {
            if (tCase.Status != TestCaseStatus.NoRun)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.StatusErrorOnTestCaseUpdate,
                    DefaultFormatting = "只能在状态{0}的时候修改测试案例，当前测试案例{1}的状态为{2}",
                    ReplaceParameters = new List<object>() { TestCaseStatus.NoRun.ToString(), tCase.ID.ToString(), tCase.Status.ToString() }
                };
                throw new UtilityException((int)TestPlatformErrorCodes.StatusErrorOnTestCaseUpdate, fragment, 1, 0);
            }

            var host = await _testHostRepository.QueryByID(slaveHost.HostID, cancellationToken);
            if (host == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestHostByID,
                    DefaultFormatting = "找不到Id为{0}的测试主机",
                    ReplaceParameters = new List<object>() { tCase.MasterHostID.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestHostByID, fragment, 1, 0);
            }
            //检查是否有名称重复的
            var newId = await _testCaseSlaveHostStore.QueryByNameNoLock(tCase.ID, slaveHost.SlaveName, cancellationToken);
            if (newId != null && slaveHost.ID != newId)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.ExistTestCaseSlaveByName,
                    DefaultFormatting = "已经存在名称为{0}的从主机",
                    ReplaceParameters = new List<object>() { slaveHost.SlaveName }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.ExistTestCaseSlaveName, fragment, 1, 0);

            }
            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {
                slaveHost.TestCaseID = tCase.ID;
                await _testCaseSlaveHostStore.Update(slaveHost, cancellationToken);
                //检查是否有名称重复的
                newId = await _testCaseSlaveHostStore.QueryByNameNoLock(tCase.ID, slaveHost.SlaveName, cancellationToken);
                if (newId != null && slaveHost.ID != newId)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TestPlatformTextCodes.ExistTestCaseSlaveByName,
                        DefaultFormatting = "已经存在名称为{0}的从主机",
                        ReplaceParameters = new List<object>() { slaveHost.SlaveName }
                    };

                    throw new UtilityException((int)TestPlatformErrorCodes.ExistTestCaseSlaveName, fragment, 1, 0);

                }
                scope.Complete();
            }
            
        }

        public async Task DeleteSlaveHosts(TestCase tCase, List<Guid> ids, CancellationToken cancellationToken = default)
        {
            List<TestCaseSlaveHost> slaveHosts = await _testCaseSlaveHostStore.QueryByCaseIdAndSlaveHostIds(tCase.ID, ids, cancellationToken);
            if (slaveHosts.Count == 0 || slaveHosts.Count != ids.Count)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestHostByID,
                    DefaultFormatting = "批量删除异常, 不能找到所有测试用例Id为{0}的测试从机Id为{1}",
                    ReplaceParameters = new List<object>() { tCase.ID.ToString(), string.Join(",", ids.ToArray()) }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestHostByID, fragment, 1, 0);
            }
            await _testCaseSlaveHostStore.DeleteSlaveHosts(ids, cancellationToken);
        }
        public async Task DeleteHistories(TestCase tCase, List<Guid> ids, CancellationToken cancellationToken = default)
        {
            List<TestCaseHistory> histories = await _testCaseHistoryStore.QueryByCaseIdAndHistoryIds(tCase.ID, ids, cancellationToken);
            if (histories.Count == 0 || histories.Count != ids.Count)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseHistoryByID,
                    DefaultFormatting = "批量删除异常, Id为{0}的测试用例,不能找到所有Id为{1}历史记录",
                    ReplaceParameters = new List<object>() { tCase.ID.ToString(), string.Join(",", ids.ToArray()) }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseHistoryById, fragment, 1, 0);
            }
            await _testCaseHistoryStore.DeleteHistories(ids, cancellationToken);
        }

        public async Task<List<TestCaseHistory>> GetHistoriesByCaseIdAndIds(TestCase tCase, List<Guid> ids, CancellationToken cancellationToken = default)
        {
            List<TestCaseHistory> histories = await _testCaseHistoryStore.QueryByCaseIdAndHistoryIds(tCase.ID, ids, cancellationToken);
            if (histories.Count == 0 || histories.Count != ids.Count)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseHistoryByID,
                    DefaultFormatting = "Id为{0}的测试用例,不能找到所有Id为{1}的历史记录",
                    ReplaceParameters = new List<object>() { tCase.ID.ToString(), string.Join(",", ids.ToArray()) }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseHistoryById, fragment, 1, 0);
            }
            return histories;
        }

        public async Task<int> TransferNetGatewayDataFile(TestCase tCase, Guid historyId, CancellationToken cancellationToken = default)
        {
            TestCaseHistory? history = await _testCaseHistoryStore.QueryByCase(tCase.ID, historyId, cancellationToken);
            if (history == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseHistoryByID,
                    DefaultFormatting = "找不到测试历史Id为{0}并且测试用例Id为{1}的历史",
                    ReplaceParameters = new List<object>() { tCase.ID.ToString(), historyId.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseHistoryById, fragment, 1, 0);
            }
            string sshEndpointName = await _systemConfigurationService.GetNetGatewayDataSSHEndpointAsync(cancellationToken);
            SSHEndpoint? sshEndpoint = await _sSHEndpointStore.QueryByName(sshEndpointName, cancellationToken);
            if (sshEndpoint == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundSSHEndPointByName,
                    DefaultFormatting = "找不到名称为{0}的SSH终结点",
                    ReplaceParameters = new List<object>() { sshEndpointName }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundSSHEndPointByName, fragment, 1, 0);
            }
            string tempPath = await _systemConfigurationService.GetNetGatewayDataTempFolderAsync(cancellationToken);
            string path = await _systemConfigurationService.GetNetGatewayDataFolderAsync(cancellationToken);
            return await sshEndpoint.TransferFile(async(oldFileName)=>
            {
                return await Task.FromResult($"01_{tCase.ID.ToString()}_{historyId.ToString()}_{Guid.NewGuid().ToString()}.cap");
            }, tempPath, path, 30, cancellationToken);
        }

        public async Task<string> CheckNetGatewayDataAnalysisStatus(TestCase tCase, Guid historyId, CancellationToken cancellationToken = default)
        {
            TestCaseHistory? history = await _testCaseHistoryStore.QueryByCase(tCase.ID, historyId, cancellationToken);
            if (history == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseHistoryByID,
                    DefaultFormatting = "找不到测试历史Id为{0}并且测试用例Id为{1}的历史",
                    ReplaceParameters = new List<object>() { tCase.ID.ToString(), historyId.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseHistoryById, fragment, 1, 0);
            }
            string sshEndpointName = await _systemConfigurationService.GetNetGatewayDataSSHEndpointAsync(cancellationToken);
            SSHEndpoint? sshEndpoint = await _sSHEndpointStore.QueryByName(sshEndpointName, cancellationToken);
            if (sshEndpoint == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundSSHEndPointByName,
                    DefaultFormatting = "找不到名称为{0}的SSH终结点",
                    ReplaceParameters = new List<object>() { sshEndpointName }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundSSHEndPointByName, fragment, 1, 0);
            }
            string path = await _systemConfigurationService.GetNetGatewayDataFolderAsync(cancellationToken);
            string command = string.Format("find {0} -type f -iname \"*{1}*\"", path, historyId);
            string rel = await sshEndpoint.ExecuteCommand(command, 30, cancellationToken);
            return rel;
        }

        private ITestCaseHandleService getHandleService(string engineType)
        {
            if (!HandleServiceFactories.TryGetValue(engineType,out IFactory<ITestCaseHandleService>? serviceFactory))
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTestCaseHandleServiceByEngine,
                    DefaultFormatting = "找不到测试引擎为{0}的测试案例处理服务，发生位置为{1}",
                    ReplaceParameters = new List<object>() { engineType, $"{this.GetType().FullName}.HandleServiceFactories" }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTestCaseHandleServiceByEngine, fragment, 1, 0);
            }

            return serviceFactory.Create();
        }
    }


    public interface ITestCaseHandleService
    {
        Task Run(TestCase tCase, CancellationToken cancellationToken = default);
        Task Stop(TestCase tCase, CancellationToken cancellationToken = default);
        Task<bool> IsEngineRun(TestCase tCase, CancellationToken cancellationToken = default);
        Task<string> GetMasterLog(TestHost host, CancellationToken cancellationToken = default);
        Task<string> GetSlaveLog(TestHost host, int idx, CancellationToken cancellationToken = default);
    }
}
