using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTSLib;
using ListDev;

namespace TreeTerminal
{
    static class Program
    {
        static int Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            if (args.Length > 0 && args[0] == "--selftest")
                return SelfTest.Run();

            new App().Run();
            return 0;
        }
    }

    sealed class App
    {
        readonly Session s = new Session();
        readonly Random rnd = new Random();

        public void Run()
        {
            while (true)
            {
                Console.WriteLine();
                TreeView.Encabezado("ÁRBOLES BINARIOS");
                Console.WriteLine();
                Console.WriteLine("1) Ver árbol");
                Console.WriteLine("2) Agregar nodo");
                Console.WriteLine("3) Cambiar valor de un nodo");
                Console.WriteLine("4) Quitar nodo");
                Console.WriteLine("5) Limpiar árbol");
                Console.WriteLine("6) Árbol aleatorio (crece en pantalla)");
                Console.WriteLine("7) Recorridos");
                Console.WriteLine("8) Presets de la vida real");
                Console.WriteLine("9) TreeSort (ordenar números)");
                Console.WriteLine("0) Salir");

                switch (Ask("Opción: "))
                {
                    case "1": TreeView.Show(s, new Frame { Title = "Estado actual" }); break;
                    case "2": AddNode(); break;
                    case "3": ChangeValue(); break;
                    case "4": RemoveNode(); break;
                    case "5": s.Reset(); Console.WriteLine("Árbol limpio."); break;
                    case "6": RandomTree(); break;
                    case "7": TraversalMenu(); break;
                    case "8": PresetMenu(); break;
                    case "9": TreeSortMenu(); break;
                    case "0": return;
                    default: Console.WriteLine("Opción no válida."); break;
                }
            }
        }

        // ---------- Edicion ----------

        void AddNode()
        {
            if (s.Tree.Empty)
            {
                int rootValue = ReadInt("Valor de la raíz: ");
                Node<int> root = s.Tree.AddRoot(rootValue);
                SetLabel(root, Ask("Etiqueta (opcional): "));
                Console.WriteLine("Raíz creada.");
                ShowGrowth(root);
                return;
            }

            Node<int> parent = PickNode("¿A qué nodo le agregas hijo? (valor): ");
            if (parent == null) return;

            string side = Ask("¿A la izquierda o a la derecha? (i/d): ").ToLowerInvariant();
            bool left = side == "i" || side == "l";
            if (!left && side != "d" && side != "r") { Console.WriteLine("Escribe i o d."); return; }

            if ((left && s.Tree.GetLeft(parent) != null) || (!left && s.Tree.GetRight(parent) != null))
            {
                Console.WriteLine("Ese lado de " + parent.Element + " ya está ocupado.");
                return;
            }

            int value = ReadInt("Valor del nuevo nodo: ");
            Node<int> child = left ? s.Tree.AddLeft(parent, value) : s.Tree.AddRight(parent, value);
            SetLabel(child, Ask("Etiqueta (opcional): "));
            ShowGrowth(child);
        }

        void ChangeValue()
        {
            Node<int> n = PickNode("¿Qué nodo cambias? (valor actual): ");
            if (n == null) return;
            int old = n.Element;
            n.Element = ReadInt("Nuevo valor: ");
            TreeView.Show(s, new Frame { Title = "Valor cambiado de " + old + " a " + n.Element });
        }

        void RemoveNode()
        {
            Node<int> n = PickNode("¿Qué nodo quitas? (valor): ");
            if (n == null) return;
            try
            {
                int removed = s.Tree.Remove(n);
                s.Labels.Remove(n);
                TreeView.Show(s, new Frame { Title = "Quitado: " + removed + " · tamaño " + s.Tree.Size });
            }
            catch (InvalidOperationException e)
            {
                // Mensaje del arbol original: solo se puede quitar un nodo con 0 o 1 hijo.
                Console.WriteLine("Error: " + e.Message);
            }
        }

        // Genera un arbol aleatorio pequeno y lo deja crecer en pantalla.
        void RandomTree()
        {
            var plan = new Session();
            int target = rnd.Next(6, 10);
            var nodes = new List<PresetNode>();

            PresetNode root = new PresetNode("", rnd.Next(1, 100));
            plan.AddPresetNode(root);
            nodes.Add(root);

            int attempts = 0;
            while (plan.Tree.Size < target && attempts++ < 500)
            {
                List<Node<int>> existing = plan.Tree.GetNodes().ToList();
                Node<int> parent = existing[rnd.Next(existing.Count)];
                string parentPath = plan.PathOf(parent);
                bool leftFree = plan.Tree.GetLeft(parent) == null;
                bool rightFree = plan.Tree.GetRight(parent) == null;
                if (!leftFree && !rightFree) continue;

                string side = leftFree && (!rightFree || rnd.Next(2) == 0) ? "L" : "R";
                var pn = new PresetNode(parentPath + side, rnd.Next(1, 100));
                plan.AddPresetNode(pn);
                nodes.Add(pn);
            }

            Grow(nodes, "Árbol aleatorio");
        }

        // ---------- Recorridos ----------

        void TraversalMenu()
        {
            if (s.Tree.Empty) { Console.WriteLine("Primero crea un árbol."); return; }

            Console.WriteLine("1) Preorden   2) Inorden   3) Postorden   4) BFS   5) Personalizado   0) Volver");
            switch (Ask("Recorrido: "))
            {
                case "1": Play(s.Visit(Order.Pre), Session.OrderName(Order.Pre)); break;
                case "2": Play(s.Visit(Order.In), Session.OrderName(Order.In)); break;
                case "3": Play(s.Visit(Order.Post), Session.OrderName(Order.Post)); break;
                case "4": Play(s.Visit(Order.Bfs), Session.OrderName(Order.Bfs)); break;
                case "5": CustomOrder(); break;
            }
        }

        void CustomOrder()
        {
            Console.WriteLine("Escribe los valores en el orden que quieras visitarlos, separados por espacios.");
            string[] tokens = Ask("Orden: ").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var seq = new List<Node<int>>();
            var seen = new HashSet<Node<int>>(ReferenceEqualityComparer.Instance);
            foreach (string t in tokens)
            {
                List<Node<int>> found = FindByText(t);
                if (found.Count == 0) { Console.WriteLine("No hay ningún nodo con '" + t + "'."); return; }
                if (found.Count > 1)
                {
                    Console.WriteLine("El valor '" + t + "' está repetido: para el orden personalizado cada valor debe ser único.");
                    return;
                }
                if (!seen.Add(found[0])) { Console.WriteLine("Ese valor aparece dos veces en el orden: " + t); return; }
                seq.Add(found[0]);
            }
            if (seq.Count == 0) { Console.WriteLine("No escribiste ningún valor."); return; }
            Play(seq, "Personalizado");
        }

        // Reproduce una secuencia de nodos: automatico o paso a paso.
        void Play(List<Node<int>> seq, string title)
        {
            Console.WriteLine();
            string mode = Ask("Modo: [a] automático  [p] paso a paso  [enter] cancelar: ").ToLowerInvariant();

            if (mode == "a")
            {
                int ms = ReadInt("Pausa entre pasos en ms [600]: ", 600);
                for (int i = 0; i < seq.Count; i++)
                {
                    TreeView.Show(s, BuildFrame(seq, i, title));
                    TreeView.Pausa(Math.Max(0, ms));
                }
                TreeView.Show(s, BuildFrame(seq, seq.Count - 1, title, "Fin del recorrido."));
            }
            else if (mode == "p")
            {
                int i = 0;
                while (true)
                {
                    TreeView.Show(s, BuildFrame(seq, i, title));
                    string c = Ask("[n] siguiente  [p] anterior  [q] salir: ").ToLowerInvariant();
                    if (c == "n")
                    {
                        if (i < seq.Count - 1) i++;
                        else Console.WriteLine("Ya es el último paso.");
                    }
                    else if (c == "p")
                    {
                        if (i > 0) i--;
                        else Console.WriteLine("Ya es el primer paso.");
                    }
                    else if (c == "q") break;
                }
            }
        }

        Frame BuildFrame(List<Node<int>> seq, int i, string title, string extra = null)
        {
            Node<int> current = seq[i];
            var done = seq.Take(i + 1).ToList();
            var doneSet = new HashSet<Node<int>>(done, ReferenceEqualityComparer.Instance);

            // Hijos ya descubiertos por un nodo visitado y todavia sin visitar.
            var pending = done.SelectMany(n => s.Children(n)).Where(c => !doneSet.Contains(c)).Distinct().ToList();

            var f = new Frame
            {
                Title = title + " · paso " + (i + 1) + " de " + seq.Count,
                Highlight = current,
                Visited = doneSet,
                Pending = new HashSet<Node<int>>(pending, ReferenceEqualityComparer.Instance),
                Legend = true
            };
            f.Lines.Add("Visitando: " + s.ShortText(current));
            f.Lines.Add("Visitados: " + string.Join(" → ", done.Select(n => s.ShortText(n))));
            f.Lines.Add("Pendientes (hijos ya descubiertos): " +
                (pending.Count == 0 ? "ninguno" : string.Join(", ", pending.Select(n => s.ShortText(n)))));
            f.Lines.Add("Faltan por visitar: " + (seq.Count - done.Count));
            if (extra != null) f.Lines.Add(extra);
            return f;
        }

        // ---------- Crecimiento animado ----------

        // Agrega los nodos uno por uno y muestra cada uno apareciendo.
        void Grow(List<PresetNode> nodes, string title)
        {
            s.Reset();
            for (int k = 0; k < nodes.Count; k++)
            {
                Node<int> added = s.AddPresetNode(nodes[k]);
                for (int step = 0; step < 3; step++)
                {
                    TreeView.Show(s, new Frame
                    {
                        Title = title + " · creciendo " + (k + 1) + " de " + nodes.Count,
                        Fading = added,
                        FadeStep = step
                    });
                    TreeView.Pausa(90);
                }
            }
            TreeView.Show(s, new Frame { Title = title + " · " + nodes.Count + " nodos" });
        }

        // Muestra un nodo recien agregado apareciendo en el arbol actual.
        void ShowGrowth(Node<int> added)
        {
            for (int step = 0; step < 3; step++)
            {
                TreeView.Show(s, new Frame { Title = "Nodo nuevo: " + added.Element, Fading = added, FadeStep = step });
                TreeView.Pausa(90);
            }
            TreeView.Show(s, new Frame { Title = "Nodo agregado: " + added.Element });
        }

        // ---------- Presets ----------

        void PresetMenu()
        {
            for (int i = 0; i < Presets.All.Length; i++)
            {
                Preset p = Presets.All[i];
                if (i == 0 || Presets.All[i - 1].Order != p.Order)
                {
                    Console.WriteLine();
                    TreeView.Escribir("── " + Session.OrderName(p.Order) + " ──", ConsoleColor.Cyan);
                    Console.WriteLine();
                }
                Console.WriteLine("  " + (i + 1) + ") " + p.Title);
            }
            Console.WriteLine("  0) Volver");

            int number;
            if (!int.TryParse(Ask("Preset: "), out number) || number < 1 || number > Presets.All.Length) return;

            Preset preset = Presets.All[number - 1];
            Grow(preset.Nodes.ToList(), preset.Title);

            var actual = s.Visit(preset.Order).Select(n => n.Element).ToArray();
            bool ok = actual.SequenceEqual(preset.Expected);
            var f = new Frame { Title = preset.Title };
            f.Lines.Add("Por qué: " + preset.Why);
            f.Lines.Add("Secuencia esperada (" + Session.OrderName(preset.Order) + "): " + string.Join(" ", preset.Expected));
            f.Lines.Add("Resultado: " + preset.Outcome);
            f.Lines.Add(ok ? "La secuencia del recorrido coincide." : "AVISO: la secuencia no coincide.");
            TreeView.Show(s, f);

            Play(s.Visit(preset.Order), Session.OrderName(preset.Order));
        }

        // ---------- TreeSort ----------

        // TreeSort: inserta cada numero en un arbol binario de busqueda (BST), lo muestra creciendo
        // y luego recorre el arbol en inorden: ese recorrido es la lista ordenada.
        void TreeSortMenu()
        {
            Console.WriteLine("Escribe números enteros separados por espacios o comas. Ej: 8 3 10 1 6");
            Console.WriteLine("(Esto reemplaza el árbol actual.)");
            string[] tokens = Ask("Números: ").Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            var values = new List<int>();
            foreach (string t in tokens)
            {
                int v;
                if (!int.TryParse(t, out v)) { Console.WriteLine("No es un número entero: " + t); return; }
                values.Add(v);
            }
            if (values.Count == 0) { Console.WriteLine("No escribiste números."); return; }

            // Construir el BST: menores a la izquierda, iguales o mayores a la derecha
            s.Reset();
            foreach (int v in values)
            {
                Node<int> added = InsertBst(v);
                ShowGrowth(added);
            }

            // Recorrido inorden = lista ordenada; se compara con TreeSort para verificar
            List<Node<int>> inorder = s.Visit(Order.In);
            var sorted = TreeSort<int>.Sort(ToArrayList(values));
            bool same = inorder.Select(n => n.Element).SequenceEqual(sorted.ToList());

            Console.WriteLine();
            Play(inorder, "TreeSort · recorrido inorden");
            Console.WriteLine("Resultado ordenado: " + string.Join(" ", sorted.ToList()));
            Console.WriteLine(same ? "Coincide con el recorrido inorden del árbol." : "AVISO: no coincide con el inorden.");
        }

        // Inserta un valor en el BST del tablero (misma regla que TreeSort: iguales a la derecha).
        Node<int> InsertBst(int value)
        {
            if (s.Tree.Empty) return s.Tree.AddRoot(value);
            Node<int> cur = s.Tree.Root;
            while (true)
            {
                if (value < cur.Element)
                {
                    Node<int> l = s.Tree.GetLeft(cur);
                    if (l == null) return s.Tree.AddLeft(cur, value);
                    cur = l;
                }
                else
                {
                    Node<int> r = s.Tree.GetRight(cur);
                    if (r == null) return s.Tree.AddRight(cur, value);
                    cur = r;
                }
            }
        }

        static ArrayList<int> ToArrayList(List<int> values)
        {
            var list = new ArrayList<int>();
            foreach (int v in values) list.Add(v);
            return list;
        }

        // ---------- Utilidades ----------

        void SetLabel(Node<int> n, string label)
        {
            if (!string.IsNullOrWhiteSpace(label)) s.Labels[n] = label.Trim();
        }

        // Busca nodos por valor o por etiqueta.
        List<Node<int>> FindByText(string text)
        {
            return s.All().Where(n => n.Element.ToString() == text
                || (s.Labels.TryGetValue(n, out string lab) && string.Equals(lab, text, StringComparison.OrdinalIgnoreCase))).ToList();
        }

        // Pide un nodo por su valor. Si hay varios con ese valor, pide cual.
        Node<int> PickNode(string prompt)
        {
            string text = Ask(prompt);
            if (text == "") return null;
            List<Node<int>> found = FindByText(text);
            if (found.Count == 0) { Console.WriteLine("No hay ningún nodo con '" + text + "'."); return null; }
            if (found.Count == 1) return found[0];

            Console.WriteLine("Hay " + found.Count + " nodos con ese valor:");
            for (int i = 0; i < found.Count; i++)
                Console.WriteLine("  " + (i + 1) + ") " + s.ShortText(found[i]) + " — " + Describe(found[i]));
            int k = ReadInt("¿Cuál? (número): ");
            return k >= 1 && k <= found.Count ? found[k - 1] : null;
        }

        string Describe(Node<int> n)
        {
            Node<int> parent = s.Tree.GetParent(n);
            if (parent == null) return "la raíz";
            return "hijo " + (s.Tree.GetLeft(parent) == n ? "izquierdo" : "derecho") + " de " + parent.Element;
        }

        static string Ask(string prompt)
        {
            Console.Write(prompt);
            string line = Console.ReadLine();
            if (line == null) Environment.Exit(0); // entrada cerrada
            return line.Trim();
        }

        static int ReadInt(string prompt, int? defaultValue = null)
        {
            while (true)
            {
                string text = Ask(prompt);
                if (text == "" && defaultValue.HasValue) return defaultValue.Value;
                int v;
                if (int.TryParse(text, out v)) return v;
                Console.WriteLine("Escribe un número entero.");
            }
        }
    }
}
