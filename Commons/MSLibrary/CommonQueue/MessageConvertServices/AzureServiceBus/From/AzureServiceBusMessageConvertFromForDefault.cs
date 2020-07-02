using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Serializer;

namespace MSLibrary.CommonQueue.MessageConvertServices.AzureServiceBus.From
{
    [Injection(InterfaceType = typeof(AzureServiceBusMessageConvertFromForDefault), Scope = InjectionScope.Singleton)]
    public class AzureServiceBusMessageConvertFromForDefault : IAzureServiceBusMessageConvertFrom
    {
        public async Task<CommonMessage> From(Message message)
        {
            var data=UTF8Encoding.UTF8.GetString(message.Body);
            return await Task.FromResult(JsonSerializerHelper.Deserialize<CommonMessage>(data));
        }
    }
}
