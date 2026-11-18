using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTSLib
{
    public sealed class LinkedBinaryTree<T> : AbstractBinaryTree<T>
    {
        // Represents a node in a linked binary tree.
        // Each node maintains references to its parent and children.
        private sealed class BinaryNode : Node<T>
        {
            // Reference to the parent node.
            // It is null if this node is the root.
            public BinaryNode Parent { get; set; }

            // Reference to the left child.
            // It is null if no left child exists.
            public BinaryNode Left { get; set; }

            // Reference to the right child.
            // It is null if no right child exists.
            public BinaryNode Right { get; set; }

            // Constructs a binary node with the specified element
            // and optional parent node
            public BinaryNode(T element, BinaryNode parent = null)
                : base(element)
            {
                Parent = parent;
            }
        }

        public override int Size { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

        public override Node<T> AddLeft(Node<T> node, T element)
        {
            throw new NotImplementedException();
        }

        public override Node<T> AddRight(Node<T> node, T element)
        {
            throw new NotImplementedException();
        }

        public override Node<T> AddRoot(T element)
        {
            throw new NotImplementedException();
        }

        public override Node<T> GetLeft(Node<T> node)
        {
            throw new NotImplementedException();
        }

        public override Node<T> GetParent(Node<T> node)
        {
            throw new NotImplementedException();
        }

        public override Node<T> GetRight(Node<T> node)
        {
            throw new NotImplementedException();
        }

        public override T Remove(Node<T> node)
        {
            throw new NotImplementedException();
        }

        

        // Falta por implementar todo...
    }
}
