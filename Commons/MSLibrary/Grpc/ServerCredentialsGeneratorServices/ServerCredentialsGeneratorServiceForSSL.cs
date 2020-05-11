using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;
using Grpc.Core;
using MSLibrary.Serializer;
using MSLibrary.DI;

namespace MSLibrary.Grpc.ServerCredentialsGeneratorServices
{
    /// <summary>
    /// 针对SSL的服务端凭证
    /// configuration格式为
    /// {
    ///     "CAFile":"CA证书基于工作目录的文件路径",
    ///     "ServerFile":"服务端证书基于工作目录的文件路径",
    ///     "KeyFile":"服务端证书私钥基于工作目录的文件路径",
    ///     "ClientStrategy":"验证客户端策略，1：不验证，2：验证"
    /// }
    /// </summary>
    [Injection(InterfaceType = typeof(ServerCredentialsGeneratorServiceForSSL), Scope = InjectionScope.Singleton)]
    public class ServerCredentialsGeneratorServiceForSSL:IServerCredentialsGeneratorService
    {
        public async Task<ServerCredentials> Generate(string type, string configuration)
        {
            var sslConfiguration=JsonSerializerHelper.Deserialize<SSLConfiguration>(configuration);
            var cacert = File.ReadAllText(sslConfiguration.CAFile);
            var servercert = File.ReadAllText(sslConfiguration.ServerFile);
            var serverkey = File.ReadAllText(sslConfiguration.KeyFile);
            var keypair = new KeyCertificatePair(servercert, serverkey);

            SslClientCertificateRequestType clientStrategy;
            switch(sslConfiguration.ClientStrategy)
            {
                case 1:
                    clientStrategy = SslClientCertificateRequestType.DontRequest;
                    break;
                default:
                    clientStrategy = SslClientCertificateRequestType.RequestAndRequireAndVerify;
                    break;
            }

            var sslCredentials = new SslServerCredentials(new List<KeyCertificatePair>() { keypair }, cacert, clientStrategy);
            return await Task.FromResult(sslCredentials);
        }

        [DataContract]
        private class SSLConfiguration
        {
            [DataMember]
            public string CAFile { get; set; }
            [DataMember]
            public string ServerFile { get; set; }
            [DataMember]
            public string KeyFile { get; set; }
            [DataMember]
            public int ClientStrategy { get; set; }
        }
    }
}
