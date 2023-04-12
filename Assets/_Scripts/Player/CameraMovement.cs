using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : Singleton<CameraMovement>
{
    private static float TOUCH_FINGER_MOVEMENT_THRESHOLD = 2f;
    public bool IsDragging { get; private set; }

    private Camera _mainCamera;

    Vector2 touchStart;
    private bool isTouchStartedOverUI;

    public float zoomOutMin = 1;
    public float zoomOutMax = 8;
    public float zoomSpeed = 1;

    [SerializeField] private Vector2 _cameraCenter;
    [SerializeField] private float _startFadeDistance;
    [SerializeField] private float _maxDistance;
    [SerializeField] private Color _maxDistanceBackgroundColor;
    private Color _initialBackgroundColor;

    private float _initialZ;

    private void Start()
    {
        _mainCamera = Camera.main;
        _initialBackgroundColor = _mainCamera.backgroundColor;
        _initialZ = transform.position.z;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            isTouchStartedOverUI = IsPointerOverUIObject();
        }

        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            Zoom(difference * 0.01f);
        }
        else if (Input.GetMouseButton(0) && !isTouchStartedOverUI)
        {
            Vector2 worldMousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 cameraPos = _mainCamera.transform.position;
            Vector2 offset = touchStart - worldMousePos;
            Vector3 clampedPos = Vector2.ClampMagnitude(cameraPos + offset - _cameraCenter, _maxDistance) + _cameraCenter;
            _mainCamera.transform.position = clampedPos.NewZ(_initialZ);
            
            float distanceFromCenter = Vector2.Distance(cameraPos, _cameraCenter);
            float lerpValue = (distanceFromCenter - _startFadeDistance) / (_maxDistance - _startFadeDistance);
            _mainCamera.backgroundColor = Color.Lerp(_initialBackgroundColor, _maxDistanceBackgroundColor, lerpValue);
            
            if (Vector2.Distance(touchStart, worldMousePos) > TOUCH_FINGER_MOVEMENT_THRESHOLD) IsDragging = true;
        }

        if (!Input.GetMouseButton(0))
        {
            IsDragging = false;
            isTouchStartedOverUI = false;
        }

        Zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    void Zoom(float increment)
    {
        _mainCamera.orthographicSize =
            Mathf.Clamp(_mainCamera.orthographicSize - increment * zoomSpeed, zoomOutMin, zoomOutMax);
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}