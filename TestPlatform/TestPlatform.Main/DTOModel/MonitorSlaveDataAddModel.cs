using MSLibrary;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FW.TestPlatform.Main.DTOModel
{
    /// <summary>
    /// InfluxDB监控数据主表
    /// </summary>
    [DataContract]
    public class MonitorSlaveDataAddModel : ModelBase
    {
        /// <summary>
        /// 测试案例ID
        /// </summary>
        [DataMember]
        public string CaseID
        {
            get
            {

                return GetAttribute<string>(nameof(CaseID));
            }
            set
            {
                SetAttribute<string>(nameof(CaseID), value);
            }
        }

        /// <summary>
        /// 测试案例ID
        /// </summary>
        [DataMember]
        public string SlaveID
        {
            get
            {

                return GetAttribute<string>(nameof(SlaveID));
            }
            set
            {
                SetAttribute<string>(nameof(SlaveID), value);
            }
        }

        /// <summary>
        /// 当前QPS
        /// </summary>
        [DataMember]
        public string QPS
        {
            get
            {

                return GetAttribute<string>(nameof(QPS));
            }
            set
            {
                SetAttribute<string>(nameof(QPS), value);
            }
        }

        /// <summary>
        /// 操作时间 年月日时分秒
        /// </summary>
        [DataMember]
        public string Time
        {
            get
            {

                return GetAttribute<string>(nameof(Time));
            }
            set
            {
                SetAttribute<string>(nameof(Time), value);
            }
        }
    }
}
