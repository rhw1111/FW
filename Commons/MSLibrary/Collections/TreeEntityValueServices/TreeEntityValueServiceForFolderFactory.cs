using System;
using System.Collections.Generic;
using System.Text;
using MSLibrary;
using MSLibrary.DI;

namespace MSLibrary.Collections.TreeEntityValueServices
{
    [Injection(InterfaceType = typeof(TreeEntityValueServiceForFolderFactory), Scope = InjectionScope.Singleton)]
    public class TreeEntityValueServiceForFolderFactory : IFactory<ITreeEntityValueService>
    {
        private readonly TreeEntityValueServiceForFolder _treeEntityValueServiceForFolder;

        public TreeEntityValueServiceForFolderFactory(TreeEntityValueServiceForFolder treeEntityValueServiceForFolder)
        {
            _treeEntityValueServiceForFolder = treeEntityValueServiceForFolder;
        }
        public ITreeEntityValueService Create()
        {
            return _treeEntityValueServiceForFolder;
        }
    }
}
