using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DAL;

namespace MSLibrary.Transaction.DAL
{
    public interface ITransactionConnectionFactory
    {
        /// <summary>
        /// 创建分布式操作记录默认读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForDTOperationRecord();

        /// <summary>
        /// 根据服务器信息创建分布式操作记录读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForDTOperationRecord(DBConnectionNames dBConnectionNames);


        /// <summary>
        /// 创建分布式操作数据读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForDTOperationData();


        string CreateAllForDTOperationData(DBConnectionNames dBConnectionNames);
    }
}
