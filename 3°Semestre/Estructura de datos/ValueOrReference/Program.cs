using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace ValueOrReference
{
    internal class Program
    {

        class Data
        {
            public int x;
            public int y;
        }

        static void Swap( int m,  int n)
        {
            int aux = m;
            m = n;
            n = aux;
        }

        static void Swap(ref int m, ref int n)  //Con referencia cambia el valor total de las variables porque pasa su direccion y donde esta guardado el valor
        {
            int aux = m;
            m = n;
            n = aux;
        }

        static void Swap(Data d1, Data d2)
        {
            int aux = d1.x; d1.x = d2.x; d2.x = aux;
                aux = d1.y; d1.y = d2.y; d2.y = aux;
        }

        static void Swap(ref Data d1, ref Data d2)
        {
            int aux = d1.x; d1.x = d2.x; d2.x = aux;
            aux = d1.y; d1.y = d2.y; d2.y = aux;
        }

        static void Swap1( ref Data d1, ref Data d2)
        {
            Data aux = d1;
            d1 = d2;
            d2 = aux;
        }

        static void Main(string[] args)
        {
            Data d1 = new Data();   //Data d1; en struc
            d1.x = 10;
            d1.y = 20;

            Data d2 = new Data();
            d2.x = -15;
            d2.y = -55;

            WriteLine($"d1 ->({d1.x}, {d1.y}), d2 ->({d2.x}, {d2.y})");

            Swap(d1, d2);
            //Swap(ref d1, ref d2);

            WriteLine($"d1 ->({d1.x}, {d1.y}), d2 ->({d2.x}, {d2.y})");

            //int m = 15;
            //int n = 10;

            //WriteLine($"m = {m}, n = {n}");

            //Swap(ref m, ref n);

            //WriteLine($"m = {m}, n = {n}");

        }
    }
}
