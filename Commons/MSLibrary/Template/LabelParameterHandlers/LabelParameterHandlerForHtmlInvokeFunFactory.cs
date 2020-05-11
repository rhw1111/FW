using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary.DI;

namespace MSLibrary.Template.LabelParameterHandlers
{
    [Injection(InterfaceType = typeof(LabelParameterHandlerForHtmlInvokeFunFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForHtmlInvokeFunFactory : IFactory<ILabelParameterHandler>
    {
        private LabelParameterHandlerForHtmlInvokeFun _labelParameterHandlerForHtmlInvokeFun;

        public LabelParameterHandlerForHtmlInvokeFunFactory(LabelParameterHandlerForHtmlInvokeFun labelParameterHandlerForHtmlInvokeFun)
        {
            _labelParameterHandlerForHtmlInvokeFun = labelParameterHandlerForHtmlInvokeFun;
        }
        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForHtmlInvokeFun;
        }
    }
}
