namespace iot_infrastructure
{
    public class WaitablePriorityQueue<E> where E : class
    {
        private readonly Semaphore semaphoreNotEmpty;
        private readonly PriorityQueue<E, E> priorityQueue;

        public WaitablePriorityQueue(IComparer<E> comparer, int initCapacity)
        {
            semaphoreNotEmpty = new Semaphore(0, initCapacity); // TODO: solve the capacity issue
            priorityQueue = new PriorityQueue<E, E>(initCapacity, comparer);
        }

        public void Enqueue(E elem)
        {
            lock (this) {
                priorityQueue.Enqueue(elem, elem);
            }
            semaphoreNotEmpty.Release();
        }

        public E Dequeue()
        {
            semaphoreNotEmpty.WaitOne();
            lock (this)
            {
                return priorityQueue.Dequeue();
            }
        }

        public bool Remove(E elem)
        {
            var removed = false;

            lock (this) {
                var reff = new Queue<E>();
                var s = priorityQueue.Count;

                while (priorityQueue.Count > 0) {
                    if (priorityQueue.Peek() == elem) {
                        priorityQueue.Dequeue();
                        removed = true;
                        break;
                    }
                    reff.Enqueue(priorityQueue.Dequeue());
                }

                while (reff.Count > 0)
                {
                    var element = reff.Dequeue();
                    priorityQueue.Enqueue(element, element);
                }
            }

            return removed;
        }
    }
}
