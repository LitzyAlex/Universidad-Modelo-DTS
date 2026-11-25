using System;
using System.Collections.Generic;
using System.Text;

namespace Grafos
{
    internal interface IGraph<T>
    {
        int VertexCount { get; } //Cantidad de vertices que hay en el grafo
        int EdgeCount { get; } //Cantidad de aristas del grafo
        bool IsDirected { get; } //Indica si el grafo es dirigido (true) o no dirigido (false)

        IEnumerable<T> Vertices { get; } //Recorre todos los vertices del grafo

        void AddVertex(T vertex); //Agrega un vertice nuevo al grafo
        void RemoveVertex(T vertex); //Elimina un vertice junto con todas las aristas que lo tocan
        bool HasVertex(T vertex); //Indica si el vertice existe en el grafo

        void AddEdge(T source, T destination, int weight = 1); //Agrega una arista de origen a destino con el peso indicado
        void RemoveEdge(T source, T destination); //Elimina la arista de origen a destino
        bool HasEdge(T source, T destination); //Indica si existe una arista de origen a destino
        bool TryGetWeight(T source, T destination, out int weight); //Intenta obtener el peso de la arista de origen a destino.

        IEnumerable<(T Vertex, int Weight)> GetNeighbors(T vertex); //Devuelve los vecinos del vertice dado. cada elemento trae el verticevecino y el peso de la arista que lleva hasta el
    }
}
