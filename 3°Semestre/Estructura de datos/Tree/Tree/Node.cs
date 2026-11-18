using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTSLib
{
    /// <summary>
    /// Represents the abstract base class for a node in a tree.
    /// </summary>
    public abstract class Node<T>
    {
        /// <summary>
        /// Gets or sets the element stored in the node.
        /// </summary>
        public T Element { get; set; }

        /// <summary>
        /// Initializes a new instance of the Node class.
        /// </summary>
        /// <param name="element">The element to store in the node.</param>
        protected Node(T element)
        {
            Element = element;
        }
    }
}
