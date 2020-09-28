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
    public class ExecuteCopyModel: ModelBase
    {
        /// <summary>
        /// 实体ID
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
        /// 父节点Guid
        /// </summary>
        [DataMember]
        public Guid? ParentID
        {
            get
            {

                return GetAttribute<Guid?>(nameof(ParentID));
            }
            set
            {
                SetAttribute<Guid?>(nameof(ParentID), value);
            }
        }

        /// <summary>
        /// 实体类型
        /// </summary>
        [DataMember]
        public string Type
        {
            get
            {
                return GetAttribute<string>(nameof(Type));
            }
            set
            {
                SetAttribute<string>(nameof(Type), value);
            }
        }
    }
}
