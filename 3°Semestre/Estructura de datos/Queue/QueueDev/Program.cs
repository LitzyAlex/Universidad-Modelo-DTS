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
        

        static void Main(string[] args)
        {
            StatickQueue<double> queue = new StatickQueue<double>(5);

            PrintQueueStatus(queue);
            queue.Enqueue(-5);

            PrintQueueStatus(queue);
            queue.Enqueue(10);
            queue.Enqueue(22);

            PrintQueueStatus(queue);

            WriteLine($"Head: {queue.Head()}");
            WriteLine($"Tail: {queue.Tail()}");
            WriteLine($"Dequeue: {queue.Dequeue()}");
            PrintQueueStatus(queue);

            queue.Enqueue(0);
            queue.Enqueue(-1);
            queue.Enqueue(55);

            PrintQueueStatus(queue);

        }
    }
}

