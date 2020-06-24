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
    public class MonitorMasterDataAddModel : ModelBase
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
        /// 当前连接数
        /// </summary>
        [DataMember]
        public string ConnectCount
        {
            get
            {

                return GetAttribute<string>(nameof(ConnectCount));
            }
            set
            {
                SetAttribute<string>(nameof(ConnectCount), value);
            }
        }

        /// <summary>
        /// 当前连接失败数
        /// </summary>
        [DataMember]
        public string ConnectFailCount
        {
            get
            {

                return GetAttribute<string>(nameof(ConnectFailCount));
            }
            set
            {
                SetAttribute<string>(nameof(ConnectFailCount), value);
            }
        }

        /// <summary>
        /// 当前请求总数
        /// </summary>
        [DataMember]
        public string ReqCount
        {
            get
            {

                return GetAttribute<string>(nameof(ReqCount));
            }
            set
            {
                SetAttribute<string>(nameof(ReqCount), value);
            }
        }

        /// <summary>
        /// 当前请求失败数
        /// </summary>
        [DataMember]
        public string ReqFailCount
        {
            get
            {

                return GetAttribute<string>(nameof(ReqFailCount));
            }
            set
            {
                SetAttribute<string>(nameof(ReqFailCount), value);
            }
        }

        /// <summary>
        /// 最大响应时间
        /// </summary>
        [DataMember]
        public string MaxDuration
        {
            get
            {

                return GetAttribute<string>(nameof(MaxDuration));
            }
            set
            {
                SetAttribute<string>(nameof(MaxDuration), value);
            }
        }

        /// <summary>
        /// 最小响应时间
        /// </summary>
        [DataMember]
        public string MinDurartion
        {
            get
            {

                return GetAttribute<string>(nameof(MinDurartion));
            }
            set
            {
                SetAttribute<string>(nameof(MinDurartion), value);
            }
        }

        /// <summary>
        /// 平均响应时间
        /// </summary>
        [DataMember]
        public string AvgDuration
        {
            get
            {

                return GetAttribute<string>(nameof(AvgDuration));
            }
            set
            {
                SetAttribute<string>(nameof(AvgDuration), value);
            }
        }
    }
}
