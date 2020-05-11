using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary
{
    /// <summary>
    /// Guid类型的扩展方法
    /// </summary>
    public static class GuidExtension
    {
        public static long ToLong(this Guid id)
        {
            return BitConverter.ToInt64(id.ToByteArray(), 0);
        }

    }
}
