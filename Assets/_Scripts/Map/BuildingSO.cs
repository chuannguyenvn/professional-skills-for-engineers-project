using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/Building", order = 1)]
public class BuildingSO : ScriptableObject
{
    [FormerlySerializedAs("name")]public string buildingName = "New building";
    public string description;
    public List<Sprite> descriptiveSprites;
    public List<Vector2> geoCoordinate;
    public VisualManager.MapColor mapColor;
    public float height = 10f;

    
}
