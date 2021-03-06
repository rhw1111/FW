﻿using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Collections;

namespace FW.TestPlatform.Main.Collections.TreeEntityValueServices
{
    [Injection(InterfaceType = typeof(TreeEntityValueServiceForTestDataSourceFactory), Scope = InjectionScope.Singleton)]
    public class TreeEntityValueServiceForTestDataSourceFactory : IFactory<ITreeEntityValueService>
    {
        private readonly TreeEntityValueServiceForTestDataSource _treeEntityValueService;

        public TreeEntityValueServiceForTestDataSourceFactory(TreeEntityValueServiceForTestDataSource treeEntityValueService)
        {
            _treeEntityValueService = treeEntityValueService;
        }
        public ITreeEntityValueService Create()
        {
            return _treeEntityValueService;
        }
    }
}
