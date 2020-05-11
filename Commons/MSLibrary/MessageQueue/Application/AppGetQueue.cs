using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.MessageQueue.Application
{
    [Injection(InterfaceType = typeof(IAppGetQueue), Scope = InjectionScope.Singleton)]
    public class AppGetQueue : IAppGetQueue
    {
        private ISQueueChooseService _sQueueChooseService;

        public AppGetQueue(ISQueueChooseService sQueueChooseService)
        {
            _sQueueChooseService = sQueueChooseService;
        }
        public async Task<SQueueData> Do(string key)
        {
             var queue=await _sQueueChooseService.Choose(key);

            if (queue == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundSQueueByMessageType,
                    DefaultFormatting = "消息类型为{0}，但没有找到对应的队列",
                    ReplaceParameters = new List<object>() {key}
                };

                throw new UtilityException((int)Errors.NotFoundSQueueByMessageType, fragment);

            }

            SQueueData data = new SQueueData()
            {
                GroupName = queue.GroupName,
                ServerName = queue.ServerName,
                Name = queue.Name,
                Code = queue.Code
            };

            return data;
        }
    }
}
