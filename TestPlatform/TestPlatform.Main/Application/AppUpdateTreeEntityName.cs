using FW.TestPlatform.Main.DTOModel;
using MSLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.DI;
using FW.TestPlatform.Main.Entities;
using MSLibrary.Collections;
using MSLibrary.LanguageTranslate;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppUpdateTreeEntityName), Scope = InjectionScope.Singleton)]
    public class AppUpdateTreeEntityName : IAppUpdateTreeEntityName
    {
        private readonly ITreeEntityRepository _treeEntityRepository;

        public AppUpdateTreeEntityName(ITreeEntityRepository treeEntityRepository)
        {
            _treeEntityRepository = treeEntityRepository;
        }

        public async Task Do(Guid treeEntityId, string name, CancellationToken cancellationToken = default)
        {
            TreeEntity? treeEntity = await _treeEntityRepository.QueryByID(treeEntityId, cancellationToken);
            if (treeEntity == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTreeEntityByID,
                    DefaultFormatting = "找不到ID为{0}的节点",
                    ReplaceParameters = new List<object>() { treeEntityId.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTreeEntityByID, fragment, 1, 0);
            }
            treeEntity.Name = name;
            await treeEntity.UpdateName(name, cancellationToken);
        }
    }
}
