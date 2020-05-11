using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.MessageQueue.Extensions
{
    /// <summary>
    /// 消息执行扩展点接口
    /// </summary>
    public interface ISMessageExecuteExtension
    {
        /// <summary>
        /// 消息类型监听者执行后调用
        /// </summary>
        /// <param name="context">执行上下文</param>
        /// <returns></returns>
        Task OnSMessageTypeListenerExecuted(ISMessageTypeListenerPostContext context);
    }

    /// <summary>
    /// 消息类型监听者提交处理上下文接口
    /// </summary>
    public interface ISMessageTypeListenerPostContext
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        Guid MessageId
        {
            get;
        }
        /// <summary>
        /// 消息类型
        /// </summary>
        string MessageType
        {
            get;
        }
        /// <summary>
        /// 消息Key
        /// </summary>
        string MessageKey
        {
            get;
        }
        /// <summary>
        /// 消息内容
        /// </summary>
        string MessageData
        {
            get;
        }

        /// <summary>
        /// 监听者Id
        /// </summary>
        Guid ListenerId
        {
            get;
        }
        /// <summary>
        /// 监听者名称
        /// </summary>
        string ListenerName
        {
            get;
        }
        /// <summary>
        /// 执行时发生的错误
        /// </summary>
        Exception ExecuteException
        {
            get;
        }

        Dictionary<string,object> Parameters
        {
            get;
        }
    }

    /// <summary>
    /// 消息类型监听者提交处理上下文工厂接口
    /// </summary>
    public interface ISMessageTypeListenerPostContextFactory
    {
        ISMessageTypeListenerPostContext Create(SMessageTypeListener listener,SMessage message,Exception executeException, Dictionary<string, object> parameters);
    }

    public class SMessageTypeListenerPostContext : ISMessageTypeListenerPostContext
    {
        public Guid MessageId
        {
            get;set;
        }

        public string MessageType
        {
            get;set;
        }

        public string MessageKey
        {
            get;set;
        }

        public string MessageData
        {
            get;set;
        }

        public Guid ListenerId
        {
            get;set;
        }

        public string ListenerName
        {
            get;set;
        }

        public Exception ExecuteException
        {
            get;set;
        }

        public Dictionary<string, object> Parameters
        {
            get;set;
        }
    }

    [Injection(InterfaceType = typeof(ISMessageTypeListenerPostContextFactory), Scope = InjectionScope.Singleton)]
    public class SMessageTypeListenerPostContextFactory : ISMessageTypeListenerPostContextFactory
    {
        public ISMessageTypeListenerPostContext Create(SMessageTypeListener listener, SMessage message, Exception executeException, Dictionary<string, object> parameters)
        {
            SMessageTypeListenerPostContext context = new SMessageTypeListenerPostContext()
            {
                MessageId = message.ID,
                MessageKey = message.Key,
                MessageType = message.Type,
                MessageData = message.Data,
                ExecuteException = executeException,
                ListenerId = listener.ID,
                ListenerName = listener.Name
            };

            return context;
        }
    }
}
