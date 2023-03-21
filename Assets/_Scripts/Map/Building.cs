using System;
using System.Collections.Generic;
using System.Linq;
using Shapes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Map
{
    public class Building : MonoBehaviour
    {
        public bool Is3D = true;
        public float BuildingHeight = 10f;

        [SerializeField] private Polygon polygon;
        [SerializeField] private PolygonCollider2D polygonCollider2D;
        
        private List<Vector2> geoCoordinates;
        private List<Vector2> worldCoordinates;
        private BuildingSO _buildingSo;

        private void OnValidate()
        {
            _buildingSo.geoCoordinate =  polygon.points.Select(MapUtilities.WorldToGeoPosition).ToList();
            
        }

        public void Init(BuildingSO buildingSo)
        {
            _buildingSo = buildingSo;
            gameObject.name = buildingSo.name;
            geoCoordinates = new List<Vector2>(buildingSo.geoCoordinate);
            worldCoordinates = geoCoordinates.Select(MapUtilities.GeoToWorldPosition).ToList();
            //polygon.Color = Color.black;
            polygon.Color = VisualManager.Instance.GetMapColor(buildingSo.mapColor);
            if (Is3D) Init3D();
            else Init2D();
        }

        private void Init2D()
        {
            foreach (var coordinate in worldCoordinates)
            {
                polygon.AddPoint(coordinate);
            }

            polygonCollider2D.pathCount = worldCoordinates.Count;
            polygonCollider2D.SetPath(0, worldCoordinates);
        }

        private void Init3D()
        {
            for (int i = 0; i < worldCoordinates.Count; i++)
            {
                var firstIndex = i;
                var secondIndex = (i + 1) % worldCoordinates.Count;

                var bottomFirstPoint = worldCoordinates[firstIndex];
                var bottomSecondPoint = worldCoordinates[secondIndex];
                var topSecondPoint = bottomSecondPoint + Vector2.up * BuildingHeight;
                var topFirstPoint = bottomFirstPoint + Vector2.up * BuildingHeight;

                var sidePolygon = Instantiate(polygon);
                sidePolygon.transform.Rotate(Vector3.right, -90);
                sidePolygon.AddPoints(new[] {bottomFirstPoint, bottomSecondPoint, topSecondPoint, topFirstPoint});
            }
        }

        private void OnMouseDown()
        {
            //Debug.Log(gameObject.name + " "+ EventSystem.current.IsPointerOverGameObject());
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
        }
    }
}