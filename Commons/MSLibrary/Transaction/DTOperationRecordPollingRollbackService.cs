using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.DI;
using MSLibrary.Storge;
using MSLibrary.LanguageTranslate;
using MSLibrary.Thread;
using MSLibrary.Collections;
using MSLibrary.Logger;

namespace MSLibrary.Transaction
{
    [Injection(InterfaceType = typeof(IDTOperationRecordPollingRollbackService), Scope = InjectionScope.Singleton)]
    public class DTOperationRecordPollingRollbackService : IDTOperationRecordPollingRollbackService
    {
        public static string ErrorLoggerCategoryName { get; set; }
                       
        private IStoreGroupRepositoryCacheProxy _storeGroupRepositoryCacheProxy;
        private IDTOperationRecordRepository _dtOperationRecordRepository;
        public DTOperationRecordPollingRollbackService(IStoreGroupRepositoryCacheProxy storeGroupRepositoryCacheProxy, IDTOperationRecordRepository dtOperationRecordRepository)
        {
            _storeGroupRepositoryCacheProxy = storeGroupRepositoryCacheProxy;
            _dtOperationRecordRepository = dtOperationRecordRepository;
        }
        public async Task<IDTOperationRecordPollingRollbackController> Execute(string storeGroupName, string memberName)
        {
            //获取存储组
            var storeGroup=await _storeGroupRepositoryCacheProxy.QueryByName(storeGroupName);
            if (storeGroup == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFounStoreGroupMemberByName,
                    DefaultFormatting = "找不到名称为{0}的存储组",
                    ReplaceParameters = new List<object>() { storeGroupName }
                };

                throw new UtilityException((int)Errors.NotFounStoreGroupByName, fragment);
            }
            //获取指定的组成员
            var groupMember = await storeGroup.GetMember(memberName);
           

            if (groupMember==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundStoreGroupMemberInGroup,
                    DefaultFormatting = "在名称为{0}的存储组下找不到名称为{1}的成员",
                    ReplaceParameters = new List<object>() { storeGroupName, memberName }
                };

                throw new UtilityException((int)Errors.NotFoundStoreGroupMemberInGroup, fragment);
            }

            var pollingResult=await PollingHelper.Polling<DTOperationRecord>(
                async()=>
                {
                    await Task.CompletedTask;
                    return
                    new AsyncInteration<DTOperationRecord>
                    (
                        async (index) =>
                {
                    if (index == 0)
                    {

                        while (true)
                        {

                            var completeList = await _dtOperationRecordRepository.QueryBySkip(groupMember.StoreInfo, (int)DTOperationRecordStatus.Complete, 0, 500);

                            await ParallelHelper.ForEach(completeList, 5,

                                async (record) =>
                                {
                                    await record.Delete();
                                }
                                );


                            if (completeList.Count < 500)
                            {
                                break;
                            }
                        }
                    }

                    var datas = await _dtOperationRecordRepository.QueryBySkip(groupMember.StoreInfo, (int)DTOperationRecordStatus.UnComplete, index * 500, 500);
                    return datas;

                }
                    );
                },5,500,
                async(record)=>
                {
                    try
                    {

                        using (var diContainer = DIContainerContainer.CreateContainer())
                        {
                            var orginialDI = ContextContainer.GetValue<IDIContainer>("DI");
                            try
                            {
                                ContextContainer.SetValue<IDIContainer>("DI", diContainer);

                                if (await record.NeedCancel())
                                {
                                    await record.Cancel();
                                }
                            }
                            finally
                            {
                                ContextContainer.SetValue<IDIContainer>("DI", orginialDI);
                            }
                        }




                    }
                    catch(Exception ex)
                    {
                        Exception rootEx = ex;
                        while(ex.InnerException!=null)
                        {
                            ex = ex.InnerException;
                        }
                        if (ex != rootEx)
                        {
                            await record.UpdateErrorMessage($"root message:{rootEx.Message},inner message:{ex.Message},root Stack:{rootEx.StackTrace},inner Stack:{ex.StackTrace}");
                        }
                        else
                        {
                            await record.UpdateErrorMessage($"message:{rootEx.Message},Stack:{rootEx.StackTrace}");
                        }
                    }
                },
                null,
                async(ex)=>
                {
                    LoggerHelper.LogError(ErrorLoggerCategoryName,$"DTOperationRecordPollingRollbackService Execute Error,ErrorMessage:{ex.Message},StackTrace:{ex.StackTrace}");
                }
                );

            DTOperationRecordPollingRollbackControllerDefault controller = new DTOperationRecordPollingRollbackControllerDefault(pollingResult);
            return controller;
        }
    }

    public class DTOperationRecordPollingRollbackControllerDefault : IDTOperationRecordPollingRollbackController
    {
        private IAsyncPollingResult _asyncPollingResult;
        public DTOperationRecordPollingRollbackControllerDefault(IAsyncPollingResult asyncPollingResult)
        {
            _asyncPollingResult = asyncPollingResult;
        }
        public async Task Stop()
        {
            await _asyncPollingResult.Stop();
        }
    }
}
