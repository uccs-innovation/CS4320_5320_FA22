using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace StudyN.Utilities
{
    public class AsyncQueue<T> : IAsyncEnumerable<T>
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);
        private readonly BufferBlock<T> _buffer = new BufferBlock<T>();

        public void Enquue(T item)
        {
            _buffer.Post(item);
        }

        public async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken token = default)
        {
            await _semaphore.WaitAsync();
            try
            {
                // Return elements until cancellationToken is triggered
                while(true)
                {
                    token.ThrowIfCancellationRequested();
                    yield return await _buffer.ReceiveAsync(token);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
