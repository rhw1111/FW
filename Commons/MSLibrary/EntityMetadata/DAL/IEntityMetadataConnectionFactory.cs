using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.EntityMetadata.DAL
{
    public interface IEntityMetadataConnectionFactory
    {
        /// <summary>
        /// 创建实体元数据读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForEntityMetadata();
        /// <summary>
        /// 创建实体元数据只读连接字符串 
        /// </summary>
        /// <returns></returns>
        string CreateReadForEntityMetadata();
    }
}
