using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Collections.Hash.DAL
{
    /// <summary>
    /// 哈希真实节点数据操作
    /// </summary>
    public interface IHashRealNodeStore
    {
        Task Add(HashRealNode node);
        Task Update(Guid groupId,HashRealNode node);

        Task DeleteByRelation(Guid groupId, Guid id);

        Task<HashRealNode> QueryByGroup(Guid groupId, Guid nodeId);

        Task<QueryResult<HashRealNode>> QueryByGroup(Guid groupId, int page, int pageSize);

        Task QueryByAll(Guid groupId, Func<HashRealNode, Task> callback);

        /// <summary>
        /// 查询指定的哈希组下面跳过指定数量后获取指定数量的真实节点（按createtime排序）
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="skipNum"></param>
        /// <param name="takeNum"></param>
        /// <returns></returns>
        Task<List<HashRealNode>> QueryHashRealNodeByCreateTime(Guid groupId, int skipNum, int takeNum);
    }
}
