using System;
using ListDev;    // ArrayList<T> (lista del equipo)

namespace SimMarket.CLI
{
    internal readonly record struct Parte(string Texto, ConsoleColor? Color);

    internal static class Cli
    {
        private static bool Redirigida => Console.IsOutputRedirected;

        public static bool FinDeEntrada { get; private set; }

        // Columnas disponibles, para recortar los renglones y que nada se desborde
        public static int Ancho
        {
            get
            {
                if (Redirigida)
                {
                    return 100;
                }
                try
                {
                    return Math.Max(40, Console.WindowWidth - 1);
                }
                catch (Exception)
                {
                    return 100;
                }
            }
        }

        // Metodos
        public static void Limpiar()
        {
            if (Redirigida)
            {
                Console.WriteLine();
                return;
            }
            Console.Clear();
        }

        public static void Escribir(string texto, ConsoleColor? color = null)
        {
            if (color.HasValue && !Redirigida)
            {
                Console.ForegroundColor = color.Value;
                Console.Write(texto);
                Console.ResetColor();
                return;
            }
            Console.Write(texto);
        }

        public static void EscribirLinea(string texto = "", ConsoleColor? color = null)
        {
            Escribir(texto, color);
            Console.WriteLine();
        }

        public static void Error(string mensaje) => EscribirLinea("   " + mensaje, ConsoleColor.Red);

        public static void Regla() => EscribirLinea(new string('─', Ancho), ConsoleColor.DarkGray);

        public static void OcultarCursor(bool ocultar)
        {
            if (Redirigida)
            {
                return;
            }
            try
            {
                Console.CursorVisible = !ocultar;
            }
            catch (Exception)
            {
                // uwuwuwuwuwuuwuwuwuwuwuwu
            }
        }

        // Barra tipo ███░░ para mostrar cuanto de algo ya se lleno
        public static string Barra(int valor, int maximo, int ancho)
        {
            int llenos = maximo <= 0 ? 0 : (int)Math.Round((double)valor / maximo * ancho);
            llenos = Math.Clamp(llenos, 0, ancho);
            return new string('█', llenos) + new string('░', ancho - llenos);
        }

        public static string Pref(Cliente c) => c.EsPreference ? " PREF" : "";

        private static string TextoSemilla(int? semilla) => semilla.HasValue ? $"semilla {semilla.Value}" : "semilla aleatoria";

        private static string Porcentaje(int parte, int total) => total == 0 ? "0%" : $"{100.0 * parte / total:0}%";

        public static string LeerLinea()
        {
            var linea = Console.ReadLine();
            if (linea == null)
            {
                FinDeEntrada = true;
                return "";
            }
            if (Console.IsInputRedirected)
            {
                Console.WriteLine(linea); 
            }
            return linea;
        }

        public static char LeerTecla()
        {
            if (Console.IsInputRedirected)
            {
                string linea = LeerLinea();
                if (FinDeEntrada)
                {
                    return 'q';
                }
                return linea.Length == 0 ? '\n' : char.ToLower(linea[0]);
            }

            ConsoleKeyInfo tecla = Console.ReadKey(true);
            if (tecla.Key == ConsoleKey.Enter || tecla.Key == ConsoleKey.Spacebar)
            {
                return '\n';
            }
            if (tecla.Key == ConsoleKey.Escape)
            {
                return 'q';
            }
            return char.ToLower(tecla.KeyChar);
        }

        public static bool HayTecla() => !Console.IsInputRedirected && Console.KeyAvailable;

        public static int PedirNumero(string pregunta)
        {
            OcultarCursor(false);
            Escribir($" {pregunta} ");
            string linea = LeerLinea();
            OcultarCursor(true);
            return int.TryParse(linea.Trim(), out int n) && n > 0 ? n : 0;
        }

