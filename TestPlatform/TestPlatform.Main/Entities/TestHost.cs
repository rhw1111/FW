using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.CommandLine.SSH;
using FW.TestPlatform.Main.Entities.DAL;
using MSLibrary.Transaction;

namespace FW.TestPlatform.Main.Entities
{
    /// <summary>
    /// 测试主机
    /// </summary>
    public class TestHost : EntityBase<ITestHostIMP>
    {
        private static IFactory<ITestHostIMP>? _testHostIMPFactory;
        public static IFactory<ITestHostIMP> TestHostIMPFactory
        {
            set
            {
                _testHostIMPFactory = value;
            }
        }
        public override IFactory<ITestHostIMP>? GetIMPFactory()
        {
            return _testHostIMPFactory;
            //throw new NotImplementedException();
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
        /// 地址
        /// </summary>
        public string Address
        {
            get
            {

                return GetAttribute<string>(nameof(Address));
            }
            set
            {
                SetAttribute<string>(nameof(Address), value);
            }
        }

        /// <summary>
        /// SSH终结点ID
        /// </summary>
        public Guid SSHEndpointID
        {
            get
            {

                return GetAttribute<Guid>(nameof(SSHEndpointID));
            }
            set
            {
                SetAttribute<Guid>(nameof(SSHEndpointID), value);
            }
        }

        /// <summary>
        /// SSH终结点
        /// </summary>
        public SSHEndpoint SSHEndpoint
        {
            get
            {

                return GetAttribute<SSHEndpoint>(nameof(SSHEndpoint));
            }
            set
            {
                SetAttribute<SSHEndpoint>(nameof(SSHEndpoint), value);
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

        public async Task<QueryResult<TestHost>> GetHosts(CancellationToken cancellationToken = default)
        {
            return await _imp.GetHosts(cancellationToken);
        }
    }

    [Injection(InterfaceType = typeof(ITestHostIMP),Scope = InjectionScope.Transient)]
    public class TestHostIMP : ITestHostIMP
    {
        private ITestHostStore _testHostStore;
        public TestHostIMP(ITestHostStore testHostStore)
        {
            _testHostStore = testHostStore;
        }
        public async Task Add(TestHost host, CancellationToken cancellationToken = default)
        {
            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {
                await _testHostStore.Add(host, cancellationToken);
                //检查是否有名称重复的
                
                scope.Complete();
            }
        }

        public async Task Delete(TestHost host, CancellationToken cancellationToken = default)
        {
            await _testHostStore.Delete(host.ID, cancellationToken);
        }

        public async Task Update(TestHost host, CancellationToken cancellationToken = default)
        {
            TestHost testHost = await _testHostStore.QueryByID(host.ID, cancellationToken);
            if(testHost != null)
            {
                await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
                {
                    await _testHostStore.Update(host, cancellationToken);
                    //检查是否有名称重复的

                    scope.Complete();
                }
            }
        }
        public async Task<QueryResult<TestHost>> GetHosts(CancellationToken cancellationToken = default)
        {
            return await _testHostStore.GetHosts(cancellationToken);
        }
    }

    public interface ITestHostIMP
    {
        Task Add(TestHost host, CancellationToken cancellationToken = default);
        Task Update(TestHost host, CancellationToken cancellationToken = default);
        Task Delete(TestHost host, CancellationToken cancellationToken = default);
        Task<QueryResult<TestHost>> GetHosts(CancellationToken cancellationToken = default);
    }
}
