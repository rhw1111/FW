using MSLibrary;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FW.TestPlatform.Main.DTOModel
{
    /// <summary>
    /// 测试案例历史详情
    /// </summary>
    [DataContract]
    public class TestCaseHistoryDetailViewData : TestCaseHistoryListViewData
    {
        /// <summary>
        /// 测试案例历史ID
        /// </summary>
        [DataMember]
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
    }
}
