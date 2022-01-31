using System.Collections.Generic;
using System.Linq.Expressions;

namespace BetterPreOrderTraverseVisitor
{
    public sealed class PriorityQueuePreOrderTraverseVisitor : ExpressionVisitor
    {
        private OptimizedSimplePriorityQueue<Expression> _priorityQueue;
        private int _currentLevel;

        public override Expression Visit(Expression node)
        {
            if (node != null)
            {
                _priorityQueue.Enqueue(node, _currentLevel);
            }

            return node;
        }

        public IEnumerable<Expression> Traverse(Expression node)
        {
            yield return node;

            _priorityQueue = new OptimizedSimplePriorityQueue<Expression>(8);
            _currentLevel = 1;
            base.Visit(node);

            while (_priorityQueue.TryDequeue(out var nextNode, out var nodeLevel))
            {
                yield return nextNode;

                _currentLevel = nodeLevel + 1;
                base.Visit(nextNode);
            }
        }
    }
}