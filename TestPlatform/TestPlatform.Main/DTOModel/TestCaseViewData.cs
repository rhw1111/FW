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
    public class TestCaseViewData:ModelBase
    {


        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
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
        public Guid MasterHostID
        {
            get
            {
                return GetAttribute<Guid>(nameof(MasterHostID));
            }
            set
            {
                SetAttribute<Guid>(nameof(MasterHostID),value);
            }
        }

        public TestHost MasterHost
        {
            get
            {
                return GetAttribute<TestHost>(nameof(MasterHost));
            }
            set
            {
                SetAttribute<TestHost>(nameof(MasterHost),value);
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
        /// 创建时间
        /// </summary>
        [DataMember]
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
        [DataMember]
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
