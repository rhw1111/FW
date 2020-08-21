using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MSLibrary.DI;
using MSLibrary.Serializer;

namespace MSLibrary.Collections.TreeEntityValueServices
{
    [Injection(InterfaceType = typeof(TreeEntityValueServiceForFolder), Scope = InjectionScope.Singleton)]
    public class TreeEntityValueServiceForFolder : ITreeEntityValueService
    {
        public async Task Delete(string value, CancellationToken cancellationToken = default)
        {
            await Task.FromResult(0);
        }

        public async Task<JObject> GetFormatValue(string value, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult<JObject>(null);
        }

        public async Task UpdateName(string name, string value, CancellationToken cancellationToken = default)
        {
            await Task.FromResult(0);
        }
    }
}
