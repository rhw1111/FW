using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MSLibrary
{
    /// <summary>
    /// 当前用户的语言编号上下文
    /// </summary>
    public class ContextCurrentUserLcid : IContext<int>
    {
        private static AsyncLocal<int> _asyncLocal = new AsyncLocal<int>();
        private static ThreadLocal<int> _threadLocal = new ThreadLocal<int>();

        public int Value
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
