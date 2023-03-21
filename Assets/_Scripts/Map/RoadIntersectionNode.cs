using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadIntersectionNode : MonoBehaviour
{
    [SerializeField] private List<RoadIntersectionNode> roadIntersectionNodes;

    void Awake()
    {
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);   
    }
    
    void OnDrawGizmosSelected()
    {
        foreach (var roadIntersectionNode in roadIntersectionNodes)
        {   
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, roadIntersectionNode.transform.position);
        }
        
    }
}
