using System;
using DTSLib;
using static System.Console;

class Program
{
    static void Main(string[] args)
    {
        LinkedBinaryTree<int> tree = new LinkedBinaryTree<int>();

        WriteLine("PRUEBAS DEL LINKED BINARY TREE");
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

        WriteLine("\n**GetChildren**");

        foreach (var child in tree.GetChildren(root))
        {
            WriteLine("Hijo de 1: " + child.Element);
        }

        WriteLine("\n**Enumerator**");

        foreach (var node in tree)
        {
            WriteLine(node.Element);
        }

        WriteLine("\n**ForEach**");

        tree.ForEach(element =>
        {
            WriteLine(element);
        });


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

        WriteLine("\n****Inorder****");
        tree.TraversalOrder = TreeTraversalOrder.InOrder;

        foreach (var node in tree.GetNodes())
        {
            WriteLine(node.Element);
        }


        // PRUEBAS DEL ARRAY BINARY TREE

        ArrayBinaryTree<int> arrayTree = new ArrayBinaryTree<int>();

        WriteLine("\nPRUEBAS DEL ARRAY BINARY TREE");

        WriteLine("\n**AddRoot**");

        var arrayRoot = arrayTree.AddRoot(1);

        WriteLine("Root: " + arrayRoot.Element);
        WriteLine("Size: " + arrayTree.Size);

        WriteLine("\n**AddLeft y AddRight**");

        var arrayLeft = arrayTree.AddLeft(arrayRoot, 2);
        var arrayRight = arrayTree.AddRight(arrayRoot, 3);

        var arrayLeftLeft = arrayTree.AddLeft(arrayLeft, 4);
        var arrayLeftRight = arrayTree.AddRight(arrayLeft, 5);

        WriteLine("Se agrego 2 y 3 a 1");
        WriteLine("Se agrego 4 y 5 a 2");

        WriteLine("Size: " + arrayTree.Size);

        WriteLine("\n**GetLeft y GetRight**");

        WriteLine("Hijo izquierdo de 1: " + arrayTree.GetLeft(arrayRoot).Element);
        WriteLine("Hijo derecho de 1: " + arrayTree.GetRight(arrayRoot).Element);

        WriteLine("\n**GetParent**");

        WriteLine("Padre de 4: " + arrayTree.GetParent(arrayLeftLeft).Element);

        WriteLine("Padre de 1: " +
            (arrayTree.GetParent(arrayRoot) == null ? "null" : "Not null"));

        WriteLine("\n**HasLeft y HasRight**");

        WriteLine("1 tiene izquierda?: " + arrayTree.HasLeft(arrayRoot));
        WriteLine("1 tiene derecha?: " + arrayTree.HasRight(arrayRoot));

        WriteLine("3 tiene izquierda?: " + arrayTree.HasLeft(arrayRight));
        WriteLine("3 tiene derecha?: " + arrayTree.HasRight(arrayRight));

        WriteLine("\n**GetChildrenCount**");

        WriteLine("Hijos de 1: " + arrayTree.GetChildrenCount(arrayRoot));
        WriteLine("Hijos de 2: " + arrayTree.GetChildrenCount(arrayLeft));

        WriteLine("\n**Tipos de nodo**");

        WriteLine("1 es raiz: " + arrayTree.IsRoot(arrayRoot));
        WriteLine("2 es interno: " + arrayTree.IsInternal(arrayLeft));
        WriteLine("4 es externo: " + arrayTree.IsExternal(arrayLeftLeft));

        WriteLine("\n**Height y Depth**");

        WriteLine("Altura del arbol: " + arrayTree.Height);
        WriteLine("Profundidad de 4: " + arrayTree.GetDepth(arrayLeftLeft));
        WriteLine("Altura de 2: " + arrayTree.GetHeight(arrayLeft));

        WriteLine("\n**GetChildren**");

        foreach (var child in arrayTree.GetChildren(arrayRoot))
        {
            WriteLine("Hijo de 1: " + child.Element);
        }

        WriteLine("\n**Eliminar un nodo con dos hijos**");

        try
        {
            arrayTree.Remove(arrayRoot);
        }
        catch (InvalidOperationException ex)
        {
            WriteLine("Error esperado: " + ex.Message);
        }

        WriteLine("\n**Eliminar una hoja**");

        var arrayRemove = arrayTree.Remove(arrayLeftRight);

        WriteLine("Eliminado: " + arrayRemove);
        WriteLine("Nuevo Size: " + arrayTree.Size);

        WriteLine("\n**Eliminar un nodo con un hijo**");

        arrayRemove = arrayTree.Remove(arrayLeft);

        WriteLine("Eliminado: " + arrayRemove);
        WriteLine("Nuevo Size: " + arrayTree.Size);

        WriteLine("Hijo izquierdo de 1 ahora: " +
            arrayTree.GetLeft(arrayRoot).Element);

        WriteLine("Padre de 4 ahora: " +
            arrayTree.GetParent(arrayLeftLeft).Element);

        WriteLine("\n**Enumerator**");

        foreach (var node in arrayTree)
        {
            WriteLine(node.Element);
        }

        WriteLine("\n**ForEach**");

        arrayTree.ForEach(element =>
        {
            WriteLine(element);
        });

        WriteLine("\n**Estado del arbol**");

        WriteLine("¿El arbol esta vacio?: " + arrayTree.Empty);
        WriteLine("Cantidad de nodos: " + arrayTree.Size);
        WriteLine("Raiz: " + arrayTree.Root?.Element);

        WriteLine("\nPRUEBAS DE RECORRIDOS");

        WriteLine("\n****Preorder****");

        arrayTree.TraversalOrder = TreeTraversalOrder.Preorder;

        foreach (var node in arrayTree.GetNodes())
        {
            WriteLine(node.Element);
        }

        WriteLine("\n****Postorder****");

        arrayTree.TraversalOrder = TreeTraversalOrder.Postorder;

        foreach (var node in arrayTree.GetNodes())
        {
            WriteLine(node.Element);
        }

        WriteLine("\n****BreadthFirst****");

        arrayTree.TraversalOrder = TreeTraversalOrder.BreadthFirst;

        foreach (var node in arrayTree.GetNodes())
        {
            WriteLine(node.Element);
        }

        WriteLine("\n****Inorder****");

        arrayTree.TraversalOrder = TreeTraversalOrder.InOrder;

        foreach (var node in arrayTree.GetNodes())
        {
            WriteLine(node.Element);
        }

        // PRUEBAS ADICIONALES DE ELIMINACION

        WriteLine("\n\n**Eliminar la raiz con un hijo**");

        ArrayBinaryTree<int> rootTree = new ArrayBinaryTree<int>();

        var root2 = rootTree.AddRoot(10);
        var child2 = rootTree.AddLeft(root2, 20);
        var grandchild2 = rootTree.AddRight(child2, 30);

        WriteLine("Raiz antes: " + rootTree.Root.Element);

        var removedRoot = rootTree.Remove(root2);

        WriteLine("Eliminado: " + removedRoot);
        WriteLine("Nueva raiz: " + rootTree.Root.Element);
        WriteLine("Hijo derecho de la nueva raiz: " +
            rootTree.GetRight(rootTree.Root).Element);

        WriteLine("Size: " + rootTree.Size);

        WriteLine("\n**Eliminar la unica raiz**");

        ArrayBinaryTree<int> emptyTree = new ArrayBinaryTree<int>();

        var onlyRoot = emptyTree.AddRoot(100);

        WriteLine("Raiz antes: " + emptyTree.Root.Element);

        var removedOnlyRoot = emptyTree.Remove(onlyRoot);

        WriteLine("Eliminado: " + removedOnlyRoot);
        WriteLine("¿Esta vacio?: " + emptyTree.Empty);
        WriteLine("Size: " + emptyTree.Size);
        WriteLine("Root: " + emptyTree.Root);

    }
}
