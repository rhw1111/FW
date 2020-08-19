using MSLibrary;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FW.TestPlatform.Main.DTOModel
{
    /// <summary>
    /// 监控数据汇总表
    /// </summary>
    [DataContract]
    public class TestCaseHistoryUpdateData : ModelBase
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
        /// 格式
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
    }
}
