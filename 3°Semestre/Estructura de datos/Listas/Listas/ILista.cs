using System;
using System.Collections.Generic;
using System.Text;

namespace Listas
{
    internal interface ILista<T>
    {
        int Size { get; } //Número total de elementos
        bool Empty { get; } //Ver si esta vacio

        void Add(T e); //Añade un elemento ya sea l inicio, final o posición especifica
        T Remove(int i); //Elimina un elemento segun su indice
        T Search(T e);  //Revisa si un elemento especifico existe

        T Consult(int i); //Muestra de un elemento segun el indice dado
    }
}