        // Pantallas (gracias chat por)
        public static void DibujarIteracion(Simulador sim, SimConfig config, ArrayList<Parte> eventos, int iteracionesAvanzadas, string controles)
        {
            Limpiar();

            string fase;
            if (sim.Iteracion == 0)
            {
                fase = $"Iteración 0/{config.IteracionesLlegada} · lista para empezar";
            }
            else if (sim.Iteracion <= config.IteracionesLlegada)
            {
                fase = $"Iteración {sim.Iteracion}/{config.IteracionesLlegada} · llegando clientes";
            }
            else
            {
                fase = $"Iteración {sim.Iteracion} · vaciando (ya no entran clientes)";
            }

            new Renglon().Add(" SimMarket ", ConsoleColor.Cyan).Add("─ " + fase + " ─ ").Add(TextoSemilla(config.Semilla), ConsoleColor.DarkGray).Imprimir();
            Regla();

            int listos = sim.ClientesEnMercado - sim.ClientesComprando;
            new Renglon()
                .Add(" En tienda ", ConsoleColor.DarkGray).Add($"{sim.ClientesEnTienda}")
                .Add(" · Comprando ", ConsoleColor.DarkGray).Add($"{sim.ClientesComprando}")
                .Add(" · Listos ", ConsoleColor.DarkGray).Add($"{listos}")
                .Add(" · En filas ", ConsoleColor.DarkGray).Add($"{sim.ClientesEnFilas}")
                .Add(" · Atendidos ", ConsoleColor.DarkGray).Add($"{sim.TotalAtendidos}/{sim.TotalLlegados}", ConsoleColor.Green)
                .Add(" · Abandonaron ", ConsoleColor.DarkGray).Add($"{sim.ClientesAbandonaron}", sim.ClientesAbandonaron > 0 ? ConsoleColor.Red : null)
                .Imprimir();

            // Cajas: una linea con sus datos y abajo su fila
            EscribirLinea();
            new Renglon().Add(" CAJAS", ConsoleColor.Yellow).Add("   ▶ cobrando · (n) artículos por cobrar", ConsoleColor.DarkGray).Imprimir();
            for (int i = 0; i < sim.NumCajas; i++)
            {
                Caja caja = sim.ObtenerCaja(i);
                var renglon = new Renglon().Add($" Caja {caja.Id,-2} ");
                if (caja.EsRapida)
                {
                    renglon.Add($"RÁPIDA ≤{caja.LimiteArticulos,-3}", ConsoleColor.Green);
                }
                else
                {
                    renglon.Add("NORMAL".PadRight(11), ConsoleColor.Blue);
                }
                renglon
                    .Add($" cap {caja.CapacidadCobro}/it  ", ConsoleColor.DarkGray)
                    .Add(Barra(caja.ClientesEnEspera, caja.MaxClientes, Math.Min(caja.MaxClientes, 10)), caja.Llena ? ConsoleColor.Red : ConsoleColor.Green)
                    .Add($" {caja.ClientesEnEspera}/{caja.MaxClientes}".PadRight(7))
                    .Add($"espera ~{caja.TiempoEsperaEstimado} it  atendidos {caja.ClientesAtendidos}", ConsoleColor.DarkGray)
                    .Imprimir();

                if (caja.Desocupada)
                {
                    EscribirLinea("     (vacía)", ConsoleColor.DarkGray);
                    continue;
                }

                var fila = new ArrayList<Parte>();
                for (int j = 0; j < caja.ClientesEnEspera; j++)
                {
                    Cliente c = caja.ClienteEnPosicion(j);
                    string texto = (j == 0 ? "▶ " : "· ") + $"#{c.Id}{Pref(c)} ({c.ArticulosPorCobrar})";
                    fila.Add(new Parte(texto, c.EsPreference ? ConsoleColor.Yellow : null));
                }
                ImprimirBloques(fila, "     ", 2);
            }

            // Mercado: los que siguen comprando o ya estan listos para formarse
            EscribirLinea();
            new Renglon().Add($" MERCADO ({sim.ClientesEnMercado})", ConsoleColor.Yellow).Add("   barra = artículos recolectados", ConsoleColor.DarkGray).Imprimir();
            if (sim.ClientesEnMercado == 0)
            {
                EscribirLinea("   (nadie comprando)", ConsoleColor.DarkGray);
            }
            else
            {
                var mercado = new ArrayList<Parte>();
                for (int i = 0; i < sim.ClientesEnMercado; i++)
                {
                    Cliente c = sim.ObtenerClienteEnMercado(i);
                    if (c.Estado == Cliente.EstadoCliente.ListoParaCaja)
                    {
                        mercado.Add(new Parte($"#{c.Id}{Pref(c)} ✓ listo", ConsoleColor.Green));
                    }
                    else
                    {
                        string texto = $"#{c.Id}{Pref(c)} {Barra(c.ArticulosRecolectados, c.ArticulosTotales, 5)} {c.ArticulosRecolectados}/{c.ArticulosTotales}";
                        mercado.Add(new Parte(texto, c.EsPreference ? ConsoleColor.Yellow : null));
                    }
                }
                ImprimirBloques(mercado, "   ", 4);
            }

            // Eventos del ultimo avance
            EscribirLinea();
            EscribirLinea(iteracionesAvanzadas > 1 ? $" EVENTOS (últimas {iteracionesAvanzadas} iteraciones)" : " EVENTOS", ConsoleColor.Yellow);
            if (eventos.Empty)
            {
                EscribirLinea("   (sin eventos)", ConsoleColor.DarkGray);
            }
            else
            {
                ImprimirBloques(eventos, "   ", 3);
            }

            EscribirLinea();
            Regla();
            EscribirLinea(" " + controles);
        }

