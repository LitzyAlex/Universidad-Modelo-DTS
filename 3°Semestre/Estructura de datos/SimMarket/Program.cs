using System;

namespace SimMarket
{
    // Version de consola para probar la logica sin Unity
    // Enter = siguiente iteracion         q = salir
    internal class Program
    {
        static void Main()
        {
            
            var sim = new Simulador(new SimConfig { Semilla = 1 });
            
            sim.ClienteSeFormo += (c, caja) => Console.WriteLine($"   -> Cliente #{c.Id}{(c.EsPreference ? " [PREF]" : "")} ({c.ArticulosTotales} art.) se forma en caja {caja.Id}");
            sim.ClienteSalio += (c, caja) => Console.WriteLine($"   <- Cliente #{c.Id} sale de caja {caja.Id}");
            sim.ClienteAbandono += c => Console.WriteLine($"   xx Cliente #{c.Id} abandona la compra (cajas llenas)");

            while (true)
            {
                sim.EjecutarIteracion();
                Console.WriteLine($"\n=== Iteración {sim.Iteracion} | en mercado: {sim.ClientesEnMercado} | comprando: {sim.ClientesComprando} | en filas: {sim.ClientesEnFilas} | atendidos: {sim.TotalAtendidos}/{sim.TotalLlegados} | abandonaron: {sim.ClientesAbandonaron} ===");

                // Clientes que siguen en el mercado (comprando o listos para formarse)
                Console.WriteLine("Clientes en el mercado:");
                for (int i = 0; i < sim.ClientesEnMercado; i++) Console.WriteLine("   " + sim.ObtenerClienteEnMercado(i));

                // Cajas con toda su fila
                for (int i = 0; i < sim.NumCajas; i++)
                {
                    Caja caja = sim.ObtenerCaja(i);
                    Console.WriteLine(caja);
                    for (int j = 0; j < caja.ClientesEnEspera; j++) Console.WriteLine("      " + (j == 0 ? "[atendiendo] " : $"[{j}] ") + caja.ClienteEnPosicion(j));
                }

                if (sim.Terminada)
                {
                    Console.WriteLine("\nLa simulación terminó.");
                    break;
                }
                
                Console.Write("\n[Enter] siguiente, [e] salir: ");
                if (Console.ReadLine()?.Trim().ToLower() == "e") break;
            }
            

        }

       
    }
}
