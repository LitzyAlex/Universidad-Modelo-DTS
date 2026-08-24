using System.Runtime.InteropServices;

class Interfaz
{
    public Jugador Crear()
    {
        Console.WriteLine("******************JUEGO****************");
        Console.WriteLine("Agregar jugador");
        do
        {
            Console.Write("Nombre: ");
            string nombre = Console.ReadLine()??"";

            Console.Write("Vida: ");
            int vida = int.Parse(Console.ReadLine()!);

            Console.Write("Ataque: ");
            int ataque = int.Parse(Console.ReadLine()!);

            Console.Write("Nivel: ");
            int nivel = int.Parse(Console.ReadLine()!);

            new Jugador(nombre, vida, ataque, nivel);

            Console.Write("Desea agregar otro jugador? (s/n):");
            string option = Console.ReadLine()??"";
        

        }while(option == "s");
    }




}