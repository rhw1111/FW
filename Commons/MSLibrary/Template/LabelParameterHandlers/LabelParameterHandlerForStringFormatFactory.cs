using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Template.LabelParameterHandlers
{
    [Injection(InterfaceType = typeof(LabelParameterHandlerForStringFormatFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForStringFormatFactory : IFactory<ILabelParameterHandler>
    {
        private LabelParameterHandlerForStringFormat _labelParameterHandlerForStringFormat;
        public LabelParameterHandlerForStringFormatFactory(LabelParameterHandlerForStringFormat labelParameterHandlerForStringFormat)
        {
            _labelParameterHandlerForStringFormat = labelParameterHandlerForStringFormat;
        }
        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForStringFormat;
        }
    }
}
