using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SystemToken.DAL
{
    /// <summary>
    /// 客户端系统登陆终结点数据操作
    /// </summary>
    public interface IClientSystemLoginEndpointStore
    {
        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="endpoint">客户端系统登陆终结点类对象</param>
        /// <returns></returns>
        Task Add(ClientSystemLoginEndpoint endpoint);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="endpoint">客户端系统登陆终结点类对象</param>
        /// <returns></returns>
        Task Update(ClientSystemLoginEndpoint endpoint);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(Guid id);
        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ClientSystemLoginEndpoint> QueryById(Guid id);
        /// <summary>
        /// 根据名称查询
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        Task<ClientSystemLoginEndpoint> QueryByName(string name);
        /// <summary>
        /// 根据名称匹配分页查询
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="page">页数</param>
        /// <param name="pageSize">条数</param>
        /// <returns></returns>
        Task<QueryResult<ClientSystemLoginEndpoint>> QueryByPage(string name, int page, int pageSize);
    }
}
