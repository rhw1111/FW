using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary;
using FW.TestPlatform.Main.Entities;

namespace FW.TestPlatform.Main.DTOModel
{
    /// <summary>
    /// 测试数据源视图数据
    /// </summary>
    [DataContract]
    public class TestCaseHostPortCheckModel:ModelBase
    {
        /// <summary>
        /// Id
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
        /// 名称
        /// </summary>
        [DataMember]
        public string Name
        {
            get
            {

                return GetAttribute<string>(nameof(Name));
            }
            set
            {
                SetAttribute<string>(nameof(Name), value);
            }
        }

        /// <summary>
        /// 是否可用
        /// </summary>
        [DataMember]
        public bool IsAvailable
        {
            get
            {

                return GetAttribute<bool>(nameof(IsAvailable));
            }
            set
            {
                SetAttribute<bool>(nameof(IsAvailable), value);
            }
        }

        /// <summary>
        /// 冲突的名称
        /// </summary>
        [DataMember]
        public string ConflictedNames
        {
            get
            {

                return GetAttribute<string>(nameof(ConflictedNames));
            }
            set
            {
                SetAttribute<string>(nameof(ConflictedNames), value);
            }
        }
    }
}
