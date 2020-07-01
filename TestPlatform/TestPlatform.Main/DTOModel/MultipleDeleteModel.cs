using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary;

namespace FW.TestPlatform.Main.DTOModel
{
    /// <summary>
    /// 批量删除模型
    /// </summary>
    [DataContract]
    public class MultipleDeleteModel: ModelBase
    {
        /// <summary>
        /// 测试用例ID
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
        /// 批量删除ID
        /// </summary>
        [DataMember]
        public List<Guid> IDS
        {
            get
            {

                return GetAttribute<List<Guid>>(nameof(IDS));
            }
            set
            {
                SetAttribute<List<Guid>>(nameof(IDS), value);
            }
        }
    }
}
