using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Template.LabelParameterHandlers
{
    [Injection(InterfaceType = typeof(LabelParameterHandlerForHtmlAFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForHtmlAFactory : IFactory<ILabelParameterHandler>
    {
        private LabelParameterHandlerForHtmlA _labelParameterHandlerForHtmlA;

        public LabelParameterHandlerForHtmlAFactory(LabelParameterHandlerForHtmlA labelParameterHandlerForHtmlA)
        {
            _labelParameterHandlerForHtmlA = labelParameterHandlerForHtmlA;
        }
        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForHtmlA;
        }
    }
}
