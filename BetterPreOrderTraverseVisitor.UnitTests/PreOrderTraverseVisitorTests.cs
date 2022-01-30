using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using Shouldly;

namespace BetterPreOrderTraverseVisitor.UnitTests
{
    [TestFixture]
    public class PreOrderTraverseVisitorTests
    {
        [Test]
        public void TraverseTest()
        {
            var expressions = new Expression[]
            {
                (Expression<Func<int, int, int>>)((a, b) => (a + b)),
                (Expression<Func<int, int, int>>)((a, b) => ((a + 1) + b * 2)),
                (Expression<Func<int, int, int, int>>)((a, b, c) => ((a + 1) + b * 2 * (c - 3))),
                CreateComplexExpression(),
            };

            foreach (var expression in expressions)
            {
                var expected = new PreOrderTraverseVisitor().Traverse(CreateComplexExpression())
                    .Select(x => x.ToString())
                    .ToArray();

                var actual = new PriorityQueuePreOrderTraverseVisitor().Traverse(CreateComplexExpression())
                    .Select(x => x.ToString())
                    .ToArray();

                actual.ShouldBe(expected, ignoreOrder: false);
            }
        }

        private static Expression CreateComplexExpression()
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