using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Template.LabelParameterHandlers
{
    [Injection(InterfaceType = typeof(LabelParameterHandlerForToTemplateFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForToTemplateFactory : IFactory<ILabelParameterHandler>
    {
        private LabelParameterHandlerForToTemplate _labelParameterHandlerForToTemplate;

        public LabelParameterHandlerForToTemplateFactory(LabelParameterHandlerForToTemplate labelParameterHandlerForToTemplate)
        {
            _labelParameterHandlerForToTemplate = labelParameterHandlerForToTemplate;
        }
        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForToTemplate;
        }
    }
}
