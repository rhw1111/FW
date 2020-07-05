using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace MSLibrary
{
    public interface IHttpClientFactoryWrapper:IHttpClientFactory
    {
        HttpClient Create();
    }
}
