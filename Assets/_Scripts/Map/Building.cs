using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Manager;
using DG.Tweening;
using Map;
using Shapes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts.Map
{
    public class Building : MonoBehaviour
    {
        private static Building latestTouchedBuilding;

        [SerializeField] private Polygon polygon;
        private Polyline _polyline;
        [SerializeField] private PolygonCollider2D polygonCollider2D;

        [Header("Road Node Entrances")] public List<RoadIntersectionNode> entrances = new();

        [Header("3D Variable")] [SerializeField]
        bool Is3D = true;

        [SerializeField] private float BuildingHeight = 10f;


        private List<Vector2> _geoCoordinates;
        private List<Vector2> _worldCoordinates;
        public BuildingSO buildingSo;

        private Color _initialColor;

        private void Start()
        {
            _polyline = Instantiate(ResourceManager.Instance.Polyline, transform);
            _polyline.SetPoints(polygon.points);
            _initialColor = _polyline.Color = polygon.Color;
        }

        public void Init(BuildingSO buildingSo)
        {
            this.buildingSo = buildingSo;
            gameObject.name = buildingSo.buildingName;
            _geoCoordinates = new List<Vector2>(buildingSo.geoCoordinate);
            _worldCoordinates = _geoCoordinates.Select(MapUtilities.GeoToWorldPosition).ToList();
            polygon.Color = VisualManager.Instance.GetColor(buildingSo.mapColor);

            transform.position = _worldCoordinates[0];
            for (int i = 0; i < _worldCoordinates.Count; i++)
            {
                _worldCoordinates[i] -= (Vector2)transform.position;
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
            latestTouchedBuilding = this;
        }

        private void OnMouseUp()
        {
            if (EvaluateClick())
            {
                Debug.Log(gameObject.name + " Clicked ");

                AppState currentState = ApplicationManager.Instance.GetState();

                if (currentState is AppState.Home or AppState.Info)
                    ApplicationManager.Instance.SetState(AppState.Info, null, new object[] {this});
            }
        }

        public void Select()
        {
            var finalColor = _initialColor.MultiplyHSV(1, 1.3f, 1.3f);
            DOVirtual.Color(polygon.Color, finalColor, 0.2f, c => polygon.Color = c);
            DOVirtual.Color(_polyline.Color, finalColor, 0.2f, c => _polyline.Color = c);

            polygon.SortingOrder = 99;
            _polyline.SortingOrder = 100;
        }

        public void Deselect()
        {
            DOVirtual.Color(polygon.Color, _initialColor, 0.2f, c => polygon.Color = c);
            DOVirtual.Color(_polyline.Color, _initialColor, 0.2f, c => _polyline.Color = c);

            polygon.SortingOrder = 0;
            _polyline.SortingOrder = 0;
        }

        private bool EvaluateClick()
        {
            
            #if UNITY_EDITOR
            return latestTouchedBuilding == this && !CameraMovement.Instance.IsDragging && !EventSystem.current.IsPointerOverGameObject();
            #endif
            
            #if UNITY_ANDROID
            return latestTouchedBuilding == this && !CameraMovement.Instance.IsDragging && 
                   (Input.touchCount>0 && !EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId));

            #endif
            
        }
    }
}