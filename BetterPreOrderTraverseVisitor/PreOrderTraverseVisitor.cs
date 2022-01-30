using System.Collections.Generic;
using System.Linq.Expressions;

namespace BetterPreOrderTraverseVisitor
{
    public sealed class PreOrderTraverseVisitor : ExpressionVisitor
    {
        private Queue<Expression> _queue = new Queue<Expression>();

        public override Expression Visit(Expression node)
        {
            if (node != null)
            {
                _queue.Enqueue(node);
            }

            return node;
        }

        public IEnumerable<Expression> Traverse(Expression node)
        {
            yield return node;
            base.Visit(node);

            while (_queue.Count > 0)
            {
                var item = _queue.Dequeue();
                var currentQueue = _queue;
                _queue = new Queue<Expression>();

                foreach (var child in Traverse(item))
                {
                    yield return child;
                }

                _queue = currentQueue;
            }
        }

        protected override Expression VisitExtension(Expression node)
        {
            return node;
        }
    }
}