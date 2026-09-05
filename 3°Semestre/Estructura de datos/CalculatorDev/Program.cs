using System; 
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using static System.Console;

namespace StackDev    
{
    internal class Program
    {
        //Diccionarios
        Dictionary<char, int> operadores = new Dictionary<char, int>()
            {
                { '+', 1 },
                { '-', 1 },
                { '*', 2 },
                { '/', 2 },
                { '^', 3 }
            };

        Dictionary<char, double> variables = new Dictionary<char, double>()
            {
                {'a',0 },
                {'b',0 },
                {'c',0 },
                {'d',0 },
                {'e',0 },
                {'f',0 },
            };

        static Dictionary<char, char> parejas = new Dictionary<char, char>()
            {
                { ')', '(' },
                { ']', '[' },
                { '}', '{' },
               // { '(', ')' },
                //{ '[', ']' },
                //{ '{', '}' },
            };




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

        //Aceptar mas parentesis


        static string Normalize(string expression)
        {
            string texto = expression;
            expression = texto.Replace(" ", "");
            return expression;
        }



        static bool CheckParentesis(string expression)
        {
            IStack<char> stack = new ArrayStack<char>(10);

            if (expression.Length == 0) { throw new ArgumentNullException("Nada"); }

            for (int i = 0; i < expression.Length; i++)
            {
                char caracter = expression[i];
                if (parejas.ContainsKey(caracter))
                {
                    if (stack.Pop() == parejas[caracter]) 
                    {
                        continue;
                    }
                    else
                    {
                        throw new Exception("No es pareja");
                    }
                }
                else
                {
                    if (parejas.ContainsValue(caracter))
                    {
                        stack.Push(caracter);
                    }
                }


            }

            if (stack.Empty) { return true; }
            else { return false; }
                
        }

        /*
        string ConvertToRPM(string expression)
        {
            IStack<char> stack = new ArrayStack<char>(10);
           // string operadores = "+-*
        /*    string texto = expression;
            expression = texto.Replace(" ", "");

            for (int i = 0; i < expression.Length; i++)
            {
                string output;
                char caracter = expression[i];
                if (char.IsDigit(caracter))
                {
                    WriteLine(output += caracter);
                }
                else if(caracter == ')')
                {
                    stack.Pop();
                }
                else if(caracter == '(' || operadores.Contains(caracter))
                {
                    stack.Push(caracter);

                }
            }
        }  

        */


        static void Main(string[] args)
        {

            string expression1 = "((a+b) -y*(4+2))";
            string papu = Normalize(expression1);
            WriteLine(CheckParentesis(papu));


        }


    }
}
