using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Collections
{
    public class Pool<T>:IDisposable where T : class
    {
        private object _lockObj = new object();
        private object _readLockObj = new object();
        private Func<T> _creator;
        private Func<T, bool> _validator;
        private Action<T> _returnHandler;
        private Action<T> _removeHandler;
        private Func<Task<T>> _creatorAsync;
        private Func<T, Task<bool>> _validatorAsync;
        public Func<T, Task> _returnHandlerAsync;
        public Func<T, Task> _removeHandlerAsync;
        private int _length;
        private string _name;
        private SemaphoreSlim _semaphore;
        private object _lockSemaphoreObj = new object();


        private ConcurrentDictionary<T, PoolItem<T>> _list = new ConcurrentDictionary<T, PoolItem<T>>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">池名称</param>
        /// <param name="creator">池中数据创建代理,只在同步方法中使用</param>
        /// <param name="validator">池中数据检查代理，每次取数据时如果未通过检查，则将移除数据，重新生成，只在同步方法中使用</param>
        /// <param name="returnHandler">当数据被放回池中的时候，需要做的处理</param>
        /// <param name="creatorAsync">池中数据创建代理,只在异步方法中使用<</param>
        /// <param name="validatorAsync">池中数据检查代理，每次取数据时如果未通过检查，则将移除数据，重新生成，只在异步方法中使用</param>
        /// <param name="returnHandlerAsync">当数据被放回池中的时候，需要做的处理,只在异步方法中使用</param>
        /// <param name="length">池的最大长度</param>
        public Pool(string name,Func<T> creator, Func<T, bool> validator,Action<T> returnHandler, Action<T> removeHandler, Func<Task<T>> creatorAsync, Func<T, Task<bool>> validatorAsync, Func<T,Task> returnHandlerAsync, Func<T, Task> removeHandlerAsync, int length)
        {
            _name = name;
            _creator = creator;
            _validator = validator;
            _creatorAsync = creatorAsync;
            _returnHandler = returnHandler;
            _removeHandler = removeHandler;
            _validatorAsync = validatorAsync;
            _returnHandlerAsync = returnHandlerAsync;
            _removeHandlerAsync = removeHandlerAsync;
            _length = length;
            _semaphore = new SemaphoreSlim(1,1);
        }

        public T Get(bool wait=false)
        {
            InnerGetValue<T> result;
            while (true)
            {
                result = InnerGet(wait);
                if (!result.Continue)
                {
                    break;
                }
            }

            return result.Result;


        }


        public async Task<T> GetAsync(bool wait = false)
        {
            InnerGetValue<T> result;
            while (true)
            {
                result = await InnerGetAsync(wait);
                if (!result.Continue)
                {
                    break;
                }
            }

            return result.Result;
        }



        public string Name
        {
            get
            {
                return _name;
            }
        }

        public void Return(T value)
        {
            if (_list.TryGetValue(value, out PoolItem<T> existItem))
            {

                if (existItem.Use)
                {
                    try
                    {
                        if (_returnHandler != null)
                        {
                            _returnHandler(existItem.Value);
                        }
                        existItem.Use = false;
                        ReleaseSemaphore();
                    }
                    catch
                    {
                        Remove(existItem.Value);
                        throw;
                    }

                }

            }
        }


        public async Task ReturnAsync(T value)
        {
            if (_list.TryGetValue(value, out PoolItem<T> existItem))
            {

                if (existItem.Use)
                {
                    try
                    {
                        if (_returnHandler != null)
                        {
                            _returnHandler(existItem.Value);
                        }
                        existItem.Use = false;
                        ReleaseSemaphore();
                    }
                    catch
                    {
                        await RemoveAsync(existItem.Value);
                        throw;
                    }
                }

            }
        }



        public List<PoolItem<T>> Items
        {
            get
            {
                return _list.Values.ToList();
            }
        }

        public void Clear()
        {
            List<T> clearList = new List<T>();
            if (_removeHandler != null)
            {
                foreach (var item in _list)
                {
                    clearList.Add(item.Value.Value);
                }
            }


            _list.Clear();

            try
            {
                if (_removeHandler != null)
                {
                    foreach (var item in clearList)
                    {
                        _removeHandler(item);
                    }
                }
            }
            finally
            {
                ReleaseSemaphore();
            }
        }

        public async Task ClearAsync()
        {
            List<T> clearList = new List<T>();
            if (_removeHandler != null)
            {
                foreach (var item in _list)
                {
                    clearList.Add(item.Value.Value);
                }
            }


            _list.Clear();

            try
            {
                if (_removeHandlerAsync != null)
                {
                    foreach (var item in clearList)
                    {
                        await _removeHandlerAsync(item);
                    }
                }
            }
            finally
            {
                ReleaseSemaphore();
            }
        }


        public void Remove(T value)
        {
            try
            {
                if (_list.TryRemove(value, out PoolItem<T> existItem))
                {
                    if (_removeHandler != null)
                    {
                        _removeHandler(existItem.Value);
                    }
                }
            }
            finally
            {
                ReleaseSemaphore();
            }
        }

        public async Task RemoveAsync(T value)
        {
            try
            {
                if (_list.TryRemove(value, out PoolItem<T> existItem))
                {
                    if (_removeHandlerAsync != null)
                    {
                        await _removeHandlerAsync(existItem.Value);
                    }
                }
            }
            finally
            {
                ReleaseSemaphore();
            }
        }

        /// <summary>
        /// 对每个未使用的项执行，执行完成以后再放回池中
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task InvokeEveryUnUseItem(Func<T,Task> callback)
        {
            //Exception error = null ;
            //获取所有未使用的项
            var unUseItems = (from item in _list.Values
                              where item.Use == false
                              select item).ToList();

            //针对每个项做取出动作,运行完毕以后，必须放回池中
            foreach(var unUseItem in unUseItems)
            {
                var canRun = false;
                lock (_readLockObj)
                {
                    if (!unUseItem.Use)
                    {
                        unUseItem.Use = true;
                        canRun = true;
                    }
                }

                if (canRun)
                {
                    try
                    {
                        await callback(unUseItem.Value);
                    }
                    finally
                    {
                        try
                        {
                            if (_returnHandlerAsync != null)
                            {
                                await _returnHandlerAsync(unUseItem.Value);
                            }
                            unUseItem.Use = false;
                            ReleaseSemaphore();
                        }
                        catch
                        {
                            await RemoveAsync(unUseItem.Value);
                            throw;

                        }
                    }
                }
            }

        }


        private void ReleaseSemaphore()
        {
            lock(_lockSemaphoreObj)
            {
                if (_semaphore.CurrentCount == 0)
                {
                    try
                    {
                        _semaphore.Release();
                    }
                    catch (SemaphoreFullException)
                    {

                    }
                }
            }
        }


        private InnerGetValue<T> InnerGet(bool wait = false)
        {

            InnerGetValue<T> result = new InnerGetValue<T>();
            T value = default(T);
            PoolItem<T> availableItem;

            try
            {
                lock (_readLockObj)
                {
                    availableItem = (from item in _list.Values
                                     where item.Use == false
                                     select item).FirstOrDefault();

                    if (availableItem != null)
                    {
                        availableItem.Use = true;
                    }

                }
            }
            catch (InvalidOperationException)
            {
                lock (_lockObj)
                {
                    availableItem = (from item in _list.Values
                                     where item.Use == false
                                     select item).FirstOrDefault();

                    if (availableItem != null)
                    {
                        availableItem.Use = true;
                    }
                }
            }

            if (availableItem != null)
            {
                if (_validator != null)
                {
                    if (! _validator(availableItem.Value))
                    {
                        Remove(availableItem.Value);
                        availableItem = null;

                        ReleaseSemaphore();
                    }
                }
            }

            //表示没有找到可用项
            if (availableItem == null)
            {

                if (_list.Count + 1 > _length)
                {
                    if (wait)
                    {
                        _semaphore.Wait();

                        result.Continue = true;
                        result.Result = null;
                        return result;
                    }
                    else
                    {
                        var fragment = new TextFragment()
                        {
                            Code = TextCodes.PoolLengthOverflow,
                            DefaultFormatting = "池{0}已经达到最大长度，最大长度为{1}",
                            ReplaceParameters = new List<object>() { _name, _length.ToString() }
                        };

                        throw new UtilityException((int)Errors.PoolLengthOverflow, fragment);
                    }
                }

                //添加项
                value = _creator();

                lock (_lockObj)
                {
                    if (_list.Count + 1 > _length)
                    {
                        if (wait)
                        {
                            _semaphore.Wait();

                            result.Continue = true;
                            result.Result = null;
                            return result;
                        }
                        else
                        {
                            var fragment = new TextFragment()
                            {
                                Code = TextCodes.PoolLengthOverflow,
                                DefaultFormatting = "池{0}已经达到最大长度，最大长度为{1}",
                                ReplaceParameters = new List<object>() { _name, _length.ToString() }
                            };

                            throw new UtilityException((int)Errors.PoolLengthOverflow, fragment);
                        }
                    }

                    _list.AddOrUpdate(value, new PoolItem<T>() { Use = true, Value = value }, (v, p) => p);
                }
            }
            else
            {

                value = availableItem.Value;
            }

            result.Continue = false;
            result.Result = value;
            return result;

        }


        private async Task<InnerGetValue<T>> InnerGetAsync(bool wait = false)
        {

            InnerGetValue<T> result = new InnerGetValue<T>();
            T value = default(T);
            PoolItem<T> availableItem;

            try
            {
                lock (_readLockObj)
                {
                    availableItem = (from item in _list.Values
                                     where item.Use == false
                                     select item).FirstOrDefault();

                    if (availableItem != null)
                    {
                        availableItem.Use = true;
                    }

                }
            }
            catch (InvalidOperationException)
            {
                lock (_lockObj)
                {
                    availableItem = (from item in _list.Values
                                     where item.Use == false
                                     select item).FirstOrDefault();

                    if (availableItem != null)
                    {
                        availableItem.Use = true;
                    }
                }
            }

            if (availableItem != null)
            {
                if (_validatorAsync != null)
                {
                    if (!await _validatorAsync(availableItem.Value))
                    {
                        await RemoveAsync(availableItem.Value);
                        availableItem = null;

                        ReleaseSemaphore();
                    }
                }
            }

            //表示没有找到可用项
            if (availableItem == null)
            {

                if (_list.Count + 1 > _length)
                {
                    if (wait)
                    {
                        await _semaphore.WaitAsync();

                        result.Continue = true;
                        result.Result = null;
                        return result;
                    }
                    else
                    {
                        var fragment = new TextFragment()
                        {
                            Code = TextCodes.PoolLengthOverflow,
                            DefaultFormatting = "池{0}已经达到最大长度，最大长度为{1}",
                            ReplaceParameters = new List<object>() { _name, _length.ToString() }
                        };

                        throw new UtilityException((int)Errors.PoolLengthOverflow, fragment);
                    }
                }

                //添加项
                value = await _creatorAsync();

                lock (_lockObj)
                {
                    if (_list.Count + 1 > _length)
                    {
                        if (wait)
                        {
                            _semaphore.Wait();

                            result.Continue = true;
                            result.Result = null;
                            return result;
                        }
                        else
                        {
                            var fragment = new TextFragment()
                            {
                                Code = TextCodes.PoolLengthOverflow,
                                DefaultFormatting = "池{0}已经达到最大长度，最大长度为{1}",
                                ReplaceParameters = new List<object>() { _name, _length.ToString() }
                            };
                            throw new UtilityException((int)Errors.PoolLengthOverflow, fragment);
                        }
                    }

                    _list.AddOrUpdate(value, new PoolItem<T>() { Use = true, Value = value }, (v, p) => p);
                }
            }
            else
            {

                value = availableItem.Value;
            }

            result.Continue = false;
            result.Result = value;
            return result;

        }

        public void Dispose()
        {
            _semaphore.Dispose();
        }

        class InnerGetValue<V>
        {
            public bool Continue { get; set; }
            public V Result { get; set; }
        }
    }

    public class PoolItem<T>
    {
        public bool Use { get; set; }
        public T Value { get; set; }
    }
}
