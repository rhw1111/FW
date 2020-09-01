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
    public class TreeEntityAddModel : ModelBase
    {
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
        public string? Value
        {
            get
            {

                return GetAttribute<string?>(nameof(Value));
            }
            set
            {
                SetAttribute<string?>(nameof(Value), value);
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
        public Guid? FolderID
        {
            get
            {
                return GetAttribute<Guid?>(nameof(FolderID));
            }
            set
            {
                SetAttribute<Guid?>(nameof(FolderID), value);
            }
        }
    }
}
