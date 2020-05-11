using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using MSLibrary.DI;
using MSLibrary.LanguageTranslate;
using MSLibrary.Context;
using MSLibrary.Thread;

namespace MSLibrary.MessageQueue.Application
{
    [Injection(InterfaceType = typeof(IAppExecuteQueueProcessGroup), Scope = InjectionScope.Singleton)]
    public class AppExecuteQueueProcessGroup : IAppExecuteQueueProcessGroup
    {
        /// <summary>
        /// 该应用使用的环境声明生成器名称
        /// </summary>
        public static string EnvironmentClaimGeneratorName
        {
            get;set;
        }

        /// <summary>
        /// 该应用使用的上下文生成器名称
        /// </summary>
        public static string ClaimContextGeneratorName
        {
            get; set;
        }
        /// <summary>
        /// 该应用使用的错误日志目录
        /// </summary>
        public static string ErrorCatalogName
        {
            get;set;
        }


        private ISQueueProcessGroupRepository _sQueueProcessGroupRepository;
        private IEnvironmentClaimGeneratorRepository _environmentClaimGeneratorRepository;
        private IClaimContextGeneratorRepository _claimContextGeneratorRepository;
        private ISMessageRepository _sMessageRepository;
        private ILoggerFactory _loggerFactory;

        public AppExecuteQueueProcessGroup(ISQueueProcessGroupRepository sQueueProcessGroupRepository, IEnvironmentClaimGeneratorRepository environmentClaimGeneratorRepository, IClaimContextGeneratorRepository claimContextGeneratorRepository, ISMessageRepository sMessageRepository, ILoggerFactory loggerFactory)
        {
            _sQueueProcessGroupRepository = sQueueProcessGroupRepository;
            _environmentClaimGeneratorRepository = environmentClaimGeneratorRepository;
            _claimContextGeneratorRepository = claimContextGeneratorRepository;
            _sMessageRepository = sMessageRepository;
            _loggerFactory = loggerFactory;
        }
        public async Task Do(string groupName)
        {
            var group=await _sQueueProcessGroupRepository.QueryByName(groupName);

            if (group==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundSQueueProcessGroupByName,
                    DefaultFormatting = "没有找到名称为{0}的队列执行组",
                    ReplaceParameters = new List<object>() { groupName }
                };

                throw new UtilityException((int)Errors.NotFoundSQueueProcessGroupByName, fragment);
            }

            //对组里的每个队列并行处理
            List<SQueue> queueList = new List<SQueue>();

            var environmentClaimGenerator= await _environmentClaimGeneratorRepository.QueryByName(EnvironmentClaimGeneratorName);
            var claimContextGenerator = await _claimContextGeneratorRepository.QueryByName(ClaimContextGeneratorName);

            if (environmentClaimGenerator==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundEnvironmentClaimGeneratorByName,
                    DefaultFormatting = "没有找到名称为{0}的上下文生成器",
                    ReplaceParameters = new List<object>() { EnvironmentClaimGeneratorName }
                };

                throw new UtilityException((int)Errors.NotFoundEnvironmentClaimGeneratorByName, fragment);
            }

            if (claimContextGenerator == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundClaimContextGeneratorByName,
                    DefaultFormatting = "没有找到名称为{0}的上下文生成器",
                    ReplaceParameters = new List<object>() { ClaimContextGeneratorName }
                };

                throw new UtilityException((int)Errors.NotFoundClaimContextGeneratorByName, fragment);
            }

            await group.GetAllQueue(async(queue)=>
            {
                 queueList.Add(queue);
                await Task.FromResult(0);
            });

            
            Parallel.ForEach(queueList, (queue) =>
             {
                 var fun = Task.Run(async ()=>
                 {
                     while (true)
                     {
                         try
                         {
                             //从当前环境中获取声明
                             var claims = await environmentClaimGenerator.Generate().ConfigureAwait(false);
                             ///初始化上下文
                             claimContextGenerator.ContextInit(claims.Claims);
                             //获取队列中的所有消息
                             await _sMessageRepository.QueryAllByQueue(queue, 500, async (messages) =>
                               {
                                   foreach(var message in messages)
                                   {
                                       var messageResult=await message.Execute();

                                       //需要退出查询
                                       if (messageResult.Status==0 || messageResult.Status == 3)
                                       {
                                           if (messageResult.Status==0)
                                           {
                                               //需要删除
                                               await message.Delete();
                                           }
                                           return false;
                                       }

                                       //如果结果出错，需要记录日志
                                       if (messageResult.Status == 2)
                                       {
                                           var logger = _loggerFactory.CreateLogger(ErrorCatalogName);
                                           logger.LogError($"AppExecuteQueueProcessGroup Message Execute Error,Type:{message.Type},Key:{message.Key},Data:{message.Data},error:{messageResult.Description}");
                                       }
                                   }

                                   return true;
                               });
                         }
                         catch(Exception ex)
                         {
                             //发生错误，需要记录日志
                             var logger = _loggerFactory.CreateLogger(ErrorCatalogName);
                             logger.LogError($"AppExecuteQueueProcessGroup Error,error:{ex.Message},stack:{ex.StackTrace}");
                         }

                         System.Threading.Thread.Sleep(5);                        
                     }
                 });

                 fun.Wait();

             });

        }
    }
}
