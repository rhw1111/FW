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
    }
}
