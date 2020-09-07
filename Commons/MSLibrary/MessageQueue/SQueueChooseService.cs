using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.MessageQueue
{
    /// <summary>
    /// 队列选择服务
    /// 在该实现中，要求key的格式为队列组名称-消息本身Key，队列组名称为SQueue中的GroupName
    /// </summary>
    [Injection(InterfaceType = typeof(ISQueueChooseService), Scope = InjectionScope.Singleton)]
    public class SQueueChooseService : ISQueueChooseService
    {
        private SQueueRepositoryHelper _sQueueRepositoryHelper;

        public SQueueChooseService(SQueueRepositoryHelper sQueueRepositoryHelper)
        {
            _sQueueRepositoryHelper = sQueueRepositoryHelper;
        }
        public async Task<SQueue> Choose(string key)
        {
            var arrayKey= key.Split('-');
            if (arrayKey.Length<2)
            {
                throw new Exception($"key {key} is invalid in QueueChooseService's Choose,it must like QueueGroupName-MessageKey");
            }

            //获取总记录数
            var count=await _sQueueRepositoryHelper.QueryGroupCount(arrayKey[0], false);
            if (count==0)
            {
                throw new Exception($"the number of queues whose groupname is {arrayKey[0]} must greater than 0");
            }

            //计算散列值
            var keyNumber=key.ToLong();
            var code = (int)(keyNumber % count);

            //获取队列
            return await _sQueueRepositoryHelper.QueryByCode(arrayKey[0], false, code);
            
        }

        public async Task<SQueue> ChooseDead(string key)
        {
            var arrayKey = key.Split('-');
            if (arrayKey.Length < 2)
            {
                throw new Exception($"key {key} is invalid in QueueChooseService's ChooseDead,it must like QueueGroupName-MessageKey");
            }

            //获取总记录数
            var count = await _sQueueRepositoryHelper.QueryGroupCount(arrayKey[0], true);
            if (count == 0)
            {
                throw new Exception($"the number of dead queues whose groupname is {arrayKey[0]} must greater than 0");
            }

            //计算散列值
            var keyNumber = key.ToLong();
            var code = (int)(keyNumber % count);

            //获取队列
            return await _sQueueRepositoryHelper.QueryByCode(arrayKey[0], true, code);
        }
    }
}
