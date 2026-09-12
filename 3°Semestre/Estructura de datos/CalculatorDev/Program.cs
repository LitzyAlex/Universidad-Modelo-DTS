using System; 
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using static System.Console;

namespace StackDev    
{
    internal class Program
    {
        //Diccionarios
        static Dictionary<char, int> operadores = new Dictionary<char, int>()
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


        static List<string> ConvertToRPN(string expression)
        {
            IStack<char> stack = new ArrayStack<char>(10);
            List<string> output = new List<string>();

            for (int i = 0; i < expression.Length; i++)
            {
                char caracter = expression[i];
                if (char.IsLetterOrDigit(caracter))
                {
                    
                    string numero = "";
                    while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.')) //verifica si estoy dentro de la expresion y si el caracter donde estoy es un numero y lo agrega
                    {
                        numero = numero + expression[i];
                        i++;
                    }

                    output.Add(numero);

                    i--;  //le restamos lo que agregamos en la ultima vuelta del while :)
                    
                }

                else if (parejas.ContainsKey(caracter)) //si contiene los valores de key ")"
                {
                    char pop;
                    bool end = false;
                    do
                    {
                        if (stack.Empty) { break;}
                        pop = stack.Pop();
                        if (pop == parejas[caracter])
                        {
                            end = true;
                        }
                        else
                        {
                            output.Add(pop.ToString());
                        }

                    } while (end==false);

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
                            {
                                output.Add(stack.Pop().ToString());
                            }

                        }
                        stack.Push(caracter);
                    }
                }
            }
            while(stack.Empty == false)
            {
                output.Add(stack.Pop().ToString());
            }
            return output;

        }

        static List<double> ExecutorOp(List<string> papu)
        {
            List<double> stack3 = new List<double>(5);

            foreach (string elemento in papu)
            {
                bool operador = Program.operadores.ContainsKey(elemento[0]);

                if (operador == true)
                {
                    double total = 0;
                    int i = (stack3.Count) - 1;

                    char op = elemento[0];

                    switch (op)
                    {
                        case '+':
                            WriteLine();
                            WriteLine("Elemento actual: " + op);

                            total = stack3[i - 1] + stack3[i];
                            stack3[i - 1] = total;
                            stack3.RemoveAt(i);

                            WriteLine("Resultado: " + total);
                            Write(stack3[i - 1] + " ");
                            WriteLine();

                            break;
                        case '-':
                            total = stack3[i - 1] - stack3[i];
                            stack3[i - 1] = total;
                            stack3.RemoveAt(i);

                            WriteLine("Resultado: " + total);
                            Write(stack3[i - 1] + " ");
                            WriteLine();

                            break;

                        case '*':
                            WriteLine();
                            WriteLine("Elemento actual: " + op);

                            total = stack3[i - 1] * stack3[i];
                            stack3[i - 1] = total;
                            stack3.RemoveAt(i);

                            WriteLine("Resultado: " + total);
                            Write(stack3[i - 1] + " ");
                            WriteLine();

                            break;

                        case '/':
                            WriteLine();
                            WriteLine("Elemento actual: " + op);

                            total = stack3[i - 1] / stack3[i];
                            stack3[i - 1] = total;
                            stack3.RemoveAt(i);

                            WriteLine("Resultado: " + total);
                            Write(stack3[i - 1] + " ");
                            WriteLine();

                            break;

                        case '^':
                            WriteLine();
                            WriteLine("Elemento actual: " + op);

                            total = Math.Pow(stack3[i - 1], stack3[i]);
                            stack3[i - 1] = total;
                            stack3.RemoveAt(i);

                            WriteLine("Resultado: " + total);
                            Write(stack3[i - 1] + " ");
                            WriteLine();

                            break;
                    }

                } else
                {
                    double numero = double.Parse(elemento);
                    stack3.Add(numero);
                    Write(numero + " ");
                }

            }
        return stack3;
        }
      


        static void Main(string[] args)
        {

            string expression1 = "{5+(8-2)}^2 *10.5";
            string normalize = Normalize(expression1);
            WriteLine(CheckParentesis(normalize));
            List<string> papu = ConvertToRPN(normalize);
            foreach (string elemento in papu)
            {
                Write(elemento + " ");
            }

            WriteLine();
            WriteLine();
            WriteLine("Obteniendo resultado...");
            WriteLine();
            ExecutorOp(papu);



        }

        
    }
}
