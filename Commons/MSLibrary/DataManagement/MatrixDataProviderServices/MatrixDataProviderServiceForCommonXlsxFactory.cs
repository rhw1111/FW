using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;

namespace MSLibrary.DataManagement.MatrixDataProviderServices
{
    [Injection(InterfaceType = typeof(MatrixDataProviderServiceForCommonXlsxFactory), Scope = InjectionScope.Singleton)]
    public class MatrixDataProviderServiceForCommonXlsxFactory:IFactory<IMatrixDataProviderService>
    {
        private MatrixDataProviderServiceForCommonXlsx _matrixDataProviderServiceForCommonXlsx;
        public MatrixDataProviderServiceForCommonXlsxFactory(MatrixDataProviderServiceForCommonXlsx matrixDataProviderServiceForCommonXlsx)
        {
            _matrixDataProviderServiceForCommonXlsx = matrixDataProviderServiceForCommonXlsx;
        }
        public IMatrixDataProviderService Create()
        {
            return _matrixDataProviderServiceForCommonXlsx;
        }
    }
}
