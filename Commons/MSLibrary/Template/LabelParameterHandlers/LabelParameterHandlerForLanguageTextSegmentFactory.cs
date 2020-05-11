using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Template.LabelParameterHandlers
{
    [Injection(InterfaceType = typeof(LabelParameterHandlerForLanguageTextSegmentFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForLanguageTextSegmentFactory : IFactory<ILabelParameterHandler>
    {
        private LabelParameterHandlerForLanguageTextSegment _labelParameterHandlerForLanguage;

        public LabelParameterHandlerForLanguageTextSegmentFactory(LabelParameterHandlerForLanguageTextSegment labelParameterHandlerForLanguage)
        {
            _labelParameterHandlerForLanguage = labelParameterHandlerForLanguage;
        }
        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForLanguage;
        }
    }
}
