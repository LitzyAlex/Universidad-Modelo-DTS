using System;
using System.Collections.Generic;

namespace TreeTerminal
{
    public enum Order { Pre, In, Post, Bfs }

    // Path: "" raiz, "L" izquierdo, "LR"... Label: significado del valor (puede ir vacio).
    public sealed class PresetNode
    {
        public string Path;
        public int Value;
        public string Label;
        public PresetNode(string path, int value, string label = "")
        {
            Path = path; Value = value; Label = label;
        }
    }

    public sealed class Preset
    {
        public Order Order;
        public string Title;
        public string Why;
        public PresetNode[] Nodes;
        public int[] Expected;
        public string Outcome;
    }

    // 12 ejemplos de la vida real, 3 por recorrido. Textos y secuencias
    // esperadas se portaron del proyecto Rust; los tests los comprueban.
    public static class Presets
    {
        // Los arboles son de enteros: los operadores van como numeros negativos.
        public const int OpAdd = -1, OpSub = -2, OpMul = -3, OpDiv = -4;

        static PresetNode N(string p, int v, string l = "") { return new PresetNode(p, v, l); }

        public static readonly Preset[] All =
        {
            // ---------------- Preorden ----------------
            new Preset {
                Order = Order.Pre,
                Title = "Notación prefija de (3 + 4) × (5 − 2)",
                Why = "Preorden escribe cada operador antes de sus operandos: eso es exactamente la notación prefija (polaca) que usan Lisp y algunas calculadoras.",
                Nodes = new[] { N("", OpMul, "×"), N("L", OpAdd, "+"), N("LL", 3), N("LR", 4), N("R", OpSub, "−"), N("RL", 5), N("RR", 2) },
                Expected = new[] { OpMul, OpAdd, 3, 4, OpSub, 5, 2 },
                Outcome = "× + 3 4 − 5 2, que vale 21."
            },
            new Preset {
                Order = Order.Pre,
                Title = "Copiar el menú de una tienda en línea",
                Why = "Para clonar un menú hay que crear cada sección antes que sus subsecciones; preorden entrega los nodos justo en ese orden.",
                Nodes = new[] { N("", 10, "Inicio"), N("L", 20, "Productos"), N("LL", 21, "Laptops"), N("LR", 22, "Celulares"), N("R", 30, "Ayuda"), N("RL", 31, "Preguntas frecuentes"), N("RR", 32, "Contacto") },
                Expected = new[] { 10, 20, 21, 22, 30, 31, 32 },
                Outcome = "Cada sección se copia antes que sus hijas; nunca falta el padre."
            },
            new Preset {
                Order = Order.Pre,
                Title = "Listar carpetas (primer hijo / siguiente hermano)",
                Why = "Una carpeta con muchos hijos se guarda en binario: izquierdo = primer hijo, derecho = siguiente hermano. Preorden lista cada carpeta antes que su contenido, como el comando tree.",
                Nodes = new[] { N("", 100, "/home"), N("L", 110, "documentos/"), N("LL", 111, "tarea.pdf"), N("LLR", 112, "notas.txt"), N("LR", 120, "fotos/"), N("LRL", 121, "playa.jpg") },
                Expected = new[] { 100, 110, 111, 112, 120, 121 },
                Outcome = "/home, documentos/, tarea.pdf, notas.txt, fotos/, playa.jpg."
            },
            // ---------------- Inorden ----------------
            new Preset {
                Order = Order.In,
                Title = "Catálogo ordenado por precio",
                Why = "En un árbol binario de búsqueda los más baratos quedan a la izquierda; inorden los lee de menor a mayor sin ordenar nada.",
                Nodes = new[] { N("", 250, "Audífonos"), N("L", 120, "Mouse"), N("LL", 80, "Cable USB"), N("LR", 180, "Teclado"), N("R", 400, "Bocina"), N("RL", 320, "Webcam"), N("RR", 2499, "Monitor") },
                Expected = new[] { 80, 120, 180, 250, 320, 400, 2499 },
                Outcome = "Precios en pesos de menor a mayor."
            },
            new Preset {
                Order = Order.In,
                Title = "Empleados por número de nómina",
                Why = "Recursos Humanos guarda a los empleados en un BST por ID; inorden genera la lista de nómina ya ordenada.",
                Nodes = new[] { N("", 1050, "Rosa"), N("L", 1020, "Luis"), N("LL", 1010, "Ana"), N("LR", 1035, "Marco"), N("LRL", 1030, "Pablo"), N("R", 1080, "Sofía"), N("RL", 1060, "Iván"), N("RR", 1095, "Elena") },
                Expected = new[] { 1010, 1020, 1030, 1035, 1050, 1060, 1080, 1095 },
                Outcome = "IDs de nómina en orden ascendente."
            },
            new Preset {
                Order = Order.In,
                Title = "Leer una fórmula en notación infija",
                Why = "Inorden coloca cada operador entre sus operandos, como la escribimos a mano. Ojo: sin paréntesis se lee 8 − 3 × 2, que no es lo que el árbol calcula.",
                Nodes = new[] { N("", OpMul, "×"), N("L", OpSub, "−"), N("LL", 8), N("LR", 3), N("R", 2) },
                Expected = new[] { 8, OpSub, 3, OpMul, 2 },
                Outcome = "8 − 3 × 2; con paréntesis por subárbol: (8 − 3) × 2 = 10."
            },
            // ---------------- Postorden ----------------
            new Preset {
                Order = Order.Post,
                Title = "Evaluar una expresión en RPN",
                Why = "Postorden da la notación polaca inversa: cada operador llega cuando sus dos operandos ya están en la pila, así se evalúa sin paréntesis.",
                Nodes = new[] { N("", OpDiv, "÷"), N("L", OpMul, "×"), N("LL", OpAdd, "+"), N("LLL", 6), N("LLR", 2), N("LR", OpSub, "−"), N("LRL", 9), N("LRR", 4), N("R", 8) },
                Expected = new[] { 6, 2, OpAdd, 9, 4, OpSub, OpMul, 8, OpDiv },
                Outcome = "6 2 + 9 4 − × 8 ÷ = ((6 + 2) × (9 − 4)) ÷ 8 = 5."
            },
            new Preset {
                Order = Order.Post,
                Title = "Tamaño total de una carpeta",
                Why = "Una carpeta pesa lo suyo más lo de su contenido. Postorden suma los hijos antes que el padre, así cada total ya está listo cuando se necesita.",
                Nodes = new[] { N("", 4, "proyecto/"), N("L", 12, "src/"), N("LL", 30, "main.rs"), N("LR", 45, "ui.rs"), N("R", 8, "assets/"), N("RL", 120, "logo.png") },
                Expected = new[] { 30, 45, 12, 120, 8, 4 },
                Outcome = "Valores en KB propios. src/ = 87 KB, assets/ = 128 KB, proyecto/ = 219 KB."
            },
            new Preset {
                Order = Order.Post,
                Title = "Liberar la memoria de una escena de juego",
                Why = "Si se libera un padre primero se pierden los punteros a sus hijos. Postorden libera de abajo hacia arriba y no deja basura.",
                Nodes = new[] { N("", 10, "Nivel"), N("L", 20, "Jugador"), N("LL", 21, "Arma"), N("LR", 22, "Mochila"), N("R", 30, "Enemigo"), N("RR", 31, "Escudo") },
                Expected = new[] { 21, 22, 20, 31, 30, 10 },
                Outcome = "Arma, Mochila, Jugador, Escudo, Enemigo y al final Nivel."
            },
            // ---------------- BFS ----------------
            new Preset {
                Order = Order.Bfs,
                Title = "Aviso que baja por el organigrama",
                Why = "Un aviso de Dirección llega primero a los gerentes y después a sus equipos. BFS recorre por niveles, igual que la cadena de mando.",
                Nodes = new[] { N("", 100, "Dirección"), N("L", 210, "Gerencia de ventas"), N("LL", 311, "Ventas norte"), N("LR", 312, "Ventas sur"), N("R", 220, "Gerencia de sistemas"), N("RL", 321, "Soporte"), N("RR", 322, "Redes") },
                Expected = new[] { 100, 210, 220, 311, 312, 321, 322 },
                Outcome = "Nivel 1: Dirección; nivel 2: gerencias; nivel 3: equipos."
            },
            new Preset {
                Order = Order.Bfs,
                Title = "Amigos a distancia 1 y 2 en una red de referidos",
                Why = "BFS encuentra primero a quien tú invitaste (distancia 1) y luego a quienes ellos invitaron (distancia 2): la base de \"personas que quizá conozcas\".",
                Nodes = new[] { N("", 7, "Tú (quien invita)"), N("L", 12, "Ana"), N("LL", 44, "Caro"), N("LR", 58, "Dani"), N("R", 31, "Beto"), N("RR", 63, "Eli") },
                Expected = new[] { 7, 12, 31, 44, 58, 63 },
                Outcome = "Distancia 1: Ana, Beto. Distancia 2: Caro, Dani, Eli."
            },
            new Preset {
                Order = Order.Bfs,
                Title = "Escalamiento en una mesa de soporte",
                Why = "Un ticket pasa por todos los técnicos de un nivel antes de escalar al siguiente; la cola de BFS reproduce ese turno.",
                Nodes = new[] { N("", 10, "Mesa de ayuda"), N("L", 21, "N2 hardware"), N("LL", 31, "N3 proveedor"), N("R", 22, "N2 software"), N("RL", 32, "N3 desarrollo"), N("RR", 33, "N3 base de datos") },
                Expected = new[] { 10, 21, 22, 31, 32, 33 },
                Outcome = "Nivel 1, luego los dos N2, luego los tres N3."
            },
        };

        // Evalua una secuencia en RPN con los codigos Op*. Null si esta mal formada o divide entre cero.
        public static int? EvalRpn(IList<int> seq)
        {
            var stack = new Stack<int>();
            foreach (int v in seq)
            {
                if (v >= OpDiv && v <= OpAdd)
                {
                    if (stack.Count < 2) return null;
                    int b = stack.Pop();
                    int a = stack.Pop();
                    if (v == OpAdd) stack.Push(a + b);
                    else if (v == OpSub) stack.Push(a - b);
                    else if (v == OpMul) stack.Push(a * b);
                    else
                    {
                        if (b == 0) return null;
                        stack.Push(a / b);
                    }
                }
                else stack.Push(v);
            }
            return stack.Count == 1 ? stack.Pop() : (int?)null;
        }
    }
}
