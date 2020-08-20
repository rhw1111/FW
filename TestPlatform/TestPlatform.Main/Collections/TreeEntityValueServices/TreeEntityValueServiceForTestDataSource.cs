using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MSLibrary.Collections;
using MSLibrary.DI;
using MSLibrary.Serializer;
using FW.TestPlatform.Main.Entities;

namespace FW.TestPlatform.Main.Collections.TreeEntityValueServices
{
    [Injection(InterfaceType = typeof(TreeEntityValueServiceForTestDataSource), Scope = InjectionScope.Singleton)]
    public class TreeEntityValueServiceForTestDataSource : ITreeEntityValueService
    {
        private readonly ITestDataSourceRepository _testDataSourceRepository;

        public TreeEntityValueServiceForTestDataSource(ITestDataSourceRepository testDataSourceRepository)
        {
            _testDataSourceRepository = testDataSourceRepository;
        }

        public async Task Delete(string value, CancellationToken cancellationToken = default)
        {
            Guid sourceId = Guid.Parse(value);
            var source = await _testDataSourceRepository.QueryByID(sourceId, cancellationToken);
            if (source != null)
            {
                await source.Delete(cancellationToken);
            }
        }

        public async Task<JObject> GetFormatValue(string value, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(JsonSerializerHelper.Deserialize<JObject>($"\"{value}\""));
        }

        public async Task UpdateName(string name, string value, CancellationToken cancellationToken = default)
        {
            Guid sourceId = Guid.Parse(value);
            var source = await _testDataSourceRepository.QueryByID(sourceId, cancellationToken);
            if (source != null)
            {
                source.Name = name;
                await source.Update(cancellationToken);
            }
        }
    }
}
