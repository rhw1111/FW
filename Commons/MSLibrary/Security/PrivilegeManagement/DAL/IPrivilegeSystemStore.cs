using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Security.PrivilegeManagement.DAL
{
    /// <summary>
    /// 权限系统的数据操作
    /// </summary>
    public interface IPrivilegeSystemStore
    {

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        Task Add(PrivilegeSystem system);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        Task Update(PrivilegeSystem system);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(Guid id);
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PrivilegeSystem> QueryById(Guid id);
        /// <summary>
        /// 根据名称查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<PrivilegeSystem> QueryByName(string name);
        /// <summary>
        /// 根据名称匹配分页查询
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<QueryResult<PrivilegeSystem>> QueryByName(string name,int page,int pageSize);
    }
}
