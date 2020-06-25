using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary;
using FW.TestPlatform.Main.Entities;

namespace FW.TestPlatform.Main.DTOModel
{
    /// <summary>
    /// 测试数据源视图数据
    /// </summary>
    [DataContract]
    public class TestCaseSlaveHostViewData: ModelBase
    {
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
    }
}
