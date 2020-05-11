using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.SerialNumber.DAL;

namespace MSLibrary.SerialNumber
{
    [Injection(InterfaceType = typeof(ISerialNumberGeneratorConfigurationRepository), Scope = InjectionScope.Singleton)]
    public class SerialNumberGeneratorConfigurationRepository : ISerialNumberGeneratorConfigurationRepository
    {
        private ISerialNumberGeneratorConfigurationStore _serialNumberGeneratorConfigurationStore;
        public SerialNumberGeneratorConfigurationRepository(ISerialNumberGeneratorConfigurationStore serialNumberGeneratorConfigurationStore)
        {
            _serialNumberGeneratorConfigurationStore = serialNumberGeneratorConfigurationStore;
        }
        public async Task<SerialNumberGeneratorConfiguration> QueryByName(string name)
        {
            return await _serialNumberGeneratorConfigurationStore.QueryByName(name);
        }
    }
}
