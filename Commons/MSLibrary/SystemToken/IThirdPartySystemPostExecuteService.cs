using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace MSLibrary.SystemToken
{
    /// <summary>
    /// 第三方系统后续处理服务接口
    /// </summary>
    public interface IThirdPartySystemPostExecuteService
    {
        Task<ThirdPartySystemPostExecuteResult> Execute(Dictionary<string,string> attributes,string configurationInfo);
    }


    [DataContract]
    public class ThirdPartySystemPostExecuteResult
    {
        [DataMember]
        public Dictionary<string, string> UserInfoAttributes { get; set; }

        [DataMember]
        public Dictionary<string, string> AdditionalRedirectUrlQueryAttributes { get; set; }
    }
}
