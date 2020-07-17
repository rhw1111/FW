using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FW.TestPlatform.Main.Entities.DAL;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Transaction;

namespace FW.TestPlatform.Main.Entities
{
    /// <summary>
    /// 测试实例的Slave主机
    /// </summary>
    public class TestCaseSlaveHost : EntityBase<ITestCaseSlaveHostIMP>
    {
        private static IFactory<ITestCaseSlaveHostIMP>? _testCaseSlaveHostIMPFactory;

        public static IFactory<ITestCaseSlaveHostIMP> TestCaseSlaveHostIMPFactory
        {
            set
            {
                _testCaseSlaveHostIMPFactory = value;
            }
        }
        public override IFactory<ITestCaseSlaveHostIMP>? GetIMPFactory()
        {
            return _testCaseSlaveHostIMPFactory;
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
        /// 所属主机ID
        /// </summary>
        public Guid HostID
        {
            get
            {

                return GetAttribute<Guid>(nameof(HostID));
            }
            set
            {
                SetAttribute<Guid>(nameof(HostID), value);
            }
        }
        /// <summary>
        /// 所属主机
        /// </summary>
        public TestHost Host
        {
            get
            {

                return GetAttribute<TestHost>(nameof(Host));
            }
            set
            {
                SetAttribute<TestHost>(nameof(Host), value);
            }
        }

        /// <summary>
        /// 所属测试用例ID
        /// </summary>
        public Guid TestCaseID
        {
            get
            {

                return GetAttribute<Guid>(nameof(TestCaseID));
            }
            set
            {
                SetAttribute<Guid>(nameof(TestCaseID), value);
            }
        }

        /// <summary>
        /// 所属测试用例
        /// </summary>
        public TestCase TestCase
        {
            get
            {

                return GetAttribute<TestCase>(nameof(TestCase));
            }
            set
            {
                SetAttribute<TestCase>(nameof(TestCase), value);
            }
        }

        /// <summary>
        /// 测试机名称
        /// 通过该名称与副本Index，来区分每个Slave
        /// </summary>
        public string SlaveName
        {
            get
            {

                return GetAttribute<string>(nameof(SlaveName));
            }
            set
            {
                SetAttribute<string>(nameof(SlaveName), value);
            }
        }

        /// <summary>
        /// 在该主机上使用的副本数量
        /// </summary>
        public int Count
        {
            get
            {

                return GetAttribute<int>(nameof(Count));
            }
            set
            {
                SetAttribute<int>(nameof(Count), value);
            }
        }

        /// <summary>
        /// 附加信息
        /// </summary>
        public string ExtensionInfo
        {
            get
            {

                return GetAttribute<string>(nameof(ExtensionInfo));
            }
            set
            {
                SetAttribute<string>(nameof(ExtensionInfo), value);
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

        public async Task Add(CancellationToken cancellationToken = default)
        {
            await _imp.Add(this, cancellationToken);
        }

        public async Task Add()
        {
            await _imp.Add(this);
        }

        public async Task Update(CancellationToken cancellationToken = default)
        {
            await _imp.Update(this, cancellationToken);
        }

        public async Task Update()
        {
            await _imp.Update(this);
        }

        public async Task Delete(CancellationToken cancellationToken = default)
        {
            await _imp.Delete(this, cancellationToken);
        }

        public async Task Delete()
        {
            await _imp.Delete(this);
        }
    }

    [Injection(InterfaceType = typeof(ITestCaseSlaveHostIMP), Scope = InjectionScope.Transient)]
    public class TestCaseSlaveHostIMP : ITestCaseSlaveHostIMP
    {
        private ITestCaseSlaveHostStore _testCaseSlaveHostStore;

        public TestCaseSlaveHostIMP(ITestCaseSlaveHostStore testCaseSlaveHostStore)
        {
            _testCaseSlaveHostStore = testCaseSlaveHostStore;
        }

        public async Task Add(TestCaseSlaveHost caseSlaveHost, CancellationToken cancellationToken = default)
        {
            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {
                await _testCaseSlaveHostStore.Add(caseSlaveHost, cancellationToken);
                //检查是否有名称重复的

                scope.Complete();
            }
        }

        public async Task Delete(TestCaseSlaveHost caseSlaveHost, CancellationToken cancellationToken = default)
        {
            await _testCaseSlaveHostStore.Delete(caseSlaveHost.ID, cancellationToken);
        }

        public async Task Update(TestCaseSlaveHost caseSlaveHost, CancellationToken cancellationToken = default)
        {
            TestCaseSlaveHost? testCaseSlaveHost = await _testCaseSlaveHostStore.QueryByID(caseSlaveHost.ID, cancellationToken);
            if (testCaseSlaveHost != null)
            {
                await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
                {
                    await _testCaseSlaveHostStore.Update(caseSlaveHost, cancellationToken);
                    //检查是否有名称重复的

                    scope.Complete();
                }
            }
        }
    }

    public interface ITestCaseSlaveHostIMP
    {
        Task Add(TestCaseSlaveHost tCaseSlaveHost, CancellationToken cancellationToken = default);
        Task Delete(TestCaseSlaveHost tCaseSlaveHost, CancellationToken cancellationToken = default);
        Task Update(TestCaseSlaveHost tCaseSlaveHost, CancellationToken cancellationToken = default);
    }
}
