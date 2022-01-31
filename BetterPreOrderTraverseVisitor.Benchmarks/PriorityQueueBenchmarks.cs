using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;
using NUnit.Framework;

namespace BetterPreOrderTraverseVisitor.Benchmarks
{
    [TestFixture]
    public class PriorityQueueBenchmarks
    {
        [Test]
        public void RunAll()
        {
            BenchmarkRunner.Run<PriorityQueueBenchmarks>(
                DefaultConfig.Instance.AddDiagnoser(MemoryDiagnoser.Default));
        }

        private static readonly SimplePriorityQueue<int> PriorityQueue = new SimplePriorityQueue<int>(1001);

        private static readonly OptimizedSimplePriorityQueue<int> OptimizedPriorityQueue =
            new OptimizedSimplePriorityQueue<int>(1001);

        [Benchmark]
        public void PriorityQueue_Enqueue_Dequeue()
        {
            for (var i = 0; i < 1000; i++)
            {
                PriorityQueue.Enqueue(i, i);
            }

            while (PriorityQueue.TryDequeue(out _, out _))
            {
            }
        }

        [Benchmark(Baseline = true)]
        public void OptimizedPriorityQueue_Enqueue_Dequeue()
        {
            for (var i = 0; i < 1000; i++)
            {
                OptimizedPriorityQueue.Enqueue(i, i);
            }

            while (OptimizedPriorityQueue.TryDequeue(out _, out _))
            {
            }
        }
    }
}