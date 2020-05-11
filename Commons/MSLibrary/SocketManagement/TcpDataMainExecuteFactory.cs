using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.SocketManagement
{
    [Injection(InterfaceType = typeof(TcpDataMainExecuteFactory), Scope = InjectionScope.Singleton)]
    public class TcpDataMainExecuteFactory : IFactory<ITcpDataExecute>
    {
        private TcpDataMainExecute _tcpDataMainExecute;

        public TcpDataMainExecuteFactory(TcpDataMainExecute tcpDataMainExecute)
        {
            _tcpDataMainExecute = tcpDataMainExecute;
        }
        public ITcpDataExecute Create()
        {
            return _tcpDataMainExecute;
        }
    }
}
