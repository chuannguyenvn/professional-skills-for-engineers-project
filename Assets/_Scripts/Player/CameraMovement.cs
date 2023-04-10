using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : Singleton<CameraMovement>
{
    private static float TOUCH_FINGER_MOVEMENT_THRESHOLD = 2f;
    public bool IsDragging { get; private set; }

    Vector2 touchStart;
    private bool isTouchStartedOverUI;
    
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;
    public float zoomSpeed = 1;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
            Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = touchStart - worldMousePos;
            Camera.main.transform.position += (Vector3)direction;
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
        Camera.main.orthographicSize =
            Mathf.Clamp(Camera.main.orthographicSize - increment * zoomSpeed, zoomOutMin, zoomOutMax);
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