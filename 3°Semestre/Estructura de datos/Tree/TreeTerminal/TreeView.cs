using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DTSLib;

namespace TreeTerminal
{
    // Lo que se dibuja en un cuadro: el arbol mas el estado del recorrido o del crecimiento.
    public sealed class Frame
    {
        public string Title = "";
        public Node<int> Highlight;                       // nodo que se visita ahora
        public HashSet<Node<int>> Visited = new HashSet<Node<int>>(ReferenceEqualityComparer.Instance);
        public HashSet<Node<int>> Pending = new HashSet<Node<int>>(ReferenceEqualityComparer.Instance);
        public Node<int> Fading;                          // nodo que aparece (crecimiento)
        public int FadeStep;                              // 0..2: de tenue a brillante
        public bool Legend;                               // mostrar leyenda de colores
        public List<string> Lines = new List<string>();   // texto debajo del arbol
    }

    // Dibujo del arbol con cajas redondeadas y conexiones de linea. Solo consola estandar.
    public static class TreeView
    {
        static readonly ConsoleColor[] Fade = { ConsoleColor.DarkGray, ConsoleColor.Gray, ConsoleColor.White };
        const ConsoleColor Lineas = ConsoleColor.Gray;

        public static bool Redirigida => Console.IsOutputRedirected;

        public static int Ancho
        {
            get
            {
                if (Redirigida) return 100;
                try { return Math.Max(60, Console.WindowWidth - 1); }
                catch (Exception) { return 100; }
            }
        }

        public static void Pausa(int ms)
        {
            if (!Redirigida && ms > 0) Thread.Sleep(ms);
        }

        // Encabezado en un solo color.
        public static void Encabezado(string texto)
        {
            Escribir(texto, ConsoleColor.Cyan);
            Console.WriteLine();
        }

        public static void Escribir(string texto, ConsoleColor? color)
        {
            if (color.HasValue && !Redirigida)
            {
                Console.ForegroundColor = color.Value;
                Console.Write(texto);
                Console.ResetColor();
            }
            else Console.Write(texto);
        }

        public static void Regla() => Escribir(new string('─', Ancho) + "\n", ConsoleColor.DarkGray);

        // Dibuja el cuadro completo: encabezado, arbol, lineas de estado.
        public static void Show(Session s, Frame f)
        {
            if (!Redirigida) Console.Clear();

            Encabezado("ÁRBOLES BINARIOS");
            if (f.Title != "") Escribir("  " + f.Title + "\n", ConsoleColor.Gray);
            Regla();

            int width = Ancho;
            List<Placed> placed = Layout(s, width, out int height, out int maxLen);
            var canvas = new Canvas(width, Math.Max(height, 3));

            foreach (Placed p in placed)
                if (p.Kids.Count > 0) DrawConnector(canvas, p);
            foreach (Placed p in placed)
                DrawNode(canvas, s, p, f, maxLen);

            if (placed.Count == 0) canvas.Put(0, 1, "(árbol vacío)", ConsoleColor.DarkGray);
            canvas.Print();

            Regla();
            foreach (string line in f.Lines) Console.WriteLine(line);
            if (f.Legend)
            {
                Escribir("▢ ", ConsoleColor.Yellow); Escribir("actual   ", ConsoleColor.Gray);
                Escribir("▢ ", ConsoleColor.DarkGray); Escribir("visitado   ", ConsoleColor.Gray);
                Escribir("▢ ", ConsoleColor.Magenta); Escribir("pendiente", ConsoleColor.Gray);
                Console.WriteLine();
            }
        }

        // ---------- Layout ----------

        sealed class Placed
        {
            public Node<int> N;
            public int X;                       // columna del punto de conexion (centro de la caja)
            public int Y;                       // fila de la parte superior de la caja
            public int Depth;
            public bool HasParent;
            public bool IsLeft;                 // true si es hijo izquierdo de su padre
            public List<Placed> Kids = new List<Placed>();
        }

