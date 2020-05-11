using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.Template
{
    [Injection(InterfaceType = typeof(ILabelParameterRepository), Scope = InjectionScope.Singleton)]
    public class LabelParameterRepository : ILabelParameterRepository
    {
        public async Task<LabelParameter> QueryByName(string name)
        {
            var labelParameter = new LabelParameter()
            {
                ID = Guid.NewGuid(),
                Name = name
            };

            return await Task.FromResult(labelParameter);
        }
    }
}
