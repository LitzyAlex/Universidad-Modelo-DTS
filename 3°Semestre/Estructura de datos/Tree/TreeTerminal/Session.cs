using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTSLib;

namespace TreeTerminal
{
    // Arbol de la sesion mas las etiquetas de cada nodo. Usa las clases originales del curso.
    public sealed class Session
    {
        public LinkedBinaryTree<int> Tree = new LinkedBinaryTree<int>();

        // Por referencia: dos nodos con el mismo valor son nodos distintos.
        public readonly Dictionary<Node<int>, string> Labels =
            new Dictionary<Node<int>, string>(ReferenceEqualityComparer.Instance);

        public void Reset()
        {
            Tree = new LinkedBinaryTree<int>();
            Labels.Clear();
        }

        // Todos los nodos, de la raiz hacia abajo (no depende del orden de recorrido).
        public List<Node<int>> All()
        {
            var list = new List<Node<int>>();
            void Walk(Node<int> n)
            {
                if (n == null) return;
                list.Add(n);
                Walk(Tree.GetLeft(n));
                Walk(Tree.GetRight(n));
            }
            Walk(Tree.Root);
            return list;
        }

        // Agrega un nodo dado por ruta ("L", "LR", ...). El padre debe existir ya.
        public Node<int> AddPresetNode(PresetNode n)
        {
            Node<int> node;
            if (n.Path == "")
            {
                node = Tree.AddRoot(n.Value);
            }
            else
            {
                Node<int> parent = At(n.Path.Substring(0, n.Path.Length - 1));
                char side = char.ToUpperInvariant(n.Path[n.Path.Length - 1]);
                node = side == 'L' ? Tree.AddLeft(parent, n.Value) : Tree.AddRight(parent, n.Value);
            }
            if (!string.IsNullOrWhiteSpace(n.Label)) Labels[node] = n.Label;
            return node;
        }

        // Reemplaza el arbol por el de un preset.
        public void Load(Preset p)
        {
            Reset();
            foreach (PresetNode n in p.Nodes) AddPresetNode(n);
        }

        // Texto corto para el dibujo: el simbolo si es operador, si no valor y etiqueta.
        public string ShortText(Node<int> n)
        {
            string label;
            bool hasLabel = Labels.TryGetValue(n, out label) && label != "";
            if (hasLabel && n.Element < 0) return label;
            return hasLabel ? n.Element + " " + label : n.Element.ToString();
        }

        // "" = raiz, "L"/"R" = hijos. Devuelve null si la ruta no existe.
        public Node<int> At(string path)
        {
            Node<int> n = Tree.Root;
            foreach (char c in path.ToUpperInvariant())
            {
                if (n == null) return null;
                if (c == 'L') n = Tree.GetLeft(n);
                else if (c == 'R') n = Tree.GetRight(n);
                else return null;
            }
            return n;
        }

        public string PathOf(Node<int> node)
        {
            var sb = new StringBuilder();
            Node<int> cur = node;
            while (true)
            {
                Node<int> parent = Tree.GetParent(cur);
                if (parent == null) break;
                sb.Insert(0, Tree.GetLeft(parent) == cur ? 'L' : 'R');
                cur = parent;
            }
            return sb.ToString();
        }

        // Los operadores (valor negativo) se muestran solo con su simbolo.
        public string Text(Node<int> n)
        {
            string label;
            bool hasLabel = Labels.TryGetValue(n, out label) && label != "";
            if (hasLabel && n.Element < 0) return label;
            return hasLabel ? n.Element + " (" + label + ")" : n.Element.ToString();
        }

        public List<Node<int>> Children(Node<int> n)
        {
            var list = new List<Node<int>>();
            Node<int> l = Tree.GetLeft(n);
            Node<int> r = Tree.GetRight(n);
            if (l != null) list.Add(l);
            if (r != null) list.Add(r);
            return list;
        }

        public List<Node<int>> Visit(Order order)
        {
            Tree.TraversalOrder = ToTreeOrder(order);
            return Tree.GetNodes().ToList();
        }

        public static TreeTraversalOrder ToTreeOrder(Order o)
        {
            switch (o)
            {
                case Order.Pre: return TreeTraversalOrder.Preorder;
                case Order.In: return TreeTraversalOrder.InOrder;
                case Order.Post: return TreeTraversalOrder.Postorder;
                default: return TreeTraversalOrder.BreadthFirst;
            }
        }

        public static string OrderName(Order o)
        {
            switch (o)
            {
                case Order.Pre: return "Preorden";
                case Order.In: return "Inorden";
                case Order.Post: return "Postorden";
                default: return "BFS (por niveles)";
            }
        }
    }
}
