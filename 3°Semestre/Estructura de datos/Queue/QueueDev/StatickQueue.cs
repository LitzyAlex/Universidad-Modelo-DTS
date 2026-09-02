using System;
using System.Collections.Generic;
using System.Text;

namespace QueueDev
{
    internal class StatickQueue<T> : IQueue<T>
    {
        //datos
        private T[] data;
        private int indexI; //index del inicio
        private int indexF; //index del final
        private int check; //sirve para contar los elementos que se agreguen

        //propiedad
        public int Size => check; //Checar luego

        public bool Empty => check == 0;

        public bool Full => check == data.Length;  

        //constructor
        public StatickQueue(int capacity)
        {
            data = new T[capacity]; //Crea un arreglo de el tamaño dado
            indexF = 0;
            indexI = 0;
            check = 0;
        }



        //metodos
        public T Head()
        {
            if (Empty)
            {
                throw new InvalidOperationException();
            }
            return data[indexI];
        }

        public T Tail()
        {
            if (Empty)
            {
                throw new InvalidOperationException();
            }

            //me tomo como 5 intentos escribirlo bien :c
            return (indexF - 1 < 0) ? data[data.Length - 1] : data[indexF - 1]; 
        }

        public T Dequeue()
        {
            if (Empty)
            {
                throw new InvalidOperationException();
            }

            T pop;
            pop = data[indexI];
            indexI++;
            check--;

            if (indexI == data.Length)
            {
                indexI = 0;
            }

            return pop;


        }

        public T Enqueue(T e)
        {
            if (Full)
            {
                throw new IndexOutOfRangeException("index");
            }

            data[indexF] = e;
            indexF++;
            check++;

            if (indexF == data.Length)
            {
                indexF = 0;
            }

            return e;  

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
