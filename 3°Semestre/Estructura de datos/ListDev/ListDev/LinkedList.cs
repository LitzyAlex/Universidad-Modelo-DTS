using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Transactions;

namespace ListDev
{
    internal class LinkedList<T> : IList<T>
    {
        //clase Nodo
        private class Node
        {
            public T Element {get; set;}    //el dato que desea guardar
            public Node Next { get; set; }  //la referencia al siguiente nodo

            //constructores
            public Node(T element) : this(element, null) { }    //1 nodo: recibe el elemento y la referencia es null
            public Node(T element, Node next)                   //2 o +: recibe el elemento y la referencia al siguiente nodo
            {
                Element = element;
                Next = next;
            }
        }

        //Atributos y propiedades
        private Node first;

        public int Size {get; set;}

        public bool Empty => first == null;


        //Constructores -> lista vacía con tamaño 0
        public LinkedList()
        {
            first = null;
            Size = 0;
        }

        //              -> recibe una colección de elementos, inicializa la lista this() y añade cada e
        public LinkedList(IEnumerable<T> elements) : this()
        {
            foreach (var e in elements)
            {
                Add(e);
            }
        }

        //Métodos
        public T this[int index]
        {
            get => Get(index);
            set => Set(index, value);
        }

        public IEnumerator<T> GetEnumerator() //para recorrer la lista con un foreach
        {
            if (Size > 0)
            {
                Node current = first;   //"explorador" que inicia como first
               
                do
                {
                    yield return current.Element; //entrega el elemento actual pero manteniendo el estado de current para la siguiente iteración
                    current = current.Next;       //avanza al siguiente nodo
                }
                while(current != null);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        //1 Get -> Litzy
        public T Get(int index)
        {
            Node current = first;

            for (int i = 0; i < index; i++)
            {
                current = current.Next;
            }

            return current.Element;
        }

        //2 Set -> Litzy
        public void Set(int index, T element)
        {
            Node current = first; //comienza en el primer nodo
            for (int i = 0; i < index; i++) //avanza hasta el indice dado
            {
                current = current.Next;
            }

            current.Element = element;
        }

        //3 Add ->
        public void Add(T element)
        {
            if (Empty) //si la lista está vacía...
            {
                first = new Node(element);  //crea un nuevo nodo y lo asigna a first
                Size++;
            }
            else
            {
                Node lastNode = first;          //empieza buscando en el primer nodo
                while (lastNode.Next != null)   //mientras haya un siguiente nodo avanza
                {
                    lastNode = lastNode.Next;   //pasa al siguiente nodo
                }

                lastNode.Next = new Node(element);  //cuando llega a lo último conecta ahí el nodo nuevo
                Size++;
            }
        }

        //4 AddAll -> Litzy
        public void AddAll(T first, params T[] elements)
        {
            Add(first);
            foreach (T e in elements)
            {
                Add(e);
            }
        }

        //5 Insert -> Octavio
        public void Insert(int index, T element)
        {
            if (index < 0 || index >= Size)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (index == 0)
            {
                var Node_New = new Node(element);
                Node_New.Next = first;
                first = Node_New;
                Size++;
                return;
            }

            var current = first;
            for (int i = 0; i < index - 1; i++)
            {
                current = current.Next;
            }

            var NewNode = new Node(element);
            NewNode.Next = current.Next; // Igual a current arriba.
            current.Next = NewNode; // Re referenciamos current. 
            Size++;
        }

        //6 RemoveAt -> Octavio
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Size)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }


            if (index == 0)
            {
                first = first.Next;
                Size--;
                return;
                // Pero el nodo sigue en memoria no ? 
            }

            var current = first;
            for (int i = 0; i < index - 1; i++)
            {
                current = current.Next;
            }

            current.Next = current.Next.Next;
            Size--;
            return;
        }

        //7 Clear -> Aili
        public void Clear()
        {   
            first = null; //solo borra la referencia al primer nodo
            Size = 0;
        }

        //8 Contains -> Litzy
        public bool Contains(T element)
        {
            Node current = first;
            while (current != null)
            {
                //Se usa asi porque element es un tipo de dato T
                //object.Equals(a,b) compara dos valores y devuelve true o false
                //object es el tipo base del que derivan todos los tipos
                if (object.Equals(current.Element, element))
                {
                    return true;
                }
                current = current.Next;
            }
            return false;
        }

        //9 All -> Aili
        public bool All(Predicate<T> condition)
        {
            //empieza en first y avanza con next hasta el último (null)
            for (var e = first; e != null; e = e.Next)
            {
                if (!condition(e.Element)) //si no cumple la condición:
                {
                    return false;
                }
            }

            //si cumple todo o la lista está vacía:
            return true;
        }

        //10 Any -> Octavio
        public bool Any(Predicate<T> condition)
        {
            for (var e = first; e != null; e = e.Next)
            {
                if (condition(e.Element))
                {
                    return true;
                }
            }

            return false;
        }

        //11 Count -> Octavio
        public int Count(Predicate<T> condition)
        {
            int matches = 0;
            for (var e = first; e != null; e = e.Next)
            {
                if (condition(e.Element))
                {
                    matches++;
                }
            }
            return matches;
        }

        //12 Find -> Aili
        public T Find(Predicate<T> match)
        {
            for (var e = first; e != null; e = e.Next)
            {
                if (match(e.Element))   //cuando encuentra el primer elemento que cumple la condición:
                {
                    return e.Element;   //lo devuelve y termina la búsqueda
                }
            }
            return default;
        }

        //13 FindAll -> Octavio
        public IList<T> FindAll(Predicate<T> match)
        {
            LinkedList<T> matches = new LinkedList<T>();
            for (var e = first; e != null; e = e.Next)
            {
                if (match(e.Element))
                {
                    matches.Add(e.Element);
                }
            }
            return matches;
        }

        //14 ForEach -> Aili
        public void ForEach(Action<T> action)
        {
            for (var e = first; e != null; e = e.Next)
            {
                action(e.Element);  //solo aplica la acción a cada elemento sin devolver nada
            }
        }

        //15 Sort -> Litzy
        public void Sort(Comparison<T> comparison)
        {
            if (Empty)
            {
                return;
            }
            Node sorted = null; //inicio de el ordenamiento
            Node current = first; //el primer nodo de nuestra lista

            while (current != null)
            {
                Node next = current.Next; //guarda el siguiente antes de modificar
                if (sorted == null || comparison(current.Element, sorted.Element) < 0)
                {
                    current.Next = sorted; //En la primera apunta a null
                    sorted = current;
                }
                else
                {
                    Node search = sorted;

                    while (search.Next != null && comparison(search.Next.Element, current.Element) <= 0)
                    {
                        search = search.Next;
                    }

                    current.Next = search.Next;
                    search.Next = current;
                }

                current = next;
            }

            first = sorted;
        }
    }
}
