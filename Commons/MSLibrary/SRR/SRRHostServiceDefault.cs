using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.SRR
{
    [Injection(InterfaceType = typeof(ISRRHostService), Scope = InjectionScope.Transient)]
    public class SRRHostServiceDefault : ISRRHostService
    {
        private Dictionary<string, List<ISRRFilter>> _typeFiltes = new Dictionary<string, List<ISRRFilter>>();

        private ISRRHostConfiguration _configuration;
        public void Configure(ISRRHostConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<SRRResponse> Handle(string sourceType, IDictionary<string, object> sourceParameters, SRRRequest request)
        {
            SRRPipeContext pipeContext = new SRRPipeContext();
            pipeContext.RequestSourceType = sourceType;
            foreach (var parameterItem in sourceParameters)
            {
                pipeContext.RequestSourceParameters[parameterItem.Key] = parameterItem.Value;
            }

            //首先找到对应源类型的middleware列表，执行middleware
            if (_configuration.Middlewares.TryGetValue(sourceType, out IList<ISRRMiddleware> middlewareList))
            {
                foreach (var item in middlewareList)
                {
                    await item.Invoke(pipeContext);
                }
            }

            //再找到对应request类型的request描述
            var description = _configuration.GetHandlerDescription(request.Type);

            if (description==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundSRRRequestHandlerDescriptionByType,
                    DefaultFormatting = "找不到请求类型为{0}的请求处理描述，发生位置：{1}",
                    ReplaceParameters = new List<object>() { request.Type, $"{this.GetType().FullName}.Handle" }
                };

                throw new UtilityException((int)Errors.NotFoundSRRRequestHandlerDescriptionByType, fragment);
            }


            //组合GlobalFilter和request描述中的Filter
            List<ISRRFilter> filters = new List<ISRRFilter>();

            if (!_typeFiltes.TryGetValue(request.Type,out List<ISRRFilter> catchFilters))
            {
                filters.AddRange(_configuration.GlobalFilters);
                filters.AddRange(description.Filters);


                var index = filters.FindLastIndex((item) =>
                { return item is ISRROverrideFilter; });

                if (index != -1)
                {
                    filters.RemoveRange(0, index);
                }

                catchFilters = new List<ISRRFilter>();
                catchFilters.AddRange(filters);
                _typeFiltes[request.Type] = catchFilters;
            }
            else
            {
                filters.AddRange(catchFilters);
            }




            SRRFilterContext filterContext = new SRRFilterContext(request);
            filterContext.Identity = pipeContext.Identity;
            foreach(var item in pipeContext.Items)
            {
                filterContext.Items.Add(item);
            }

            SRRResponse response = null;

            try
            {
                //执行filter的pre操作
                foreach (var item in filters)
                {
                    item.ExecutePreSync(filterContext);
                    await item.ExecutePre(filterContext);
                }

                response = await description.HandlerFactory.Create().Execute(request);
                filterContext.Response = response;

                //执行filter的post操作
                filters.Reverse();
                foreach (var item in filters)
                {
                    item.ExecutePostSync(filterContext);
                    await item.ExecutePost(filterContext);
                }
            }
            finally
            {
                List<Exception> exs = new List<Exception>();
                foreach (var item in filters)
                {
                    try
                    {
                        item.Finally(filterContext);
                        await item.Finally(filterContext);
                    }
                    catch(Exception ex)
                    {
                        exs.Add(ex);
                    }
                }

                if (exs.Count>0)
                {
                    throw exs[0];
                }
            }


            return response;
        }
    }
}
