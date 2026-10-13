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
            
           var list = new LinkedList<int>();
           list.Add(10);
           list.Add(-4);
           list.Add(5);
           list.Add(27);
           list.Add(-36);

           WriteLine();
           Write("\n[");
           foreach (int e in list)
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
