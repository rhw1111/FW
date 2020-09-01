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
    public class TestDataSourceAddModel:ModelBase
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
        /// 类型
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

        /// <summary>
        /// 数据
        /// </summary>
        [DataMember]
        public string Data
        {
            get
            {

                return GetAttribute<string>(nameof(Data));
            }
            set
            {
                SetAttribute<string>(nameof(Data), value);
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
