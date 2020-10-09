using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Grpc.Interceptors
{
    public class GrpcLogContextData
    {
        public GrpcLogContextData(string requestUri, string requestPath, string requestBasePath, long duration)
        {
            RequestUri = requestUri;
            RequestPath = requestPath;
            RequestBasePath = requestBasePath;
            Duration = duration;
        }

        public string RequestUri { get; private set; }

        public string RequestPath { get; private set; }
        public string RequestBasePath { get; private set; }

        public long Duration { get; private set; }
    }
}
