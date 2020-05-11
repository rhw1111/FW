using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Template.LabelParameterHandlers
{
    [Injection(InterfaceType = typeof(LabelParameterHandlerForDateTimeFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForDateTimeFactory : IFactory<ILabelParameterHandler>
    {
        private LabelParameterHandlerForDateTime _labelParameterHandlerForDateTime;

        public LabelParameterHandlerForDateTimeFactory(LabelParameterHandlerForDateTime labelParameterHandlerForDateTime)
        {
            _labelParameterHandlerForDateTime = labelParameterHandlerForDateTime;
        }
        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForDateTime;
        }
    }
}
