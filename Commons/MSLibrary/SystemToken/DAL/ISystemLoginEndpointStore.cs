using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.SystemToken.DAL
{
    /// <summary>
    /// 系统登录终结点数据操作
    /// </summary>
    public interface ISystemLoginEndpointStore
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        Task Add(SystemLoginEndpoint endpoint);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        Task Update(SystemLoginEndpoint endpoint);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(Guid id);
        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SystemLoginEndpoint> QueryById(Guid id);
        /// <summary>
        /// 根据名称查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<SystemLoginEndpoint> QueryByName(string name);
        /// <summary>
        /// 根据名称匹配分页查询
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<SystemLoginEndpoint>> QueryByPage(string name,int page,int pageSize);
    }
}
