using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.DataManagement.DAL;

namespace MSLibrary.DataManagement
{
    [Injection(InterfaceType = typeof(IMatrixDataHandlerRepository), Scope = InjectionScope.Singleton)]
    public class MatrixDataHandlerRepository:IMatrixDataHandlerRepository
    {
        private IMatrixDataHandlerStore _matrixDataHandlerStore;

        public MatrixDataHandlerRepository(IMatrixDataHandlerStore matrixDataHandlerStore)
        {
            _matrixDataHandlerStore = matrixDataHandlerStore;
        }
        public async Task<MatrixDataHandler> QueryByName(string name)
        {
            return await _matrixDataHandlerStore.QueryByName(name);
        }
    }
}
