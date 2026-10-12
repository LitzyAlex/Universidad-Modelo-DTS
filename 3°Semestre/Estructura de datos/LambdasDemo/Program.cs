using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using static System.Console;

namespace LambdasDemo
{
    internal class Program
    {
        delegate double Operation(int a, int b);
            

        static double Suma(int a, int b)
        {
            return a + b;
        }

        static double Resta(int a, int b)
        {
            return a - b;
        }

        static double DoOperation(int a, int b, Operation op)
        {
            return op(a, b);
        }



        static void Main(string[] args)
        {

            int m = 10;
            int n = 43;
            Operation op = Suma;

            WriteLine($"m + n: {op(m, n)}");
            op = Resta;
            WriteLine($"m + n: {op(m, n)}");

            Write("\n");
            WriteLine($"m + n: {DoOperation(m, n, Suma)}");
            WriteLine($"m - n: {DoOperation(m, n, Resta)}");

            Write("\n");
            WriteLine($"m + n: {DoOperation(m, n, (a,b) => a + b)}");
            WriteLine($"m - n: {DoOperation(m, n, (a,b) => a - b)}");
            WriteLine($"m * n: {DoOperation(m, n, (a,b) => a * b)}");
            WriteLine($"m / n: {DoOperation(m, n, (a,b) => b / a)}");


            //WriteLine($"m + n: {Suma(m, n)}");
            //WriteLine($"m - n: {Resta(m, n)}");


        }
    }
}
