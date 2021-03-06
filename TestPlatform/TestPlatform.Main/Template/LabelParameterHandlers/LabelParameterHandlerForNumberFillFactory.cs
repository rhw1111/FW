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
    [Injection(InterfaceType = typeof(LabelParameterHandlerForNumberFillFactory), Scope = InjectionScope.Singleton)]
    public class LabelParameterHandlerForNumberFillFactory : IFactory<ILabelParameterHandler>
    {
        private readonly LabelParameterHandlerForNumberFill _labelParameterHandlerForNumberFill;

        public LabelParameterHandlerForNumberFillFactory(LabelParameterHandlerForNumberFill labelParameterHandlerForNumberFill)
        {
            _labelParameterHandlerForNumberFill = labelParameterHandlerForNumberFill;
        }

        public ILabelParameterHandler Create()
        {
            return _labelParameterHandlerForNumberFill;
        }
    }
}
