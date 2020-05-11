using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Template.LabelParameterHandlers
{
    [Injection(InterfaceType = typeof(LabelParameterHandlerForUTCDateTimeFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForUTCDateTimeFactory : IFactory<ILabelParameterHandler>
    {
        private LabelParameterHandlerForUTCDateTime _labelParameterHandlerForUTCDateTime;

        public LabelParameterHandlerForUTCDateTimeFactory(LabelParameterHandlerForUTCDateTime labelParameterHandlerForUTCDateTime)
        {
            _labelParameterHandlerForUTCDateTime = labelParameterHandlerForUTCDateTime;
        }
        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForUTCDateTime;
        }
    }
}
