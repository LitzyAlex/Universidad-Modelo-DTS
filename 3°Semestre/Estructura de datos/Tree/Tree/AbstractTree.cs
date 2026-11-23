using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ListDev;

namespace DTSLib
{
    public abstract class AbstractTree<T> : ITree<T>
    {
        public abstract int Size { get; protected set; }
        public bool Empty => Size == 0;
        public int Height => Empty ? -1 : GetHeight(Root);
        public Node<T> Root { get; protected set; }

        protected AbstractTree()
        {
            Size = 0;
            TraversalOrder = TreeTraversalOrder.Preorder;
        }

        public abstract Node<T> GetParent(Node<T> node);
        public abstract IEnumerable<Node<T>> GetChildren(Node<T> node);
        public virtual int GetChildrenCount(Node<T> node) => GetChildren(node).Count();

        public int GetDepth(Node<T> node)
        {
            if (IsRoot(node))
                return 0;
            else
                return 1 + GetDepth(GetParent(node));
        }

        public int GetHeight(Node<T> node)
        {
            int height = 0;

            foreach (var child in GetChildren(node))
            {
                height = Math.Max(height, 1 + GetHeight(child));
            }
            return height;
        }

        public bool IsRoot(Node<T> node) => node != null && node == Root;
        public bool IsInternal(Node<T> node) => GetChildrenCount(node) > 0;
        public bool IsExternal(Node<T> node) => GetChildrenCount(node) == 0;

        public abstract Node<T> AddRoot(T element);
        public abstract T Remove(Node<T> node);

        public virtual IEnumerable<Node<T>> GetNodes()
        {
            if (Empty)
                yield break;

            switch (TraversalOrder)
            {
                case TreeTraversalOrder.Preorder:
                    foreach (var node in Preorder(Root))
                        yield return node;
                    break;

                case TreeTraversalOrder.Postorder:
                    foreach (var node in Postorder(Root))
                        yield return node;
                    break;

                case TreeTraversalOrder.BreadthFirst:
                    foreach (var node in BreadthFirst(Root))
                        yield return node;
                    break;
                case TreeTraversalOrder.InOrder:
                    foreach (var node in InOrder(Root))
                        yield return node;
                    break;

                default:
                    throw new NotSupportedException(
                        "The specified traversal order is not supported.");
            }
        }

        protected virtual IEnumerable<Node<T>> Preorder(Node<T> root)
        {
            if (root == null)   // hay un nodo válido?
            {
                yield break;
            }

            yield return root;  // devuelve el nodo actual

            foreach (var child in GetChildren(root))   // recursivo y devuelve todos los hijos
            {
                foreach (var child2 in Preorder(child)) // recorrde todos los hijos de cada hijo
                {
                    yield return child2;
                }
            }
        }

        protected virtual IEnumerable<Node<T>> Postorder(Node<T> root)
        {
            if (root == null)
            {
                yield break;
            }

            foreach (var child in GetChildren(root))   // recursivo y devuelve todos los hijos
            {
                foreach (var child2 in Postorder(child)) // recorrde todos los hijos de cada hijo
                {
                    yield return child2;
                }
            }

            yield return root;  // devuelve el nodo actual
        }

        // In-order solo tiene sentido en arboles binarios.
        protected virtual IEnumerable<Node<T>> InOrder(Node<T> root)
        {
            throw new NotSupportedException("In-order solo aplica a arboles binarios.");
        }

        protected virtual IEnumerable<Node<T>> BreadthFirst(Node<T> root)
        {
            if (root == null)
            {
                yield break;
            }

            ArrayList<Node<T>> niveles = new ArrayList<Node<T>>();  // lista para guardar los nodos de cada nivel
            niveles.Add(root);  // agrega la raíz a la lista

            for (int i = 0; i < niveles.Size; i++) // mientras i sea menor a la cantidad de nodos en la lista
            {
                Node<T> current = niveles.Get(i);   // devuelve el nodo que se está procesando en la iteración
                yield return current;

                foreach (var child in GetChildren(current)) // recorre los hijos del nodo actual
                {
                    niveles.Add(child); // los añade a la lista
                }
            }
        }

        public void ForEach(Action<T> action)
        {
            foreach (var node in GetNodes())
            {
                action(node.Element);
            }
        }

        public TreeTraversalOrder TraversalOrder { get; set; }

        public IEnumerator<Node<T>> GetEnumerator() => GetNodes().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
