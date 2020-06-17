using System;
using System.Collections.Generic;
using System.Text;

namespace MSLibrary
{
    public class WrapperObject<T>
    {
        public WrapperObject(T value)
        {
            Value = value;
        }

        public T Value
        {
            get;set;
        }
    }
}
