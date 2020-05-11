using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MSLibrary.DI;

namespace MSLibrary.Security.RequestController.Application
{

    [Injection(InterfaceType = typeof(IAppRequestControl), Scope = InjectionScope.Singleton)]
    public class AppRequestControl : IAppRequestControl
    {
        private IRequestTrackerRepository _requestTrackerRepository;
        private ILoggerFactory _loggerFactory;
        private static string _errorCategoryName;

        public static string ErrorCategoryName
        {
            set
            {
                _errorCategoryName = value;
            }
        }

        public AppRequestControl(IRequestTrackerRepository requestTrackerRepository,ILoggerFactory loggerFactory)
        {
            _requestTrackerRepository = requestTrackerRepository;
            _loggerFactory = loggerFactory;
        }
        public async Task<IRequestControlResult> Do(string actionName)
        {
            //获取全局请求跟踪
            var globalTracker = await _requestTrackerRepository.QueryGlobal();
            //获取针对Action的请求跟踪
            var tracker = await _requestTrackerRepository.QueryByRequestKey(actionName);

            RequestControlResult result = new RequestControlResult(globalTracker, tracker,_loggerFactory,_errorCategoryName);
            return result;
        }
    }
}
