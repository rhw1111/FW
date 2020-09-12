using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSLibrary;
using MSLibrary.DI;
using System.Net;


namespace MSLibrary.Azure.Function
{
    public class FunctionBase
    {

        public FunctionBase(IServiceProvider serviceProvider, params object[] parameters)
        {
            ContextContainer.SetValue(ContextTypes.ServiceProvider, serviceProvider);
            FunctionInitLoad.Init(parameters);
        }

        protected async Task<IActionResult> HttpExceptionWrapper(Func<Task<object>> action,ILogger log)
        {
            try
            {
                var result = await action();
                return new OkObjectResult(result);
            }
            catch (UtilityException ex)
            {
                log.LogError(ex, string.Empty);
                IActionResult result = null;



                ErrorMessage errorMessage = new ErrorMessage()
                {
                    Type=ex.Type,
                     Level=ex.Level,
                    Code = ex.Code,
                    Message = await ex.GetCurrentLcidMessage()
                };


                if (!UtilityExceptionTypeStatusCodeMappings.Mappings.TryGetValue(ex.Type, out int statusCode))
                {
                    statusCode = 500;
                }

                result = new ObjectResult(errorMessage)
                {
                    StatusCode = statusCode
                };


                return result;
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Empty);
                var result = new ObjectResult($"{ex.Message},{ ex.ToString() }")
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };

                return result;
            }
        }

        protected async Task ExceptionWrapper(Func<Task> action, ILogger log)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                log.LogError(ex, string.Empty);
                throw;
            }
        }


    }

    public static class FunctionInitLoad
    {
        private static bool _init = false;

        private static object _lock = new object();
        public static void Init(params object[] parameters)
        {
            if (!_init)
            {
                lock (_lock)
                {
                    if (!_init)
                    {
                        var method = FunctionBaseHelper.FunctionStartupType.GetMethod("Execute");
                        method.Invoke(null, parameters);
                        _init = true;
                    }
                }
            }
        }

    }

    public static class FunctionBaseHelper
    {
        public static Type FunctionStartupType { get; set; }
    }
}
