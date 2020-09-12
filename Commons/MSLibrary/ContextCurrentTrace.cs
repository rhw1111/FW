using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary
{
    /// <summary>
    /// 当前请求链路上下文
    /// </summary>
    public class ContextCurrentTrace : IContext<IRequestTraceInofContext>
    {
        private static AsyncLocal<IRequestTraceInofContext> _asyncLocal = new AsyncLocal<IRequestTraceInofContext>();
        private static ThreadLocal<IRequestTraceInofContext> _threadLocal = new ThreadLocal<IRequestTraceInofContext>();

        public IRequestTraceInofContext Value
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


    /// <summary>
    /// 链路追踪信息上下文
    /// </summary>
    public interface IRequestTraceInofContext
    {
        string GetTraceID();
        string GetLinkID();
        string GetChildLinkID();
    }

    public class RequestTraceInofContextDefault : IRequestTraceInofContext
    {
        private string _traceID;
        private string _linkID;
        private int _childNo = 0;

        public RequestTraceInofContextDefault(string traceID, string linkID)
        {
            _traceID = traceID;
            _linkID = linkID;
        }

        public string GetChildLinkID()
        {
            var currentNo = Interlocked.Increment(ref _childNo);
            return $"{_linkID}.{currentNo.ToString()}";
        }

        public string GetLinkID()
        {
            return _linkID;
        }

        public string GetTraceID()
        {
            return _traceID;
        }
    }

}
