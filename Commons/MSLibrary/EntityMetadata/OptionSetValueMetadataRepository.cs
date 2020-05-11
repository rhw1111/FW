using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.EntityMetadata.DAL;
using MSLibrary.DI;

namespace MSLibrary.EntityMetadata
{
    [Injection(InterfaceType = typeof(IOptionSetValueMetadataRepository), Scope = InjectionScope.Singleton)]
    public class OptionSetValueMetadataRepository : IOptionSetValueMetadataRepository
    {
        private IOptionSetValueMetadataStore _optionSetValueMetadataStore;

        public OptionSetValueMetadataRepository(IOptionSetValueMetadataStore optionSetValueMetadataStore)
        {
            _optionSetValueMetadataStore = optionSetValueMetadataStore;
        }

        public async Task<OptionSetValueMetadata> QueryById(Guid id)
        {
            return await _optionSetValueMetadataStore.QueryById(id);
        }

        public async Task<OptionSetValueMetadata> QueryByName(string name)
        {
            return await _optionSetValueMetadataStore.QueryByName(name);
        }

        public async Task<QueryResult<OptionSetValueMetadata>> QueryByName(string name, int page, int pageSize)
        {
            return await _optionSetValueMetadataStore.QueryByName(name,page,pageSize);
        }
    }
}
