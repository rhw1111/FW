using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MSLibrary.Collections.Hash.DAL;
using MSLibrary.LanguageTranslate;
using MSLibrary.DI;

namespace MSLibrary.Collections.Hash.HashGroupStrategyServices
{
    /// <summary>
    /// 取模策略服务
    /// 用key与group的Count取模后获得的数字来查找code大于该值的第一个Node
    /// </summary>
    [Injection(InterfaceType = typeof(HashGroupStrategyForMod), Scope = InjectionScope.Singleton)]
    public class HashGroupStrategyForMod : IHashGroupStrategyService
    {
        private const long _max = int.MaxValue;
        private IHashNodeStore _hashNodeStore;

        public HashGroupStrategyForMod(IHashNodeStore hashNodeStore)
        {
            _hashNodeStore = hashNodeStore;
        }

        public async Task<string> GetHashNodeKey(HashGroup group, string key, params int[] status)
        {
            long intKey;
            try
            {
                intKey = Convert.ToInt64(key);
            }
            catch
            {
                intKey = key.ToInt();
            }

            intKey = intKey % group.Count;

            var node = await _hashNodeStore.QueryFirstByGreaterCode(group.ID, intKey, status);
            if (node == null)
            {
                node = await _hashNodeStore.QueryByMinCode(group.ID, status);
            }

            if (node == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundHashNodeByGroup,
                    DefaultFormatting = "没有找到名称为{0}的哈希组的任何节点",
                    ReplaceParameters = new List<object>() { group.Name }
                };

                throw new UtilityException((int)Errors.NotFoundHashNodeByGroup, fragment);
            }

            return node.RealNode.NodeKey;
        }

        public string GetHashNodeKeySync(HashGroup group, string key, params int[] status)
        {
            long intKey;
            try
            {
                intKey = Convert.ToInt64(key);
            }
            catch
            {
                intKey = key.ToInt();
            }

            intKey = intKey % group.Count;

            var node =  _hashNodeStore.QueryFirstByGreaterCodeSync(group.ID, intKey, status);
            if (node == null)
            {
                node =  _hashNodeStore.QueryByMinCodeSync(group.ID, status);
            }

            if (node == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundHashNodeByGroup,
                    DefaultFormatting = "没有找到名称为{0}的哈希组的任何节点",
                    ReplaceParameters = new List<object>() { group.Name }
                };

                throw new UtilityException((int)Errors.NotFoundHashNodeByGroup, fragment);
            }

            return node.RealNode.NodeKey;
        }
    }
}
