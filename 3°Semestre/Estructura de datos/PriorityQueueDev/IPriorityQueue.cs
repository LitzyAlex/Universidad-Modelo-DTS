using System;
using System.Collections.Generic;
using System.Text;

namespace PriorityQueueDev
{
    internal interface IPriorityQueue<T>
    {
        int Size { get; } //Retorna el número de elementos
        bool Empty { get; } //Retorna True si esta vacio

        void Enqueue(T e, int p); //Agrega un nuevo elemento e al Priority, asociandolo una prioridad p
        T Dequeue(); //Remueve y retorna el elemento con mayor prioridad
        T Head();  //Retorna el elemento con mayor prioridad sin removerlo

        T Tail(); //Retorna el elemento de menor prioridad
        void Mostrar();

    }
}
