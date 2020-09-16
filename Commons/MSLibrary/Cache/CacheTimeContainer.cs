using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary.Cache
{
    public class CacheTimeContainer<T>
    {
        private DateTime _cacheTime = DateTime.UtcNow;
        private int _timeout;

        public CacheTimeContainer(T value,int timeout,int type)
        {
            Value = value;
            _timeout = timeout;
            Type = type;
        }

        public T Value
        {
            get;
            set;
        }
        public int Type
        {
            get;
            set;
        }

        public bool Expire()
        {
            if (_timeout<0)
            {
                return false;
            }
            if ((DateTime.UtcNow-_cacheTime).TotalSeconds>_timeout)
            {
                return true;
            }
            return false;
        }
  
    }
}
