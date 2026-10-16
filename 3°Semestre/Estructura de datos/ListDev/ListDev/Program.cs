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
        public class Student //Ejemplo
        {
            public int Id { get; }
            public string Name { get; }
            public double Grade { get; }

            public Student(int id, string name, double grade)
            {
                Id = id;
                Name=name;
                Grade=grade;
            }
        }

        static void Main(string[] args)
        {
           WriteLine("\n******Lista inicial****");
           var list = new LinkedList<int>();
           list.Add(10);
           list.Add(-4);
           list.Add(5);
           list.Add(27);
           list.Add(-36);

           Write("[");
           foreach (int e in list)
           {
               Write($"{e} ");
           }
           WriteLine("]\n");


            //pruebas Litzy
            WriteLine("\n******Prueba de Get, Set e index****");
            WriteLine($"Get indice 3: {list.Get(3)}");
            WriteLine("Set indice (3,0): ");
            list.Set(3, 0);

            Write("\n[");
            foreach (int e in list)
            {
                Write($"{e} ");
            }
            WriteLine("]");

            WriteLine("\nlist[27] = 3");
            list[3] = 27;

            Write("\n[");
            foreach (int e in list)
            {
                Write($"{e} ");
            }
            WriteLine("]");

            WriteLine("\n******Prueba AddAll****");
            WriteLine("Añadir (100,2,56,34,-11): "); 
            list.AddAll(100, 2, 56, 34, -11);

            Write("\n[");
            foreach (int e in list)
            {
                Write($"{e} ");
            }
            WriteLine("]");

            WriteLine("\n******Prueba Sort****");
            list.Sort((x,y) => x.CompareTo(y));
            Write("\n[");
            foreach (int e in list)
            {
                Write($"{e} ");
            }
            WriteLine("]");

            list.Sort((x, y) => y.CompareTo(x));
            Write("\n[");
            foreach (int e in list)
            {
                Write($"{e} ");
            }
            WriteLine("]");


            //pruebas Octavio
            WriteLine("\n******Prueba Insert******");
            WriteLine("Insertar 50 en indice 2:");
            list.Insert(2, 50);

            Write("[");
            foreach (int e in list)
            {
                Write($"{e} ");
            }
            WriteLine("]");

            WriteLine("\n******Prueba Any******");
            WriteLine($"Numeros negativos?: {list.Any(x => x < 0)}");
            WriteLine($"Numeros pares?: {list.Any(x => x % 2 == 0)}");

            WriteLine("\n******Prueba RemoveAt******");
            WriteLine("Eliminar elemento en indice 2:");
            list.RemoveAt(2);

            Write("[");
            foreach (int e in list)
            {
                Write($"{e} ");
            }
            WriteLine("]");

            WriteLine("\n******Prueba FindAll******");
            WriteLine("Numeros negativos:");

            var negativos = list.FindAll(x => x < 0);

            Write("[");
            foreach (int e in negativos)
            {
                Write($"{e} ");
            }
            WriteLine("]");

            WriteLine("\n******Prueba Count******");
            WriteLine($"Cantidad de negativos: {list.Count(x => x < 0)}");
            WriteLine($"Cantidad de positivos: {list.Count(x => x > 0)}");
            WriteLine($"Cantidad de pares: {list.Count(x => x % 2 == 0)}");

            //pruebas Aili
            WriteLine("\n******Prueba All******");
            WriteLine($"Todos los negativos: {list.All(x => x < 0)}");
            WriteLine($"Todos los pares: {list.All(x => x % 2 == 0)}");

            WriteLine("\n******Prueba Find******");
            WriteLine($"Mayor a 30: {list.Find(e => e >= 30)}");

            WriteLine("\n******Prueba Foreach******");
            Write("\nPositivos: [ ");
            list.FindAll(e => e > 0).ForEach(e => Write($"{e} "));
            WriteLine("]");

            Write("\nNegativos: [ ");
            list.FindAll(e => e < 0).ForEach(e => Write($"{e} "));
            WriteLine("]");

            WriteLine("\n******Prueba Clear******");
            list.Clear();
            Write("[");
            foreach (int e in negativos)
            {
                Write($"{e} ");
            }
            WriteLine("]");

            /*
            //************ArrayList**************
            var list = new ArrayList<int>(new int[] { 1,-3, 5, 0, -20, 35, -53, 27, 72 });

            Write("\n[");
            list.ForEach(e => Write($"{e} "));
            WriteLine("]");

            //list.Sort((x, y) => (x > y ? 1 : (x == y ? 0 : -1)));
            list.Sort((x, y) => x.CompareTo(y));

             Write("\n[");
             list.ForEach(e => Write($"{e} "));
             WriteLine("]");

             list.Sort((x, y) => (x * x > y * y ? 1 : (x * x == y * y ? 0 : -1)));

             Write("\n[");
             list.ForEach(e => Write($"{e} "));
             WriteLine("]");

             WriteLine();

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
            //********Introduccion ArrayList*******
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

            /*
            //****************Ejemplo practico**********************
            var students = new ArrayList<Student>()
            {
                new Student(65153, "Juan", 88.62),
                new Student(51918, "Erika", 95.74),
                new Student(89816, "Elizabeth", 44.36),
                new Student(35875, "Marco", 52.45),
                new Student(15792, "Javier", 68.12),
            };

            //students.Sort((x, y) => x.Id.CompareTo(y.Id));
            //students.Sort((x, y) => x.Name.CompareTo(y.Name));
            //students.Sort((x, y) => y.Grade.CompareTo(x.Grade));

            students = (ArrayList<Student>)students.FindAll(s => s.Grade < 60);
            students.Sort((x, y) => y.Grade.CompareTo(x.Grade)); 

            foreach (var s in students)
            {
                WriteLine($"{s.Id} - {s.Name} - {s.Grade}");
            }
            */

        }
    }
}
