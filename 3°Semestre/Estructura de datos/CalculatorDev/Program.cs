using System; 
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Text;
using System.Threading.Tasks;

using static System.Console;

namespace StackDev    
{
    internal class Program
    {
        //Diccionarios
        internal static Dictionary<char, int> operadores = new Dictionary<char, int>()
            {
                { '+', 1 },
                { '-', 1 },
                { '*', 2 },
                { '/', 2 },
                { '^', 3 }
            };

        Dictionary<char, double> variables = new Dictionary<char, double>(); //debe estar vacio al inicio

        static Dictionary<char, char> parejas = new Dictionary<char, char>()
            {
                { ')', '(' },
                { ']', '[' },
                { '}', '{' },
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


        internal static string Normalize(string expression)
        {
            string texto = expression;
            expression = texto.Replace(" ", "").ToLowerInvariant();
            return expression;
        }



        internal static bool CheckParentesis(string expression)
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


        internal static List<string> ConvertToRPN(string expression)
        {
            IStack<char> stack = new ArrayStack<char>(10);
            List<string> output = new List<string>();

            for (int i = 0; i < expression.Length; i++)
            {
                char caracter = expression[i];

                // Es un negativo cuando:
                // -- Es e primer caracter de la expresion.
                // -- El caracter anterior es un operador (*-).
                // -- El caracter anterior es un parentesis de apertura.
                bool Negativo = caracter == '-' && (i == 0 || operadores.ContainsKey(expression[i - 1]) || parejas.ContainsValue(expression[i - 1]));

                if (Negativo)
                {
                    string numero = "-";
                    i++; // saltamos el '-' para empezar a leer los digitos
                    while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.'))
                    {
                        numero += expression[i];
                        i++;
                    }
                    output.Add(numero);
                    i--; // regresamos al -
                }
                else if (char.IsLetterOrDigit(caracter))
                {
                    string numero = "";
                    while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.'))
                    {
                        numero = numero + expression[i];
                        i++;
                    }

                    output.Add(numero);
                    i--;
                }
                else if (parejas.ContainsKey(caracter))
                {
                    char pop;
                    bool end = false;
                    do
                    {
                        if (stack.Empty) { break; }
                        pop = stack.Pop();
                        if (pop == parejas[caracter])
                        {
                            end = true;
                        }
                        else
                        {
                            output.Add(pop.ToString());
                        }
                    } while (end == false);
                }
                else
                {
                    if (parejas.ContainsValue(caracter))
                    {
                        stack.Push(caracter);
                    }
                    else
                    {
                        while (stack.Empty == false && (operadores.ContainsKey(stack.Peek()) && operadores[stack.Peek()] >= operadores[caracter]))
                        {
                            output.Add(stack.Pop().ToString());
                        }
                        stack.Push(caracter);
                    }
                }
            }

            while (stack.Empty == false)
            {
                output.Add(stack.Pop().ToString());
            }

            return output;
        }

        //el método se puede usar desde el mismo proyecto sin tener que crear un objeto
        //devuelve una lista de double, recibe el parámetro "papu" de strings
        internal static List<double> ExecutorOp(List<string> papu)
        {
            List<double> stack3 = new List<double>(5);  //crea stack3 de tamaño 5 para almacenar los números

            foreach (string elemento in papu)   //recorre los elementos de papu
            {
                //el elemento es de un caracter? este caracter existe como llave en el diccionario? 
                bool operador = elemento.Length == 1 && Program.operadores.ContainsKey(elemento[0]);

                if (operador == true)
                {
                    double total = 0;       //almacenar el total de la operación
                    //.count = cuántos elementos hay en la lista
                    int i = (stack3.Count) - 1; //el índice del último número del stack3

                    if (stack3.Count < 2) //si el stack3 no tiene mínimo 2 números es Error
                    {
                        WriteLine("Error");
                    } 
                    else 
                    {
                        char op = elemento[0];  //guarda el primer caracter del elemento en op

                        //imprimir stack actual
                        WriteLine();
                        foreach (double numero in stack3)
                        {
                            Write(numero + " ");
                        }
                        WriteLine();
                        WriteLine("Operador actual: " + op);
                        //

                        switch (op)
                        {
                            case '+':
                                total = stack3[i - 1] + stack3[i];
                                stack3[i - 1] = total;  //reemplaza el penúltimo número por el total calculado
                                stack3.RemoveAt(i);     //elimina el último número del stack

                                WriteLine("Resultado: " + total);

                                break;

                            case '-':
                                total = stack3[i - 1] - stack3[i];
                                stack3[i - 1] = total;
                                stack3.RemoveAt(i);

                                WriteLine("Resultado: " + total);

                                break;

                            case '*':
                                total = stack3[i - 1] * stack3[i];
                                stack3[i - 1] = total;
                                stack3.RemoveAt(i);

                                WriteLine("Resultado: " + total);

                                break;

                            case '/':
                                if (stack3[i] == 0) //validar división != 0
                                {
                                    WriteLine("Error división entre 0");
                                }
                                else
                                {
                                    total = stack3[i - 1] / stack3[i];
                                    stack3[i - 1] = total;
                                    stack3.RemoveAt(i);

                                    WriteLine("Resultado: " + total);
                                }

                                break;

                            case '^':
                                //Math clase de C# con funciones matemáticas
                                total = Math.Pow(stack3[i - 1], stack3[i]); //calcular potencia = (base, exponente)
                                stack3[i - 1] = total;
                                stack3.RemoveAt(i);

                                WriteLine("Resultado: " + total);

                                break;
                        }
                    }

                } else
                {
                    double numero = double.Parse(elemento); //convierte el string a double y lo nombra "numero"
                    stack3.Add(numero); //añade el numero al final del stack
                }

            }

        if (stack3.Count == 1)  //validar que solo haya un resultado final
            {
                return stack3;
            } else
            {
                WriteLine("Error al calcular el resultado");
                return stack3;
            }
        }
      


        static void Main(string[] args)
        {
            Cli.Help();
            while (true)
            {
                Write("[Papu@papu]# ");
                string input = Console.ReadLine();

                if (input == null)
                {
                    break;
                }

                Cli.Verifica(input);
            }
        }

        
    }
}