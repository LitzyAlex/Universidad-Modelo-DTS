using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ListDev
{
    internal class LinkedList<T> : IList<T>
    {
        //clase Nodo
        private class Node
        {
            public T Element {get; set;}
            public Node Next { get; set; }

            public Node(T element) : this(element, null) { }
            public Node(T element, Node next)
            {
                Element = element;
                Next = next;
            }
        }

        //Atributos y propiedades
        private Node first;

        public int Size {get; set;}

        public bool Empty => first == null;

        //Constructores
        public LinkedList()
        {
            first = null;
            Size = 0;
        }

        public LinkedList(IEnumerable<T> elements) : this()
        {
            foreach (var e in elements)
            {
                Add(e);
            }
        }

        //Metodos
        public IEnumerator<T> GetEnumerator()
        {
            if (Size > 0)
            {
                Node current = first;
               
                do
                {
                    yield return current.Element;
                    current = current.Next;
                }
                while(current != null);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T element)
        {
            if (Empty)
            {
                first = new Node(element);
                Size++;
            }
            else
            {
                Node lastNode = first;
                while (lastNode.Next != null)
                {
                    lastNode = lastNode.Next;
                }
                
                lastNode.Next = new Node(element);
                Size++;
            }
        }

        public T this[int index] {
            get => Get(index);
            set => Set(index, value);
        }

        public void AddAll(T first, params T[] elements)
        {
            Add(first);
            foreach (T e in elements)
            {
                Add(e);
            }
        }

        public bool All(Predicate<T> condition)
        {
            throw new NotImplementedException();
        }

        public bool Any(Predicate<T> condition)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T element)
        {
            Node current = first;
            while (current != null) 
            {
                //Se usa asi porque element es un tipo de dato T
                //object.Equals(a,b) compara dos valores y devuelve true o false
                //object es el tipo base del que derivan todos los tipos
                if (object.Equals(current.Element,element)) 
                {
                    return true;
                }
                current = current.Next;
            }
            return false;
        }

        public int Count(Predicate<T> condition)
        {
            throw new NotImplementedException();
        }

        public T Find(Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public IList<T> FindAll(Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public void ForEach(Action<T> action)
        {
            throw new NotImplementedException();
        }

        public T Get(int index) //Perdon lo hice para el index ;u;
        {
            Node current = first;

            for (int i = 0; i < index; i++)
            {
                current = current.Next;
            }

            return current.Element;
        }


        public void Insert(int index, T element)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void Set(int index, T element)
        {
            Node current = first; //comienza en el primer nodo
            for (int i = 0; i < index; i++) //avanza hasta el indice dado
            {
                current = current.Next;
            }
            
            current.Element = element;
        }

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
                if(sorted == null || comparison(current.Element, sorted.Element) < 0)
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
