using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace EnumerableDemo
{
    class Demo<T> : IEnumerable<T>
    {
        class DemoEnumerator<T> : IEnumerator<T> //para crear un objeto DemoEnumerador
        {
            private Demo<T> demoRef;
            private int index;

            public DemoEnumerator(Demo<T> demo)
            {
                demoRef = demo;
                index = -1;
            }

            public T Current
            {
                get
                {
                    switch (index)
                    {
                        case 0:
                            return demoRef.x;

                        case 1:
                            return demoRef.y;

                        case 2:
                            return demoRef.z;
                    }

                    throw new InvalidOperationException();
                }
            } 


            public bool MoveNext()
            {
                if (index < 2)
                {
                    index++;
                    return true;
                }
                return false;
            }
            public void Reset()
            {
                index = -1;
            }

            public void Dispose()
            {
                
            }

            object IEnumerator.Current => Current;
        }
        private T x;//0
        private T y;//1
        private T z; //2

        public Demo(int x, int y, int z)
        {
            this.x =(T)(object)x;                                                                                          
            this.y =(T)(object)y;
            this.z =(T)(object)z;
        }
        public IEnumerator<T> GetEnumerator()
        {
            //return new DemoEnumerator<T>(this); //me paso a mi mismo
            yield return x;
            yield return y;
            yield return z;
        }

        IEnumerator IEnumerable.GetEnumerator() //Es la version antigua que llama a la nueva, hay que ponerla, se hereda esta
        {
            return GetEnumerator();
        }
    }

    class FibonacciSeries : IEnumerable<int>
    {
        public IEnumerator<int> GetEnumerator()
        {
            int a = 0;
            int b = 1;
            int c;
            yield return a; //memoriza el lugar donde se queda
            yield return b;
            
            for(; ; )
            {
                c = a + b;
                yield return c;
                a = b;
                b = c;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            FibonacciSeries fs = new FibonacciSeries();
            int counter = 0;
            foreach (int e in fs)
            {
                Write($"{e} ");
                if (counter++ == 10) { break;  }
            }



            /*
            Demo<int> demo = new Demo<int>(5,11,25);
            
            foreach (int e in demo) 
            {
                Write($"{e} ");
            }
            

            
            //Esto es lo que se hace cuando se escribe un foreach
            IEnumerator<int> enumerator = demo.GetEnumerator();
            while (enumerator.MoveNext())
            {
                int e=enumerator.Current;
                Write($"{e} ");
            }
            */
            
        }
    }
}