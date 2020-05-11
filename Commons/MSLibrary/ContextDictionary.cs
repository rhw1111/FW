using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MSLibrary
{
    /// <summary>
    /// 字典上下文
    /// </summary>
    public class ContextDictionary : IContext<ConcurrentDictionary<string, object>>
    {
        private static AsyncLocal<ConcurrentDictionary<string, object>> _asyncLocal = new AsyncLocal<ConcurrentDictionary<string, object>>();
        private static ThreadLocal<ConcurrentDictionary<string, object>> _threadLocal = new ThreadLocal<ConcurrentDictionary<string, object>>();

        public ConcurrentDictionary<string, object> Value
        {
            get
            {
                return _asyncLocal.Value;
            }

            set
            {
                _asyncLocal.Value = value;
                _threadLocal.Value = value;
            }
        }

        public bool IsAuto()
        {
            return !_threadLocal.IsValueCreated;
        }
    }
}
