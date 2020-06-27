﻿using MSLibrary.DI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FW.TestPlatform.Main.Code.GenerateAdditionFuncServices
{
    /// <summary>
    /// 针对Locust的附件函数生成服务
    /// </summary>
    [Injection(InterfaceType = typeof(GenerateAdditionFuncServiceForLocustDesSecurity), Scope = InjectionScope.Singleton)]
    public class GenerateAdditionFuncServiceForLocustDesSecurity : IGenerateAdditionFuncService
    {
        public Task<string> Generate()
        {
            throw new NotImplementedException();
        }
    }
}