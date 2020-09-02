using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary;

namespace FW.TestPlatform.Main.DTOModel
{
    /// <summary>
    /// 测试数据源视图数据
    /// </summary>
    [DataContract]
    public class TestDataSourceViewData:ModelBase
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



        /// <summary>
        /// 修改时间
        /// </summary>
        [DataMember]
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
        /// <summary>
        /// 树结点Id
        /// </summary>
        [DataMember]
        public Guid? TreeID
        {
            get
            {

                return GetAttribute<Guid?>(nameof(TreeID));
            }
            set
            {
                SetAttribute<Guid?>(nameof(TreeID), value);
            }
        }
    }
}