        public static void DibujarResumen(Simulador sim, int? semilla, double promedioEnTienda, int maximoEnTienda, bool interrumpida)
        {
            Limpiar();
            new Renglon().Add(" SimMarket ", ConsoleColor.Cyan).Add(interrumpida ? "─ Resumen (simulación interrumpida)" : "─ Resumen final").Imprimir();
            Regla();

            new Renglon().Add(" Iteraciones ", ConsoleColor.DarkGray).Add($"{sim.Iteracion}").Add("   " + TextoSemilla(semilla), ConsoleColor.DarkGray).Imprimir();

            int llegados = sim.TotalLlegados;
            new Renglon()
                .Add(" Llegaron ", ConsoleColor.DarkGray).Add($"{llegados}")
                .Add("   Atendidos ", ConsoleColor.DarkGray).Add($"{sim.TotalAtendidos} ({Porcentaje(sim.TotalAtendidos, llegados)})", ConsoleColor.Green)
                .Add("   Abandonaron ", ConsoleColor.DarkGray).Add($"{sim.ClientesAbandonaron} ({Porcentaje(sim.ClientesAbandonaron, llegados)})", sim.ClientesAbandonaron > 0 ? ConsoleColor.Red : null)
                .Imprimir();

            if (interrumpida)
            {
                new Renglon().Add(" Siguen en tienda ", ConsoleColor.DarkGray).Add($"{sim.ClientesEnTienda}").Imprimir();
            }

            var tiempo = new Renglon().Add(" Tiempo en tienda (atendidos): ", ConsoleColor.DarkGray);
            if (sim.TotalAtendidos > 0)
            {
                tiempo.Add($"promedio {promedioEnTienda:0.0} it · máximo {maximoEnTienda} it");
            }
            else
            {
                tiempo.Add("nadie ha terminado de pagar");
            }
            tiempo.Imprimir();

            EscribirLinea();
            EscribirLinea(" Caja  Tipo     Cap/it  Atendidos  Artículos", ConsoleColor.Yellow);
            for (int i = 0; i < sim.NumCajas; i++)
            {
                Caja caja = sim.ObtenerCaja(i);
                string tipo = caja.EsRapida ? "RÁPIDA" : "NORMAL";
                EscribirLinea($" {caja.Id,4}  {tipo,-7}  {caja.CapacidadCobro,6}  {caja.ClientesAtendidos,9}  {caja.ArticulosProcesados,9}");
            }

            EscribirLinea();
            Regla();
            EscribirLinea(interrumpida ? " [c] continuar  [r] reiniciar  [q] salir" : " [r] reiniciar  [q] salir");
        }

        // Acomoda pedazos uno tras otro y salta de renglon cuando ya no caben (maximo maxLineas renglones)
        private static void ImprimirBloques(ArrayList<Parte> bloques, string sangria, int maxLineas)
        {
            int ancho = Ancho;
            int lineas = 0;
            var renglon = new Renglon().Add(sangria);

            for (int i = 0; i < bloques.Size; i++)
            {
                Parte bloque = bloques[i];
                bool vacio = renglon.Largo == sangria.Length;

                if (!vacio && renglon.Largo + 2 + bloque.Texto.Length > ancho)
                {
                    if (lineas == maxLineas - 1)
                    {
                        renglon.Add($"  … y {bloques.Size - i} más", ConsoleColor.DarkGray).Imprimir();
                        return;
                    }
                    renglon.Imprimir();
                    lineas++;
                    renglon = new Renglon().Add(sangria);
                    vacio = true;
                }

                if (!vacio)
                {
                    renglon.Add("  ");
                }
                renglon.Add(bloque.Texto, bloque.Color);
            }

            if (renglon.Largo > sangria.Length)
            {
                renglon.Imprimir();
            }
        }
    }

    // Un renglon armado por pedazos de colores; si no cabe en la terminal se recorta con "…"
    internal class Renglon
    {
        private readonly ArrayList<Parte> partes = new ArrayList<Parte>();

        public int Largo { get; private set; }

        public Renglon Add(string texto, ConsoleColor? color = null)
        {
            partes.Add(new Parte(texto, color));
            Largo += texto.Length;
            return this;
        }

        public void Imprimir()
        {
            int ancho = Cli.Ancho;
            int usado = 0;

            for (int i = 0; i < partes.Size; i++)
            {
                Parte parte = partes[i];
                if (usado + parte.Texto.Length > ancho)
                {
                    int cabe = ancho - usado - 1;
                    if (cabe > 0)
                    {
                        Cli.Escribir(parte.Texto.Substring(0, cabe), parte.Color);
                    }
                    Cli.Escribir("…", ConsoleColor.DarkGray);
                    break;
                }
                Cli.Escribir(parte.Texto, parte.Color);
                usado += parte.Texto.Length;
            }
            Console.WriteLine();
        }
    }
}