        // Cada nivel ocupa 4 filas: caja (3) + linea de conexion (1).
        // Cada subarbol se empaqueta con su contorno para que los hermanos no se encimen.
        // Un hijo unico se desplaza a su lado (izquierda o derecha) para que se distinga.
        static List<Placed> Layout(Session s, int width, out int height, out int maxLen)
        {
            var all = new List<Placed>();
            int leaves = 0, maxDepth = 0;

            Placed Build(Node<int> n, int depth, bool hasParent)
            {
                if (n == null) return null;
                var p = new Placed { N = n, Depth = depth, Y = 4 * depth, HasParent = hasParent };
                all.Add(p);
                maxDepth = Math.Max(maxDepth, depth);
                Placed l = Build(s.Tree.GetLeft(n), depth + 1, true);
                Placed r = Build(s.Tree.GetRight(n), depth + 1, true);
                if (l != null) { l.IsLeft = true; p.Kids.Add(l); }
                if (r != null) p.Kids.Add(r);
                if (p.Kids.Count == 0) leaves++;
                return p;
            }
            Build(s.Tree.Root, 0, false);

            maxLen = Math.Clamp(width / Math.Max(leaves, 1) - 6, 2, 12);
            int gap = maxLen + 6;               // separacion minima entre cajas del mismo nivel
            int shift = gap / 2 + 1;            // desplazamiento de un hijo unico a su lado

            if (all.Count > 0) Pack(all[0], gap, shift);

            if (all.Count == 0)
            {
                height = 3;
                return all;             // arbol vacio: nada que colocar
            }
            int minX = all.Min(q => q.X), maxX = all.Max(q => q.X);
            int margin = Math.Max(0, (width - (maxX - minX)) / 2);
            foreach (Placed q in all) q.X = q.X - minX + margin;

            height = 4 * maxDepth + 3;
            return all;
        }

        // Coloca el subarbol de p con p en x = 0 y devuelve su contorno: x minimo y maximo por nivel.
        static Dictionary<int, (int Lo, int Hi)> Pack(Placed p, int gap, int shift)
        {
            var prof = new Dictionary<int, (int Lo, int Hi)> { [p.Depth] = (0, 0) };
            p.X = 0;

            if (p.Kids.Count == 1)
            {
                Placed c = p.Kids[0];
                var pc = Pack(c, gap, shift);
                int o = c.IsLeft ? -shift : shift;
                Shift(c, o);
                Merge(prof, Offset(pc, o));
            }
            else if (p.Kids.Count == 2)
            {
                Placed l = p.Kids[0], r = p.Kids[1];
                var pl = Pack(l, gap, shift);
                var pr = Pack(r, gap, shift);

                int d = gap;                    // distancia entre raices de los dos hijos
                foreach (var kv in pl)
                    if (pr.TryGetValue(kv.Key, out var rr))
                        d = Math.Max(d, kv.Value.Hi - rr.Lo + gap);

                int lo = -(d / 2), ro = d - d / 2;
                Shift(l, lo);
                Shift(r, ro);
                Merge(prof, Offset(pl, lo));
                Merge(prof, Offset(pr, ro));
            }
            return prof;
        }

        static void Shift(Placed p, int o)
        {
            p.X += o;
            foreach (Placed k in p.Kids) Shift(k, o);
        }

        static Dictionary<int, (int Lo, int Hi)> Offset(Dictionary<int, (int Lo, int Hi)> prof, int o)
        {
            var res = new Dictionary<int, (int Lo, int Hi)>();
            foreach (var kv in prof) res[kv.Key] = (kv.Value.Lo + o, kv.Value.Hi + o);
            return res;
        }

        static void Merge(Dictionary<int, (int Lo, int Hi)> dst, Dictionary<int, (int Lo, int Hi)> src)
        {
            foreach (var kv in src)
            {
                if (dst.TryGetValue(kv.Key, out var cur))
                    dst[kv.Key] = (Math.Min(cur.Lo, kv.Value.Lo), Math.Max(cur.Hi, kv.Value.Hi));
                else
                    dst[kv.Key] = kv.Value;
            }
        }

