using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    /*[SerializeField] private Canvas mainCanvas;
    [SerializeField] private bool isOnScrollPage = false;

    [SerializeField] private LayerMask uiLayerMask;
    private void Update()
    {
        if( Input.GetMouseButtonDown(0))
        {
            MouseClicked();
            
        }
    }
    
    
    void MouseClicked(){
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 1f, uiLayerMask );
        Debug.Log(hit.transform.gameObject);
    }
    */
    
    #region Unused
    //
    // public Vector3 WorldToUISpace(Canvas parentCanvas, Vector3 worldPos)
    // {
    //     if (parentCanvas == null) parentCanvas = mainCanvas;
    //     //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
    //     Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
    //     Vector2 movePos;
    //
    //     //Convert the screenpoint to ui rectangle local point
    //     RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
    //     //Convert the local point to world point
    //     return parentCanvas.transform.TransformPoint(movePos);
    // }
    // public Vector3 WorldToUISpace(Vector3 worldPos)
    // {
    //     
    //     //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
    //     Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
    //     Vector2 movePos;
    //
    //     //Convert the screenpoint to ui rectangle local point
    //     RectTransformUtility.ScreenPointToLocalPointInRectangle(mainCanvas.transform as RectTransform, screenPos, mainCanvas.worldCamera, out movePos);
    //     //Convert the local point to world point
    //     return mainCanvas.transform.TransformPoint(movePos);
    // }
    

    #endregion


}
