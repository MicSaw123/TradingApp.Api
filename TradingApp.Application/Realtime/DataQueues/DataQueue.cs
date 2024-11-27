using System.Collections.Concurrent;

namespace TradingApp.Application.Realtime.DataQueues
{
    public class DataQueue
    {
        private readonly ConcurrentQueue<(string connectionId, int pageSize, int page)> _queue = new();

        public void Enqueue(string connectionId, int pageSize, int page)
        {
            _queue.Enqueue((connectionId, pageSize, page));
        }

        public bool Dequeue(out (string connectionId, int pageSize, int page) paginationData)
        {
            return _queue.TryDequeue(out paginationData);
        }
    }
}
