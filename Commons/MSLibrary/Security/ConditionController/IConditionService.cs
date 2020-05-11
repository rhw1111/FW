using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MSLibrary.Security.ConditionController
{
    /// <summary>
    /// 逻辑条件服务
    /// 用于解析XML条件组合，返回验证结果
    /// </summary>
    public interface IConditionService
    {
        /// <summary>
        /// 验证逻辑条件是否满足
        /// </summary>
        /// <param name="securityElement">逻辑条件</param>
        /// <param name="parameters">辅助参数</param>
        /// <returns></returns>
        Task<ValidateResult> Validate(XDocument securityDoc, Dictionary<string, object> parameters);
    }
}
