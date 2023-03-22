using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RoadIntersectionNode : MonoBehaviour
{
    
    [SerializeField, FormerlySerializedAs("roadIntersectionNodes")] public List<RoadIntersectionNode> adjacentRoadNodes;

    void OnDrawGizmosSelected()
    {
        foreach (var roadIntersectionNode in adjacentRoadNodes)
        {   
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, roadIntersectionNode.transform.position);
        }
        
    }

    
}
