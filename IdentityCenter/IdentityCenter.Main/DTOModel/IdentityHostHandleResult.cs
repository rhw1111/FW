using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Tokens;
using MSLibrary;

namespace IdentityCenter.Main.DTOModel
{
    /// <summary>
    /// 认证主机处理结果
    /// </summary>
    [DataContract]
    public class IdentityHostHandleResult:ModelBase
    {
        /// <summary>
        /// 允许的跨域源列表
        /// </summary>
        [DataMember]
        public List<string> AllowedCorsOrigins
        {
            get
            {

                return GetAttribute<List<string>>(nameof(AllowedCorsOrigins));
            }
            set
            {
                SetAttribute<List<string>>(nameof(AllowedCorsOrigins), value);
            }
        }

        /// <summary>
        /// 签名密钥
        /// </summary>
        [DataMember]
        public SigningCredentials SigningCredentials
        {
            get
            {

                return GetAttribute<SigningCredentials>(nameof(SigningCredentials));
            }
            set
            {
                SetAttribute<SigningCredentials>(nameof(SigningCredentials), value);
            }
        }

        /// <summary>
        /// 签名证书
        /// </summary>
        [DataMember]
        public X509Certificate2 SignCredentialCertificate
        {
            get
            {

                return GetAttribute<X509Certificate2>(nameof(SignCredentialCertificate));
            }
            set
            {
                SetAttribute<X509Certificate2>(nameof(SignCredentialCertificate), value);
            }
        }
    }
}
