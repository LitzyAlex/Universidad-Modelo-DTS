using System;
using System.Collections.Generic;
using System.Text;

namespace QueueDev
{
    internal interface IQueue<T>
    {
        int Size { get; } //Tamaño de los datos
        bool Empty { get; } //Ver si esta vacio
        bool Full { get; }  //Ver si esta lleno

        T Enqueue(T e); //Agregar
        T Dequeue(); //Eliminar
        T Head();  //Retorna el primer elemento del queue

        T Tail(); //Retorna el ultimo elemento del queue
    }
}
