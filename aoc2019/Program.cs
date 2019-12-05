using System;
using System.Diagnostics;

namespace aoc2019
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = Stopwatch.StartNew();
            day5.Go();
            watch.Stop();

            Console.WriteLine("Time taken: {0} ms ({1} minutes)", watch.ElapsedMilliseconds, watch.Elapsed.Minutes);
        }
    }
}
