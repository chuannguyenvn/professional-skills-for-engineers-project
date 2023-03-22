using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public static class MapUtilities
    {
        private static readonly float ConversionConst = 0.000008950159495243074f;
        private static readonly Vector2 GeoAnchor = new (10.772802f, 106.659716f);
        private static readonly Vector2 WorldAnchor = new (0, 0);

        public static Vector2 GeoToWorldPosition(Vector2 geoPosition)
        {
            var reversedPos = (geoPosition - GeoAnchor) / ConversionConst;
            return new Vector2(reversedPos.y, reversedPos.x);
        }
        
        public static Vector2 WorldToGeoPosition(Vector2 worldPosition)
        {
            var reversedPos = (worldPosition - WorldAnchor) * ConversionConst;
            return new Vector2(reversedPos.y, reversedPos.x);
        }

        
        // This class represents a directed graph using
        // adjacency list representation
        public class Graph
        {
            private const int INF = 2147483647;
         
            private int _numOfVertices;
            private List<int[]>[] adj;
         
            public Graph(int numOfVertices)
            {   
                // No. of vertices
                this._numOfVertices = numOfVertices;
                // In a weighted graph, we need to store vertex
                // and weight pair for every edge
                this.adj = new List<int[]>[numOfVertices];
         
                for (int i = 0; i < numOfVertices; i++)
                {
                    this.adj[i] = new List<int[]>();
                } 
            }
         
            public void AddEdge(int u, int v, int w)
            {
                this.adj[u].Add(new int[] { v, w });
                this.adj[v].Add(new int[] { u, w });
            }

            // Prints shortest paths from src to all other vertices
            public void ShortestPath(int src)
            {
                // Create a priority queue to store vertices that
                // are being preprocessed.
                SortedSet<int[]> pQueue = new SortedSet<int[]>(new DistanceComparer());

                // Create an array for distances and initialize all
                // distances as infinite (INF)
                int[] dist = new int[_numOfVertices];
                for (int i = 0; i < _numOfVertices; i++)
                {
                    dist[i] = INF;
                }

                // Insert source itself in priority queue and initialize
                // its distance as 0.
                pQueue.Add(new int[] { 0, src });
                dist[src] = 0;

                /* Looping till priority queue becomes empty (or all
                    distances are not finalized) */
                while (pQueue.Count > 0)
                {
                  // The first vertex in pair is the minimum distance
                  // vertex, extract it from priority queue.
                  // vertex label is stored in second of pair (it
                  // has to be done this way to keep the vertices
                  // sorted by distance)
                  int[] minDistVertex = pQueue.Min;
                  pQueue.Remove(minDistVertex);
                  int u = minDistVertex[1];

                  // 'i' is used to get all adjacent vertices of a
                  // vertex
                  foreach (int[] adjVertex in this.adj[u])
                  {
                    // Get vertex label and weight of current
                    // adjacent of u.
                    int v = adjVertex[0];
                    int weight = adjVertex[1];

                    // If there is a shorter path to v through u.
                    if (dist[v] > dist[u] + weight)
                    {
                      // Updating distance of v
                      dist[v] = dist[u] + weight;
                      pQueue.Add(new int[] { dist[v], v });
                    }
                  }
                }

                // Print shortest distances stored in dist[]
                Debug.Log("Vertex Distance from Source");
                for (int i = 0; i < _numOfVertices; ++i)
                    Debug.Log(i + "\t" + dist[i]);
            }

            private class DistanceComparer : IComparer<int[]>
            {
                public int Compare(int[] x, int[] y)
                {
                    if (x[0] == y[0]) {
                        return x[1] - y[1];
                    }
                    return x[0] - y[0];
                }
            }
        }
        
    }
}