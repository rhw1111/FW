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
    public class TestCaseAddModel: ModelBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        /// 状态
        /// </summary>
        [DataMember]
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
        [DataMember]
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
        /// 父节点ID
        /// </summary>
        public Guid? ParentID
        {
            get
            {
                return GetAttribute<Guid?>(nameof(ParentID));
            }
            set
            {
                SetAttribute<Guid?>(nameof(ParentID), value);
            }
        }
    }
}
