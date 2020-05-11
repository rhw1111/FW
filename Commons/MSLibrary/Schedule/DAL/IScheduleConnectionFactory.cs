using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Schedule.DAL
{
    /// <summary>
    /// 有关调度的数据连接字符串工厂
    /// </summary>
    public interface IScheduleConnectionFactory
    {
        /// <summary>
        /// 创建有关调度的读写连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateAllForSchedule();
        /// <summary>
        /// 创建有关调度的只读连接字符串
        /// </summary>
        /// <returns></returns>
        string CreateReadForSchedule();

    }
}
