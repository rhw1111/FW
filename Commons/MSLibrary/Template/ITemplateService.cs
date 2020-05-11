using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Template
{
    /// <summary>
    /// 模板服务接口
    /// 负责将参数化文本转换成实际文本
    /// </summary>
    public interface ITemplateService
    {
        /// <summary>
        /// 模板转换
        /// </summary>
        /// <param name="content">模板内容</param>
        /// <param name="parameters">上下文参数</param>
        /// <returns></returns>
        Task<string> Convert(string content, TemplateContext context);
    }


    /// <summary>
    /// 模板上下文
    /// </summary>
    public class TemplateContext
    {
        private int _lcid;
        private Dictionary<string, object> _parameters;

        public TemplateContext(int lcid, Dictionary<string, object> parameters)
        {
            _lcid = lcid;
            _parameters = parameters;
        }

        /// <summary>
        /// 语言编号
        /// </summary>
        public int Lcid
        {
            get
            {
                return _lcid;
            }
        }

        /// <summary>
        /// 上下文参数集
        /// </summary>
        public Dictionary<string, object> Parameters
        {
            get
            {
                return _parameters;
            }
        }
    }
}
