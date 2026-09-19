using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace StackDev
{

    internal class Program2
    {
        static void Mover(int n_discos, 
            IStack<int> A, int torreA, 
            IStack<int> C, int torreC,
            IStack<int> B, int torreB)
        {
            if (n_discos == 1)
            {
                C.Push(A.Pop());
                Imprimir(torreA, torreC);
            } else
            { 
                Mover(n_discos - 1, A, torreA, B, torreB, C, torreC);
                Imprimir(torreA, torreC);
                Mover(n_discos - 1, B, torreB, C, torreC, A, torreA);
            }
        }

        static void Imprimir(int A, int C)
        {
            WriteLine($"{A} -> {C}");
        }

        static void Main(string[] args)
        {
            WriteLine("Torre Hanoi recursiva");
            WriteLine("Ingrese el numero de discos:");

            int discos = Convert.ToInt32(ReadLine());

            IStack<int> uno = new StaticStack<int>(discos, 1);
            IStack<int> dos = new StaticStack<int>(discos);
            IStack<int> tres = new StaticStack<int>(discos);

            Mover(discos, uno, tres, dos);

            WriteLine("\nListo :)");

            PrintStackStatus(uno);
            PrintStackStatus(dos);
            PrintStackStatus(tres);
        }

    }
}
