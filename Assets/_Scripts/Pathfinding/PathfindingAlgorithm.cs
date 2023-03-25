using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingAlgorithm
{
    
     public class Edge
    {
        public Edge(Vertex source, Vertex destination, float weight)
        {
            Source = source;
            Destination = destination;
            Weight = weight;
        }

        public float Weight { get; set; }
        public Vertex Source { get; set; }
        public Vertex Destination { get; set; }
    }

    public class Vertex
    {
        public Vertex(int key)
        {
            Key = key;
        }

        public int Key { get; set; }
    }

    public class GraphVertexList
    {
        private Dictionary<Vertex, List<Edge>> adjList;

        public GraphVertexList()
        {
            adjList = new Dictionary<Vertex, List<Edge>>();
        }

        public Dictionary<Vertex, List<Edge>> AdjList
        {
            get
            {
                return adjList;
            }
        }

        public void AddEdgeDirected(Vertex source, Vertex destination, float weight)
        {
            if (adjList.ContainsKey(source))
            {
                adjList[source].Add(new Edge(source, destination, weight));
            }
            else
            {
                adjList.Add(source, new List<Edge> { new Edge(source, destination, weight) });
            }

            if (!adjList.ContainsKey(destination))
            {
                adjList.Add(destination, new List<Edge>());
            }
        }

        // Don't actually do this (i.e. actually write 2 classes for directed/undirected):
        public void AddEdgeUndirected(Vertex source, Vertex destination, float weight)
        {
            if (adjList.ContainsKey(source))
            {
                adjList[source].Add(new Edge(source, destination, weight));
            }
            else
            {
                adjList.Add(source, new List<Edge> { new Edge(source, destination, weight) });
            }

            if (adjList.ContainsKey(destination))
            {
                adjList[destination].Add(new Edge(destination, source, weight));
            }
            else
            {
                adjList.Add(destination, new List<Edge> { new Edge(destination, source, weight) });
            }
        }

        public void RemoveEdge(Vertex source, Vertex destination)
        {
            if (adjList.ContainsKey(source))
            {
                var found = adjList[source].Find(edge => edge.Destination == destination);
                if (found != null)
                {
                    adjList[source].Remove(found);
                }

                Debug.Log("Found"+ found.Source+" "+ found.Destination + " "+ found.Weight + " And result "+ adjList[source].Find(edge => edge.Destination == destination));
            }
        }
    }
    
    
    public class GraphAdjacencyMatrix
    {
        private float[,] adjMatrix;

        public GraphAdjacencyMatrix(int vertices)
        {
            adjMatrix = new float[vertices, vertices];
        }

        public float[,] AdjMatrix
        {
            get
            {
                return adjMatrix;
            }
        }

        public void AddEdgeDirected(int source, int destination, float weight)
        {
            adjMatrix[source, destination] = weight;
        }

        public void AddEdgeUndirected(int source, int destination, float weight)
        {
            adjMatrix[source, destination] = weight;
            adjMatrix[destination, source] = weight;
        }
    }

    public static Dictionary<Vertex, Vertex> backTrackingVertices;
    public static Dictionary<int, int> backTrackingMatrix;
}
