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
    [Injection(InterfaceType = typeof(TreeEntityValueServiceForTestCase), Scope = InjectionScope.Singleton)]
    public class TreeEntityValueServiceForTestCase : ITreeEntityValueService
    {
        private readonly ITestCaseRepository _testCaseRepository;

        public TreeEntityValueServiceForTestCase(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
        }
        public async Task Delete(string value, CancellationToken cancellationToken = default)
        {

            Guid testId = Guid.Parse(value);
            var testCase=await _testCaseRepository.QueryByID(testId, cancellationToken);
            if (testCase != null)
            {
                await testCase.Delete(cancellationToken);
            }
        }

        public async Task<JObject> GetFormatValue(string value, CancellationToken cancellationToken = default)
        {
            return await Task.FromResult(JsonSerializerHelper.Deserialize<JObject>($"\"{value}\""));
        }

        public async Task UpdateName(string name, string value, CancellationToken cancellationToken = default)
        {
            Guid testId = Guid.Parse(value);
            var testCase = await _testCaseRepository.QueryByID(testId, cancellationToken);
            if (testCase != null)
            {
                testCase.Name = name;
                await testCase.Update(cancellationToken);
            }
        }
    }
}
