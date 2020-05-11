using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;
using Grpc.Core;
using MSLibrary.Serializer;
using MSLibrary.DI;

namespace MSLibrary.Grpc.ChannelCredentialsGeneratorServices
{
    /// <summary>
    /// 针对SSL的通道凭证
    /// configuration格式为
    /// {
    ///     "CAFile":"CA证书基于工作目录的文件路径",
    ///     "ClientFile":"客户端证书基于工作目录的文件路径",
    ///     "KeyFile":"客户端证书私钥基于工作目录的文件路径",
    ///     "ClientStrategy":"验证客户端策略，1：不验证，2：验证"
    /// }
    /// </summary>
    [Injection(InterfaceType = typeof(ChannelCredentialsGeneratorServiceForSSL), Scope = InjectionScope.Singleton)]
    public class ChannelCredentialsGeneratorServiceForSSL:IChannelCredentialsGeneratorService
    {
        public async Task<ChannelCredentials> Generate(string type, string configuration)
        {
            var sslConfiguration = JsonSerializerHelper.Deserialize<SSLConfiguration>(configuration);
            var cacert = File.ReadAllText(sslConfiguration.CAFile);

            SslClientCertificateRequestType clientStrategy;
            switch (sslConfiguration.ClientStrategy)
            {
                case 1:
                    clientStrategy = SslClientCertificateRequestType.DontRequest;
                    break;
                default:
                    clientStrategy = SslClientCertificateRequestType.RequestAndRequireAndVerify;
                    break;
            }

            SslCredentials sslCredentials;
            if (clientStrategy== SslClientCertificateRequestType.RequestAndRequireAndVerify)
            {
                var servercert = File.ReadAllText(sslConfiguration.ChannelFile);
                var serverkey = File.ReadAllText(sslConfiguration.KeyFile);
                var keypair = new KeyCertificatePair(servercert, serverkey);
                sslCredentials = new SslCredentials(cacert, keypair);
            }
            else
            {
                sslCredentials = new SslCredentials(cacert);
            }
            return await Task.FromResult(sslCredentials);
        }

        [DataContract]
        private class SSLConfiguration
        {
            [DataMember]
            public string CAFile { get; set; }
            [DataMember]
            public string ChannelFile { get; set; }
            [DataMember]
            public string KeyFile { get; set; }
            [DataMember]
            public int ClientStrategy { get; set; }
        }
    }
}
