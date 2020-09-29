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
    public class TestCaseListViewData:ModelBase
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
        /// 测试用例状态
        /// </summary>
        [DataMember]
        public string Status
        {
            get
            {

                return GetAttribute<string>(nameof(Status));
            }
            set
            {
                SetAttribute<string>(nameof(Status), value);
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
        /// 树节点ID
        /// </summary>
        [DataMember]
        public Guid? TreeID
        {
            get
            {

                return GetAttribute<Guid?>(nameof(TreeID));
            }
            set
            {
                SetAttribute<Guid?>(nameof(TreeID), value);
            }
        }
    }
}
