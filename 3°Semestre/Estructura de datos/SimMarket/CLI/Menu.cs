using System;
using System.Globalization;

namespace SimMarket.CLI
{
    // Menu inicial: usar los valores por defecto, un preset o personalizar todo
    internal static class Menu
    {
        public static SimConfig Pedir()
        {
            Cli.Limpiar();
            new Renglon().Add(" SimMarket ", ConsoleColor.Cyan).Add("─ Simulador de supermercado").Imprimir();
            Cli.Regla();
            Cli.EscribirLinea(" 1) Valores por defecto   " + Describir(new SimConfig()));
            Cli.EscribirLinea(" 2) Tienda chica          " + Describir(SimConfig.TiendaChica()));
            Cli.EscribirLinea(" 3) Hora pico             " + Describir(SimConfig.HoraPico()));
            Cli.EscribirLinea(" 4) Supermercado          " + Describir(SimConfig.Supermercado()));
            Cli.EscribirLinea(" 5) Personalizar");
            Cli.EscribirLinea();

            int opcion = PedirEntero("Opción", 1, 1, 5);
            SimConfig config = opcion switch
            {
                2 => SimConfig.TiendaChica(),
                3 => SimConfig.HoraPico(),
                4 => SimConfig.Supermercado(),
                _ => new SimConfig() // default.
            };

            if (opcion == 5)
            {
                Personalizar(config);
            }

            config.Semilla = PedirSemilla();
            return config;
        }

        private static string Describir(SimConfig c) =>
            $"({c.NumCajas} cajas, {c.NumCajasRapidas} {(c.NumCajasRapidas == 1 ? "rápida" : "rápidas")}, {c.MaxClientesPorCaja} por caja, {c.IteracionesLlegada} it de llegada)";

        // Pregunta cada valor; si al final la config no es valida se dice por que y se vuelve a preguntar
        private static void Personalizar(SimConfig c)
        {
            while (true)
            {
                Cli.EscribirLinea();
                Cli.EscribirLinea(" Enter = dejar el valor entre corchetes", ConsoleColor.DarkGray);

                Cli.EscribirLinea(" Cajas", ConsoleColor.Yellow);
                c.NumCajas = PedirEntero("Total de cajas", c.NumCajas);
                c.NumCajasRapidas = PedirEntero("Cajas rápidas", c.NumCajasRapidas);
                c.LimiteArticulosRapida = PedirEntero("Límite de artículos en rápida", c.LimiteArticulosRapida);
                c.MinCapacidadCobro = PedirEntero("Capacidad de cobro mínima (art/it)", c.MinCapacidadCobro);
                c.MaxCapacidadCobro = PedirEntero("Capacidad de cobro máxima (art/it)", c.MaxCapacidadCobro);
                c.MaxClientesPorCaja = PedirEntero("Máximo de clientes por caja", c.MaxClientesPorCaja);

                Cli.EscribirLinea(" Clientes", ConsoleColor.Yellow);
                c.MaxLlegadasPorIteracion = PedirEntero("Llegadas máximas por iteración", c.MaxLlegadasPorIteracion);
                c.IteracionesLlegada = PedirEntero("Iteraciones con llegadas", c.IteracionesLlegada);
                c.MinArticulos = PedirEntero("Artículos por cliente (mín)", c.MinArticulos);
                c.MaxArticulos = PedirEntero("Artículos por cliente (máx)", c.MaxArticulos);
                c.MinIteracionesPorRecoleccion = PedirEntero("Recolecta cada N iteraciones (mín)", c.MinIteracionesPorRecoleccion);
                c.MaxIteracionesPorRecoleccion = PedirEntero("Recolecta cada N iteraciones (máx)", c.MaxIteracionesPorRecoleccion);
                c.MinArticulosPorRecoleccion = PedirEntero("Artículos por recolección (mín)", c.MinArticulosPorRecoleccion);
                c.MaxArticulosPorRecoleccion = PedirEntero("Artículos por recolección (máx)", c.MaxArticulosPorRecoleccion);
                c.ProbPreference = PedirDecimal("Probabilidad de Preference (0 a 1)", c.ProbPreference);

                try
                {
                    c.Validar();
                    return;
                }
                catch (ArgumentException e)
                {
                    // Sin entrada no hay forma de corregirlo, mejor que truene a que se cicle para siempre
                    if (Cli.FinDeEntrada)
                    {
                        throw;
                    }
                    Cli.EscribirLinea();
                    Cli.Error("✗ " + e.Message);
                    Cli.EscribirLinea(" Vuelve a revisar los valores:", ConsoleColor.DarkGray);
                }
            }
        }

        private static int PedirEntero(string etiqueta, int actual, int min = int.MinValue, int max = int.MaxValue)
        {
            while (true)
            {
                Cli.Escribir($"   {etiqueta} ");
                Cli.Escribir($"[{actual}]", ConsoleColor.DarkGray);
                Cli.Escribir(": ");

                string linea = Cli.LeerLinea().Trim();
                if (linea.Length == 0)
                {
                    return actual;
                }
                if (int.TryParse(linea, out int valor) && valor >= min && valor <= max)
                {
                    return valor;
                }

                bool conRango = min != int.MinValue || max != int.MaxValue;
                Cli.Error(conRango ? $"Escribe un número entero entre {min} y {max}" : "Escribe un número entero");
            }
        }

        // Acepta 0.1 o 0,1
        private static double PedirDecimal(string etiqueta, double actual)
        {
            while (true)
            {
                Cli.Escribir($"   {etiqueta} ");
                Cli.Escribir($"[{actual.ToString("0.##", CultureInfo.InvariantCulture)}]", ConsoleColor.DarkGray);
                Cli.Escribir(": ");

                string linea = Cli.LeerLinea().Trim().Replace(',', '.');
                if (linea.Length == 0)
                {
                    return actual;
                }
                if (double.TryParse(linea, NumberStyles.Float, CultureInfo.InvariantCulture, out double valor))
                {
                    return valor;
                }
                Cli.Error("Escribe un número decimal, por ejemplo 0.1");
            }
        }

        // Si no se escribe nada se inventa una al azar, asi siempre se muestra y se puede repetir la corrida
        private static int PedirSemilla()
        {
            Cli.EscribirLinea();
            while (true)
            {
                Cli.Escribir("   Semilla ");
                Cli.Escribir("[vacío = aleatoria]", ConsoleColor.DarkGray);
                Cli.Escribir(": ");

                string linea = Cli.LeerLinea().Trim();
                if (linea.Length == 0)
                {
                    return Random.Shared.Next(1, 100000);
                }
                if (int.TryParse(linea, out int semilla))
                {
                    return semilla;
                }
                Cli.Error("Escribe un número entero o deja vacío");
            }
        }
    }
}
