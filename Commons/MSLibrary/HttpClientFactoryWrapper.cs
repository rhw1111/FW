using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary
{
    [Injection(InterfaceType = typeof(IHttpClientFactoryWrapper), Scope = InjectionScope.Singleton)]
    public class HttpClientFactoryWrapper : IHttpClientFactoryWrapper
    {
        public HttpClient Create()
        {
            var clientFactory = DIContainerContainer.Get<IHttpClientFactory>();
            return clientFactory.CreateClient();
        }

        public HttpClient CreateClient(string name)
        {
            var clientFactory= DIContainerContainer.Get<IHttpClientFactory>();
            return clientFactory.CreateClient(name);
        }
    }
}