        // Caja redondeada de 3 filas:
        //   ╭───╮
        //   │ 5 │
        //   ╰───╯
        static void DrawNode(Canvas cv, Session s, Placed p, Frame f, int maxLen)
        {
            string t = s.ShortText(p.N);
            if (t.Length > maxLen) t = t.Substring(0, Math.Max(1, maxLen - 1)) + "…";
            int bw = t.Length + 4;
            int left = p.X - bw / 2;
            ConsoleColor c = ColorOf(p, f);

            cv.Put(left, p.Y, "╭" + new string('─', bw - 2) + "╮", c);
            cv.Put(left, p.Y + 1, "│ " + t + " │", c);
            cv.Put(left, p.Y + 2, "╰" + new string('─', bw - 2) + "╯", c);

            // Puntos de conexion: arriba si tiene padre, abajo si tiene hijos
            if (p.HasParent) cv.Put(p.X, p.Y, "┴", c);
            if (p.Kids.Count > 0) cv.Put(p.X, p.Y + 2, "┬", c);

            if (ReferenceEquals(p.N, f.Highlight))
            {
                cv.Put(left - 2, p.Y + 1, "▶", ConsoleColor.Yellow);
                cv.Put(left + bw + 1, p.Y + 1, "◀", ConsoleColor.Yellow);
            }
        }

        // Linea de conexion entre un padre y sus hijos, en la fila 3 bajo la caja del padre.
        static void DrawConnector(Canvas cv, Placed parent)
        {
            int row = parent.Y + 3;
            int lo = parent.X, hi = parent.X;
            foreach (Placed k in parent.Kids)
            {
                lo = Math.Min(lo, k.X);
                hi = Math.Max(hi, k.X);
            }
            for (int x = lo; x <= hi; x++)
            {
                bool up = x == parent.X;
                bool down = parent.Kids.Any(k => k.X == x);
                bool left = x > lo;
                bool right = x < hi;
                cv.Put(x, row, Glyph(up, down, left, right).ToString(), Lineas);
            }
        }

        static char Glyph(bool up, bool down, bool left, bool right)
        {
            if (up && down && left && right) return '┼';
            if (up && down && left) return '┤';
            if (up && down && right) return '├';
            if (up && left && right) return '┴';
            if (down && left && right) return '┬';
            if (up && left) return '┘';
            if (up && right) return '└';
            if (down && left) return '┐';
            if (down && right) return '┌';
            if (up || down) return '│';
            if (left || right) return '─';
            return ' ';
        }

        static ConsoleColor ColorOf(Placed p, Frame f)
        {
            if (ReferenceEquals(p.N, f.Fading)) return Fade[Math.Min(f.FadeStep, Fade.Length - 1)];
            if (ReferenceEquals(p.N, f.Highlight)) return ConsoleColor.Yellow;
            if (f.Visited.Contains(p.N)) return ConsoleColor.DarkGray;
            if (f.Pending.Contains(p.N)) return ConsoleColor.Magenta;
            return ConsoleColor.White;
        }

        // Lienzo: una foto en memoria que se vuelca de una sola vez.
        sealed class Canvas
        {
            readonly int w, h;
            readonly char[,] ch;
            readonly ConsoleColor?[,] col;

            public Canvas(int width, int height)
            {
                w = width; h = height;
                ch = new char[h, w];
                col = new ConsoleColor?[h, w];
                for (int y = 0; y < h; y++)
                    for (int x = 0; x < w; x++) ch[y, x] = ' ';
            }

            public void Put(int x, int y, string text, ConsoleColor? color)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    int xx = x + i;
                    if (y < 0 || y >= h || xx < 0 || xx >= w) continue;
                    ch[y, xx] = text[i];
                    col[y, xx] = color;
                }
            }

            public void Print()
            {
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        if (Redirigida) Console.Write(ch[y, x]);
                        else
                        {
                            Console.ForegroundColor = col[y, x] ?? ConsoleColor.Gray;
                            Console.Write(ch[y, x]);
                        }
                    }
                    if (!Redirigida) Console.ResetColor();
                    Console.WriteLine();
                }
            }
        }
    }
}
