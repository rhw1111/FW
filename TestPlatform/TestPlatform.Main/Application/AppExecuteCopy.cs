using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;
using FW.TestPlatform.Main.Configuration;
using System.Linq;
using MSLibrary.LanguageTranslate;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppExecuteCopy), Scope = InjectionScope.Singleton)]
    public class AppExecuteCopy : IAppExecuteCopy
    {
        private readonly IEntityTreeCopyService _entityTreeCopyService;
        public AppExecuteCopy(IEntityTreeCopyService entityTreeCopyService)
        {
            _entityTreeCopyService = entityTreeCopyService;
        }
        public async Task<bool> Do(ExecuteCopyModel model, CancellationToken cancellationToken = default)
        {
            return await _entityTreeCopyService.Execute(model.Type, model.ID, model.ParentID);
        }
    }
}
