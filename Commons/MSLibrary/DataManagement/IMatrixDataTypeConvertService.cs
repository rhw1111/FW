using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.DataManagement
{
    /// <summary>
    /// 矩阵数据类型转换服务
    /// </summary>
    public interface IMatrixDataTypeConvertService
    {
        Task<object> Convert(string type,object value);
    }
}
