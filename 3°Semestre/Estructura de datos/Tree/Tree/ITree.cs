using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTSLib
{
    /// <summary>
    /// Defines the operations supported by a generic tree.
    /// </summary>
    public interface ITree<T> : IEnumerable<Node<T>>
    {
        // Tree properties

        /// <summary>
        /// Gets the number of nodes in the tree.
        /// </summary>
        int Size { get; }

        /// <summary>
        /// Gets whether the tree contains no nodes.
        /// </summary>
        bool Empty { get; }

        /// <summary>
        /// Gets the height of the tree.
        /// Returns -1 if the tree is empty.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Gets the root node of the tree, or null if the tree is empty.
        /// </summary>
        Node<T> Root { get; }

        // Node access operations

        /// <summary>
        /// Gets the parent of the specified node, or null if it is the root.
        /// </summary>
        Node<T> GetParent(Node<T> node);

        /// <summary>
        /// Gets the children of the specified node.
        /// </summary>
        IEnumerable<Node<T>> GetChildren(Node<T> node);

        /// <summary>
        /// Gets the number of children of the specified node.
        /// </summary>
        int GetChildrenCount(Node<T> node);

        // Node properties

        /// <summary>
        /// Gets the depth of the specified node.
        /// The root node has a depth of 0.
        /// </summary>
        int GetDepth(Node<T> node);
        
        /// <summary>
        /// Gets the height of the subtree rooted at the specified node.
        /// A leaf node has a height of 0.
        /// </summary>
        int GetHeight(Node<T> node);

        /// <summary>
        /// Determines whether the specified node is the root.
        /// </summary>
        bool IsRoot(Node<T> node);

        /// <summary>
        /// Determines whether the specified node has at least one child.
        /// </summary>
        bool IsInternal(Node<T> node);

        /// <summary>
        /// Determines whether the specified node has no children.
        /// </summary>
        bool IsExternal(Node<T> node);

        // Tree modification operations

        /// <summary>
        /// Creates the root node with the specified element.
        /// </summary>
        Node<T> AddRoot(T element);

        /// <summary>
        /// Removes the specified node from the tree.
        /// Only leaf nodes or nodes with a single child can be removed.
        /// If the node has one child, that child replaces the removed node.
        /// </summary>
        T Remove(Node<T> node);

        // Tree traversal operations

        /// <summary>
        /// Gets all nodes in the tree following the configured traversal order.
        /// </summary>
        IEnumerable<Node<T>> GetNodes();

        /// <summary>
        /// Performs the specified action on each node's element,
        /// following the configured traversal order.
        /// </summary>
        void ForEach(Action<T> action);

        // Traversal configuration

        /// <summary>
        /// Gets or sets the traversal order used by tree traversal operations.
        /// </summary>
        TreeTraversalOrder TraversalOrder { get; set; }
    }
}
