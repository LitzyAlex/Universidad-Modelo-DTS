using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace QueueDev
{
    internal class ArrayQueue<T> : IQueue<T>
    {
        //datos
        private const int DefaultCapacity = 5;
        private T[] data;
        private int indexI;
        private int indexF;
        private int check;

        //propiedades
        public int Capacity { get; private set; }
        public int Size => check;
        public bool Empty => check == 0;
        public bool Full => check == Capacity;


        // Constructores
        public ArrayQueue()
        {
            Capacity = DefaultCapacity;
            data = new T[Capacity];
            indexF = -1;
            indexI = 0;
            check = 0;
        }
        public ArrayQueue(int capacity) 
        {
            Capacity = capacity < DefaultCapacity ? DefaultCapacity : capacity;
            data = new T[Capacity];
            indexF = -1;
            indexI = 0;
            check = 0;
        }

        public ArrayQueue(ArrayQueue<T> data)  
        {
            Capacity = data.Capacity;           
            this.data = new T[Capacity];       
            indexF = data.indexF;               
            indexI = data.indexI;               
            check = data.check;             

            for (int i = 0; i < data.Capacity; i++)
            {
                this.data[i] = data.data[i];
            }
        }   

        // Metodos
        //expandir arreglo
        public T Enqueue(T e)
        {
            if (Full)
            {
                Console.WriteLine("Expandiendo arreglo...");
                Console.WriteLine();

                T[] newData = new T[Capacity * 2];
                for (int i = 0; i < Capacity; i++)
                {
                    newData[i] = data[(indexI + i) % Capacity];
                }
                indexI = 0;
                indexF = Capacity - 1;
                Capacity *= 2;
                data = newData;
            }
            indexF = (indexF + 1) % Capacity;
            data[indexF] = e;
            check++;
            return e;
        }

        //disminuir arreglo (corregir)
        public T Dequeue()
        {
            if (Empty)
            {
                throw new IndexOutOfRangeException("El arreglo está vacío");
            }
            if (Capacity / 2 >= DefaultCapacity && check == Capacity / 5)
            {
                Console.WriteLine("Reduciendo arreglo...");
                Console.WriteLine();

                T[] newData = new T[Capacity / 2];
                for (int i = 0; i < check; i++)
                {
                    newData[i] = data[(indexI + i) % Capacity];
                }
                indexI = 0;
                indexF = check - 1;
                Capacity /= 2;
                data = newData;
            }

            T e = data[indexI];
            indexI = (indexI + 1) % Capacity;
            check--;
            return e;
        }

        //mostrar el primer elemento actual sin eliminarlo
        public T Head()
        {
            if (Empty)
            {
                throw new IndexOutOfRangeException("El arreglo está vacio");
            }
            return data[indexI];
        }

        //mostrar el último elemento actual sin eliminarlo
        public T Tail()
        {
            if (Empty)
            {
                throw new IndexOutOfRangeException("El arreglo está vacio");
            }
            return data[indexF];
        }

#if DEBUG
        public string DataPeek()
        {
            string aux = "[";

            for (int i = 0; i < Size; i++)
            {
                int index = (indexI + i) % data.Length;
                aux += $"{data[index]},";
            }
            aux += "]";
            return aux;
        }
#endif
    }
}
