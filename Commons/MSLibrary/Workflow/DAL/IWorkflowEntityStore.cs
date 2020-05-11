using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MSLibrary.Workflow.DAL
{
    /// <summary>
    /// 审核实体数据操作
    /// </summary>
    /// 
    public interface IWorkflowEntityStore
    {
        /// <summary>
        /// 查询审核实体的字段信息
        /// </summary>
        /// <param name="entityName">审核实体名称</param>
        /// <param name="entityColumnName">审核实体字段名称</param>
        /// <param name="entityKey">审核实体主键</param>
        /// <returns>审核实体的字段信息</returns>
        Task<string> QueryAuditStatusByKey(string entityName, string entityColumnName, Dictionary<string, string> entityKey);
    }
}
