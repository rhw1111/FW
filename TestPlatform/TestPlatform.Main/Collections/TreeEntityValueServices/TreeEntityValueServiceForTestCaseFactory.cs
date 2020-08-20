using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;
using MSLibrary.Collections;

namespace FW.TestPlatform.Main.Collections.TreeEntityValueServices
{
    [Injection(InterfaceType = typeof(TreeEntityValueServiceForTestCaseFactory), Scope = InjectionScope.Singleton)]
    public class TreeEntityValueServiceForTestCaseFactory : IFactory<ITreeEntityValueService>
    {
        private readonly TreeEntityValueServiceForTestCase _treeEntityValueServicesForTestCase;

        public TreeEntityValueServiceForTestCaseFactory(TreeEntityValueServiceForTestCase treeEntityValueServicesForTestCase)
        {
            _treeEntityValueServicesForTestCase = treeEntityValueServicesForTestCase;
        }
        public ITreeEntityValueService Create()
        {
            return _treeEntityValueServicesForTestCase;
        }
    }
}
