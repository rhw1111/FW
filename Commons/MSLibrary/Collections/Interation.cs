using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MSLibrary.Collections
{
    public class Interation<T> :IEnumerator<T>,IEnumerable<T>
    {
        private Func<int, IList<T>> _getDataFun;
        private int _index = 0;
        private int _getIndex = 0;
        private IList<T> _datas;
        private T _current;
        public Interation(Func<int, IList<T>> getDataFun)
        {
            _getDataFun = getDataFun;
        }
        public T Current
        {
            get
            {
                return _current;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return _current;
            }
        }

        public void Dispose()
        {
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            if (_datas == null || _index > _datas.Count - 1)
            {
                _datas = _getDataFun(_getIndex);
                _getIndex++;
                _index = 0;
            }

            if (_datas == null || _datas.Count == 0)
            {
                _current = default(T);
                return false;
            }

            _current = _datas[_index];
            _index++;
            return true;

        }

        public void Reset()
        {
            _index = 0;
            _getIndex = 0;
            _datas = null;
            _current = default(T);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
    }
}
