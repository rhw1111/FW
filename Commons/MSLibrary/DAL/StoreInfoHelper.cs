using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.Collections.Hash;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.DAL
{

    public static class StoreInfoHelper
    {

        public async static Task<StoreInfo> GetHashStoreInfo(IStoreInfoResolveService storeInfoResolveService,IHashGroupRepositoryCacheProxy hashGroupRepositoryCacheProxy, string hashGroupName, params string[] keys)
        {
            //var helper = HashGroupRepositoryHelperFactory.Create(hashGroupRepository);
            var group = await hashGroupRepositoryCacheProxy.QueryByName(hashGroupName);
            if (group == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundHashGroupByName,
                    DefaultFormatting = "没有找到名称为{0}的一致性哈希组",
                    ReplaceParameters = new List<object>() { hashGroupName }
                };

                throw new UtilityException((int)Errors.NotFoundHashGroupByName,fragment);
            }

            //获取前缀的哈希节点关键字,
            var strKey = await group.GetHashNodeKey(string.Join(".", keys), 1, 2, 3);
            //解析关键字为服务器信息和数据表信息
            var storeResult = await storeInfoResolveService.Execute(strKey);

            return storeResult;

        }




        public static StoreInfo GetHashStoreInfoSync(IStoreInfoResolveService storeInfoResolveService, IHashGroupRepositoryCacheProxy hashGroupRepositoryCacheProxy, string hashGroupName, params string[] keys)
        {

            
            var group = hashGroupRepositoryCacheProxy.QueryByNameSync(hashGroupName);
            if (group == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundHashGroupByName,
                    DefaultFormatting = "没有找到名称为{0}的一致性哈希组",
                    ReplaceParameters = new List<object>() { hashGroupName }
                };

                throw new UtilityException((int)Errors.NotFoundHashGroupByName, fragment);
            }

            //获取前缀的哈希节点关键字,
            var strKey = group.GetHashNodeKeySync(string.Join(".", keys), 1, 2, 3);

            //解析关键字为服务器信息和数据表信息
            var storeResult = storeInfoResolveService.ExecuteSync(strKey);

            return storeResult;
        }




        public async static Task<List<StoreInfo>> GetHashStoreInfos(IStoreInfoResolveService storeInfoResolveService, IHashGroupRepositoryCacheProxy hashGroupRepositoryCacheProxy, string hashGroupName)
        {
            

            List<StoreInfo> result = new List<StoreInfo>();
            var group = await hashGroupRepositoryCacheProxy.QueryByName(hashGroupName);
            if (group == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundHashGroupByName,
                    DefaultFormatting = "没有找到名称为{0}的一致性哈希组",
                    ReplaceParameters = new List<object>() { hashGroupName }
                };

                throw new UtilityException((int)Errors.NotFoundHashGroupByName, fragment);
            }

            //获取组下面的所有节点
            await group.GetHashRealNode(async (node) =>
            {
                var strKey = node.NodeKey;
                //解析关键字为服务器信息和数据表信息
                var storeResult = await storeInfoResolveService.Execute(strKey);
                result.Add(storeResult);
            });
            return result;

        }




        public async static Task<StoreInfo> GetHashStoreInfo(HashGroup group, IStoreInfoResolveService storeInfoResolveService, params string[] keys)
        {

            //获取前缀的哈希节点关键字,
            var strKey = await group.GetHashNodeKey(string.Join(".", keys), 1, 2, 3);
            //解析关键字为服务器信息和数据表信息
            var storeResult = await storeInfoResolveService.Execute(strKey);

            return storeResult;

        }




        public static StoreInfo GetHashStoreInfoSync(HashGroup group, IStoreInfoResolveService storeInfoResolveService,  params string[] keys)
        {

            //获取前缀的哈希节点关键字,
            var strKey = group.GetHashNodeKeySync(string.Join(".", keys), 1, 2, 3);

            //解析关键字为服务器信息和数据表信息
            var storeResult = storeInfoResolveService.ExecuteSync(strKey);

            return storeResult;
        }




        public async static Task<List<StoreInfo>> GetHashStoreInfos(HashGroup group, IStoreInfoResolveService storeInfoResolveService)
        {

            List<StoreInfo> result = new List<StoreInfo>();

            //获取组下面的所有节点
            await group.GetHashRealNode(async (node) =>
            {
                var strKey = node.NodeKey;
                //解析关键字为服务器信息和数据表信息
                var storeResult = await storeInfoResolveService.Execute(strKey);
                result.Add(storeResult);
            });
            return result;

        }





    }
}
