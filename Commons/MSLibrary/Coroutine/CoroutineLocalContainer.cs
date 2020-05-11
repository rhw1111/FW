using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Coroutine
{
    /// <summary>
    /// 协程本地数据容器
    /// 所有关于协程本地变量的操作，都是用该静态类
    /// </summary>
    public static class CoroutineLocalContainer
    {
        public static ICoroutineLocal _coroutineLocal;

        /// <summary>
        /// 当前使用的协程本地数据接口
        /// </summary>
        public static ICoroutineLocal CoroutineLocal
        {
            set
            {
                _coroutineLocal = value;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            _coroutineLocal.Init();
        }
        /// <summary>
        /// 生成指定名称协程本地数据
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, object> Generate(string name)
        {
            return _coroutineLocal.Generate(name);
        }
        /// <summary>
        /// 获取指定协程名称的本地数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Dictionary<string, object> Get(string name)
        {
            return _coroutineLocal.Get(name);
        }
        /// <summary>
        /// 获取当前运行协程的本地数据
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, object> GetCurrent()
        {
            return _coroutineLocal.GetCurrent();
        }
        /// <summary>
        /// 移除指定名称的本地数据
        /// </summary>
        /// <param name="name"></param>
        public static void Remove(string name)
        {
            _coroutineLocal.Remove(name);
        }
        /// <summary>
        /// 设置当前运行的协程名称
        /// </summary>
        /// <param name="name"></param>
        public static void SetCurrentCoroutineName(string name)
        {
            _coroutineLocal.SetCurrentCoroutineName(name);
        }
        /// <summary>
        /// 获取当前运行的协程名称
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentCoroutineName()
        {
            return _coroutineLocal.GetCurrentCoroutineName();
        }
    }
}
