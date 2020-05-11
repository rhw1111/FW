using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SystemToken.DAL
{
    /// <summary>
    /// 验证终结点数据操作
    /// </summary>
    public interface IAuthorizationEndpointStore
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        Task Add(AuthorizationEndpoint endpoint);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        Task Update(AuthorizationEndpoint endpoint);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(Guid id);
        /// <summary>
        /// 新增与系统登录终结点的关联关系
        /// </summary>
        /// <param name="authorizationId"></param>
        /// <param name="systemLoginEndpointId"></param>
        /// <returns></returns>
        Task AddSystemLoginEndpointRelation(Guid authorizationId,Guid systemLoginEndpointId);
        /// <summary>
        /// 删除与系统登陆终结点的关联关系
        /// </summary>
        /// <param name="authorizationId"></param>
        /// <param name="systemLoginEndpointId"></param>
        /// <returns></returns>
        Task DeleteSystemLoginEndpointRelation(Guid authorizationId, Guid systemLoginEndpointId);
        /// <summary>
        /// 根据关联的系统登录终结点分页查询匹配名称的关联验证终结点
        /// </summary>
        /// <param name="systemLoginEndpointId"></param>
        /// <param name="authorizationName"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<AuthorizationEndpoint>> QueryBySystemLoginEndpointRelationPage(Guid systemLoginEndpointId,string authorizationName,int page,int pageSize);
        /// <summary>
        /// 根据关联的系统登录终结点查询指定Id的关联验证终结点
        /// </summary>
        /// <param name="systemLoginEndpointId"></param>
        /// <param name="authorizationId"></param>
        /// <returns></returns>
        Task<AuthorizationEndpoint> QueryBySystemLoginEndpointRelation(Guid systemLoginEndpointId, Guid authorizationId);
        /// <summary>
        /// 根据关联的系统登录终结点查询指定名称的关联验证终结点
        /// </summary>
        /// <param name="systemLoginEndpointId"></param>
        /// <param name="authorizationName"></param>
        /// <returns></returns>
        Task<AuthorizationEndpoint> QueryBySystemLoginEndpointRelation(Guid systemLoginEndpointId, string authorizationName);
        /// <summary>
        /// 根据Id查询验证终结点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AuthorizationEndpoint> QueryById(Guid id);
        /// <summary>
        /// 根据名称查询验证终结点
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<AuthorizationEndpoint> QueryByName(string name);

        /// <summary>
        /// 分页查询匹配名称的验证终结点
        /// </summary>
        /// <param name="authorizationName"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<AuthorizationEndpoint>> QueryByPage(string authorizationName, int page, int pageSize);

    }
}
