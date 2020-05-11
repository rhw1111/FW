using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MSLibrary.LanguageTranslate;

namespace MSLibrary.Collections
{
    /// <summary>
    /// 共享池
    /// 当池中数量未达到上限时，创建新的对象填充池
    /// 当数量达到上限时，随机选取对象，当对象不可用时，移除旧对象，创建新对象
    /// </summary>
    public class SharePool<T> where T : class
    {
        private Func<T> _creator;
        private Func<T, bool> _validator;
        private Action<T> _removeHandler;
        private Func<Task<T>> _creatorAsync;
        private Func<T, Task<bool>> _validatorAsync;
        public Func<T, Task> _removeHandlerAsync;


        private int _length;
        private string _name;

        private ConcurrentDictionary<T, T> _list = new ConcurrentDictionary<T, T>();


        public SharePool(string name, Func<T> creator, Func<T, bool> validator,Action<T> removeHandler, Func<Task<T>> creatorAsync, Func<T, Task<bool>> validatorAsync, Func<T, Task> removeHandlerAsync, int length)
        {
            _name = name;
            _creator = creator;
            _validator = validator;
            _creatorAsync = creatorAsync;
            _removeHandler = removeHandler;
            _validatorAsync = validatorAsync;
            _removeHandlerAsync = removeHandlerAsync;
            _length = length;
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }


        public T Get()
        {
            T v;
            if (_list.Count()< _length)
            {
                v = CreateItem();
            }
            else
            {
                v = GetItemRandom();
                if (_validator != null)
                {
                    if (!_validator(v))
                    {
                        RemoveItem(v);
                        v = CreateItem();
                    }
                }
            }

            return v;
        }

        public async Task<T> GetAsync()
        {
            T v;
            if (_list.Count() < _length)
            {
                v = await CreateItemAsync();
            }
            else
            {
                v = GetItemRandom();
                if (_validatorAsync != null)
                {
                    if (!await _validatorAsync(v))
                    {
                        await RemoveItemAsync(v);
                        v =await CreateItemAsync();
                    }
                }
            }

            return v;
        }



        public void Clear()
        {
            lock (_list)
            {
                foreach (var item in _list)
                {
                    RemoveItem(item.Value);
                }

                _list.Clear();
            }
        }

        public async Task ClearAsync()
        {
            List<T> removeList = new List<T>();
            lock (_list)
            {
                foreach (var item in _list)
                {
                    removeList.Add(item.Value);
                }

                _list.Clear();
            }

            foreach(var item in removeList)
            {
                await RemoveItemAsync(item);
            }
        }


        private T GetItemRandom()
        {
            var count= _list.Count();
            Random ran = new Random(DateTime.Now.Millisecond * Guid.NewGuid().GetHashCode());
            return _list.Values.ToList()[ran.Next(0, count)];
        }


        private T CreateItem()
        {
            var v = _creator();
            _list[v] = v;

            if (_list.Count > _length)
            {
                lock (_list)
                {
                    if (_list.Count > _length)
                    {
                        RemoveItem(v);
                    }
                }
            }

            return v;
        }


        private async Task<T> CreateItemAsync()
        {
            var v =await _creatorAsync();
            _list[v] = v;

            if (_list.Count > _length)
            {
                T removeItem = null ;
                lock (_list)
                {
                    if (_list.Count > _length)
                    {

                        _list.TryRemove(v, out removeItem);
                    }
                }

                if (removeItem!=null)
                {
                    await RemoveItemAsync(removeItem);
                }
            }

            return v;
        }


        private void RemoveItem(T v)
        {
            if (_removeHandler!=null)
            {
                _removeHandler(v);
            }

            _list.TryRemove(v, out v);
        }


        private async Task RemoveItemAsync(T v)
        {
            if (_removeHandlerAsync != null)
            {
                await _removeHandlerAsync(v);
            }

            _list.TryRemove(v, out v);
        }

    }
}
