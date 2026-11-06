using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PriorityQueueDev
{
    internal class PriorityQueue<T> : IPriorityQueue<T>
    {
        //clase Nodo
        private class Node
        {
            public T Element { get; set; }    //el dato que desea guardar
            public Node Next { get; set; }  //la referencia al siguiente nodo
            public int Priority {  get; set; }

            //constructores
            public Node(T element, int priority) : this(element, null, priority) { }    //1 nodo: recibe el elemento y la referencia es null
            public Node(T element, Node next, int priority)                   //2 o +: recibe el elemento y la referencia al siguiente nodo
            {
                Element = element;
                Next = next;
                Priority = priority;
            }
        }

        private Node first;

        public int Size { get; set; }

        public bool Empty => first == null;

        //Constructores
        public PriorityQueue()
        {
            first = null;
            Size = 0;
        }

        //metodos
        public void Enqueue(T e, int p)
        {
            if (Empty) //si la lista está vacía...
            {
                first = new Node(e,p);  //crea un nuevo nodo y lo asigna a first
                Size++;
            }
            else
            {
                Node lastNode = first;          //empieza buscando en el primer nodo
                while (lastNode.Next != null)   //mientras haya un siguiente nodo avanza
                {
                    lastNode = lastNode.Next;   //pasa al siguiente nodo
                }

                lastNode.Next = new Node(e,p);  //cuando llega a lo último conecta ahí el nodo nuevo
                Size++;
            }
        }

        public T Dequeue()
        {
            if (Empty)
            {
                throw new InvalidOperationException("Esta vacia");
            }

            Node current = first;
            Node betterN = current;
            Node before = null;
            Node betterB = null;

            while (current != null)
            {
                if (current.Priority < betterN.Priority)
                {
                    betterN = current;
                    betterB = before;
                }

                before = current;
                current = current.Next;
            }

            if (betterB == null)
            {
                first = first.Next;
            }
            else
            {
                betterB.Next = betterN.Next;
            }

            Size--;

            return betterN.Element;

        }    


        public T Head()
        {
            if (Empty)
            {
                throw new InvalidOperationException("Esta vacia");
            }

            Node current = first;
            Node betterN = current;

            while (current != null)
            {
                if (current.Priority < betterN.Priority)
                {
                    betterN = current;
                }

                current = current.Next;
            }

            return betterN.Element;
        }

        public T Tail()
        {
            if (Empty)
            {
                throw new InvalidOperationException("Esta vacia");
            }

            Node current = first;
            Node betterN = current;

            while (current != null)
            {
                if (current.Priority > betterN.Priority)
                {
                    betterN = current;
                }

                current = current.Next;
            }

            return betterN.Element;
        }
        public void Mostrar()
        {
            Node current = first;
            Console.Write("[ ");

            while (current != null)
            {
                Console.Write("(" + current.Element + ", " + current.Priority + ")");

                if (current.Next != null)
                {
                    Console.Write(", ");
                }

                current = current.Next;
            }

            Console.WriteLine(" ]");
        }
    }
    
}
