using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Xrm.DAL
{
    public interface IXrmConnectionFactory
    {
        /// <summary>
        /// 创建有关Xrm的读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForXrm();
        /// <summary>
        /// 创建有关Xrm的只读连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateReadForXrm();
    }
}
