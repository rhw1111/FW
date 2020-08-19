using FW.TestPlatform.Main.DTOModel;
using MSLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using FW.TestPlatform.Main.Entities;
using System.Reflection;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppGetNetGatewayDataFormatTypes), Scope = InjectionScope.Singleton)]
    public class AppGetNetGatewayDataFormatTypes : IAppGetNetGatewayDataFormatTypes
    {
        private readonly ITestHostRepository _testHostRepository;

        public AppGetNetGatewayDataFormatTypes(ITestHostRepository testHostRepository)
        {
            _testHostRepository = testHostRepository;
        }

        public List<string> Do(CancellationToken cancellationToken = default)
        {
            List<string> result = new List<string>();
            Type types = typeof(FW.TestPlatform.Main.NetGatewayDataFormatTypes);
            FieldInfo[] dataFormateTypes = types.GetFields();
            foreach (var fieldInfo in dataFormateTypes)
            {
                result.Add(fieldInfo.Name.ToString());
            }
            return result;
        }
    }
}
