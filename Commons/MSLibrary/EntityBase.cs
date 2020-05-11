using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;


namespace MSLibrary
{
    /// <summary>
    /// 领域实体基类
    /// </summary>
    public abstract class EntityBase<T> : ModelBase
    {
        protected T _imp;
        /// <summary>
        /// 获取具体实现工厂
        /// </summary>
        /// <returns></returns>
        public abstract IFactory<T> GetIMPFactory();


        private static IFactory<IIMPGenerateService> _impGenerateServiceFactory;

        public static IFactory<IIMPGenerateService> IMPGenerateServiceFactory
        {
            set
            {
                _impGenerateServiceFactory = value;
            }
        }

        public EntityBase()
        {
            var impFactory=GetIMPFactory();
            if (impFactory==null)
            {
                if (_impGenerateServiceFactory != null)
                {
                    _imp=_impGenerateServiceFactory.Create().Generate<T>();
                }
                else
                {
                    var di = ContextContainer.GetValue<IDIContainer>("DI");
                    if (di == null)
                    {
                        _imp = DIContainerContainer.Get<T>();
                    }
                    else
                    {
                        _imp = di.Get<T>();
                    }
                }
            }
            else
            {
                _imp = impFactory.Create();
            }

        }

        public Dictionary<string, object> Extensions = new Dictionary<string, object>();

        public V GetExtension<V>(string name)
        {
            if (Extensions.TryGetValue(name, out object value))
            {
                return (V)value;
            }
            else
            {
                return default(V);
            }
        }

        public void SetExtension<V>(string name, V value)
        {
            if (Extensions == null)
            {
                lock (this)
                {
                    if (Extensions == null)
                    {
                        Extensions = new Dictionary<string, object>();
                    }
                }

            }
            Extensions[name] = value;
        }

        public T GetIMP()
        {
            return _imp;
        }
    }


    /// <summary>
    /// IMP实现对象的生成服务
    /// </summary>
    public interface IIMPGenerateService
    {
        T Generate<T>();
    }
}
