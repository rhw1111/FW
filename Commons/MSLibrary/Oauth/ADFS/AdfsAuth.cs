using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MSLibrary.Oauth.ADFS
{
    [DataContract]
    public class AdfsAuth
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }

        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }

        [DataMember(Name = "expires_in")]
        public int Expires { get; set; }
        [DataMember(Name = "refresh_token")]
        public string RefreshToken { get; set; }
    }
}
