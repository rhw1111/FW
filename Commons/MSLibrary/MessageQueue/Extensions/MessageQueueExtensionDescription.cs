using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.MessageQueue.Extensions
{
    /// <summary>
    /// 消息队列总体扩展点介入类
    /// 所有消息队列的扩展点都通过该类的静态属性进行扩展或替换
    /// </summary>
    public static class MessageQueueExtensionDescription
    {
        /// <summary>
        /// 消息执行扩展点接口工厂
        /// </summary>
        public static IFactory<ISMessageExecuteExtension> SMessageExecuteExtensionFactory
        {
            get;set;
        }

        /// <summary>
        /// 消息操作扩展点接口工厂
        /// </summary>
        public static IFactory<ISMessageOperationExtension> SMessageOperationExtensionFactory
        {
            get; set;
        }

        /// <summary>
        /// 消息类型监听者执行后调用
        /// </summary>
        /// <param name="context">执行上下文</param>
        /// <returns></returns>
        public static async Task OnSMessageTypeListenerExecuted(ISMessageTypeListenerPostContext context)
        {
            if (SMessageExecuteExtensionFactory != null)
            {
                await SMessageExecuteExtensionFactory.Create().OnSMessageTypeListenerExecuted(context);
            }
        }

        /// <summary>
        /// 消息创建后调用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task OnSMessageAddExecuted(ISMessageOperationContext context)
        {
            if (SMessageOperationExtensionFactory != null)
            {
                await SMessageOperationExtensionFactory.Create().OnSMessageAddExecuted(context);
            }
        }

        /// <summary>
        /// 消息转移到死队列后调用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task OnSMessageAddToDeadExecuted(ISMessageOperationContext context)
        {
            if (SMessageOperationExtensionFactory != null)
            {
                await SMessageOperationExtensionFactory.Create().OnSMessageAddToDeadExecuted(context);
            }
        }
    }
}
