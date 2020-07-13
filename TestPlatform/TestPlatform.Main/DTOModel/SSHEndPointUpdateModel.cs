using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MSLibrary;

namespace FW.TestPlatform.Main.DTOModel
{
    /// <summary>
    /// SSH终结点更新模型
    /// </summary>
    [DataContract]
    public class SSHEndPointUpdateModel : SSHEndPointAddModel
    {
        /// <summary>
        /// 名称
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
    }
}
