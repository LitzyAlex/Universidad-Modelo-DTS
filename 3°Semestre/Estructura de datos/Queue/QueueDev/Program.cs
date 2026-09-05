using System; 
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using static System.Console;   

namespace QueueDev
{
    internal class Program
    {
        
        static void PrintQueueStatus<T> (StatickQueue<T> queue)
        {
            #if DEBUG //Es te if es del procesador de texto
            WriteLine(queue.DataPeek());
            #endif

            WriteLine($"S: {queue.Size}, E: {queue.Empty}, F: {queue.Full}");
            WriteLine();
        }

        static void PrintQueueStatus<T> (ArrayQueue<T> queue)
        {
            #if DEBUG //Es te if es del procesador de texto
            WriteLine(queue.DataPeek());
            #endif

            WriteLine($"S: {queue.Size}, E: {queue.Empty}, F: {queue.Full}");
            WriteLine();
        }
        

        static async Task Main(string[] args)
        {
            StatickQueue<double> queue = new StatickQueue<double>(5);

            WriteLine("////////////// Statick Queue //////////////");
            PrintQueueStatus(queue);
            queue.Enqueue(-5);

            PrintQueueStatus(queue);
            queue.Enqueue(10);
            queue.Enqueue(22);

            PrintQueueStatus(queue);

            WriteLine($"Head: {queue.Head()}");
            WriteLine($"Tail: {queue.Tail()}");
            WriteLine($"Dequeue: {queue.Dequeue()}\n");

            PrintQueueStatus(queue);

            queue.Enqueue(0);
            queue.Enqueue(-1);
            queue.Enqueue(55);

            PrintQueueStatus(queue);

            ArrayQueue<double> array_queue = new ArrayQueue<double>();

            WriteLine("////////////// Array Queue //////////////");
            WriteLine();
            WriteLine("////////////// Constructor SIN parametro //////////////");
            PrintQueueStatus(array_queue);
            array_queue.Enqueue(-5);

            PrintQueueStatus(array_queue);
            array_queue.Enqueue(10);
            array_queue.Enqueue(22);

            PrintQueueStatus(array_queue);

            WriteLine($"Head: {array_queue.Head()}");
            WriteLine($"Tail: {array_queue.Tail()}");
            WriteLine($"Dequeue: {array_queue.Dequeue()}\n");

            PrintQueueStatus(array_queue);

            array_queue.Enqueue(0);
            array_queue.Enqueue(-1);
            array_queue.Enqueue(55);

            PrintQueueStatus(array_queue);

            ArrayQueue<double> array_queue2 = new ArrayQueue<double>(5);

            WriteLine("////////////// Array Queue //////////////");
            WriteLine();
            WriteLine("////////////// Constructor CON parametro //////////////");
            PrintQueueStatus(array_queue2);
            array_queue2.Enqueue(-5);

            PrintQueueStatus(array_queue2);
            array_queue2.Enqueue(10);
            array_queue2.Enqueue(22);

            PrintQueueStatus(array_queue2);

            WriteLine($"Head: {array_queue2.Head()}");
            WriteLine($"Tail: {array_queue2.Tail()}");
            WriteLine($"Dequeue: {array_queue2.Dequeue()}\n");

            PrintQueueStatus(array_queue2);

            array_queue2.Enqueue(0);
            array_queue2.Enqueue(-1);
            array_queue2.Enqueue(55);

            PrintQueueStatus(array_queue2);

            ArrayQueue<double> array_queue3 = new ArrayQueue<double>(array_queue);

            WriteLine("////////////// Array Queue //////////////");
            WriteLine();
            WriteLine("////////////// Constructor COPIA //////////////");
            PrintQueueStatus(array_queue3);

            WriteLine($"Head: {array_queue3.Head()}");
            WriteLine($"Tail: {array_queue3.Tail()}");
            WriteLine($"Dequeue: {array_queue3.Dequeue()}\n");

            PrintQueueStatus(array_queue3);

            array_queue3.Enqueue(10);

            PrintQueueStatus(array_queue3);

            array_queue3.Enqueue(20);

            PrintQueueStatus(array_queue3);

            Console.WriteLine(array_queue3.Capacity);
        }
    }
}

