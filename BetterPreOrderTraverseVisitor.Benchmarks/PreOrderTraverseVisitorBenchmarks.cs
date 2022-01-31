using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;
using NUnit.Framework;

namespace BetterPreOrderTraverseVisitor.Benchmarks
{
    [TestFixture]
    public class PreOrderTraverseVisitorBenchmarks
    {
        [Test]
        public void RunAll()
        {
            BenchmarkRunner.Run<PreOrderTraverseVisitorBenchmarks>(
                DefaultConfig.Instance.AddDiagnoser(MemoryDiagnoser.Default));
        }

        [Benchmark]
        public Expression New_IEnumerable_First()
        {
            return new PriorityQueuePreOrderTraverseVisitor().Traverse(Expr).First();
        }

        [Benchmark]
        public Expression New_IEnumerable_Last()
        {
            return new PriorityQueuePreOrderTraverseVisitor().Traverse(Expr).Last();
        }

        [Benchmark]
        public int New_IEnumerable_Count()
        {
            return new PriorityQueuePreOrderTraverseVisitor().Traverse(Expr).Count();
        }

        [Benchmark]
        public Expression[] New_IEnumerable_ToArray()
        {
            return new PriorityQueuePreOrderTraverseVisitor().Traverse(Expr).ToArray();
        }

        [Benchmark]
        public Expression Old_IEnumerable_First()
        {
            return new PreOrderTraverseVisitor().Traverse(Expr).First();
        }

        [Benchmark]
        public Expression Old_IEnumerable_Last()
        {
            return new PreOrderTraverseVisitor().Traverse(Expr).Last();
        }

        [Benchmark]
        public int Old_IEnumerable_Count()
        {
            return new PreOrderTraverseVisitor().Traverse(Expr).Count();
        }

        [Benchmark]
        public Expression[] Old_IEnumerable_ToArray()
        {
            return new PreOrderTraverseVisitor().Traverse(Expr).ToArray();
        }

        // private static readonly Expression<Func<int, int, int, int>> Expr = (a, b, c) => ((a + 1) + b * 2 * (c - 3));
        private static readonly Expression Expr = CreateExpression();

        private static Expression CreateExpression()
        {
            return (Expression<Func<IEnumerable<Payment>, IEnumerable<Payment>>>)(paymentList => paymentList
                    .Where(payment1 =>
                        payment1.PaymentFields.Any(field => field.FieldName == "ItemA" && field.FieldValue == "50")
                    )
                    .Where(payment2 =>
                        payment2.PaymentFields.First(field => field.FieldName == "ItemA").FieldValue ==
                        payment2.PaymentFields.First(field => field.FieldName == "ItemB").FieldValue
                    )
                );
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class Payment
        {
            public int Id { set; get; }
            public string Name { set; get; }
            public List<PaymentFields> PaymentFields { set; get; }
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class PaymentFields
        {
            public string FieldName { set; get; }
            public string FieldValue { set; get; }
        }
    }
}