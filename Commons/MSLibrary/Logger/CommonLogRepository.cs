using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Logger.DAL;

namespace MSLibrary.Logger
{

    [Injection(InterfaceType = typeof(ICommonLogRepository), Scope = InjectionScope.Singleton)]
    public class CommonLogRepository : ICommonLogRepository
    {
        private ICommonLogStore _commonLogStore;

        public CommonLogRepository(ICommonLogStore commonLogStore)
        {
            _commonLogStore = commonLogStore;
        }
        public async Task<CommonLog> QueryByID(Guid id, Guid parentID, string parentAction)
        {
            return await _commonLogStore.QueryByID(id, parentID, parentAction);
        }

        public async Task<QueryResult<CommonLog>> QueryByParentId(Guid parentID, string parentAction, int page, int pageSize)
        {
            return await _commonLogStore.QueryByParentId(parentID, parentAction, page, pageSize);
        }

        public async Task<QueryResult<CommonLog>> QueryLocal(string message, int page, int pageSize)
        {
            return await _commonLogStore.QueryLocal(message, page, pageSize);
        }

        public async Task<CommonLog> QueryLocalByID(Guid id)
        {
            return await _commonLogStore.QueryLocalByID(id);
        }

        public async Task<List<CommonLog>> QueryRootByConditionTop(string parentAction, int? level, int top)
        {
            return await _commonLogStore.QueryRootByConditionTop(parentAction, level, top);
        }
    }
}
