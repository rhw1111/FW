using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Transaction
{
    /// <summary>
    /// 用于标识在范围内的所有数据库操作使用ALL连接
    /// </summary>
    public class DBAllScope : IDisposable
    {
        private object _preValue = null;
        public DBAllScope()
        {
            var dict = ContextContainer.GetValue<ConcurrentDictionary<string, object>>(ContextTypes.Dictionary.ToString());

            if (dict != null)
            {
                if (dict.TryGetValue("dball", out object obj))
                {
                    _preValue = obj;
                }
                dict["dball"] = "1";
            }
        }
        public void Dispose()
        {
            var dict = ContextContainer.GetValue<ConcurrentDictionary<string, object>>(ContextTypes.Dictionary.ToString());

            if (dict != null)
            {
                if (_preValue != null)
                {
                    dict["dball"] = _preValue;
                }
                else
                {
                    dict.TryRemove("dball",out object v);
                }
            }
        }

        public static bool IsAll()
        {
            bool result = false;
            var dict = ContextContainer.GetValue<ConcurrentDictionary<string, object>>(ContextTypes.Dictionary.ToString());

            if (dict != null)
            {
                    if (dict.TryGetValue("dball", out object obj))
                    {
                        result = true;
                    }            
            }
            return result;
        }
    }
}
