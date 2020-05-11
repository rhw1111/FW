using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.DataManagement
{
    /// <summary>
    /// 矩阵数据处理方仓储
    /// </summary>
    public interface IMatrixDataHandlerRepository
    {
        Task<MatrixDataHandler> QueryByName(string name);
    }
}
