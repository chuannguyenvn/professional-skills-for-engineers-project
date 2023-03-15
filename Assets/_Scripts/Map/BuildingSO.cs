using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/Building", order = 1)]
public class BuildingSO : ScriptableObject
{
    public string name = "New building";
    public List<Vector2> geoCoordinate;
    
    public VisualManager.MapColor mapColor;
    public float height = 10f;

    
}
