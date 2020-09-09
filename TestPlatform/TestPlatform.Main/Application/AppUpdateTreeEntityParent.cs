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
    [Injection(InterfaceType = typeof(IAppUpdateTreeEntityParent), Scope = InjectionScope.Singleton)]
    public class AppUpdateTreeEntityParent : IAppUpdateTreeEntityParent
    {
        private readonly ITreeEntityRepository _treeEntityRepository;

        public AppUpdateTreeEntityParent(ITreeEntityRepository treeEntityRepository)
        {
            _treeEntityRepository = treeEntityRepository;
        }

        public async Task Do(Guid treeEntityId, Guid? parentId, CancellationToken cancellationToken = default)
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
            await treeEntity.UpdateParent(parentId, cancellationToken);
        }
    }
}
