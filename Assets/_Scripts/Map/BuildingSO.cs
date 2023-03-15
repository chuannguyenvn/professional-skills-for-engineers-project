using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/Building", order = 1)]
public class BuildingSO : ScriptableObject
{
    public string name = "New building";
    public Color color = Color.black;
    public List<Vector2> geoCoordinate;
    public float height = 10f;
}
