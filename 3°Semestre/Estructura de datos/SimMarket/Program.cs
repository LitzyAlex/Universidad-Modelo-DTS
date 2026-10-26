using System;
using System.Text;
using SimMarket.CLI;

namespace SimMarket
{
    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8; // para que se vean ▶ █ ░ ─ y los acentos
            Console.CancelKeyPress += (_, _) =>
            {
                Cli.OcultarCursor(false);
                Console.ResetColor();
            };

            SimConfig config = Menu.Pedir();

            Cli.OcultarCursor(true);
            try
            {
                new Stats(config).Ejecutar();
            }
            finally
            {
                Cli.OcultarCursor(false);
                Console.ResetColor();
            }
        }
    }
}
