using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;

namespace MSLibrary.SocketManagement
{
    [Injection(InterfaceType = typeof(TcpClientDataMainExecuteFactory), Scope = InjectionScope.Singleton)]
    public class TcpClientDataMainExecuteFactory : IFactory<ITcpClientDataExecute>
    {
        private TcpClientDataMainExecute _tcpClientDataMainExecute;

        public TcpClientDataMainExecuteFactory(TcpClientDataMainExecute tcpClientDataMainExecute)
        {
            _tcpClientDataMainExecute = tcpClientDataMainExecute;
        }

        public ITcpClientDataExecute Create()
        {
            return _tcpClientDataMainExecute;
        }
    }
}
