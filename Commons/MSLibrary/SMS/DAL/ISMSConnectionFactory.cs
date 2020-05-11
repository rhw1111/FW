using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.SMS.DAL
{
    public interface ISMSConnectionFactory
    {
        /// <summary>
        /// 创建针对短信管理的读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateSMSManagementAllForSMS();
        /// <summary>
        /// 创建针对短信管理的只读连接字符串 
        /// </summary>
        /// <returns></returns>
        string CreateSMSManagementReadForSMS();

        /// <summary>
        /// 创建针对短信记录的读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateSMSRecordAllForSMS();
        /// <summary>
        /// 创建针对短信记录的只读连接字符串 
        /// </summary>
        /// <returns></returns>
        string CreateSMSRecordReadForSMS();
    }
}
