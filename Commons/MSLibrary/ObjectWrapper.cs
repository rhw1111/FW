using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary
{
    public class ObjectWrapper<T>
    {
        private T _v;
        public ObjectWrapper(T v)
        {
            _v = v;
        }

        public T Value
        {
            get
            {
                return _v;
            }
            set
            {
                _v = value;
            }
        }
    }
}
