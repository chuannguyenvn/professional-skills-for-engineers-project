using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : Singleton<CameraMovement>
{
    public bool IsStaticTouch => !(isZooming || isDragging);

    [Header("Properties")] 
    private Camera _mainCamera;
    private Vector2 touchStartScreenPosition;
    private Vector2 touchStartWorldPosition;
    private float _initialZoomSize;
    private bool isTouchStartedOverUI;
    private bool isZooming;
    private bool isDragging;

    [Header("Scale GameObject")] 
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject destinationIcon;
    
    [Header("Zooming")] 
    [SerializeField] private float zoomOutMin = 1;
    [SerializeField] private float zoomOutMax = 8;
    [SerializeField] private float zoomSpeed = 1;

    [Header("Camera Constraint")] [SerializeField]
    private Vector2 _cameraCenter;
    [SerializeField] private float _startFadeDistance;
    [SerializeField] private float _maxDistance;
    [SerializeField] private Color _maxDistanceBackgroundColor;
    [SerializeField] private float _touchScreenMovementThreshold = 1f;

    private Color _initialBackgroundColor;

    private float _initialZ;

    private void Start()
    {
        _mainCamera = Camera.main;
        _initialBackgroundColor = _mainCamera.backgroundColor;
        _initialZ = transform.position.z;
        _initialZoomSize = Camera.main.orthographicSize;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(touchStartWorldPosition, _touchScreenMovementThreshold);
    }

    void Update()
    {
#if UNITY_ANDROID
        MoveInAndroid();
#else
        MoveInEditor();
#endif
    }

    private void MoveInAndroid()
    {
        switch (Input.touchCount)
        {
            case 1:
            {
                Touch touchZero = Input.GetTouch(0);

                if (touchZero.phase == TouchPhase.Began || isZooming)
                {
                    touchStartScreenPosition = touchZero.position;
                    touchStartWorldPosition = _mainCamera.ScreenToWorldPoint(touchZero.position);
                    isTouchStartedOverUI = IsPointerOverUIObject(touchZero.position);
                    if (isZooming)
                    {
                        isZooming = false;
                        isDragging = true; // It actually have been dragged in Zoom
                    }
                }
                else if (!isTouchStartedOverUI)
                {
                    Vector2 worldMousePos = _mainCamera.ScreenToWorldPoint(touchZero.position);
                    Vector2 cameraPos = _mainCamera.transform.position;
                    Vector2 offset = touchStartWorldPosition - worldMousePos;
                    Vector3 clampedPos = Vector2.ClampMagnitude(cameraPos + offset - _cameraCenter, _maxDistance) +
                                         _cameraCenter;
                    _mainCamera.transform.position = clampedPos.NewZ(_initialZ);

                    float distanceFromCenter = Vector2.Distance(cameraPos, _cameraCenter);
                    float lerpValue = (distanceFromCenter - _startFadeDistance) / (_maxDistance - _startFadeDistance);
                    _mainCamera.backgroundColor =
                        Color.Lerp(_initialBackgroundColor, _maxDistanceBackgroundColor, lerpValue);

                    if (Vector2.Distance(touchStartScreenPosition, touchZero.position) > _touchScreenMovementThreshold)
                        isDragging = true;
                }

                break;
            }
            case 2:
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                isZooming = true;
                Zoom(difference * 0.01f);
                break;
            }
            case 0:
                isDragging = false;
                isTouchStartedOverUI = false;
                isZooming = false;
                break;
        }
    }

    void Zoom(float increment)
    {
        _mainCamera.orthographicSize =
            Mathf.Clamp(_mainCamera.orthographicSize - increment * zoomSpeed, zoomOutMin, zoomOutMax);

        Vector3 scaleIcon = Vector3.one * _mainCamera.orthographicSize / _initialZoomSize;
        player.transform.localScale = scaleIcon;
        destinationIcon.transform.localScale = scaleIcon;
    }


    private void MoveInEditor()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStartScreenPosition = Input.mousePosition;
            touchStartWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            isTouchStartedOverUI = IsPointerOverUIObject(Input.mousePosition);
        }

        if (Input.GetMouseButton(0) && !isTouchStartedOverUI)
        {
            Vector2 worldMousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 cameraPos = _mainCamera.transform.position;
            Vector2 offset = touchStartWorldPosition - worldMousePos;
            Vector3 clampedPos = Vector2.ClampMagnitude(cameraPos + offset - _cameraCenter, _maxDistance) +
                                 _cameraCenter;
            _mainCamera.transform.position = clampedPos.NewZ(_initialZ);

            float distanceFromCenter = Vector2.Distance(cameraPos, _cameraCenter);
            float lerpValue = (distanceFromCenter - _startFadeDistance) / (_maxDistance - _startFadeDistance);
            _mainCamera.backgroundColor = Color.Lerp(_initialBackgroundColor, _maxDistanceBackgroundColor, lerpValue);

            if (Vector2.Distance(touchStartScreenPosition, Input.mousePosition) > _touchScreenMovementThreshold)
                isDragging = true;
        }

        if (!Input.GetMouseButton(0))
        {
            isDragging = false;
            isTouchStartedOverUI = false;
        }

        Zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    public static bool IsPointerOverUIObject(Vector2 position)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(position.x, position.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}