using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ScrollPage : MonoBehaviour
{
    private RectTransform _rectTransform;

    [Header("Awake Animation")] 
    [SerializeField] private float yPositionMovingUp=1;
    [SerializeField] private float duration=0.5f;
    [SerializeField] private Ease easeType;


    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        _rectTransform.DOMoveY(yPositionMovingUp, duration).SetEase(easeType);

    }

    private void OnDisable()
    {
        _rectTransform.DOMoveY(-yPositionMovingUp, duration).SetEase(easeType);
    }
}
