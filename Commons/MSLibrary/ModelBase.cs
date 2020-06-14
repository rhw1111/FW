using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace MSLibrary
{
    /// <summary>
    /// DTO对象基类
    /// </summary>
    [DataContract]
    public class ModelBase
    {
        public Dictionary<string, object> Attributes = new Dictionary<string, object>();
        private object _lockObj = new object();

        protected T GetAttribute<T>(string attributeName)
        {
            object value;
            if (Attributes.TryGetValue(attributeName, out value))
            {
                return (T)value;
            }
            else
            {
                return default(T);
            }
        }

        protected void SetAttribute<T>(string attributeName, T value)
        {
            if (Attributes == null)
            {
                lock (_lockObj)
                {
                    if (Attributes == null)
                    {
                        Attributes = new Dictionary<string, object>();
                    }
                }

            }
            Attributes[attributeName] = value;
        }

        /// <summary>
        /// 反序列化之前初始化
        /// </summary>
        /// <param name="c"></param>
        [OnDeserializing]
        private void DeserializeInit(StreamingContext c)
        {
            Attributes = new Dictionary<string, object>();

            
        }



    }
}
