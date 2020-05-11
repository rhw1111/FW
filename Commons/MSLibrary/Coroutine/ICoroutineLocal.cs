using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MSLibrary.Coroutine
{
    /// <summary>
    /// 协程本地数据
    /// 负责管理协程要用到的本地数据
    /// </summary>
    public interface ICoroutineLocal
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void Init();
        /// <summary>
        /// 生成指定名称协程本地数据
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> Generate(string name);
        /// <summary>
        /// 获取指定协程名称的本地数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Dictionary<string, object> Get(string name);
        /// <summary>
        /// 获取当前运行协程的本地数据
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> GetCurrent();
        /// <summary>
        /// 移除指定名称的本地数据
        /// </summary>
        /// <param name="name"></param>
        void Remove(string name);
        /// <summary>
        /// 设置当前运行的协程名称
        /// </summary>
        /// <param name="name"></param>
        void SetCurrentCoroutineName(string name);
        /// <summary>
        /// 获取当前运行的协程名称
        /// </summary>
        /// <returns></returns>
        string GetCurrentCoroutineName();
    }
}
