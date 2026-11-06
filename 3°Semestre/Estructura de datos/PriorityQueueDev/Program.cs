using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using static System.Console;

namespace PriorityQueueDev
{
    internal class Program
    {
        static void Main(string[] args)
        {
           PriorityQueue<int> queue = new PriorityQueue<int>();
            queue.Enqueue(10, 5);
            queue.Enqueue(57, 0);
            queue.Enqueue(69, 1);
            queue.Enqueue(20, 1);
            queue.Enqueue(93, 7);
            queue.Enqueue(11, 2);
            queue.Mostrar();

            Console.Write($"Elimina el elemento de mayor prioridad(menor numero): {queue.Dequeue()}\n");
            queue.Mostrar();

            Console.Write($"Elemento de mayor prioridad(menor numero): {queue.Head()}\n");
            Console.Write($"Elemento de menor prioridad(mayor numero): {queue.Tail()}\n");



        }
    }
}