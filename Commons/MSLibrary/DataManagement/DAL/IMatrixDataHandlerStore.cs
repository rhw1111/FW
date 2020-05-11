using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.DataManagement.DAL
{
    public interface IMatrixDataHandlerStore
    {
        Task<MatrixDataHandler> QueryByName(string name);
    }
}
