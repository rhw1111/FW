using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.CommonQueue.MessageConvertServices.AzureServiceBus.From
{
    [Injection(InterfaceType = typeof(AzureServiceBusMessageConvertFromForDefaultFactory), Scope = InjectionScope.Singleton)]
    public class AzureServiceBusMessageConvertFromForDefaultFactory : IFactory<IAzureServiceBusMessageConvertFrom>
    {
        private AzureServiceBusMessageConvertFromForDefault _azureServiceBusMessageConvertFromForDefault;

        public AzureServiceBusMessageConvertFromForDefaultFactory(AzureServiceBusMessageConvertFromForDefault azureServiceBusMessageConvertFromForDefault)
        {
            _azureServiceBusMessageConvertFromForDefault = azureServiceBusMessageConvertFromForDefault;
        }
        public IAzureServiceBusMessageConvertFrom Create()
        {
            return _azureServiceBusMessageConvertFromForDefault;
        }
    }
}
