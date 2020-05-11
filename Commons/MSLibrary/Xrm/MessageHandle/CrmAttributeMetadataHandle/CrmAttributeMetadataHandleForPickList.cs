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
    [Injection(InterfaceType = typeof(CrmAttributeMetadataHandleForPickList), Scope = InjectionScope.Singleton)]
    public class CrmAttributeMetadataHandleForPickList : ICrmAttributeMetadataHandle
    {
        public async Task<object> Execute(string body)
        {
            var result=JsonSerializerHelper.Deserialize<CrmPicklistAttributeMetadata>(body);
            return await Task.FromResult(result);
        }
    }
}
