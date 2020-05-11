using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.ExpressionCalculate
{
    /// <summary>
    /// 公式计算服务
    /// </summary>
    public interface IFormulaCalculateService
    {
        Task<bool> IsIndividual();
        Task<object> ParameterConvert(int index, string value);
        Task<object> Calculate(IList<object> parameters,object executeContext);
    }
}
