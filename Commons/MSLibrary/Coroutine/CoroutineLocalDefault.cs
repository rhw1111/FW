using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Coroutine
{
    /// <summary>
    /// 协程本地数据的默认实现
    /// </summary>
    public class CoroutineLocalDefault : ICoroutineLocal
    {
        private static ThreadLocal<Dictionary<string, Dictionary<string, object>>> _coroutineLocal = new ThreadLocal<Dictionary<string, Dictionary<string, object>>>();

        private static ThreadLocal<string> _currentName = new ThreadLocal<string>();

        public Dictionary<string, object> Generate(string name)
        {
            if (_coroutineLocal.Value==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CoroutineLocalNotInit,
                    DefaultFormatting = "协程本地数据尚未初始化",
                    ReplaceParameters = new List<object>() {  }
                };

                throw new UtilityException((int)Errors.CoroutineLocalNotInit,fragment);
            }
            var item= new Dictionary<string, object>();
            _coroutineLocal.Value[name] = item;
            return item;
        }

        public Dictionary<string, object> Get(string name)
        {
            if (_coroutineLocal.Value == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CoroutineLocalNotInit,
                    DefaultFormatting = "协程本地数据尚未初始化",
                    ReplaceParameters = new List<object>() { }
                };

                throw new UtilityException((int)Errors.CoroutineLocalNotInit, fragment);
            }
            if (!_coroutineLocal.Value.ContainsKey(name))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCoroutineLocalByName,
                    DefaultFormatting = "找不到名称为{0}的协程本地数据",
                    ReplaceParameters = new List<object>() {name }
                };

                throw new UtilityException((int)Errors.CoroutineLocalNotInit, fragment);
            }

            return _coroutineLocal.Value[name];
        }

        public Dictionary<string, object> GetCurrent()
        {
            if (_coroutineLocal.Value == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CoroutineLocalNotInit,
                    DefaultFormatting = "协程本地数据尚未初始化",
                    ReplaceParameters = new List<object>() {  }
                };

                throw new UtilityException((int)Errors.CoroutineLocalNotInit,fragment);
            }
            var name = GetCurrentCoroutineName();

            if (!_coroutineLocal.Value.ContainsKey(name))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCoroutineLocalByName,
                    DefaultFormatting = "找不到名称为{0}的协程本地数据",
                    ReplaceParameters = new List<object>() { name }
                };

                throw new UtilityException((int)Errors.CoroutineLocalNotInit, fragment);
            }

            return _coroutineLocal.Value[name];
        }

        public string GetCurrentCoroutineName()
        {
            if (_currentName.Value==null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCoroutineLocalByCurrent,
                    DefaultFormatting = "当前协程本地数据尚未指定",
                    ReplaceParameters = new List<object>() {  }
                };

                throw new UtilityException((int)Errors.NotFoundCoroutineLocalByCurrent, fragment);
            }
            return _currentName.Value;
        }

        public void Init()
        {
            _coroutineLocal.Value = new Dictionary<string, Dictionary<string, object>>();
        }

        public void Remove(string name)
        {
            if (_coroutineLocal.Value == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CoroutineLocalNotInit,
                    DefaultFormatting = "协程本地数据尚未初始化",
                    ReplaceParameters = new List<object>() { }
                };

                throw new UtilityException((int)Errors.CoroutineLocalNotInit, fragment);
            }
            if (!_coroutineLocal.Value.ContainsKey(name))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCoroutineLocalByName,
                    DefaultFormatting = "找不到名称为{0}的协程本地数据",
                    ReplaceParameters = new List<object>() { name }
                };

                throw new UtilityException((int)Errors.CoroutineLocalNotInit, fragment);
            }

            _coroutineLocal.Value.Remove(name);
        }

        public void SetCurrentCoroutineName(string name)
        {
            if (_coroutineLocal.Value == null)
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.CoroutineLocalNotInit,
                    DefaultFormatting = "协程本地数据尚未初始化",
                    ReplaceParameters = new List<object>() { }
                };

                throw new UtilityException((int)Errors.CoroutineLocalNotInit, fragment);
            }
            if (!_coroutineLocal.Value.ContainsKey(name))
            {
                var fragment = new TextFragment()
                {
                    Code = TextCodes.NotFoundCoroutineLocalByName,
                    DefaultFormatting = "找不到名称为{0}的协程本地数据",
                    ReplaceParameters = new List<object>() { name }
                };

                throw new UtilityException((int)Errors.CoroutineLocalNotInit, fragment);
            }
            _currentName.Value = name;
        }
    }
}
