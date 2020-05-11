using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.SRR
{
    [Injection(InterfaceType = typeof(SRRRequestHandlerDescriptionDefault), Scope = InjectionScope.Transient)]
    public class SRRRequestHandlerDescriptionDefault : ISRRRequestHandlerDescription
    {
        public IList<ISRRFilter> Filters
        {
            get;
        } = new List<ISRRFilter>();

        public IFactory<ISRRRequestHandler> HandlerFactory
        {
            get;set;
        }
    }
}
