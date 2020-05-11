using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using MSLibrary.DI;
using MSLibrary.Serializer;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.DAL
{
    /// <summary>
    /// 存储信息解析服务的默认实现，
    /// 在默认实现中，info为StoreInfo的Json序列化字符串，反序列化后，组装成StoreInfoResolveServiceResult返回
    /// </summary>
    [Injection(InterfaceType = typeof(IStoreInfoResolveService), Scope = InjectionScope.Singleton)]
    public class StoreInfoResolveServiceDefault : IStoreInfoResolveService
    {
        public async Task<StoreInfo> Execute(string info)
        {

            return await Task.FromResult(ExecuteSync(info));
        }

        public StoreInfo ExecuteSync(string info)
        {
            StoreInfo storeInfo = null;
            try
            {
                storeInfo = JsonSerializerHelper.Deserialize<StoreInfo>(info);
                if (storeInfo.DBConnectionNames==null)
                {
                    storeInfo = null;
                }

            }
            catch (JsonReaderException)
            {
                storeInfo = null;
            }

            if (storeInfo == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.JsonDeserializeError,
                    DefaultFormatting = "类型{0}反序列化失败，要反序列化的字符串为{1}，发生位置{2}",
                    ReplaceParameters = new List<object>() { typeof(StoreInfo).FullName, info, "MSLibrary.DAL.StoreInfoResolveServiceDefault.Execute" }
                };

                throw new UtilityException((int)Errors.JsonDeserializeError, fragment);
            }

            return storeInfo;
        }
    }


}
