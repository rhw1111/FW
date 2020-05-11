using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MSLibrary.Xrm.Convert
{
    /// <summary>
    /// 查询结果的JToken转换服务
    /// </summary>
    public interface ICrmRetrieveJTokenConvertService
    {
        Task<T> Convert<T>(JToken json, Dictionary<string, object> extensionParameters = null);
    }
}
