using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.DAL
{
    /// <summary>
    /// 存储信息
    /// </summary>
    [DataContract]
    public class StoreInfo
    {
        /// <summary>
        /// 数据库连接信息
        /// </summary>
        [DataMember]
        public DBConnectionNames DBConnectionNames { get; set; }
        /// <summary>
        /// 数据库表名称键值对
        /// 键为实体名称
        /// </summary>
        [DataMember]
        public Dictionary<string, string> TableNames { get; set; }
    }

    /// <summary>
    /// 数据库连接名称
    /// </summary>
    [DataContract]
    public class DBConnectionNames
    {
        /// <summary>
        /// 读写库的连接名称
        /// </summary>
        [DataMember]
        public string ReadAndWrite { get; set; }
        /// <summary>
        /// 只读库的连接名称
        /// </summary>
        [DataMember]
        public string Read { get; set; }
    }
}
