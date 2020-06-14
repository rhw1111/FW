using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.CommonQueue
{
    public interface IQueueRealExecuteService
    {
        Task Product(CommonQueueProductEndpoint endpoint, string configuration, CommonMessage message);
        Task<ICommonQueueEndpointConsumeController> Consume(CommonQueueConsumeEndpoint endpoint, string configuration, Func<CommonMessage, Task<MessageHandleResult>> messageHandle);
    }
}
