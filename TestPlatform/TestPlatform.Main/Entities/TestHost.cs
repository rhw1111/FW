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
using Microsoft.Extensions.Azure;
using MSLibrary.CommandLine.SSH.DAL;
using MSLibrary.LanguageTranslate;

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

        public async Task<List<TestHost>> GetHosts(CancellationToken cancellationToken = default)
        {
            return await _imp.GetHosts(cancellationToken);
        }

        public async Task Add()
        {
            await _imp.Add(this);
        }

        public async Task Update()
        {
            await _imp.Update(this);
        }

        public async Task Delete()
        {
            await _imp.Delete(this);
        }

        public async Task Add(CancellationToken cancellationToken = default)
        {
            await _imp.Add(this, cancellationToken);
        }

        public async Task Update(CancellationToken cancellationToken = default)
        {
            await _imp.Update(this, cancellationToken);
        }

        public async Task Delete(CancellationToken cancellationToken = default)
        {
            await _imp.Delete(this, cancellationToken);
        }
    }

    [Injection(InterfaceType = typeof(ITestHostIMP),Scope = InjectionScope.Transient)]
    public class TestHostIMP : ITestHostIMP
    {
        private ITestHostStore _testHostStore;
        private ISSHEndpointStore _sSHEndpointStore;
        public TestHostIMP(ITestHostStore testHostStore, ISSHEndpointStore sSHEndpointStore)
        {
            _testHostStore = testHostStore;
            _sSHEndpointStore = sSHEndpointStore;
        }
        public async Task Add(TestHost host, CancellationToken cancellationToken = default)
        {
            var sshEndpoint = await _sSHEndpointStore.QueryByID(host.SSHEndpointID, cancellationToken);
            if (sshEndpoint == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundSSHEndPointByID,
                    DefaultFormatting = "找不到Id为{0}的SSH终结点",
                    ReplaceParameters = new List<object>() { host.SSHEndpointID.ToString() }
                };
                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundSSHEndPointByID, fragment, 1, 0);
            }
            await _testHostStore.Add(host, cancellationToken);
        }

        public async Task Delete(TestHost host, CancellationToken cancellationToken = default)
        {
            await _testHostStore.Delete(host.ID, cancellationToken);
        }

        public async Task Update(TestHost host, CancellationToken cancellationToken = default)
        {
            var sshEndpoint = await _sSHEndpointStore.QueryByID(host.SSHEndpointID, cancellationToken);
            if (sshEndpoint == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundSSHEndPointByID,
                    DefaultFormatting = "找不到Id为{0}的SSH终结点",
                    ReplaceParameters = new List<object>() { host.SSHEndpointID.ToString() }
                };
                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundSSHEndPointByID, fragment, 1, 0);
            }
            await _testHostStore.Update(host, cancellationToken);
        }
        public async Task<List<TestHost>> GetHosts(CancellationToken cancellationToken = default)
        {
            List<TestHost> hostList = new List<TestHost>();
            var result = _testHostStore.GetHosts(cancellationToken);
            await foreach(var item in result)
            {
                hostList.Add(item);
            }
            return hostList;
        }
        public async Task<bool> IsUsedByTestHostsOrSlaves(TestHost host, CancellationToken cancellationToken = default)
        {
            bool result = false;
            result = await _testHostStore.IsUsedByTestCases(host.ID ,cancellationToken);
            if (result)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.TestHostIsUsedByTestCases,
                    DefaultFormatting = "Id为{0}的主机正在被其它的测试用例使用，不能被删除",
                    ReplaceParameters = new List<object>() { host.ID.ToString() }
                };
                throw new UtilityException((int)TestPlatformErrorCodes.TestHostIsUsedByTestCases, fragment, 1, 0);
            }
            result = await _testHostStore.IsUsedBySlaveHosts(host.ID, cancellationToken);
            if (result)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.TestHostIsUsedBySlaves,
                    DefaultFormatting = "Id为{0}的主机正在被其它的从主机使用，不能被删除",
                    ReplaceParameters = new List<object>() { host.ID.ToString() }
                };
                throw new UtilityException((int)TestPlatformErrorCodes.TestHostIsUsedBySlaves, fragment, 1, 0);
            }
            return result;
        }
    }

    public interface ITestHostIMP
    {
        Task Add(TestHost host, CancellationToken cancellationToken = default);
        Task Update(TestHost host, CancellationToken cancellationToken = default);
        Task Delete(TestHost host, CancellationToken cancellationToken = default);
        Task<List<TestHost>> GetHosts(CancellationToken cancellationToken = default);
        Task<bool> IsUsedByTestHostsOrSlaves(TestHost host, CancellationToken cancellationToken = default);
    }
}
