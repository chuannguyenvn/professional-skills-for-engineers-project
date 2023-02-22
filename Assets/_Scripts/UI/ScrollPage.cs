using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ScrollPage : MonoBehaviour
{
    private RectTransform _contentRTransform;

    [Header("Awake Animation")] 
    [SerializeField] private float yPositionMovingUp = 0;
    [SerializeField] private float duration=0.5f;
    [SerializeField] private Ease easeType;


    private void Awake()
    {
        _contentRTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        _contentRTransform.DOMoveY(yPositionMovingUp, duration).SetEase(easeType);
    }
    
    public void DisableAnimation()
    {
        _contentRTransform.DOMoveY(-yPositionMovingUp, duration).SetEase(easeType).SetRelative().OnComplete(Disable);
        
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
