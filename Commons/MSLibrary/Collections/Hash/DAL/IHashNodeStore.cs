using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Collections.Hash.DAL
{
    /// <summary>
    /// 哈希节点数据操作
    /// </summary>
    public interface IHashNodeStore
    {
        Task Add(HashNode node);
        Task Update(Guid groupId,HashNode node);
        Task UpdateStatus(Guid nodeId, int status);

        Task DeleteByRelation(Guid groupId,Guid id);

        Task<HashNode> QueryByGroup(Guid groupId, Guid nodeId);

        Task<QueryResult<HashNode>> QueryByGroup(Guid groupId, int page, int pageSize);

        /// <summary>
        /// 查询指定组中第一个code大于指定code的指定状态的节点
        /// 如果找不到，返回null
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="status"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<HashNode> QueryFirstByGreaterCode(Guid groupId, long code, params int[] status);

        /// <summary>
        /// 查询指定组中第一个code大于指定code的指定状态的节点（同步）
        /// 如果找不到，返回null
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="code"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        HashNode QueryFirstByGreaterCodeSync(Guid groupId, long code, params int[] status);

        /// <summary>
        /// 查询指定组中第一个code小于指定code的指定状态的节点
        /// 如果找不到，返回null
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="status"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<HashNode> QueryFirstByLessCode(Guid groupId, long code, params int[] status);
        /// <summary>
        /// 查询指定组中指定状态的最小code的节点
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<HashNode> QueryByMinCode(Guid groupId, params int[] status);

        /// <summary>
        /// 查询指定组中指定状态的最小code的节点(同步)
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        HashNode QueryByMinCodeSync(Guid groupId, params int[] status);

        /// <summary>
        /// 查询指定组中指定状态的最大code的节点
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<HashNode> QueryByMaxCode(Guid groupId, params int[] status);


        /// <summary>
        /// 查询指定组中的最小code的节点
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<HashNode> QueryByMinCode(Guid groupId);


        /// <summary>
        /// 查询指定组中指定状态的最大code的节点
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<HashNode> QueryByMaxCode(Guid groupId);

        /// <summary>
        /// 查询指定节点的第一个小于指定节点Code值的节点
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        Task<HashNode> QueryFirstLessNode(Guid groupId, Guid nodeId);
        /// <summary>
        /// 查询指定节点的第一个大于指定节点Code值的节点
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        Task<HashNode> QueryFirstGreaterNode(Guid groupId, Guid nodeId);

        Task QueryByStatus(Guid groupId,int status,Func<HashNode,Task> callback);
        Task<QueryResult<HashNode>> QueryByStatus(Guid groupId, int status, int page,int pageSize);



        Task<List<HashNode>> QueryOrderByCode(Guid groupId, int skipNum, int takeNum);

    }
}
