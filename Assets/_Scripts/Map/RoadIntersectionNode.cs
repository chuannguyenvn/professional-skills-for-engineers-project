using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadIntersectionNode : MonoBehaviour
{
    [SerializeField] public List<RoadIntersectionNode> roadIntersectionNodes;

    void OnDrawGizmosSelected()
    {
        foreach (var roadIntersectionNode in roadIntersectionNodes)
        {   
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, roadIntersectionNode.transform.position);
        }
        
    }

    
}
