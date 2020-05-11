using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection.Repositories;
using MSLibrary.DI;

namespace MSLibrary.AspNet
{
    /// <summary>
    /// 数据保护的XML文件仓储
    /// 读取指定的数据保护XML文件
    /// </summary>
    public class DataProtectionXmlRepository : IXmlRepository
    {
        private string _filePath;

        public DataProtectionXmlRepository(string filePath)
        {
            _filePath = filePath;
        }
        public virtual IReadOnlyCollection<XElement> GetAllElements()
        {
            return GetAllElementsCore().ToList().AsReadOnly();
        }

        private IEnumerable<XElement> GetAllElementsCore()
        {
            yield return XElement.Load(_filePath);
        }
        public virtual void StoreElement(XElement element, string friendlyName)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            StoreElementCore(element, friendlyName);
        }

        private void StoreElementCore(XElement element, string filename)
        {
        }
    }
}
