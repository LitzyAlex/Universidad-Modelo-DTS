using System;
using System.Collections.Generic;
using System.Linq;
using DTSLib;
using ListDev;

namespace TreeTerminal
{
    // dotnet run -- --selftest : comprobaciones con las clases originales. Sin paquetes externos.
    static class SelfTest
    {
        static int failures;

        static void Check(string name, bool ok, string detail = "")
        {
            if (!ok) failures++;
            Console.WriteLine((ok ? "OK    " : "FAIL  ") + name + (detail == "" ? "" : "  " + detail));
        }

        static string Seq(IEnumerable<int> xs) { return string.Join(" ", xs); }

        static List<int> Values(LinkedBinaryTree<int> t, TreeTraversalOrder o)
        {
            t.TraversalOrder = o;
            return t.GetNodes().Select(n => n.Element).ToList();
        }

        // 1(2(4,5),3)
        static LinkedBinaryTree<int> Sample()
        {
            var t = new LinkedBinaryTree<int>();
            Node<int> r = t.AddRoot(1);
            Node<int> two = t.AddLeft(r, 2);
            t.AddRight(r, 3);
            t.AddLeft(two, 4);
            t.AddRight(two, 5);
            return t;
        }

        public static int Run()
        {
            Console.WriteLine("Comprobaciones de árboles");
            Console.WriteLine("-------------------------");
            RunTraversals();
            RunRules();
            RunTreeSort();
            RunPresets();

            Console.WriteLine();
            Console.WriteLine(failures == 0 ? "TODAS LAS COMPROBACIONES PASAN" : failures + " COMPROBACIÓN(ES) FALLARON");
            return failures == 0 ? 0 : 1;
        }

        static void RunTraversals()
        {
            LinkedBinaryTree<int> t = Sample();
            Check("inorden 1(2(4,5),3)", Seq(Values(t, TreeTraversalOrder.InOrder)) == "4 2 5 1 3");
            Check("preorden 1(2(4,5),3)", Seq(Values(t, TreeTraversalOrder.Preorder)) == "1 2 4 5 3");
            Check("postorden 1(2(4,5),3)", Seq(Values(t, TreeTraversalOrder.Postorder)) == "4 5 2 3 1");
            Check("BFS 1(2(4,5),3)", Seq(Values(t, TreeTraversalOrder.BreadthFirst)) == "1 2 3 4 5");

            var empty = new LinkedBinaryTree<int>();
            Check("recorrido de arbol vacio", Values(empty, TreeTraversalOrder.InOrder).Count == 0);
        }

        static void RunRules()
        {
            var t = Sample();
            Node<int> root = t.Root;
            bool threw = Throws(() => t.AddRoot(9));
            Check("AddRoot falla si ya hay raiz", threw);

            threw = Throws(() => t.AddLeft(root, 9));
            Check("AddLeft falla si el hijo ya existe", threw);

            threw = Throws(() => t.Remove(root));
            Check("Remove falla con 2 hijos", threw);

            Node<int> leaf = t.GetLeft(t.GetLeft(root));   // el 4
            t.Remove(leaf);
            Check("Remove quita una hoja", t.Size == 4 && Seq(Values(t, TreeTraversalOrder.InOrder)) == "2 5 1 3");

            var chain = new LinkedBinaryTree<int>();
            Node<int> r = chain.AddRoot(1);
            Node<int> two = chain.AddLeft(r, 2);
            chain.AddLeft(two, 4);
            chain.Remove(two);
            Check("Remove con un hijo: el hijo toma su lugar", chain.GetLeft(r) != null && chain.GetLeft(r).Element == 4);
        }

