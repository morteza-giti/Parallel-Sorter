using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Sorter
    {
        public static Queue<int> ParallelMergeSort(Queue<Queue<int>> pending)
        {
            var working = 0;
            var tasks = Enumerable.Range(0, Environment.ProcessorCount).Select(_ =>
                Task.Factory.StartNew(() =>
                {
                    Queue<int> l1, l2, l3;
                    while (pending.Count >= 2)
                    {
                        lock (pending)
                        {
                            Interlocked.Increment(ref working);
                            l1 = pending.Dequeue();
                            l2 = pending.Dequeue();
                        }

                        l3 = new Queue<int>();

                        while (l1.Count > 0 || l2.Count > 0)
                        {
                            if (l1.Count > 0 && l2.Count > 0)
                            {
                                if (l1.First() <= l2.First())
                                {
                                    l3.Enqueue(l1.Dequeue());
                                }
                                else
                                {
                                    l3.Enqueue(l2.Dequeue());
                                }
                            }
                            else if (l1.Count > 0)
                            {
                                while (l1.Count > 0)
                                {
                                    l3.Enqueue(l1.Dequeue());
                                }
                            }
                            else if (l2.Count > 0)
                            {
                                while (l2.Count > 0)
                                {
                                    l3.Enqueue(l2.Dequeue());
                                }
                            }
                        }

                        lock (pending)
                        {
                            pending.Enqueue(l3);
                            Interlocked.Decrement(ref working);
                        }

                        while (Thread.VolatileRead(ref working) > 0 && !(pending.Count >= 2))
                        {
                            Thread.Sleep(5);
                        }
                    }

                }, TaskCreationOptions.LongRunning)
            );

            Task.WaitAll(tasks.ToArray());
            return pending.Dequeue();
        }
        public static Queue<int> MergeSort(Queue<Queue<int>> pending)
        {
            Queue<int> l1, l2, l3;
            while (pending.Count >= 2)
            {
                l1 = pending.Dequeue();
                l2 = pending.Dequeue();
                l3 = new Queue<int>();

                while (l1.Count > 0 || l2.Count > 0)
                {
                    if (l1.Count > 0 && l2.Count > 0)
                    {
                        if (l1.First() <= l2.First())
                        {
                            l3.Enqueue(l1.Dequeue());
                        }
                        else
                        {
                            l3.Enqueue(l2.Dequeue());
                        }
                    }
                    else if (l1.Count > 0)
                    {
                        while (l1.Count > 0)
                        {
                            l3.Enqueue(l1.Dequeue());
                        }
                    }
                    else if (l2.Count > 0)
                    {
                        while (l2.Count > 0)
                        {
                            l3.Enqueue(l2.Dequeue());
                        }
                    }
                }
                pending.Enqueue(l3);
            }

            return pending.Dequeue(); ;
        }

    }
}
