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
    public class TreeEntityViewModel : ModelBase
    {
        /// <summary>
        /// 父节点ID
        /// </summary>
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
        /// 值
        /// </summary>
        [DataMember]
        public string Value
        {
            get
            {

                return GetAttribute<string>(nameof(Value));
            }
            set
            {
                SetAttribute<string>(nameof(Value), value);
            }
        }

        /// <summary>
        /// 测试配置
        /// </summary>
        [DataMember]
        public int Type
        {
            get
            {
                return GetAttribute<int>(nameof(Type));
            }
            set
            {
                SetAttribute<int>(nameof(Type), value);
            }
        }
        /// <summary>
        /// 父节点ID
        /// </summary>
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
        /// 创建时间
        /// </summary>
        [DataMember]
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
