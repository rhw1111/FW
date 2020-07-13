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
    /// 主机新建数据模型
    /// </summary>
    [DataContract]
    public class TestHostUpdateModel: TestHostAddModel
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
    }
}
