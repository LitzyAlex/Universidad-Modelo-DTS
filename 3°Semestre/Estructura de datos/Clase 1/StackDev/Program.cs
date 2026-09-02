using System; //Se usa para ocupar de otros codigos
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using static System.Console;   //Para no escribir todo eso, solo para las clases estaticas

namespace StackDev    //Es el apellido de la variable
{
    internal class Program
    {
        static void PrintStackStatus<T>(IStack<T> stack)
        {
            #if DEBUG //Es te if es del procesador de texto
            WriteLine(stack.DataPeek());
            #endif

            if (stack is ArrayStack<T>)
            {
                ArrayStack<T> astack = stack as ArrayStack<T>;
                WriteLine($"C: {astack.Capacity}, S: {stack.Size}, E: {stack.Empty}, F: {stack.Full}");
            }
            else
            {
                WriteLine($"S: {stack.Size}, E: {stack.Empty}, F: {stack.Full}");
            }
            
            WriteLine();
        }

        static void Main(string[] args)
        {
            WriteLine("StatickStack");
            IStack<double> stack = new ArrayStack<double>(5);

            PrintStackStatus(stack);
            stack.Push(-5.25);

            PrintStackStatus(stack);
            stack.Push(10);
            stack.Push(22.6);

            PrintStackStatus(stack);

            WriteLine($"Peek: {stack.Peek()}");
            WriteLine($"Pop: {stack.Pop()}");

            stack.Push(0.5);
            stack.Push(-1.78);
            stack.Push(55.22);
            stack.Push(47.05);

            PrintStackStatus(stack);

            WriteLine($"Pop: {stack.Pop()}");
            WriteLine($"Pop: {stack.Pop()}");
            WriteLine($"Pop: {stack.Pop()}");
            WriteLine($"Pop: {stack.Pop()}");

            PrintStackStatus(stack);

        }
    }
}
