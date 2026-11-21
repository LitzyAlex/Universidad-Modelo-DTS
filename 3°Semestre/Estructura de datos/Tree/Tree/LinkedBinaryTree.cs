using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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

        public override int Size { get; protected set; }

        public LinkedBinaryTree() : base() { }

        public override Node<T> AddRoot(T element)
        {
            if (!Empty)
            {
                throw new InvalidOperationException("El arbol ya tiene una raiz");
            }

            Root = new BinaryNode(element);
            Size++;

            return Root;
        }


        public override Node<T> AddLeft(Node<T> node, T element)
        {
            if(node == null)
            {
                throw new ArgumentNullException(nameof(node)); //no existe el nodo
            }

            BinaryNode parent = node as BinaryNode; //convertirlo a nodo binario
            if(parent.Left != null)
            {
                throw new InvalidOperationException("El nodo ya tiene un hijo izquierdo");
            }

            BinaryNode newNode = new BinaryNode(element,parent); //creo el nuevo nodo binario
            parent.Left= newNode; //se lo agrego a la izquierda a su padre
            Size++;

            return newNode;

        }

        public override Node<T> AddRight(Node<T> node, T element)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node)); //no existe el nodo
            }

            BinaryNode parent = node as BinaryNode; //convertirlo a nodo binario
            if (parent.Right != null)
            {
                throw new InvalidOperationException("El nodo ya tiene un hijo derecho");
            }

            BinaryNode newNode = new BinaryNode(element, parent); //creo el nuevo nodo binario
            parent.Right = newNode; //se lo agrego a la izquierda a su padre
            Size++;

            return newNode;
        }


        public override Node<T> GetLeft(Node<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            BinaryNode current = node as BinaryNode;
            return current.Left;
        }

        public override Node<T> GetRight(Node<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            BinaryNode current = node as BinaryNode;
            return current.Right;
        }

        public override Node<T> GetParent(Node<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            BinaryNode current = node as BinaryNode;
            return current.Parent;
        }


        public override T Remove(Node<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            BinaryNode current = node as BinaryNode;

            if (current.Left != null && current.Right != null) 
            {
                throw new InvalidOperationException("No se puede remover porque tiene 2 hijos");
            }

            //Checar cual de los dos hijos existe
            BinaryNode child= current.Left != null ? current.Left : current.Right;

            //Checar por si es el nodo raíz
            if (current == Root)
            {
                Root = child;
                if (child != null)
                {
                    child.Parent = null;
                }
            }
            else
            {
                BinaryNode parent = current.Parent;

                if (parent.Left == current)
                {
                    parent.Left = child;
                }
                else
                {
                    parent.Right = child;
                }

                if(child != null)
                {
                   child.Parent = parent;
                }
                
            }

            Size--;

            return current.Element;
            

        }

        
    }
}
