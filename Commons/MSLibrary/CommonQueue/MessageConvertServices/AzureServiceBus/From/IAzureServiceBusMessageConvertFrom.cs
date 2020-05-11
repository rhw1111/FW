using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace MSLibrary.CommonQueue.MessageConvertServices.AzureServiceBus.From
{
    public interface IAzureServiceBusMessageConvertFrom
    {
        Task<CommonMessage> From(Message message);
    }
}
