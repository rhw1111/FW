using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary;
using FW.TestPlatform.Main.Entities;
using MSLibrary.CommandLine.SSH;

namespace FW.TestPlatform.Main.DTOModel
{
    /// <summary>
    /// 测试数据源视图数据
    /// </summary>
    [DataContract]
    public class TestHostViewData:ModelBase
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
        /// SSH终结点名子
        /// </summary>
        public string SSHEndpointName
        {
            get
            {
                return GetAttribute<string>(nameof(SSHEndpointName));
            }
            set
            {
                SetAttribute<string>(nameof(SSHEndpointName), value);
            }
        }
        /// <summary>
        /// 是否可用
        /// </summary>
        [DataMember]
        public bool IsAvailable
        {
            get
            {
                return GetAttribute<bool>(nameof(IsAvailable));
            }
            set
            {
                SetAttribute<bool>(nameof(IsAvailable), value);
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
    }
}
