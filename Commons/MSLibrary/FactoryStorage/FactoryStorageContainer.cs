using System;
using System.Collections.Concurrent;
using System.Text;

namespace MSLibrary.FactoryStorage
{
    /// <summary>
    /// 保存工厂键值对的存储
    /// </summary>
    public static class FactoryStorageContainer
    {
        private static ConcurrentDictionary<string, object> _factoryList = new ConcurrentDictionary<string, object>();

        private static IFactoryStorageInit _factoryStorageInit;

        public static IFactoryStorageInit FactoryStorageInit
        {
            set
            {
                _factoryStorageInit = value;
            }
        }

        /// <summary>
        /// 增加一个指定名称的工厂实例
        /// </summary>
        /// <typeparam name="T">工厂要创建的接口</typeparam>
        /// <param name="name">名称</param>
        /// <param name="factory">工厂实例</param>
        public static void Add<T>(string name, IFactory<T> factory)
        {
            var strKey = GenerateKey<T>(name);
            _factoryList[strKey] = factory;
        }

        /// <summary>
        /// 获取指定名称的工厂实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IFactory<T> Get<T>(string name)
        {
            var strKey = GenerateKey<T>(name);
            if (_factoryList.TryGetValue(strKey, out object value))
            {
                if (value is IFactory<T>)
                {
                    return (IFactory<T>)value;
                }
                else
                {
                    throw new Exception($"{name} factory in FactoryStorage is {value.GetType().FullName}, but request is {typeof(T).FullName}");
                }
            }
            else
            {
                throw new Exception($"not found {name} factory in FactoryStorage");
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init(params string[] assemblyNames)
        {
            _factoryStorageInit.Execute(assemblyNames);
        }

        private static string GenerateKey<T>(string name)
        {
            return $"{typeof(T).FullName}-{name}";
        }



    }
}
