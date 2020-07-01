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
    public class TestCaseHistoryViewModel : TestCaseHistorySummyAddModel
    {
        /// <summary>
        /// 测试案例ID
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



        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime
        {
            get
            {
                return GetAttribute<DateTime>(nameof(ModifyTime));
            }
            set
            {
                SetAttribute<DateTime>(nameof(ModifyTime), value);
            }
        }
    }
}
