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

        

      

        public T this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void AddAll(T first, params T[] elements)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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

        public T Get(int index)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void Sort(Comparison<T> comparison)
        {
            throw new NotImplementedException();
        }
    }
}
