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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForTcpRRWithConnectInvokeFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForTcpRRWithConnectInvokeFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForTcpRRWithConnectInvoke _labelParameterHandlerForTcpRRWithConnectInvoke;

        public LabelParameterHandlerForTcpRRWithConnectInvokeFactory(LabelParameterHandlerForTcpRRWithConnectInvoke labelParameterHandlerForTcpRRWithConnectInvoke)
        {
            _labelParameterHandlerForTcpRRWithConnectInvoke = labelParameterHandlerForTcpRRWithConnectInvoke;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForTcpRRWithConnectInvoke;
        }
    }
}
