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
    public class TestCaseHistorySummyAddModel : ModelBase
    {
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
        /// 当前连接数
        /// </summary>
        [DataMember]
        public int ConnectCount
        {
            get
            {

                return GetAttribute<int>(nameof(ConnectCount));
            }
            set
            {
                SetAttribute<int>(nameof(ConnectCount), value);
            }
        }

        /// <summary>
        /// 当前连接失败数
        /// </summary>
        [DataMember]
        public int ConnectFailCount
        {
            get
            {

                return GetAttribute<int>(nameof(ConnectFailCount));
            }
            set
            {
                SetAttribute<int>(nameof(ConnectFailCount), value);
            }
        }

        /// <summary>
        /// 当前请求总数
        /// </summary>
        [DataMember]
        public int ReqCount
        {
            get
            {

                return GetAttribute<int>(nameof(ReqCount));
            }
            set
            {
                SetAttribute<int>(nameof(ReqCount), value);
            }
        }

        /// <summary>
        /// 当前请求失败数
        /// </summary>
        [DataMember]
        public int ReqFailCount
        {
            get
            {

                return GetAttribute<int>(nameof(ReqFailCount));
            }
            set
            {
                SetAttribute<int>(nameof(ReqFailCount), value);
            }
        }

        /// <summary>
        /// 最大响应时间
        /// </summary>
        [DataMember]
        public float MaxDuration
        {
            get
            {

                return GetAttribute<float>(nameof(MaxDuration));
            }
            set
            {
                SetAttribute<float>(nameof(MaxDuration), value);
            }
        }

        /// <summary>
        /// 最小响应时间
        /// </summary>
        [DataMember]
        public float MinDurartion
        {
            get
            {

                return GetAttribute<float>(nameof(MinDurartion));
            }
            set
            {
                SetAttribute<float>(nameof(MinDurartion), value);
            }
        }

        /// <summary>
        /// 平均响应时间
        /// </summary>
        [DataMember]
        public float AvgDuration
        {
            get
            {

                return GetAttribute<float>(nameof(AvgDuration));
            }
            set
            {
                SetAttribute<float>(nameof(AvgDuration), value);
            }
        }

        /// <summary>
        /// 最大每秒请求数
        /// </summary>
        [DataMember]
        public float MaxQPS
        {
            get
            {

                return GetAttribute<float>(nameof(MaxQPS));
            }
            set
            {
                SetAttribute<float>(nameof(MaxQPS), value);
            }
        }

        /// <summary>
        /// 最小每秒请求数
        /// </summary>
        [DataMember]
        public float MinQPS
        {
            get
            {

                return GetAttribute<float>(nameof(MinQPS));
            }
            set
            {
                SetAttribute<float>(nameof(MinQPS), value);
            }
        }

        /// <summary>
        /// 平均每秒请求数
        /// </summary>
        [DataMember]
        public float AvgQPS
        {
            get
            {

                return GetAttribute<float>(nameof(AvgQPS));
            }
            set
            {
                SetAttribute<float>(nameof(AvgQPS), value);
            }
        }
    }
}