        static void RunTreeSort()
        {
            Check("TreeSort vacio", TreeSort<int>.Sort(Make(new int[0])).ToList().Count == 0);
            Check("TreeSort un elemento", Seq(TreeSort<int>.Sort(Make(new[] { 5 })).ToList()) == "5");
            Check("TreeSort ordenado", Seq(TreeSort<int>.Sort(Make(new[] { 1, 2, 3, 4, 5 })).ToList()) == "1 2 3 4 5");
            Check("TreeSort invertido", Seq(TreeSort<int>.Sort(Make(new[] { 5, 4, 3, 2, 1 })).ToList()) == "1 2 3 4 5");
            Check("TreeSort duplicados", Seq(TreeSort<int>.Sort(Make(new[] { 3, 1, 3, 2, 1, 3 })).ToList()) == "1 1 2 3 3 3");
            Check("TreeSort negativos", Seq(TreeSort<int>.Sort(Make(new[] { 0, -7, 12, -3, 8, 0 })).ToList()) == "-7 -3 0 0 8 12");

            var words = new ArrayList<string>();
            foreach (string w in new[] { "pera", "manzana", "uva", "kiwi" }) words.Add(w);
            Check("TreeSort strings", string.Join(" ", TreeSort<string>.Sort(words).ToList()) == "kiwi manzana pera uva");

            // La entrada no debe cambiar
            var input = Make(new[] { 3, 1, 2 });
            TreeSort<int>.Sort(input);
            Check("TreeSort no modifica la entrada", Seq(input.ToList()) == "3 1 2");
        }

        static ArrayList<int> Make(int[] xs)
        {
            var l = new ArrayList<int>();
            foreach (int x in xs) l.Add(x);
            return l;
        }

        static void RunPresets()
        {
            Check("12 presets", Presets.All.Length == 12);
            foreach (Order o in new[] { Order.Pre, Order.In, Order.Post, Order.Bfs })
            {
                int n = Presets.All.Count(p => p.Order == o);
                Check("3 presets de " + Session.OrderName(o), n == 3, "(hay " + n + ")");
            }

            foreach (Preset p in Presets.All)
            {
                var s = new Session();
                s.Load(p);
                int size = s.Tree.Size;
                Check("preset carga todos sus nodos: " + p.Title, size == p.Nodes.Length, "(" + size + " de " + p.Nodes.Length + ")");

                var got = s.Visit(p.Order).Select(n => n.Element).ToArray();
                Check("secuencia esperada: " + p.Title, got.SequenceEqual(p.Expected),
                    got.SequenceEqual(p.Expected) ? "" : "(obtenida: " + Seq(got) + ")");
            }

            // Presets de ordenamiento: inorden debe salir ascendente
            foreach (string t in new[] { "Catálogo", "Empleados" })
            {
                Preset p = Presets.All.First(x => x.Title.StartsWith(t));
                var s = new Session();
                s.Load(p);
                var v = s.Visit(Order.In).Select(n => n.Element).ToList();
                Check("BST ascendente: " + t, v.Zip(v.Skip(1), (a, b) => a < b).All(x => x));
            }

            // RPN
            Preset rpn = Presets.All.First(x => x.Title.StartsWith("Evaluar una expresión en RPN"));
            Check("RPN del preset vale 5", Presets.EvalRpn(rpn.Expected) == 5);
            Check("RPN notación prefija no es RPN", Presets.EvalRpn(new[] { Presets.OpMul, 1, 2 }) == null);
            Check("RPN 3 4 + 5 2 - * vale 21", Presets.EvalRpn(new[] { 3, 4, Presets.OpAdd, 5, 2, Presets.OpSub, Presets.OpMul }) == 21);
            Check("RPN mal formada da null", Presets.EvalRpn(new[] { 1, Presets.OpAdd }) == null);
            Check("RPN division entre cero da null", Presets.EvalRpn(new[] { 1, 0, Presets.OpDiv }) == null);

            // Carpeta: suma de tamaños en postorden
            Preset folder = Presets.All.First(x => x.Title.StartsWith("Tamaño total"));
            var fs = new Session();
            fs.Load(folder);
            Check("tamaño de carpeta: total 219", Total(fs, fs.Tree.Root) == 219);
        }

        static int Total(Session s, Node<int> n)
        {
            return n.Element + s.Children(n).Sum(c => Total(s, c));
        }

        static bool Throws(Action a)
        {
            try { a(); return false; }
            catch (InvalidOperationException) { return true; }
        }
    }
}
