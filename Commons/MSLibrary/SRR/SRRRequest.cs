using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.SRR
{
    /// <summary>
    /// 消息请求
    /// </summary>
    public class SRRRequest
    {
        private object _lockObj = new object();
        /// <summary>
        /// 类型
        /// </summary>
        public virtual string Type { get;}
        /// <summary>
        /// 请求参数键值对
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

        protected T GetParameter<T>(string parameterName)
        {
            object value;
            if (Parameters.TryGetValue(parameterName, out value))
            {
                return (T)value;
            }
            else
            {
                return default(T);
            }
        }

        protected void SetParameter<T>(string parameterName, T value)
        {
            if (Parameters == null)
            {
                lock (_lockObj)
                {
                    if (Parameters == null)
                    {
                        Parameters = new Dictionary<string, object>();
                    }
                }

            }
            Parameters[parameterName] = value;
        }


    }
}
