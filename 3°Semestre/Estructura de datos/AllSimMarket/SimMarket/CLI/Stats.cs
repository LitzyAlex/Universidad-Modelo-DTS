using System;
using System.Threading;
using ListDev;    

namespace SimMarket.CLI
{
    // La sesion en consola: el loop principal, las teclas, los eventos de cada iteracion y las estadisticas
    internal class Stats
    {
        private const string Controles = "[Enter] siguiente  [n] avanzar N  [a] auto  [f] hasta el final  [r] reiniciar  [q] salir";

        private readonly SimConfig config;
        private readonly ArrayList<Parte> eventos = new ArrayList<Parte>(); // lo que paso en el ultimo avance
        private Simulador sim;

        private int iteracionesUltimoAvance;
        private int sumaTiempoEnTienda;      // de los clientes que ya pagaron
        private int tiempoMaximoEnTienda;
        private int velocidadAutoMs = 500;

        public Stats(SimConfig config)
        {
            this.config = config;
            sim = CrearSimulador();
        }

        // Crea un simulador nuevo con la misma config (y semilla) y reinicia las estadisticas
        private Simulador CrearSimulador()
        {
            eventos.Clear();
            iteracionesUltimoAvance = 0;
            sumaTiempoEnTienda = 0;
            tiempoMaximoEnTienda = 0;

            var s = new Simulador(config);
            s.ClienteLlego += c => eventos.Add(new Parte($"+ #{c.Id}{Cli.Pref(c)} entró", ConsoleColor.DarkGray));
            s.ClienteSeFormo += (c, caja) => eventos.Add(new Parte($"→ #{c.Id} a caja {caja.Id}", ConsoleColor.Green));
            s.ClienteAbandono += c => eventos.Add(new Parte($"✗ #{c.Id} abandonó", ConsoleColor.Red));
            s.ClienteSalio += (c, caja) =>
            {
                eventos.Add(new Parte($"← #{c.Id} pagó en caja {caja.Id}", ConsoleColor.Cyan));
                int tiempo = c.IteracionSalida - c.IteracionLlegada;
                sumaTiempoEnTienda += tiempo;
                tiempoMaximoEnTienda = Math.Max(tiempoMaximoEnTienda, tiempo);
            };
            return s;
        }

        public void Ejecutar()
        {
            while (true)
            {
                if (sim.Terminada)
                {
                    if (!MostrarResumen(false))
                    {
                        return;
                    }
                    continue;
                }

                Cli.DibujarIteracion(sim, config, eventos, iteracionesUltimoAvance, Controles);

                switch (Cli.LeerTecla())
                {
                    case '\n':
                        Avanzar(1);
                        break;
                    case 'n':
                        int n = Cli.PedirNumero("¿Cuántas iteraciones?");
                        if (n > 0)
                        {
                            Avanzar(n);
                        }
                        break;
                    case 'a':
                        Automatico();
                        break;
                    case 'f':
                        Avanzar(int.MaxValue); // se detiene solo cuando la simulacion termina
                        break;
                    case 'r':
                        sim = CrearSimulador();
                        break;
                    case 'q':
                        if (!MostrarResumen(true))
                        {
                            return;
                        }
                        break;
                }
            }
        }

        private void Avanzar(int n)
        {
            eventos.Clear();
            iteracionesUltimoAvance = 0;
            while (iteracionesUltimoAvance < n && !sim.Terminada)
            {
                sim.EjecutarIteracion();
                iteracionesUltimoAvance++;
            }
        }

        private void Automatico()
        {
            while (!sim.Terminada)
            {
                Avanzar(1);
                Cli.DibujarIteracion(sim, config, eventos, iteracionesUltimoAvance,
                    $"AUTO cada {velocidadAutoMs} ms   [+] más rápido  [-] más lento  [otra tecla] pausar");

                for (int espera = 0; espera < velocidadAutoMs; espera += 50)
                {
                    if (Cli.HayTecla())
                    {
                        char tecla = Cli.LeerTecla();
                        if (tecla == '+' || tecla == '=')
                        {
                            velocidadAutoMs = Math.Max(50, velocidadAutoMs / 2);
                        }
                        else if (tecla == '-')
                        {
                            velocidadAutoMs = Math.Min(4000, velocidadAutoMs * 2);
                        }
                        else
                        {
                            return;
                        }
                    }
                    Thread.Sleep(50);
                }
            }
        }

        private bool MostrarResumen(bool interrumpida)
        {
            double promedio = sim.TotalAtendidos > 0 ? (double)sumaTiempoEnTienda / sim.TotalAtendidos : 0;
            Cli.DibujarResumen(sim, config.Semilla, promedio, tiempoMaximoEnTienda, interrumpida);

            while (true)
            {
                char tecla = Cli.LeerTecla();
                if (tecla == 'r')
                {
                    sim = CrearSimulador();
                    return true;
                }
                if (tecla == 'c' && interrumpida)
                {
                    return true;
                }
                if (tecla == 'q')
                {
                    return false;
                }
            }
        }
    }
}
