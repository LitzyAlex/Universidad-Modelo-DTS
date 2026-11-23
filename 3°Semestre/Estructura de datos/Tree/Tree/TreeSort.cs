using DTSLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ListDev;


namespace DTSLib
{
    public static class TreeSort<T> where T : IComparable<T>
    {
        // Inserta todos los elementos en un arbol binario de busqueda y los saca en in-order
        public static ArrayList<T> Sort(ArrayList<T> elements)
        {
            var tree = new LinkedBinaryTree<T>();

            foreach (T element in elements)
            {
                Insert(tree, element);
            }

            tree.TraversalOrder = TreeTraversalOrder.InOrder;

            var sorted = new ArrayList<T>();
            foreach (var node in tree.GetNodes())
            {
                sorted.Add(node.Element);
            }

            return sorted;
        }

        // Los iguales van a la derecha
        private static void Insert(LinkedBinaryTree<T> tree, T element)
        {
            if (tree.Empty)
            {
                tree.AddRoot(element);
                return;
            }

            Node<T> current = tree.Root;

            while (true)
            {
                if (element.CompareTo(current.Element) < 0)
                {
                    Node<T> left = tree.GetLeft(current);

                    if (left == null)
                    {
                        tree.AddLeft(current, element);
                        return;
                    }

                    current = left;
                }
                else
                {
                    Node<T> right = tree.GetRight(current);

                    if (right == null)
                    {
                        tree.AddRight(current, element);
                        return;
                    }

                    current = right;
                }
            }
        }
    }
}
