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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForHttpPostWithConnectInvokeFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForHttpPostWithConnectInvokeFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForHttpPostWithConnectInvoke _labelParameterHandlerForHttpPostWithConnectInvoke;

        public LabelParameterHandlerForHttpPostWithConnectInvokeFactory(LabelParameterHandlerForHttpPostWithConnectInvoke labelParameterHandlerForHttpPostWithConnectInvoke)
        {
            _labelParameterHandlerForHttpPostWithConnectInvoke = labelParameterHandlerForHttpPostWithConnectInvoke;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForHttpPostWithConnectInvoke;
        }
    }
}
