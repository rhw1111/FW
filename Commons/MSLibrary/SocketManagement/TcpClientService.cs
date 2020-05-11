using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using MSLibrary.LanguageTranslate;
using MSLibrary.DI;

namespace MSLibrary.SocketManagement
{
    [Injection(InterfaceType = typeof(ITcpClientService), Scope = InjectionScope.Singleton)]
    public class TcpClientService : ITcpClientService
    {
        private ITcpClientEndpointRepository _tcpClientEndpointRepository;

        private static Dictionary<string, TcpClientEndpoint> _endpointList = new Dictionary<string, TcpClientEndpoint>();

        public TcpClientService(ITcpClientEndpointRepository tcpClientEndpointRepository)
        {
            _tcpClientEndpointRepository = tcpClientEndpointRepository;
        }

        public async Task<byte[]> Send(string endpointName, byte[] data)
        {
            if (!_endpointList.TryGetValue(endpointName,out TcpClientEndpoint endpoint))
            {
                var newEndpoint = await _tcpClientEndpointRepository.QueryByName(endpointName);

                lock (_endpointList)
                {
                    if (!_endpointList.TryGetValue(endpointName, out endpoint))
                    {
                        var fragment = new TextFragment()
                        {
                            Code = TextCodes.NotFoundTcpClientEndpointByName,
                            DefaultFormatting = "没有找到名称为{0}的Tcp客户端终结点",
                            ReplaceParameters = new List<object>() { endpointName }
                        };

                        endpoint =newEndpoint?? throw new UtilityException((int)Errors.NotFoundTcpClientEndpointByName,fragment);
                        _endpointList.Add(endpointName, endpoint);
                    }
                }
            }

            return await endpoint.Send(data);
        }

        public async Task Dispose()
        {
            var endpointList=_endpointList.Values.ToList();
            foreach(var item in endpointList)
            {
                await item.Dispose();
            }
        }
    }
}
