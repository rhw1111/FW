using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.SRR
{
    public abstract class SRRRequestHandlerBase : ISRRRequestHandler
    {
        public async Task<SRRResponse> Execute(SRRRequest request)
        {
            var requireType = await GetRequireType();
   
            if (request.GetType()!= requireType)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.MiddleDataTypeNotMatchHandler,
                    DefaultFormatting = "实际类型{0}与中间数据处理器{1}要求的请求类型{2}不匹配",
                    ReplaceParameters = new List<object>() { request.GetType().FullName, this.GetType().FullName, requireType.FullName }
                };
                throw new UtilityException((int)Errors.MiddleDataTypeNotMatchHandler, fragment);
            }

            return await InnerExecute(request);
        }

        protected abstract Task<SRRResponse> InnerExecute(SRRRequest request);

        protected abstract Task<Type> GetRequireType();
    }
}
