using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RoadIntersectionNode : MonoBehaviour
{
    
    [SerializeField, FormerlySerializedAs("roadIntersectionNodes")] public List<RoadIntersectionNode> adjacentRoadNodes = new List<RoadIntersectionNode>();

    void OnDrawGizmosSelected()
    {
        foreach (var roadIntersectionNode in adjacentRoadNodes)
        {   
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, roadIntersectionNode.transform.position);
        }
        
    }

    
    private void OnValidate()
    {
        if (adjacentRoadNodes == null) return;
        foreach (var roadNode in adjacentRoadNodes)
        {
            if(roadNode.adjacentRoadNodes == null ) continue;
                
            if(roadNode.adjacentRoadNodes.Find(node => node == this ) == null ) roadNode.adjacentRoadNodes.Add(this);
        }
    }
    
}
