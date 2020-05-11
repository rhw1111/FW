using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;

namespace MSLibrary.OData.ClientInitializations
{
    [Injection(InterfaceType = typeof(ODataClientInitializationForAADSTSFactory), Scope = InjectionScope.Singleton)]
    public class ODataClientInitializationForAADSTSFactory : IFactory<IODataClientInitialization>
    {
        private ODataClientInitializationForAADSTS _oDataClientInitializationForAADSTS;

        public ODataClientInitializationForAADSTSFactory(ODataClientInitializationForAADSTS oDataClientInitializationForAADSTS)
        {
            _oDataClientInitializationForAADSTS = oDataClientInitializationForAADSTS;
        }
        public IODataClientInitialization Create()
        {
            return _oDataClientInitializationForAADSTS;
        }
    }
}
