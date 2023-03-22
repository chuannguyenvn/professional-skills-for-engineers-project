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
        [SerializeField] private Polygon polygon;
        [SerializeField] private PolygonCollider2D polygonCollider2D;

        [Header("Road Node Entrances")] 
        public List<RoadIntersectionNode> entrances = new();

        [Header("3D Variable")]
        [SerializeField] bool Is3D = true;
        [SerializeField] private float BuildingHeight = 10f;

        
        private List<Vector2> _geoCoordinates;
        private List<Vector2> _worldCoordinates;
        public BuildingSO buildingSo;
        

        public void Init(BuildingSO buildingSo)
        {
            this.buildingSo = buildingSo;
            gameObject.name = buildingSo.name;
            _geoCoordinates = new List<Vector2>(buildingSo.geoCoordinate);
            _worldCoordinates = _geoCoordinates.Select(MapUtilities.GeoToWorldPosition).ToList();
            polygon.Color = VisualManager.Instance.GetMapColor(buildingSo.mapColor);

            transform.position = _worldCoordinates[0];
            for (int i = 0 ; i< _worldCoordinates.Count; i++)
            {
                _worldCoordinates[i] -=  (Vector2)transform.position;
            }

            if (Is3D) Init3D();
            else Init2D();
        }

        private void Init2D()
        {
            foreach (var coordinate in _worldCoordinates)
            {
                polygon.AddPoint(coordinate);
            }

            polygonCollider2D.pathCount = _worldCoordinates.Count;
            polygonCollider2D.SetPath(0, _worldCoordinates);
        }

        private void Init3D()
        {
            for (int i = 0; i < _worldCoordinates.Count; i++)
            {
                var firstIndex = i;
                var secondIndex = (i + 1) % _worldCoordinates.Count;

                var bottomFirstPoint = _worldCoordinates[firstIndex];
                var bottomSecondPoint = _worldCoordinates[secondIndex];
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