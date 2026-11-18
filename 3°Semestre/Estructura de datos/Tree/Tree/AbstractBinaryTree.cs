using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTSLib
{
    public abstract class AbstractBinaryTree<T> : AbstractTree<T>, IBinaryTree<T>
    {
        public abstract Node<T> AddLeft(Node<T> node, T element);
        public abstract Node<T> AddRight(Node<T> node, T element);

        public abstract Node<T> GetLeft(Node<T> node);
        public abstract Node<T> GetRight(Node<T> node);

        public bool HasLeft(Node<T> node) => GetLeft(node) != null;
        public bool HasRight(Node<T> node) => GetRight(node) != null;

        /// <summary>
        /// Gets the children of the specified node.
        /// Children are returned in left-to-right order.
        /// </summary>
        public override IEnumerable<Node<T>> GetChildren(Node<T> node)
        {
            Node<T> left = GetLeft(node);
            Node<T> right = GetRight(node);

            if (left != null)
            {
                yield return left;
            }

            if (right != null)
            {
                yield return right;
            }
        }
    }
}
