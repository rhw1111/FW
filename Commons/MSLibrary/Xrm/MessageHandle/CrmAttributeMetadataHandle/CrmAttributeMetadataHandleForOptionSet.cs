using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;
using MSLibrary.Xrm.Metadata;
using MSLibrary.Serializer;

namespace MSLibrary.Xrm.MessageHandle.CrmAttributeMetadataHandle
{
    [Injection(InterfaceType = typeof(CrmAttributeMetadataHandleForOptionSet), Scope = InjectionScope.Singleton)]
    public class CrmAttributeMetadataHandleForOptionSet : ICrmAttributeMetadataHandle
    {
        public async Task<object> Execute(string body)
        {
            CrmPicklistAttributeMetadata result = new CrmPicklistAttributeMetadata();
            var jObject = JsonSerializerHelper.Deserialize<JObject>(body);
            result.OptionSet= JsonSerializerHelper.Deserialize<CrmOptionSetMetadata>(body);
            result.MetadataId = Guid.Parse(jObject["MetadataId"].Value<string>());

            return await Task.FromResult(result);
        }
    }
}
