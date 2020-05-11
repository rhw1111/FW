using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MSLibrary.Security.RequestController.Application
{
    /// <summary>
    /// 应用层处理请求控制
    /// </summary>
    public interface IAppRequestControl
    {
        Task<IRequestControlResult> Do(string actionName);
    }

    public interface IRequestControlResult : IDisposable
    {
        Task<ValidateResult> Execute();
    }

    public class RequestControlResult : IRequestControlResult
    {
        private RequestTracker _globalTracker;
        private RequestTracker _tracker;
        private ILoggerFactory _loggerFactory;
        private string _errorCategoryName;
        public RequestControlResult(RequestTracker globalTracker, RequestTracker tracker, ILoggerFactory loggerFactory, string errorCategoryName)
        {
            _globalTracker = globalTracker;
            _tracker = tracker;
            _loggerFactory = loggerFactory;
            _errorCategoryName = errorCategoryName;
        }
        public async void Dispose()
        {
            try
            {
                await _globalTracker.Exit();
            }
            catch (Exception ex)
            {
                var log=_loggerFactory.CreateLogger(_errorCategoryName);
                log.LogError(string.Format("execute AppRequestControl error, message:{0},stack trace:{1}", ex.Message, ex.StackTrace));
            }

            if (_tracker != null)
            {
                try
                {
                    await _tracker.Exit();
                }
                catch (Exception ex)
                {
                    var log = _loggerFactory.CreateLogger(_errorCategoryName);
                    log.LogError(string.Format("execute AppRequestControl error, message:{0},stack trace:{1}", ex.Message, ex.StackTrace));
                }
            }

        }

        public async Task<ValidateResult> Execute()
        {
            ValidateResult result = new ValidateResult()
            {
                Result = true
            };

            if (_globalTracker != null)
            {
                result = await _globalTracker.Access();

                if (!result.Result)
                {
                    return result;
                }
            }

            if (_tracker != null)
            {
                result = await _tracker.Access();
            }

            return result;
        }
    }

}
