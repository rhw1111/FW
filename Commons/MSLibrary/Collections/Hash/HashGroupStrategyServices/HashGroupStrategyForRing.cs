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
    /// 实现哈希环策略服务
    /// </summary>
    [Injection(InterfaceType = typeof(HashGroupStrategyForRing), Scope = InjectionScope.Singleton)]
    public class HashGroupStrategyForRing : IHashGroupStrategyService
    {
        private const long _max = int.MaxValue;
        private IHashNodeStore _hashNodeStore;

        public HashGroupStrategyForRing(IHashNodeStore hashNodeStore)
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
                intKey = GetKeyValue(key, _max);
            }

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
                intKey = GetKeyValue(key, _max);
            }

            var node = _hashNodeStore.QueryFirstByGreaterCodeSync(group.ID, intKey, status);
            if (node == null)
            {
                node = _hashNodeStore.QueryByMinCodeSync(group.ID, status);
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


        private long GetKeyValue(string strKey, long max)
        {
            //强制转成大写
            var i = (long)strKey.ToUpper().ToInt();
            long result = i;

            if (strKey.Length < 1 || i < 10)
            {
                return i;
            }


            var vStr1 = strKey.Substring(strKey.Length - 1, 1).ToInt();

            var strI = i.ToString();
            var v1 = int.Parse(strI.Substring(strI.Length - 1, 1));
            var v2 = int.Parse(strI.Substring(strI.Length - 2, 1));
            var p = v1 + v2;
            if ((p == 3 || p == 7 || p == 9 || p == 11 || p == 13 || p == 15 || p == 17) && v1 != 0 && v1 != 1 && (vStr1 % 2 == 1))
            {

                while (true)
                {
                    var preResult = result;
                    result = result * v1;
                    if (result >= max || result <= 0)
                    {
                        result = preResult;
                        break;
                    }

                    var strResult = result.ToString();
                    var lResult = int.Parse(strResult.Substring(strResult.Length - 1, 1));

                    if (lResult == 5 || lResult == 6 || lResult == 7 || lResult == 8)
                    {
                        v1 = v1 * v1;
                    }
                    else
                    {
                        break;
                    }
                }

            }

            return result % max;
        }
    }
}
