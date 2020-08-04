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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForTimeFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForTimeFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForTime _labelParameterHandlerForTime;

        public LabelParameterHandlerForTimeFactory(LabelParameterHandlerForTime labelParameterHandlerForTime)
        {
            _labelParameterHandlerForTime = labelParameterHandlerForTime;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForTime;
        }
    }
}
