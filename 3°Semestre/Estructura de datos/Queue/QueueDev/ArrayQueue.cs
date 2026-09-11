using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace QueueDev
{
    //interfaz IQueue => qué debe poder hacer la cola
    //clase ArrayQueue => cómo lo hace 
    //T => tipo generico    
    internal class ArrayQueue<T> : IQueue<T>
    {
        //datos
        private const int DefaultCapacity = 5; //espacios iniciales que no cambia
        private T[] data;   //arreglo de datos
        private int indexI;
        private int indexF;
        private int check;  //contador para cantidad de elementos en el arreglo

        //propiedades
        public int Capacity { get; private set; } //espacios que tiene el arreglo (se puede leer pero no editar)
        public int Size => check;
        public bool Empty => check == 0;
        public bool Full => check == Capacity;


        // Constructores
        //constructor con espacios vacíos
        public ArrayQueue()
        {
            Capacity = DefaultCapacity;
            data = new T[Capacity];
            indexF = -1;
            indexI = 0;
            check = 0;
        }

        //constructor con capacidad
        public ArrayQueue(int capacity) 
        {
            //si la capacidad es menor a la capacidad por defecto, se asigna esta capacidad 
            //sino se asigna la capacidad dada
            Capacity = capacity < DefaultCapacity ? DefaultCapacity : capacity;
            data = new T[Capacity];
            indexF = -1;
            indexI = 0;
            check = 0;
        }

        //constructor de copia
        public ArrayQueue(ArrayQueue<T> data)  
        {
            Capacity = data.Capacity;         //la capacidad del nuevo arreglo sea igual al anterior
            this.data = new T[Capacity];      //para el data del nuevo arreglo respetando la capacidad
            indexF = data.indexF;               
            indexI = data.indexI;               
            check = data.check;             

            for (int i = 0; i < data.Capacity; i++) //mete posición por posición los elementos
            {
                this.data[i] = data.data[i]; //en la posición del arreglo2 se mete el elemento del arreglo1
            }
        }   

        //Métodos
        //añadir elemento al final
        public T Enqueue(T e)
        {
            if (Full) //si está llena se expande el arreglo
            {
                Console.WriteLine("Expandiendo arreglo...");
                Console.WriteLine();

                T[] newData = new T[Capacity * 2];  //duplica la capacidad = 10
                for (int i = 0; i < Capacity; i++)
                {
                    newData[i] = data[(indexI + i) % Capacity]; 
                }
                indexI = 0;
                indexF = Capacity - 1;
                Capacity *= 2;
                data = newData; //reemplaza por el nuevo arreglo
            }
            indexF = (indexF + 1) % Capacity; //aumenta 1 el índice final, sino regresa al inicio
            data[indexF] = e; //agrega el elemento
            check++; //aumenta el contador
            return e;
        }

        //quitar elemento al inicio
        public T Dequeue()
        {
            if (Empty)
            {
                throw new IndexOutOfRangeException("El arreglo está vacío");
            }
            //si se puede reducir sin pasarse del Default y el tamaño actual es 1/5 se reduce a la mitad el arreglo
            if (Capacity / 2 >= DefaultCapacity && check == Capacity / 5) 
            {
                Console.WriteLine("Reduciendo arreglo...");
                Console.WriteLine();

                T[] newData = new T[Capacity / 2];  //nuevo arreglo con espacios reducidos
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
            indexI = (indexI + 1) % Capacity; //avanza 1 el índice inicial, sino regresa al inicio
            check--;    //elimina un elemento del contador
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
