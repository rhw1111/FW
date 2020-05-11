using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.MessageQueue.Extensions
{
    /// <summary>
    /// 消息操作扩展点接口
    /// </summary>
    public interface ISMessageOperationExtension
    {
        /// <summary>
        /// 消息创建后调用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task OnSMessageAddExecuted(ISMessageOperationContext context);
        /// <summary>
        /// 消息转移到死队列后调用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task OnSMessageAddToDeadExecuted(ISMessageOperationContext context);
    }



    /// <summary>
    /// 消息操作上下文接口
    /// </summary>
    public interface ISMessageOperationContext
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

        Dictionary<string, object> Parameters
        {
            get;
        }
    }

    /// <summary>
    /// 消息操作上下文实现
    /// </summary>
    public class SMessageOperationContext : ISMessageOperationContext
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        public Guid MessageId
        {
            get;
            set;
        }
        /// <summary>
        /// 消息类型
        /// </summary>
        public string MessageType
        {
            get;
            set;
        }
        /// <summary>
        /// 消息关键字
        /// </summary>
        public string MessageKey
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string MessageData
        {
            get;
            set;
        }

        /// <summary>
        /// 额外参数
        /// </summary>
        public Dictionary<string, object> Parameters
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 消息操作上下文工厂接口
    /// </summary>
    public interface ISMessageOperationContextFactory
    {
        ISMessageOperationContext Create(SMessage message, Dictionary<string, object> parameters);
    }


    [Injection(InterfaceType = typeof(ISMessageOperationContextFactory), Scope = InjectionScope.Singleton)]
    public class SMessageOperationContextFactory : ISMessageOperationContextFactory
    {
        public ISMessageOperationContext Create(SMessage message, Dictionary<string, object> parameters)
        {
            SMessageOperationContext context = new SMessageOperationContext()
            {
                MessageId = message.ID,
                MessageType = message.Type,
                MessageKey = message.Key,
                MessageData = message.Data,
                Parameters = parameters
            };

            return context;
        }
    }

}
