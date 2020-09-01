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

namespace FW.TestPlatform.Main.Application
{
    [Injection(InterfaceType = typeof(IAppQueryTreeEntities), Scope = InjectionScope.Singleton)]
    public class AppQueryTreeEntities : IAppQueryTreeEntities
    {
        private readonly ITreeEntityRepository _treeEntityRepository;

        public AppQueryTreeEntities(ITreeEntityRepository treeEntityRepository)
        {
            _treeEntityRepository = treeEntityRepository;
        }

        public async Task<QueryResult<TreeEntityViewModel>> Do(string? matchName, int? type, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            QueryResult<TreeEntityViewModel> result = new QueryResult<TreeEntityViewModel>();
            var queryResult = await _treeEntityRepository.Query(matchName, type, page, pageSize, cancellationToken);

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
