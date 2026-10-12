using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using static System.Console;   

namespace ListDev    
{
    internal class Program
    {

        static void Main(string[] args)
        {
            var list = new LinkedList<int>();
            list.Add(10);
            list.Add(-4);
            list.Add(5);
            list.Add(27);
            list.Add(-36);

            WriteLine();


            /*
            var list = new ArrayList<int>(new int[] { 1,-3, 5, 0, -20, 35, -53, 27, 72 });

            Write("\n[");
            list.ForEach(e => Write($"{e} "));
            WriteLine("]");

            int sum = 0;
            list.ForEach(e => sum +=e);
            WriteLine($"Promedio = {sum/(double)list.Size}");

            WriteLine($"Todos son positivos = {list.All(e => e >= 0)}");
            WriteLine($"Hay algun negativo? = {list.Any(e => e >= 0)}");
            WriteLine($"Hay algun cero? = {list.Any(e => e == 0)}");
            WriteLine($"# positivos = {list.Count(e => e > 0)}");
            WriteLine($"# negativos = {list.Count(e => e < 0)}");
            WriteLine($"# ceros = {list.Count(e => e == 0)}");
            WriteLine($"Mayor a 50 = {list.Find(e => e >= 50)}");
            WriteLine($"Mayor a 50(magnitud) = {list.Find(e => Math.Abs(e) >= 50)}");

            Write("\nPositivos = [");
            list.FindAll(e => e >= 0).ForEach(e => Write($"{e} "));
            WriteLine("]");

            Write("\nNegativos = [");
            list.FindAll(e => e < 0).ForEach(e => Write($"{e} "));
            WriteLine("]");


            /*
            ArrayList<int> list = new ArrayList<int>();
            list.Add(10);
            list.Add(-4);
            list.Add(5);
            list.Add(27);
            list.Add(-36);

            list.AddAll(100,2,56,34,-11);

            list[4] = -360;

            Write("\n[");
            for(int i = 0; i < list.Size; i++)
            {
                Write($"{list[i]} ");
            }
            WriteLine("]");

            //list.Clear();
            list.Insert(4,-20);

            list.RemoveAt(0);

            Write("\n[");
            foreach (int e in list)
            {
                Write($"{e} ");
            }
            WriteLine("]");

            */

        }
    }
}
