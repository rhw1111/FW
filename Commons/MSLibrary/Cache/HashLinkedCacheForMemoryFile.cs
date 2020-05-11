using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.IO.MemoryMappedFiles;
using MSLibrary.Collections;

namespace MSLibrary.Cache
{
    /// <summary>
    /// 实现基于hash和双向链表存储的缓存容器
    /// 实际数据存储在内存映射文件中
    /// </summary>
    public class HashLinkedCacheForMemoryFile<TKey, TValue> : ICache<TKey, TValue>
    {
        private ConcurrentDictionary<TKey, LinkedListNode<KeyValuePair<TKey, TKey>>> _dict;
        private NLinkedList<KeyValuePair<TKey, TKey>> _linked;
        private INLinkedListStrategy _linkedStrategy;
        private int _length;
        private ConcurrentDictionary<int, MemoryMappedFile> _memoryFileList = new ConcurrentDictionary<int, MemoryMappedFile>();

        public HashLinkedCacheForMemoryFile(int fileCount, int fileSize)
        {
            _linked = new NLinkedList<KeyValuePair<TKey, TKey>>();
            _dict = new ConcurrentDictionary<TKey, LinkedListNode<KeyValuePair<TKey, TKey>>>();
            _linkedStrategy = new NLinkedListStrategyLRU();
            _length = 1000;
            _linked.MaxLength = _length;

            _linked.OnAdded = (node, value) =>
            {
                _dict[value.Key] = node;
               
            };
            _linked.OnRemoved = (value) =>
            {
                _dict.TryRemove(value.Key, out LinkedListNode<KeyValuePair<TKey, TKey>> v);
            };

            try
            {
                for (var index = 0; index <= fileCount - 1; index++)
                {
                    _memoryFileList[index]=MemoryMappedFile.CreateOrOpen(Guid.NewGuid().ToString(), (long)1024 * 1024 * fileSize);
                }
            }
            catch
            {
                foreach (var item in _memoryFileList)
                {
                    try
                    {
                        item.Value.Dispose();
                    }
                    catch
                    {

                    }
                    throw;
                }


            }
        }

        private int CalculateFileMapping(TKey key)
        {
            int count = _memoryFileList.Count;

            return key.ToString().ToInt() % count;        
        }


        private string ReadFromFile(MemoryMappedFile file)
        {
            byte[] bufferBytes = new byte[1024 * 1024 * 10];
            string result = string.Empty;
            using (var fileStream = file.CreateViewStream())
            {
                while(true)
                {
                    var resultLength=fileStream.Read(bufferBytes, 0, bufferBytes.Length);

                    if (resultLength<bufferBytes.Length)
                    {
                        result = $"{result}{UTF8Encoding.UTF8.GetString(bufferBytes.Take(resultLength).ToArray())}";
                        break;
                    }
                }
                fileStream.Close();
            }

            return result;
        }

        public NLinkedList<KeyValuePair<TKey, TKey>> Linked
        {
            get
            {
                return _linked;
            }
        }
        public INLinkedListStrategy LinkedStrategy
        {
            set
            {
                _linkedStrategy = value;
            }
        }

        public int Length
        {
            get
            {
                return _length;
            }

            set
            {
                _length = value;
                _linked.MaxLength = value;
            }
        }

        public int CurrentLength
        {
            get
            {
                return _dict.Count;
            }
        }


        public void Clear()
        {
            throw new NotImplementedException();
        }

        public TValue GetValue(TKey key)
        {
            throw new NotImplementedException();
        }

        public void Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public void SetValue(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }
    }
}
