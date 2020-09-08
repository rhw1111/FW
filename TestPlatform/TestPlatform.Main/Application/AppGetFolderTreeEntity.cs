using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary;
using MSLibrary.DI;
using FW.TestPlatform.Main.DTOModel;
using FW.TestPlatform.Main.Entities;
using System.Linq;
using System.Diagnostics.Tracing;
using MSLibrary.Transaction;
using MSLibrary.Collections;
using MSLibrary.Collections.DAL;
using MSLibrary.LanguageTranslate;

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppGetFolderTreeEntity), Scope = InjectionScope.Singleton)]
    public class AppGetFolderTreeEntity : IAppGetFolderTreeEntity
    {
        private readonly ITreeEntityStore _treeEntityStore;
        public AppGetFolderTreeEntity(ITreeEntityStore treeEntityStore)
        {
            _treeEntityStore = treeEntityStore;
        }
        public async Task<TreeEntityViewModel> Do(TreeEntityAddModel model, CancellationToken cancellationToken = default)
        {
            TreeEntityViewModel result;
            var tree = await _treeEntityStore.QueryByName(model.FolderID, model.Name);
            if (tree != null && tree.Type != TreeEntityValueServiceTypes.Folder)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.ExistSameNameTreeEntity,
                    DefaultFormatting = "已经存在名称为{0}的节点",
                    ReplaceParameters = new List<object>() { tree.Name }
                };
                throw new UtilityException((int)Errors.ExistSameNameTreeEntity, fragment, 1, 0);
            }
            else if(tree != null && tree.Type == TreeEntityValueServiceTypes.Folder)
            {
                result = new TreeEntityViewModel()
                {
                    ID = tree.ID,
                    Name = tree.Name,
                    Type = tree.Type,
                    Value = tree.Value,
                    ParentID = tree.ParentID,
                    CreateTime = tree.CreateTime.ToCurrentUserTimeZone()
                };
                return result;
            }
            await using (DBTransactionScope scope = new DBTransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 0, 30) }))
            {
                TreeEntity treeEntity = new TreeEntity
                {
                    ID = Guid.NewGuid(),
                    ParentID = model.FolderID,
                    Value = null,
                    Name = model.Name,
                    Type = TreeEntityValueServiceTypes.Folder
                };
                await treeEntity.Add(cancellationToken);
                scope.Complete();
                result = new TreeEntityViewModel()
                {
                    ID = treeEntity.ID,
                    Name = treeEntity.Name,
                    Type = treeEntity.Type,
                    ParentID = treeEntity.ParentID,
                    CreateTime = treeEntity.CreateTime.ToCurrentUserTimeZone()
                };
            }
            return result;
        }
    }
}
