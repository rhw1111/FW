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
    public class FormulasViewData:ModelBase
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
        /// 名称
        /// </summary>
        [DataMember]
        public string NameDesc
        {
            get
            {

                return GetAttribute<string>(nameof(NameDesc));
            }
            set
            {
                SetAttribute<string>(nameof(NameDesc), value);
            }
        }
    }
}
