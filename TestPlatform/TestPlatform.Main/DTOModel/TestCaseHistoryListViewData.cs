using MSLibrary;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FW.TestPlatform.Main.DTOModel
{
    /// <summary>
    /// 测试案例列表Model
    /// </summary>
    [DataContract]
    public class TestCaseHistoryListViewData : ModelBase
    {
        /// <summary>
        /// 测试案例历史ID
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
        /// 测试案例ID
        /// </summary>
        [DataMember]
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
        /// 实时监控地址
        /// </summary>
        [DataMember]
        public string MonitorUrl
        {
            get
            {
                return GetAttribute<string>(nameof(MonitorUrl));
            }
            set
            {
                SetAttribute<string>(nameof(MonitorUrl), value);
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
    }
}
