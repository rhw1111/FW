using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary;

namespace FW.TestPlatform.Main.DTOModel
{
    /// <summary>
    /// 测试数据源新建模型
    /// </summary>
    [DataContract]
    public class TestCaseRunModel: ModelBase
    {
        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public Guid CaseId
        {
            get
            {
                return GetAttribute<Guid>(nameof(CaseId));
            }
            set
            {
                SetAttribute<Guid>(nameof(CaseId), value);
            }
        }

        /// <summary>
        /// 测试引擎类型
        /// </summary>
        [DataMember]
        public bool IsStop
        {
            get
            {
                return GetAttribute<bool>(nameof(IsStop));
            }
            set
            {
                SetAttribute<bool>(nameof(IsStop), value);
            }
        }
    }
}
