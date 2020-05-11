using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Transaction.DTOperationRecordServices
{
    [Injection(InterfaceType = typeof(DTOperationRecordServiceForLocalFactory), Scope = InjectionScope.Singleton)]
    public class DTOperationRecordServiceForLocalFactory : IFactory<IDTOperationRecordService>
    {
        private DTOperationRecordServiceForLocal _dtOperationRecordServiceForLocal;

        public DTOperationRecordServiceForLocalFactory(DTOperationRecordServiceForLocal dtOperationRecordServiceForLocal)
        {
            _dtOperationRecordServiceForLocal = dtOperationRecordServiceForLocal;
        }
        public IDTOperationRecordService Create()
        {
            return _dtOperationRecordServiceForLocal;
        }
    }
}
