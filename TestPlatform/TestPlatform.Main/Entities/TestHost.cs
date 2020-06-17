using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.CommandLine.SSH;

namespace FW.TestPlatform.Main.Entities
{
    /// <summary>
    /// 测试主机
    /// </summary>
    public class TestHost : EntityBase<ITestHostIMP>
    {
        public override IFactory<ITestHostIMP> GetIMPFactory()
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
    }

    public interface ITestHostIMP
    {
        Task Add(TestHost host, CancellationToken cancellationToken = default);
        Task Update(TestHost host, CancellationToken cancellationToken = default);
        Task Delete(TestHost host, CancellationToken cancellationToken = default);
    }
}
