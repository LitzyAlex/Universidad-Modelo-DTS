using System;
using DTSLib;
using static System.Console;

class Program
{
    static void Main(string[] args)
    {
        LinkedBinaryTree<int> tree = new LinkedBinaryTree<int>();

        WriteLine("PRUEBAS DEL ÁRBOL BINARIO");
        WriteLine("\n**AddRoot**");

        var root = tree.AddRoot(1);

        WriteLine("Root: " + root.Element);
        WriteLine("Size: " + tree.Size);

        WriteLine("\n**Addchild**");

        var left = tree.AddLeft(root, 2);
        var right = tree.AddRight(root, 3);

        var leftLeft = tree.AddLeft(left, 4);
        var leftRight = tree.AddRight(left, 5);

        WriteLine("Se agrego 2 y 3 a 1");
        WriteLine("Se agrego 4 y 5 a 2");

        WriteLine("Size: " + tree.Size);

        WriteLine("Hijo izquierdo de 1: " + tree.GetLeft(root).Element);

        WriteLine("Hijo derecho de 1: " + tree.GetRight(root).Element);

        WriteLine("\n**GetParent**");

        WriteLine("Padre de 4: " + tree.GetParent(leftLeft).Element);

        WriteLine("Padre de 1: " + (tree.GetParent(root) == null ? "null" : "Not null"));

        WriteLine("\n**Haschild**");

        WriteLine("1 tiene izuierda?: " + tree.HasLeft(root));
        WriteLine("1 tiene derecha?: " + tree.HasRight(root));

        WriteLine("3 tiene izuierda?: " + tree.HasLeft(right));
        WriteLine("3 tiene derecha?: " + tree.HasRight(right));

        WriteLine("\n**Countchildren**");

        WriteLine("Hijos de 1: " + tree.GetChildrenCount(root));

        WriteLine("Hijos de 2: " + tree.GetChildrenCount(left));

        WriteLine("\n**Tipos de nodo**");

        WriteLine("1 es raiz: " + tree.IsRoot(root));
        WriteLine("2 es interno: " + tree.IsInternal(left));
        WriteLine("4 es externo: " + tree.IsExternal(leftLeft));

        WriteLine("\n**Height y depth**");

        WriteLine("Altura del tree: " + tree.Height);
        WriteLine("profundidad de 4: " + tree.GetDepth(leftLeft));
        WriteLine("altura de 2: " + tree.GetHeight(left));

        WriteLine("\n**Eliminar hojas**");

        var remove = tree.Remove(leftRight);

        WriteLine("Eliminado: " + remove);
        WriteLine("Nuevo Size: " + tree.Size);

        WriteLine("\n**Eliminar un nodo con un hijo**");

        remove = tree.Remove(left);

        WriteLine("Eliminado: " + remove);
        WriteLine("Nuevo Size: " + tree.Size);

        WriteLine("EL hijo izquierdo de 1 es ahora: " + tree.GetLeft(root).Element);

        WriteLine("El padre de 4 es ahora: " + tree.GetParent(leftLeft).Element);

        /*
        WriteLine("\n**Eliminar raiz**");

        tree.Remove(right);
        remove = tree.Remove(root);

        WriteLine("Eliminar raiz: " + remove);
        WriteLine("Esta vacio: " + tree.Empty);
        WriteLine("Size: " + tree.Size);

        remove = tree.Remove(tree.Root);

        WriteLine("Eliminar ultima raiz: " + remove);
        WriteLine("Esta vacio: " + tree.Empty);
        WriteLine("Size: " + tree.Size);

        WriteLine("\n**Agregar otra raiz**");

        var newRoot = tree.AddRoot(0);

        WriteLine("Nueva raiz: " + newRoot.Element);
        WriteLine("Size: " + tree.Size);
        */

        WriteLine("\nPRUEBAS DE RECORRIDOS");

        WriteLine($"¿El árbol está vacío? {tree.Empty}");
        WriteLine($"Cantidad de nodos: {tree.Size}");
        WriteLine($"Raíz: {tree.Root?.Element}");

        WriteLine("\n****Preorder****");
        tree.TraversalOrder = TreeTraversalOrder.Preorder;

        foreach (var node in tree.GetNodes())
        {
            WriteLine(node.Element);
        }

        WriteLine("\n****Postorder****");
        tree.TraversalOrder = TreeTraversalOrder.Postorder;

        foreach (var node in tree.GetNodes())
        {
            WriteLine(node.Element);
        }

        WriteLine("\n****BreadthFirst****");
        tree.TraversalOrder = TreeTraversalOrder.BreadthFirst;

        foreach (var node in tree.GetNodes())
        {
            WriteLine(node.Element);
        }
    }
}
