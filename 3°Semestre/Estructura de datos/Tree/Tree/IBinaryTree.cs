using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTSLib
{
    /// <summary>
    /// Defines the operations supported by a binary tree.
    /// </summary>
    public interface IBinaryTree<T> : ITree<T>
    {
        /// <summary>
        /// Adds a new node as the left child of the specified node.
        /// </summary>
        /// <param name="node">The parent node.</param>
        /// <param name="element">The element to store in the new node.</param>
        /// <returns>The newly created node.</returns>
        Node<T> AddLeft(Node<T> node, T element);

        /// <summary>
        /// Adds a new node as the right child of the specified node.
        /// </summary>
        /// <param name="node">The parent node.</param>
        /// <param name="element">The element to store in the new node.</param>
        /// <returns>The newly created node.</returns>
        Node<T> AddRight(Node<T> node, T element);

        /// <summary>
        /// Gets the left child of the specified node, or null if none exists.
        /// </summary>
        Node<T> GetLeft(Node<T> node);

        /// <summary>
        /// Gets the right child of the specified node, or null if none exists.
        /// </summary>
        Node<T> GetRight(Node<T> node);

        /// <summary>
        /// Determines whether the specified node has a left child.
        /// </summary>
        bool HasLeft(Node<T> node);

        /// <summary>
        /// Determines whether the specified node has a right child.
        /// </summary>
        bool HasRight(Node<T> node);
    }
}
