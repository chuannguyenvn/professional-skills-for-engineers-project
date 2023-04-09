using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarAlgorithm : PathfindingAlgorithm
{
    public class AVertex : PathfindingAlgorithm.Vertex
    {
        public AVertex(int key) : base(key)
        {
            
        }
    }
    
    /*
    public static List<Vertex> AStarShortestPath(GraphVertexList graphVertexList, Vertex source, Vertex destination)
    {
        Priority_Queue.SimplePriorityQueue<Vertex> openSet = new ();
        HashSet<Vertex> closeSet = new();
        openSet.Enqueue(source, source.fCost);
        
        while (openSet.Count > 0)
        {
            var currentMinFCostItem = openSet.Dequeue();
            closeSet.Add(currentMinFCostItem);

            if (currentMinFCostItem == endNode)
            {
                return Retrace(startNode, endNode);;
            }

            foreach (var adjacentItem in currentMinFCostItem.adjacentItems)
            {
                if (closeSet.Contains(adjacentItem))
                {
                    continue;
                }

                int newGCostToNeighbour = currentMinFCostItem.gCost + GetDistanceCost(currentMinFCostItem, adjacentItem);
                if (newGCostToNeighbour < adjacentItem.gCost || !openSet.Contains(adjacentItem))
                {
                    adjacentItem.gCost = newGCostToNeighbour;
                    adjacentItem.hCost = GetDistanceCost(adjacentItem, endNode);
                    adjacentItem.parentCell = currentMinFCostItem;

                    if (!openSet.Contains(adjacentItem))
                    {
                        openSet.Enqueue(adjacentItem, adjacentItem.fCost);
                    }
                }

            }
        }
        //Not found a path to the end
        return null;
    }
    
    public List<StackStorageGridCell> FindPath(StackStorageGridCell startNode, StackStorageGridCell endNode)
    {
       
    }

    /// <summary>
    /// Get a forward Item that the pathfinding was found
    /// </summary>
    protected List<StackStorageGridCell> Retrace(StackStorageGridCell start, StackStorageGridCell end)
    {
        List<StackStorageGridCell> path = new();
        StackStorageGridCell currentNode = end;
        while (currentNode != start && currentNode!= null)
        {
            //Debug.Log("Path "+ currentNode.xIndex +" "+ currentNode.zIndex );
            path.Add(currentNode);
            currentNode = currentNode.parentCell;
        }
        path.Add(start);
        path.Reverse();
        return path;
    }

    protected virtual int GetDistanceCost(StackStorageGridCell start, StackStorageGridCell end)
    {
        (int xDiff, int zDiff) = StackStorageGridCell.GetIndexDifferenceAbsolute(start, end);

        //return xDiff > zDiff ? 14*zDiff+ 10*(xDiff-zDiff) : 14*xDiff + 10*(zDiff-xDiff);
        return 10 * xDiff + 10 * zDiff;
    }*/
    
}
