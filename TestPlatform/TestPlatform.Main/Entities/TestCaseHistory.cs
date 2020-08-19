using MSLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Entities
{
    public class TestCaseHistory : EntityBase<ITestCaseHistoryIMP>
    {
        private static IFactory<ITestCaseHistoryIMP>? _testCaseHistoryIMPFactory;

        public static IFactory<ITestCaseHistoryIMP> TestCaseHistoryIMPFactory
        {
            set
            {
                _testCaseHistoryIMPFactory = value;
            }
        }

        public override IFactory<ITestCaseHistoryIMP>? GetIMPFactory()
        {
            return _testCaseHistoryIMPFactory;
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
        /// 所属Case的ID
        /// </summary>
        public Guid CaseID
        {
            get
            {

                return GetAttribute<Guid>(nameof(CaseID));
            }
            set
            {
                SetAttribute<Guid>(nameof(CaseID), value);
            }
        }

        /// <summary>
        /// 所属Case
        /// </summary>
        public TestCase Case
        {
            get
            {

                return GetAttribute<TestCase>(nameof(Case));
            }
            set
            {
                SetAttribute<TestCase>(nameof(Case), value);
            }
        }


        /// <summary>
        /// 报告总结
        /// </summary>
        public string Summary
        {
            get
            {
                return GetAttribute<string>(nameof(Summary));
            }
            set
            {
                SetAttribute<string>(nameof(Summary), value);
            }
        }

        /// <summary>
        /// 网关数据格式
        /// </summary>
        public string NetGatewayDataFormat
        {
            get
            {
                return GetAttribute<string>(nameof(NetGatewayDataFormat));
            }
            set
            {
                SetAttribute<string>(nameof(NetGatewayDataFormat), value);
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

    public interface ITestCaseHistoryIMP
    {
        Task Add(TestCaseHistory history, CancellationToken cancellationToken = default);
        Task Delete(TestCaseHistory history, CancellationToken cancellationToken = default);
    }
}
