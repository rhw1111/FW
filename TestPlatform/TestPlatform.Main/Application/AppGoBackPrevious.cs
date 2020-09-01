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
    [Injection(InterfaceType = typeof(IAppGoBackPrevious), Scope = InjectionScope.Singleton)]
    public class AppGoBackPrevious : IAppGoBackPrevious
    {
        private readonly ITreeEntityRepository _treeEntityRepository;

        public AppGoBackPrevious(ITreeEntityRepository treeEntityRepository)
        {
            _treeEntityRepository = treeEntityRepository;
        }

        public async Task<QueryResult<TreeEntityViewModel>> Do(Guid treeEntityId,int page, int pageSize, CancellationToken cancellationToken = default)
        {
            QueryResult<TreeEntityViewModel> result = new QueryResult<TreeEntityViewModel>();
            var treeEntity = await _treeEntityRepository.QueryByID(treeEntityId, cancellationToken);
            if (treeEntity == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TestPlatformTextCodes.NotFoundTreeEntityByID,
                    DefaultFormatting = "找不到ID为{0}的测试案例",
                    ReplaceParameters = new List<object>() { treeEntityId.ToString() }
                };

                throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTreeEntityByID, fragment, 1, 0);
            }
            QueryResult<TreeEntity> queryResult = new QueryResult<TreeEntity>();
            if (treeEntity.ParentID != null)
            {
                TreeEntity? parentEntity = await _treeEntityRepository.QueryByID(treeEntity.ParentID.Value, cancellationToken);
                if (parentEntity == null)
                {
                    var fragment = new TextFragment()
                    {
                        Code = TestPlatformTextCodes.NotFoundTreeEntityByID,
                        DefaultFormatting = "找不到ID为{0}的测试案例",
                        ReplaceParameters = new List<object>() { treeEntity.ParentID.Value.ToString() }
                    };

                    throw new UtilityException((int)TestPlatformErrorCodes.NotFoundTreeEntityByID, fragment, 1, 0);
                }
                queryResult = await treeEntity.GetChildren(string.Empty, null, page, pageSize, cancellationToken);
            }
            else
                queryResult = await _treeEntityRepository.QueryRoot(string.Empty, null, page, pageSize, cancellationToken);

            result.CurrentPage = queryResult.CurrentPage;
            result.TotalCount = queryResult.TotalCount;

            foreach(var item in queryResult.Results)
            {
                result.Results.Add(
                    new TreeEntityViewModel()
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Type = item.Type,
                        Value = item.Value,
                        CreateTime = item.CreateTime
                    }
                    );
            }

            return result;
        }
    }
}
