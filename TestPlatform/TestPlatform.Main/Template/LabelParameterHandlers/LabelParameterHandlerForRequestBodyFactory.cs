﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime;
using Newtonsoft.Json.Linq;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Template;

namespace FW.TestPlatform.Main.Template.LabelParameterHandlers
{
    [Injection(InterfaceType = typeof(LabelParameterHandlerForRequestBodyFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForRequestBodyFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForRequestBody _labelParameterHandlerForRequestBody;

        public LabelParameterHandlerForRequestBodyFactory(LabelParameterHandlerForRequestBody labelParameterHandlerForRequestBody)
        {
            _labelParameterHandlerForRequestBody = labelParameterHandlerForRequestBody;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForRequestBody;
        }
    }
}
