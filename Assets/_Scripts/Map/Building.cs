using System;
using System.Collections.Generic;
using Shapes;
using UnityEngine;

namespace Map
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private Polygon polygon;
        
        public void Init(string buildingName, List<Vector2> coordinates)
        {
            gameObject.name = buildingName;
            foreach (var coordinate in coordinates)
            {
                var worldPos = MapHelper.GeoToWorldPosition(coordinate);
                polygon.AddPoint(worldPos);
            }
        }
    }
}