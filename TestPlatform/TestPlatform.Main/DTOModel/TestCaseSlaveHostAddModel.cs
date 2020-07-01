using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary;

namespace FW.TestPlatform.Main.DTOModel
{
    /// <summary>
    /// 测试数据源新建模型
    /// </summary>
    [DataContract]
    public class TestCaseSlaveHostAddModel : ModelBase
    {
        /// <summary>
        /// 所属主机ID
        /// </summary>
        [DataMember]
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
        /// 所属测试用例ID
        /// </summary>
        [DataMember]
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
        /// 测试机名称
        /// 通过该名称与副本Index，来区分每个Slave
        /// </summary>
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
    }
}
