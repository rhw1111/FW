using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.CommonQueue.QueueRealExecuteServices
{
    [Injection(InterfaceType = typeof(QueueRealExecuteServiceForAzureServiceBusFactory), Scope = InjectionScope.Singleton)]
    public class QueueRealExecuteServiceForAzureServiceBusFactory : IFactory<IQueueRealExecuteService>
    {
        private QueueRealExecuteServiceForAzureServiceBus _queueRealExecuteServiceForAzureServiceBus;

        public QueueRealExecuteServiceForAzureServiceBusFactory(QueueRealExecuteServiceForAzureServiceBus queueRealExecuteServiceForAzureServiceBus)
        {
            _queueRealExecuteServiceForAzureServiceBus = queueRealExecuteServiceForAzureServiceBus;
        }
        public IQueueRealExecuteService Create()
        {
            return _queueRealExecuteServiceForAzureServiceBus;
        }
    }
}
