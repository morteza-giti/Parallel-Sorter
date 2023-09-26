using ConsoleApp1;
using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        var unsorted = new Queue<Queue<int>>();
        long listLength = 100000;
        int maxSize = 1000,
            numSorts = 10;

        Random rand = new Random();

        Console.WriteLine("Creating list of size " + listLength);

        for (int i = 0; i < listLength; ++i)
        {
            var l = new Queue<int>();
            l.Enqueue(rand.Next(maxSize));
            unsorted.Enqueue(l);
        }

        Console.WriteLine("Staring Normal Sorting for " + listLength + " elements from 0 to " + maxSize);

        long last = 0;
        Stopwatch normalSortingStopWatch = new Stopwatch();
        for (int i = 0; i < numSorts; ++i)
        {
            var pending = new Queue<Queue<int>>();
            foreach (Queue<int> q in unsorted)
            {
                pending.Enqueue(new Queue<int>(q));
            }
            normalSortingStopWatch.Start();
            Queue<int> sorted = Sorter.MergeSort(pending);
            normalSortingStopWatch.Stop();
            Console.WriteLine("MergeSort " + i + " - " + (normalSortingStopWatch.ElapsedMilliseconds - last) + "ms");
            last = normalSortingStopWatch.ElapsedMilliseconds;
        }
        Console.WriteLine("Normal Sorting: " + normalSortingStopWatch.ElapsedMilliseconds / numSorts + "ms (AVG)");

        long lastParallel = 0;
        Stopwatch parallelSortingStopWatch = new Stopwatch();
        Console.WriteLine("Starting Parallel for " + listLength + " elements from 0 to " + maxSize);
        for (int i = 0; i < numSorts; ++i)
        {
            var pending = new Queue<Queue<int>>();
            foreach (Queue<int> q in unsorted)
            {
                pending.Enqueue(new Queue<int>(q));
            }
            parallelSortingStopWatch.Start();
            Queue<int> sortedParallel = Sorter.ParallelMergeSort(pending);
            parallelSortingStopWatch.Stop();
            Console.WriteLine("ParallelMergeSort " + i + " - " + (parallelSortingStopWatch.ElapsedMilliseconds - lastParallel) + "ms");
            lastParallel = parallelSortingStopWatch.ElapsedMilliseconds;
        }
        Console.WriteLine("Parallel Sorting: " + parallelSortingStopWatch.ElapsedMilliseconds / numSorts + "ms (AVG)");
        Console.ReadKey();
    }
}