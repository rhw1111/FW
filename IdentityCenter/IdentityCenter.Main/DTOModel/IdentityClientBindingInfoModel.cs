using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary;

namespace IdentityCenter.Main.DTOModel
{
    [DataContract]
    public class IdentityClientBindingInfoModel:ModelBase
    {
        /// <summary>
        /// 绑定名称
        /// </summary>
        [DataMember]
        public string BindingName
        {
            get
            {

                return GetAttribute<string>(nameof(BindingName));
            }
            set
            {
                SetAttribute<string>(nameof(BindingName), value);
            }
        }
        /// <summary>
        /// 绑定类型
        /// </summary>
        [DataMember]
        public string BindingType
        {
            get
            {

                return GetAttribute<string>(nameof(BindingType));
            }
            set
            {
                SetAttribute<string>(nameof(BindingType), value);
            }
        }
    }
}
