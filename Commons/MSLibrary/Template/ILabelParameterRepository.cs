using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Template
{
    /// <summary>
    /// 标签参数仓储
    /// </summary>
    public interface ILabelParameterRepository
    {
        /// <summary>
        /// 根据名称查询标签参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<LabelParameter> QueryByName(string name);
    }
}
