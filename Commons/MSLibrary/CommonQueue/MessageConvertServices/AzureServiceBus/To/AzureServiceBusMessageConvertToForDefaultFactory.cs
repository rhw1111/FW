using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.CommonQueue.MessageConvertServices.AzureServiceBus.To
{
    [Injection(InterfaceType = typeof(AzureServiceBusMessageConvertToForDefaultFactory), Scope = InjectionScope.Singleton)]
    public class AzureServiceBusMessageConvertToForDefaultFactory : IFactory<IAzureServiceBusMessageConvertTo>
    {
        private AzureServiceBusMessageConvertToForDefault _azureServiceBusMessageConvertToForDefault;

        public AzureServiceBusMessageConvertToForDefaultFactory(AzureServiceBusMessageConvertToForDefault azureServiceBusMessageConvertToForDefault)
        {
            _azureServiceBusMessageConvertToForDefault = azureServiceBusMessageConvertToForDefault;
        }
        public IAzureServiceBusMessageConvertTo Create()
        {
            return _azureServiceBusMessageConvertToForDefault;
        }
    }
}
