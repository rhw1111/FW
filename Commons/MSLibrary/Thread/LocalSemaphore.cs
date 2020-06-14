using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSLibrary.Thread
{
    public class LocalSemaphore:IDisposable
    {
        private SemaphoreSlim _lock;

        private AsyncLocal<object> _local = new AsyncLocal<object>();

        public LocalSemaphore(int initCount, int maxCount)
        {

            _lock = new SemaphoreSlim(initCount, maxCount);
        }

        public void Dispose()
        {
            _lock.Dispose();
        }

        public async Task SyncOperator(Func<Task> action)
        {
            bool needWait = true;
            if (_local.Value != null)
            {
                needWait = false;
            }
            if (needWait)
            {

                await _lock.WaitAsync();

                if (_local.Value == null)
                {
                    _local.Value = new object();
                }

            }
            try
            {

                await action();
            }
            finally
            {
                if (needWait)
                {
                    _lock.Release();
                }
            }


        }

        public void SyncOperator(Action action)
        {
            //Console.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId);
            bool needWait = true;
            if (_local.Value != null)
            {
                needWait = false;
            }
            if (needWait)
            {

                _lock.Wait();

                if (_local.Value == null)
                {
                    _local.Value = new object();
                }

            }
            try
            {

                action();
            }
            finally
            {
                if (needWait)
                {
                    _lock.Release();
                }
            }


        }

    }
}
