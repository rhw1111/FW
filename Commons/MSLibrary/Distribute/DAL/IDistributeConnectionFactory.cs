using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Distribute.DAL
{
    public interface IDistributeConnectionFactory
    {
        /// <summary>
        /// 创建有关分布式协调的读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForDistributeCoordinator();
        /// <summary>
        /// 创建有关分布式协调的只读连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateReadForDistributeCoordinator();
    }
}
