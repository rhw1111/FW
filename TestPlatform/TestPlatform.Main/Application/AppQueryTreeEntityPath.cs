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
    [Injection(InterfaceType = typeof(IAppQueryTreeEntityPath), Scope = InjectionScope.Singleton)]
    public class AppQueryTreeEntityPath : IAppQueryTreeEntityPath
    {
        private readonly ITreeEntityRepository _treeEntityRepository;

        public AppQueryTreeEntityPath(ITreeEntityRepository treeEntityRepository)
        {
            _treeEntityRepository = treeEntityRepository;
        }

        public async Task<List<string>> Do(Guid treeEntityId, CancellationToken cancellationToken = default)
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
            IList<string> list = await treeEntity.GetPath(cancellationToken);
            List<string> path = new List<string>(list);
            return path;
        }
    }
}
