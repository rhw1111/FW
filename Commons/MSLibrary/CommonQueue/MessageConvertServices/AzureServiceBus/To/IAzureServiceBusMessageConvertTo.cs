using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace MSLibrary.CommonQueue.MessageConvertServices.AzureServiceBus.To
{
    public interface IAzureServiceBusMessageConvertTo
    {
        Task<Message> To(CommonMessage commonMessage);
    }
}
