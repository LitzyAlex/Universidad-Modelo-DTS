using System;
using ListDev;

namespace DTSLib
{
    public sealed class ArrayBinaryTree<T> : AbstractBinaryTree<T>
    {
        // Representa un nodo de este arbol
        private sealed class ArrayNode : Node<T>
        {
            public int Index { get; set; }

            public ArrayNode(T element, int index) : base(element)
            {
                Index = index;
            }
        }

        // Guarda los nodos
        private ArrayList<ArrayNode> data;

        public override int Size { get; protected set; }

        public ArrayBinaryTree() : base()
        {
            data = new ArrayList<ArrayNode>();
        }

        // Calcula los indices
        private int GetParentIndex(int index)
        {
            return (index - 1) / 2;
        }

        private int GetLeftIndex(int index)
        {
            return 2 * index + 1;
        }

        private int GetRightIndex(int index)
        {
            return 2 * index + 2;
        }

        //Metodos heredados
        public override Node<T> AddRoot(T element)
        {
            if (!Empty)
            {
                throw new InvalidOperationException("El arbol ya tiene una raiz");
            }

            ArrayNode newRoot = new ArrayNode(element, 0);

            data.SetAt(0, newRoot);
            Root = newRoot;

            Size++;

            return newRoot;
        }

        public override Node<T> AddLeft(Node<T> node, T element)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            ArrayNode parent = node as ArrayNode;

            int index = GetLeftIndex(parent.Index);

            if (data.Get(index) != null)
            {
                throw new InvalidOperationException("El nodo ya tiene un hijo izquierdo");
            }

            ArrayNode newNode = new ArrayNode(element, index);

            data.SetAt(index, newNode);

            Size++;

            return newNode;
        }

        public override Node<T> AddRight(Node<T> node, T element)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            ArrayNode parent = node as ArrayNode;

            int index = GetRightIndex(parent.Index);

            if (data.Get(index) != null)
            {
                throw new InvalidOperationException("El nodo ya tiene un hijo derecho");
            }

            ArrayNode newNode = new ArrayNode(element, index);

            data.SetAt(index, newNode);

            Size++;

            return newNode;
        }

        public override Node<T> GetLeft(Node<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            ArrayNode current = node as ArrayNode;

            int index = GetLeftIndex(current.Index);

            if (index >= data.Size)
            {
                return null;
            }

            return data.Get(index);
        }

        public override Node<T> GetRight(Node<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            ArrayNode current = node as ArrayNode;

            int index = GetRightIndex(current.Index);

            if (index >= data.Size)
            {
                return null;
            }

            return data.Get(index);
        }

        public override Node<T> GetParent(Node<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            ArrayNode current = node as ArrayNode;

            if (current.Index == 0)
            {
                return null;
            }

            int index = GetParentIndex(current.Index);

            return data.Get(index);
        }

        public override T Remove(Node<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            ArrayNode current = node as ArrayNode;

            Node<T> left = GetLeft(current);
            Node<T> right = GetRight(current);

            if (left != null && right != null)
            {
                throw new InvalidOperationException("No se puede remover porque tiene 2 hijos");
            }

            ArrayNode child = left != null ? left as ArrayNode : right as ArrayNode;

            if (child != null)
            {
                // Guardamos los nodos y sus nuevos indices
                ArrayList<ArrayNode> nodes = new ArrayList<ArrayNode>();
                ArrayList<int> newIndexes = new ArrayList<int>();

                nodes.Add(child);
                newIndexes.Add(current.Index);

                // Recorrer los nodos para calcular sus nuevas posiciones
                for (int i = 0; i < nodes.Size; i++)
                {
                    ArrayNode currentNode = nodes.Get(i);
                    int newIndex = newIndexes.Get(i);

                    ArrayNode leftChild = GetLeft(currentNode) as ArrayNode;
                    ArrayNode rightChild = GetRight(currentNode) as ArrayNode;

                    if (leftChild != null)
                    {
                        nodes.Add(leftChild);
                        newIndexes.Add(GetLeftIndex(newIndex));
                    }

                    if (rightChild != null)
                    {
                        nodes.Add(rightChild);
                        newIndexes.Add(GetRightIndex(newIndex));
                    }
                }

                // Limpiar las posiciones anteriores
                for (int i = 0; i < nodes.Size; i++)
                {
                    data.SetAt(nodes.Get(i).Index, null);
                }

                // Colocar los nodos en sus nuevas posiciones
                for (int i = 0; i < nodes.Size; i++)
                {
                    nodes.Get(i).Index = newIndexes.Get(i);
                    data.SetAt(newIndexes.Get(i), nodes.Get(i));
                }

                if (current == Root)
                {
                    Root = child;
                }
            }
            else
            {
                data.SetAt(current.Index, null);

                if (current == Root)
                {
                    Root = null;
                }
            }

            Size--;

            return current.Element;
        }
    }
}
