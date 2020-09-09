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
    [Injection(InterfaceType = typeof(IAppQueryChild), Scope = InjectionScope.Singleton)]
    public class AppQueryChild : IAppQueryChild
    {
        private readonly ITreeEntityRepository _treeEntityRepository;

        public AppQueryChild(ITreeEntityRepository treeEntityRepository)
        {
            _treeEntityRepository = treeEntityRepository;
        }

        public async Task<TreeEntityViewModel?> Do(Guid? parentId, string name, CancellationToken cancellationToken = default)
        {
            TreeEntityViewModel? viewModel = null;
            TreeEntity? relTreeEntity = null;
            if (parentId != null)
            {
                TreeEntity? treeEntity = await _treeEntityRepository.QueryByID(parentId.Value, cancellationToken);
                if (treeEntity == null)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TestPlatformTextCodes.NotFoundTreeEntityByID,
                        DefaultFormatting = "找不到ID为{0}的节点",
                        ReplaceParameters = new List<object>() { parentId.Value.ToString() }
                    };

                    throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTreeEntityByID, fragment, 1, 0);
                }
                relTreeEntity = await treeEntity.GetChildren(name);
            }
            else
                relTreeEntity = await _treeEntityRepository.QueryRootChild(name, cancellationToken);
            
            if(relTreeEntity != null)
            {
                viewModel = new TreeEntityViewModel()
                {
                    ID = relTreeEntity.ID,
                    ParentID = relTreeEntity.ParentID,
                    Name = relTreeEntity.Name,
                    Value = relTreeEntity.Value,
                    Type = relTreeEntity.Type,
                    CreateTime = relTreeEntity.CreateTime
                };
            }
            return viewModel;
        }
    }
}
