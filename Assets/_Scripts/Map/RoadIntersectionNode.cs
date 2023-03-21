using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadIntersectionNode : MonoBehaviour
{
    void Awake()
    {
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);   
    }
}
