using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary
{
    /// <summary>
    /// 单例抽象工厂
    /// 所有单例工厂都继承该类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingletonFactory<T> : IFactory<T>
        where T:class
    {
        private T _obj = default(T);
        private object _lockObj = new object();



        public T Create()
        {
            if (_obj==default(T))
            {
                lock(_lockObj)
                {
                    if (_obj==default(T))
                    {
                        _obj = RealCreate();
                    }
                }
            }
            return _obj;
        }

        protected abstract T RealCreate();
    }
}
